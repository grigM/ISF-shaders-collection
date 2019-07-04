/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#14498.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

//jistyles edit - thanks iq!
// Created by inigo quilez - iq/2013
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// original here: https://www.shadertoy.com/view/MsXGRf - please preserve credits in downstream experiments.

// tweaked for sandbox uniforms + removed input texture handlers


// hash based 3d value noise
vec4 hash( const vec4 n )
{
    return fract(sin(n)*8.5453);
}

float noise( in vec3 x )
{
    vec3 f = fract(x);
    vec3 p = x - f;

    f = f * f * (3.0 - 2.0 * f);
    float n = p.x + p.y*57.0 + 113.0*p.z;
	
    vec4 v = mix( hash(vec4(  0.0, 57.0, 113.0, 170.0) + n),
		  hash(vec4(  1.0, 58.0, 114.0, 171.0) + n), f.xxxx);
	
    return mix(mix(v.x, v.y, f.y),
    	       mix(v.z, v.w, f.y), f.z);
}

vec4 map( vec3 p )
{
	vec3 r = p;
	
	float den = -0.6 - p.y;
    // invert space	
    p.y += 0.6;
	p = -4.0*p/dot(p,p);


    // twist space	
	//float an = -1.0*sin(0.1*TIME + 1.0*length(p.xz)  + 1.0*p.y);
	//float co = cos(an);
	//float si = sin(an);
	//p.xz = mat2(co,-si,si,co)*p.xz;

     // distort	
	p.xz += 1.0*(-1.0+2.0*noise( p*1.1 ));

    // pattern
	float f;
	vec3 q = p*0.85                     - vec3(0.0,1.0,0.0)*TIME*0.12;
    f  = 0.50000*noise( q ); q = q*2.02 - vec3(0.0,1.0,0.0)*TIME*0.12;
    f += 0.25000*noise( q ); q = q*2.03 - vec3(0.0,1.0,0.0)*TIME*0.12;


	den = clamp( (den + 4.0*f)*1.2, 0.0, 1.0 );
	
	vec3 col = 1.2*mix( vec3(0.2,0.8,1.6), 0.9*vec3(0.3,1.2,0.35), den ) ;
	col += 0.05*sin(0.05*q);
	col *= 1.0 - 0.8*smoothstep(0.6,1.0,sin(0.7*q.x)*sin(0.7*q.y)*sin(0.7*q.z))*vec3(0.1,0.5,0.8);
	col *= 1.0 + 1.0*smoothstep(0.5,1.0,1.0-length( (fract(q.xz*0.12)-0.5)/0.5 ))*vec3(1.0,0.9,0.8) ;
	col = mix( vec3(0.8,0.3,0.2), col, clamp( (r.y+0.1)/1.5, 0.0, 1.0 ) );

	return vec4( col, den );
}


vec3 raymarch( in vec3 ro, in vec3 rd )
{
	vec4 sum = vec4( 0.0 );
	vec3 bg = vec3(1.4,0.5,0.1)*1.3;

	float t = 0.0;

	for( int i=0; i<64; i++ )
	{
		if( sum.a > 0.99 ) continue;
		vec3 pos = ro + t*rd;
		vec4 col = map( pos );
		
		col.xyz = mix( bg, col.xyz, exp(-0.002*t*t*t) );
		
		col.a *= 0.5;
		col.rgb *= col.a;

		sum = sum + col*(1.0 - sum.a);	
		
		t += 0.15;
		
	}

	sum.xyz = mix( bg, sum.xyz/(0.001+sum.w), sum.w );
	return clamp( sum.xyz, 0.0, 1.0 );
}

void main(void)
{
    // inputs	
    vec2 q = gl_FragCoord.xy / RENDERSIZE.xy;
    vec2 p = -1.0 + 2.0*q;
    p.x *= RENDERSIZE.x/ RENDERSIZE.y;
	

	
    // camera
	float an = -0.07*TIME + 3.0*mouse.x;
    vec3 ro = 4.5*normalize(vec3(cos(an), 0.5, sin(an)));
	ro.y += 8.0*mouse.y-3.0;
	vec3 ta = vec3(0.0, 0.5, 0.0);
	float cr = -0.4*cos(0.02*TIME);
	
	// build ray
    vec3 ww = normalize( ta - ro);
    vec3 uu = normalize(cross( vec3(sin(cr),cos(cr),0.0), ww ));
    vec3 vv = normalize(cross(ww,uu));
    vec3 rd = normalize( p.x*uu + p.y*vv + 2.5*ww );
		
    // raymarch	
	vec3 col = raymarch( ro, rd );
	
	// contrast
	col = col*col*(3.0-2.0*col)*1.4 - 0.4;
	
    col.y *= 1.05;	
    // vignetting		
	col *= 0.25 + 0.75*pow( 16.0*q.x*q.y*(1.0-q.x)*(1.0-q.y), 0.1 );
	
    gl_FragColor = vec4( col, 1.0 );
}



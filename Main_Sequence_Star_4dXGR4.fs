/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex09.jpg"
    },
    {
      "NAME" : "AudioWaveformImage",
      "PATH" : "tex09.jpg"
    }
  ],
  "CATEGORIES" : [
    "3dnoise",
    "perlin",
    "star",
    "corona",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4dXGR4 by flight404.  Inspired by trisomie21's 3d noise fireball, I made some tweaks and ended up with a star.\n\nEDIT: Fixed the negative sqrt issue as per iq's suggestion.",
  "INPUTS" : [
       {
      "NAME": "freq2",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": -1.0,
		"MAX": 2
    },
    {
      "NAME": "freq3",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": -1.0,
		"MAX": 3
    },
    {
      "NAME": "fadeParam1",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": 0.5,
		"MAX": 4
    },
    {
      "NAME": "fadeParam2",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0.0,
		"MAX": 1
    }     
  ]
}
*/


// based on https://www.shadertoy.com/view/lsf3RH by
// trisomie21 (THANKS!)
// My apologies for the ugly code.

float snoise(vec3 uv, float res)	// by trisomie21
{
	const vec3 s = vec3(1e0, 1e2, 1e4);
	
	uv *= res;
	
	vec3 uv0 = floor(mod(uv, res))*s;
	vec3 uv1 = floor(mod(uv+vec3(1.), res))*s;
	
	vec3 f = fract(uv); f = f*f*(3.0-2.0*f);
	
	vec4 v = vec4(uv0.x+uv0.y+uv0.z, uv1.x+uv0.y+uv0.z,
		      	  uv0.x+uv1.y+uv0.z, uv1.x+uv1.y+uv0.z);
	
	vec4 r = fract(sin(v*1e-3)*1e5);
	float r0 = mix(mix(r.x, r.y, f.x), mix(r.z, r.w, f.x), f.y);
	
	r = fract(sin((v + uv1.z - uv0.z)*1e-3)*1e5);
	float r1 = mix(mix(r.x, r.y, f.x), mix(r.z, r.w, f.x), f.y);
	
	return mix(r0, r1, f.z)*2.-1.;
}

float freqs[4];

void main()
{
	freqs[0] = IMG_NORM_PIXEL(AudioWaveformImage,mod(vec2( 0.01, 0.25 ),1.0)).x;
	freqs[1] = freq2;//IMG_NORM_PIXEL(AudioWaveformImage,mod(vec2( 0.07, 0.25 ),1.0)).x;
	freqs[2] = freq3;//IMG_NORM_PIXEL(AudioWaveformImage,mod(vec2( 0.15, 0.25 ),1.0)).x;
	freqs[3] = IMG_NORM_PIXEL(AudioWaveformImage,mod(vec2( 0.30, 0.25 ),1.0)).x;

	float brightness	= freqs[1] * 0.25 + freqs[2] * 0.25;
	float radius		= 0.24 + brightness * 0.2;
	float invRadius 	= 1.0/radius;
	
	vec3 orange			= vec3( 0.8, 0.65, 0.3 );
	vec3 orangeRed		= vec3( 0.8, 0.35, 0.1 );
	float time		= TIME * 0.1;
	float aspect	= RENDERSIZE.x/RENDERSIZE.y;
	vec2 uv			= gl_FragCoord.xy / RENDERSIZE.xy;
	vec2 p 			= -0.5 + uv;
	p.x *= aspect;

	float fade		= pow( length( fadeParam1 * p ), fadeParam2 );
	float fVal1		= 1.0 - fade;
	float fVal2		= 1.0 - fade;
	
	float angle		= atan( p.x, p.y )/6.2832;
	float dist		= length(p);
	vec3 coord		= vec3( angle, dist, time * 0.1 );
	
	float newTime1	= abs( snoise( coord + vec3( 0.0, -time * ( 0.35 + brightness * 0.001 ), time * 0.015 ), 15.0 ) );
	float newTime2	= abs( snoise( coord + vec3( 0.0, -time * ( 0.15 + brightness * 0.001 ), time * 0.015 ), 45.0 ) );	
	for( int i=1; i<=7; i++ ){
		float power = pow( 2.0, float(i + 1) );
		fVal1 += ( 0.5 / power ) * snoise( coord + vec3( 0.0, -time, time * 0.2 ), ( power * ( 10.0 ) * ( newTime1 + 1.0 ) ) );
		fVal2 += ( 0.5 / power ) * snoise( coord + vec3( 0.0, -time, time * 0.2 ), ( power * ( 25.0 ) * ( newTime2 + 1.0 ) ) );
	}
	
	float corona		= pow( fVal1 * max( 1.1 - fade, 0.0 ), 2.0 ) * 50.0;
	corona				+= pow( fVal2 * max( 1.1 - fade, 0.0 ), 2.0 ) * 50.0;
	corona				*= 1.2 - newTime1;
	vec3 sphereNormal 	= vec3( 0.0, 0.0, 1.0 );
	vec3 dir 			= vec3( 0.0 );
	vec3 center			= vec3( 0.5, 0.5, 1.0 );
	vec3 starSphere		= vec3( 0.0 );
	
	vec2 sp = -1.0 + 2.0 * uv;
	sp.x *= aspect;
	sp *= ( 2.0 - brightness );
  	float r = dot(sp,sp);
	float f = (1.0-sqrt(abs(1.0-r)))/(r) + brightness * 0.5;
	if( dist < radius ){
		corona			*= pow( dist * invRadius, 24.0 );
  		vec2 newUv;
 		newUv.x = sp.x*f;
  		newUv.y = sp.y*f;
		newUv += vec2( time, 0.0 );
		
		vec3 texSample 	= IMG_NORM_PIXEL(iChannel0,mod(newUv,1.0)).rgb;
		float uOff		= ( texSample.g * brightness * 4.5 + time );
		vec2 starUV		= newUv + vec2( uOff, 0.0 );
		starSphere		= IMG_NORM_PIXEL(iChannel0,mod(starUV,1.0)).rgb;
	}
	
	float starGlow	= min( max( 1.0 - dist * ( 1.0 - brightness ), 0.0 ), 1.0 );
	//gl_FragColor.rgb	= vec3( r );
	gl_FragColor.rgb	= vec3( f * ( 0.75 + brightness * 0.3 ) * orange ) + starSphere + corona * orange + starGlow * orangeRed;
	gl_FragColor.a		= 1.0;
}

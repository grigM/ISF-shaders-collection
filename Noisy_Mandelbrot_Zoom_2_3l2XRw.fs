/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3l2XRw by jarble.  This shader is based on https:\/\/www.shadertoy.com\/view\/lsX3W4",
  "INPUTS" : [

  ]
}
*/


// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

// this function is from https://www.shadertoy.com/view/4djSRW
float hash12(vec2 p)
{
	vec3 p3  = fract(vec3(p.xyx) * .1031);
    p3 += dot(p3, p3.yzx + 33.33);
    return fract((p3.x + p3.y) * p3.z);
}

void main() {



    vec2 p = -1.0 + 2.0 * gl_FragCoord.xy / RENDERSIZE.xy;
    p.x *= RENDERSIZE.x/RENDERSIZE.y;
    // animation	
	float tz = 0.5 - 0.5*cos(0.225*TIME);
    float zoo = pow( 0.5, 13.0*tz );
	vec2 c = vec2(-0.05,.6805) + p*zoo;
    // iterate
    float di =  1.0;
    vec2 z  = vec2(0.0);
    float m2 = 0.0;
    vec2 dz = vec2(0.0);
    for( int i=0; i<300; i++ )
    {
        if( m2>1024.0 ) { di=0.0; break; }
		// Z' -> 2·Z·Z' + 1
        dz = 2.0*vec2(z.x*dz.x-z.y*dz.y, z.x*dz.y + z.y*dz.x) + vec2(1.0,0.0);
			
        // Z -> Z² + c			
        z = vec2( z.x*z.x - z.y*z.y, 2.0*z.x*z.y ) + c+hash12(z*tz)*zoo;
			
        m2 = dot(z,z);
    }
    // distance	
	// d(c) = |Z|·log|Z|/|Z'|
	float d = 0.5*sqrt(dot(z,z)/dot(dz,dz))*log(dot(z,z));
    if( di>0.5 ) d=0.0;
	
    // do some soft coloring based on distance
	d = clamp( pow(4.0*d/zoo,0.2), 0.0, 1.0 );
    vec3 col = vec3( d );
    
    gl_FragColor = vec4( col, 1.0 );
}

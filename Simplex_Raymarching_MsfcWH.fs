/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "raymarching",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MsfcWH by Tidensbarn.  Simplex noise raymarching\nMouse enabled",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


vec3 hash( vec3 p ) 
{
	p = vec3( dot(p,vec3(127.1,311.7, 74.7)),
			  dot(p,vec3(269.5,183.3,246.1)),
			  dot(p,vec3(113.5,271.9,124.6)));

	return -1.0 + 2.0*fract(sin(p)*43758.5453123);
}

float noise( in vec3 p )
{
    vec3 i = floor( p );
    vec3 f = fract( p );
	
	vec3 u = f*f*(3.0-2.0*f);

    return mix( mix( mix( dot( hash( i + vec3(0.0,0.0,0.0) ), f - vec3(0.0,0.0,0.0) ), 
                          dot( hash( i + vec3(1.0,0.0,0.0) ), f - vec3(1.0,0.0,0.0) ), u.x),
                     mix( dot( hash( i + vec3(0.0,1.0,0.0) ), f - vec3(0.0,1.0,0.0) ), 
                          dot( hash( i + vec3(1.0,1.0,0.0) ), f - vec3(1.0,1.0,0.0) ), u.x), u.y),
                mix( mix( dot( hash( i + vec3(0.0,0.0,1.0) ), f - vec3(0.0,0.0,1.0) ), 
                          dot( hash( i + vec3(1.0,0.0,1.0) ), f - vec3(1.0,0.0,1.0) ), u.x),
                     mix( dot( hash( i + vec3(0.0,1.0,1.0) ), f - vec3(0.0,1.0,1.0) ), 
                          dot( hash( i + vec3(1.0,1.0,1.0) ), f - vec3(1.0,1.0,1.0) ), u.x), u.y), u.z );
}


void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy *2. - 1.;
	
    float len = length((iMouse.xy / RENDERSIZE.xy) * 2. - 1. - uv);
    float len2 = len * len;
    
    vec3 v = vec3(uv * 4.3 * len2, TIME * 3.61 );
    
    float n=0.;
    
    float z = 1. + 1. / (5.+len *  115.);
    
    float max = 6.;
    
    float m = iMouse.x * 1. + RENDERSIZE.x /2.;
    
    
    for(float i = 0.01;i<1.;i+=.009) {
        
        v.z += 7112.*(log(1.+i*0.00003));
        v.xy *= z;
        float f = noise(v);
        if(f > .009 * (28.-len2)) {
            n = 1.0- i;
            break;
        }
    }
    
    
    gl_FragColor = vec4(n*n*0.6*len,0.,n*n*1.0,1.);
}

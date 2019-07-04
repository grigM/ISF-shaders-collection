/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "DEPTH_DIV",
			"TYPE": "float",
			"DEFAULT": 120.0,
			"MIN": 0.0,
			"MAX": 1000.0
		},
		{
			"NAME": "SCALE_DIV",
			"TYPE": "float",
			"DEFAULT": 20.0,
			"MIN": 0.0,
			"MAX": 400.0
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#31078.0"
}
*/


//https://www.shadertoy.com/view/XsyGDV

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


const int LOOPS = 400;
//float DEPTH_DIV = 120.;
//float SCALE_DIV = 20.;

#define PI 3.1415926535897932384626433832795028

//Functions
vec3 rX(vec3 p, float a) { //YZ
	float c,s;vec3 q=p;
	c = cos(a); s = sin(a);
	p.y = c * q.y - s * q.z;
	p.z = s * q.y + c * q.z;
    return p;
}

vec3 rY(vec3 p, float a) { //XZ
	float c,s;vec3 q=p;
	c = cos(a); s = sin(a);
	p.x = c * q.x + s * q.z;
	p.z = -s * q.x + c * q.z;
    return p;
}

vec3 rZ(vec3 p, float a) { //XY
	float c,s;vec3 q=p;
	c = cos(a); s = sin(a);
	p.x = c * q.x - s * q.y;
	p.y = s * q.x + c * q.y;
    return p;
}

float rand(float co) { return fract(sin(co*(91.3458)) * 47453.5453); }
float rand(vec2 co){ return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453); }
float rand(vec3 co){ return rand(co.xy+rand(co.z)); }

vec3 hsl2rgb( in vec3 c )
{
    vec3 rgb = clamp( abs(mod(c.x*6.0+vec3(0.0,4.0,2.0),6.0)-3.0)-1.0, 0.0, 1.0 );

    return c.z + c.y * (rgb-0.5)*(1.0-abs(2.0*c.z-1.0));
}

//Map, 1 is wall, 0 is empty.
float map(vec3 p) { 
    p = floor((p+vec3(0.,0.,TIME))*10.)/10.;
	return float(rand(p)>0.96);
}
#define clamps(x) clamp(x,0.,1.)
float test(vec2 uv) {
    vec2 a = vec2(
        cos(TIME*.58)*.4
        ,
        sin(TIME*.52)*.4
        );
    
    vec2 b = vec2(
        cos(TIME*.55)*.4
        ,
        sin(TIME*.44)*.4
        );
    
    return clamps((length(uv-a)*length(uv-b))-.05);
}


void main( void ) {
	vec2 uv = (gl_FragCoord.xy / RENDERSIZE.xy)-.5;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    
    //Raycaster / Layering
    vec3 p; //Position
    float a = 0.; //"Alpha"
    float b = floor(test(uv)*30.)+1.;
    if (mod(floor(gl_FragCoord.x),b)==b-1.&&mod(floor(gl_FragCoord.y),b)==b-1.) {
        for (int i = 0; i < LOOPS; i++) {
            vec3 pos = vec3(uv*((float(i)/SCALE_DIV)+1.),float(i)/DEPTH_DIV);
            if (map(pos) >= 1.) {
                a = 1.;
                break; //Pixel doesn't need to be filled anymore. Stop the loop.
            } else {
                p = pos;
            }
        }
    }
    float fog = (p.z*DEPTH_DIV)/float(LOOPS);
    
	gl_FragColor = vec4(vec3(1.-fog)*a*hsl2rgb(vec3((TIME+(p.z*.5)),sin((p.x*3.)+TIME),1.5+sin((p.y*3.)+TIME))),1.0);
}
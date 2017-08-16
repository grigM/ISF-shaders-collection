/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1,
		"MIN": 0,
		"MAX": 5
	},
	{
		"NAME": "TL",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": 0,
		"MAX": 10
	},
	{
		"NAME": "sin1",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0,
		"MAX": 1
	},
	{
		"NAME": "normRotSpeed",
		"TYPE": "float",
		"DEFAULT": 0.8,
		"MIN": 0,
		"MAX": 10
	},
	{
		"NAME": "traceStart",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": -3,
		"MAX": 3
	},
	{
		"NAME": "traceIt",
		"TYPE": "float",
		"DEFAULT": 18,
		"MIN": 4,
		"MAX": 36
	},
	{
		"NAME": "traceD",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0,
		"MAX": 2.0
	},

	{
		"NAME": "rayNorm",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0,
		"MAX": 1
	},
	{
		"NAME": "mapWidth",
		"TYPE": "float",
		"DEFAULT": 0.3,
		"MIN": 0.1,
		"MAX": 1.0
	},
	
	{
		"NAME": "chrAbX",
		"TYPE": "float",
		"DEFAULT": 0.97,
		"MIN": 0,
		"MAX": 1
	},
	{
		"NAME": "chrAbY",
		"TYPE": "float",
		"DEFAULT": 0.97,
		"MIN": 0,
		"MAX": 1
	},
	{
		"NAME": "bright",
		"TYPE": "float",
		"DEFAULT": 0.07,
		"MIN": 0.01,
		"MAX": 1
	},
	{
		"NAME": "fractP1",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0.1,
		"MAX": 1
	},
	{
		"NAME": "fractP2",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": 1.0,
		"MAX": 3.0
	},
	{
		"NAME": "fractP3",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.5,
		"MAX": 1.5
	},
	{
		"NAME": "fractP4",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0.0,
		"MAX": 3.0
	}


		

  ],
    "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#41555.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable




float map(vec3 p)
{    
    vec3 q = (fract(p * fractP1) * fractP2) - fractP3;
	q.y = q.x * fractP4;
    
	return length(q) - mapWidth;
}

float trace(vec3 origin, vec3 ray) 
{    
    float t = traceStart;
    for (int i = 0; i < int(traceIt); i++) {
        vec3 p = origin + ray * t;
        float d = map(p);
        t += d * traceD;
    }
    return t;
}


void main( void ) {

	
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;    
    uv = uv * 2.0 - 1.0;
    
    // Aspect ratio.
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;                        
    
    // RGB
    vec3 c;
    
    float s1 = sin(((TIME*speed)+TL) * sin1);
    
    // Compute RGB separately.
    for (int i = 0; i < 3; i++) {
        
        // Move origin.
        vec3 origin = vec3(0.0, 0.0, ((TIME*speed)+TL));
        
        // Some kind of chromatic aberration.
        uv.x *= chrAbX;
        uv.y *= chrAbY;
        
    	vec3 ray = normalize(vec3(uv, rayNorm));
        
        // Spiral rotation (XY).
    	float the = ((TIME*speed)+TL) + length(uv) * s1;
    	ray.xy *= mat2(cos(the), -sin(the), sin(the), cos(the));
        
        // Normal rotation (XZ).
        the = ((TIME*speed)+TL) * normRotSpeed;
        ray.xz *= mat2(cos(the), -sin(the), sin(the), cos(the));
                
        float t = trace(origin, ray);
        
        // Visualize depth.
    	c[i] = 1.0 / (1.0 + t * t * bright);
    }    
       
        
	gl_FragColor = vec4(c, 1.0);
}
/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "tunnel",
    "triangles",
    "oscillation",
    "stairs",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XlGSRW by cacheflowe.  A wobbly triangle tunnel",
  "INPUTS" : [
	{
      "NAME": "steps",
      "TYPE": "float",
      "MIN": 0.0,
      "MAX": 50.0,
      "DEFAULT": 36.0
    },
    
    {
		"NAME": "rotate",
		"TYPE": "float",
		"DEFAULT": 0,
		"MIN": -1,
		"MAX": 1
	},
    {
      "NAME": "brighten",
      "TYPE": "float",
      "MIN": 0.0,
      "MAX": 4.0,
      "DEFAULT": 1.4
    },
    
   	{
      "NAME": "speed",
      "TYPE": "float",
      "MIN": 0.0,
      "MAX": 4.0,
      "DEFAULT": 2.0
    },
    
    {
      "NAME": "startSize",
      "TYPE": "float",
      "MIN": 5.0,
      "MAX": 10.0,
      "DEFAULT": 5.0
    },
    
    {
      "NAME": "endSize",
      "TYPE": "float",
      "MIN": 0.0,
      "MAX": 5.0,
      "DEFAULT": 0.0
    }
    ,
    
    {
      "NAME": "yOffset",
      "TYPE": "float",
      "MIN": -0.8,
      "MAX": 0.8,
      "DEFAULT": -0.22
    },
    
    {
      "NAME": "yfreqStart",
      "TYPE": "float",
      "MIN": -0.8,
      "MAX": 0.8,
      "DEFAULT": 0.2
    },
    
    {
      "NAME": "ySinFreq",
      "TYPE": "float",
      "MIN": -0.5,
      "MAX": 0.5,
      "DEFAULT": 0.25
    }
    ,
    
    {
      "NAME": "ySinSpeed",
      "TYPE": "float",
      "MIN": -5.0,
      "MAX": 5.0,
      "DEFAULT": 3.0
    }
  ]
}
*/

#define PI 3.14159265359

// triangle shape from: https://thebookofshaders.com/edit.php?log=160414041142

const float TWO_PI = 6.28318530718;
//const int steps = 36;
//const float brighten = 1.4;

float map(float value, float low1, float high1, float low2, float high2) {
   return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
}

float triangle(vec2 p, float size) {
    vec2 q = abs(p);
    return max(q.x * 0.866025 + p.y * 0.5, -p.y * 0.5) - size * 0.5;
}

float hexagon(vec2 p, float radius) {
    vec2 q = abs(p);
    return max(abs(q.y), q.x * 0.866025 + q.y * 0.5) - radius;
}

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

void main() {



    float time = TIME * speed;
    vec2 st = (2. * gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.y;
    st -= vec2(0, 0.3); // offset overall y a bit for more "floor"
    
    st = rotate2d(rotate*PI ) * st;
    
    // start white and head towards black as triangles shrink
    float col = 1.; 
    float sizeStart = startSize + cos(time); 
    float sizeEnd = endSize; 
    float stepSize = sizeStart / float(steps); 
    for(int i = 0; i < int(steps); i++) {
        float curStepSize = float(i) * stepSize;
        float stepColor = map(curStepSize, sizeStart, sizeEnd, 1., 0.05);
        float yCompensate = float(i) * yOffset; // triangle isn't centered, so we can offset for better concentricity
        vec2 stMoved = st + yfreqStart * vec2(0, yCompensate + sin(float(i) * ySinFreq + time * ySinSpeed)); // offset wobble y down the tunnel, 3x faster than main oscillation
        if(triangle(stMoved, curStepSize) > 0.) {
        	col = stepColor;
        }
    }
	gl_FragColor = vec4(vec3(col * brighten), 1.0);
}

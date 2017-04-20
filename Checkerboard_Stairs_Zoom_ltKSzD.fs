/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "zoom",
    "blackandwhite",
    "checkerboard",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ltKSzD by cacheflowe.  A simple checkboard test. Changing black & white to gradients really transformed the look on this one. I'm learning the basics, slowly.",
  "INPUTS" : [
	{
            "NAME": "speed",
            "TYPE": "float",
            "DEFAULT": 2.0,
            "MIN": 0.0,
            "MAX": 8.0
    },
    {
            "NAME": "BumpDistorDistance",
            "TYPE": "float",
            "DEFAULT": 5.0,
            "MIN": 0.0,
            "MAX": 10.0
    },
    {
            "NAME": "cellSize",
            "TYPE": "float",
            "DEFAULT": 2.0,
            "MIN": 1.0,
            "MAX": 10.0
    },
    {
            "NAME": "zoomOscilateParam",
            "TYPE": "float",
            "DEFAULT": 4.0,
            "MIN": 0.0,
            "MAX": 10.0
    },
    {
            "NAME": "zoomOutParam",
            "TYPE": "float",
            "DEFAULT": 7.0,
            "MIN": 0.0,
            "MAX": 14.0
    },
    {
            "NAME": "cellXParam",
            "TYPE": "float",
            "DEFAULT": 2.0,
            "MIN": 0.0,
            "MAX": 10.0
    },
    {
            "NAME": "cellYParam",
            "TYPE": "float",
            "DEFAULT": 2.0,
            "MIN": 0.0,
            "MAX": 10.0
    },
    {
            "NAME": "smoothstepIN",
            "TYPE": "float",
            "DEFAULT": 0.3,
            "MIN": 0.0,
            "MAX": 1.0
    },
    {
            "NAME": "smoothstepOUT",
            "TYPE": "float",
            "DEFAULT": 0.7,
            "MIN": 0.0,
            "MAX": 1.0
    }
  ]
}
*/


#define PI     3.14159265358
#define TWO_PI 6.28318530718

void main()
{
	vec2 uv = (2. * gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.y;	 // center coordinates
    float time = TIME * speed;					  				 // adjust time
    float dist = length(uv) * BumpDistorDistance;									 // adjust distance from center
    float cellSizeAdjust = dist/cellSize + dist * sin(PI + time);		     // adjust cell size from center
    float zoom = zoomOscilateParam * sin(time);									 // oscillate zoom
    uv *= zoomOutParam + cellSizeAdjust + zoom;                                // zoom out
	vec3 col = vec3(1. - fract(uv.y)); 							     // default fade to black
    if(floor(mod(uv.x, cellXParam)) == floor(mod(uv.y, cellYParam))) {				 // make checkerboard when cell indices are both even or both odd
        col = vec3(fract(uv.x)); 									 // use fract() to make the gradient along x & y
	}
    col = smoothstep(smoothstepIN,smoothstepOUT, col);									 // smooth out the color
	gl_FragColor = vec4(col,1.0);
}
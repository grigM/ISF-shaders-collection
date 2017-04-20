/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "SPEED",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 5.0
	},
	{
		"NAME": "gridsize",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0.0,
		"MAX": 2.0
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39830.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable






#define draw(dc) c = mix(dc,c,clamp(d/pixelSize, 0., 1.))


//2d box signed distance
float sdBox(vec2 p, vec2 b)
{
  vec2 d = abs(p) - b;
  return min(max(d.x,d.y),0.0) + length(max(d,0.0));
}



void main( )
{
    float pixelSize = length(1.0/RENDERSIZE.xy);
	vec2 uv = (gl_FragCoord.xy*2.0 - RENDERSIZE.xy)/RENDERSIZE.x;
    
    //infinite zoom, repeating offset and scaling
    float zoomRepeat = fract(TIME*SPEED),
        scale = zoomRepeat;
    uv += zoomRepeat*0.5;
    uv *= 1.0-scale*0.5;
    
    
    vec3 c = vec3(1.);//start with white background
    
    //layers of grids, first smallest is fading in
    for (int i = 1; i < 3; i++) {
        float sz = gridsize/pow(2.,float(3-i)),
              sz2 = sz/2.0;
      	float d = abs(sdBox(mod(abs(uv),sz)-sz2, vec2(sz2)))-0.004*(1.0-scale*0.75);
    	
        vec3 rc = vec3(0.);
        if (i == 1) rc = vec3(1.-zoomRepeat);
        draw(rc);
    }
    
    //output color
    gl_FragColor = vec4(c, 1.);
}
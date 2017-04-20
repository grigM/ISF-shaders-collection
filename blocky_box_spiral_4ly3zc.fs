/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "spin",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4ly3zc by abje.  a blocky spiral, inspired by fb39ca4's shader: https:\/\/www.shadertoy.com\/view\/XsXXDH",
  "INPUTS" : [
	{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": -1.0,
			"MAX": 1.0
	}
  ]
}
*/


#define rot2(spin) mat2(sin(spin),cos(spin),-cos(spin),sin(spin))

void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.y*2.0-RENDERSIZE.xy/RENDERSIZE.y;
    
    float l = 0.0;
    mat2 rot = rot2(TIME * speed);
    
    for(int i = 0; i < 256; i++) {
        
        uv *= 1.333 * rot;
        
        if(uv.y > 1.0) {
            break;
        }
        l++;
    }
    
	gl_FragColor = vec4(mod(l, 2.0));
}
/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
	{
			"NAME": "speedOpen",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 16.0
	},
	{
			"NAME": "invers",
			"TYPE": "bool",
			"DEFAULT": false
	}

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34992.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec4 color = vec4(1, 1, 1, 1);

void render (float a){
	float alpha = 0.0;
	if(clamp(1. - a, 0., 1.)!=0.0){
		alpha = 1.0;
	}
	gl_FragColor = vec4(color.xyz * clamp(1. - a, 0., 1.), alpha);
}

#define PI acos(-1.)

void main( void ) {
	if(invers){
		gl_FragColor = vec4(1.0);
	}
	vec2 pos = vec2(gl_FragCoord.x, RENDERSIZE.y - gl_FragCoord.y);
	
	vec2 center = RENDERSIZE / 2.;
	
	vec2 dir = center - pos;
	float angle = atan(dir.x, -dir.y) + PI;
	
	if (angle > mod(TIME * speedOpen, PI * 4.) - PI * 2. && angle < mod(TIME * speedOpen, PI * 4.))
	render(length(pos - center) - min(RENDERSIZE.x, RENDERSIZE.y) / 2. + 20.);
}
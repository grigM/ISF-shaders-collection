/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
		{
			"NAME": "mod_amp",
			"TYPE": "float",
			"DEFAULT": 0.0000001,
			"MIN": 0.00000001,
			"MAX": 0.0000001
	},
	{
			"NAME": "shade_pos_ofset",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": -0.7,
			"MAX": 2.0
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#3646.2"
}
*/


// by rotwang

#ifdef GL_ES
precision mediump float;
#endif


float rand(vec2 co)
{
	
	// implementation found at: lumina.sourceforge.net/Tutorials/Noise.html
	return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main( void ) {

	float aspect = RENDERSIZE.x / RENDERSIZE.y;
	vec2 unipos = ( gl_FragCoord.xy / RENDERSIZE );
 	vec2 pos = unipos*2.0-1.0;//bipolar
	pos.x *= aspect;

	float shade = abs(pos.y-shade_pos_ofset) * rand( mod(pos,mod_amp));
	float mask = step(pos.y-shade_pos_ofset,0.0);
	shade *= mask;
    gl_FragColor = vec4(vec3(shade),1.0);

}
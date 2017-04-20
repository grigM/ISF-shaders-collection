/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS": [
	{
			"NAME": "SCALE",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": 0.0,
			"MAX": 0.8
		},
		{
			"NAME": "SPEED",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		}
	]
,
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34898.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main( void ) {
	
	vec2 pos = ( gl_FragCoord.xy / RENDERSIZE.xy ) - vec2(0.5,0.5);	
        float horizon = 0.0; 
        float fov = 0.5; 
	float scaling = SCALE;
	

	vec3 p = vec3(pos.x, fov, pos.y - horizon);      
	vec2 s = vec2(p.x/p.z, p.y/p.z) * scaling;
	
	//checkboard texture
	float color = sign((mod(s.x, 0.1) - 0.05) * (mod(s.y+(TIME*SPEED), 0.1) - 0.05));	
	//fading
	color *= sin(p.z)*3.0;
	
	gl_FragColor = vec4( vec3(color), 2.0 );

}
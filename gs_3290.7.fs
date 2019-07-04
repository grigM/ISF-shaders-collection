/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "BarrelPower",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": -2.0,
			"MAX": 4.0
		},
		{
			"NAME": "grid",
			"TYPE": "float",
			"DEFAULT": 8.0,
			"MIN": 2.0,
			"MAX": 16.0
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#3290.7"
}
*/


// WIP
// by rotwang, playing with barrel distortion

#ifdef GL_ES
precision mediump float;
#endif




// copied from Little Grasshopper
// Given a vec2 in [-1,+1], generate a texture coord in [0,+1]
vec2 barrel_distortion(vec2 p)
{
    float theta  = atan(p.y, p.x);
    float radius = length(p);
    radius = pow(radius, BarrelPower);
    p.x = radius * cos(theta);
    p.y = radius * sin(theta);
    return 0.5 * (p + 1.0);
}




void main( void ) {

	float aspect = RENDERSIZE.x / RENDERSIZE.y;
	vec2 unipos = ( gl_FragCoord.xy / RENDERSIZE ); 
	vec2 pos = unipos *2.0-1.0;
	pos *= aspect;
	pos = barrel_distortion(pos);
	
	float n = 1.0/ float(int(grid));
	vec2 pm =  mod( pos, n);

	vec3 clr_a = vec3(pos-pm,0.5);
	vec3 clr_b = vec3(0.5, pos-pm);
	
	vec3 clr = mix(clr_a, clr_b, 0.5);
	
	gl_FragColor = vec4( clr, 1.0 );

}
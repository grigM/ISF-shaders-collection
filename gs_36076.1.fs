/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36076.1"
}
*/


//MrOMGWTF
//I have no idea what is it
//Looks cool tho

//precision highp float;

float motionblur_size = 3.66;

vec3 thing(vec2 uv, vec2 pos, vec3 color, float rad)
{
	return color * (1.0 / distance(uv, pos) * rad);	
}

void main( void ) {

	float time0 = TIME / 60.;
	float time1 = TIME / 50.;
	float time2 = TIME / 40.;
	float time3 = TIME / 100.;
	float time4 = TIME / 53.;
	float time5 = TIME / 74.;
	float time6 = TIME / 16.;
	float time7 = TIME / 27.;
	float time8 = TIME / 56.;
	float time9 = TIME / 72.;
	
   	vec2 p=(gl_FragCoord.xy/RENDERSIZE.x)*2.0-vec2(1.0,RENDERSIZE.y/RENDERSIZE.x);
	p=p*3.0;
	vec3 color = vec3(0.0);
	color += thing(p, vec2(0.0), vec3(9.0, 1.0, 0.6), 0.05);
	p=p*1.8;
	color += thing(p, vec2(sin(time1 * 99.0), cos(time1 * 99.0)) * 1.25, vec3(-8.5, 0.5, 1.0), 0.5*(.5 + .5*cos(10.0*TIME)));
	p=p/1.8;
	color += thing(p, vec2(sin(time2 * 99.0), cos(time2 * 199.0)) * 1.25, vec3(9.5, 0.5, 1.0), 0.01);


	

	gl_FragColor = vec4( color, 1.0 );

}
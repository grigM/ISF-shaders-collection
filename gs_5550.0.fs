/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#5550.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159


float round(float v)
{
	if(v - floor(v) >= 0.5) return floor(v)+1.0;
	else return floor(v);

}

vec2 round(vec2 v)
{
	vec2 ret = vec2(0.0);
	if(v.x - floor(v.x) >= 0.5) ret.x = floor(v.x)+1.0;
	else ret.x = floor(v.x);
	if(v.y - floor(v.y) >= 0.5) ret.y = floor(v.y)+1.0;
	else ret.y = floor(v.y);
	return ret;
}

float triwave(float x)
{
	return 1.0-4.0*abs(0.5-fract(0.5*x + 0.25));
}

//from #3611.0

float rand(vec2 co){
	float t = mod(TIME,16.0);
    return fract(sin(dot(co.xy ,vec2(1.9898,7.233))) * t*t);
}

//from http://github.prideout.net/barrel-distortion/

float BarrelPower = 1.085;
vec2 Distort(vec2 p)
{
    float theta  = atan(p.y, p.x);
    float radius = length(p);
    radius = pow(radius, BarrelPower);
    p.x = radius * cos(theta);
    p.y = radius * sin(theta);
    return 0.5 * (p + 1.0);
}

vec3 shader( vec2 p, vec2 RENDERSIZE ) {

	vec2 pos = ( p / RENDERSIZE.xy );

	vec3 color = vec3(0.0);
	
	float wave = 1.-abs(pos.y - (sin((cos(pos.x + cos(TIME) +(3.*sin(TIME /2.))))*PI*4.+cos(TIME))*0.25+1.)); // 'just string together a bunch of random stuff' remix
	
	color = vec3(0.,pow(wave,16.),0.);
	color += vec3(pow(wave,32.));
	
	return vec3(color);

}

float xxx = 4.0;

float pixelsize = 2.0 * xxx;

void main( void ) {

	vec2 position = ( gl_FragCoord.xy);
	
	vec3 color = vec3(0.0);
	
	vec2 dposition = Distort(position/RENDERSIZE-.5)*(RENDERSIZE*2.0);
	
	vec2 rposition = round(((dposition-(pixelsize/2.0))/pixelsize));
	
	
	color = vec3(shader(rposition,RENDERSIZE/pixelsize));
	
	color = clamp(color,0.0625,1.0);
	
	//color *= (rand(rposition)*0.25+0.75);
	
	color *= abs(sin(rposition.y*8.0+(TIME*16.0))*0.15+0.85);
	
	color *= vec3(clamp( abs(triwave(dposition.x/pixelsize))*3.0 , 0.0 , 1.0 ));
	color *= vec3(clamp( abs(triwave(dposition.y/pixelsize))*3.0 , 0.0 , 1.0 ));
	
	float darkness = sin((position.x/RENDERSIZE.x)*PI)*sin((position.y/RENDERSIZE.y)*PI);
	
	color *= vec3(clamp( darkness*4.0 ,0.0 ,1.0 ));
	
	gl_FragColor = vec4( color, 1.0 );

}
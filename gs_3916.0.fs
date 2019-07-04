/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
      "NAME": "desaturation",
      "TYPE": "float",
      "MIN": 0.0,
      "MAX": 1,
      "DEFAULT": 0.0
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#3916.0"
}
*/


// by rotwang

#ifdef GL_ES
precision highp float;
#endif

const float PI = 3.1415926535;
const float TWOPI = PI*2.0;



	
	
vec3 hsv2rgb(float h,float s,float v) {
	return mix(vec3(1.),clamp((abs(fract(h+vec3(3.,2.,1.)/3.)*6.-3.)-1.),0.,1.),s)*v;
}

float ring( vec2 p )
{
	float len = length(p)-0.55;
	len *= length(p*p)-0.66;
  	float d = len*len*512.0;

    return 1.0-d*d;
}


vec3 ring_clr( vec2 p )
{
	float d = ring( p );
	float angle = (atan(p.x, p.y)+PI)/TWOPI;
	angle += .01 * sin(TIME);
	float hue = angle;
	float lum = d;
	vec3 clr = hsv2rgb(hue,0.66,lum + .1 * sin(TIME) );

    return clr;
}

vec2 sincostime( vec2 p ){
	p.x=p.x+sin(p.x*2.0+TIME)*0.4-cos(p.y*1.0-TIME)*0.5-sin(p.x*3.0+TIME)*0.3+cos(p.y*3.0-TIME)*0.1;
	p.y=p.y+sin(p.x*5.0+TIME)*0.7+cos(p.y*8.0-TIME)*0.3+sin(p.x*4.0+TIME)*0.5-cos(p.y*6.0-TIME)*0.3;
	return p;
}

void main(void)
{

	vec2 unipos = (gl_FragCoord.xy / RENDERSIZE);
	vec2 pos = unipos*2.0-1.0;
	pos.x *= RENDERSIZE.x / RENDERSIZE.y;

	pos=sincostime(pos);
	vec3 clr = ring_clr(pos);
	
	
	
	
	
	vec3 grayXfer = vec3(0.3, 0.59, 0.11);
	vec3 gray = vec3(dot(grayXfer, clr));
		
    
  
  
    
    
    gl_FragColor = vec4(mix(clr, gray, desaturation), 1.);
    
		
}
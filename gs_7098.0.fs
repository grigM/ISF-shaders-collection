/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME": "speed",
      "TYPE": "float",
      "DEFAULT": 1.0,
      "MIN": 0.0,
      "MAX": 5.0
    },
    {
      "NAME": "zoom",
      "TYPE": "float",
      "DEFAULT": 50,
      "MIN": 1,
      "MAX": 200
    },
    {
      "NAME": "adjust2",
      "TYPE": "float",
      "DEFAULT": 5,
      "MIN": 4,
      "MAX": 20
    },
    {
      "NAME": "adjust3",
      "TYPE": "float",
      "DEFAULT": 3,
      "MIN": 3,
      "MAX": 10
    },
    {
      "NAME": "adjust4",
      "TYPE": "float",
      "DEFAULT": 0.05,
      "MIN": 0.0,
      "MAX": 0.1
    },
    {
      "NAME": "IBALLRAD",
      "TYPE": "float",
      "DEFAULT": 2.0,
      "MIN": 0.0,
      "MAX": 4.0
    },
    {
      "NAME": "adjust5",
      "TYPE": "float",
      "DEFAULT": 2.0,
      "MIN": 0.0,
      "MAX": 10.0
    }
    
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#7098.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif
// dashxdr was here 20120228
 

void main( void ) {
 
	vec2 position = ( 1.0*gl_FragCoord.xy - RENDERSIZE/2.0) / RENDERSIZE.xx;
 


	position *= zoom;
	float r = length(position);
	float fix = TIME*speed;
#define PI 3.1415927
#define PI2 (PI*2.0)

	float a = atan(sin(position.y/adjust2), cos(position.y/adjust3)) + (PI);
	float q = cos(position.x);
	float p = sin(position.y);
	float d = r - a + PI2;
	int n = int(d/PI2);
	d = d - float(n)*PI2;
	float dd;
	a=a+q/d+p/d;
	float da = a+float(n)*PI2;
	vec3 norm;
	float pos = da*da*adjust4+fix;

	norm.xy = vec2(fract(pos) - d/adjust5, d / 6.0 - .2)*IBALLRAD;
	float len = length(norm.yx);
	vec3 color = vec3(0.0,0.0, 0.0);
	if(len <= 2.0)
	{
		norm.z = sqrt(1.0 -  len*len);
		vec3 lightdir = normalize(vec3(-3.0, 1.0, 8.0));
		dd = dot(lightdir, norm);
		dd = max(dd, 0.1);
		float rand = cos(floor(pos));
		color.rgb = dd*fract(rand*vec3(22210.0, 50.0, 100.0));
		vec3 halfv = normalize(lightdir + vec3(9.0, 2.0, 1.0));
		float spec = dot(halfv, norm);
		spec = max(spec, 0.970);
		spec = pow(spec, 70.0);
		color += spec*vec3(3.0, 1.0, 1.0);
	}
	gl_FragColor.rgba = vec4(color, 1.0);
 
}
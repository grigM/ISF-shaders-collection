/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
       "DEFAULT" : [
        0.5,
        0.5
      ],
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#56559.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float rand(vec2 n) { 
	return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}



float noise(vec2 p){
	vec2 ip = floor(p);
	vec2 u = fract(p);
	u = u*u*(3.0-2.0*u);
	
	float res = mix(
		mix(rand(ip),rand(ip+vec2(1.0,0.0)),u.x),
		mix(rand(ip+vec2(0.0,1.0)),rand(ip+vec2(1.0,1.0)),u.x),u.y);
	return res*res;
}

#define NUM_OCTAVES 5



float fbm(vec2 x) {
	float v = 0.0;
	float a = 0.5;
	vec2 shift = vec2(100);
	// Rotate to reduce axial bias
    mat2 rot = mat2(cos(0.5), sin(0.5), -sin(0.5), cos(0.50));
	for (int i = 0; i < NUM_OCTAVES; ++i) {
		v += a * noise(x);
		x = rot * x * 2.0 + shift;
		a *= 0.5;
	}
	return v;
}

void main( void ) {

	vec2 p= ( gl_FragCoord.xy / RENDERSIZE.xy ) *0.5-1.;
	
	float r = atan(p.x,p.y)+TIME*0.01;
	
	float c = fbm(mouse.x*50.*vec2(cos(r),sin(r))+mouse.y*10.*vec2(fbm(p+.01*TIME),-fbm(p+.01*TIME)));
	vec3 col =vec3(c);
	if(c<0.4&&c>0.3)
	{
		c =1.;
	}
	else
	{
		c=0.;
	}
	gl_FragColor = vec4( vec3(c), 1.0 );

}

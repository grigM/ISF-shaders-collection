/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35369.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif
 
 
 

#define rotate(angle) mat2(cos(angle), sin(-angle), sin(angle), cos(angle))

float hex( vec2 p, vec2 h )
{
	vec2 q = abs(p);
	return max(q.x-h.y,max(q.x+q.y*0.57735,q.y*1.1547)-h.x);
}


float hix(vec2 p, vec2 h){
	float retur = 1.;
	for(float i = 0.; i <= 1.; i += 1./10.){
		p += -(mouse-.5)*i;
		p *= rotate(0.1*cos(4./(1.+i)+TIME));
		p *= 1.1;
		if(hex(p, h) > 0.){
			retur = 1.-retur;
		}
	}
	return retur;
}
 
void main( void )
{

	float radius = 0.25;
	vec2 p =  gl_FragCoord.xy/RENDERSIZE - vec2(0.50, .50);
	p.y /= RENDERSIZE.x / RENDERSIZE.y;
 
	float d = hix(p, vec2(radius));
	float c = 1.0 - smoothstep(0.0, .007, d*1.50);
	
	gl_FragColor = vec4(c);
//	gl_FragColor = vec4(gl_FragCoord.xy, 0.0, 0.0);
 
}
 
/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
	{
			"NAME": "saturation",
			"LABEL": "saturation",
			"TYPE": "float",
			"DEFAULT": 0.85,
			"MIN": 0.0,
			"MAX": 1.0
		}
	
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35499.2"
}
*/


//  Tried to make some more interesting colours, and use a little recursion, to make the 
//  patterns more intense.
//  By @dennishjorth.

#ifdef GL_ES
precision mediump float;
#endif


#define MAX_ITER 10

float rand( float n )
{
    return fract(n);
}
void main( void ) {
	vec2 sp = vv_FragNormCoord;//vec2(.4, .7);
	vec2 p = sp*6.0 - vec2(125.0);
	vec2 i = p;
	float c = 1.0;
	float cc = rand(1.);
	int ccc = int(cc);
	float rc = 0.0;
	
	float inten = 0.0095;
	
		for (int n = 0; n < MAX_ITER; n++) 
		{
			float t = TIME* (0.05 - (0.75 / float(n+1)));
			i = p + vec2(rc*0.4+sin(i.x+TIME*0.24+cos(i.y+sin(cos(t - i.x) + sin(t + i.y))+TIME*0.35)), 
				     rc*0.4+sin(i.y+TIME*0.23+cos(i.x+cos(sin(t - i.y) + cos(t + i.x))+TIME*0.26)));
			c += 1.0/length(vec2(p.x / (sin(i.x+t)/inten),p.y / (cos(i.y+t)/inten)));
			rc = c+rc*0.9;
		}
			c /= float(MAX_ITER);
			c = 1.5-sqrt(c);
		float cb = pow(c,25.0);
	        float cr = cb;
	        float cg = cr;
	        
	        
	        vec3 f = vec3(cr*1.5,cg,cb*5.0);
	        
	      	f=mix(vec3(length(f)),f,saturation);
	       
		//gl_FragColor = vec4(cr*1.5,cg,cb*5.0,1.0);
		
		
			gl_FragColor = vec4(f,1.);
	
} 
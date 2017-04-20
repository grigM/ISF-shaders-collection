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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34716.1"
}
*/


/*** Sierpinski carpet ***/

#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159


const float it = 6.0; // Number of iterations

void main( void ) {

	float mx = max(RENDERSIZE.x, RENDERSIZE.y);
	vec2 scrs = RENDERSIZE/mx;
	vec2 uv = gl_FragCoord.xy/mx;
	vec2 m = vec2(mouse.x/scrs.x,mouse.y*(scrs.y/scrs.x));
	
	//uv+=m;
	float v = pow(3.0,it)+100.0;
	
	gl_FragColor = vec4(0.0); // Background color
	
	for (float i = 0.0; i < it; i++)
	{
		if(floor(mod(uv.x*v,3.0))==1.0 && floor(mod(uv.y*v,3.0))==1.0){
			 
			gl_FragColor = vec4(((sin(i*uv.y-TIME*0.5+2.0*PI/3.0)+1.0))/2.0, // RED
					    ((sin(i*uv.y-TIME*0.5+4.0*PI/3.0)+1.0))/2.0, // GREEN
					    ((sin(i*uv.y-TIME*0.5+6.0*PI/3.0)+1.0))/2.0, // BLUE
					    1.0);					// ALPHA
		                            // *** fun colors !! 
		}
		v/=3.0;	
		//uv.y =uv.y+TIME/30.;// let's scrolling gtr 
		
		(mouse.x>0.5) ? uv.x =uv.x+TIME/30. : uv.x =uv.x-TIME/30.;
		(mouse.y<0.5) ? uv.y =uv.y+TIME/30. : uv.y =uv.y-TIME/30.;
	}
}
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
           "DEFAULT": 1.00,
            "MIN": 0.0,
            "MAX": 10.0
          },
		{	
            "NAME": "fract",
            "TYPE": "float",
           "DEFAULT": 20.00,
            "MIN": 1.0,
            "MAX": 80.0
          },
          {	
            "NAME": "sin",
            "TYPE": "float",
           "DEFAULT": 4.00,
            "MIN": 1.0,
            "MAX": 12.0
          },
          {	
            "NAME": "rad",
            "TYPE": "float",
           "DEFAULT": 0.30,
            "MIN": 0.0,
            "MAX": 2.0
          },
          {	
            "NAME": "enable_circ",
            "TYPE": "bool",
           	"DEFAULT": 0
          },


  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60083.0"
}
*/


// amiga demo style 

#ifdef GL_ES
precision mediump float;
#endif

//#extension GL_OES_standard_derivatives : enable


void main( void ) {

	 
	vec2 p =  (gl_FragCoord.xy - .5 * RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
    //p.x *= RENDERSIZE.x/RENDERSIZE.y;
	p.y -= -0.5;
    float d =  1.0;
	if(enable_circ){
		
		d =  smoothstep(0.1,1.0,length(p-vec2(0.0, 0.5)))/rad+log(p.y/20.0+0.5);
		p.y -= 0.5;
	}
	
	
	
    float a =   fract(float(int(fract))*p.y)/d;
    float b =   fract(float(int(fract))*p.y)/d;
    float c =   fract(float(int(fract))*p.y)/d;
    	
  
   	 
   	 
    
	 gl_FragColor = vec4((p.y*a)*abs(sin(sin*p.y+(TIME*speed)*1.)),
			     		(p.y*b)*abs(sin(sin*p.y+(TIME*speed)*2.)),
			     		(p.y*c)*abs(sin(sin*p.y+(TIME*speed)*3.)),1.0)*2.;
	 

}
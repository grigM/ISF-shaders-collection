/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4lGGRK by cutmod.  based on https:\/\/www.shadertoy.com\/view\/4sc3DB with a few minor tweaks to glitch it out.",
  "PASSES" : [
    {
      "TARGET" : "BufferA",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {

    }
  ],
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    },
    
    {
      "NAME" : "draw",
      "TYPE" : "bool",
      "DEFAULT": true
    },
    
    {
      "NAME" : "clear",
      "TYPE" : "event"
    }
  ]
}
*/


//based on https://www.shadertoy.com/view/4sc3DB

//based on https://www.shadertoy.com/view/4sc3DB

void main() {
	if (PASSINDEX == 0)	{
	
	if(clear){
		gl_FragColor = vec4(0.,0.,0.,0);
		
	}else{

	 //   vec2 uv = (gl_FragCoord.xy/RENDERSIZE.xy);
	    vec2 uv = 0.999*(gl_FragCoord.xy/RENDERSIZE.xy)+0.001;
	
	    if (draw) {      
	        if (length(vec2(sin(TIME)*.75, cos(TIME*1.1294)*.75)-(uv*2.-1.)) < .03) {
	                    gl_FragColor = vec4(1.,.9,1.,1.);
	        return;
	        }
	    } else {
	    if (length(iMouse.xy/RENDERSIZE.xy-uv) < .03) {
	        gl_FragColor = vec4(1.,0.,1.,1.);
	        return;
	    }
	    }
	    
	    vec4 c = IMG_NORM_PIXEL(BufferA,mod(uv,1.0))*5.;
	    
	    vec2 odr = 0.75/RENDERSIZE.xy;
	    
	    vec4 cLeft = IMG_NORM_PIXEL(BufferA,mod(uv-vec2(odr.x,0.),1.0)),
	         cRight = IMG_NORM_PIXEL(BufferA,mod(uv+vec2(odr.x,0.),1.0)),
	         cUp = IMG_NORM_PIXEL(BufferA,mod(uv-vec2(0.,odr.y),1.0)),
	         cDown = IMG_NORM_PIXEL(BufferA,mod(uv+vec2(0.,odr.y),1.0));
	    
	    c += cLeft.wyzx*(abs(cos(TIME+uv.x*32.234+cRight.w*32.234))+1.);
	    c += cRight.zxyw*(abs(cos(uv.x*32.234+cLeft.z*32.34+TIME*1.36))+1.);
	    c += cUp*(abs(cos(TIME*2.12+uv.y*32.1432+cDown.y*32.24))+1.);
	    c += cDown.wzyx*(abs(cos(uv.y*32.345+cUp.x*32.234))+1.);
	       c=(c*c)/3.0;
	           
	    gl_FragColor = max(c/11.6-.0001, 0.);
	}
	}
	else if (PASSINDEX == 1)	{


		gl_FragColor = IMG_NORM_PIXEL(BufferA,mod(gl_FragCoord.xy / (RENDERSIZE.xy),1.0));
	}
}

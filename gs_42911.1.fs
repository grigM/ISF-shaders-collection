/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#42911.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif
 
  

const float pi=3.14159265359;

 
vec2 tile_num = vec2(80.0,40.0);

void main( void ) {
	vec2 p  = gl_FragCoord.xy/RENDERSIZE.xy;
	    // p.x *= RENDERSIZE.x/RENDERSIZE.y;
	
	vec2  p2 = floor(p*tile_num)/tile_num;
        
	  p -= p2;
	
          p *= tile_num;

	  p2 += vec2(step(p.y,p.x)/(2.0*tile_num.x),step(p.x,p.y)/(2.0*tile_num.y));
	
	  
	
	
	float vr = 0.5*sin(40.* ( p2.y+p2.x*0.0)+TIME*2.)+0.5 ;
	
	float vg = 0.5*sin(30.* ( p2.y+p2.x*0.0)+TIME*3.)+0.5 ;
	
	float vb = 0.5*sin(20.* ( p2.y+p2.x*0.0)+TIME*4.)+0.5 ;
	  	
	gl_FragColor = vec4(vr,vg,vb,1);
}
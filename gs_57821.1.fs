/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57821.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


//https://www.shadertoy.com/view/XsVSzW

void main( void )
{

	vec2 uv =  8.*( gl_FragCoord.xy / RENDERSIZE.xy )-4.0  ;
	
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
		
	float p = length(uv);
	
	uv =p*floor(uv*32.0)/32.0;

	float i0=1.2 ;
	float i1=2.95;
	float i2=1.4;
	vec2 i4=vec2(1.0,1.0);
	for(int s=0;s<8;s++)
	{
		vec2 r = vec2(0.0,0.0);
		r=vec2(cos(uv.y*i0-i4.y+TIME/i1),sin(uv.x*i0+i4.x+TIME/i1))/i2;
		r+=vec2(-r.y,r.x)*0.5;
		uv.xy+=r*p;
        
		i0*=1.93;
		i1*=0.45;
		i2*=1.8;
		i4+=r.xy*1.0+0.1*TIME*i1;
		
		if(p>2.0) break;              // ---------------------
	}                                                          // |
	float r=sin(uv.x-TIME)*0.5+0.9;                            // |
	float b=sin(uv.y+TIME)*0.5+0.5;                            // |
	float g=sin((sqrt(uv.x*uv.x+uv.y*uv.y)+TIME))*0.5+0.5;     // |
	vec3 c=vec3(r,g,b);                                        // |
	if(p<2.5)   //<-______________________________________________/
	
	gl_FragColor = vec4(c,1.0);
}
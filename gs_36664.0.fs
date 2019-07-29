/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36664.0",
  "INPUTS" : [

  ],
  "PERSISTENT_BUFFERS" : [
    "backbuffer"
  ],
  "PASSES" : [
    {
      "TARGET" : "backbuffer"
    }
  ]
}
*/


//--- FTL ---
// by Catzpaw 2016
#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec3 star(vec2 uv,float scale,float seed){
	uv*=scale;vec2 s=floor(uv),f=fract(uv),p;float k=3.,d;
	p=.5+.44*sin(11.*fract(sin((s+seed)*mat2(7.5,3.3,6.2,5.4))*55.))-f;d=length(p);k=min(d,k);
	k=smoothstep(0.,k,0.007);
    	return k*vec3(k,k,1.);
}

void main(void){
	//vec2 sp = (vv_FragNormCoord*0.5+0.5)*1.0-0.5;
	
	vec2 sp=(gl_FragCoord.xy*2.-RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y); 
	
	sp = mix(sp,sp/dot(sp,sp),max(0.0,sin(TIME*0.1)));
	vec4 b=IMG_NORM_PIXEL(backbuffer,mod(abs(fract(sp*0.5*sin(TIME*0.2)/RENDERSIZE.xy)),1.0));
	vec2 uv=sp*0.5;
	//uv /= 1.-dot(uv,uv);
	vec3 c=vec3(0);
	for(float i=0.;i<20.;i+=2.)c+=star(uv,mod(20.+i-TIME*1.,20.),i*5.1);
	gl_FragColor = vec4(c+b.gbb*.8,1);
}
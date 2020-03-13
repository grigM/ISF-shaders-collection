/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59153.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/WsVXWR
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

#define TIME (0.1*TIME+2.*length(vv_FragNormCoord))

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE

// Emulate some GLSL ES 3.x
#define round(x) (floor((x) + 0.5))

// --------[ Original ShaderToy begins here ]---------- //
float smin(float a,float b,float k){
	float h=clamp(.5+.5*(b-a)/k,0.,1.);
	return mix(b,a,h)-k*h*(1.-h);
}

float sdf(vec2 p){
	float d0=length(p-vec2(.12,2.12))-2.868,
    	  d1=length(p-vec2(-1.96,-1.165))-.5358,
    	  d2=length(p-vec2(-2.89,.57))-.3312,
    	  d3=length(p-vec2(.66,-1.25))-1.7749,
    	  d4=length(p-vec2(1.93,-3.77))-2.1737,
    	  d5=length(p-vec2(-2.63,-.22))-.2404,
    	  d6=length(p-vec2(-2.596,-.56))-.1956,
    	  d7=length(p-vec2(-1.596,-.2))-1.;
    return min(d7,max(-p.y-4.,smin(min(d5,d6),smin(min(d0,min(d1,d4)),min(d2,d3),1.),.4)));
}

void mainImage(out vec4 O,in vec2 U){
    vec2 R=iResolution.xy,
    	 p=10.*(U+U-R)/R.y;
    float b=1.,
          f=abs(1./sin(iTime))-1.;
    p=p+f*f*(fract(sin(floor(p.yx)))-.5);
    p.x*=sign(sin(iTime/2.));
    p.y*=sign(cos(iTime/2.));
    vec2 q=abs(p)-9.;
  	float d0=length(max(q,0.))+min(max(q.x,q.y),0.);
    float d1=sdf(p);
    float d2=abs(p.x)+abs(p.y-2.)-1.;
    //Old looping code
    /*for(float i=0.;i<=1.;i+=.1){
    	b=min(b,abs(mix(d0,d1,i)));
    }
    for(float i=0.;i<=1.;i+=.1){
    	b=min(b,abs(mix(d1,d2,i)));
    }*/
    
    //Thanks to FabriceNeyret2 The loops are gone!
    float i=round(d0/(d0-d1)*10.)/10.;
    if(i>=0.&&i<=1.)b=min(b,abs(mix(d0,d1,i)));
    i=round(d1/(d1-d2)*10.)/10.;
    if(i>=0.&&i<=1.)b=min(b,abs(mix(d1,d2,i)));
    
    b=min(b,max(abs(d1)-.05,0.));
    
    O=1.-vec4(smoothstep(0.,30./R.y,b));
    /*O.r+=b*p.y*p.x;
    O.b-=b*p.y*p.x;*/
    O=sqrt(max(O,0.));
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
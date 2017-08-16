/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#41554.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


void main( void ) {

    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv -=.5;
    uv.x*=RENDERSIZE.x/RENDERSIZE.y;
    float c=1.;
    for(int i=0;i<30;++i){
        float y=.7-1.4*float(i)/30.+.1*sin(TIME+uv.x*10./(1.+exp(-.1*pow(abs(float(i)-15.),2.)))+float(i)*6.2832/4.)*exp(-5.*pow(length(uv),2.));
        c=mix(c,1.-exp(-50.*(y-uv.y)),smoothstep(uv.y,uv.y+2./RENDERSIZE.y,y));
    }
	gl_FragColor = vec4(c);
}
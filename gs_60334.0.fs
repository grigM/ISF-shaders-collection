/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60334.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/ldyXD3
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE

// --------[ Original ShaderToy begins here ]---------- //
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	float t=iTime*.1;
	vec2 uv = gl_FragCoord.xy / iResolution.xy-.5;
	vec2 ouv=uv;
	uv.x*=iResolution.x/iResolution.y;
	vec3 rd=normalize(vec3(uv,2.));
	rd.xy*=mat2(cos(t),sin(t),-sin(t),cos(t));
	vec3 ro=vec3(t+sin(t*6.53583)*.05,.01+sin(t*352.4855)*.0015,-t*3.);
	vec3 p=ro;
	float v=0., td=-mod(ro.z,.005);
	for (int r=0; r<150; r++) {
		v+=pow(max(0.,.01-length(abs(.01-mod(p,.02))))/.01,10.)*exp(-2.*pow((1.+td),2.));
		p=ro+rd*td;
		td+=.005;
	}
	fragColor = vec4(v*v*v,v*v,v,0.)*8.*max(0.,1.-length(ouv*ouv)*2.5);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
    gl_FragColor.a = 1.0;
}
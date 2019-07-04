/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#48302.1"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/XlX3Rj
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy globals
float iTime = 0.0;
vec3  iResolution = vec3(0.0);

#define texture(s, uv) vec4(0.0)
#define textureLod(s, uv, lod) vec4(0.0)

// --------[ Original ShaderToy begins here ]---------- //
#define TIME iTime*.02


#define width .005
float zoom = .18;

float shape=0.;
vec3 color=vec3(0.),randcol;

void formula(vec2 z, float c) {
	float minit=0.;
	float o,ot2,ot=ot2=1000.;
	for (int i=0; i<9; i++) {
		z=abs(z)/clamp(dot(z,z),.1,.5)-c;
		float l=length(z);
		o=min(max(abs(min(z.x,z.y)),-l+.25),abs(l-.25));
		ot=min(ot,o);
		ot2=min(l*.1,ot2);
		minit=max(minit,float(i)*(1.-abs(sign(ot-o))));
	}
	minit+=1.;
	float w=width*minit*2.;
	float circ=pow(max(0.,w-ot2)/w,6.);
	shape+=max(pow(max(0.,w-ot)/w,.25),circ);
	vec3 col=normalize(.1+texture(iChannel1,vec2(minit*.1)).rgb);
	color+=col*(.4+mod(minit/9.-TIME*10.+ot2*2.,1.)*1.6);
	color+=vec3(1.,.7,.3)*circ*(10.-minit)*3.*smoothstep(0.,.5,.15+texture(iChannel0,vec2(.0,1.)).x-.5);
}


void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	vec2 pos = fragCoord.xy / iResolution.xy - .5;
	pos.x*=iResolution.x/iResolution.y;
	vec2 uv=pos;
	float sph = length(uv); sph = sqrt(1. - sph*sph); // curve for spheric distortion
	uv=normalize(vec3(uv,sph)).xy;
	float a=.5+mod(.5,1.)*.5;
	vec2 luv=uv;
	float b=a*5.48535;
//	zoom*=1.+sin(TIME*3.758123)*.8;
	uv*=mat2(cos(b),sin(b),-sin(b),cos(b));
	uv+=vec2(sin(a),cos(a*.5))*8.;
	uv*=zoom;
	float pix=.5/iResolution.x*zoom/sph;
	float dof=max(1.,(10.-mod(TIME,1.)/.01));
	float c=1.5+mod(floor(TIME),6.)*.125;
	for (int aa=0; aa<36; aa++) {
		vec2 aauv=floor(vec2(float(aa)/6.,mod(float(aa),6.)));
		formula(uv+aauv*pix*dof,c);
	}
	shape/=36.; color/=36.;
	vec3 colo=mix(vec3(.15),color,shape)*(1.-length(pos))*min(1.,abs(.5-mod(TIME+.5,1.))*10.);	
	colo*=vec3(1.2,1.1,1.0);
	fragColor = vec4(colo,1.0);
}
// --------[ Original ShaderToy ends here ]---------- //
#undef TIME

void main(void)
{
    iTime = TIME;
    iResolution = vec3(RENDERSIZE, 0.0);

    mainImage(gl_FragColor, gl_FragCoord.xy);
}
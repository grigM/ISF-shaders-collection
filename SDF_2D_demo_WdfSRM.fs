/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WdfSRM by saidwho12.  okogkdfgdf\n",
  "INPUTS" : [

  ]
}
*/


#define rot(x) mat2(cos(x),-sin(x),sin(x),cos(x))

float udBox(vec2 p, vec2 b) {
	return length(max(abs(p)-b,0.));
}

float sdBox(vec2 p, vec2 b) {
  vec2 d = abs(p) - b;
  return length(max(d,0.0)) + min(max(d.x,d.y),0.0); 
}

#define NAND(a,b) (-max(a,b))
#define NOT1(x) NAND(x,x)
#define AND(a,b) NOT1(NAND(a,b))
#define OR(a,b) NAND(NOT1(a),NOT1(b))
#define XOR(a,b) AND(OR(a,b),NAND(a,b))
#define NOT(a,b) AND(a,NOT1(b))
//#define XOR(A,B) OR(AND(A,NOT1(B)), AND(B,NOT1(A)))

#define PI acos(-1.)
#define TAU (PI+PI)

void main() {

	vec2 R = RENDERSIZE.xy, p = /*rot(.1*TIME) **/ (gl_FragCoord.xy+gl_FragCoord.xy-R)/R.y;
    
    float t = XOR( length(p-vec2(-.25*sin(TIME),0))-.5, length(p-vec2(.25*sin(TIME),cos(TIME)))-.5 );
    t = AND(t,sdBox(p,vec2(.5)));
    
    float N = 32.;
    
    float a = max(sin(N*(t-.05*TIME)*6.28),0.) * exp(-8.*abs(t)) * (.7+.3*sin(.15*TAU*N*abs(t)-9.*TIME));
    
    gl_FragColor.rgb = mix(vec3(a),vec3(.34), smoothstep(3./R.y,0.,t));
    gl_FragColor.rgb = mix(vec3(gl_FragColor), .5 + .5*sin( 4.*TIME + vec3(0,1./2.,1)*PI ), smoothstep(3./R.y,0.,abs(t)-.005));
}

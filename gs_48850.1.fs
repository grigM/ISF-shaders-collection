/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#48850.1"
}
*/


// nabr - 2017, Ziad 2018
// Attribution 3.0 International (CC BY 3.0)
// https://creativecommons.org/licenses/by/3.0/
// original https://www.shadertoy.com/view/XlsBRn

// always want to update this thx for watching

//precision lowp float;

mat2 m =mat2(0.8,0.6, -0.6, 0.8);

float rand(vec2 n) { 
	return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}

float noise(vec2 n) {
	const vec2 d = vec2(0.0, 1.0);
  	vec2 b = floor(n), f = smoothstep(vec2(0.0), vec2(1.0), fract(n));
	return mix(mix(rand(b), rand(b + d.yx), f.x), mix(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}

float fbm(vec2 p){
	float f=.0;
	f+= .5000*noise(p); p*= m*2.02;
	f+= .2500*noise(p); p*= m*2.03;
	f+= .1250*noise(p); p*= m*2.01;
	f+= .0625*noise(p); p*= m*2.04;
	
	f/= 0.9375;
	
	return f;
}
void main(){

    float t = TIME*.1;
	
    vec3 ro = vec3(.0, .0, -15.);
    vec3 rd = normalize(vec3((gl_FragCoord.xy * 2. - RENDERSIZE.xy) / min(RENDERSIZE.x, RENDERSIZE.y), 1.));
    
    // ---- rotation 

    float  s = sin(t), c = cos(t);
    mat3 r = mat3(1., 0, 0,0, c, -s,0, s, c) * mat3(c, 0, s,0, 1, 0, -s, 0, c);
	
    // ---- positions 
    vec3  n = vec3(  mod(t,5.)/12.+1.49  );

    // ---- cube length (max(abs(x) - y, 1.) )
	float flicker = fract( mod(TIME*1.3,.45) / sin(TIME*1.2) );
	for (int i = 0; i < 9; i++) {
    ro += (length(sin(ro*sin(ro*1.1)*r)-n-cos(3.1)/atan(ro.z*tan(.7)/120.-exp(.0001*t),-rd.z) )-.9) * rd;
		ro-=smoothstep(.2,.9,fract(ro));
	}

    // ---- shading
    gl_FragColor.rgb = (vec3(.1, .25, 0.27)  *-ro.z +0.8);
    gl_FragColor.a = 1.;

}
/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#26700.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


// You're so stupid and lazy. Initialize your variables or stop making shaders.
// depth of field approx, tests...

float evalZ(float fx, float fy){
	return sin(0.5*TIME+0.3*fx)*cos(0.5*TIME+0.8*fy);
}

const int Nx = 9;
const int Ny = 6;

void main( void ) {

	vec2 pos =  gl_FragCoord.xy / RENDERSIZE.x - vec2(0.5, 0.5*RENDERSIZE.y/RENDERSIZE.x);
	pos*=RENDERSIZE.x/RENDERSIZE.y;
	
	vec3 rgb = vec3(0.0);
	
	
	for(int x = -Nx; x < Nx; ++x){	
		for(int y = -Ny; y < Ny; ++y){
			float fx = float(x);
			float fy = float(y);
			float z = evalZ(fx,fy);
			vec3 p = vec3(0.1*fx, 0.1*float(y), z);
			p = p*(1.+0.1*z);
			
			
			
			float s = 0.01*abs(p.z)+0.002;
			float R0 = 0.01+0.01*z;
			
			float d = distance(pos, p.xy);
			float i = 1.-smoothstep(R0-2.*s,R0+2.*s,d);
			i*=(1.)/(0.1+300.*s);
			
			rgb+=i;	
		}
	}
	
	gl_FragColor = vec4(rgb,1.);

}
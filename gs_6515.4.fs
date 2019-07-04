/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#6515.4"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


#define STEP 0.2
#define R_STEP 0.1

#define GRID_R_STEP 0.06
#define GRID_ANG_STEP (3.14 / 9.0)

float grid(vec2 p){
	if(mod(p.y, GRID_R_STEP) < 0.00){
	//	return 1.0;
	}
	if(mod(p.x, GRID_ANG_STEP) < 0.00){
		return 1.0;
	}
	return 0.0;
}

float gear(vec2 p){
	if(p.y < 0.0){
		return 0.0;
	}
	p.x += p.y * 1.;
	if(mod(p.x, R_STEP) < R_STEP * 0.4){
		p.y += STEP * 0.;
	}
	if(mod(p.y, STEP) < STEP * 0.041){
		return 1.0;
	}
	else{
		return 0.0;
	}
}

float color(vec2 p){
	return gear(p) + grid(p);	
}


vec2 transform(vec2 p){
	float r = length(p);
	if(r < 0.001){
		return vec2(-1.0);
	}
	return vec2(atan(p.y, p.x) + TIME * 0.5, 1.0 / length(p) + TIME * 1.0);
}

void main( void ) {

	vec2 pos = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / RENDERSIZE.y ;

		
	gl_FragColor = vec4(color(transform(pos)));

}
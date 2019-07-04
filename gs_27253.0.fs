/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#27253.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


float al_tri(vec2 p, vec2 p0, vec2 p1, vec2 p2) {
    float s = p0.y * p2.x - p0.x * p2.y + (p2.y - p0.y) * p.x + (p0.x - p2.x) * p.y;
    float t = p0.x * p1.y - p0.y * p1.x + (p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y;

    //if ((s < 0.) != (t < 0.)) {
    //return 0.;
    //}

    float A = -p1.y * p2.x + p0.y * (p2.x - p1.x) + p0.x * (p1.y - p2.y) + p1.x * p2.y;
    if (A < 0.0)
    {
        s = -s; 
        t = -t;
        A = -A;
    }
	
    return float(s > 0. && t > 0. && (s + t) < A);
}

vec3 HueShift (in vec3 Color, in float Shift)
{
    vec3 P = vec3(0.55735)*dot(vec3(0.55735),Color);
    
    vec3 U = Color-P;
    
    vec3 V = cross(vec3(0.55735),U);    

    Color = U*cos(Shift*6.2832) + V*sin(Shift*6.2832) + P;
    
    return vec3(Color);
}


float tri(vec2 p, vec2 p0, vec2 p1, vec2 p2) {
    float ret = 0.;
    for (int i = 0; i < 16; i++) { 
    vec2 q = vec2(floor(mod(float(i), 4.0)) / 4.0, floor(float(i)) / 16.0);
    vec2 uv = p+(q/600.);
    ret += al_tri(uv,p0,p1,p2);
    }
    return ret/16.;
}
void main( void ) {
	#define PI 3.141592653589793238462
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	vec2 suv = ((vec2(uv.x,((uv.y-0.5)*(RENDERSIZE.y/RENDERSIZE.x))+0.5)-.5)*2.)+.5;
	
	vec3 colors = vec3(0.);
	
	float atans = (atan(suv.x-0.5,suv.y-0.5)+PI)/(PI*2.);
	
	for (float i = 0.; i < 7.; i++) {
		vec2 a = (vec2(cos(i*3.),sin((i*3.)+TIME))+1.)/2.;
		vec2 b = (vec2(cos(i+0.5*3.),sin(i+0.5*3.))+1.)/2.;
		vec2 c = (vec2(cos((i+3.)-TIME),sin(i+1.*3.))+1.)/2.;
		colors += vec3(HueShift( vec3(tri(suv,a,b,c),0.,0.) , atans*i/4.2 ));
	}
	
	
	
	
	gl_FragColor = vec4(1.-colors, 1.0 );

}
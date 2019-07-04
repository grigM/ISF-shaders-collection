/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#6186.7"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

// T21 : More colors


float rand(vec3 n, float res){
	n = floor(n*res);
	return fract(sin((n.x+n.y*1e2+n.z*1e4)*1e-2)*1e5);
}

float color(vec3 n, float res){
	vec3 edge = abs(abs(fract(n)-.5)-.2);
	edge += edge.yzx;
	float onedge = min(edge.x,min(edge.y,edge.z));
	return mix(rand(n, res), .999, step(onedge, .05));
}

mat3 RotZAxis(float a) {
	float cosa = cos(a), sina = sin(a);
	return mat3(
		cosa, -sina, 0.,
		sina,  cosa, 0.,
		0., 0., 1.);
}

vec3 GetCoord(vec3 p) {
	float a = TIME*(rand(p, 1.)*2.-1.);
	p = fract(p)-.5;
	return p*RotZAxis(a);
}

float distance(vec3 p) {
	return length(max(max(abs(GetCoord(p))-.2,0.), abs(p)-2.8));
}

void main( void ) {
	vec3 lookAt = vec3(0);
	vec3 dir = normalize(vec3(mouse.x-.5,mouse.y-.5,1.));
	vec3 left = normalize(cross(dir,vec3(0,1,0)));
	vec3 up = cross(dir,left);
	
	vec3 pos = -dir*10.;
	
	vec2 screen = (gl_FragCoord.xy-RENDERSIZE*.5)/RENDERSIZE.x;
	
	vec3 ray = normalize(dir+left*screen.x+up*screen.y);
	
	float dsum = 0.;
	float d;
	float s= 0.;
	
	for (int i = 0; i < 100; i++) {
		d = distance(pos+ray*dsum);
		dsum += d;
		s += 1.;
		if((d)<.001) break;
		if(dsum > 20.) { s=100.; break; }
	}
	pos += dsum*ray;
	
	float c = color(GetCoord(pos)+vec3(.5,0.5,0.5)+floor(pos), 9.);
	gl_FragColor = vec4(1.-(s/30.))*.5 + vec4(vec3(fract(c*16.),fract(c*32.),fract(c*64.))*(3.-pos.z)*.1,1.);
}
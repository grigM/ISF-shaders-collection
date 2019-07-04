/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#53594.5"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define S smoothstep


// IQ
float sdTriangle( in vec2 p, in vec2 p0, in vec2 p1, in vec2 p2 )
{
    vec2 e0 = p1-p0, e1 = p2-p1, e2 = p0-p2;
    vec2 v0 = p -p0, v1 = p -p1, v2 = p -p2;

    vec2 pq0 = v0 - e0*clamp( dot(v0,e0)/dot(e0,e0), 0.0, 1.0 );
    vec2 pq1 = v1 - e1*clamp( dot(v1,e1)/dot(e1,e1), 0.0, 1.0 );
    vec2 pq2 = v2 - e2*clamp( dot(v2,e2)/dot(e2,e2), 0.0, 1.0 );
    
    float s = sign( e0.x*e2.y - e0.y*e2.x );
    vec2 d = min(min(vec2(dot(pq0,pq0), s*(v0.x*e0.y-v0.y*e0.x)),
                     vec2(dot(pq1,pq1), s*(v1.x*e1.y-v1.y*e1.x))),
                     vec2(dot(pq2,pq2), s*(v2.x*e2.y-v2.y*e2.x)));

    return -sqrt(d.x)*sign(d.y);
}
// end IQ

mat2 rotate(float a) {
	float c = cos(a);
	float s = sin(a);
	return mat2(c, s, -s, c);
}

float hash(vec2 p) {
	return fract(4346.45 * sin(dot(p, vec2(45.45, 757.5))));
}

void main() {
	vec2 uv = (2. * gl_FragCoord.xy - RENDERSIZE) / RENDERSIZE.y;
	vec3 col = vec3(0.);
	
	uv *= 4.;
	uv += TIME / 2.;
	
	vec2 i = floor(uv);
	vec2 f = fract(uv) - .5;
	
	
	float _t = floor(hash(i + floor(TIME / 5. + i.x * 10.)) * 4.) * 3.14 / 2.;
	f *= rotate(_t + floor(hash(i) * 4.) * 3.14 / 2.);
	
	
	
	//float d = dot(f, vec2(1));
	vec2 a = vec2(-.5, -.5);
	vec2 b = vec2(.5, .5);
	vec2 c = vec2(-.5, .5);
	
	// need the proper distance 
	float d = sdTriangle(f, a, b, c);
	
	//col += d;
	col += abs(.01 / d);
	float s = S(.015, .0, d);
	col += s;
	
	float inv = 1. - s;
	
	float k = floor(hash(i) * 6.) / 6.;
	col *= .5 + .5 * cos(TIME + 6.28 * k  + d * 2. + vec3(23, 21, 0));
	//col += .6 * inv * (.5 + .5 * cos(uv.x + TIME / 100. + vec3(23, 21, 0)));
	col += inv;
	
	gl_FragColor = vec4(col, 1.);
}
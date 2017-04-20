/*
{
  "CATEGORIES" : [
    "Automatically Converted"
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36530.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec3 key = vec3(3.0 * (mouse.x * 2.0 - 1.0), mouse.y * 2.0 - 1.0, 0.0);
float hash(float n) {
	return fract(sin(n)*43578.5453);
}

float hash(vec2 n) {
	return hash(dot(n, vec2(48.343, 9.343)));
}

void rotate(inout vec2 p, float a) {
	float s = sin(a);
	float c = cos(a);
	
	p = mat2(c, s, -s, c)*p;
}

float de(vec3 p) {
	vec3 op = p;
	vec2 t = floor(p.xz);
	p.xz = fract(p.xz) - 0.5;
	p.x *= 2.0*floor(fract(hash(t))*1.8) - 1.0;
	
	float d = abs(1.0 - 2.0*abs(dot(p.xz, vec2(1))))/(2.0*sqrt(5.0));
	
	d = max(d - 0.5/4.0, p.y + 0.5);
	
	d = min(d, p.y + 1.0);
	
	d = min(d, length(op - key) - 0.05);
	
	return d;
}

float trace(vec3 ro, vec3 rd, float mx) {
	float t = 0.0;
	for(int i = 0; i < 100; i++) {
		float d = de(ro + rd*t);
		if(d < 0.001 || t >= mx) break;
		t += d;
	}
	return t < mx ? t : -1.0;
}

vec3 normal(vec3 p) {
	vec2 h = vec2(0.001, 0.0);
	vec3 n = vec3(
		de(p + h.xyy) - de(p - h.xyy),
		de(p + h.yxy) - de(p - h.yxy),
		de(p + h.yyx) - de(p - h.yyx)
	);
	return normalize(n);
}

vec3 render(vec3 ro, vec3 rd) {
	vec3 col = vec3(0);
	
	float t = trace(ro, rd, 24.0);
	if(t < 0.0) return col;
	
	vec3 pos = ro + rd*t;
	vec3 nor = normal(pos);

	vec3 lig = normalize(key - pos);
	float dis = length(key - pos) - 0.06;
	
	float sha = step(0.0, -trace(pos + nor*0.001, lig, dis));
	
	col = vec3(0.1);
	col += clamp(dot(lig, nor), 0.0, 1.0)*sha;
	
	//col = mix(col, vec3(0), 1.0 - exp(-0.2*t));
	return col;
}

const float aa = 5.0;
vec3 scene(vec2 uv) {
	vec3 ro = vec3(3.0*sin(0.0), 1, -3.0*cos(0.0));
	vec3 ww = normalize(-ro);
	vec3 uu = normalize(cross(vec3(0, 1, 0), ww));
	vec3 vv = normalize(cross(ww, uu));
	
	
	vec3 rd = normalize(uv.x*uu + uv.y*vv + 1.97*ww);
	
	vec3 col = render(ro, rd)/aa;
	vec2 of = vec2(2.0/(5.0*RENDERSIZE.y));
	for(float i = 0.0; i < (aa - 1.0); i++) {
		uv += of;
		rd = normalize(uv.x*uu + uv.y*vv + 1.97*ww);
		
		col += render(ro, rd)/aa;
	}
	
	return col;
}

void main( void ) {
	vec2 uv = (-RENDERSIZE + 2.0*gl_FragCoord.xy)/RENDERSIZE.y;
	
	vec3 col = scene(uv);
	gl_FragColor = vec4(col, 1);
}
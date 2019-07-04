/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#53392.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


mat2 rotate(float a) {
	float c = cos(a);
	float s = sin(a);
	return mat2(c, s, -s, c);
}

void main() {
	vec2 uv = (2. * gl_FragCoord.xy - RENDERSIZE) / RENDERSIZE.y;
	vec3 col = vec3(0.);
		
	vec3 ro = vec3(0, 0, -3);
	vec3 rd = vec3(uv, 1);
	vec3 p = vec3(0);
	
	float t = 0.0;
	for (int i = 0; i < 256; i++) {
		p = ro + rd * t;
		

		p.xz *= rotate(TIME / 2.);	
		p.z = mod(p.z, 2.0) - 1.0;
		
		// hex shaping
		vec2 _p = abs(p.xy);
		float d = dot(_p, vec2(0.5, 0.86));
		d = max(d, _p.x);
		d = abs(d) - 1.0;
		d = length(vec2(d, p.z)) - 0.1;
		
		if (t > 64.0) break;
		
		t += 0.5 * d;
		
	}
	
	//p = abs(p) / 90.0;
	//col = clamp(normalize(cross(dFdx(p), dFdy(p))), -1.0, 1.0);
	vec3 normal = clamp(normalize(cross(dFdx(p), dFdy(p))), -1.0, 1.0);
	//float dx = dot(normal, normalize(vec3(-1.0, 0.0, 0.0)));
	//float dy = dot(normal, normalize(vec3(0.0, -1.0, 0.0)));
	//float dz = dot(normal, normalize(vec3(0.0, 0.0, -1.0)));
	//vec3 d = cross(normal, normalize(vec3(dx, dy, 1.0)));
	
	col += max(dot(normal, normalize(ro - p)), 0.);
	col *= vec3(exp2(-t*0.1));
	
	gl_FragColor = vec4(col, 1.);
}
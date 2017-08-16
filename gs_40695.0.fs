/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40695.0"
}
*/


/*
bezier curves on the gpu. 
*/

#ifdef GL_ES
precision mediump float;
#endif


float bezierDist (vec2 p0, vec2 p1, vec2 p2, vec2 point){
	vec2 A = p1 - p0;
	vec2 B = p0 - 2. * p1 + p2;
	vec2 C = p0 - point;
	
	float a = dot(B, B);
	float b = 3. * dot(A, B) / a;
	float c = (2. * dot(A, A) + dot(C, B)) / a;
	float d = dot(C, A) / a;
	float p = c - b * b / 3.;
	float q = b * (2. * b * b - 9. * c) / 27. + d;
	float p3 = p * p * p;
	float D = q * q + 4. * p3 / 27.;
	float offset = -b / 3.;
	
	if (D >= 0.) {
		float z = sqrt(D);
		float u = (-q + z) / 2.;
		float v = (-q - z) / 2.;
		
		float t = clamp(sign(u) * pow(abs(u), 1. / 3.) + sign(v) * pow(abs(v), 1. / 3.) + offset, 0., 1.);
		float t1 = 1. - t;
		
		return distance(p0 * t1 * t1 + p1 * t1 * t * 2. + p2 * t * t, point);
	} else {
		float u = 2. * sqrt(-p / 3.);
		float v = acos(-sqrt( -27. / p3) * q / 2.) / 3.;
		
		vec3 t = clamp(u * cos(v + vec3(0., 2., 4.) * 1.0471975512) + offset, 0., 1.);
		vec3 t1 = 1. - t;
		
		return min(min(
			distance(p0 * t1.x * t1.x + p1 * t1.x * t.x * 2. + p2 * t.x * t.x, point),
			distance(p0 * t1.y * t1.y + p1 * t1.y * t.y * 2. + p2 * t.y * t.y, point)),
			distance(p0 * t1.z * t1.z + p1 * t1.z * t.z * 2. + p2 * t.z * t.z, point)
		);
	}
}


void main (){
	float dist = 0.;
        vec2 p1 = vec2(0.1, 0.1) * RENDERSIZE;
	vec2 p2 = vec2(0.5, 0.5) * RENDERSIZE;
	vec2 p3 = vec2(0.5, 0.9) * RENDERSIZE;
        vec2 coord = gl_FragCoord.xy;
		
	dist = bezierDist(p1, p2, p3, coord);
//	dist = clamp(dist - 10., 0., 1.);
	dist = sin(dist/10.0-TIME*3.0)*255.0;
	
	gl_FragColor = vec4(dist, dist, dist, 1.);
}
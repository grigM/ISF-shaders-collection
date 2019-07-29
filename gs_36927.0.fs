/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36927.0"
}
*/


// regularPolygon.frag  yuki_b

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



#define maxpart 10
#define pi 3.1415

vec3 regularPolygon(int parts, float radius, float blur, vec3 color, vec2 pos, float rotate, vec2 p){
	float a = 2.0 * pi * (1.0 / float(parts));
	float size = radius * cos(a /2.0);
	mat2 rmat = mat2(cos(a), -sin(a), sin(a), cos(a));
	float r = 0.0;
	vec2 addv = vec2(sin(-a + pi + rotate), cos(-a + pi + rotate));
	for(int i=0; i<maxpart; i++){
		addv = rmat * addv;
		float r1 = max(r, smoothstep(size, size+blur*(abs(sin(TIME*0.1)*0.15)+0.15), dot(p - pos, normalize(addv))));
		float r2 = max(r, smoothstep(size, size+blur, dot(p - pos, normalize(addv))));
		r = mix(r1, r2, 0.5);
		if (i==parts) break;
	}
	return (1.0 - r) * color;	
}

void main( void ) {

	vec2 p = ( gl_FragCoord.xy / RENDERSIZE.xy ); 
	p.y = 0.5 + ((p.y - 0.5) * (RENDERSIZE.y/RENDERSIZE.x));
	float rot = TIME * 0.91;
	vec3 col = regularPolygon(3, 0.1, 0.22, vec3(1.0, 1.0, 0.0), vec2(0.2, 0.65), rot, p);
	col += regularPolygon(4, 0.1, 0.22, vec3(0.0, 1.0, 1.0), vec2(0.5, 0.65), rot, p);
	col += regularPolygon(5, 0.1, 0.22, vec3(1.0, 0.5, 0.0), vec2(0.8, 0.65), rot, p);
	col += regularPolygon(6, 0.1, 0.22, vec3(1.0, 0.0, 1.0), vec2(0.2, 0.35), rot, p);
	col += regularPolygon(7, 0.1, 0.22, vec3(0.5, 0.9, 0.5), vec2(0.5, 0.35), rot, p);
	col += regularPolygon(8, 0.1, 0.22, vec3(0.5, 0.5, 0.9), vec2(0.8, 0.35), rot, p);
	gl_FragColor = vec4( col, 1.0 );

}
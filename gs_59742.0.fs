/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59742.0"
}
*/


//precision highp float;
void main()
{
	float tm = TIME * .125;
	vec2 st = (gl_FragCoord.xy * 2. - RENDERSIZE) / max(RENDERSIZE.x, RENDERSIZE.y);
	
	vec3 d = vec3(st, -.5);
	
	float t = d.z / dot(vec3(0, 1, 0), d);
	ivec3 h = ivec3(vec3(0, 0, TIME) + (t * d));
	gl_FragColor = vec4((vec3((t) <= 1.), 
				sin( vec3(0, 1, 3) - 1./d.y) + vec3(( h.z) - 2 * ((h.x + h.z) / 2))), 1);
}
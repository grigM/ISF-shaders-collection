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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#30892.4"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float abs_dist(vec2 a, vec2 b){
	vec2 d = abs(b - a);
	return abs(abs(max(d.x,d.y)-50.)-50.);
}

void main() {

	vec2 position = gl_FragCoord.xy;
	
	vec2 point0 = vec2(100.0, 100.0);
	vec2 point1 = mouse * RENDERSIZE;
	vec2 point2 = vec2(200.0, 150.0);
	
	float dist = 1e20;
	
	//dist = min(dist, abs_dist(position, point0));
	dist = min(3000., abs_dist(position, point1));
	dist = min(dist, abs_dist(position, point2));
	
	dist *= 0.01;
	
	float f = floor(dist*30.0)/30.0;

	gl_FragColor = vec4(f);

}
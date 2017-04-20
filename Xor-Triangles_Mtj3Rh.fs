/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "procedural",
    "2d",
    "triangles",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Mtj3Rh by Xor.  2D procedural infinite scrolling triangles.",
  "INPUTS" : [

  ]
}
*/


float r(float n)
{
 	return fract(abs(sin(n*55.753)*367.34));   
}
float r(vec2 n)
{
    return r(dot(n,vec2(2.46,-1.21)));
}
float cycle(float n)
{
 	return cos(fract(n)*2.0*3.141592653)*0.5+0.5;
}
void main() {



    float a = (radians(60.0));
    float zoom = 96.0;
	vec2 c = (gl_FragCoord.xy+vec2(TIME*zoom,0.0))*vec2(sin(a),1.0);
    c = ((c+vec2(c.y,0.0)*cos(a))/zoom)+vec2(floor((c.x-c.y*cos(a))/zoom),0.0);
    float n = cycle(r(floor(c*4.0))*0.2+r(floor(c*2.0))*0.3+r(floor(c))*0.5+TIME*0.125);
	gl_FragColor = vec4(n*2.0,pow(n,2.0),0.0,1.0);
}

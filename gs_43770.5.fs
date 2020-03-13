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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#43770.5"
}
*/


#ifdef GL_ES
precision highp float;
#endif



float triglined(vec2 p1, vec2 p2, vec2 p0){
float b = distance(p1,p0);
float lang = atan(p2.y - p1.y,p2.x - p1.x);
float pang = atan(p0.y-p1.y,p0.x-p1.x);
float dang = lang-pang;

return sin(dang)*b;

}


void main( void ) {

	vec2 position  = gl_FragCoord.xy - RENDERSIZE / 2.;
	vec2 p = 30. * position / RENDERSIZE.x;
	
		float mack = abs(triglined(vec2(30.,30.)/RENDERSIZE.xy,mouse,gl_FragCoord.xy/RENDERSIZE.xy));

	gl_FragColor = vec4(0.02/(mack));

}
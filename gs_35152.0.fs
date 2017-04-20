/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35152.0"
}
*/



void main() {
	float c = fract(sin(length(floor(gl_FragCoord.xy/4.0) + vec2(TIME)))*1e6);
	gl_FragColor = vec4(c, c, c, 1.0);
}
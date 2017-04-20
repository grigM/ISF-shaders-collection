/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "sphere",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XslcD2 by aiekick.  2D Quad",
  "INPUTS" : [

  ]
}
*/


void main() {
	vec4 fragCoordPos = gl_FragCoord;


    fragCoordPos.xy -= gl_FragColor.xy=RENDERSIZE.xy/2.0;
    
    fragCoordPos.xy /= gl_FragColor.y;
    
    float d = pow(abs(.5 - max(abs(gl_FragCoord.x),abs(gl_FragCoord.y))), .2);
        
    fragCoordPos.xy += d;
    
    fragCoordPos.xy *= fragCoordPos.xy;
    
    gl_FragColor = vec4(fragCoordPos.xy,d,1) * d * (0.8 + 0.2 * cos(10. * d+TIME*10.));
}

/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/ww1.shadertoy.com\/view\/WtfSR8 by mosaic.  Variety of transforms on the coordinate system (uv)\nBonus effect: click & drag\n\nOriginal triangle pattern from https:\/\/www.shadertoy.com\/view\/ltjGWt",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


float divScotFlag( vec2 uv) {
    float botRight = step( uv.x + uv.y, 1. );
    float botLeft = step( uv.x + 1. - uv.y, 1. );
    
    // Scottish flag ( sorta )
    float scotFlag = abs(botRight - botLeft);
    float divider = step( uv.x, .5);
    
    // Divided scottish flag
    return abs( divider - scotFlag);
}

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

void main() {



	vec2 uv = (gl_FragCoord.xy-.5*RENDERSIZE.xy) / RENDERSIZE.y; // Aspect Ratio adjustment
    
    //if (iMouse.z <= 0.){
    
    if (uv.x < -0.5) uv=(uv+0.6)*rotate2d(TIME/5.); //rotate
         
    else if (uv.x >= -.5 && uv.x < 0.)  uv.x+=sin(TIME/5.+uv.y);  //wave
        
    else if (uv.x >=0. && uv.x < 0.5)  uv*=(2./1.+sin(TIME/2.)); //zoom
        
    else if ( uv.x >= 0.5)    uv.y/=((uv.x))+cos(TIME/2.)+0.8; // flower
    
         //}
          //mod instead of "pow" here is nice too
   	//uv=(pow(abs(rotate2d(3.14*.25*sin(TIME/5.))*uv),iMouse.xy/RENDERSIZE.y)-TIME/20.);
         
    uv.y *= 5./3.; // adjust height width ratio
    // Checker pattern
	gl_FragColor = vec4(
        vec3( divScotFlag( fract(uv * 4.) ) )+.4, 1.0);
}

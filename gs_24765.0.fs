/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#24765.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main( void ) {
    float myTime = TIME / 50.0;

    vec2 p = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
    p *= mat2(cos(myTime*1.0), sin(myTime*1.0), -sin(myTime*1.0), cos(myTime*1.0));
    
    vec3 destColor = vec3(0.0);
    for(float i = 2.0; i < 10.0; i++){
        float j = i * i;
        vec2 q = p + vec2(sin(myTime * j)*length(cos(myTime)), cos(myTime * j)*length(sin(myTime)));
        destColor += 0.02 * abs(atan(myTime)) / length(q);
    }
    float g = destColor.r * abs(sin(myTime*5.0));
    float b = destColor.r * abs(sin(myTime*3.0));
    float r = destColor.r * abs(cos(myTime*0.2));
    
    destColor = vec3(0.0);
    for(float i = 2.0; i < 10.0; i++){
        float j = i * i;
        float tt = myTime + 1.0;
        vec2 q = p + vec2(sin(tt * j)*length(cos(tt)), cos(tt * j)*length(sin(tt)));
        destColor += 0.02 * abs(atan(tt)) / length(q);
    }
    
    g += destColor.r * abs(sin(myTime*2.0));
    b += destColor.r * abs(sin(myTime*5.0));
    r += destColor.r * abs(cos(myTime*0.5));
    
    gl_FragColor = vec4(r, g, b, 1.0);

}
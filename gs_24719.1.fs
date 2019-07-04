/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#24719.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main( void ) {

    vec2 p = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
    p *= mat2(cos(TIME*1.0), sin(TIME*1.0), -sin(TIME*1.0), cos(TIME*1.0));
    
    vec3 destColor = vec3(0.0);
    for(float i = 2.0; i < 20.0; i++){
        float j = i * i;
        vec2 q = p + vec2(sin(TIME * j)*length(cos(TIME)), cos(TIME * j)*length(sin(TIME)));
        destColor += 0.01 * abs(atan(TIME)) / length(q);
    }
    float g = destColor.r * abs(sin(TIME*5.0));
    float b = destColor.r * abs(sin(TIME*3.0));
    float r = destColor.r * abs(cos(TIME*0.2));
    
    destColor = vec3(0.0);
    for(float i = 2.0; i < 20.0; i++){
        float j = i * i;
        float tt = TIME + sin(TIME)*2.;
        vec2 q = p + vec2(sin(tt * j)*length(cos(tt)), cos(tt * j)*length(sin(tt)));
        destColor += 0.01 * abs(atan(tt)) / length(q);
    }
    
    g += destColor.r * abs(sin(TIME*2.0));
    b += destColor.r * abs(sin(TIME*5.0));
    r += destColor.r * abs(cos(TIME*0.5));
    
    gl_FragColor = vec4(r, g, b, 1.0);

}
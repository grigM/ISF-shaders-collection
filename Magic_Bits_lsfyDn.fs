/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "distance",
    "rainbow",
    "magic",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/lsfyDn by rls.  My first real shader. This is less daunting than I was expecting.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    float ar = RENDERSIZE.x/RENDERSIZE.y;
    uv.x = uv.x*ar;
    vec2 m = iMouse.xy/RENDERSIZE.xy;
    float t = TIME;
    
    float speed = 10.0;
    float amp = 0.5;
    float sep = 0.7;
    
    float a = distance(uv,vec2(sin(t)/5.0+0.75,cos(t)/5.0+0.75))*3.0;
    float b = distance(uv,vec2(sin(t*1.1*-1.0)/5.0+0.25,cos(t*1.4*-1.0)/5.0+0.75))*3.0;
    float c = distance(uv,vec2(sin(t*1.2*-1.0)/5.0+0.75,cos(t*1.5*-1.0)/5.0+0.25))*3.0;
    float d = distance(uv,vec2(sin(t*1.3)/5.0+0.25,cos(t*1.6*-1.0)/5.0+0.25))*3.0;
    
    float comb = a*b*c*d;
    
    float red = sin(comb*(3.0)-t*speed)/2.0+0.5;
    float green = sin(comb*(3.0+sep)-t*speed)/2.0+0.5;
    float blue = sin(comb*(3.0+sep*2.0)-t*speed)/2.0+0.5;
    
    vec3 color = vec3(red-comb+amp,green-comb+amp,blue-comb+amp);
    
	gl_FragColor = vec4(color,1.0);
}

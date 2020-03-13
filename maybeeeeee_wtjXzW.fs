/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wtjXzW by lennyjpg.  eeeeeee",
  "INPUTS" : [

  ]
}
*/


# define PI 3.141592653589793
float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main() {



    vec2 uv = gl_FragCoord.xy/RENDERSIZE.y;
    vec2 u = fract(uv.xy*5.);
    vec2 s = fract(uv.xy*10.+0.5);
    u-=0.5;
    vec2 g = TIME*0.2+floor(uv*5.);
    float r = .7;
    float angle = PI*0.25+floor(rand(floor(g))*5.)*PI*.5;      
    u.x+=sin(angle)*r;
    u.y+=cos(angle)*r;
  
    float d = length(u);
 	float k = smoothstep(d,d*2.0,0.5);
    k =fract(d*angle/11.1 - TIME*sin(angle*123.4)*0.1);
    
    k -= step(length(s-0.5),0.02);
    k= floor(k+0.5);
    gl_FragColor = vec4(k);
}

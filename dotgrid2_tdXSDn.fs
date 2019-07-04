/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tdXSDn by lennyjpg.  asdfasdfasdfasdf",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


// Simplex 2D noise
//
vec3 permute(vec3 x) { return mod(((x*34.0)+1.0)*x, 289.0); }

float snoise(vec2 v){
  const vec4 C = vec4(0.211324865405187, 0.366025403784439,
           -0.577350269189626, 0.024390243902439);
  vec2 i  = floor(v + dot(v, C.yy) );
  vec2 x0 = v -   i + dot(i, C.xx);
  vec2 i1;
  i1 = (x0.x > x0.y) ? vec2(1.0, 0.0) : vec2(0.0, 1.0);
  vec4 x12 = x0.xyxy + C.xxzz;
  x12.xy -= i1;
  i = mod(i, 289.0);
  vec3 p = permute( permute( i.y + vec3(0.0, i1.y, 1.0 ))
  + i.x + vec3(0.0, i1.x, 1.0 ));
  vec3 m = max(0.5 - vec3(dot(x0,x0), dot(x12.xy,x12.xy),
    dot(x12.zw,x12.zw)), 0.0);
  m = m*m ;
  m = m*m ;
  vec3 x = 2.0 * fract(p * C.www) - 1.0;
  vec3 h = abs(x) - 0.5;
  vec3 ox = floor(x + 0.5);
  vec3 a0 = x - ox;
  m *= 1.79284291400159 - 0.85373472095314 * ( a0*a0 + h*h );
  vec3 g;
  g.x  = a0.x  * x0.x  + h.x  * x0.y;
  g.yz = a0.yz * x12.xz + h.yz * x12.yw;
  return 130.0 * dot(m, g);
}

void main() {



    vec2 uv = gl_FragCoord.xy/RENDERSIZE.x;
    vec2 p = gl_FragCoord.xy  + iMouse.xy + TIME ;    
    
    float dd = snoise(TIME*0.03+uv*.5)*3.0;
    float ee = snoise(TIME*0.03+uv*.5)*370.0;
    p.x+=sin(dd)*ee;
    p.y+=cos(dd)*ee;
    p.y -= TIME * 100.;
    p.x -= TIME * 100.;
    p.y+=sin(p.y*.1)*3.1;
    float s = 100.;
    
    vec2 c = vec2(sin(TIME*3.),cos(TIME*3.))*0.2+1.2;
    uv*=c;
    //p+=snoise(p*.11)*uv.y*100.0;
        
    float n = 1.0;//snoise(uv*7.0)*2.0;
    float t = 5. * TIME ;
    
    t += sin(uv.x*10.1);
    float k = cos(t)+1.;
   	vec2 g = mod(p,s)/s;
    float e = g.x*g.y;
    float v = 1.0;
    if( sin(t) > 0. ){
	    p.x -= k * step(s, mod(p.y, s * 2.)) * s;   
    }else{
        p.y -= k * step(s, mod(p.x, s * 2.)) * s;
    }
    
    float d = length(mod(p,s)-0.5*s);    
    v = uv.y * 1.2;
    float points = smoothstep(d,d*.9,1.0+70.*v);
    gl_FragColor = vec4(.2+points);  
}

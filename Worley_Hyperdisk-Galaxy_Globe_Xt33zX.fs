/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "noise",
    "worley",
    "intestine",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xt33zX by CaliCoastReplay.  Yeis' Worley noise and mattdamon's hyperbolic disc translation produced an unexpectedly poignant effect. ",
  "INPUTS" : [

  ]
}
*/



vec3 rgb2hsv(vec3 c)
{
    vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
    vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}


float length2(vec2 p){
    return dot(p,p);
}


float noise(vec2 n) {
    return abs(1.0 - 2.0 * fract(sin(cos(dot(n, vec2(1297.98,123.14)))) * 83758.5456));
}


float worley(vec2 p) {
    //create a huge number
 float d = 1e10;
    //check points in nine directions
 for (int xo = -1; xo <= 1; ++xo) 
 {
  for (int yo = -1; yo <= 1; ++yo) 
  {
   vec2 tp = floor(p) + vec2(xo, yo);
   d = min(d, length2(p - tp - noise(tp)));
  }
 }
    float r = 1.18-sin(sqrt(d));
      return r;
}

float worley_layered(vec2 n)
{
    
    return (worley(n)*2.0 + worley(n*2.0)*3.0 + worley(n*4.0) + worley(n*8.0))/7.0 + noise(n)/80.0;
}

float intensity(vec2 uv)
{
    vec2 uv2 = uv;
    uv.x += 0.001;
   float w = worley_layered(uv) +
       worley_layered(uv + TIME/8.0)/5.0;
    return w*w*w*w*sqrt(w)*0.81;
}

//hyperbolic disc/radial distortion adapted from https://www.shadertoy.com/view/XllSWf
void HyperbolicDisc( vec2 fragCoord) {
    fragCoord -= RENDERSIZE.xy * 0.5;
    fragCoord /= RENDERSIZE.x;
    float r = length(fragCoord);
    vec2 d = fragCoord / r *.8 ;
    fragCoord = d / atan(r * (2.5 )) / 2.0;
    fragCoord *= RENDERSIZE.x;
    fragCoord += RENDERSIZE.xy *0.5;
    fragCoord *= 0.4;
}

void main() {



    HyperbolicDisc(gl_FragCoord.xy);
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;   
    vec2 oldUV = uv;
    uv -= TIME/18.0;
    uv.y /= 1.7;
    float bi = intensity(1.2 * uv );
    bi *= bi + sin(TIME)/5.0 + .3;
    vec3 tint = vec3(0.25, 0.47, 0.97);
    vec3 color = tint * bi;
    vec3 lowrangeTint = vec3(0.98, 0.8, 0.7);
    if (bi < 0.3)
        color *= lowrangeTint * 0.97;
    vec3 hsv = rgb2hsv(color);
    hsv.z *= 1.2;
    hsv.z -= hsv.y/7.0;
    hsv.z += oldUV.x/3.5;
    hsv.z += 0.4;
    hsv.z *= hsv.z * hsv.z;
    hsv.z /= 3.7;
    color = hsv2rgb(hsv);
    
	gl_FragColor = ( vec4(color, 1.0) );
}

/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tl2XD1 by foran.  metal rain",
  "INPUTS" : [

  ]
}
*/



  const int octaves = 4;
  
  float sinnoise(vec3 loc){

      float t = TIME*0.2;
      vec3 p = loc;

      for (int i=0; i<octaves; i++){
          p += cos( p.xxz * 3. + vec3(0., t, 1.6)) / 3.;
          p += sin( p.yzz + t + vec3(t, 1.6, 0.)) / 2.;
          // p += sin( p.zyx + t * 2. + vec3(0,1.6,t)) / 2.;
          p *= 1.3;
      }

      p += fract(sin(p+vec3(13, 7, 3))*5e5)*.03-.015;

       return dot(p, p);
     // return length(p);

  }
void main() {



    vec2 uv = .92*(gl_FragCoord.xy - 0.5 * RENDERSIZE.xy) / min(RENDERSIZE.y, RENDERSIZE.x);
    
    float shade = sinnoise(vec3(uv * 5., 1.));
    shade = sin(shade) * .53 + .46;
    gl_FragColor = vec4(vec3(shade),1.0);
  }

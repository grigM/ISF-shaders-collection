/*{
	"CREDIT": "by mojovideotech",
  "CATEGORIES": [
    "noise"
  ],
  "DESCRIPTION": "",
  "INPUTS": [
         {
      		"MAX": [0.5,0.5],
      		"MIN": [0.25,0.25],
      		"NAME": "offset",
      		"DEFAULT":[0.33,0.33],
      		"TYPE": "point2D"
         },
         {
            "NAME": "color1",
            "TYPE": "float",
           "DEFAULT": 0.55,
            "MIN": 0.01,
            "MAX": 2.0
          },
           {
            "NAME": "color2",
            "TYPE": "float",
           "DEFAULT": 0.66,
            "MIN": 0.01,
            "MAX": 2.0
           },
            {
            "NAME": "color3",
            "TYPE": "float",
           "DEFAULT": 0.77,
            "MIN": 0.01,
            "MAX": 2.0
          },
            {
            "NAME": "detail",
            "TYPE": "float",
           "DEFAULT": 0.07,
            "MIN": 0.0,
            "MAX": 1.0
          },
            {
            "NAME": "brightness",
            "TYPE": "float",
           "DEFAULT": 0.53,
            "MIN": 0.0,
            "MAX": 1.0
          },
           {
            "NAME": "scale",
            "TYPE": "float",
           "DEFAULT": 0.0025,
            "MIN": 0.00125,
            "MAX": 0.01
          },
           {
            "NAME": "phase1",
            "TYPE": "float",
           "DEFAULT": 0.88,
            "MIN": 0.01,
            "MAX": 0.99
          },
          {
            "NAME": "phase2",
            "TYPE": "float",
           "DEFAULT": 0.46,
            "MIN": -0.5,
            "MAX": 0.5
          },
           {
            "NAME": "rate",
            "TYPE": "float",
           "DEFAULT": -0.6,
            "MIN": -10,
            "MAX": 10
          }
  ]
}
*/


// MakeMyNoiseTheP-Flow2 by mojovideotech
// based on :
// http://glslsandbox.com/e#3155.0
// GLSL implementation of 2D "flow noise" as presented
// by Ken Perlin and Fabrice Neyret at Siggraph 2001.
// Author: Stefan Gustavson (stefan.gustavson@liu.se)
// Distributed under the terms of the MIT license.
// changes by George Toledo, 2011 and 2012.

#ifdef GL_ES
precision mediump float;
#endif

#define F2 0.3183098861
#define G2 0.1784575679
#define K 0.028876065 
#define TPI 6.28318530718

float permute(float x) {
  return mod((34.0 * x + 1.0)*x, 289.0);
}

vec2 grad2(vec2 p, float rot) {
  float u = permute(permute(p.x) + p.y) * K + rot;
 //   u = fract(u) * TPI;
//  return vec2(cos(u-phase1), sin(u+phase2));
    u = 4.0 * fract(u) - 2.0;
  return vec2(abs(u)-phase1, abs(abs(u+phase2)-2.0)-1.0);

}

float srdnoise(in vec2 P, in float rot, out vec2 grad) {
  vec2 Ps = P + dot(P, vec2(F2));
  vec2 Pi = floor(Ps);
  vec2 P0 = Pi - dot(Pi, vec2(G2));
  vec2 v0 = P - P0;
  vec2 i1 = (v0.x > v0.y) ? vec2(1.0, 0.0) : vec2 (0.0, 1.0);
  vec2 v1 = v0 - i1 + G2;
  vec2 v2 = v0 - 1.0 + 2.0 * G2;
   Pi = mod(Pi, 289.0);
  float ta,tb,tc;
  ta = dot(v0,v0);
  tb = dot(v1,v1);
  tc = dot(v2,v2);
  vec3 t = vec3(ta,tb,tc);
  t = vec3(max(0.5-t,0.0));
  vec3 t2 = t*t;
  vec3 t4 = t2*t2;
  vec2 g0 = grad2(Pi, rot);
  vec2 g1 = grad2(Pi + i1, rot);
  vec2 g2 = grad2(Pi + 1.0, rot);
  vec3 gv = vec3(dot(g0,v0), dot(g1,v1), dot(g2,v2));
  vec3 n = t4 * gv; 
  vec3 temp = t2 * t * gv;
  vec3 gradx = temp * vec3(v0.x, v1.x, v2.x);
  vec3 grady = temp * vec3(v0.y, v1.y, v2.y);
  grad.x = -8.0 * (gradx.x + gradx.y + gradx.z);
  grad.y = -8.0 * (grady.x + grady.y + grady.z);
  grad.x += dot(t4, vec3(g0.x, g1.x, g2.x));
  grad.y += dot(t4, vec3(g0.y, g1.y, g2.y));
  grad *= 40.0;
  return 40.0 * (n.x + n.y + n.z);
}

vec3 hue(vec3 hue) {
	return clamp(abs(mod(hue,vec3(color2,color3,color1))),0.01,1.0);
//	return clamp(abs(mod(hue * color3 + vec3(0.0,0.0,0.0),color1) - color2) - 1.0,0.0,1.0);
}


void main(void) {
  	vec2 p, g1, g2;
	 p.x = ( gl_FragCoord.y - (RENDERSIZE.x*offset.x))*scale;
	 p.y = ( gl_FragCoord.x - (RENDERSIZE.y*offset.y))*scale;
	float T = TIME * rate;
  	float n1 = srdnoise(p*0.5, 0.2*T, g1);
  	float n2 = srdnoise(p*2.0 + g1*0.5, 0.51*T, g2);
  	float n3 = srdnoise(p*4.0 + g1*0.5 + g2*0.25, 0.77*T, g2);
  	vec3 col = vec3(color1,color2,color3) + vec3(n1+0.95*n2+0.75*n3);
  gl_FragColor = vec4(mix(vec3(hue(col)),col/2.0,1.0-detail),1.0);	
  gl_FragColor *= vec4((vec3(0.0),0.75+brightness/3.0));	
}
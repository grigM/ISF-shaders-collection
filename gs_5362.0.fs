/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#5362.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


// NEBULA - CoffeeBreakStudios.com
// Work in progress...
// mod: Switched from classic to simplex noise
// mod: Reduced number of stars

//
// Description : Array and textureless GLSL 2D/3D/4D simplex 
//               noise functions.
//      Author : Ian McEwan, Ashima Arts.
//  Maintainer : ijm
//     Lastmod : 20110822 (ijm)
//     License : Copyright (C) 2011 Ashima Arts. All rights reserved.
//               Distributed under the MIT License. See LICENSE file.
//               https://github.com/ashima/webgl-noise
//

float phi = 2.0/(1.0+sqrt(5.0));
float vx_offset = 0.54;
float rt_w = 0.5; // GeeXLab built-in
float rt_h = 0.5; // GeeXLab built-in
float hatch_y_offset = 1.; // 5.0
float lum_threshold_1 = 1.0; // 1.0
float lum_threshold_2 = 0.7; // 0.7
float lum_threshold_3 = 0.5; // 0.5
float lum_threshold_4 = 0.3; // 0.3
vec3 mod289(vec3 x)
{
  return x - floor(x * (1.0 / 289.0)) * 289.0;
}

vec4 mod289(vec4 x)
{
  return x - floor(x  *(1.0 / 289.0)) * 289.0;
}

vec4 permute(vec4 x)
{
  return mod289(((x *34.0)+1.0)*x);
}

vec4 taylorInvSqrt(vec4 r)
{
  return 1.79284291400159 - 0.85373472095314 * r * phi;
}

vec3 fade(vec3 t) {
  return t*t*t*(t*(t*6.0-15.0)+10.0);
}

float rand(float x) {
	float res = 0.0;
	
	for (int i = 0; i < 5; i++) {
		res += 0.244 * float(i) * sin(x * 0.68171 * float(i));
		
	}
	return res;
	
}

// Simplex Perlin noise
float snoise(vec3 v)
  { 
  const vec2  C = vec2(1.0/6.0, 1.0/3.0) ;
  const vec4  D = vec4(0.0, 0.5, 1.0, 2.0);

// First corner
  vec3 i  = floor(v + dot(v, C.yyy) );
  vec3 x0 =   v - i + dot(i, C.xxx) ;

// Other corners
  vec3 g = step(x0.yzx, x0.xyz);
  vec3 l = 1.0 - g;
  vec3 i1 = min( g.xyz, l.zxy );
  vec3 i2 = max( g.xyz, l.zxy );

  //   x0 = x0 - 0.0 + 0.0 * C.xxx;
  //   x1 = x0 - i1  + 1.0 * C.xxx;
  //   x2 = x0 - i2  + 2.0 * C.xxx;
  //   x3 = x0 - 1.0 + 3.0 * C.xxx;
  vec3 x1 = x0 - i1 + C.xxx;
  vec3 x2 = x0 - i2 + C.yyy; // 2.0*C.x = 1/3 = C.y
  vec3 x3 = x0 - D.yyy;      // -1.0+3.0*C.x = -0.5 = -D.y

// Permutations
  i = mod289(i); 
  vec4 p = permute( permute( permute( 
             i.z + vec4(0.0, i1.z, i2.z, 1.0 ))
           + i.y + vec4(0.0, i1.y, i2.y, 1.0 )) 
           + i.x + vec4(0.0, i1.x, i2.x, 1.0 ));

// Gradients: 7x7 points over a square, mapped onto an octahedron.
// The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
  float n_ = 0.142857142857; // 1.0/7.0
  vec3  ns = n_ * D.wyz - D.xzx;

  vec4 j = p - 49.0 * floor(p * ns.z * ns.z);  //  mod(p,7*7)

  vec4 x_ = floor(j * ns.z);
  vec4 y_ = floor(j - 7.0 * x_ );    // mod(j,N)

  vec4 x = x_ *ns.x + ns.yyyy;
  vec4 y = y_ *ns.x + ns.yyyy;
  vec4 h = 1.0 - abs(x) - abs(y);

  vec4 b0 = vec4( x.xy, y.xy );
  vec4 b1 = vec4( x.zw, y.zw );

  //vec4 s0 = vec4(lessThan(b0,0.0))*2.0 - 1.0;
  //vec4 s1 = vec4(lessThan(b1,0.0))*2.0 - 1.0;
  vec4 s0 = floor(b0)*2.0 + 1.0;
  vec4 s1 = floor(b1)*2.0 + 1.0;
  vec4 sh = -step(h, vec4(0.0));

  vec4 a0 = b0.xzyw + s0.xzyw*sh.xxyy ;
  vec4 a1 = b1.xzyw + s1.xzyw*sh.zzww ;

  vec3 p0 = vec3(a0.xy,h.x);
  vec3 p1 = vec3(a0.zw,h.y);
  vec3 p2 = vec3(a1.xy,h.z);
  vec3 p3 = vec3(a1.zw,h.w);

//Normalise gradients
  vec4 norm = taylorInvSqrt(vec4(dot(p0,p0), dot(p1,p1), dot(p2, p2), dot(p3,p3)));
  p0 *= norm.x;
  p1 *= norm.y;
  p2 *= norm.z;
  p3 *= norm.w;

// Mix final noise value
  vec4 m = max(0.6 - vec4(dot(x0,x0), dot(x1,x1), dot(x2,x2), dot(x3,x3)), 0.0);
  m = m * m;
  return 42.0 * dot( m*m, vec4( dot(p0,x0), dot(p1,x1), 
                                dot(p2,x2), dot(p3,x3) ) );
  }

float surface3 ( vec3 coord, float frequency ) {
	
	float n = 0.0;	
		
	n += 1.0	* abs( snoise( coord * frequency ) );
	n += 0.5	* abs( snoise( coord * frequency * 2.0 ) );
	n += 0.25	* abs( snoise( coord * frequency * 4.0 ) );
	n += 0.125	* abs( snoise( coord * frequency * 8.0 ) );
	n += 0.0625	* abs( snoise( coord * frequency * 16.0 ) );
	n += 0.03125    	* abs( snoise( coord * frequency * 32.0 ) );
	
	return n;
}
	

vec2 complex_mul(vec2 factorA, vec2 factorB){
   return vec2( factorA.x*factorB.x - factorA.y*factorB.y, factorA.x*factorB.y + factorA.y*factorB.x);
}

vec2 torus_mirror(vec2 uv){
	return vec2(1.)-abs(fract(uv*.5)*2.-1.);
}

float sigmoid(float x) {
	return 2./(1. + exp2(-x)) - 1.;
}

float smoothcircle(vec2 uv, float radius, float sharpness){
	return 0.5 - sigmoid( ( length( (uv - 0.5)) - radius) * sharpness) * 0.5;
}


 

void main() {

 

	vec2 posScale = vec2(2.0);

	

	vec2 aspect = vec2(1.,RENDERSIZE.y/RENDERSIZE.x);

	vec2 uv = 0.5 + (gl_FragCoord.xy * vec2(1./RENDERSIZE.x,1./RENDERSIZE.y) - 0.5)*aspect;

	float mouseW = atan((mouse.y - 0.5)*aspect.y, (mouse.x - 0.5)*aspect.x);

	vec2 mousePolar = vec2(0.,1.); vec2(sin(mouseW), cos(mouseW));

	vec2 offset = (mouse - 0.5)*0.;

	offset =  - complex_mul(offset, mousePolar) +TIME*0.0;

	vec2 uv_distorted = uv;

	

	float filter = smoothcircle( uv_distorted, 0.15, 48.);

	uv_distorted = complex_mul(((uv_distorted - 0.5)*mix(2., 12., filter)), mousePolar) + offset;

	

	vec2 position = uv_distorted;//gl_FragCoord.xy / RENDERSIZE.xy;

	

	float n = surface3(vec3(position*sin(TIME*0.1), TIME * 0.05)*mat3(1,0,0,0,.8,.6,0,-.6,.8),.9);

	float n2 = surface3(vec3(position*cos(TIME*0.1), TIME * 0.03)*mat3(1,0,0,0,.8,.6,0,-.6,.8),.5);

    	float lum = length(n);

    	float lum2 = length(n2);

 

	vec3 tc = pow(vec3(1.0-lum),vec3(rand(35.0)+cos(TIME)+3.0,rand(12.0)+sin(TIME)+4.0,8.));

	vec3 tc2 = pow(vec3(1.1-lum2),vec3(5,rand(position.y)+cos(TIME)+7.,rand(position.x)+sin(TIME)+2.0));

	vec3 curr_color = (tc*0.5) + (tc2*0.3);

	

	

	//Let's draw some stars

	

	float scale = sin(0.3 * TIME) + 5.0;

 

	vec2 position2 = ((position ) * scale);

 

	float gradient = 0.0;

	vec3 color = vec3(0.0);

 

	float fade = 0.0;

	float z;

 

	vec2 centered_coord = position2 - vec2(sin(TIME*0.1),sin(TIME*0.1));

	for (float i=1.0; i<=60.0; i++)

	{

		vec2 star_pos = vec2(sin(i) * 250.0, sin(i*i*i) * 250.0);

		float z = mod(i*i - 10.0*TIME, 256.0);

		float fade = (256.0 - z) /256.0;

		vec2 blob_coord = star_pos / z;

		gradient += ((fade / 384.0) / pow(length(centered_coord - blob_coord), 1.5)) * ( fade);

	}

 

	curr_color = curr_color + gradient;

	

	gl_FragColor = vec4(curr_color.x,curr_color.y, curr_color.z, 1.0);

}

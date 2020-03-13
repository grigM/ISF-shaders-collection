/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wdKSWw by KilledByAPixel.  Evolved using zzart.3d2k.com",
  "INPUTS" : [

  ]
}
*/


// ZzArt - Generation: 11 (133004417)

const float PI=3.141592653589793;
vec3 CosinePalette( float t, vec3 a, vec3 b, vec3 c, vec3 d ) { return a + b*cos( PI*2.*(c*t+d)); }
vec4 lengthA(vec4 a)      { return vec4(length(a)); }
vec4 asinA(vec4 a)        { return asin(clamp(a,-1.,1.)); }
vec4 acosA(vec4 a)        { return acos(clamp(a,-1.,1.)); }
vec4 logA(vec4 a)         { return log(abs(a)); }
vec4 log2A(vec4 a)        { return log2(abs(a)); }
vec4 sqrtA(vec4 a)        { return sqrt(abs(a)); }
vec4 inversesqrtA(vec4 a) { return inversesqrt(abs(a)); }
vec4 pow2(vec4 a)         { return a*a; }
vec4 pow3(vec4 a)         { return a*a*a; }

void main() {



	gl_FragColor=gl_FragCoord.yxyx/RENDERSIZE.yxyx;
	gl_FragColor+=vec2(0,-.1).yxyx;
	gl_FragColor.xy *= vec2(-6.961, 2.807);
	gl_FragColor.xy += vec2(-9.299, 2.808);
	gl_FragColor.wz *= vec2(-6.961, 2.807);
	gl_FragColor.wz += vec2(-9.299, 2.808);
	vec4 b = gl_FragColor;
    
	// Generated Code - Line Count: 11
	for (int i = 0; i < 2; ++i)
	{
		b.xwyz = (vec4(9.128, -0.496, -0.044, 9.615)).yzwz;
		gl_FragColor.xywz -= cos(gl_FragColor+TIME).xzzw;
		gl_FragColor.zwxy *= (gl_FragColor).zxyx;
		b.yzwx += (vec4(-1.740, -3.156, 4.371, 2.672)).zxzy;
		b.yxwz = abs(b).wzxz;
		gl_FragColor.zwxy /= (b).yzwy;
		gl_FragColor.ywxz -= (b).wxzw;
		gl_FragColor.yxzw -= log2A(gl_FragColor).zzxw;
	}

	// Cosine palettes by iq
	gl_FragColor.x = gl_FragColor.x * -0.018+0.063;
	gl_FragColor.xyz = b.x * CosinePalette(gl_FragColor.x,
 	vec3(0.410, 0.045, 0.042),
 	vec3(0.960, 0.279, 0.187),
 	vec3(0.742, 0.722, 0.604),
 	vec3(0.949, 0.047, 0.964));
 	
 	gl_FragColor = vec4(vec3(gl_FragColor),1.0);
}

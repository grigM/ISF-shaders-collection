/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#49809.3",
  "INPUTS" : [
    {
      "NAME" : "camRotationSpeed",
      "TYPE" : "float",
      "MAX" : 0.59999999999999998,
      "DEFAULT" : 0.10000000000000001,
      "MIN" : -0.59999999999999998
    },
    {
      "NAME" : "camRotationOffset",
      "TYPE" : "float",
      "MAX" : 5,
      "DEFAULT" : 0,
      "MIN" : 0
    },
    {
      "NAME" : "linesMoveSpeed",
      "TYPE" : "float",
      "MAX" : 3,
      "DEFAULT" : 0.59999999999999998,
      "MIN" : 0
    },
    {
      "NAME" : "linesMoveOffset",
      "TYPE" : "float",
      "MAX" : 5,
      "DEFAULT" : 0,
      "MIN" : 0
    },
    {
      "NAME" : "sliceParam",
      "TYPE" : "float",
      "MAX" : 2000,
      "DEFAULT" : 1000,
      "MIN" : 50
    },
    {
      "NAME" : "itemLenth",
      "TYPE" : "float",
      "MAX" : 100,
      "DEFAULT" : 98,
      "MIN" : 50.5
    },
    {
      "NAME" : "hueParam",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 0.59999999999999998,
      "MIN" : 0
    }
  ],
  "ISFVSN" : "2"
}
*/


#ifdef GL_ES
precision highp float;
#endif

// Starfields will never die by WAHa.06x36^SVatG


vec3 hsv(float h, float s, float v) {
	vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	vec3 p = abs(fract(vec3(h) + K.xyz) * 6.0 - K.www);
	return v * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), s);
}

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main(void) {
	vec2 p = (2.0 * gl_FragCoord.xy - RENDERSIZE.xy) / min(RENDERSIZE.x, RENDERSIZE.y);
	vec3 v = vec3(p, 1.0 - length(p) * 0.2);

	float ta = ((TIME) * camRotationSpeed)-camRotationOffset;
	mat3 m=mat3(
		0.0,1.0,0.0,
		-sin(ta),0.0,cos(ta),
		cos(ta),0.0,sin(ta));
	m*=m*m;
	m*=m;
	v=m*v;

	float a = (atan(v.y, v.x) / 3.141592 / 2.0 + 0.5);
	float slice = floor(a * sliceParam);
	float phase = rand(vec2(slice, 0.0));
	float dist = rand(vec2(slice, 1.0)) * 3.0;
	float hue = rand(vec2(slice, 2.0));

	float z = dist / length(v.xy) * v.z;
	float Z = mod(z + phase + ((TIME) * (linesMoveSpeed)-linesMoveOffset), 1.0);
	float d = sqrt(z * z + dist * dist);

	float c = exp(-Z * (102.0-itemLenth) + 0.3) / (d * d + 1.0);
	gl_FragColor = vec4(hsv(hue, hueParam * (1.0 - clamp(2.0 * c - 1.0, 0.0, 1.0)), clamp(2.0 * c, 0.0, 1.0)), 1.0);
}
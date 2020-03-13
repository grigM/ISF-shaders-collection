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
    },
    {
            "NAME": "size",
            "TYPE": "float",
           "DEFAULT": 1.00,
            "MIN": 0.0,
            "MAX": 2.0
          },
          {
            "NAME": "sub_size",
            "TYPE": "float",
           "DEFAULT": 1.00,
            "MIN": 0.0,
            "MAX": 2.0
          },

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60646.4"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec3 getSphereNormal(vec2 pos) {
	// gotta find the z such that pos.x^2 + pos.y^2 + z^2 = 1
	return normalize(vec3(pos, sqrt(sub_size-pos.x*pos.x - pos.y * pos.y)));
}

vec4 normalColor(vec3 normal) {
	return vec4(normal / 2.0 + vec3(0.5), 1.0);
}

vec3 spec(vec3 L, vec3 N, vec3 V, float E) {
	vec3 T = vec3(1.0, 1.0, 0.0);
	T = normalize(T - dot(N, T) * N);
	//return T;
	return vec3(pow(clamp(dot(N, normalize(L + V)), 0.0, 1.0), E*pow(dot(T, L), 2.0)));
}

vec3 diff(vec3 L, vec3 N) {
	return vec3(clamp(dot(L, N), 0.0, 1.0));
}

void main( void ) {

	vec2 position = (( gl_FragCoord.xy / RENDERSIZE.xy * 2.0 ) - vec2(1.0)) * vec2(RENDERSIZE.x / RENDERSIZE.y, 1.0)/size;
	vec3 normal = getSphereNormal(position);
	vec3 light = getSphereNormal((mouse.xy - vec2(0.5)) * 2.0);
	vec3 view = vec3(0.0, 0.0, 1.0);
	
	vec3 color;

	// ambient
	//color += vec3(0.01, 0.01, 0.01);
	
	// diffuse
	color += vec3(0.4, 0.8, 0.9) * diff(light, normal) / 2.0;
	
	// specular
	color += 1.0 * spec(light, normal, view, 1000.0) / 2.0;
	
	//mask
	//color *= smoothstep(1.0, 0.95, dot(position, position));

	gl_FragColor = vec4(color, 1.0);

}
/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "phong",
    "sphere",
    "blinn",
    "blinnphong",
    "shaderequation",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Mslyz2 by cmalessa.  This is a simple implementation of the modified Blinn-Phong Shading model (as presented in Real-Time Rendering)  applied to a sphere with multiple light sources.",
  "INPUTS" : [

  ]
}
*/


#define PI 3.1415926535897932384626433832795
#define t TIME * 0.5

vec3 shade(float r, vec2 center, vec2 pos, vec3 l[2])
{
    float z = sqrt(r * r - pos.x * pos.x - pos.y * pos.y);
    vec3 n = normalize(vec3(pos.x, pos.y, z)); 		// Surface normal

    vec3 c_diff = vec3(0.9, 0.0, 0.7);				// Diffuse color    
    vec3 c_spec = vec3(0.0, 1.0, 1.0);				// Specular color

    float m = 11.0; 								// Surface Smoothness
    float El = 0.9; 								// Irradiance of light source
    vec3 Kd = c_diff.xyz / PI; 						// Diffuse term
    vec3 Ks = c_spec.xyz * ((m + 8.0) / (8.0 * PI));// Specular term

    vec3 Lo = vec3(0.0);
    for (int i = 0; i < 2; i++)
    {
        vec3 h = normalize(l[i] + n); // Half vector

        float cosTi = max(dot(n, l[i]), 0.0);
        float cosTh = max(dot(n, h), 0.0);
        Lo += (Kd.xyz + Ks.xyz * pow(cosTh, m)) * El * cosTi; // Outgoing radiance
    }  

    return Lo; 
    
}


void main() {



	//	Sphere Definition
	vec2 center = RENDERSIZE.xy / 2.0;
	float r = RENDERSIZE.y / 3.0;
	vec2 pos = gl_FragCoord.xy - center;
	//	Light vector
	vec3 l[2];
    l[0] = normalize(vec3(sin(t), sin(t), cos(t)));
    l[1] = normalize(vec3(-sin(t), cos(t), sin(t)));
    // Outgoing radiance
	vec3 Lo = shade(r, center, pos, l);
	gl_FragColor = vec4(Lo, 1.0);
}

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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#50520.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define iMouse (mouse*RENDERSIZE.xy)
#define iResolution RENDERSIZE
#define iTime TIME

//------------------------------------------------------
// scaffold morph, https://www.shadertoy.com/view/MlyfRy
// original by anneka
// slightly modified by I.G.P.
//------------------------------------------------------

#define repeat(p,r) (mod(p,r)-r/2.)
#define PI 3.14159265358979323846

float rand(vec2 c){
  return fract(sin(dot(c.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

vec3 eye() {
  return vec3(.2, .5, 1.);
}

mat2 rotation(float angle) {
  float c = cos(angle), s = sin(angle);
  return mat2(c, s, -s, c);
}

float sdBox( vec3 p, vec3 b )
{
  vec3 d = abs(p) - b;
  return length(max(d,0.0))
         + min(max(d.x,max(d.y,d.z)),0.0); // remove this line for an only partially signed sdf
}

float sdSphere (vec3 p, float r) {
  return length(p) - r;
}

float sdTorus( vec3 p, vec2 t ) {
  vec2 q = vec2(length(p.xz)-t.x,p.y);
  return length(q)-t.y;
}

float subtract(float a, float b) {
  return max(-a, b);
}

float wireframe_cube(vec3 p, float size, float weight) {
  float cut_size = size - weight;
  float long_size = size + 1.;
  float base = sdBox(p, vec3(size));
  float cut1 = sdBox(p, vec3(long_size, cut_size, cut_size));
  float cut2 = sdBox(p, vec3(cut_size, long_size, cut_size));
  float cut3 = sdBox(p, vec3(cut_size, cut_size, long_size));
  return subtract(cut1, subtract(cut2, subtract(cut3, base)));
}

vec3 mColor;

float sdf(vec3 pos) {
    float scene = 100.;
	vec3 eye = eye();
	float lerp = clamp((sin(iTime*.5)*.9)+.5, 0., 1.);
	vec3 p1 = pos;
	vec3 p2 = pos;
	vec3 p3 = pos;

	mat2 rot1 = rotation(iTime*.21 + iMouse.x/iResolution.y);
	mat2 rot2 = rotation(iTime*.21 + PI*.25);
	mat2 rot3 = rotation(iTime*.31 + PI*.5);

	p1.yz *= rot1;
	p1.xy *= rot1;
	p1.xz *= rot1;

	p2.yz *= rot2;
	p2.xy *= rot2;
	p2.xz *= rot2;

	p3.yz *= rot3;
	p3.xy *= rot3;
	p3.xz *= rot3;

	float torus1 = sdTorus(p1, vec2(.4, .05));
	float torus2 = sdTorus(p2, vec2(.4, .05));
	float torus3 = sdTorus(p3, vec2(.4, .05));

	float tori = min(min(torus1, torus2), torus3);

	float wireframes = min(wireframe_cube(p1, .3, .05)
                          ,wireframe_cube(p2, .3, .05));

	scene = mix(wireframes, tori, lerp);
	mColor = mix(vec3(1,1,0.5), vec3(0.5,1,1), lerp);
    
	scene = max(scene, -sdSphere(eye-pos, .5));
    return scene;
}

vec3 lookAt (vec3 from, vec3 target, vec2 uv) {
    vec3 forward = normalize(target - from);
    vec3 right = normalize(cross(forward, vec3(0,1,0)));
    vec3 up = normalize(cross(forward, right));
    return normalize(forward * .5 + uv.x * right + uv.y * up);
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec2 uv = fragCoord/iResolution.xy;
    
    uv = (uv-.5) / 2.;
    uv.x *= iResolution.x/iResolution.y;

    vec4 color = vec4(0.);

    float shade = 0.;
    float distTotal = 0.;

    vec3 eye = eye();
    vec3 target = vec3(0);
    vec3 ray = lookAt(eye, target, uv);
    const float count = 50.;

    for (float i = count; i > 0.; --i) {

        float dist = sdf(eye);

        if (dist < .0001) {
            shade = i/count;
            break;
        }
        eye += ray * dist;
        distTotal += dist;
    }
    color = vec4(mColor * vec3(pow(shade, 2.)), 1.);
    color.rgb += rand(uv+vec2(iTime*.001))*.15;
    
    fragColor = color;
}

void main( void ) 
{
    mainImage(gl_FragColor, gl_FragCoord.xy );
}
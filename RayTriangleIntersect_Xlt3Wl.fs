/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "3d",
    "raycasting",
    "triangle",
    "intersection",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xlt3Wl by albertelwin.  Practicing ray-triangle intersection :)",
  "INPUTS" : [
	{
            "NAME": "speed",
            "TYPE": "float",
           "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 10.0
          }
  ]
}
*/



#define SUPERSAMPLING 1

#define TAU 6.28318530717958647692
#define TO_RAD 0.01745329251
#define GAMMA 2.2
#define MAX_DIST 10.0

float dither(vec2 xy, float t) {
    float s = (xy.x * 12.9898) + (xy.y * 78.2330);
    float r = fract(sin((t * 12.9898) + s) * 43758.5453) + fract(sin((t * 78.2330) + s) * 43758.5453);
    return (r * 0.00392156886) - 0.00196078443;
}

void main(){
    float fov = 90.0;
    float tan_fov = tan(fov * 0.5 * TO_RAD);
    vec2 image_plane = vec2(tan_fov, tan_fov * (RENDERSIZE.y / RENDERSIZE.x));

#define MAX_SAMPLE_COUNT 8
    vec2 sample_offsets[MAX_SAMPLE_COUNT];
    //TODO: Should probably pick some better sample points :/
    sample_offsets[0] = vec2(0.378246, 0.575671);
    sample_offsets[1] = vec2(0.899594, 0.448347);
    sample_offsets[2] = vec2(0.933531, 0.504929);
    sample_offsets[3] = vec2(0.945769, 0.950468);
    sample_offsets[4] = vec2(0.077456, 0.405469);
    sample_offsets[5] = vec2(0.397382, 0.684774);
    sample_offsets[6] = vec2(0.960479, 0.437971);
    sample_offsets[7] = vec2(0.436872, 0.610370);

#if SUPERSAMPLING
    #define SAMPLE_COUNT MAX_SAMPLE_COUNT
#else
    #define SAMPLE_COUNT 1
#endif

    float a = TIME * speed + 2.9;
    
    float cos_a = cos(a);
    float sin_a = sin(a);

#if 1
    //NOTE: Just baking the y-rotation into the vertices :)
    vec3 A = vec3(-cos_a - sin_a, 1.0, sin_a - cos_a) * 0.8;
    vec3 B = vec3( sin_a - cos_a,-1.0, sin_a + cos_a) * 0.8;
    vec3 C = vec3( cos_a + sin_a, 1.0, cos_a - sin_a) * 0.8;
    vec3 D = vec3( cos_a - sin_a,-1.0,-sin_a - cos_a) * 0.8;

#define VERTEX_COUNT 12
    vec3 v[VERTEX_COUNT];
    v[ 0] = A; v[ 1] = B; v[ 2] = C;
    v[ 3] = C; v[ 4] = B; v[ 5] = D;
    v[ 6] = D; v[ 7] = A; v[ 8] = C;
    v[ 9] = A; v[10] = D; v[11] = B;
#else
#define VERTEX_COUNT 3
    vec3 v[VERTEX_COUNT];
    v[0] = vec3( 0.0,-1.0, 0.0);
    v[1] = vec3(-0.866 * cos_a, 0.5, 0.866 * sin_a);
    v[2] = vec3( 0.866 * cos_a, 0.5,-0.866 * sin_a);
#endif

    vec3 color = vec3(0.0);
	float alph = 0.0;
    for(int sample_index = 0; sample_index < SAMPLE_COUNT; sample_index++) {
        vec2 sample_pos = gl_FragCoord.xy + sample_offsets[sample_index];
        vec3 image_point = vec3((2.0 * sample_pos / RENDERSIZE.xy - 1.0) * image_plane, -2.0);

        vec3 ro = vec3(0.0, 0.0, -3.0);
        vec3 rd = normalize(image_point - ro);

        for(int i = 0; i < VERTEX_COUNT; i += 3) {
            vec3 v0 = v[i + 0];
            vec3 v1 = v[i + 1];
            vec3 v2 = v[i + 2];

            vec3 e0 = v1 - v0;
            vec3 e1 = v2 - v0;

            vec3 n = cross(e0, e1);
            float d2 = dot(n, n);
            vec3 m = n * (1.0 / d2);
            n = m * sqrt(d2);

          	float dot_nd = dot(n, rd);
            float t = dot(v0 - ro, n) / dot_nd;
            vec3 p = ro + t * rd;
            
            vec3 b;
            b.y = dot(cross(p - v2, e1), m);
            b.z = dot(cross(e0, p - v0), m);
            b.x = 1.0 - (b.y + b.z);

            if(b.x >= 0.0 && b.y >= 0.0 && b.z >= 0.0) {
                color += b;
            }
            
            
        }
    }

    color *= (1.0 / float(SAMPLE_COUNT));
    //color += dither(gl_FragCoord.xy, fract(TIME*speed));
    color = pow(color, vec3(1.0 / GAMMA));
	if(color.x != 0. && color.y != 0. && color.z != 0.){
		alph = 1.0;
	}
    gl_FragColor = vec4(color, alph);
}
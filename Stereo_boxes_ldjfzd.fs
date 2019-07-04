/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ldjfzd by danb.  I saw a gif and just wanted to make a shader of it. Code is far from being nice or efficient, but i don't care... ^_^\n[url]http:\/\/gph.is\/2vGZBdt[\/url]",
  "INPUTS" : [

  ]
}
*/


const int MAX_ITER = 1000;
const float MAX_DIST = 20.0;
const float EPSILON = 0.001;

const float pi = 3.14159;

float box(vec3 pos, vec3 size)
{
    return length(max(abs(pos) - size, 0.0));
}

mat2 rot(float phi)
{
    return mat2(cos(phi), -sin(phi),
                sin(phi),  cos(phi));
}

float wave_model(float phi)
{
    phi = mod(phi, 4.0 * pi);
    
    return   phi <= pi / 2. ? phi
           : phi <= 2. * pi ? pi / 2.
           : phi <= 5.0 * pi / 2. ? pi / 2. - (phi - 2. * pi)
           : 0.;
}

#define W(dt) smoothstep(0.0, 1.0, wave_model(t + dt)) * cube_dist_param + 0.2

float cube_size = 0.1;
float cube_dist_param = 0.21;
float scale = 0.75;
float distfunc(vec3 pos)
{
	float t = TIME * 2.0;
    
    float last_dist = 10000.0;
    for (float i = -1.0; i < 2.0; i += 1.0)
    {
        for (float j = -1.0; j < 2.0; j += 1.0)
    	{
            for (float k = -1.0; k < 2.0; k += 1.0)
    		{
                float a = W(pi / 2.0);
                float b = W(pi);
                float c = W(0.0);
                vec3 rotpos = pos;
                rotpos.yz *= rot(-pi / 5.0);
                rotpos.xz *= rot(pi / 4.0);
        		float next_dist = box(rotpos + vec3(i * a, j * b, k * c), vec3(cube_size)) * scale;
        		last_dist = min(last_dist, next_dist);
            }
        }
    }
    
    return last_dist;
}

void main() {



	float t = TIME;
    
    vec3 color = vec3(0.1484375, 0.140625, 0.15234375);
    
    // screenPos can range from -1 to 1
    vec2 s_pos =  (2.0 * gl_FragCoord.xy - RENDERSIZE.xy)  / RENDERSIZE.y;
    
    // up vector
    vec3 up = vec3(0.0, 1.0, 0.0);
    
    // camera position
	vec3 c_pos = vec3(0.0, 0.0, 8.0);
    // camera target
    vec3 c_targ = vec3(0.0, 0.0, 0.0);
    // camera direction
    vec3 c_dir = normalize(c_targ - c_pos);
    // camera right
    vec3 c_right = cross(c_dir, up);
    // camera up
    vec3 c_up = cross(c_right, c_dir);
    // camera to screen distance
    float c_sdist = 2.0;
    
    // compute the ray direction
    vec3 r_dir = normalize(c_dir);
    // ray progress, just begin at the cameras position
    vec3 r_prog = c_pos + c_right * s_pos.x + c_up * s_pos.y;
    
    float total_dist = 0.0;
    float dist = EPSILON;
    
    for (int i = 0; i < MAX_ITER; i++)
    {
        if (dist < EPSILON || total_dist > MAX_DIST)
        {
            break;
        }
        
        dist = distfunc(r_prog);
        total_dist += dist;
        r_prog += dist * r_dir;
    }
    
    if (dist < EPSILON)
    {   
        vec2 eps = vec2(0.0, EPSILON);
        vec3 normal = normalize(vec3(distfunc(r_prog + eps.yxx) - distfunc(r_prog - eps.yxx),
                                     distfunc(r_prog + eps.xyx) - distfunc(r_prog - eps.xyx),
                                     distfunc(r_prog + eps.xxy) - distfunc(r_prog - eps.xxy)));
        
        vec3 l1_col = vec3(0.83203125, 0.21875, 0.19921875);
        vec3 l1_dir = normalize(vec3(4.0, 1.95, -1.0));
        
        vec3 l2_col = vec3(0.109375, 0.5546875, 0.59375);
        vec3 l2_dir = normalize(vec3(0.0, -1.0, -0.35));
        
        vec3 l3_col = vec3(0.8984375, 0.59765625, 0.05078125);
        vec3 l3_dir = normalize(vec3(-4.0, 1.95, -1.0));
        
        float l1_diffuse = max(0.0, dot(-l1_dir, normal));
		float l1_specular = pow(l1_diffuse, 32.0);
        float l2_diffuse = max(0.0, dot(-l2_dir, normal));
		float l2_specular = pow(l2_diffuse, 32.0);
        float l3_diffuse = max(0.0, dot(-l3_dir, normal));
		float l3_specular = pow(l3_diffuse, 32.0);
        color = (l1_col * (l1_diffuse + l1_specular) +
                 l2_col * (l2_diffuse + l2_specular) +
                 l3_col * (l3_diffuse + l3_specular));
    }
    gl_FragColor = vec4(color, 1.0);
}

/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tlsSzr by mlkn.  Box Loop: https:\/\/www.shadertoy.com\/view\/ldSfWm\nBox Loop 2: https:\/\/www.shadertoy.com\/view\/lllyWN",
  "INPUTS" : [

  ]
}
*/


// https://www.shadertoy.com/view/MslGD8 - basic voronoi
// https://www.shadertoy.com/view/llG3zy - voronoi edge dist
// https://www.shadertoy.com/view/Mtyczw - manhattan edge dist
// https://www.shadertoy.com/view/MstGRr - 2d box distance

#define BOX_SIZE 0.4
#define LOOP_PERIOD 3.7

#define M_PI 3.14159265
#define EPSILON .00001

float invert = 1.0;
float cycle = 0.0;
float edgeCut = 0.03;
float panicTerm = 0.1;
float metricsTerm = 0.0;
float finalSpread = 1.0;
float t = 0.0;

float rot = 0.0;
float nextBoxRot = 0.0;
float scale = 1.0;
float nextBoxScale = 1.0;

vec2 hash( vec2 p )
{
    p = vec2(dot(p,vec2(127.1,311.7)),dot(p,vec2(269.5,183.3)));
    vec2 result = fract(sin(p)*18.5453);
    return 0.5 + panicTerm*sin(t * t * t * 15.0 + cycle + 6.2831*result);
}

float dist_minkowski(vec2 a, vec2 b, float p) {
    // manhattan: return abs(a.x - b.x) + abs(a.y - b.y);
    return pow(pow(abs(a.x - b.x),p) + pow(abs(a.y - b.y),p),1.0/p);
}

vec2 square_test(vec2 a, vec2 b){
    return sqrt((a - b) * (a - b));
}

float vec_max(vec2 a){
    return max(abs(a.x), abs(a.y));
}

float edge_dist_manhattan(vec2 center_a, vec2 center_b, vec2 p){
	
    int max_comp, min_comp;
    vec2 max_base, min_base, split_dir;
    float dist;

    vec2 center_max = max(center_a, center_b);
    vec2 center_min = min(center_a, center_b);
    vec2 square = square_test(center_max, center_min);

    if(vec_max(square) == square.x){
        max_comp = 0;
    }else{
        max_comp = 1;
    }
    min_comp = int(mod(float((max_comp + 1)), 2.0));
    
    vec2 split_pos_a, split_pos_b;
    float u = square[min_comp] / 2.;
    
    float mid = (center_max[max_comp] + center_min[max_comp]) / 2.;
    
    split_pos_a = center_min;
    split_pos_a[max_comp] = mid + u;

    split_pos_b = center_max;
    split_pos_b[max_comp] = mid - u;

    max_base = max(split_pos_a, split_pos_b);
    min_base = min(split_pos_a, split_pos_b);

    split_dir = split_pos_b - split_pos_a;
    vec2 split_dir_test = normalize(center_a - center_b);
    
    vec2 split_origin = max_base;
    vec2 split_tip = min_base;
    if(abs(dot(normalize(split_dir), split_dir_test)) < abs(dot(normalize(min_base - max_base), split_dir_test))){
        split_dir = min_base - max_base;
        split_origin = split_pos_a;
        split_tip = split_pos_b;
    }

    if(square[min_comp] <= 1e-4){
        split_dir = vec2(0.);
        split_dir[max_comp] = 1.;
    }

    vec2 midp = (min_base + max_base) / 2.;
    vec2 dir = p - midp;
    dist = dot(dir, normalize(split_dir));

    vec2 new_pos = p - normalize(split_dir) * dist;

    if(length(midp - new_pos) > length(split_dir) / 2.){
        vec2 nearest_sample;
        vec2 split_o_p = new_pos - split_origin;
        vec2 split_t_p = new_pos - split_tip;

        if(vec_max(split_o_p) < vec_max(split_t_p)){
            nearest_sample = split_origin;
        }else{
            nearest_sample = split_tip;
        }
                
        vec2 n = vec2(0.);
        n[max_comp] = 1.;
        
        vec2 ro = normalize(split_dir);
        float d = dot(p - nearest_sample, n) / dot(ro, n);
        dist = d;
    }

   return abs(dist);
}

vec4 voronoi( in vec2 x )
{
    vec2 n = floor(x);
    vec2 f = fract(x);

    // first pass: regular voronoi
	vec2 closestPoint;
    float id;

    float minDist = 8.0;
    for( int j=-1; j<=1; j++ )
    for( int i=-1; i<=1; i++ )
    {
        vec2 cell = vec2(float(i),float(j));
		vec2 point = hash(n + cell);
        
        vec2 r = cell - f + point;
        float dist = dist_minkowski(r,vec2(0,0), metricsTerm + 1.0);

        if( dist < minDist )
        {
            minDist = dist;
            closestPoint = r;
            id = point.x + point.y;
        }
    }

    // second pass: distance to borders
    minDist = 6.66;
    for( int j=-1; j<=1; j++ )
    for( int i=-1; i<=1; i++ )
    {
        vec2 cell = vec2(float(i),float(j));
		vec2 point = hash(n + cell);

		vec2 r = cell - f + point;

        if( dot(closestPoint-r,closestPoint-r) > EPSILON ) // skip the same cell
        {
            float curDist = mix( // wrong but result is acceptable
                edge_dist_manhattan(f - closestPoint, f - r, f),
                dot(0.5*(closestPoint+r), normalize(r-closestPoint)),
                step(0.03, metricsTerm));
            minDist = min( minDist, curDist );
        }
    }

    return vec4( minDist, closestPoint, id );
}

float box_dist(vec2 p, vec2 size, float radius) {
	size -= vec2(radius);
	vec2 d = abs(p) - size;
  	return min(max(d.x, d.y), 0.0) + length(max(d, 0.0)) - radius;
}

vec2 rotate_around(vec2 p, vec2 c, float angle) {
	vec2 t = p - c;
    vec2 rot = vec2(
        cos(angle) * t.x - sin(angle) * t.y,
        sin(angle) * t.x + cos(angle) * t.y
    );
	return c + rot;
}

float findVal(vec2 myFragCoord)
{
    vec2 p = myFragCoord / RENDERSIZE.xy;
    float aspectRatio = RENDERSIZE.x / RENDERSIZE.y;
    p.x *= aspectRatio;
    vec2 center = vec2(0.5 * aspectRatio, 0.5);
    
    vec2 rotatedP = rotate_around(p, center, rot);
    vec2 c = (rotatedP - center) * scale;
    
    vec2 rotatedP2 = rotate_around(p, center, nextBoxRot);
    vec2 nextBoxC = (rotatedP2 - center) * nextBoxScale;
    float nextBoxDist = box_dist(nextBoxC, vec2(BOX_SIZE), 0.0);
    
    float d = smoothstep(0.0, 0.002, box_dist(c, vec2(BOX_SIZE), 0.0));

    vec4 voro = voronoi((p - center) * mix(4.0, finalSpread, max(t - 0.5, 0.0)*2.0));
    
    float val = min(smoothstep(edgeCut-0.01,edgeCut, voro.x) * (1.0 - d)
                    + smoothstep(0.001, 0.01, nextBoxDist), 1.0);
    val = val * invert - min(invert, 0.0);
    
    return val;
}

void main() {

   

    t = mod(TIME, LOOP_PERIOD) / LOOP_PERIOD;
    cycle = float(int(TIME / LOOP_PERIOD));
    if (mod(cycle, 2.0) < 1.0) invert = -1.0;
    panicTerm = min(max(t - 0.1, 0.01) * 0.4, 0.3);
	metricsTerm = smoothstep(0.45, 0.55, 1.0 - t);
    finalSpread = 15.0 - invert * 7.0;
    edgeCut = min(t * 0.05, 0.05) + 3.3 * pow(max((t - 0.7)/0.7, 0.0), 2.0);
    
    rot = max(min((t - 0.5) * 2.05, 1.0), 0.0);
    nextBoxRot = -pow(rot, 0.3) * M_PI * 0.5 * invert;
    rot = rot*rot*rot* M_PI / 2.0 * invert;  
    scale = 1.0 + max(t-0.5, 0.0) * 5.0;
    nextBoxScale = max((t - 0.5), 0.0) / 0.5;
    
    gl_FragColor = vec4(vec3(findVal(gl_FragCoord.xy)), 1.0);
}

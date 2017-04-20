/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4sjyRz by psykotic.  fooling around. poly is \"kaleidoscopic max norm\", not euclidean, so using it for aa will blur the vertices for very acute angles, but it makes it easy to do miter joints for outlining. could fix with voronoi regions as in roundrect.",
  "INPUTS" : [

  ]
}
*/


float dunion(float d1, float d2) {
    return min(d1, d2);
}

float dintersect(float d1, float d2) {
    return max(d1, d2);
}

float halfplane(vec2 p, vec2 n, float d) {
    return dot(n, p) + d;
}

float disk(vec2 p, vec2 c, float r) {
    return distance(c, p) - r;
}

float poly(vec2 p, float n, vec2 c, float r) {
    float pi = 3.1415926;
    p -= c;
    float b = 2.0*pi / n;
    float a = atan(p.y, p.x);
    a = mod(a + 0.5*b, b) - 0.5*b;
    return length(p)*cos(a) - r;
}

float roundrect(vec2 p, vec2 c, vec2 r) {
    p = abs(p - c) - r;
	if (p.x >= 0.0 && p.y >= 0.0)
        return length(p);
    else
        return max(p.x, p.y);
}

vec2 rotate(vec2 v, vec2 c, float a) {
    v -= c;
    vec2 w = vec2(cos(a), sin(a));
    return c + vec2(w.x * v.x - w.y * v.y, w.y * v.x + w.x * v.y);
}

float ramp(float x) {
    return x;
}

vec4 over(vec4 c1, vec4 c2) {
    return c1 + (1.0 - c1.a) * c2;
}

float scene(vec2 p) {
    float t = 0.5 * sin(TIME) + 0.5;
    float t2 = 0.5 * sin(2.0 * TIME) + 0.5;
    float boxdist = roundrect(p, vec2(1.45, 0.35 + 0.3 * t2), vec2(0.15 + 0.08 * t2, 0.15));
    p = rotate(p, vec2(0.5, 0.5), TIME);
    float sides = 3.0 + 3.0 * t;
    float radius = 0.1 + 0.15 * t2;
    float dist = dunion(boxdist,
      					dunion(poly(p, sides, vec2(0.3, 0.5), 0.2),
                        disk(p, vec2(0.9, 0.5), radius)));
    return dist;
}

void main() {



    float pixwidth = length(1.0 / RENDERSIZE.xy);
	vec2 pos = gl_FragCoord.xy / RENDERSIZE.y;
    
    float aawidth = 1.0; // 0.0 to turn off aa or > 1.0 to see sdf falloff.
    float aa = aawidth * pixwidth;
    float dist = scene(pos);
    float dist2 = scene(pos + vec2(-0.02, 0.02));
    vec4 interior = vec4(0.9, 0.2, 0.2, 0.0);
    
    if (dist <= 0.0)
        interior.a = 1.0;
    else if (dist < aa)
        interior.a = ramp(1.0 - dist / aa);
    interior.rgb *= interior.a;
    
	vec4 outline = vec4(0.15, 0.15, 0.15, 0.0);
    float outline_radius = pixwidth * 10.0;
    if (dist >= -aa) {
        if (dist < 0.0)
            outline.a = ramp(1.0 - (-dist / aa));
        else if (dist <= outline_radius)
        	outline.a = 1.0;
		else if (dist < outline_radius + aa)
            outline.a = ramp(1.0 - (dist - outline_radius) / aa);
    }
        
    outline.rgb *= outline.a;
    vec4 shadow = vec4(0.1, 0.1, 0.1, 0.0);
    if (dist2 <= outline_radius)
        shadow.a = 1.0;
     else if (dist2 < outline_radius + aa)
        shadow.a = ramp(1.0 - (dist2 - outline_radius) / aa);
    shadow.a *= 0.2;
    shadow.rgb *= shadow.a;
    
    vec4 background = vec4(0.6, 0.6, 0.8, 1.0);
    gl_FragColor = over(outline, over(interior, over(shadow, background)));
}
	

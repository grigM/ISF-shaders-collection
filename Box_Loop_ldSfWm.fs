/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ldSfWm by mlkn.  No easings involved.\n\nBox Loop 2: https:\/\/www.shadertoy.com\/view\/lllyWN\nBox Loop 3: https:\/\/www.shadertoy.com\/view\/tlsSzr",
  "INPUTS" : [

  ]
}
*/


#define NUM_LINES 1
#define BOX_SIZE 0.3
#define LOOP_PERIOD 0.9

#define M_PI 3.14159265
#define SQRT_2 1.4142135

float t = 0.0;

// https://www.shadertoy.com/view/MstGRr
float getBoxDist(vec2 p, vec2 size, float radius) {
	size -= vec2(radius);
	vec2 d = abs(p) - size;
  	return min(max(d.x, d.y), 0.0) + length(max(d, 0.0)) - radius;
}

vec2 rotateAroundPoint(vec2 p, vec2 c, float angle) {
	vec2 t = p - c;
    vec2 rot = vec2(
        cos(angle) * t.x - sin(angle) * t.y,
        sin(angle) * t.x + cos(angle) * t.y
    );
	return c + rot;
}

float linesTriangle(float x, float y) {
    float lineWidth = BOX_SIZE / float(NUM_LINES);
	float progress = max((2.0 - t * 4.0)* BOX_SIZE, 0.0);

    for (int i = 0; i < NUM_LINES; i++) {
        float lOffset = float(i) * lineWidth;
        vec2 l = vec2(x, y) - vec2(0.0, lOffset);

        if (l.y > BOX_SIZE && l.y < BOX_SIZE + lineWidth &&
            l.x - l.y + BOX_SIZE * 2.0 > lOffset &&
            -l.x - l.y + BOX_SIZE * 2.0 > lOffset + progress) {
            return 0.0;
        }
    }
    return 1.0;
}

void main() {



    vec2 p = gl_FragCoord.xy / RENDERSIZE.xy;
    float aspectRatio = RENDERSIZE.x / RENDERSIZE.y;
    p.x *= aspectRatio;
    vec2 center = vec2(0.5 * aspectRatio, 0.5);
    
    t = mod(DATE.w, LOOP_PERIOD) / LOOP_PERIOD;
    
    float rot = max(min(t * 2.5 - 0.4, 1.0), 0.0) * M_PI / 4.0;
    p = rotateAroundPoint(p, center, rot);
    
    float scale = 1.0 + t * (SQRT_2 - 1.0);
    vec2 c = (p - center) * scale;
    
    float d = getBoxDist(c, vec2(BOX_SIZE), 0.0);
    d = smoothstep(0.0, 0.001, d);
    // d = smoothstep(d, 0.001, 0.0); // debug
    
    float triangles = linesTriangle(c.x, c.y) * linesTriangle(c.x, -c.y) * 
        linesTriangle(c.y, c.x) * linesTriangle(c.y, -c.x);
    d = min(d, triangles);
	gl_FragColor = vec4(vec3(d),1.0);
}

/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XlcGWf by spacetug.  Drawing lines and circles",
  "INPUTS" : [

  ]
}
*/


float circleD(vec2 center, float radius)
{
    return length(center) - radius;
}

float lineD(vec2 o, float slope) {
    return 0.0;
}

vec4 circle(vec2 center, float radius, float lw, vec3 color) {
    float d = circleD(center, radius);
    float a = smoothstep(lw, 0.0, d);
    return vec4(color.rgb, a);    
}

vec4 circumference(vec2 center, float radius, float lw, vec3 color) {
    float d = circleD(center, radius);
    float a = smoothstep(1.0, 0.0, abs(d)/lw);
    return vec4(color.rgb,a);    
}

vec4 waveThing(vec2 uv, float lw, vec3 color) {
	float a = smoothstep(1.0,0.0,abs(cos((uv.x)*4.0)/4.0 - uv.y)/lw);
	return vec4(color.rgb, a);
}

vec4 lineThing(float t, float c, vec2 uv, float lw, vec3 color) {
    float a = smoothstep(1.0,0.0,abs(uv.x*t + c - uv.y) / lw);
	return vec4(color.rgb, a);
}

void main()
{
    const float radius = 0.5;
    
    float linew = 0.008;//RENDERSIZE.y * 0.00003;
    float x = TIME;

    float aspect = RENDERSIZE.x / RENDERSIZE.y;
    
    vec2 uv = (gl_FragCoord.xy / RENDERSIZE.xy) * 2.0 - 1.0;
    uv.x *= aspect;
    
    vec2 center = uv;
    
    vec4 acc = vec4(0,0,0,0);
    vec4 c = circumference(center, radius, linew, vec3(0,1,0));
    acc = mix(acc, c, c.a);
    
    float t = 1.0/sin(x) * 4.0;

    c = lineThing(0.0, 0.7, uv, 0.03 + 0.01 * cos(x * 4.0), vec3(1,0,1));
    acc = mix(acc, c, c.a);
    
   	c = circle(uv - vec2(1.,.7), 0.1 + 0.09 * abs(sin(x)), .01, vec3(.1,1,1));
    acc = mix(acc, c, c.a);

   	c = circle(uv - vec2(-1.,.7), 0.1 + 0.09 * abs(sin(x+1.)), .01, vec3(.1,1,1));
    acc = mix(acc, c, c.a);

    c = lineThing(t, 0.0, uv, 0.05, vec3(1,1,1));
    acc = mix(acc, c, c.a);
    
    c = lineThing(-t, 0.0,uv, 0.05, vec3(1,1,1));
    acc = mix(acc, c, c.a);

    c = circle(center, 0.11 + 0.01 * cos(x*4.0), linew, vec3(1,1,0));
    acc = mix(acc, c, c.a);
    
    center = vec2(radius * sin(x), radius * cos(x)) + center;
    c = circumference(center, 0.05, linew, vec3(1,0,0));
    acc = mix(acc, c, c.a);
    
    center = vec2(0.05 * sin(x * 4.0), 0.05 * cos(x * 4.0)) + center;
    c = circumference(center, 0.02, linew, vec3(0,0,1));
    acc = mix(acc, c, c.a);

    c = waveThing(uv - vec2(x, -0.25), linew, vec3(0.5,1.0,.5));
    acc = mix(acc, c, c.a * 0.5);

    c = lineThing(0.0, 0.02 * sin(x*10.), uv + vec2(0,0.15), linew, vec3(1.,0.2,0.5));
    acc = mix(acc, c, c.a);

    c = lineThing(0.0, 0.0, uv + vec2(0,0.25), 0.03 + 0.005 * cos(x), vec3(1.,0.2,0.5));
    acc = mix(acc, c, c.a);
    
    c = lineThing(0.0, -0.02 * sin(x*10.), uv + vec2(0,0.35), linew, vec3(1.,0.2,0.5));
    acc = mix(acc, c, c.a);

    c = waveThing(uv - vec2(-x, -0.25), linew, vec3(0.5,1.0,.5));
    acc = mix(acc, c, c.a * 0.5);

    c = waveThing(uv + vec2(0, 0.25), linew, vec3(1,0.5,.5));
    acc = mix(acc, c, c.a);
    
    float xx = sin (x*0.2) * aspect;
    center = vec2(xx, cos((xx)*4.0)/4.0) - (uv + vec2(0, 0.25));
    c = circumference(center, 0.02, linew, vec3(1,0,1));
    acc = mix(acc, c, c.a);

    center = vec2(0.05 * sin(x * -4.0), 0.05 * cos(x * -4.0)) + center;
    c = circumference(center, 0.04, linew, vec3(0,1,1));
    acc = mix(acc, c, c.a);
    
    xx = sin (x*5.) * aspect;
    c = circle(uv + vec2(xx, .25), 0.1 + 0.025 * sin(x) * aspect, linew, vec3(0.2,0.5,1));
    acc = mix(acc, c, c.a);
    
    gl_FragColor = acc;
}
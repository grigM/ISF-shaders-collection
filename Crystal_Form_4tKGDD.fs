/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "fasdf",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tKGDD by Imsure1200q_1UWE130.  sadas",
  "INPUTS" : [

  ]
}
*/


float rand(vec2 co){
    return fract(sin(dot(co ,vec2(12.9898,78.233))) * 43758.5453);
}
float time = TIME;
//Noise
float noise(float p){
    float fl = floor(p);
  float fc = fract(p);
    return float(mix(rand(vec2(fl)), rand(vec2(fl + 1.0)), fc));
}
//Noise 2
float noise(vec2 n) {
    const vec2 d = vec2(0.0, 1.0);
  vec2 b = floor(n), f = smoothstep(vec2(0.0), vec2(1.0), fract(n));
    return mix(mix(rand(b), rand(b + d.yx), f.x), mix(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}
vec4 box(vec3 p, float w){
    p = abs(p);
    float dx = p.x-w;
    float dy = p.y-w;
    float dz = p.z-w;
    float m = max(p.x-w, max(p.y-w, p.z-w));
    return vec4(m,dx,dy,dz);
}
mat3 rotateX(float a){
    return mat3(1.,0.,0.,
                0.,cos(a), -sin(a),
                0.,sin(a), cos(a));
}

mat3 rotateY(float a){
    return mat3(cos(a), 0., -sin(a),
                0.,1.,0.,
                sin(a), 0., cos(a));
}

mat3 doRot()
{
    return rotateX(floor(time)*1.9)*rotateY(floor(time)*1.4);
}
vec4 map(vec3 p){
    for (int i = 0; i < 5; i++){
        p = abs(p*doRot() + vec3(0.1, .0, .0));
        p.y -= .8;
        p.x -= .06;
        p.z -= sin(time*80.)*.1*pow((1.-fract(time)),4.);
        p.xy = p.yx;
    }
    return box(p, .7);
}
vec3 normal(vec3 pos)
{
	vec3 eps = vec3( 0.001, 0.0, 0.0 );
	vec3 nor = vec3(
	    map(pos+eps.xyy).x - map(pos-eps.xyy).x,
	    map(pos+eps.yxy).x - map(pos-eps.yxy).x,
	    map(pos+eps.yyx).x - map(pos-eps.yyx).x );
	return normalize(nor);
}
void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv = uv * 2.0 - 1.0;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    vec3 rd = normalize(vec3(uv, 1.0));
    vec3 t = normal(rd);
    vec3 fog = 1.0/(1.0+t*t*noise(t.xy));
    vec3 fc = vec3(fog);
	gl_FragColor = vec4(fc*noise(fc.xy), 1.0);
}
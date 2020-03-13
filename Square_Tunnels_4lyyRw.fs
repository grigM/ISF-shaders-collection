/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4lyyRw by momoro.  Random WIP",
  "INPUTS" : [

  ]
}
*/




#define PI 3.141592654

float map(float value, float inMin, float inMax, float outMin, float outMax) {
  return outMin + (outMax - outMin) * (value - inMin) / (inMax - inMin);
}


mat2 rotate(float angle) {
    return mat2(cos(angle), -sin(angle),
                sin(angle), cos(angle)
    );
}


vec3 rgb2hsv(vec3 c)
{
    vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
    vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

vec3 mod289(vec3 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
vec2 mod289(vec2 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
vec3 permute(vec3 x) { return mod289(((x*34.0)+1.0)*x); }


float atan201(float x, float y) {
    return (atan(x, y) + PI) / (PI * 2.);
}

float sin01(float n) {
    return sin(n)/2.+.5;
}

vec2 sin01(vec2 n) {
    return sin(n)/2.+.5;
}

vec4 blend(vec4 bg, vec4 fg) {
    vec4 c = vec4(0.);
    c.a = 1.0 - (1.0 - fg.a) * (1.0 - bg.a);
    if(c.a < .00000) return c;
    
    c.r = fg.r * fg.a / c.a + bg.r * bg.a * (1.0 - fg.a) / c.a;
    c.g = fg.g * fg.a / c.a + bg.g * bg.a * (1.0 - fg.a) / c.a;
    c.b = fg.b * fg.a / c.a + bg.b * bg.a * (1.0 - fg.a) / c.a;
    
    return c;
}

float snoise(vec2 v) {

    // Precompute values for skewed triangular grid
    const vec4 C = vec4(0.211324865405187,
                        // (3.0-sqrt(3.0))/6.0
                        0.366025403784439,
                        // 0.5*(sqrt(3.0)-1.0)
                        -0.577350269189626,
                        // -1.0 + 2.0 * C.x
                        0.024390243902439);
                        // 1.0 / 41.0

    // First corner (x0)
    vec2 i  = floor(v + dot(v, C.yy));
    vec2 x0 = v - i + dot(i, C.xx);

    // Other two corners (x1, x2)
    vec2 i1 = vec2(0.0);
    i1 = (x0.x > x0.y)? vec2(1.0, 0.0):vec2(0.0, 1.0);
    vec2 x1 = x0.xy + C.xx - i1;
    vec2 x2 = x0.xy + C.zz;

    // Do some permutations to avoid
    // truncation effects in permutation
    i = mod289(i);
    vec3 p = permute(
            permute( i.y + vec3(0.0, i1.y, 1.0))
                + i.x + vec3(0.0, i1.x, 1.0 ));

    vec3 m = max(0.5 - vec3(
                        dot(x0,x0),
                        dot(x1,x1),
                        dot(x2,x2)
                        ), 0.0);

    m = m*m ;
    m = m*m ;

    // Gradients:
    //  41 pts uniformly over a line, mapped onto a diamond
    //  The ring size 17*17 = 289 is close to a multiple
    //      of 41 (41*7 = 287)

    vec3 x = 2.0 * fract(p * C.www) - 1.0;
    vec3 h = abs(x) - 0.5;
    vec3 ox = floor(x + 0.5);
    vec3 a0 = x - ox;

    // Normalise gradients implicitly by scaling m
    // Approximation of: m *= inversesqrt(a0*a0 + h*h);
    m *= 1.79284291400159 - 0.85373472095314 * (a0*a0+h*h);

    // Compute final noise value at P
    vec3 g = vec3(0.0);
    g.x  = a0.x  * x0.x  + h.x  * x0.y;
    g.yz = a0.yz * vec2(x1.x,x2.x) + h.yz * vec2(x1.y,x2.y);
    return dot(m, g);
}


float fbm1d(float x, float amplitude, float frequency, float offset) {
    x += offset;
    float y = 0.;
    // Properties
    const int octaves = 8;
    float lacunarity = 1.144;
    float gain = 1.092;
    
    // Initial values
    //sin(u_time) * 5. + 10.;
    //sin(u_time/10. + 10.);
    
    // Loop of octaves
    for (int i = 0; i < octaves; i++) {
        y += amplitude * snoise(vec2(frequency*x));
        frequency *= lacunarity;
        amplitude *= gain;
    }
    
    return y;
}


float smin(float a, float b, float k){

   float f = max(0., 1. - abs(b - a)/k);
   return min(a, b) - k*.25*f*f;
}

float smax(float a,float b, float k)
{
    return -smin(-a,-b,k);
}

float smootheststep(float t) {
    return -20.*pow(t, 7.)+70.*pow(t,6.)-84.*pow(t,5.)+35.*pow(t,4.); // when smootherstep's second derivative isn't enough
}


float pcsmooth(float x) {
    return -
        pow(cos((PI*(x)/2.)), 3.) + 1.;
}

void main() {

    
    
    vec4 color;
    vec2 st = (gl_FragCoord.xy - 0.5 * RENDERSIZE.xy) / RENDERSIZE.y;
    
    
    st /=20.036;
    //st += vec2(0.000,-0.055);
    
    st *= rotate(TIME/.9);
    
    /////////////////////////////////////////////////////////////
    // Blobby circle
    #define arcs 7.
    
    float f = atan201(st.x, st.y); // angle
    float m = smoothstep(0., 1., fract(f * arcs)); // mix amount
	
     m = pcsmooth(fract(f*arcs));
    
    f = ceil(f * arcs) / arcs; // split up
    
    float fNext = f + 1./arcs;
    // fNext = f;
    
    
    // Smooth the blob at the end
    bool e = false;
    if(f > (arcs-1.)/arcs + .01) {
		fNext = 1./arcs;
    }
    
    // Add noise
    float amp = 10000.;
    float freq = 30.;
    
    float offset = 1.144 + TIME/300.;
    f = fbm1d(f, amp, freq, offset);
    fNext = fbm1d(fNext, amp, freq, offset);
    
    f =  map(f, -20., 1., 0.648, 0.712);
    fNext = map(fNext, -20., 1., 0.648, 0.712);
    float len = length(st);
    float mx = mix(f, fNext, m);
    len += mx * 0.080;
    
  //  float c = smoothstep(len, 0.712, 0.900);
    
   // color.rgb = vec3(c);
  //  color.a = 1.;
    //color.rgb = vec3(mx);
    
    #define steps 90.
    for(float i=0.; i<steps; i++) {
   		float df = max(abs(st.x), abs(st.y));
   //     df = len -1.984;
        // df = length(st);
        
        float incr = 0.4 - (0.4 * i/steps);
        
        float f = 1.0 - smoothstep(incr-.001, incr, df);
        // SHADOW
       f -=  (1.0 - (fract(df*steps) + 0.0)) * 1. * f;
        
        vec3 rgb = vec3(f);
        
        vec3 hsv = vec3(1.);
        hsv.x = pow(2. * fract((i*6.)/steps + TIME/2.3) - 1., 2.);
        hsv.x = map(hsv.x, 0., 1., 0.0, 0.108);
        hsv.y = .9;
        vec3 rgb2 = hsv2rgb(hsv);
        rgb = rgb2 * rgb * 3.112;
        float a = (i/steps + 0.640) * f;
       // a  = 0.208;
        color = blend(color, vec4(rgb, a * f));
    }
    
   // st = st * rotate(-300.148 + -TIME/.3);
    st += vec2(0,0);
    st *= 1.128 + sin(TIME);
    
    for(float i=0.; i<steps; i++) {
   		float df = max(abs(st.x), abs(st.y));
      //  df = len  *.3;
        // df = length(st);
        
        float incr = 0.4 - (0.4 * i/steps);
        
        float f = 1.0 - smoothstep(incr-.001, incr, df);
        // SHADOW
       f -=  (1.0 - (fract(df*steps) + 0.)) * 1.* f;
        
        vec3 rgb = vec3(f);
        
        vec3 hsv = vec3(1.);
        hsv.x = pow(2. * fract((i*6.)/steps + TIME/9.3) - 1., 2.);
        hsv.x = map(hsv.x, 0., 1., 0.0, 0.964);
        hsv.y = .9;
        hsv.z = .3;
        vec3 rgb2 = hsv2rgb(hsv);
        rgb = rgb2 * rgb * 3.112;
        float a = (i/steps + 0.01) - 0.472;
       // a  = 0.208;
        color = blend(color, vec4(rgb, a * f));
    }
        
    color = color;
    
    gl_FragColor = color;
    
}

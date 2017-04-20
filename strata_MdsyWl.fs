/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "stripes",
    "metal",
    "purple",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MdsyWl by mahalis.  experimenting with height maps",
  "INPUTS" : [

  ]
}
*/


float smoothStripe(float v, float stripeWidth, float smoothingWidth) {
    return smoothstep(-smoothingWidth,smoothingWidth,abs(1. - 2. * fract(v / stripeWidth)) - 0.5);
}

float singleNoise(vec2 v, vec2 axis, vec2 f1, vec2 f2, float o1, float o2) {
    const float m2 = 0.5;
    vec2 dv = vec2(dot(v, axis), dot(v, vec2(axis.y, -axis.x)));
    return (sin(dv.x * f1.x + dv.y*f1.y + o1) + cos(dv.x * f2.x + dv.y * f2.y + o2) * m2) / (1. + m2);
}

float layeredNoise(vec2 v, float t) {
    vec2 noiseBasis1 = normalize(vec2(0.,1.) + 0.193 * vec2(cos(t * 0.302 + 0.44), cos(t * 0.223 + 0.91)));
    vec2 noiseBasis2 = vec2(sqrt(3.0)/2., -0.5);
    vec2 noiseBasis3 = normalize(vec2(-noiseBasis2.x, noiseBasis2.y) + 0.181 * vec2(cos(t * 0.118 + 1.), cos(t * 0.323)));
    return singleNoise(v, noiseBasis1, vec2(1.13, 1.04), vec2(1.98), 0., 0.19) + singleNoise(v, noiseBasis2, vec2(1.29, 1.32), vec2(1.88), 2.03,1.1) + singleNoise(v, noiseBasis3, vec2(1.01, 0.96), vec2(2.03), 2.93, 0.553);
}

const float stripeWidth = 1.8;

float noiseValue(vec2 uv, float time) {
    return layeredNoise(uv * 6. - time * 0.1, time) + time * 0.298;
}

float height(float noiseValue);
float height(float noiseValue) {
    return pow(0.5 - 0.5*cos(fract(2.*noiseValue / stripeWidth + 0.5) * 3.1415 * 2.), 0.5);
}

void main() {

    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv -= 0.5;
    uv.y *= RENDERSIZE.y / RENDERSIZE.x;
    uv *= 1.2;
    
    float time = sin(TIME * 0.05) * 40.;
    float v = noiseValue(uv, time);
    
    float stripeValue = smoothStripe(v, stripeWidth, 0.01);
    
    vec2 eps = vec2(0.01,0);
    float vx = noiseValue(uv + eps.xy, time);
    float vy = noiseValue(uv + eps.yx, time);
    
    float h = height(v);
    vec3 n = normalize(vec3((vec2(height(vx), height(vy)) - h) / eps.x, 42));
    const vec3 lightPos = vec3(0, 2, 1);
    vec3 toLight = normalize(lightPos - vec3(uv, 0));
    float lightIntensity = smoothstep(0.,0.1,dot(normalize(lightPos),toLight) - 0.9); // spotlight attenuation
    
    float lightValue = smoothstep(0.,0.1,max(0.0,dot(n, toLight)) - 0.6) * lightIntensity;
    
    vec3 color = mix(vec3(0.2),vec3(0.6,0.2,1.),float(stripeValue));
    
    gl_FragColor = vec4(color * lightValue, 1.);
}

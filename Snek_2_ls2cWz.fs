/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ls2cWz by Kos.  this time it turns",
  "INPUTS" : [

  ]
}
*/


#define M_PI (355./113.)

ivec2 resolution = ivec2(20, 14); // 48x27
float speed = 8.;
float magic = 0.1;

vec4 colorOff = vec4(0, .15, 0, 0);
vec4 colorOn = vec4(.6, 1., 0., 0);

float zztop(float x, float a, float b) {
    float d = b-a;
    return x - mix(0., d, clamp(x/d - a/d, 0., 1.));
}

int zztop(int x, int a, int b) {
    return int(magic+zztop(float(x), float(a), float(b)));
}

float aatop(float x, float a, float b) {
    float d = b-a;
    return mix(0., d, clamp(x/d - a/d, 0., 1.));
}

int aatop(int x, int a, int b) {
    return int(magic+aatop(float(x), float(a), float(b)));
}


float colorAt(int frame, ivec2 gridCoord) {
    //int x = frame % resolution.x;
    int t = int(mod(float(frame), float(20+28+15)));
    
    int x = zztop(t, 20, 35);
    int y = aatop(t, 20, 35)+5;
    
    ivec2 head = ivec2(x, y);
    return (gridCoord == head) ? 1. : 0.;
}
    

void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    ivec2 gridCoord = ivec2(uv * vec2(resolution));
    
    int frame = int(TIME*speed);
    float frameFraction = (TIME*speed - float(frame));
    
    gl_FragColor = vec4(0);
    gl_FragColor += colorOn * colorAt(frame, gridCoord); // 1
    gl_FragColor += colorOn * colorAt(frame-1, gridCoord)*(1.-frameFraction*0.25); // 1..0.75
    gl_FragColor += colorOn * colorAt(frame-2, gridCoord)*(1.-frameFraction*0.25 - 0.25); //  0.75 .. 0.5
    gl_FragColor += colorOn * colorAt(frame-3, gridCoord)*(1.-frameFraction*0.25 - 0.5); // 0.5 .. 0.25
    gl_FragColor += colorOn * colorAt(frame-4, gridCoord)*(1.-frameFraction*0.25 - 0.75); // 0.25 .. 0
    gl_FragColor += colorOff;
    
    vec2 gridOffset = uv - (vec2(gridCoord) / vec2(resolution));
    gridOffset *= vec2(resolution);
    
    float pixelShape = sin(gridOffset.x*M_PI) * sin(gridOffset.y*M_PI);
    pixelShape = pow(pixelShape, 0.1);    
    
    gl_FragColor *= pixelShape;
    
    //gl_FragColor = vec4(aatop(uv.x, 0.2, 0.5));
}

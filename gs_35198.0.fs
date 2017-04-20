/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35198.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

// MODS BY NRLABS 2016 


#define iGlobalTime TIME 
#define iResolution RENDERSIZE

void mainImage(out vec4 fragColor, in vec2 fragCoord);
void main( void ) { mainImage(gl_FragColor,gl_FragCoord.xy); }

// ---------------------------------------------------------------------

// https://www.shadertoy.com/view/llVGzh

#define PI 3.1415
#define NRINGS 7
#define NBALLS 64

//colour palette 1
const vec3 CP1A = vec3(0.5, 0.5, 0.5);
const vec3 CP1B = vec3(0.5, 0.5, 0.5);
const vec3 CP1C = vec3(2.0, 1.0, 0.0);
const vec3 CP1D = vec3(0.50, 0.20, 0.25);
//colour palette 2
const vec3 CP2A = vec3(0.5, 0.5, 0.5);
const vec3 CP2B = vec3(0.5, 0.5, 0.5);
const vec3 CP2C = vec3(1.0, 1.0, 1.0);
const vec3 CP2D = vec3(0.00, 0.10, 0.20);

const float ROT = PI * 0.03125; //rotation between balls in ring

float t = iGlobalTime * 0.015;

//IQ
//cosine based palette, 4 vec3 params
vec3 palette1(in float t) {
    return CP1A + CP1B * cos(6.28318 * (CP1C * t + CP1D));
}
vec3 palette2(in float t) {
    return CP2A + CP2B * cos(6.28318 * (CP2C * t + CP2D));
}

vec2 rotate(inout vec2 p, float a) {
    float x = p.x * cos(a) - p.y * sin(a);
    float y = p.y * cos(a) + p.x * sin(a);
    return vec2(x, y);
}

vec4 drawScene(vec2 uv) {
    
    vec4 pc = vec4(0.0);//pixel colour
    vec4 lc = vec4(1.0);//light colour
    float xl = mod(t, PI * 32.0); //long timer rotates animation
    float x = mod(t, PI * 4.0); //stop start dynamic rotations

    //iterate for number of rings
    for (int r = 0; r < NRINGS; r++) {
        
        //iterate for number of balls in ring
        for (int b = 0; b < NBALLS; b++) {
            
            float fr = float(r);
            float fb = float(-b);
            
            lc = vec4(palette1(fr * 0.05 + fb * 0.05), 1.0);
            float td = 1.0; // TIME direction
            
            //alternate direction and colouring of rings
            if (mod(fr, 2.0) == 0.0) {
                td = -td; //reverse TIME direction
                lc = vec4(palette2(fr * 0.05 + fb * 0.05), 1.0);
            }
            
            vec2 sp = vec2(0.9 - fr * 0.1, 0.0); //start point
            float a = ROT * fb; //rotation angle
            
            //alternate rotation scenes
            float xt = x;
            if (xl > PI * 4.0 && xl <= PI * 8.0) {
                xt = x / (fb * 0.03);    
            } else if (xl > PI * 4.0 && xl <= PI * 10.0) {
                xt = x - a;
            } else if (xl > PI * 10.0) {
                xt = x * (fb);
            }
            
            //animate
            if (xt <= PI * 2.0) {
                a += xt * td;
            }
            
            vec2 point = rotate(sp, a); //rotated point
            float d = distance(uv, point); //distance from current pixel to rotated point
            pc += lc / (d * d * (2000.0 + fr * 2000.0)); //add some colour
        }
    }

    return pc;
}


void mainImage(out vec4 fragColor, in vec2 fragCoord) {
    if( t < 1000.0 )
    {
	t += 1000.0;
    }
    vec2 uv = fragCoord.xy / iResolution.xy;
    uv = uv * 2.0 - 1.0;
    uv.x *= iResolution.x / iResolution.y;
    fragColor = drawScene(vv_FragNormCoord/cos(TIME/8.+5.*dot(vv_FragNormCoord, vv_FragNormCoord) + pow(length(vv_FragNormCoord), -0.33)));
}

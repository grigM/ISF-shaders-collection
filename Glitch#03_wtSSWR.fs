/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/wtSSWR by 104.  Abstract digital painting",
    "IMPORTED": {
    },
    "INPUTS": [
    ]
}

*/



const float z = 5.;
const float complexity = 18.;
const float speed = 1./3.;
const float density = .5; // 0-1

 
vec4 hash42(vec2 p)
{
	vec4 p4 = fract(vec4(p.xyxy) * vec4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.wzxy+33.33);
    return fract((p4.xxyz+p4.yzzw)*p4.zywx);
}

mat2 rot2D(float r){
    return mat2(cos(r), sin(r), -sin(r), cos(r));
}

void main() {
	
	float PI = atan(1.)*4.;



    vec2 R = RENDERSIZE.xy;
    vec2 uv = gl_FragCoord.xy/R.xy;
    vec2 N = uv;
    uv.x *= R.x/R.y;
    uv *= z;
    float t = TIME;
    uv += floor(t*speed)*z*1.618; // scene switcher
    gl_FragColor = vec4(1);
    for (float i = 1.;i <= complexity; ++ i) {
        vec4 h = hash42(floor(uv+i));
        vec2 p = fract(uv+i)-.5;
        p *= rot2D(h.x*PI*2.);
        uv += p * h.z;
        h = hash42(floor(uv));
        h = fract(h + TIME*.03);// animate scene
        if (i < 2. || h.w > density) {
            gl_FragColor *= h+.0;
        }
    }
  	gl_FragColor=(step(.5,gl_FragColor)*.7+.15) * mod(gl_FragCoord.y,3.)/2.;
}




/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WsK3W1 by 104.  90's inspired?",
  "INPUTS" : [

  ]
}
*/


const float PARTITIONS = 9.;
const float SHADETHRESH = .9;

vec3 dtoa(float d, vec3 amount){
    return vec3(1. / clamp(d*amount, vec3(1), amount));
}

vec4 hash42(vec2 p)
{
	vec4 p4 = fract(vec4(p.xyxy) * vec4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.wzxy+33.33);
    return fract((p4.xxyz+p4.yzzw)*p4.zywx);
}

#define nsin(x) (sin(x)*.5+.5)
#define q(x,p) (floor((x)/(p))*(p))

void main() {



    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy-.5;
    vec2 R = RENDERSIZE.xy;
    uv.x *= R.x / R.y;
    uv *= 2.;
    vec2 cellUL = vec2(-.5);
    vec2 cellBR = vec2(.5);
    float seed = 1e3;
    gl_FragColor = vec4(0);
    vec3 lineColor;
    vec3 a, a2;
    
    for(float i = 0.; i < PARTITIONS; ++ i) {
        vec4 h = hash42(1e3*cellUL+seed); // xy = pos to divide between cellUL / cellBR.
        vec4 h2 = hash42(1e2*cellUL+seed);
        h.xy = mix(cellUL, cellBR, h.xy);
        seed = h2.z+i;
        vec2 uv2 = uv;
        uv2 += q(sin(TIME*(h.z-.5)+(h.w*6.28))*.5, h2.w+.01);
        uv2.x += uv2.y * (h2.x-.5);
      	uv2.y += uv2.x * (h2.y-.5);
        float dl = min(length(uv2.x - h.x), length(uv2.y - h.y));
        if (h2.x > SHADETHRESH) {
            gl_FragColor.rgb = mix(h2.ywx, lineColor, a2 * nsin(150.*(uv.x+uv.y)));
        }
        dl = fract(dl);// cheap way to add more angles.
        a = dtoa(dl, vec3(800));
        a2 = dtoa(dl-.05, vec3(40));
        lineColor = hash42(h2.xz*1e3).rgb;
        gl_FragColor.rgb = max(gl_FragColor.rgb, lineColor * a);
        
        if (uv2.x < h.x) {
            if (uv2.y < h.y) {
                cellBR = h.xy;
            } else {
              	cellUL.y = h.y;
              	cellBR.x = h.x;
            }
        } else {
            if (uv2.y > h.y) {
                cellUL = h.xy;
            } else {
                cellUL.x = h.x;
                cellBR.y = h.y;
            }
	    }
        uv *= 1.05;
    }
    
    vec2 N = gl_FragCoord.xy / RENDERSIZE.xy-.5;
    gl_FragColor = clamp(gl_FragColor,0.,1.);
    gl_FragColor = pow(gl_FragColor,gl_FragColor-gl_FragColor+.8);
    gl_FragColor.a = 1.;
}


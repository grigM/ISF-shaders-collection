/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WlsXzM by 104.  I just want to touch it.",
  "INPUTS" : [

  ]
}
*/



const float PI = 3.141592654;

vec3 hash32(vec2 p){
	vec3 p3 = fract(vec3(p.xyx) * vec3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yxz+19.19);
    return fract((p3.xxy+p3.yzz)*p3.zyx);
}
vec4 disco(vec2 uv) {
    float v = abs(cos(uv.x * PI * 2.) + cos(uv.y *PI * 2.)) * .5;
    uv.x -= .5;
    vec3 cid2 = hash32(vec2(floor(uv.x - uv.y), floor(uv.x + uv.y)));
    return vec4(cid2, v);
}
float nsin(float t) {return sin(t)*.5+.5; }

void main() {



    vec2 R = RENDERSIZE.xy;
    vec2 uv = gl_FragCoord.xy / R - .5;
    uv.x *= R.x / R.y;
    float t = (TIME + 129.) * .6; //t = 0.;
    uv = uv.yx;
    uv *= 2.+sin(t)*.2;
    uv.x += t*.5;
    
    gl_FragColor = vec4(1);
    float sgn = -1.;
    for(float i = 1.; i <= 5.; ++i) {
        vec4 d = disco(uv);
        float curv = pow(d.a, .5-((1./i)*.3));
        curv = pow(curv, .8+(d.b * 2.));
        curv = smoothstep(nsin(t)*.3+.2,.8,curv);
        gl_FragColor += sgn * d * curv;
        gl_FragColor *= d.a;
        sgn = -sgn;
        uv += 100.;// move to a different cell
        uv += sin(d.ar*7.33+t*1.77)*(nsin(t*.7)*.1+.04);
    }
    
    // post
   	gl_FragColor.gb *= vec2(1.,.5);//tint
    vec2 N = (gl_FragCoord.xy / R )- .5;
    gl_FragColor = clamp(gl_FragColor,.0,1.);
    gl_FragColor = pow(gl_FragColor, vec4(.2));
    gl_FragColor.rgb -= hash32(gl_FragCoord.xy + TIME).r*(1./255.);
    
    N = pow(abs(N), vec2(2.5));
    N *= 7.;
    gl_FragColor *= 1.5-length(N);// ving
    gl_FragColor = clamp(gl_FragColor,.0,1.);
    gl_FragColor.a = 1.;
}



/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#58816.0"
}
*/


// by @jimhejl 

#ifdef GL_ES
precision mediump float;
#endif

const vec3 c0 = vec3(0.20,0.60,1.00);
const vec3 c1 = vec3(0.99,0.60,0.20);

float sdCapsule(vec3 p, vec3 a, vec3 b, float r)
{
    vec3 ab = b - a;
    float t = dot(p - a, ab) / dot(ab, ab);
    t = clamp(t, 0.0, 1.0);
    return length((a + t * ab) - p) - r;
}

float flare(float e, float i, float s) { return exp(1.-(e*i))*s; }

vec3 ToneMapFilmicALU_HEJL(vec3 y)
{
    vec3 x = max(vec3(0.), y-vec3(0.004));          // approx linear segment
    return (y*(2.2*y+0.5))/(y*(6.2*y+5.7)+0.46);    // approx gamma(2.2) & filmic response
}


void main( void ) 
{
    vec2 unipos = (gl_FragCoord.xy / RENDERSIZE);
    vec2 pos = unipos*2.0-1.0;
    pos.x *= RENDERSIZE.x / RENDERSIZE.y;

    vec2 t = mix(vec2(1.5),vec2(1.0),vec2(sin(TIME),cos(TIME)));

    float d0  = sdCapsule(vec3(pos.xy,0.),vec3(-.8,-.3,.0), vec3(-.8,.3,.0),.2);
    vec3 clr1 = c0 * flare(d0,6.3,.8) + flare(d0,3.3*(1.-t.x),0.08); 

    float d1  = sdCapsule(vec3(pos.xy,0.),vec3(-.8,mix(-.3,.3,t.x*.84),0.0), vec3(0.8,mix(0.6,-0.3,t.y),0.0), 0.12); 
    vec3 clr2 = mix(c0,c1,unipos.x)  * (flare(d1,7.3,.8) + flare(d1,4.3*t.x,0.09)) * .75; 

    float d2  = sdCapsule(vec3(pos.xy,0.),vec3(0.8,-0.3,0.0), vec3(0.8,0.3,0.0),.2);
    vec3 clr3 = c1 * flare(d2,2.3,0.8) + flare(d2,3.3,1.08);  
    vec3 cOut =clr1+clr2+clr3;
    gl_FragColor = vec4(mix(ToneMapFilmicALU_HEJL(pow(cOut,vec3(42.2))),cOut,.2),1.);
}
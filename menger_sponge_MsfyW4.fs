/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "normals",
    "mengersponge",
    "cuberaymarching",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MsfyW4 by abje.  a menger sponge",
  "INPUTS" : [

  ]
}
*/


vec3 box(vec3 p, float s) {
    p = max(abs(p)-s,0.0);
    return (p);
}
float box(vec2 p) {
    p = abs(p);
    return max(p.x,p.y);
}

vec3 middle(vec3 p) {
    float small = min(min(p.x,p.y),p.z);
    float large = max(max(p.x,p.y),p.z);
    
    return vec3(lessThan(abs(p-(p.x+p.y+p.z-small-large)),vec3(0.000001)));
}

vec4 map(vec3 p) {
    vec3 dists = vec3(0.0);
    
    for (float i = 1.0; i < 5.0; i++) {
        
        float num = 1.0/pow(3.0,i);
        vec3 p2 = num-abs(mod(p+num*3.0,num*6.0)-num*3.0);
        dists = max(dists, middle(p2)*p2);
        
    }
    
    float len = max(max(dists.x,dists.y),dists.z);
    return vec4(dists,len);
    //return vec4(dists,dists.x);
}

void main() {



	vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE.xy) / RENDERSIZE.y;
    
    vec3 start = vec3(0.0,0.37,TIME*0.1);
    vec3 p = start;
    vec3 dir = vec3(uv,1.0)/max(max(abs(uv.x),abs(uv.y)),1.0);
    
    float dist = 0.0;
    vec4 len = vec4(0.0);
    
    for (int i = 0; i < 100; i++) {
        len = map(p);
        if (len.w < 0.001||dist > 4.0) {
            break;
        }
        dist += len.w-0.0001;
        p = start+dir*dist;
    }
    
    if (len.w < 0.001) {
		gl_FragColor = vec4(vec3(equal(len.xyz,vec3(len.w))),1.0);
    }
}

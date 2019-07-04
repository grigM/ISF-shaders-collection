/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54443.0"
}
*/


//precision mediump float;

const float EPS = 0.001;

float sceneDist(vec3 p) {
    vec3 q = p * 10.0;
    return length(p) - 1.0 + 13.97*sin(q.x)*sin(q.y)+sin(q.z);
}

vec3 genNormal(vec3 p) {
    return normalize(vec3(
        sceneDist(p + vec3(EPS, 0.0, 0.0)) - sceneDist(p + vec3(-EPS, 0.0, 0.0)),
        sceneDist(p + vec3(0.0, EPS, 0.0)) - sceneDist(p + vec3(0.0, -EPS, 0.0)),
        sceneDist(p + vec3(0.0, 0.0, EPS)) - sceneDist(p + vec3(0.0, 0.0, -EPS))
    ));
}

void main( void ) {
    vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
    vec3 rayDir = normalize(vec3(uv.xy, -1.0));

    vec3 camPos = vec3(0.2*sin(TIME/3.0), 0.2*cos(TIME/2.0), 15);
    float dist = 0.0;
    vec3  distPos = camPos;
    for(int i = 0; i < 32; i++){
        distPos += dist * rayDir;
        dist = sceneDist(distPos);
        if (dist < EPS) break;
    }

    vec3 color = vec3(0);
    if(dist < EPS){
        vec3 normal = genNormal(distPos);
        color = vec3((normal+1.0)/2.0);
    }
    gl_FragColor = vec4(color, 1.0);
}
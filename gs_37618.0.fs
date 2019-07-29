/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#37618.0"
}
*/


//This is extracted from one of DestroyThingsBeautiful's QTZ Packages
//I don't know where or when I found it.
//It's my absolute favourite shader of all times


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



// Tweaked by T21 : 3d noise

float rand(vec3 n, float res)
{
  n = floor(n*res+.5);
  return fract(sin((n.x+n.y*1e2+n.z*1e4)*1e-4)*1e5);
}

float map( vec3 p )
{
    p = mod(p,vec3(1.0, 1.0, 1.0))-0.5;
    return length(p.xy)-.1;
}

void main( void )
{
    vec2 pos = (gl_FragCoord.xy*2.0 - RENDERSIZE.xy) / RENDERSIZE.y;
    vec3 camPos = vec3(1., 1., 1.);
    vec3 camTarget = vec3(0.0, 0.0, 0.0);

    vec3 camDir = normalize(camTarget-camPos);
    vec3 camUp  = normalize(vec3(0.0, 1.0, 0.0));
    vec3 camSide = cross(camDir, camUp);
    float focus = 2.0;

    vec3 rayDir = normalize(camSide*pos.x + camUp*pos.y + camDir*focus);
    vec3 ray = camPos;
    float d = 0.0, total_d = 0.0;
    const int MAX_MARCH = 100;
    const float MAX_DISTANCE = 5.0;
    float c = 1.0;
    for(int i=0; i<MAX_MARCH; ++i) {
        d = map(ray);
        total_d += d;
        ray += rayDir * d;
        if(abs(d)<0.001) { break; }
        if(total_d>MAX_DISTANCE) { c = 0.; total_d=MAX_DISTANCE; break; }
    }
	
    float fog = 5.0;
    vec4 result = vec4( vec3(1.,.98,.9) * c * (fog - total_d) / fog, 1.0 );
    ray.z += 0.5*TIME*cos(floor(ray.x*3.+ray.y*2.));
    float r = rand(ray, 22.);
    gl_FragColor = result*(step(r,.3)+r*.2+.1);
}
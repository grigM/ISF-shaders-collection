/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#46811.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


vec3 rotatex(in vec3 p, float ang) {    
    return vec3(p.x, p.y*cos(ang) - p.z*sin(ang), p.y*sin(ang) + p.z*cos(ang)); 
}
vec3 rotatey(in vec3 p, float ang) {    
    return vec3(p.x*cos(ang) - p.z*sin(ang), p.y, p.x*sin(ang) + p.z*cos(ang)); 
}
vec3 rotatez(in vec3 p, float ang) {    
    return vec3(p.x*cos(ang) - p.y*sin(ang), p.x*sin(ang) + p.y*cos(ang), p.z); 
}
float scene(vec3 p)
{
    p = rotatey(p, 0.3*TIME);
    p = rotatex(p, 0.2*TIME);
    p = rotatez(p, 0.4*TIME);
    float d0 = length(max(abs(p) - 0.5, 0.0))- 0.01 + clamp(sin((p.x+p.y+p.z)*20.0)*0.03, 0.0, 1.0); 
    float d1 = length(p) - 0.5; 
    return mix(d0,d1, sin(TIME));
//  return sin(min(d0,d1)*1.6); 
//  return length(p) - 1.0; 
}

vec3 get_normal(vec3 p)
{
    vec3 eps = vec3(0.01, 0.0, 0.0); 
    float nx = scene(p + eps.xyy) - scene(p - eps.xyy); 
    float ny = scene(p + eps.yxy) - scene(p - eps.yxy); 
    float nz = scene(p + eps.yyx) - scene(p - eps.yyx); 
    return normalize(vec3(nx,ny,nz)); 
}
void main( void ) 
{
    vec3 color = vec3(0); 
    vec2 p = 2.0*gl_FragCoord.xy/RENDERSIZE - 1.0; 
    p.x *= RENDERSIZE.x/RENDERSIZE.y; 

    if (abs(p.y) < 0.7) {
    vec3 ro = vec3(0.0, 0.0, 2.0); 
    vec3 rd = normalize(vec3(p.x, p.y, -1.4)); 
    
    color = (1.0 - vec3(length(p*0.5)))*0.2;   

    vec3 pos = ro; 
    float dist = 0.0; 
    for (int i = 0; i < 64; i++) {
        float d = scene(pos); 
        pos += rd*d; 
        dist += d; 
    }
    
    if (dist < 100.0) {
        vec3 n = get_normal(pos);
        vec3 r = reflect(normalize(pos - ro),n); 
        vec3 h = -normalize(n + pos - ro ); 
        float diff  = 1.0*clamp(dot(n, normalize(vec3(1,1,1))), 0.0, 1.0); 
        float diff2 = 0.2*clamp(dot(n, normalize(vec3(0.7,-1,0.5))), 0.0, 1.0); 
        float diff3 = 0.1*clamp(dot(n, normalize(vec3(-0.7,-0.4,0.7))), 0.0, 1.0); 
        //float spec = pow(clamp(dot(r, normalize(vec3(1,1,1))), 0.0, 1.0), 50.0); 
        float spec = pow(clamp(dot(h, normalize(vec3(1,1,1))), 0.0, 1.0), 50.0); 
        float amb = 0.2 + pos.y; 
        color = diff*vec3(1,1,1) + diff2*vec3(1,0,0)  + diff3*vec3(1,0,1) + spec*vec3(1,1,1)  + amb*vec3(0.2,0.2,0.2); 
        color /= dist;
    }
    }
    
    gl_FragColor = vec4(color, 4.0); 
}
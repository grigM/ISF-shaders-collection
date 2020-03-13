/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#56788.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/3tfSWs
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
float iTime = 0.0;
#define iResolution RENDERSIZE
const vec4 iMouse = vec4(0.0);

// Protect glslsandbox uniform names
#define TIME        stemu_time

// --------[ Original ShaderToy begins here ]---------- //

#define MIN_FLOAT 1e-6
#define MAX_FLOAT 1e6

struct Box{ vec3 origin; vec3 bounds;};
struct Ray{ vec3 origin, dir;};
struct HitRecord{ float t[2]; vec3 p[2];};
    
vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
    vec2 xy = fragCoord - size / 2.;
    float z = size.y / tan(radians(fieldOfView) / 2.);
    return normalize(vec3(xy, -z));
}

mat4 viewMatrix(vec3 eye, vec3 center, vec3 up) {
    vec3 f = normalize(center - eye),
         s = normalize(cross(f, up)),
         u = cross(s, f);
    return mat4(vec4(s, 0.), vec4(u, 0.), vec4(-f, 0.), vec4(vec3(0.), 1.));
}

mat3 calcLookAtMatrix(in vec3 camPosition, in vec3 camTarget, in float roll) {
  vec3 ww = normalize(camTarget - camPosition);
  vec3 uu = normalize(cross(ww, vec3(sin(roll), cos(roll), 0.0)));
  vec3 vv = normalize(cross(uu, ww));

  return mat3(uu, vv, ww);
}

float fbm1x(float x, float TIME){
	float amplitude = 1.;
    float frequency = 1.;
    float y = sin(x * frequency);
    float t = 0.01*(-TIME * 130.0);
    y += sin(x*frequency*2.1 + t)*4.5;
    y += sin(x*frequency*1.72 + t*1.121)*4.0;
    y += sin(x*frequency*2.221 + t*0.437)*5.0;
    y += sin(x*frequency*3.1122+ t*4.269)*2.5;
    y *= amplitude*0.06;
    return y;
}

float noise3D(vec3 p){
    return fract(sin(dot(p ,vec3(12.9898,78.233,128.852))) * 43758.5453)*2.0-1.0;
}

float simplex3D(vec3 p){
    float f3 = 1.0/3.0;
    float s = (p.x+p.y+p.z)*f3;
    int i = int(floor(p.x+s));
    int j = int(floor(p.y+s));
    int k = int(floor(p.z+s));
    
    float g3 = 1.0/6.0;
    float t = float((i+j+k))*g3;
    float x0 = float(i)-t;
    float y0 = float(j)-t;
    float z0 = float(k)-t;
    x0 = p.x-x0;
    y0 = p.y-y0;
    z0 = p.z-z0;
    int i1,j1,k1;
    int i2,j2,k2;
    if(x0>=y0)
    {
        if      (y0>=z0){ i1=1; j1=0; k1=0; i2=1; j2=1; k2=0; } // X Y Z order
        else if (x0>=z0){ i1=1; j1=0; k1=0; i2=1; j2=0; k2=1; } // X Z Y order
        else            { i1=0; j1=0; k1=1; i2=1; j2=0; k2=1; } // Z X Z order
    }
    else 
    { 
        if      (y0<z0) { i1=0; j1=0; k1=1; i2=0; j2=1; k2=1; } // Z Y X order
        else if (x0<z0) { i1=0; j1=1; k1=0; i2=0; j2=1; k2=1; } // Y Z X order
        else            { i1=0; j1=1; k1=0; i2=1; j2=1; k2=0; } // Y X Z order
    }
    float x1 = x0 - float(i1) + g3; 
    float y1 = y0 - float(j1) + g3;
    float z1 = z0 - float(k1) + g3;
    float x2 = x0 - float(i2) + 2.0*g3; 
    float y2 = y0 - float(j2) + 2.0*g3;
    float z2 = z0 - float(k2) + 2.0*g3;
    float x3 = x0 - 1.0 + 3.0*g3; 
    float y3 = y0 - 1.0 + 3.0*g3;
    float z3 = z0 - 1.0 + 3.0*g3;            
    vec3 ijk0 = vec3(i,j,k);
    vec3 ijk1 = vec3(i+i1,j+j1,k+k1);   
    vec3 ijk2 = vec3(i+i2,j+j2,k+k2);
    vec3 ijk3 = vec3(i+1,j+1,k+1);       
    vec3 gr0 = normalize(vec3(noise3D(ijk0),noise3D(ijk0*2.01),noise3D(ijk0*2.02)));
    vec3 gr1 = normalize(vec3(noise3D(ijk1),noise3D(ijk1*2.01),noise3D(ijk1*2.02)));
    vec3 gr2 = normalize(vec3(noise3D(ijk2),noise3D(ijk2*2.01),noise3D(ijk2*2.02)));
    vec3 gr3 = normalize(vec3(noise3D(ijk3),noise3D(ijk3*2.01),noise3D(ijk3*2.02)));
    float n0 = 0.0;
    float n1 = 0.0;
    float n2 = 0.0;
    float n3 = 0.0;
    float t0 = 0.5 - x0*x0 - y0*y0 - z0*z0;
    if(t0>=0.0)
    {
        t0*=t0;
        n0 = t0 * t0 * dot(gr0, vec3(x0, y0, z0));
    }
    float t1 = 0.5 - x1*x1 - y1*y1 - z1*z1;
    if(t1>=0.0)
    {
        t1*=t1;
        n1 = t1 * t1 * dot(gr1, vec3(x1, y1, z1));
    }
    float t2 = 0.5 - x2*x2 - y2*y2 - z2*z2;
    if(t2>=0.0)
    {
        t2 *= t2;
        n2 = t2 * t2 * dot(gr2, vec3(x2, y2, z2));
    }
    float t3 = 0.5 - x3*x3 - y3*y3 - z3*z3;
    if(t3>=0.0)
    {
        t3 *= t3;
        n3 = t3 * t3 * dot(gr3, vec3(x3, y3, z3));
    }
    return 96.0*(n0+n1+n2+n3);
}

float sdSphere( vec3 p, float s ){
    return length(p)-s;
}

float dot2( in vec2 v ) { return dot(v,v); }
float sdCone( in vec3 p, in float h, in float r1, in float r2 )
{
    vec2 q = vec2( length(p.xz), p.y );
    
    vec2 k1 = vec2(r2,h);
    vec2 k2 = vec2(r2-r1,2.0*h);
    vec2 ca = vec2(q.x-min(q.x,(q.y < 0.0)?r1:r2), abs(q.y)-h);
    vec2 cb = q - k1 + k2*clamp( dot(k1-q,k2)/dot2(k2), 0.0, 1.0 );
    float s = (cb.x < 0.0 && ca.y < 0.0) ? -1.0 : 1.0;
    return s*sqrt( min(dot2(ca),dot2(cb)) );
}

float smin1(float a, float b, float k){
    float s = max((k-abs(a-b))*(.5/k),0.);
    s*=s;
    s*=s*(2.*k);
    return min(a-s,b-s);
}

vec2 uv = vec2(0.);
float TIME = 0.;
#define LAYERS_CNT 14.
int layer(){
    return int(floor(mod(abs(uv.x*.25 - TIME - uv.y*.5), LAYERS_CNT)));
}

float stripe(){
    return smoothstep(.4975, .495, distance(fract(uv.x*.25 - TIME - uv.y*.5), .5));
}

#define MIN_DIST 0.
#define MAX_DIST 20.
#define EPSILON MIN_FLOAT
#define MAX_MARCHING_STEPS 64

float noiseInside(vec3 p){
  if(layer() > 3){
  	p.xz *= 1. + pow(abs(p.y), .5) * .3;
  	float a = p.y * 2.;
  	p.xz *= mat2(cos(a), sin(a), -sin(a), cos(a));
  }
  if(layer() > 4)
  	p = p * vec3(1., .5 + p.y * .1, 1.) - vec3(0., iTime * 1.25, 0.);
  
  return simplex3D(p);
}

float scene(vec3 p){
    vec3 mp = p;
    mp.y -= 1.62999;
    if(layer() >= 2){
        float yFactor = p.y;
        float TIME = iTime * 2.;
        mp.xz -= vec2(fbm1x(yFactor, TIME), fbm1x(yFactor + 78.233, TIME))
               * smoothstep(0., 3., yFactor);
        
    }
    if(layer() >= 1){
        return smin1( sdCone(mp, 1.5, .8, .1 ),
                      sdSphere(p, 1.), 1.);
    }else{
        return sdSphere(p, 1.);
    }
}

float march(vec3 eye, vec3 marchingDirection, float start, float end) {
    float depth = start;
    for (int i = 0; i < MAX_MARCHING_STEPS; i++) {
        float dist = scene(eye + depth * marchingDirection);
        if (dist < MIN_FLOAT) {
            return depth;
        }
        depth += dist;
        if (depth >= end) {
            return end;
        }
    }
    return end;
}


vec3 norm(vec3 p) {
    return normalize(vec3(
        scene(vec3(p.x + EPSILON, p.y, p.z)) - scene(vec3(p.x - EPSILON, p.y, p.z)),
        scene(vec3(p.x, p.y + EPSILON, p.z)) - scene(vec3(p.x, p.y - EPSILON, p.z)),
        scene(vec3(p.x, p.y, p.z  + EPSILON)) - scene(vec3(p.x, p.y, p.z - EPSILON))
    ));
}

vec3 makeClr(vec2 fragCoord){
    vec3 viewDir = rayDirection(60., iResolution.xy, fragCoord);
    float ang = (iResolution.x - iMouse.x) * .01;
    vec3 origin = vec3(7. * sin(ang), 0., 7. * cos(ang));
    mat4 viewToWorld = viewMatrix(origin, vec3(0., 1., 0.), vec3(0., 1., 0.));
    vec3 dir = (viewToWorld * vec4(viewDir, 1.0)).xyz;
    
    Ray r = Ray(origin, dir);
    
    vec3 finalColor = vec3(0.);
    float dist = march(r.origin, r.dir, MIN_DIST, MAX_DIST);
    if (dist < MAX_DIST - MIN_FLOAT) {
        if(layer() < 3){
        	vec3 p = (r.origin + dist * r.dir);
        	finalColor = norm(p) * .5;
        }else{
        	Ray trRay = Ray(r.origin + dist * r.dir, r.dir);
        
            const float stepSize = .025;
            float t = MIN_FLOAT * 20.;
            for(float i=0.; i<300.; i++){
                vec3 p = trRay.origin + trRay.dir * t;
                float s = scene(p);
                if(s > 0.)
                    break;
                float noiseVal = noiseInside(p);
                float cutOffDist = .25 * smoothstep(-.3, -.15, s);
                if(distance(noiseVal, cutOffDist) < stepSize + stepSize * 12. * cutOffDist){
                    if(layer() <= 5){
                		finalColor = vec3(pow(1. - i/300., 16.));
                        break;
                    }else if(layer() == 6){
                        finalColor += (1. - i/300.) * .15;
                    }else{
						finalColor += abs(norm(p)) * (1. - i/300.) * .15;
                    }
                }
                t += stepSize;
            }
        }
    }
    return finalColor;
}

#define AA 1
void mainImage( out vec4 fragColor, in vec2 fragCoord ){
    uv = fragCoord/iResolution.xy;
    TIME = iTime;
    fragColor = vec4(0.);
    for(int y = 0; y < AA; ++y)
        for(int x = 0; x < AA; ++x){
            fragColor.rgb += clamp(makeClr(fragCoord + vec2(x, y) / float(AA)), 0., 1.);
        }
    
    fragColor.rgb /= float(AA * AA);
    if(layer() < 7)
    	fragColor.rgb = mix(vec3(1.), fragColor.rgb, stripe());
}
// --------[ Original ShaderToy ends here ]---------- //

#undef TIME

void main(void)
{
    iTime = TIME;
    mainImage(gl_FragColor, gl_FragCoord.xy);
    gl_FragColor.a = 1.0;
}
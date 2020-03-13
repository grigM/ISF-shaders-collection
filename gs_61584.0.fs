/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#61584.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


// Necip's second shader transfer
// Origin: https://www.shadertoy.com/view/ltdGDn



float smoothing = 0.1;
float ballradius = 0.0;
float metaPow = 1.0;
float densityMin = 4.0;
float densityMax= 7.0;
float densityEvolution = 0.4;
float rotationSpeed = 0.005;
vec2 moveSpeed = vec2(0.1,0.0);
float distortion = 0.05;
float nstrenght = 1.0;
float nsize = 1.0;
vec3 lightColor = vec3(7.0,8.0,10.0);

float saturate1(float x)
{
    return clamp(x, 0.0, 1.0);
}
vec2 rotuv(vec2 uv, float angle, vec2 center)
{    
   	return mat2(cos(angle), -sin(angle), sin(angle), cos(angle)) * (uv - center) + center;
}
float hash(float n)
{
   return fract(sin(dot(vec2(n,n) ,vec2(12.9898,78.233))) * 43758.5453);  
}  

float metaBall(vec2 uv)
{
	return length(fract(uv) - vec2(0.5));
}

float metaNoiseRaw(vec2 uv, float density)
{
    float v = 0.99;
    float r0 = hash(2015.3548);
    float s0 = TIME*(r0-0.5)*rotationSpeed;
    vec2 f0 = TIME*moveSpeed*r0;
    vec2 c0 = vec2(hash(31.2), hash(90.2)) + s0;   
    vec2 uv0 = rotuv(uv*(1.0+r0*v), r0*360.0 + s0, c0) + f0;    
    float metaball0 = saturate1(metaBall(uv0)*density);
    
    for(int i = 0; i < 25; i++)
    {
        float inc = float(i) + 1.0;
    	float r1 = hash(2015.3548*inc);
        float s1 = TIME*(r1-0.5)*rotationSpeed;
        vec2 f1 = TIME*moveSpeed*r1;
    	vec2 c1 = vec2(hash(31.2*inc), hash(90.2*inc))*100.0 + s1;   
    	vec2 uv1 = rotuv(uv*(1.0+r1*v), r1*360.0 + s1, c1) + f1 - metaball0*distortion;    
    	float metaball1 = saturate1(metaBall(uv1)*density);
        
        metaball0 *= metaball1;
    }
    
    return pow(metaball0, metaPow);
}

float metaNoise(vec2 uv)
{ 
    float density = mix(densityMin,densityMax,sin(TIME*densityEvolution)*0.5+0.5);
    return 1.0 - smoothstep(ballradius, ballradius+smoothing, metaNoiseRaw(uv, density));
}

vec4 calculateNormals(vec2 uv, float s)
{
    float offsetX = nsize*s/RENDERSIZE.x;
    float offsetY = nsize*s/RENDERSIZE.y;
	vec2 ovX = vec2(0.0, offsetX);
	vec2 ovY = vec2(0.0, offsetY);
    
	float X = (metaNoise(uv - ovX.yx) - metaNoise(uv + ovX.yx)) * nstrenght;
    float Y = (metaNoise(uv - ovY.xy) - metaNoise(uv + ovY.xy)) * nstrenght;
    float Z = sqrt(1.0 - saturate1(dot(vec2(X,Y), vec2(X,Y))));
    
    float c = abs(X+Y);
	return normalize(vec4(X,Y,Z,c));
}

// void mainImage( out vec4 fragColor, in vec2 fragCoord )
void main( void ) {

	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    vec2 uv2 = uv;
    uv2.x *= RENDERSIZE.x/RENDERSIZE.y;
    uv2 *= vec2(1.0,0.75);
    uv2.y += sin(uv.x*0.5);
    uv2 += TIME*moveSpeed;

    vec2 sphereUvs = uv - vec2(0.5);
    float vign = length(sphereUvs);
    sphereUvs = (sphereUvs / (1.0 + vign))*1.5;
    
    float noise = metaNoise(uv2);
    
    vec4 n = calculateNormals(uv2, smoothstep(0.0, 0.5, 1.0 - vign));
    vec3 lDir = normalize(vec3(1.0,1.0,0.0));
    float l = max(0.0, dot(n.xyz, lDir));
      
    // vec4 tex = texture(iChannel0, sphereUvs + n.xy + TIME*moveSpeed*-0.2).xxxx*0.75;
    //tex *= 1.0 - vign;
    
    // vec3 col = mix(tex.xyz*0.75, tex.xyz+l*lightColor, noise);
    
	vec3 col = vec3(noise);
	gl_FragColor = vec4(n.w*col*5.0 + col, 1.0);
}
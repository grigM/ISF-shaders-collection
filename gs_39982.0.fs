/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39982.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define timer TIME*0.4

// -----------------------------------------------

//https://www.shadertoy.com/view/MsByDm
#define PI 3.141592653589
float sdBox( vec3 p, vec3 b ) {
  vec3 d = abs(p) - b;
  return min(max(d.x,max(d.y,d.z)),0.0) + length(max(d,0.0));
}
float sdTorus( vec3 p, vec2 t )
{
  vec2 q = vec2(length(p.xz)-t.x,p.y);
  return length(q)-t.y;
}

float opRep( vec3 p, vec3 c )
{
    //vec3 offset = vec3(0.,0.,0.);
    //c = c+offset;
    vec3 q = mod(p,c)-0.5*c;
    //return sdTorus( q , vec2(0.1, 0.05));
    
    float s = 0.1;
    //s=  (1.-fract(iDate.w*2.))*0.01 + 0.09;
//    return sdBox( q , vec3(s));//vec3(0.1, 0.1, 0.1));
    return sdTorus( q , vec2(s));//vec3(0.1, 0.1, 0.1));
}

// Raymarching
const float rayEpsilon = 0.001;
const float rayMin = 0.1;
const float rayMax = 1000.0;
const int rayCount = 64;

// Camera

vec3 right = vec3(1, 0, 0);
vec3 up = vec3(0, 1, 0);

// Colors
vec3 lightColor = vec3(1.0, 1.0, 1.0);
vec3 skyColor = vec3(0, 0, 0.1);
vec3 shadowColor = vec3(0, 0, 0);

float sphere( vec3 p, float s ) { return length(p)-s; }
float reflectance(vec3 a, vec3 b) { return dot(normalize(a), normalize(b)) * 0.5 + 0.5; }
vec2 kaelidoGrid(vec2 p) { return vec2(step(mod(p, 2.0), vec2(1.0))); }
vec3 rotateY(vec3 v, float t) { 
	float cost = cos(t); float sint = sin(t);
  	return vec3(v.x * cost + v.z * sint, v.y, -v.x * sint + v.z * cost); }
vec3 rotateX(vec3 v, float t) { 
	float cost = cos(t); float sint = sin(t);
  	return vec3(v.x, v.y * cost - v.z * sint, v.y * sint + v.z * cost); }
vec3 rotateZ(vec3 p, float angle) { 
	float c = cos(angle); float s = sin(angle);
  	return vec3(c*p.x+s*p.y, -s*p.x+c*p.y, p.z); }


vec2 rotation(vec2 p, float angle)
{
    return mat2(cos(angle), -sin(angle), sin(angle), cos(angle)) * p;
}



void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec3 eye = vec3(0, 0, -1.5);
	vec3 front = vec3(0.,0.,1.);//vec3(0, 0, 1.);//iGlobalTime+2.);   
    
    // Ray from UV
	vec2 uv = fragCoord.xy * 2.0 / RENDERSIZE.xy - 1.0;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    vec3 ray = normalize(front + right * uv.x + up * uv.y);
    
    // Color
    vec3 color = skyColor;
    
    // Animation
    float translationTime = timer * 0.5;
    
    // Raymarching
    float t = 0.0;
    for (int r = 0; r < rayCount; ++r)
    {
        //vec3 dist = float(r)*0.1*vec3(sin(iDate.w)*0.01,0., 0.);
        // Ray Position
        vec3 p = eye + ray * t;
        p.xy = rotation(p.xy, t*0.8*sin(timer));
        p.yz = rotation(p.yz, t*0.08*sin(timer));
        p.z = p.z + timer*1.5;
        p.y = p.y + sin(timer*.2)*3.;
        
        // Distance to Sphere
        float d;// = sphere(p, 0.5);
        d = opRep(p,vec3(0.5,0.5,0.5));
        
        // Distance min or max reached
        if (d < rayEpsilon || t > rayMax)
        {
            // Shadow from ray count
            color = mix(lightColor, shadowColor, float(r) / float(rayCount));
            
            // Sky color from distance
            color = mix(color, skyColor, smoothstep(rayMin, rayMax, t));
            break;
        }
        
        // Distance field step
        t += d;
    }
    
    // Hop
	fragColor = vec4(color, 1);
}

// -----------------------------------------------

void main( void ) {
	
	mainImage( gl_FragColor, gl_FragCoord.xy );
}
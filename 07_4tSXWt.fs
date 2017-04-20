/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex04.jpg"
    },
    {
      "NAME" : "iChannel1",
      "PATH" : [
        "cube02_0.jpg",
        "cube02_1.jpg",
        "cube02_2.jpg",
        "cube02_3.jpg",
        "cube02_4.jpg",
        "cube02_5.jpg"
      ],
      "TYPE" : "cube"
    }
  ],
  "CATEGORIES" : [
    "raymarch",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tSXWt by yiwenl.  07",
  "INPUTS" : [

  ]
}
*/


vec2 rotate(vec2 pos, float angle) {
	float c = cos(angle);
	float s = sin(angle);

	return mat2(c, s, -s, c) * pos;
}

float smin( float a, float b, float k )
{
    float res = exp( -k*a ) + exp( -k*b );
    return -log( res )/k;
}

float smin( float a, float b )
{
    return smin(a, b, 3.0);
}

float box( vec3 p, vec3 b ) {
  vec3 d = abs(p) - b;
  return min(max(d.x,max(d.y,d.z)),0.0) +
         length(max(d,0.0));
}

float box(vec3 p, float b) {
	return box(p, vec3(b));
}
float iSphere(vec3 pos, float radius) {
    return length(pos) - radius;
}


float map(vec3 pos) {
    pos.xz = rotate(pos.xz, sin(TIME*.1)*.5);
	pos.yz = rotate(pos.yz, cos(TIME*.15)*.5);
    float dBox = box(pos, 2.0);
    
    float s1 = box(pos - vec3(sin(TIME*.55)*1.85, cos(TIME*.19) * 0.95, sin(TIME*.91) * 1.21) * 1.0, 2.6451);
    float s2 = box(pos - vec3(cos(TIME*.43)*1.55, sin(TIME*.38) * 1.12, cos(TIME*.76) * 1.67) * 1.4, 2.564821);
    float s3 = box(pos - vec3(sin(TIME*.26)*2.52, cos(TIME*.57) * 0.56, sin(TIME*.12) * 1.58) * 1.2, 2.98441);
    float s4 = box(pos - vec3(sin(TIME*.97)*1.72, sin(TIME*.22) * 0.81, cos(TIME*.34) * 0.97) * 1.5, 2.12373);
    float s5 = box(pos - vec3(sin(TIME*.62)*1.47, cos(TIME*.76) * 0.73, sin(TIME*.75) * 1.45) * 1.7, 2.2748186);
    
    return smin(s1, smin(s2, smin(s3, smin(s4, s5))));
}

const float PI = 3.141592657;
const vec3 lightDirection = vec3(1.0, 1.0, -1.0);
const vec4 lightBlue = vec4(186.0, 209.0, 222.0, 255.0)/255.0;

float diffuse(vec3 normal) {
    return max(dot(normal, normalize(lightDirection)), 0.0);   
}

float specular(vec3 normal, vec3 dir) {
	vec3 h = normalize(normal - dir);
	return pow(max(dot(h, normal), 0.0), 40.0);
}

vec3 envLight(vec3 normal, vec3 dir) {
	vec3 eye = -dir;
	vec3 r = reflect( eye, normal );
    float m = 2. * sqrt( pow( r.x, 2. ) + pow( r.y, 2. ) + pow( r.z + 1., 2. ) );
    vec3 color = textureCube( iChannel1, r ).rrr;
    return color;
}

float ao( in vec3 pos, in vec3 nor ){
	float occ = 0.0;
    float sca = 1.0;
    for( int i=0; i<5; i++ )
    {
        float hr = 0.01 + 0.06*float(i)/4.0;
        vec3 aopos =  nor * hr + pos;
        float dd = map( aopos );
        occ += -(dd-hr)*sca;
        sca *= 0.95;
    }
    return clamp( 1.0 - 3.0*occ, 0.0, 1.0 );    
}


float softshadow( in vec3 ro, in vec3 rd, in float mint, in float tmax ) {
	float res = 1.0;
    float t = mint;
    for( int i=0; i<16; i++ ) {
		float h = map( ro + rd*t );
        res = min( res, 8.0*h/t );
        t += clamp( h, 0.02, 0.10 );
        if( h<0.001 || t>tmax ) break;
    }
    return clamp( res, 0.0, 1.0 );
}

vec3 getColor(vec3 pos, vec3 normal, vec3 dir) {
    vec3 color = envLight(normal, dir);
    float _ao = ao(pos, normal);
    vec3  lig     = normalize( vec3(1.0, 1.0, -1.0) );
	float shadow  = softshadow(pos, lig, 0.02, 2.5 );
    shadow = mix(shadow, 1.0, .5);
    return color * _ao * shadow;
}

vec3 computeNormal(vec3 pos) {
	vec2 eps = vec2(0.01, 0.0);

	vec3 normal = vec3(
		map(pos + eps.xyy) - map(pos - eps.xyy),
		map(pos + eps.yxy) - map(pos - eps.yxy),
		map(pos + eps.yyx) - map(pos - eps.yyx)
	);
	return normalize(normal);
}



void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv = -1.0 + uv * 2.0;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    
    float focus = 1.25;
    vec3 pos = vec3(0.0, 0.0, -10.0);
    vec3 dir = normalize(vec3(uv, focus));
    
    vec4 color = vec4(.0);
    float d;
    bool hit = false;
    const int NUM_ITER = 64;
    for(int i=0; i<NUM_ITER; i++) {
        d = map(pos);
        if(d < 0.0001) {
            hit = true;
        }
        
        pos += d * dir;
        if(length(pos) > 10.0) break;
    }
    
    if(hit) {
		vec3 normal = computeNormal(pos);
		color.rgb = getColor(pos, normal, dir);
		color.a = 1.0;
    }
    
    color = color;
    
	gl_FragColor = vec4(color);
}
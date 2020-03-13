/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wd3SR7 by ankd.  I try to render metaball with webcam capture image.",
  "INPUTS" : [
	
	{
      "TYPE" : "image",
      "NAME" : "inputImage"
    },
    {
      
      "NAME" : "drawBack",
      "TYPE" : "bool",
      "DEFAULT": 0
    },
    {
			"NAME": "drawBackOpacity",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
    {
			"NAME": "ball_move_speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 5.0
		},
		
		{
			"NAME": "rot_speed",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 1.0
		},
		
    {
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 10.0
		},
		
		{
			"NAME": "BALL_NUM",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": 1.0,
			"MAX": 50.0
		}
		,
		
		{
			"NAME": "smooth_union",
			"TYPE": "float",
			"DEFAULT": 0.75,
			"MIN": 0.0,
			"MAX": 2.0
		}
		
  ]
}
*/


const float PI = 3.14159265359;
const float HALF_PI = 0.5*PI;
const float TWO_PI = 2.0*PI;

float hash(in float v) { return fract(sin(v)*43237.5324); }
vec3 hash3(in float v) { return vec3(hash(v), hash(v*99.), hash(v*9999.)); }

float sphere(in vec3 p, in float r) { return length(p)-r; }
float opSmoothUnion( float d1, float d2, float k ) {
    float h = clamp( 0.5 + 0.5*(d2-d1)/k, 0.0, 1.0 );
    return mix( d2, d1, h ) - k*h*(1.0-h);
}

//#define BALL_NUM 10
float map(in vec3 p) {
  float res = 1e5;
  for(int i=0;i<int(BALL_NUM);i++) {
    float fi = float(i)+1.;
    float r = 0.+1.5*hash(fi);
    vec3 offset = 2.*sin(hash3(fi)*(TIME*ball_move_speed));
    res = opSmoothUnion(res, sphere(p-offset, r), smooth_union);
  }
  return res;
}

vec3 normal(in vec3 p) {
	vec2 e = vec2(1., -1.)*1e-3;
    return normalize(
    	e.xyy * map(p+e.xyy)+
    	e.yxy * map(p+e.yxy)+
    	e.yyx * map(p+e.yyx)+
    	e.xxx * map(p+e.xxx)
    );
}

mat3 lookAt(in vec3 eye, in vec3 tar, in float r) {
	vec3 cz = normalize(tar - eye);
    vec3 cx = normalize(cross(cz, vec3(sin(r), cos(r), 0.)));
    vec3 cy = normalize(cross(cx, cz));
    return mat3(cx, cy, cz);
}

void main() {



    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    vec2 p = (gl_FragCoord.xy*2. - RENDERSIZE.xy) / min(RENDERSIZE.x, RENDERSIZE.y);
    vec4 color = vec4(0.);
    vec3 ro = 5.*vec3(cos((TIME*rot_speed)*1.1), 0., sin((TIME*rot_speed)*1.1));
    //ro = vec3(0., 0., 5.);
    vec3 rd = normalize(lookAt(ro, vec3(0.), 0.) * vec3(p,  zoom));
    
    vec2 tmm = vec2(0., 10.);
    float t = 0.;
    for(int i=0;i<200;i++) {
        float tmp = map(ro + rd*t);
        if(tmp<0.001 || tmm.y<t) break;
        t += tmp*0.7;
    }
  
    if(tmm.y<t) {// background
        color = vec4(0.);
        if(drawBack){
        	
        	color = vec4(IMG_NORM_PIXEL(inputImage, uv).rgb, drawBackOpacity);
        }
    } else {// object
        vec3 pos = ro + rd*t;
        vec3 nor = normal(pos);
        vec3 ref = reflect(rd, nor);
        vec2 texCoord = ref.xy*0.5+0.5;
        
        
        
        color = vec4(IMG_NORM_PIXEL(inputImage, texCoord).rgb, 1.0);
        color += vec4(vec3(pow(1.-clamp(dot(-rd, nor), 0., 1.), 2.)), 1.0);
    }
    gl_FragColor = color;
}

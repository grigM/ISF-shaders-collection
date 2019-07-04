/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex01.jpg"
    }
  ],
  "CATEGORIES" : [
    "fullmoon",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llj3Dm by shezard.  Some nightish scene",
  "INPUTS" : [
  		{
			"NAME": "noise_apm",
			"TYPE": "float",
			"DEFAULT": 1.5,
			"MIN": 0,
			"MAX": 2.0
		},
		{
			"NAME": "mountain_vis",
			"TYPE": "float",
			"DEFAULT": 0.95,
			"MIN": 0,
			"MAX": 1.1
		},
		{
			"NAME": "star_vis",
			"TYPE": "float",
			"DEFAULT": 40.0,
			"MIN": 40,
			"MAX": 60.0
		},
		{
			"NAME": "moon_size",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": 0,
			"MAX": 1.0
		},

  ]
}
*/


float rand(in vec2 uv){
    return fract(sin(dot(uv.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

float noise1d(in float p) {
  	float fl = floor(p);
    float fc = fract(p);
    
  	return mix(rand(vec2(fl,fl)),rand(vec2(fl,fl)+1.0),fc);   
}

float noise(in vec2 uv) {
	return sin(noise_apm*uv.x)*sin(noise_apm*uv.y);
}

const mat2 m = mat2( 0.80,  0.60, -0.60,  0.80 );

float fbm1d(in float p) {
        float f = 0.0;
    f += 0.5000*noise1d(p); p = p*2.02;
    f += 0.2500*noise1d(p); p = p*2.03;
    f += 0.1250*noise1d(p); p = p*2.01;
    f += 0.0625*noise1d(p);
    return f/0.9375;
}

float fbm(vec2 uv) {
    float f = 0.0;
    f += 0.5000*noise(uv+TIME*.1); uv = m*uv*2.02;
    f += 0.2500*noise(uv+TIME*.1); uv = m*uv*2.03;
    f += 0.1250*noise(uv+TIME*.1); uv = m*uv*2.01;
    f += 0.0625*noise(uv+TIME*.1);
    return f/0.9375;
}

float fbm2(in vec2 uv) {
   vec2 p = vec2(fbm(uv + vec2(0.0,0.0)),
                 fbm(uv + vec2(5.2,1.3)));

   return fbm(uv + 4.0*p);
}

float xRatio = RENDERSIZE.x/RENDERSIZE.y;

float r;

vec3 mountain(in vec2 p, in vec2 offset) {
     
    float c = 0.0;
    if(-p.y > fbm1d(p.x)) {
        
       float mountain = (1.0 - (ceil(-p.y) * -p.y)) * length(p-offset) * .125;
    
       c = max(c,mountain);
    }
    
    return c * (mountain_vis + .05 * IMG_NORM_PIXEL(iChannel0,mod(p*.5,1.0)).rgb);
}

vec3 scene( in vec2 p) {
    
    vec2 offset = vec2(1.3,-.6);
    
    vec3 c = vec3(.9) * length(p+offset) + fbm2(p*20.0+20.0)*.0025; 
    
    	 c = clamp(c,.0,moon_size) / length(p+offset) - fbm2(p+vec2(TIME*.1,0))*.025;
        
    float stars = (1.0 - clamp(0.0,0.025,r*10.0) * star_vis) * .4 * (p.y * 3.0 + 1.0) * (.25 + fbm2(p*200.0)) * ceil(p.y);
    
    c = max(c, stars);
    
    c = max(c,mountain(p, offset));
    
    c = max(c,mountain(p+vec2(-0.4,0.1), offset+vec2(-0.4,0.1)));
    
    c = max(c,mountain(p+vec2(-0.55,0.2), offset+vec2(-0.55,0.2)));
    
    return c;
}

void main(){
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    vec2 p = -1.0 + 2.0 * uv;
    	 p.x *= xRatio;

    	 r = rand(uv);
    
    vec3 c = scene(p);
    
	gl_FragColor = vec4(c,1.0);
}
/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
	{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 6.0
	},
	{
			"NAME": "count",
			"TYPE": "float",
			"DEFAULT": 32.0,
			"MIN": 0.0,
			"MAX": 80.0
	},
	{
			"NAME": "period",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 6.0
	},
	{
			"NAME": "amo",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": 5.0,
			"MAX": 30.0
	},
	
	{
			"NAME": "phase_shift",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 0.2
	},
	
	{
			"NAME": "lines_blur",
			"TYPE": "float",
			"DEFAULT": 0.01,
			"MIN": 0.01,
			"MAX": 2.0
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35194.0"
}
*/




float d2y(float d){ d*= 400.; return 1./(d*d);}

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

float gauss(float s, float x){
    return (0.85)*exp(-x*x/(2.*s*s));
}
#if 1
float blur(float dist, float width, float blur, float intens){
    float w = width;
    float e = 0.85*blur;
    dist*=0.75;
    float b = smoothstep(-w-e, -w+e, dist)-smoothstep(w-e, w+e, dist);
    return 1.0*pow(b,1.9)*(1.+80.*blur)*intens;
    //return 0.9*b*intens;
}
#else
float blur(float dist, float width, float blur, float intens){
    float w = width;
    dist = max(abs(dist)-width,0.);
    float b = gauss(0.1+w*10.*blur,dist);
    return b*intens;
}
#endif
float d2y2(float d, float i){
    float b = 0.04*i+0.0001;
    return blur(d , 0.03, b, 0.4);
}



float f(float x){
    return blur(0.5*x, 0.01, 0.4+0.9, 1.);
}


//#define N 32
// hauteur de la vague
float wave(float x, int i){
    float i_f=float(i);
    float fy = (amo-0.1/i_f)*sin(x*period+2.8*(TIME*speed)+phase_shift*i_f);
    return fy * (0.4+0.3*cos(x));
}

void main(void)
{
    vec2 uv = (gl_FragCoord.xy / RENDERSIZE - vec2(0.5)) * vec2(RENDERSIZE.x / RENDERSIZE.y, 1.0) * 2.0;
    uv.y *= 8.;
    uv.x *= 1.5;

	float yf = 0.*d2y(distance(uv.y*2., f(uv.x)));
    vec3 col = vec3(1.);
    for(int i = 0; i<int(count); ++i){
        float i_f = float(i)*lines_blur+0.1;
        float y = d2y2(distance(2.*uv.y, wave(uv.x, i)),i_f);
        col -= 1.0*y ;
        
        
    }
    
    gl_FragColor = vec4(vec3(yf)+(222./255.)-col, 1);
}
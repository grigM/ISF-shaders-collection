/*{
	"DESCRIPTION": "",
	"CREDIT": "",
	"ISFVSN": "2",
	"CATEGORIES": [
		"XXX"
	],
	"INPUTS": [
		{
			"NAME": "scale_w",
			"TYPE": "float",
			"DEFAULT": 20.0,
			"MIN": 2.0,
			"MAX": 200.0
		},
		{
			"NAME": "scale_h",
			"TYPE": "float",
			"DEFAULT": 20.0,
			"MIN": 2.0,
			"MAX": 200.0
		},
				
		{
			"NAME": "scroll_speed",
			"TYPE": "float",
			"DEFAULT": 0.33,
			"MIN": -1.0,
			"MAX": 1.0
		},
		
		{
			"NAME": "peak_speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 3.0
		},
		
		{
			"NAME": "colorize",
			"TYPE": "bool",
			"DEFAULT": 0.0
		},
	]
	
}*/


#define PI 3.14159265359
#define HALF_PI 1.57079632675
#define TWO_PI 6.283185307

//uniform vec2 u_resolution;
//uniform vec2 u_mouse;
//uniform float u_time;


vec3 palette(float t,vec3 a,vec3 b,vec3 c,vec3 d )
{
    return a + b * cos( TWO_PI * (c*t+d));
}

float random(float x)
{
    return fract(sin(x*100.00)*10000.0);
}

float random(vec2 st)
{
    return fract(
        dot(
            sin(st.x*100.00),
            sin(st.y*100.00)        
        )*10000.0);
}

float smoothCellRandom(float x,float scale)
{   
    float iPos = floor(x*scale);
    float fPos = fract(x*scale);

    return mix(random(iPos), random(iPos + 1.0), smoothstep(0.,1.,fPos));
}

float peakCellRandom(float x,float scale)
{   
    float iPos = floor(x*scale);
    float fPos = fract(x*scale);

    return mix(random(iPos), random(iPos + 1.0), fPos);
}

float cellRandom(float x,float scale)
{   
    float iPos = floor(x*scale);
    float fPos = fract(x*scale);

    return random(iPos);
}

void main()	{
	
	
	
	 vec2 st = gl_FragCoord.xy / RENDERSIZE;

    vec3 color = vec3(0.0);
    //    color.x = abs(sin(u_time));
    float t = TIME * scroll_speed;
    
    float cell = cellRandom(st.x+t,scale_w);
    float r = floor(cell*st.y*scale_h);
    float f = st.y * abs(sin( cell*1000.0 + (TIME*peak_speed)));
    r *= f;
    r = 1.0 - step(1.0,r);
    r += 1.0-f;

    float d = smoothCellRandom(st.x+t,scale_w);
    // d *= r;
    d = r;

    vec3 a = vec3(0.5,0.5,0.5);
    vec3 b = vec3(0.5,0.5,0.5);
    vec3 c = vec3(0.5,.5,0.5);
	if(colorize){
		c = vec3(0.0,.1,0.2);
	}

    color = palette(d+fract(t)*2.0,vec3(0.5),a,b,c);
    // color += d;

    gl_FragColor = vec4(color,1.0);
	
	
	
}

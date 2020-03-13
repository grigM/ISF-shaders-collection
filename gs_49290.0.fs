/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
		{
			"NAME": "AMOUNT_1",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 50.0
			
		},
		{
			"NAME": "AMOUNT_2",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 50.0
			
		},
		{
			"NAME": "AMOUNT_3",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 50.0
			
		},
		{
			"NAME": "show_1",
			"TYPE": "bool",
			"DEFAULT": true
			
			
		},
		{
			"NAME": "show_2",
			"TYPE": "bool",
			"DEFAULT": true
			
			
		},
		{
			"NAME": "show_3",
			"TYPE": "bool",
			"DEFAULT": true
			
			
		},
		
		{
			"NAME": "amp",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 3.0
			
		},
		
		{
			"NAME": "rot",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -2.0,
			"MAX": 2.0
			
		},
		
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 3.0
			
		},
		
		
		{
			"NAME": "x_pos_1",
			"TYPE": "float",
			"DEFAULT": -0.7,
			"MIN": -2.0,
			"MAX": 2.0
			
		},
		{
			"NAME": "x_pos_2",
			"TYPE": "float",
			"DEFAULT": 0.4,
			"MIN": -2.0,
			"MAX": 2.0
			
		},
		{
			"NAME": "x_pos_3",
			"TYPE": "float",
			"DEFAULT": 1.4,
			"MIN": -2.0,
			"MAX": 2.0
			
		},
		{
			"NAME": "color_r",
			"TYPE": "float",
			"DEFAULT": 0.175,
			"MIN": 0.0,
			"MAX": 3.0
			
		},
		{
			"NAME": "color_g",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 3.0
			
		},
		{
			"NAME": "color_b",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 3.0
			
		},
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#49290.0"
}
*/


//precision lowp float;



const float count = 0.0;

float Hash( vec2 p, in float s ){
    return fract(sin(dot(vec3(p.xy,10.0 * abs(sin(s))),vec3(27.1,61.7, 12.4)))*273758.5453123);
}

float noise(in vec2 p, in float s)
{
    vec2 i = floor(p);
    vec2 f = fract(p);
    f *= f * (3.0-2.0*f);
    return mix(mix(Hash(i + vec2(0.,0.), s), Hash(i + vec2(1.,0.), s),f.x),mix(Hash(i + vec2(0.,1.), s), Hash(i + vec2(1.,1.), s),f.x),f.y) * s;
}

float fbm(vec2 p)
{
     float v = 0.0;
     v += noise(p*1., 0.375)*amp;
     v += noise(p*2., 0.25)*amp;
     v += noise(p*4., 0.125)*amp;
     v += noise(p*8., 0.0625)*amp;
     return v;
}

void main( void ) 
{

	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy ) * 2.0 - 1.0;
	mat2 r = mat2(cos(rot),-sin(rot), 
			sin(rot), cos(rot));
        uv *= r;
        
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;

	vec3 finalColor = vec3( 0.0 );
		
		if(show_1){
		float t = abs(1.0 / (((uv.x-x_pos_1-(amp/2.0)) + fbm( uv + (TIME*speed))) * (50.0-AMOUNT_1)));
		finalColor +=  t * vec3( color_r, color_g, color_b );
		}
	
	if(show_2){
	float t_2 = abs(1.0 / (((uv.x-x_pos_2-(amp/2.0)) + fbm( uv + (TIME*speed))) * (50.0-AMOUNT_2)));
		finalColor +=  t_2 * vec3( color_r, color_g, color_b );
	}
	
	if(show_3){		
	float t_3 = abs(1.0 / (((uv.x-x_pos_3-(amp/2.0)) + fbm( uv + (TIME*speed))) * (50.0-AMOUNT_3)));
		finalColor +=  t_3 * vec3( color_r, color_g, color_b );
	}
	gl_FragColor = vec4( finalColor, 4.0 );

}
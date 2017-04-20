/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XdSGzR by ahihi.  :V",
  "INPUTS" : [{
			"NAME": "colors_count",
			"TYPE": "long",
			"VALUES": [
				5,
				4,
				3,
				2
			],
			"LABELS": [
				"5",
				"4",
				"3",
				"2"
			],
			"DEFAULT": 4
		},{
			"NAME": "n_sub",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0,
			"MAX": 4.0
		},{
			"NAME": "radius_iter_count",
			"TYPE": "float",
			"DEFAULT": 1.5,
			"MIN": 0,
			"MAX": 6.0
		},{
			"NAME": "radius_iter_speed",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0,
			"MAX": 1.0
		},{
			"NAME": "sin_val",
			"TYPE": "float",
			"DEFAULT": 50.0,
			"MIN": 0,
			"MAX": 100.0
		},{
			"NAME": "sin_speed",
			"TYPE": "float",
			"DEFAULT": 0.4,
			"MIN": 0.2,
			"MAX": 0.8
		},{
			"NAME": "sin_amplitude",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 1.0
		},{
			"NAME": "ksin_val1",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 0.6
		},{
			"NAME": "ksin_val2",
			"TYPE": "float",
			"DEFAULT": 3.0,
			"MIN": 0.0,
			"MAX": 6.0
		},{
			"NAME": "rotation_speed",
			"TYPE": "float",
			"DEFAULT": 0.04,
			"MIN": 0.0,
			"MAX": 0.2
		},{
			"NAME": "zoom_speed",
			"TYPE": "float",
			"DEFAULT": 5.00,
			"MIN": -5.0,
			"MAX": 20.0
		},{
			"NAME": "color_fade",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		
		
		{
			"NAME": "c1",
			"TYPE": "color",
			"DEFAULT": [
				1.0,
				0.0,
				0.0,
				1.0
			]
		},
		{
			"NAME": "c2",
			"TYPE": "color",
			"DEFAULT": [
				1.0,
				0.5,
				0.0,
				1.0
			]
		},
		{
			"NAME": "c3",
			"TYPE": "color",
			"DEFAULT": [
				1.0,
				1.0,
				0.0,
				1.0
			]
		}

  ]
}
*/


#define PI 3.141592653589793
#define TAU 6.283185307179586

void main()
{
	vec2 p = 2.0*(0.5 * RENDERSIZE.xy - gl_FragCoord.xy) / RENDERSIZE.xx;
	float angle = atan(p.y, p.x);
	float turn = (angle + PI) / TAU;
	float radius = sqrt(p.x*p.x + p.y*p.y);
	
	float rotation = rotation_speed * TAU * TIME;
	float turn_1 = turn + rotation;
	
	//float n_sub = 2.0;
	
	float turn_sub = mod(float(n_sub) * turn_1, float(n_sub));
	
	float k_sine = ksin_val1 * sin(ksin_val2 * TIME);
	float sine = k_sine * sin(sin_val * (pow(radius, sin_amplitude) - sin_speed * TIME));
	float turn_sine = turn_sub + sine;

	int n_colors = colors_count;
	int i_turn = int(mod(float(n_colors) * turn_sine, float(n_colors)));
	
	int i_radius = int(radius_iter_count/pow(radius*radius_iter_speed, 0.6) + zoom_speed * TIME);
		
	int i_color = int(mod(float(i_turn + i_radius), float(n_colors)));
	
	vec3 color;
	if(i_color == 0) { 
		color = vec3(1.0, 1.0, 1.0);		  
	} else if(i_color == 1) {
		color = vec3(0.0, 0.0, 0.0);	
	} else if(i_color == 2) {
		color = vec3(c1.r, c1.g, c1.b);	
	} else if(i_color == 3) {
		color = vec3(c2.r, c2.g, c2.b);	
	} else if(i_color == 4) {
		color = vec3(c3.r, c3.g, c3.b);	
	}
	
	color *= pow(radius, color_fade)*1.0;
	
	gl_FragColor = vec4(color, 1.0);
}
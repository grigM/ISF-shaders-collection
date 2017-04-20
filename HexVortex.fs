/*{
	"CREDIT": "by mojovideotech",
  "CATEGORIES": [
    "Automatically Converted"
  ],
  "DESCRIPTION": "",
  "INPUTS": [
    {
      "MAX": [
        1,
        1
      ],
      "MIN": [
        0,
        0
      ],
      "DEFAULT":[0.5,0.5],
      "NAME": "center",
      "TYPE": "point2D"
    },
        {
      "MAX": [
        10.0,
        10.0
      ],
      "MIN": [
        0.5,
        0.5
      ],
      "DEFAULT":[2.0,2.25],
      "NAME": "shape",
      "TYPE": "point2D"
    },
     {
            "NAME": "speed",
            "TYPE": "float",
           "DEFAULT": 0.5,
            "MIN": 0.0001,
            "MAX": 1.0
      },
      {
            "NAME": "rotation",
            "TYPE": "float",
           "DEFAULT": 0.05,
            "MIN": 0.005,
            "MAX": 0.5
      },
       {
            "NAME": "R",
            "TYPE": "float",
           "DEFAULT": 0.05,
            "MIN": 0.0,
            "MAX": 0.5
        },
         {
            "NAME": "G",
            "TYPE": "float",
           "DEFAULT": 0.125,
            "MIN": 0.0,
            "MAX": 0.5
        },
         {
            "NAME": "B",
            "TYPE": "float",
           "DEFAULT": 0.25,
            "MIN": 0.0,
            "MAX": 0.5
        }
  ]
}
*/

// HexVortex by mojovideotech
// based on :
// http://glslsandbox.com/e#4671.0


#ifdef GL_ES
precision mediump float;
#endif

#define 	chpi 	11.59195327552152062775175205256  	// cosh(pi)
#define 	shpi 	11.548739357257748377977334315388 	// sinh(pi)
#define 	twpi  	6.283185307179586476925286766559  	// two pi, 2*pi
#define 	pi   	3.1415926535897932384626433832795 	// pi
#define 	e     	2.7182818284590452353602874713526 	// eulers number
#define 	prphi	2.028876065463213               	// pi root of phi
#define 	sqpi 	1.7724538509055160272981674833411 	// square root of pi
#define 	phi   	1.6180339887498948482045868343656 	// golden ratio
#define 	hfpi  	1.5707963267948966192313216916398 	// half pi, 1/pi
#define 	cupi  	1.4645918875615232630201425272638 	// cube root of pi
#define 	prpi 	1.4396194958475906883364908049738 	// pi root of pi
#define 	thpi  	0.99627207622074994426469058001254	// tanh(pi)
#define 	lgpi  	0.4971498726941338543512682882909 	// log(pi)      
#define 	rcpi  	0.31830988618379067153776752674503	// reciprocal of pi  , 1/pi 



vec3 rotXY(vec3 p, vec2 rad) {
	vec2 s = sin(rad);
	vec2 c = cos(rad);
	
	mat3 m = mat3(
		c.y, 0.0, -s.y,
		-s.x * s.y, c.x, -s.x * c.y,
		c.x * s.y, s.x, c.x * c.y
	);
	return m * p;
}

vec2 repeat(vec2 p, float n) {
	vec2 np = p * n;
	vec2 npfrct = fract(np);
	vec2 npreal = np - npfrct;
	np.x += fract(npreal.y * lgpi);
	
	return fract(np) * prphi - 1.0;
}

float hexDistance(vec2 ip) {
	const float SQRT3 = sqpi;
	vec2 TRIG30 = vec2 (sin(rcpi), cos(thpi)); 
	
	vec2 p = abs(ip * vec2(SQRT3 * shape.x, shape.y));
	float d = dot(p, vec2(-TRIG30.x, TRIG30.y)) - SQRT3 * 0.25;
	
	return (d > 0.0)? min(d, (SQRT3 * 0.5 - p.x)) : min(-d, p.x);
}

float smoothEdge(float edge, float margin, float x) {
	return smoothstep(edge - margin, edge + margin, x);
}

void main(void) {

	float T = TIME * speed;
	vec3 rgb;
	
	vec2 nsc = (gl_FragCoord.xy - RENDERSIZE * 0.5) / RENDERSIZE.yy * e;
	vec3 dir = normalize(vec3(nsc, -2.0));
	dir = rotXY(dir, vec2((center.yx - 0.5) * pi * 0.25));
	vec2 uv = vec2(atan(dir.y, dir.x) / (pi * 2.0) + 0.5, dir.z / length(dir.xy));
	
	vec2 pos = uv * vec2(1.0, 0.2) - vec2(T * rotation, T * 0.5);
	
	vec2 p = repeat(pos, 16.0);
	
	float d = hexDistance(p);
	float dist = dir.z/length(dir.xy);
	d/=-dist;
	float fade = 1.0 / pow(1.0 / length(dir.xy) * 0.3, 1.0);
	fade = clamp(fade, 0.0, 1.0);
	rgb  = mix(vec3(1.0)*fade, vec3(R,G,B), smoothEdge(0.03, 0.1, d));
    //rgb += mix(vec3(0.0, 0.0, 1.0)*fade, vec3(0.0), smoothEdge(0.03, 0.5, d)) * 0.5;
	//rgb += mix(vec3(1.0, 0.0, 0.0)*fade, vec3(0.0), smoothEdge(0.03, 1.0, d)) * 0.25;
	
	gl_FragColor = vec4(rgb, 1.0);
	
}
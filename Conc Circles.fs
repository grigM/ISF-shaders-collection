/*{
	"CREDIT": "by joshpbatty",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"XXX"
	],
	"INPUTS": [
		{
			"NAME": "num_rings",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.1,
			"MAX": 5.0
		}
	]
}*/

float PI = 3.1415926;
//#define CONSTRAINED_TO_CIRCLE //as opposed to filling the whole screen
#define AA 1.0
#define AA_SAMPS 64	

//mmalex's random functions
float srand(vec2 a) { return fract(sin(dot(a,vec2(1233.224,1743.335)))); }
vec2 rand(vec2 r) { return fract(3712.65*r+0.61432); }

float easeWithSteps(float t, float steps)
{
	float frac = 1.0 / steps;	
	float eT = mod(t, frac);
	float x = eT / frac;
	return t - eT + frac * x*x*x*(x*(x*6.0 - 15.0) + 10.0); // fancy smoothstep (see wikipeed)
}

float map(float x, float xmin, float xmax, float ymin, float ymax)
{
	// miss u processing
	return clamp(ymin + (x - xmin) / (xmax - xmin) * (ymax - ymin), ymin, ymax);
}


float colorAtCoord(vec2 coord, float t)
{
	float steps = 1. + mod(floor(t), 7.0); // pattern cycling
	t = mod(t, 1.0);
	
	vec2 uv = coord.xy / RENDERSIZE.xy;
	vec2 p = uv - vec2(.5,.5);
	p.y *= RENDERSIZE.y / RENDERSIZE.x;
	
	float angle = atan(p.y, p.x);
	angle = mod( angle + PI * 2.0, PI * 2.0);
	
	float dist = length(p);
	
	float ring = floor(pow(dist*1.5,.5) * num_rings - t); // tweak!
	float ringFrac = ring / num_rings;
	
	float ringTime = map(t, ringFrac * .125, 1.0 - ringFrac * .125, 0.0, 1.0);
	ringTime = easeWithSteps(ringTime, steps); // aand tweak!
	
	float color = 0.0;//vec4 color = vec4(0.0,0.0,0.0,1.0);
	float tAngle =  PI * 2.0 * mod(ringTime * ring*1.0, 1.0);
	//if ( mod(ring, 2.0) == 0.0) tAngle = PI * 2.0 - tAngle;
	tAngle *= mod(ring, 2.0)*2.0 - 1.0;
	float si = sin(tAngle);
	float co = cos(tAngle);
	color = step(0., dot(vec2(-si,co),p)); 
	// if ((angle > tAngle && angle < tAngle + PI) || angle < tAngle - PI)
	// 	color = vec4(1.0,1.0,1.0,1.0);
	return color;
}


void main() {
		// assume 60fps
	float t = TIME * speed;
	float c=0.;
	
	// mmalex's AA/blur code.
	vec2 aa=vec2( srand(gl_FragCoord.xy), srand(gl_FragCoord.yx) );
	t+=1.0/60.0/float(AA_SAMPS)*aa.x;	
	
	for (int i=0;i<(AA_SAMPS);i++) {
		aa=rand(aa);
		c+=colorAtCoord(gl_FragCoord.xy+aa, t*.05);
		t+=1.0/60.0/float(AA_SAMPS);
	}	
	c=sqrt(c/float(AA_SAMPS));
	gl_FragColor = vec4(c);
}
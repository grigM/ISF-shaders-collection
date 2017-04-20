/*{
	"DESCRIPTION": "Your shader description",
	"CREDIT": "by you",
	"CATEGORIES": [
		"Your category"
	],
	"INPUTS": [
		{
			"LABEL": "Mouse X",
			"NAME": "mX",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"LABEL": "Mouse Y",
			"NAME": "mY",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "formuparam",
			"LABEL": "formuparam",
			"TYPE": "float",
			"DEFAULT": 0.67,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "stepsize",
			"LABEL": "stepsize",
			"TYPE": "float",
			"DEFAULT": 0.12,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "IterationLimit",
			"LABEL": "Iterations",
			"TYPE": "float",
			"DEFAULT": 9.0,
			"MIN": 1.0,
			"MAX": 20.0
		},
		{
			"NAME": "StepsLimit",
			"LABEL": "StepsLimit",
			"TYPE": "float",
			"DEFAULT": 14.0,
			"MIN": 1.0,
			"MAX": 20.0
		},
		{
			"NAME": "zoom",
			"LABEL": "zoom",
			"TYPE": "float",
			"DEFAULT": 5.71,
			"MIN": 0.0,
			"MAX": 100.0
		},
		{
			"NAME": "tile",
			"LABEL": "tile",
			"TYPE": "float",
			"DEFAULT": 1.27,
			"MIN": 0.0,
			"MAX": 10.0
		},
		{
			"NAME": "speed",
			"LABEL": "speed",
			"TYPE": "float",
			"DEFAULT": 0.01,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "XM",
			"LABEL": "XM",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "YM",
			"LABEL": "YM",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "ZM",
			"LABEL": "ZM",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "brightness",
			"LABEL": "brightness",
			"TYPE": "float",
			"DEFAULT": 0.08,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "darkmatter",
			"LABEL": "darkmatter",
			"TYPE": "float",
			"DEFAULT": 0.41,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "distfading",
			"LABEL": "distfading",
			"TYPE": "float",
			"DEFAULT": 0.64,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "saturation",
			"LABEL": "saturation",
			"TYPE": "float",
			"DEFAULT": 0.85,
			"MIN": 0.0,
			"MAX": 1.0
		}
	]
}*/

vec3 iResolution = vec3(RENDERSIZE, 1.);
float iGlobalTime = TIME;
vec2 iMouse = vec2(mX*RENDERSIZE.x, mY*RENDERSIZE.y);

// Based on: "Star Nest" by Pablo RomÃ¡n Andrioli

// This content is under the MIT License.

#define iterations 20
#define volsteps 20

//#define formuparam 0.53
//#define stepsize 0.1

//#define zoom   5.100
//#define tile   0.850
//#define speed  0.010 

//#define brightness 0.015
//#define darkmatter 0.300
//#define distfading 0.730
//#define saturation 0.850


void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	//get coords and direction
	vec2 uv=fragCoord.xy/iResolution.xy-.5;
	uv.y*=iResolution.y/iResolution.x;
	vec3 dir=vec3(uv*zoom,1.);
	float time=iGlobalTime*speed+.25;

	//mouse rotation
	float a1=.5+iMouse.x/iResolution.x*2.;
	float a2=.8+iMouse.y/iResolution.y*2.;
	mat2 rot1=mat2(cos(a1),sin(a1),-sin(a1),cos(a1));
	mat2 rot2=mat2(cos(a2),sin(a2),-sin(a2),cos(a2));
	dir.xz*=rot1;
	dir.xy*=rot2;
	vec3 from=vec3(1.,.5,0.5);
	from+=vec3(time*XM*-5.,time*YM*-5.,time*ZM*-5.);
	from.xz*=rot1;
	from.xy*=rot2;
	
	//volumetric rendering
	float s=0.1,fade=1.;
	vec3 v=vec3(0.);
	for (int r=0; r<volsteps; r++) {
		
		if (r > int (StepsLimit)) {break;}
		
		vec3 p=from+s*dir*.5;
		p = abs(vec3(tile)-mod(p,vec3(tile*2.))); // tiling fold
		float pa,a=pa=0.;
		for (int i=0; i<iterations; i++) { 
			
			if (i > int (IterationLimit)) {break;}
			
			p=abs(p)/dot(p,p)-formuparam; // the magic formula
			a+=abs(length(p)-pa); // absolute sum of average change
			pa=length(p);
		}
		float dm=max(0.,darkmatter*10.-a*a*.001); //dark matter
		a*=a*a; // add contrast
		if (r>6) fade*=1.-dm; // dark matter, don't render near
		//v+=vec3(dm,dm*.5,0.);
		v+=fade;
		v+=vec3(s,s*s,s*s*s*s)*a*brightness*fade; // coloring based on distance
		fade*=1.0-distfading; // distance fading
		s+=stepsize;
	}
	v=mix(vec3(length(v)),v,saturation); //color adjust
	fragColor = vec4(v*.01,1.);	
	
}

void main(void) {
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
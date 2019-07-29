/*{
  "CREDIT": "by mojovideotech",
  "DESCRIPTION": "",
  "CATEGORIES": [
    "generator",
    "space"
  ],
  "INPUTS": [
    {
      "NAME": "nebula",
      "TYPE": "bool",
      "DEFAULT": 0
    },
    {
      "NAME": "brightness",
      "TYPE": "float",
      "DEFAULT": 0.25,
      "MIN": 0,
      "MAX": 0.5
    },
    {
      "NAME": "zoom",
      "TYPE": "float",
      "DEFAULT": 2.5,
      "MIN": 0.25,
      "MAX": 20
    },
    {
      "NAME": "saturation",
      "TYPE": "float",
      "DEFAULT": 0.5,
      "MIN": 0,
      "MAX": 1
    },
    {
      "NAME": "distfading",
      "TYPE": "float",
      "DEFAULT": 0.25,
      "MIN": 0,
      "MAX": 0.5
    },
    {
      "NAME": "depth",
      "TYPE": "float",
      "DEFAULT": 0.25,
      "MIN": 0.1,
      "MAX": 1
    },
    {
      "NAME": "density",
      "TYPE": "float",
      "DEFAULT": 0.75,
      "MIN": 0.5,
      "MAX": 2
    },
    {
      "NAME": "morph",
      "TYPE": "float",
      "DEFAULT": 0.89,
      "MIN": 0.5,
      "MAX": 1
    },
    {
      "NAME": "rate",
      "TYPE": "float",
      "DEFAULT": 0.005,
      "MIN": -0.1,
      "MAX": 0.1
    },
    {
      "NAME": "move",
      "TYPE": "point2D",
      "DEFAULT": [
        0,
        0
      ]
    }
  ]
}*/

///////////////////////////////////////////
// SpaceIsThePlace  by mojovideotech
//
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0
//
// based on :
// shadertoy.com/\XlfGRj	by Kali
///////////////////////////////////////////
 
#define iterations 7
#define volsteps 7
#define pi   3.141592653589793 	// pi

float field(in vec3 p) {
	float strength = 7. + .03 * log(1.e-6 + fract(sin(TIME) * 4373.11));
	float accum = 0.;
	float prev = 0.;
	float tw = 0.;
	for (int i = 0; i < 7; ++i) {
		float mag = dot(p, p);
		p = abs(p) / mag + vec3(-.5, -.4, -1.5);
		float w = exp(-float(i) / 7.);
		accum += w * exp(-strength * pow(abs(mag - prev), 2.3));
		tw += w;
		prev = mag;
	}
	return max(0., 5. * accum / tw - .7);
}

void main() {
	vec2 uv=gl_FragCoord.xy/RENDERSIZE.xy-.5;
  	uv.y*=RENDERSIZE.y/RENDERSIZE.x;
	vec3 p = vec3(uv / 4., 0) + vec3(1., -1.3, 0.);
	p.x -= 0.25*move.x;
	p.y -= 0.25*move.y;
	p.x += 0.25*(1.0)*5.0*cos(0.01*TIME)+0.001*TIME;
	p.y += 0.25*(1.0)*5.0*sin(0.01*TIME)+0.001*TIME;
	p.z += 0.003*TIME;
	float speed = rate * cos(TIME*0.02 + pi/4.0);
	vec3 dir=vec3(uv*zoom,1.);
	vec3 from=vec3(0.0, 0.0,0.0);
	vec3 forward = vec3(0.,0.,1.);
	float a1 = pi * (move.x/RENDERSIZE.x-.5);
	mat2 rot1 = mat2(cos(a1),sin(a1),-sin(a1),cos(a1));
	float a2 = pi * (move.y/RENDERSIZE.y-.5); .6;
	mat2 rot2 = mat2(cos(a2),sin(a2),-sin(a2),cos(a2));
	p.xz *= rot1;
	p.xy *= rot1;
	p.yz *= -rot2;
	p = abs(vec3(density)-mod(p,vec3(density*2.)));
	float t = field(p);
	float v2 = (1. - exp((abs(uv.x) - 1.) * 6.)) * (1. - exp((abs(uv.y) - 1.) * 6.));
	from.x += 5.0*(0.01*rate) + 0.001*TIME;
	from.y += 5.0*(0.01*rate) +0.001*TIME;
	from.z += 0.003*TIME;
	dir.xz*=rot1;
	forward.xz *= rot1;
	dir.xy*=rot1;
	forward.xy *= rot1;
	dir.yz*=-rot2;
	forward.yz *= -rot2;
	from.xz*=rot1;
	from.xy*=rot1;
	from.yz*=-rot2;
	from += (move.x-.5)*vec3(-forward.z,0.,forward.x);
	from += (move.y-.5)*vec3(0.,-forward.z, forward.y);
	float zooom = (TIME-3311.)*speed;
	from += forward* zooom;
	float sampleShift = mod( zooom, depth );
	float zoffset = -sampleShift;
	sampleShift /= depth; 
	float s=0.1;
	vec3 v=vec3(0.);
	for (int r=0; r<volsteps; r++) {
		vec3 p=from+(s+zoffset)*dir;
		p = abs(vec3(density)-mod(p,vec3(density*2.))); 
		float pa,a=pa=0.;
		for (int i=0; i<iterations; i++) {
			p=abs(p)/dot(p,p)-morph; 
			float D = abs(length(p)-pa); 
			a += i > 7 ? min( 12., D) : D;
			pa=length(p);
		}
		a*=a*a;
		float s1 = s+zoffset;
		float fade = pow(distfading,max(0.,float(r)-sampleShift));
		float dm=max(0.,2.0-a*a*.001);
		if (r>3) fade*=1.-dm; 
		if ( r == 0 ) fade *= 1. - sampleShift;
		if ( r == volsteps-1 ) fade *= sampleShift;
		v+=vec3(s1,s1*s1,s1*s1*s1*s1)*a*brightness*fade;
		s+=depth;
	}
	v=mix(vec3(length(v)),v,saturation); 
	if (nebula) {	
		vec4 forCol = vec4(v*.01,1.);
		vec4 backCol = mix(.4, 1., v2) * vec4(1.8 * t * t * t, 1.4 * t * t, t, 1.0);
		backCol *= 0.2;
		backCol.b *= 1.0;
		backCol.r = mix(backCol.r, backCol.b, 0.2);
		forCol.g *= max((backCol.r * 4.0), 1.0);
		forCol.r += backCol.r * 0.05;
		forCol.b += 0.5*mix(backCol.g, backCol.b, 0.8);
		gl_FragColor = forCol;
	}
	else	
	{	
		vec4 col = vec4(vec3(v*.01),1.);
		gl_FragColor = col;
	}
}
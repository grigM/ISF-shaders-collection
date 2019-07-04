/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#52057.0"
}
*/



// Is it meant to be so ... jiggly? the stars seem to jump. Pretty though!


//CBS
//Parallax scrolling fractal galaxy.
//Inspired by JoshP's Simplicity shader: https://www.shadertoy.com/view/lslGWr

// http://www.fractalforums.com/new-theories-and-research/very-simple-formula-for-fractal-patterns/
// Ported from ShaderToys.com by redexe@gmail.com

#ifdef GL_ES
precision mediump float;
#endif


float makePoint(float x,float y,float fx,float fy,float sx,float sy,float t){
   float xx=x+sin(t*fx)*sx;
   float yy=y+cos(t*fy)*sy;
   return 1.0/sqrt(xx*xx+yy*yy);
}

float field(in vec3 p,float s) {
	float strength = 7. + .03 * log(1.e-6 + fract(sin(TIME) * 12000.11));
	float accum = s/6.;
	float prev = 0.;
	float tw = 0.;
	for (int i = 0; i < 26; ++i) {
		float mag = dot(p, p);
		p = abs(p) / mag + vec3(-.5, -.4, -1.5);
		float w = exp(-float(i) / 7.);
		accum += w * exp(-strength * pow(abs(mag - prev), 2.2));
		tw += w;
		prev = mag;
	}
	return max(0., 5. * accum / tw - .7);
}

// Less iterations for second layer
float field2(in vec3 p, float s) {
	float strength = 7. + .03 * log(1.e-6 + fract(sin(TIME) * 4373.11));
	float accum = s/4.;
	float prev = 0.1;
	float tw = 0.;
	for (int i = 0; i < 18; ++i) {
		float mag = dot(p, p);
		p = abs(p) / mag + vec3(-.5, -.4, -1.5);
		float w = exp(-float(i) / 7.);
		accum += w * exp(-strength * pow(abs(mag - prev), 2.2));
		tw += w;
		prev = mag;
	}
	return max(0., 5. * accum / tw - .7);
}

vec3 nrand3( vec2 co )
{
	vec3 a = fract( cos( co.x*10.3e-3 + co.y )*vec3(10.3e5, 4.7e5, 2.9e5) );
	vec3 b = fract( sin( co.x*0.8e-3 + co.y )*vec3(8.1e5, 1.0e5, 0.1e5) );
	vec3 c = mix(a, b, 0.5);
	return c;
}


void main() {
	vec4 asd;
	if (true) {
	
   vec2 p=(gl_FragCoord.xy/RENDERSIZE.x)*2.0-vec2(1.0,RENDERSIZE.y/RENDERSIZE.x);

   p=p*2.0;
   
   float x=p.x;
   float y=p.y;

   float a=
       makePoint(x,y,3.3,2.9,0.3,0.3,TIME);
   a=a+makePoint(x,y,1.9,2.0,0.4,0.4,TIME);
   a=a+makePoint(x,y,0.8,0.7,0.4,0.5,TIME);
   a=a+makePoint(x,y,2.3,0.1,0.6,0.3,TIME);
   a=a+makePoint(x,y,0.8,1.7,0.5,0.4,TIME);
   a=a+makePoint(x,y,0.3,1.0,0.4,0.4,TIME);
   a=a+makePoint(x,y,1.4,1.7,0.4,0.5,TIME);
   a=a+makePoint(x,y,1.3,2.1,0.6,0.3,TIME);
   a=a+makePoint(x,y,1.8,1.7,0.5,0.4,TIME);   
   
   float b=
       makePoint(x,y,1.2,1.9,0.3,0.3,TIME);
   b=b+makePoint(x,y,0.7,2.7,0.4,0.4,TIME);
   b=b+makePoint(x,y,1.4,0.6,0.4,0.5,TIME);
   b=b+makePoint(x,y,2.6,0.4,0.6,0.3,TIME);
   b=b+makePoint(x,y,0.7,1.4,0.5,0.4,TIME);
   b=b+makePoint(x,y,0.7,1.7,0.4,0.4,TIME);
   b=b+makePoint(x,y,0.8,0.5,0.4,0.5,TIME);
   b=b+makePoint(x,y,1.4,0.9,0.6,0.3,TIME);
   b=b+makePoint(x,y,0.7,1.3,0.5,0.4,TIME);

   float c=
       makePoint(x,y,3.7,0.3,0.3,0.3,TIME);
   c=c+makePoint(x,y,1.9,1.3,0.4,0.4,TIME);
   c=c+makePoint(x,y,0.8,0.9,0.4,0.5,TIME);
   c=c+makePoint(x,y,1.2,1.7,0.6,0.3,TIME);
   c=c+makePoint(x,y,0.3,0.6,0.5,0.4,TIME);
   
   vec3 d=vec3(a,b,c)/32.0;
   
   asd = vec4(d.x,d.y,d.z,1.0);	
	}
	
    vec2 uv = 2. * gl_FragCoord.xy / RENDERSIZE.xy - 1.;
	vec2 uvs = uv * RENDERSIZE.xy / max(RENDERSIZE.x, RENDERSIZE.y);

	vec3 p = vec3(uvs / 4., 0) + vec3(1., -1.3, 0.);
//	p += .2 * vec3(sin(TIME / 16.), sin(TIME / 12.),  sin(TIME / 128.));
	
	uvs.x -= sin(asd.x) - cos(asd.y) - tan(asd.z);
        uvs.y -= sin(asd.x) - cos(asd.y) - tan(asd.z);
	
	float freqs[4];
	freqs[0] = 0.04;
	freqs[1] = 0.5;
	freqs[2] = 0.3;
	freqs[3] = 0.3;
	
	float t = field(p,freqs[2]);
	float v = (1. - exp((abs(uv.x) - 1.) * 6.)) * (1. - exp((abs(uv.y) - 1.) * 6.));
	
    //Second Layer
	vec3 p2 = vec3(uvs / (4.+sin(TIME*0.11)*0.2+0.2+sin(TIME*0.15)*0.3+0.4), 1.5) + vec3(2., -1.3, -1.);
	// p2 += 0.25 * vec3(sin(TIME / 16.), sin(TIME / 12.),  sin(TIME / 128.));
	float t2 = field2(p2,freqs[3]);
	//vec4 c2 = mix(.4, 1., v) * vec4(1.3 * t2 * t2 * t2 ,1.8  * t2 * t2 , t2* freqs[0], t2);
	vec4 c2 = mix(.4, 1., v) * vec4(1.8  * t2 * t2 ,1.8  * t2 * t2 , 1.8  * t2 * t2, t2);
	

	
	
	//Let's add some stars
	//Thanks to http://glsl.heroku.com/e#6904.0
	vec2 seed = p.xy * 1.1;	
	seed = floor(seed * RENDERSIZE.x);
	vec3 rnd = nrand3( seed );
	vec4 starcolor = vec4(pow(rnd.y,19.0));
	
	//Second Layer
	vec2 seed2 = p2.xy * 2.0;
	seed2 = floor(seed2 * RENDERSIZE.x);
	vec3 rnd2 = nrand3( seed2 );
	starcolor += vec4(pow(rnd2.y,40.0));
	
	gl_FragColor = c2;
}
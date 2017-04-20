/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "SPEED",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 5.0
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39450.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float random(float p) {
  return fract(sin(p)*10000.);
}

float noise(vec2 p) {
  return random(p.x + p.y*10000.);
}

vec2 sw(vec2 p) {return vec2( floor(p.x) , floor(p.y) );}
vec2 se(vec2 p) {return vec2( ceil(p.x)  , floor(p.y) );}
vec2 nw(vec2 p) {return vec2( floor(p.x) , ceil(p.y)  );}
vec2 ne(vec2 p) {return vec2( ceil(p.x)  , ceil(p.y)  );}

float smoothNoise(vec2 p) {
  vec2 inter = smoothstep(0., 1., fract(p));
  float s = mix(noise(sw(p)), noise(se(p)), inter.x);
  float n = mix(noise(nw(p)), noise(ne(p)), inter.x);
  return mix(s, n, inter.y);
  return noise(nw(p));
}

float movingNoise(vec2 p) {
  float total = 0.0;
  total += smoothNoise(p     - (TIME*SPEED));
  total += smoothNoise(p*3.  + (TIME*SPEED)) / 3.;
  total += smoothNoise(p*5.  - (TIME*SPEED)) / 5.;
  total += smoothNoise(p*7.  + (TIME*SPEED)) / 7.;
  total += smoothNoise(p*13. - (TIME*SPEED)) / 13.;
  total /= 1. + 1./2. + 1./4. + 1./8. + 1./16.;
  return total;
}

float nestedNoise(vec2 p) {
  float x = movingNoise(p);
  float y = movingNoise(p + 100.);
  return movingNoise(p + vec2(x, y));
}


float fact = 10.;


vec3 hsv2rgb( in vec3 c )
{
	float r,g,b =1.0;
	
	float t = mod(0.+(TIME*SPEED)/2.,16.0);
	float tt = mod((TIME*SPEED),16.0);
	
	fact = floor(1.+tt  );
	
	r = floor(0.+t  );
	b = floor(6.+t*4.0  );
	g = floor(4.+t*2.0  );
      	
	
	vec3 rgb = clamp( abs(mod(c.x*fact+vec3(r,g,b),6.0)-3.0)-1.0, 0.0, 1.0 );
	
		
	//rgb = rgb*rgb*(3.0-2.0*rgb); // cubic smoothing	
	
	

	return c.z * mix( vec3(1.0), rgb, c.y);
}
void main( void ) {

	vec2 p = (2.*gl_FragCoord.xy - RENDERSIZE.xy )/RENDERSIZE.y;
	
	vec2 pp = p * 6.;
  	//p.x  = nestedNoise(p);
	p.y  = nestedNoise(p);
	
	float t = mod(0.+(TIME*SPEED),7.0);
			
	int dir = int(floor(t  ));

	if(dir == 0) 	gl_FragColor = vec4( hsv2rgb(vec3(p.y+(TIME*SPEED)*0.2, 1.0, 1.0)), 1.0 );
	if(dir == 1) 	gl_FragColor = vec4( hsv2rgb(vec3(p.x+(TIME*SPEED)*0.2, 1.0, 1.0)), 1.0 );
	if(dir == 2) 	gl_FragColor = vec4( hsv2rgb(vec3(p.y+p.x+(TIME*SPEED)*0.2, 1.0, 1.0)), 1.0 );
	if(dir == 3) 	gl_FragColor = vec4( hsv2rgb(vec3(p.y-p.x+(TIME*SPEED)*0.2, 1.0, 1.0)), 1.0 );
	if(dir == 4) 	gl_FragColor = vec4( hsv2rgb(vec3(length(p.x)+(TIME*SPEED)*0.2, 1.0, 1.0)), 1.0 );
        if(dir == 5) 	gl_FragColor = vec4( hsv2rgb(vec3(length(p.y)+(TIME*SPEED)*0.2, 1.0, 1.0)), 1.0 );
	
	if(dir == 6) 	gl_FragColor = vec4( hsv2rgb(vec3(length(p)+(TIME*SPEED)*0.2, 1.0, 1.0)), 1.0 );
}
/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60126.5"
}
*/


// more fanny batter (deep computed)
#ifdef GL_ES
precision highp float;
#endif

#define PI 3.14159

float vDrop(vec2 uv,float t)
{
    uv.x = uv.x*128.0;						// H-Count
    float dx = fract(uv.x);
    uv.x = floor(uv.x);
    uv.y *= 0.05;							// stretch
    float o=sin(uv.x*215.4);				// offset
    float s=cos(uv.x*33.1)*.3 +.7;			// speed
    float trail = mix(905.0,25.0,s);			// trail length
    float yv = fract(uv.y + t*s + o) * trail;
    yv = 1.0/yv;
    yv = smoothstep(0.0,1.0,yv*yv);
    yv = sin(yv*PI)*(s*5.0);
    float d2 = sin(dx*PI);
    return yv*(d2*d2);
}

void main( void ) {
	
	float t = TIME*0.1;
	float t2 = TIME*4.0;

	
	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy ) - 0.5;
	position.x = dot(position,position)*4.0;
	position.x *= RENDERSIZE.x/RENDERSIZE.y;
	position *= 0.35;
	
	position.y *= 4.+sin((position.y*position.x)*15.40+t2)*.15;
	
	float foff = sin(TIME*.5)*0.75;
	float den = 0.1;
	float amp = .1;
	float freq = 9792416.;
	float offset = 0.2-sin(position.x*0.5)*0.05;


	vec3 colour = vec3 (.84, 0.5, .1) * 
		 ((1.0 / abs((position.y + (amp * sin(((position.x*4.0 + t) + offset) *freq)))) * den)
		+ (1.0 / abs((position.y + (amp * sin(((position.x*4.0 + t) + offset) *freq+foff)))) * den)
		+ (1.0 / abs((position.y + (amp * sin(((position.x*4.0 + t) + offset) *freq-foff)))) * den));


	float dd = (1.0 / abs((position.y + (amp * sin(((position.y*32.0 + t) + offset) *freq+foff)))) * den);
	colour *= vec3(1.25,0.61,.29)*dd;
	
	
	vec2 p = (gl_FragCoord.xy - 0.5 * RENDERSIZE.xy) / RENDERSIZE.y;
	vec2 pp = p;
	float d = length(p)+0.2;
	p = vec2(atan(p.x, p.y) / PI, 2.5 / d);
	t =  TIME*0.4;
	vec3 col = vec3(1.55,0.65,.225) * vDrop(p,t);	// red
	col += vec3(0.55,0.75,1.225) * vDrop(p,t+0.33);	// blue
	col += vec3(0.45,1.15,0.425) * vDrop(p,t+0.66);	// green
	
	colour = clamp(colour,vec3(0.0),vec3(1.0));
	colour *= 0.4/abs(pp.x*0.5);
	
	colour+=col;
	
	gl_FragColor = vec4( colour, 1.0 );

}
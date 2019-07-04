/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#3877.3"
}
*/


#ifdef GL_ES
precision highp float;
#endif


float unosc()
{
	return (1.0 + sin(TIME))*0.5;
}


float smoothHouse( vec2 p, float h, float smooth )
{
    vec2 q;

	q.y = abs(p.x);
	q.x = p.y ;
	
	float rf =  -5.0 + unosc()*10.0;
    float d = dot(q, vec2(rf, 1.0));
	
	float shade = max(d, q.y)-h;
	shade = smoothstep(0.0+smooth, 0.0-smooth, shade);
    return shade;
}


void main(void)
{

	vec2 unipos = (gl_FragCoord.xy / RENDERSIZE);
	vec2 pos = unipos*2.0-1.0;
	pos.x *= RENDERSIZE.x / RENDERSIZE.y;

    float shade = smoothHouse( pos, 0.5, 0.01 );
    vec3 clr = vec3(shade*0.2, shade*0.6, shade*1.0);
	
	
    gl_FragColor = vec4(clr,1.0);
}
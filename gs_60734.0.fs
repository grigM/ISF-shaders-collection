/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60734.0"
}
*/


// more fanny batter (deep computed) - disco
#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.141592

vec3 pal(float x)
{
	return max(min(sin(vec3(x,x+PI*2.0/3.0,x+PI*4.0/3.0))+0.5,1.0),0.0);
}

void main( void ) {
	
	float t = TIME*0.9;
	//float t2 = tan(TIME)*-1.0;

	
	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy ) - 0.5;
	position *= 2.25;
	position.y *= dot(position,position);
	
	position.y *= 1.0+sin(position.x*3.0)*0.2;
	
	float foff = 0.3;
	float den = 0.05;
	float amp = mouse.y;
	float freq = 5.0+mouse.x*10.0;
	float offset = 0.1-tan(position.x*0.5)*5.05;

        float modifer = 0.;
	
	for(float i = 0.0; i < 3.0; i+=1.0)
		modifer += 6.0/abs((position.y + (amp*sin(((position.x*4.0 + t) + offset) *freq+i*foff))))*den;;
	
	vec3 colour = pal(position.x*2.)*0.25* modifer;	//vec3 (0.13, 0.18, 0.4) * 
	//	 ((1.0 / abs((position.y + (amp * sin(((position.x*4.0 + t) + offset) *freq)))) * den)
	//	+ (1.0 / abs((position.y + (amp * sin(((position.x*4.0 + t) + offset) *freq+foff)))) * den)
	//	+ (1.0 / abs((position.y + (amp * sin(((position.x*4.0 + t) + offset) *freq-foff)))) * den));
	
	gl_FragColor = vec4( colour, 2.0 );


}
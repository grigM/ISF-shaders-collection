/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#45469.0"
}
*/


//--- hatsuyuki ---
// by Catzpaw 2016
#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float snow(vec2 uv,float scale)
{
	float w = 1.0;
	
	//uv += TIME / scale;
	uv.x += TIME * 7.0 / scale;
	//uv.x += sin(uv.y + TIME * 0.5) / scale;
	uv *= scale;
	
	vec2 s = floor(uv)*8.;
	vec2 f = fract(uv);
	float k = 3.0;
	
	vec2 p = 1.0 * sin(11.0 * fract(sin((s + scale) * mat2(7.0, 3.0, 6.0, 5.0)) * 5.0)) - f;
	
	float d = length(p);
	
	k = min(d, k);
	k = smoothstep(0.0, k, sin(f.x + f.y) * 0.01);
	
	return k * w;
}

void main(void){
	vec2 uv=(gl_FragCoord.xy*2.-RENDERSIZE.xy)/min(RENDERSIZE.x,RENDERSIZE.y); 
	vec3 finalColor=vec3(0);
	float c=0.0;
	c+=snow(uv,10.);
	c+=snow(uv,8.);
	c+=snow(uv,6.);
	c+=snow(uv,5.);
	c+=snow(uv,3.);
	c+=snow(uv,1.);
	vec3 rainColour = vec3(0.99, 0.99, 0.99)-0.5;
	finalColor=vec3(c) / rainColour;
	gl_FragColor = vec4(finalColor,1.0);
}
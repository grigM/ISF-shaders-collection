/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60434.3"
}
*/


// poo
#ifdef GL_ES
precision highp float;
#endif


float rect(vec2 uv, vec2 pos, vec2 size,float sharpness) {
return  clamp(1. - length(max(abs(uv - pos)-size, 0.0))*sharpness, 0.0, 1.0);
}
float hash(float v)
{
    return fract(fract(v/1e4)*v-1e6);
}
void main( void ) 
{
	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );
	float ym = 1.0-abs(1.0-(position.y*2.0));
	ym *= 1.9;
	
	position.y += sin(TIME*0.53-position.x*3.2)*0.15;
	position *= 1.0+sin(position.y*1.4+position.x+TIME*1.1)*0.1;
	float color = 0.0;
	
	float t =0.5+(sin(TIME*0.35)*0.01);
	
	float i_f = 1.5+sin(position.x*2.0+TIME*0.4)*0.17;
	for(int i = 0;i < 60;i++)
	{
		i_f += 1.;
		float sharpness = 2000.;
		float r = hash(i_f) * 60.;
		color += (rect(position, vec2( mod(t * (i_f * r * .2), 3.) - 1. , mod(r,1.)   ), vec2(0.3,0.13) * (i_f * 0.025) , sharpness) * (0.02 * (i_f * 0.15) ));		
		color += (rect(position, vec2( mod(t * (i_f * r * .2), 3.) - 1. , mod(r,1.)   ), vec2(0.3,0.13) * (i_f * 0.025) , sharpness * 0.02) * (0.02 * (i_f * 0.15) ));		
	}
	
	vec3 col1 = vec3(color,color*0.7,color*0.6);
	
	
	vec3 col2 = vec3(ym*.8,ym*0.5,ym*.5);
	col1 = mix(col2,col1+col2*col2*(0.8+sin(TIME*3.3+(position.y*14.0)+(position.x*7.0))*0.4),color);
	
	
	gl_FragColor = vec4(col1*.35, 1.0 );
	
	
}
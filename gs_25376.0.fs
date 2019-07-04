/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#25376.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


vec3 circle(vec2 uv, vec3 pos, vec3 color, vec3 prevcolors) {
	//pos.xy position, pos.z size
	float circle = clamp(1.-((length(uv-pos.xy)-pos.z)*500.),0.,1.);
	vec3 final = vec3(0.);
	final = clamp(circle*color,0.,1.);
	final = mix(prevcolors,final,circle);
	return vec3(final);
}

void main( void ) {

	vec2 uv = 2.0*( gl_FragCoord.xy / RENDERSIZE.xy )-1.0;
	uv.x *= RENDERSIZE.x/RENDERSIZE.y; 


	//vec2 oruv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	//vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	//uv -= 0.5;
	//uv *= vec2(2.,1.);
	uv += 0.5;
	
	vec3 color = vec3(0.);
	
	for(int i = 0; i < 19; i++) {
	float ii = (float(i)/3.02);
	color = circle(uv,vec3((-sin(ii+TIME)/3.)+0.5,(cos(ii+TIME)/3.)+0.5,0.02),vec3(1.,0.,0.),color);
	color = circle(uv,vec3((sin(ii+TIME)/3.5)+0.5,(cos(ii+TIME)/3.5)+0.5,0.02),vec3(0.,1.0,0.),color);
	color = circle(uv,vec3((-sin(ii+TIME)/4.3)+0.5,(cos(ii+TIME)/4.3)+0.5,0.02),vec3(0.,0.0,1.),color);
	color = circle(uv,vec3((sin(ii+TIME)/5.5)+0.5,(cos(ii+TIME)/5.5)+0.5,0.02),vec3(1.,1.,0.),color);
	color = circle(uv,vec3((-sin(ii+TIME)/7.5)+0.5,(cos(ii+TIME)/7.5)+0.5,0.02),vec3(0.,1.0,1.),color);	
	color = circle(uv,vec3((-sin(ii+TIME)/1.9)+0.5,(cos(ii+TIME)/4.9)+0.5,0.02),vec3(0.,1.0,1.),color);	
	color = circle(uv,vec3((-sin(ii+TIME)/1.4)+0.5,(cos(ii+TIME)/3.9)+0.5,0.02),vec3(0.,0.4,1.),color);	
	}
	
	
	gl_FragColor = vec4( color, 1.0 );

}
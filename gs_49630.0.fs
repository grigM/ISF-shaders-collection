/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#49630.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float circle(vec2 uv, vec2 pos, float r) {
	return length(uv+pos)/r;
}
float rand(vec2 p) {
	return cos(tan(p.y*sin(p.x)*20012.510)*20.102);
}

float noise(vec2 n) {
	const vec2 d = vec2(0.0, 1.);
        vec2 b = floor(n), f = smoothstep(vec2(0.0), vec2(1.0), fract(n));
	return mix(mix(rand(b), rand(b + d.yx), f.x), mix(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}

void main( void ) {

	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy )-.5;
	vec2 cuv = uv;
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
	//uv.x += sin(uv.x/5.)*20.;
	//uv.y += length(uv)+rand(uv)/400.+noise(uv*20.+TIME/200.)/200.;
	uv.x += rand(uv)/1000.+noise(uv*20.)/400.+noise(uv*20.+TIME/200.)/200.;
	uv.x += (2.)*log(length(uv))-(TIME*2.);

	vec3 color = vec3(0.0);
	
	for (float i=0.; i< 100.; i++) {
		//color += smoothstep(0.7,0.4,circle(uv+rand(uv)/1000., .5+vec2(uv+noise(vec2(i/10.,i+TIME/20.))*1.1 - 1.,noise(vec2(i/20.,i/2.))*1.1 - 1.), .03))/2.;
		//color = vec3(noise(uv*4. +noise(uv+TIME)+noise(uv*4.+TIME*2.) - TIME));
		color.r = float(noise(uv*4. +noise(uv+TIME)+noise(uv*4.+TIME*2.) - TIME))*.3+.7;
		color.g = float(noise(uv*4. +noise(uv+TIME)+noise(uv*4.+TIME*2. +.4) - TIME))*0.4+.4;
		color.b = float(noise(uv*4. +noise(uv+TIME)+noise(uv*4.+TIME*2. +.6) - TIME))*0.3+.5;
		//color.r = float(noise(5.+vec2(noise(uv+5.+TIME/6.))*13. + noise(uv*2.+TIME*2.)*3.)/2.+.5);
		//color.g = float(noise(2.+vec2(noise(uv+5.+TIME/6.))*12. + noise(uv*2.+TIME*2.+.2)*3.)/2.+.5);
		//color.b = float(noise(2.+vec2(noise(uv+5.+TIME/6.))*12. + noise(uv*2.+TIME*2.+.4)*3.)/2.+.5);
	}

	color *=log(1.+length(cuv))*3.;
	

	gl_FragColor = vec4(color, 1.0 );

}
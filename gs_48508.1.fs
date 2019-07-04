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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#48508.1"
}
*/


/*Ziad*/
#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


mat2 m =mat2(0.8,0.6, -0.6, 0.8);

float rand(vec2 n) { 
	return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}

float noise(vec2 n) {
	const vec2 d = vec2(0.0, 1.0);
  	vec2 b = floor(n), f = smoothstep(vec2(0.0), vec2(1.0), fract(n));
	return mix(mix(rand(b), rand(b + d.yx), f.x), mix(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}

float fbm(vec2 p){
	float f=.0;
	f+= .5000*noise(p); p*= m*2.02;
	f+= .2500*noise(p); p*= m*2.03;
	f+= .1250*noise(p); p*= m*2.01;
	f+= .0625*noise(p); p*= m*2.04;
	
	f/= 0.9375;
	
	return f;
}




void main() {
	vec2 st = (gl_FragCoord.xy - .5 * RENDERSIZE) / RENDERSIZE.y;
	float t =  sqrt((st.x)*(st.x)/(st.y+.8)/(.8-st.y) );
	t*=smoothstep(.2,.9,fract(fbm(10.*vec2(t))));
	t+=fract(fbm(st));
	vec2 uv = st * t + vec2(0.01, t + TIME);
	vec3 color=vec3(1.);
	
	
	float flicker = fract( mod(TIME*1.3,.45) / sin(TIME*1.2) );
	for (float i = 0.; i < 20.; i++) {
		color *= vec3(dot(cos(uv * (3. *( 3.0*mouse.x) * i*2.))/sin(TIME-mouse.y), sin(uv * 100.) - 
					mod(sin(uv + TIME - length(uv * 10.)),TIME)) + vec4(0.02, 1, 2, 0)) *
				(length(st));
		color *= (fbm(8.*color.xy));
		color /=  flicker;
	}	
	
	gl_FragColor=vec4(color,1.);
}
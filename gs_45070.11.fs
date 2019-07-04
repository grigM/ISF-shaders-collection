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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#45070.11"
}
*/


//paint tunnel by @samloeschen
//simple planar distortion tunnel made out of IQ voronoise


#ifdef GL_ES
precision highp float;
#endif

#extension GL_OES_standard_derivatives : enable



vec3 hash3( vec2 p ) {
    vec3 q = vec3( dot(p,vec2(127.1,311.7)), 
				   dot(p,vec2(269.5,183.3)), 
				   dot(p,vec2(419.2,371.9)) );
	return fract(sin(q)*43758.5453);
}


//iq voronoise
float iqnoise( in vec2 x, float u, float v ) {
    vec2 p = floor(x);
    vec2 f = fract(x);
		
	float k = 1.0+63.0*pow(1.0-v,4.0);
	
	float va = 0.0;
	float wt = 0.0;
    for( int j=-2; j<=2; j++ )
    for( int i=-2; i<=2; i++ )
    {
        vec2 g = vec2( float(i),float(j) );
		vec3 o = hash3( p + g )*vec3(u,u,1.0);
		vec2 r = g - f + o.xy;
		float d = dot(r,r);
		float ww = pow( 1.0-smoothstep(0.0,1.414,sqrt(d)), k );
		va += o.z*ww;
		wt += ww;
    }
	
    return va/wt;
}

//iq palette
vec3 palette(in float t) {
	//colors
	vec3 a = vec3(0.5, 0.5, 0.5);  
	vec3 b = vec3(0.5, 0.5, 0.5);
	vec3 c = vec3(1.0, 1.0, 1.0);
	vec3 d = vec3(0.3, 0.15, 0.87);
	
	return a + b * cos(6.28318 * (c * t + d));
}

float sin01(in float t){
	return (sin(t) + 1.) * 0.5;
}


void main( void ) {
	
	vec2 point = gl_FragCoord.xy / RENDERSIZE.xy;
	//vec2 mouse = (mouse.xy - 0.5) * 0.1; mouse is kind of ugly?
	vec2 center = vec2(0.5 - (sin(TIME * 0.5) * 0.05), 0.5 - (sin(TIME * 0.25) * 0.1));

	//aspect ratio
	float aspect = RENDERSIZE.x / RENDERSIZE.y;
	point.x *= aspect;
	center.x *= aspect;
	
	//center circle
	float circle = pow(smoothstep(0.3, 0., length(point - center)), 1.2) * 0.55;
	
	//circle glow
	float glow = pow(smoothstep(1.4, 0.2, length(point - center)), 1.2) * 0.45;
	
	
	//planar distortion
	float rInv = 1. / length(point - center);
	point = point * rInv - vec2(rInv, 0.0);
	
	//sample noise
	vec2 uv = vec2(point.x, point.y + TIME * 0.5) * 1.7;
	float noise = iqnoise(uv, 0.5, 1.2);
	float stepNoise = floor(noise * 10.3) / 11.; //stepping irregularly gives nice shapes 
	
	//draw outline around stepped noise
	float outline = 1.0 - step(smoothstep(stepNoise - 0.03, stepNoise, noise) - (smoothstep(stepNoise, stepNoise + 0.03, noise)), 0.03);
	vec3 outlineCol = outline * palette(sin01(TIME * 0.2));
	
	//get tunnel color from palette. I tried directly attenuating this by length of the fragment but it kept blowing out to white
	vec3 palette = palette((stepNoise * 0.5) + sin01(TIME * 0.4) * 0.2);
	
	//combine
	vec3 finalColor = palette + outlineCol + circle + glow;
	//finalColor = vec3(glow + circle);
	gl_FragColor = vec4(finalColor, 1.0);	
}


/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#44435.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float random (in vec2 st) {
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233)))*
        43758.5453123);
}

// Based on Morgan McGuire @morgan3d
// https://www.shadertoy.com/view/4dS3Wd
float noise (in vec2 st) {
    vec2 i = floor(st);
    vec2 f = fract(st);

    // Four corners in 2D of a tile
    float a = random(i);
    float b = random(i + vec2(1.0, 0.0));
    float c = random(i + vec2(0.0, 1.0));
    float d = random(i + vec2(1.0, 1.0));

    vec2 u = f * f * (3.0 - 2.0 * f);

    return mix(a, b, u.x) +
            (c - a)* u.y * (1.0 - u.x) +
            (d - b) * u.x * u.y;
}

#define OCTAVES 5
float fbm (in vec2 st) {
    // Initial values
    float value = 0.0;
    float amplitude = .5;
    float frequency = 0.;
    //
    // Loop of octaves
    for (int i = 0; i < OCTAVES; i++) {
        value += amplitude * noise(st);
        st *= 2.;
        amplitude *= .5;
    }
    return value;
}

// @author: https://twitter.com/c0de4
// sigoto osamari mashita ka????

void main( void ) {
	float t = TIME*.1;
	float c = 0.;
	
	vec2 p = ( gl_FragCoord.xy * 2. - RENDERSIZE.xy ) / min(RENDERSIZE.x, RENDERSIZE.y);
	
	for(float i = 1.; i < 4.; i++) {
		
		vec2 n = vec2(p*noise(vec2(fbm(p.yx-t*i))));

		c += .05 / abs(0.8 - length(p+n));
		//c -= abs(length(p)-noise(n*i)) / dot(p, p);
	}



	gl_FragColor = vec4( vec3( c*.9, c*.9, c), 1.0 );

}
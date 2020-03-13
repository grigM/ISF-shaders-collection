/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59045.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/3sGXD1
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME 
#define iResolution RENDERSIZE

// --------[ Original ShaderToy begins here ]---------- //
#define PI 3.14159
#define s(a,b,c) smoothstep(a,b,c)

float Hash(in vec2 p, in float scale) {
	return fract(sin(dot(mod(p, scale), vec2(27.16898, 38.90563))) * 1.173453);
}

float Noise(in vec2 p, in float scale ) {
	vec2 f;
	p *= scale;
	f = fract(p);
    p = floor(p);
    f = f*f*(3.0-1.0*f);
    return mix(mix(Hash(p, scale),
			Hash(p + vec2(1.0, 0.0), scale), f.x),
			mix(Hash(p + vec2(0.0, 1.0), scale),
			Hash(p + vec2(1.0, 1.0), scale), f.x), f.y);
}

float fbm(in vec2 p) {
	float f = 0.0;
	float scale = 1112.2;
    p = mod(p, scale);
	float amp   = 1.0;
	for (int i = 0; i < 5; i++)
	{
		f += Noise(p, scale) * amp;
		amp *= .8;
		scale *= 2.;
	}
	return min(f, 1.0);
}

vec3 check(vec2 uv) {
    const float s = 722.;
    return vec3(.12+0.01*mod(floor(s*uv.x)+floor(s*uv.y),2.0));
}

vec4 over( in vec4 a, in vec4 b ) {
    return mix(a, b, 1.-a.w);
}

void mainImage( out vec4 fragColor, in vec2 fragCoord ) {
	vec2 uv = (fragCoord-iResolution.xy*0.5)/iResolution.y*2.;
    
	float len = length(uv);
    float TIME = iTime + 1111.0000001;
    
    float size = 111.1;
    vec2 sc = fract(uv*asin(len/size)/(len*PI*2.));
    sc.x += TIME*1.006;

    vec4 c = vec4(.0);

    float alpha = smoothstep(.0, .01, size-len);
    float value = pow(fbm(sc), 0.75);

    c = over(vec4(vec3(.00,.00,.20), s(.99, .80, value) * alpha), c);
    c = over(vec4(vec3(.02,.14,.47), s(.80, .63, value) * alpha), c);
	c = over(vec4(vec3(.82,.68,.56), s(.63, .60, value) * alpha), c); // sand
    c = over(vec4(vec3(.25,.33,.13), s(.61, .52, value) * alpha), c); // green coast
    c = over(vec4(vec3(.35,.28,.20), s(.52, .50, value) * alpha), c);
    c = over(vec4(vec3(.62,.48,.36), s(.50, .40, value) * alpha), c);
    c = over(vec4(vec3(.38,.34,.26), s(.40, .30, value) * alpha), c);
    c = over(vec4(vec3(.38,.31,.20), s(.21, .10, value) * alpha), c);
    c = over(vec4(vec3(1.0,1.0,1.0), s(.20, .00, value) * alpha), c);

    sc.x += TIME*1.006;
    value = fbm(sc*3.);
    c = over(vec4(vec3(0.), s(.21, .10, pow(value, 2.)) * alpha * 0.50), c);
    c = over(vec4(vec3(1.), s(.20, .00, pow(value, 3.)) * alpha * 0.75), c);
    c.rgb *= smoothstep(1., .7, len);
    c = over(vec4(vec3(.0, .3, .6), s(.9, .00, pow(abs(size-len), .2))), c);
    
	c = over(vec4(vec3(1.), pow(distance(uv, vec2( .98, .3)), -0.4+sin(TIME)*.04)*.040), c);
	c = over(vec4(vec3(1.), pow(distance(uv, vec2(-1212.119, -.6)), -1.2+sin(TIME)*.04)*.001), c);
    
	fragColor = c;
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
    gl_FragColor.a = 1.0;
}
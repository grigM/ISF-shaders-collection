/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60955.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/WldXR8
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE

// --------[ Original ShaderToy begins here ]---------- //
float rand(float n) {
	return fract(sin(n*54.3575121)*84.6873217);
}

float rand(vec2 n) { 
	return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}

float noise(vec2 p){
	vec2 ip = floor(p);
	vec2 u = fract(p);
	u = u*u*(3.0-2.0*u);
	
	float res = mix(
		mix(rand(ip),rand(ip+vec2(1.0,0.0)),u.x),
		mix(rand(ip+vec2(0.0,1.0)),rand(ip+vec2(1.0,1.0)),u.x),u.y);
	return res*res;
}

vec4 circle(vec2 gv, vec2 id, float t) {
    gv *= 2.;
    vec2 offset = vec2(rand(id),rand(rand(id)))*.5-.5;
    gv += offset;
    float m = length(gv);
    m = noise(vec2(m, t+rand(id)*453.6542143)*10.);
    m = floor(m*8.)/8.;
    m = pow(m, 2.);
    float size = mix(.1,.5,rand(id));
    size = 1.-step(size, length(gv));
    m *= size;
    return vec4(m,m,m,size);
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec2 uv = (fragCoord-.5*iResolution.xy)/iResolution.y;
    vec3 col = vec3(0.);
    
    float z = 3.;
    
    
    vec3 m = vec3(0.);
    
    for(float i=0.; i<=100.; i+=1.){
        vec2 uv_ = uv + vec2(rand(i), rand(rand(i)));
        vec2 id = floor(uv_*z);
        vec2 gv = (fract(uv_*z)-.5);
    	vec4 m_ = circle(gv, id, iTime*.1);
        m = mix(m, m_.rgb, m_.a);
    }
    
    
   
    
    col = vec3(m);

    // Output to screen
    fragColor = vec4(col,1.0);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60124.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/wsKXRK
 */

#ifdef GL_ES
precision highp float;
#endif
// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE
const vec4 iMouse = vec4(0.5);

// --------[ Original ShaderToy begins here ]---------- //
// V-Drop - Del 19/11/2019 - (Tunnel mix - Enjoy)
// vertical version: https://www.shadertoy.com/view/tdGXWm
#define PI 3.14159

float vDrop(vec2 uv,float t)
{
    uv.x = uv.x*128.0;						// H-Count
    float dx = fract(uv.x);
    uv.x = floor(uv.x);
    uv.y *= 0.05;							// stretch
    float o=sin(uv.x*215.4);				// offset
    float s=cos(uv.x*33.1)*.3 +.7;			// speed
    float trail = mix(905.0,25.0,s);			// trail length
    float yv = fract(uv.y + t*s + o) * trail;
    yv = 1.0/yv;
    yv = smoothstep(0.0,1.0,yv*yv);
    yv = sin(yv*PI)*(s*5.0);
    float d2 = sin(dx*PI);
    return yv*(d2*d2);
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec2 p = (fragCoord.xy - 0.5 * iResolution.xy) / iResolution.y;
    float d = length(p)+0.1;
	p = vec2(atan(p.x, p.y) / PI, 2.5 / d);
    if (iMouse.z>0.5)
    	p.y *= 0.5;
    float t =  iTime*0.4;
    vec3 col = vec3(1.55,0.65,.225) * vDrop(p,t);	// red
    col += vec3(0.55,0.75,1.225) * vDrop(p,t+0.33);	// blue
    col += vec3(0.45,1.15,0.425) * vDrop(p,t+0.66);	// green
	fragColor = vec4(col*(d*d), 1.0);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
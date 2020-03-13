/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
      "NAME" : "speed",
      "TYPE" : "float",
      "MAX" : 3.0,
      "DEFAULT" : 1.0,
      "MIN" : -3.0
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#61073.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/WlcSD8
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME*speed
#define iResolution RENDERSIZE

// Emulate a black texture
#define texelFetch(s, uv, lod) vec4(0.0)

// --------[ Original ShaderToy begins here ]---------- //
// For this to work, it needs a rectangle which can be split into a square and another rectangle
// with the same edge-length ratios, such as x:1 where 1 / (x - 1) == x or (x - 1) == 1 / x
// and the golden ratio satisfies this equation.

// This variant by FabriceNeyret2 does it "the other way around" and draws boxes outwards from the center
// by placing the boxes on a golden spiral path: https://www.shadertoy.com/view/3llGD7

float phi = (sqrt(5.) + 1.) / 2.;

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec2 uv = fragCoord / iResolution.y;

    // Jittered TIME value for cheap motionblur
    float t = iTime + texelFetch(iChannel0, ivec2(fragCoord.xy) & 1023, 0).r / 60.;
    
    vec2 pp = vec2(pow(phi - 1., 4.), pow(phi - 1., 3.));
    
    // Calculate the limit point of nested transformations for zooming in to    
    vec2 zc = vec2(1., pp.y) / (1. - pp.x);
    
    uv -= .5 * vec2(iResolution.x / iResolution.y, 1);
    uv += vec2(cos(t / 3.), sin(t / 2.)) * .1;
    
    float a = t;
    mat2 m = mat2(cos(a), sin(a), -sin(a), cos(a));
    
    // Exponential scaling transform, for a seamless (self-similar) zooming animation.
    float scale = pow(pp.x, 1. + fract(t));
    
    uv = m * uv * scale + zc;

    vec3 c = vec3(0);

    // Repeatedly subdivide pixelspace into a square and rectangle with edge lengths in ratio 1:(phi-1)
    // Note that such a rectangle has the same shape as a rectangle with ratio 1:phi
    for(int i = 0; i < 32; ++i)
    {
        float j = float(i) + floor(t) * 4.;
        if(uv.x < 1.)
        {
            // Pixel is inside this square. Pick a colour and break out.
            c = sin(vec3(j, j * 2., j * 3.)) * .5 + .5;
            break;
        }
        // Pixel is inside the rectangle, so continue subdividing.
        uv = (uv - vec2(1., 1.)).yx * vec2(-1, 1) * phi;
    }
    
    fragColor.rgb = sqrt(c);
    fragColor.a = 1.;
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
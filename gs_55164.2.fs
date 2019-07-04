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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#55164.2"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/wtj3RG
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE

// --------[ Original ShaderToy begins here ]---------- //
#define RADIUS .475
#define ISTEP .015

#define SF 2./min(iResolution.x,iResolution.y)
#define RADIUS_EXP2 RADIUS*RADIUS

#define BLACK_COL vec3(0,0,0)/255.
#define WHITE_COL vec3(245,248,250)/255.

mat2 rot (float a){
	float ca = cos(a);
    float sa = sin(a);
    return mat2(ca,-sa,sa,ca);
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec2 uv = (fragCoord - .5*iResolution.xy)/iResolution.y;

    uv *= vec2(1.3333, 1.);  // /(.75,1.)

    float m = 0.;
    float i = -RADIUS + fract(iTime*.25) * ISTEP;
    for(int j = 0; j<100; j++) {

        vec2 iuv = uv - vec2(i, 0.);
        iuv *= rot(- i*2.);

        float l = length(iuv);
        float r = mix(atan(iuv.x, iuv.y), sqrt(RADIUS_EXP2 - i*i), (.5-mouse.x)*10.);

        m += abs(smoothstep(SF, .0, abs(r-l)) *(iuv.y));

        if (i >= RADIUS)
            break;
        i+=ISTEP;
    }

    vec3 col = mix(BLACK_COL, WHITE_COL, m);

    fragColor = vec4(col, 1.);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
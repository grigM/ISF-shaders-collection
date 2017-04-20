/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel1",
      "PATH" : "tex12.png"
    }
  ],
  "CATEGORIES" : [
    "2d",
    "oscilloscope",
    "sound",
    "visualization",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XttSzf by Quinchilion.  My take on the XY oscilloscope.",
  "INPUTS" : [

  ]
}
*/


/*

Based on the work inka's started over here: https://www.shadertoy.com/view/4tdXzX

Fixed the alignment issues. I don't really understand the math behind it, but
it seems to be caused by different sample rates. I just eyeballed a value that
makes it match up nicely.

Next, instead of drawing lines, I draw points, but in a stochastic sort of way,
where each pixel is testing against a different, randomly chosen set of points.
This gives noisy results, but should represent the data more accurately.
To reduce the noise (and to add a nice bloom) the value for each pixel is
accumulated according to the inverse square law, rather than using hard distance
checks.

It still doesn't match the reference video in some parts... maybe because some
of the frequencies used are too low?

----------

BASED ON: https://www.shadertoy.com/view/MsXGDn

Reference: https://www.youtube.com/watch?v=YqSvkNjWnnQ

*/

#define ReturnTuning .224399476
#define Alignment 0.9375

#define NumSamples 150
#define Scale 0.4
#define Intensity 3.0
#define LineWidth 0.001

vec2 getPoint(float x) {
    float left = x * Alignment;
    float right = fract(x - ReturnTuning / Alignment) * Alignment;
    
    float pointX = texture2D(iChannel0,vec2(left, 1.0)).x;
    float pointY = texture2D(iChannel0,vec2(right, 1.0)).x;

    return vec2(pointX, pointY);
}

void main()
{
	vec2 uv = (gl_FragCoord.xy / RENDERSIZE.xy) * 2.0 - 1.0;
	uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    uv = uv * Scale + 0.5;
    
    float oneOverN = 1.0 / float(NumSamples);
    
    float posOffset = IMG_NORM_PIXEL(iChannel1,mod((gl_FragCoord.xy / 256.0),1.0)).x;
    float timeOffset = TIME * 343.42;
    float xOffset = fract(posOffset + timeOffset) * oneOverN;
    
    float acc = 0.0;
    
	for (int i = 0; i < NumSamples; i++) {
        float x = float(i) * oneOverN + xOffset;
        vec2 point = getPoint(x);

        vec2 pa = point - uv;
        float d = dot(pa, pa);
        
        acc += 1.0 / (d + LineWidth * LineWidth); // Inverse square law
	}
	
    acc = acc * oneOverN * Intensity * LineWidth;
    vec3 color = vec3(0.1, 1.0, 0.2) * acc;
    color = color / (color + 1.0); // Basic tonemapping
    
	gl_FragColor = vec4(color, 1.0);
}
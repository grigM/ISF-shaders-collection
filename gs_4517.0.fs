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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#4517.0"
}
*/


// Unknown Pleasures
// by @simesgreen
// modded to be a bit closer to the album cover!
// http://en.wikipedia.org/wiki/Unknown_Pleasures

#ifdef GL_ES
precision highp float;
#endif
 
 
float hash( float n )
{
    return fract(sin(n)*4358.5453);
}
 
float fade(float t)
{
     //return t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
     return t * t * (3.0 - 2.0 * t);
}
		    
// 1d noise
float noise(float x)
{
   float p = floor(x);
   float f = fract(x);
   f = fade(f);
   float r = mix(hash(p), hash(p+1.0), f);
   r = r*r;
   return r;
}

// 2d noise
float noise( in vec2 x )
{
    vec2 p = floor(x);
    vec2 f = fract(x);
    f = f*f*(3.0-2.0*f);

    float n = p.x + p.y*57.0;
    float res = mix(mix( hash(n+  0.0), hash(n+  1.0),f.x),
                    mix( hash(n+ 57.0), hash(n+ 58.0),f.x), f.y);
    res = res*res;
    return res;
}

float fbm(float x)
{
    return noise(x)*0.5 +
	   noise(x*2.0)*0.25 +
	   noise(x*4.0)*0.125 +
	   noise(x*8.0)*0.0625;
}

float fbm(vec2 x)
{
    return noise(x)*0.5 +
	   noise(x*2.0)*0.25 +
	   noise(x*4.0)*0.125 +
	   noise(x*8.0)*0.0625;
}

void main(void)
{
    vec2 p = (gl_FragCoord.xy / RENDERSIZE.xy)*2.0-1.0;
    p.x *= RENDERSIZE.x/RENDERSIZE.y;
	
    float w = 0.05 + smoothstep(0.3, 0.15, abs(p.x))*0.2;
    //w *= mouse.y;
	
    const float linew = 0.004;
    const int waves = 60;
    const float freq = 5.0;
	
    float c = 0.0;
    float yp = -1.0;
	
    // check waves bottom to top
    for(int i=0; i<waves; i++) {
	float y = -0.6 + fbm( vec2(p.x*freq + float(i)*10.0 + mouse.x*10.0, TIME*0.3)) * w + float(i)*0.02;
	c += smoothstep(linew, 0.0, abs(y - p.y)) * ((y > yp) ? 1.0 : 0.0);
	yp = max(y, yp);
    }

    c = c * (((p.x > -0.5) && (p.x < 0.5)) ? 1.0 : 0.0);
	
    gl_FragColor = vec4(vec3(c),1.0);
}
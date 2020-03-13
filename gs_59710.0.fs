/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59710.0"
}
*/


// Real-Time Jizz-Rain using the famous rendering technique 'inverse piss flow' by nvidia.
// optimized with a bellend scalar as demonstrated way back at SIGGRAPH 2014 by AllYourBase.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
// (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

//precision highp float;

#define PI 3.14159

float nutsack(vec2 uv)
{
    uv.x *= sin(1.+uv.y*.125)*0.5;
    float t =  TIME*0.4;
    uv.x = uv.x*64.0;
    float dx = fract(uv.x);
    uv.x = floor(uv.x);
    uv.y *= 0.15;
    float o=sin(uv.x*215.4);
    float s=cos(uv.x*33.1)*.3 +.7;
    float trail = mix(145.0,15.0,s);
    float yv = 1.0/(fract(uv.y + t*s + o) * trail);
    yv = smoothstep(0.0,1.0,yv*yv);
    yv = sin(yv*PI)*(s*5.0);
    float d = sin(dx*PI);
    return yv*(d*d);
}

void main(void)
{ 
 vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
 vec3 col = vec3(1.1,0.9,0.9)*nutsack(uv);		// Get the jizz flowing
 gl_FragColor=vec4(col,1.);				// output the spunk
}
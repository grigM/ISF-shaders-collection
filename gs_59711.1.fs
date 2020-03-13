/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59711.1"
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
void main()
{

    vec2 uv = (gl_FragCoord.xy * 2. - RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
    uv = vec2(floor(160. * uv.x * sin(1. + uv.y * .125)), -uv.y * .5);
    float s = cos(uv.x * 33.125) * .3 + .7;
    float trail = 1. / (fract(uv.y + TIME * .4 * s + sin(uv.x * 215.4)) * mix(145., 15., s));
    gl_FragColor = vec4(.25 - vec3(.1, 1, 2) * (float(mod(TIME, 5.)) - trail * trail), 1);
}//n
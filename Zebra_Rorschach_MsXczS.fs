/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "raymarch",
    "2tweets",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MsXczS by lherm.  Kind of a failed experiment",
  "INPUTS" : [

  ]
}
*/


void main() {



    gl_FragColor-=gl_FragColor;
    vec2 R = RENDERSIZE.xy;
	vec3 p = vec3((gl_FragCoord.xy+gl_FragCoord.xy-R.xy)/R.y, .5), z=normalize(p);
    for (float i = 1.; i > 0.; i-=.01)
    {
        float T = TIME,
              x = length(cos(p)-cos(p.z-T)*sin(p.y-T))-1.;     
        gl_FragColor=i*i-gl_FragColor;
        if (x < .005) break;
        p += z * i;
    }
}

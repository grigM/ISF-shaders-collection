/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtVyz1 by raphaelk.  First circle",
  "INPUTS" : [

  ]
}
*/







void main() {



		    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
			uv -= 0.5;
		    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
			float d = length(uv);
		    float r = 0.2;
    
		    float c = smoothstep(r, r + tan(TIME * 0.5), d);
		    gl_FragColor = vec4(vec3(c), 1.0);
		}





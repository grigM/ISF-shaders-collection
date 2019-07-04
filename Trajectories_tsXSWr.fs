/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tsXSWr by Kali.  More experiments moving a fractal with feedback",
  "PASSES" : [
    {
      "TARGET" : "BufferA",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {

    }
  ],
  "INPUTS" : [

  ]
}
*/


vec3 kset(vec2 p) {
    for (int i=0; i<8; i++) {
		p=abs(p*2.)/dot(p,p)-1.;
    }
    return vec3(normalize(p),min(200.,length(p)));
}

void main() {
	if (PASSINDEX == 0)	{


		float t=TIME*.2;
	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	    vec2 p = (uv-.5);
	    p.x*=RENDERSIZE.x/RENDERSIZE.y;
		p+=vec2(sin(t),cos(t));
		p+=abs(1.-mod(t*.05,2.))*2.;
	    vec3 k=kset(p);
	    vec3 col = vec3(k.xy,0.5)*k.z*.5;
	    vec3 backbuff = IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).rgb;    
	    col=mix(backbuff,col,.002);
	    gl_FragColor = vec4(col,1.0);
	}
	else if (PASSINDEX == 1)	{


	   
	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	
	    vec3 col = IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).rgb;
	        
	    gl_FragColor = vec4(col,1.0);
	}
}

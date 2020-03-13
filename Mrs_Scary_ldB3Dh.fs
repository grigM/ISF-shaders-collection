/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ldB3Dh by Dave_Hoskins.  She speaks with forked eyes!!! Playing with TekF's 'Retro Parallax' ( https:\/\/www.shadertoy.com\/view\/4sSGD1 )\nIt uses smoothstep to zoom and pan the camera using the video's frame time information.\n\n",
  "INPUTS" : [
    
    {
    	"NAME" : "inputImage",
      	"TYPE" : "image"
    }
  ]
}
*/


// By Dave Hoskins.
// Playing with TekF's 'Retro Parallax'
// https://www.shadertoy.com/view/4sSGD1
// To keep her scary noggin in the frame it uses
// the channel time to pan the video.
// Then move back towards the text.

mat2 RotateMat(float angle)
{
	float si = sin(angle);
	float co = cos(angle);
	return mat2(co, si, -si, co);
}


vec3 Colour(in float h)
{
	h = h * 4.0;
	return clamp( abs(mod(h+vec3(0.,4.,2.),6.)-3.)-1., 0., 1. );
}

void main() {



	float time = TIME;
	// Rough panning...
	vec2 pixel = (gl_FragCoord.xy - RENDERSIZE.xy*.5)/RENDERSIZE.xy 
	+ vec2(0.0,.1-smoothstep(9.0, 12.0, time)*.35
	+ smoothstep(18.0, 20.0, time)*.15);
	vec3 col;
	float n;
	float inc = (smoothstep(17.35, 18.5, time)-smoothstep(18.5, 21.0, time)) * (time-16.0) * 0.1;
	mat2 rotMat = RotateMat(inc);
	for (int i = 1; i < 50; i++)
	{
		pixel = pixel * rotMat;
		float depth = 40.0+float(i) + smoothstep(18.0, 21.0, time)*65.;
		vec2 uv = pixel * depth/210.0;
		// Shifting the pan to the text near the end...
		// And shifts to the right again for the last line of text at 23:00!
		
		  
    //vec3 col = IMG_NORM_PIXEL(inputImage, uv3).xyz;
    
    
		col = IMG_NORM_PIXEL(inputImage, fract(uv+vec2(.5 + smoothstep(20.0, 21.0, time)*.11
		 + smoothstep(23.0, 23.5, time)*.04
		  , .7-smoothstep(20.0, 21.0, time)*.2))).rgb;
		  col = mix(col, col * Colour((float(i)/50.0+TIME)), smoothstep(18.5, 21.5, time));
		  if ((1.0-(col.y*col.y)) < float(i+1) / 50.0)
		  {
		  	break;
		  }
	}
	// Some contrast...
	col = min(col*col*1.5, 1.0);
	// Fade to red evil face...
	float gr = smoothstep(17., 16., time) + smoothstep(18.5, 21.0, time);
	float bl = smoothstep(17., 15., time) + smoothstep(18.5, 21.0, time);
	col = col * vec3(1.0, gr, bl);
	// Cut off the messy end...
	col *= smoothstep(29.5, 28.2, time);
	gl_FragColor = vec4(col, 1.0);
}

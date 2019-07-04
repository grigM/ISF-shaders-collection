/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "8de3a3924cb95bd0e95a443fff0326c869f9d4979cd1d5b6e94e2a01f5be53e9.jpg"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tjfWd by battlebottle.  Modified from:\n\nhttps:\/\/www.shadertoy.com\/view\/4d2Xzw\n\nuse the mouse to change the blur intensity and day-night cycle",
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
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


const float SRGB_GAMMA = 1.0 / 2.2;

//Modified from:
//
//https://www.shadertoy.com/view/4d2Xzw
//
//A bokeh blur is calculated using by evenly sampling the area of a circle around each pixel, 
//and averaging the result. With a limited set of samples this creates visual artefacts as the 
//blur radius gets bigger. Using lod hides these artifacts without significantly decreasing the 
//blur quality in general.
//
//My math for calculating the LOD is probably wrong, but it's close enough to prove the concept.
//High intensity lights were added to the photo of London to help highlight the bokeh effect.


#define GOLDEN_ANGLE 2.39996

#define ITERATIONS 128

const float SRGB_GAMMA = 1.0 / 2.2;

mat2 rot = mat2(cos(GOLDEN_ANGLE), sin(GOLDEN_ANGLE), -sin(GOLDEN_ANGLE), cos(GOLDEN_ANGLE));

//Sourced from: //https://www.shadertoy.com/view/4d2Xzw
vec3 Bokeh(sampler2D tex, vec2 uv, float radius)
{
	vec3 acc = vec3(0);
    float r = 1.;
    
    vec2 vangle = vec2(0.0,radius*.01 / sqrt(float(ITERATIONS)));
    
	for (int j = 0; j < ITERATIONS; j++)
    {
        r += 1. / r;
	    vangle = rot * vangle;
        vec3 col = textureLod(tex, uv + (r-1.) * vangle, sqrt(radius)).xyz * 1.0;
        acc += col;
	}
    
	return acc / (vec3(float(ITERATIONS)));
}


void main() {
	if (PASSINDEX == 0)	{


	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	   
	    //Sample texture and convert to linear sRGB color space
	    gl_FragColor =  vec4(pow(IMG_NORM_PIXEL(iChannel0,mod(uv,1.0)).rgb, vec3(1.0 / SRGB_GAMMA)), 1.0);
	    
	    //Day-night cycle
	    float dayNightCycle = sin(TIME / 5.0) / 2.0 + 0.5;
	    
	    if (iMouse.z > 0.0) {
	     	dayNightCycle = 1.0 - iMouse.y / RENDERSIZE.y;
	    }
	    
	    
	    vec3 nightColor = vec3(0.1, 0.15, 0.9);
	    vec3 dayColor = vec3(1.0, 0.9, 0.95);
	    vec3 lightColor = nightColor * dayNightCycle + dayColor * (1.0 - dayNightCycle);
	    
	    gl_FragColor = vec4(gl_FragColor.rgb * lightColor, 1.0);
	    
	    
	    //Street lamps
		if(distance( uv, vec2(0.1,0.54)) < 0.01){
		    gl_FragColor += vec4(vec3(3.6, 3.4, 5.33) * 6.0, 1.0);
		}
		if(distance( uv, vec2(0.382,0.53)) < 0.008){
		    gl_FragColor += vec4(vec3(3.6, 3.4, 5.33) * 6.0, 1.0);
		}
		if(distance( uv, vec2(0.672,0.53)) < 0.008){
		    gl_FragColor += vec4(vec3(3.6, 3.4, 5.33) * 6.0, 1.0);
		}
	    
	    //Bus break lights
		if(distance( uv, vec2(0.22,0.354)) < 0.003){
		    gl_FragColor += vec4(vec3(4.6, 2.4, 1.33) * 4.0, 1.0);
		}
		if(distance( uv, vec2(0.255,0.357)) < 0.003){
		    gl_FragColor += vec4(vec3(4.6, 2.4, 1.33) * 4.0, 1.0);
		}
	    
	    //Bus indicator light
	    if (mod(TIME, 1.0) > 0.5) {
			if(distance( uv, vec2(0.22,0.365)) < 0.003){
			    gl_FragColor += vec4(vec3(4.6, 4.4, 1.33) * 4.0, 1.0);
			}
	    }
	}
	else if (PASSINDEX == 1)	{


	    //get uv with correct aspect ratio
	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.x + vec2(0.0,0.15); 
	    
	    //calculate the blur radius we want
		float blurRadius = 1.6 - 1.6*cos(mod(TIME*.10 +.25, 3.0) * 6.283); 
	    
	    if (iMouse.z > 0.0) {
	     	blurRadius = (iMouse.x / RENDERSIZE.x) ;   
	    	blurRadius *= 8.0;
	    }
	    
	    //Get a bokeh blurred sample of Buffer A and convert to sRGB
	    gl_FragColor = vec4(pow((Bokeh(BufferA, uv, blurRadius)), vec3(SRGB_GAMMA)), 1.0);
	}
}

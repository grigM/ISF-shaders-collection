/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/4tjfWd by battlebottle.  Modified from:\n\nhttps://www.shadertoy.com/view/4d2Xzw\n\nuse the mouse to change the blur intensity and day-night cycle",
    "IMPORTED": {
        "iChannel0": {
            "NAME": "iChannel0",
            "PATH": "8de3a3924cb95bd0e95a443fff0326c869f9d4979cd1d5b6e94e2a01f5be53e9.jpg"
        }
    },
    "INPUTS": [
        {
            "NAME": "iMouse",
            "TYPE": "point2D"
        }
    ],
    "PASSES": [
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferA"
        },
        {
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

#define PI 3.14159265359

const float SRGB_GAMMA = 1.0 / 2.2;

mat2 rot = mat2(cos(GOLDEN_ANGLE), sin(GOLDEN_ANGLE), -sin(GOLDEN_ANGLE), cos(GOLDEN_ANGLE));


vec2 randomPointInCircle(vec2 rand) {
	float a = rand.x * 2.0 * PI;
	float r = sqrt(rand.y);
	float x = r * cos(a);
	float y = r * sin(a);
    return vec2(x,y);
    
    
}


//Sourced from: //https://www.shadertoy.com/view/4d2Xzw
vec3 Bokeh(sampler2D tex, vec2 uv, float radius, sampler2D noise, vec2 noiseCoord)
{
	vec3 acc = vec3(0);
    float r = 1.;
    
    int iters = int(max(1.0, float(ITERATIONS) * pow(radius, 2.0)));
    
    vec2 vangle = vec2(0.0,radius*.01 / sqrt(float(ITERATIONS)));
    
	for (int j = 0; j < iters; j++)
    {
        r += 1. / r;
	    vangle = rot * vangle;
        vec3 col = textureLod(tex, uv + (r-1.) * vangle, 0.0).xyz;//(radius / 1.5) * sqrt(128.0 / float(iters)) ).xyz * 1.0;
        acc += col;
	}
    
	return acc / (vec3(float(iters)));
}


const mat3 ACESInputMat = mat3(
    0.59719, 0.35458, 0.04823,
    0.07600, 0.90834, 0.01566,
    0.02840, 0.13383, 0.83777
);

// ODT_SAT => XYZ => D60_2_D65 => sRGB
const mat3 ACESOutputMat = mat3(
     1.60475, -0.53108, -0.07367,
    -0.10208,  1.10813, -0.00605,
    -0.00327, -0.07276,  1.07602
);

vec3 RRTAndODTFit(vec3 v)
{
    vec3 a = v * (v + 0.0245786) - 0.000090537;
    vec3 b = v * (0.983729 * v + 0.4329510) + 0.238081;
    return a / b;
}

vec3 ACESFitted(vec3 color)
{
    color = color * ACESInputMat;

    // Apply RRT and ODT
    color = RRTAndODTFit(color);

    color = color * ACESOutputMat;

    // Clamp to [0, 1]
    color = clamp(color, 0.0, 1.0);

    return color;
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
			if(distance( uv, vec2(0.219,0.365)) < 0.0025){
			    gl_FragColor += vec4(vec3(4.6, 4.4, 1.33) * 4.0, 1.0);
			}
	    }
	    
	    //fade out
	    
	    if ((1.0 - dayNightCycle) < 0.12) {
	    	float fade = (1.0 - dayNightCycle) * 10.0 -.2;
	        gl_FragColor *= fade;
	        gl_FragColor = max(gl_FragColor, vec4(0.0));
	    };
	    
	    
	}
	else if (PASSINDEX == 1)	{


	    //get uv with correct aspect ratio
	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.x + vec2(0.0,0.15); 
	    
	    //calculate the blur radius we want
		float blurRadius = 1.6 - 1.6*cos(mod(TIME*.10 +.25, 3.0) * 6.283); 
	    
	    if (iMouse.z > 0.0) {
	     	blurRadius = iMouse.x / RENDERSIZE.x ;   
	    	blurRadius *= 4.0 ;//* (RENDERSIZE.x / 512.0);
	    }
	    
	    
	    //apply bokeh blur
	    vec3 bokeh = Bokeh(BufferA, uv, blurRadius, iChannel1, gl_FragCoord.xy);
	    
	    //apply ACES Filmic tonemap
	    vec3 bokehACES = ACESFitted(bokeh);
	    
	    //Get a bokeh blurred sample of Buffer A and convert to sRGB
	    gl_FragColor = vec4(pow(bokehACES, vec3(SRGB_GAMMA)), 1.0);
	    
	    //gl_FragColor = texture(iChannel1,  );
	}

}

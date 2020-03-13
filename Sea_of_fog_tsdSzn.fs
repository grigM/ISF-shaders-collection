/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/tsdSzn by jaszunio15.  Many thanks to pik33. I was inspired by his shader: \nhttps://www.shadertoy.com/view/wdcSzr",
    "IMPORTED": {
    },
    "INPUTS": [
    ],
    "PASSES": [
        {
        },
        {
        }
    ]
}

*/


#define TIMESCALE 1.0

//Uncoment if you have better PC
//#define HIGH_QUALITY

//Display parameters
#ifdef HIGH_QUALITY 
	#define STRENGTH 0.7
    #define LAYERS_COUNT 200.0
    #define LAYERS_DISTANCE 0.00225
    #define COLOR_MULTIPLIER 0.05
    #define NOISE_SHARPNESS 0.9
    #define VOLUMETRIC_CUT_WIDTH 0.01
    #define DISTORTION_POW 1.3
    #define DISTORTION_SPEED 0.4
    #define DISTORTION_BASE_ITERATION 8.0
    #define DISTORTION_ITERATIONS 20.0
    #define FOG_DISTANCE 17.0
#else
	#define STRENGTH 0.7
    #define LAYERS_COUNT 45.0
    #define LAYERS_DISTANCE 0.0095
    #define COLOR_MULTIPLIER 0.074
    #define NOISE_SHARPNESS 1.0
    #define VOLUMETRIC_CUT_WIDTH 0.05
    #define DISTORTION_POW 1.3
    #define DISTORTION_SPEED 0.4
    #define DISTORTION_BASE_ITERATION 8.0
    #define DISTORTION_ITERATIONS 15.0
    #define FOG_DISTANCE 17.0
#endif


//Misc
#define TIME (TIME * TIMESCALE)

//Useful functions
float hash12(vec2 x)   
{
    return fract(sin(dot(x, vec2(342.243, 234.4281))) * 235.2412);
}

float hash11(float x)   
{
    return fract(sin(x * 342.243) * 235.2412);
}

float noise12(vec2 uv)
{
 	vec2 rootUV = floor(uv);
    vec2 fractUV = smoothstep(0.0, 1.0, fract(uv));
    
    float v00 = hash12(rootUV + vec2(0.0, 0.0));
    float v01 = hash12(rootUV + vec2(0.0, 1.0));
    float v10 = hash12(rootUV + vec2(1.0, 0.0));
    float v11 = hash12(rootUV + vec2(1.0, 1.0));
    
    float v0 = mix(v00, v01, fractUV.y);
    float v1 = mix(v10, v11, fractUV.y);
    
    return pow(mix(v0, v1, fractUV.x), NOISE_SHARPNESS);
}
/*
	Many thanks to pik33. I was inspired by his shader: 
	https://www.shadertoy.com/view/wdcSzr

	I reused his idea to distort space using only sine functions, but I made it as a 3D volumetric plane.

	You can switch to HIGH_QUALITY mode in the common tab :)

	Preview generated at 11.88 s.

	1.01 - add HIGH QUALITY
	1.02 - add balanced HIGH_QUALITY and vignette 
	1.03 - lower flight
*/

vec3 colorFromUV(vec2 uv, float shift)
{
 	return 0.7 + 0.3*cos(TIME * 0.2 + uv.xyx * 0.1 + vec3(0,2,4) + pow(shift, 4.0)); //XD
}

vec3 fancyLayer(vec2 uv, float cut)
{
    uv *= 0.1;

    for (float i = DISTORTION_BASE_ITERATION; i <= DISTORTION_BASE_ITERATION + DISTORTION_ITERATIONS; i++)
    {
     	uv.x += STRENGTH * sin(uv.y * pow(DISTORTION_POW, i) + TIME * DISTORTION_SPEED + i * 0.18) / pow(DISTORTION_POW, i);
        uv.y += STRENGTH * sin(uv.x * pow(DISTORTION_POW, i) + TIME * DISTORTION_SPEED + i * 0.21) / pow(DISTORTION_POW, i);
    }
    
    float fancyness = noise12(uv * 10.0 + TIME * 0.1 * 5.0);
    float noise = noise12(uv * 2.0 + TIME * 0.5 + 21.0);
    
    vec3 col = colorFromUV(uv, noise);
    
    return smoothstep(cut - VOLUMETRIC_CUT_WIDTH, cut + VOLUMETRIC_CUT_WIDTH, fancyness) * (2.0 + fancyness) / 3.0 * col;
}


vec3 uvToCastPlane(vec2 uv)
{
 	return vec3(uv.x, uv.y, -1.0 + sin(TIME * 0.2) * 0.2);   
}

//xy - plane uv
//z - distance to plane point
vec3 rayCastPlane(vec3 rayOrigin, vec3 rayDirection, float planeHeight)
{
    rayDirection /= rayDirection.y;
    float distanceToPlane = abs(rayOrigin.y - planeHeight);
    rayDirection *= distanceToPlane;
    return vec3(rayOrigin.xz + rayDirection.xz, length(rayDirection));
}

void main() {
	if (PASSINDEX == 0)	{
	}
	else if (PASSINDEX == 1)	{


	    vec2 uv = (2.0 * gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.x;
	    
	    //Camera rotation and position
	    vec3 angle = vec3(-0.42 + cos(TIME * 0.3) * 0.08, sin(TIME * 0.2) * 0.5, cos(TIME * 0.21) * 0.1);
	    mat3x3 rotationMatrix = mat3x3(cos(angle.z), -sin(angle.z), 0.0,
	                                  sin(angle.z), cos(angle.z), 0.0,
	                                  0.0, 0.0, 1.0)
	        				  * mat3x3(1.0, 0.0, 0.0,
	                                  0.0, cos(angle.x), -sin(angle.x),
	                                  0.0, sin(angle.x), cos(angle.x))
	        				  * mat3x3(cos(angle.y), 0.0, -sin(angle.y),
	                                  0.0, 1.0, 0.0,
	                                  sin(angle.y), 0.0, cos(angle.y));
	    
	    vec3 cameraShift = vec3(0.0, sin(TIME * 0.24) * 0.12 - 0.36, TIME * 1.3);
	    
	    //Creating ray
	    vec3 rayOrigin = vec3(0.0, 0.0, 0.0) + cameraShift;
	    vec3 castPoint = uvToCastPlane(uv) * rotationMatrix + cameraShift;
	    vec3 rayDirection = castPoint - rayOrigin;
	    
	    //Raycast vase plane to get fog
	    vec3 planeUV = rayCastPlane(rayOrigin, rayDirection, -0.5);
	    float fog = 0.0;
	    if(rayDirection.y > 0.0) fog = 1.0;
	    else fog = sqrt(smoothstep(-0.1, FOG_DISTANCE, distance(cameraShift.xz, planeUV.xy)));
	
	    //ambient lighting
	    vec3 backgroundColor = colorFromUV(rayOrigin.xz, 0.0) * 0.3 * smoothstep(0.14, 0.0, abs(rayDirection.y - 0.02));
	    
	    //Adding many planes with small height differencies to create volumentric effect
	    vec3 col = vec3(0.0) + colorFromUV(rayOrigin.xz, 0.0) * 0.2;
	    
	    for (float i = 1.0; i <= LAYERS_COUNT ; i++)
	    {
	        planeUV = rayCastPlane(rayOrigin, rayDirection, -0.5 - i * LAYERS_DISTANCE - hash12(planeUV.xy) * LAYERS_DISTANCE);
	        col += fancyLayer(planeUV.xy, 1.1 - pow(i / LAYERS_COUNT, 2.0)) * COLOR_MULTIPLIER * pow(0.99, i);
	    }
	    
	    //Mixing volumetric plane, fog and ambient lighting with some postprocessing
	    float vignette = smoothstep(2.5, 0.4, length(uv));
		col = smoothstep(-0.0, 1.1, col * (1.0 - fog) + backgroundColor * fog) * vignette;
	    
	    //Output to screen
	    gl_FragColor = vec4(col,1.0);
	}

}

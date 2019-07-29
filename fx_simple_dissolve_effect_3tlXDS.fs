/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/3tlXDS by mrwen33.  简单的溶解特效",
    "IMPORTED": {
        "iChannel0": {
            "NAME": "iChannel0",
            "PATH": "92d7758c402f0927011ca8d0a7e40251439fba3a1dac26f5b8b62026323501aa.jpg"
        },
        "iChannel2": {
            "NAME": "iChannel2",
            "PATH": "bd6464771e47eed832c5eb2cd85cdc0bfc697786b903bfd30f890f9d4fc36657.jpg"
        }
    },
    "INPUTS": [
    
    	{
     		"NAME" : "inputImage",
      		"TYPE" : "image"
    	},
    	
    	
    ],
    "PASSES": [
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferA"
        },
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferB"
        },
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferC"
        },
        {
        }
    ]
}

*/


//Ramp texture

//origin render result

//iChannel0: 溶解噪声
//BufferA: 边沿火焰材质
//iChannel2: MainTex
vec4 noiseMask(vec2 uv, float scale, vec2 speed){
    uv.x = uv.x*RENDERSIZE.x/RENDERSIZE.y;
    vec4 sampleColor = IMG_NORM_PIXEL(iChannel0,mod(uv*scale+TIME*speed,1.0));
    float gray = sampleColor.x;
	return vec4(gray,gray,gray,gray);
}

vec4 colorRamp(float x){
	return IMG_NORM_PIXEL(BufferA,mod(vec2(x,x),1.0));
}

vec4 mainTex(vec2 uv){
		return IMG_NORM_PIXEL(inputImage,mod(uv,1.0));
}

float fireEdge(float dissolvedNoise, float fireSize){
	return step(0.0-fireSize, dissolvedNoise)*(1.0-step(0.0, dissolvedNoise));
}

vec4 fire(float dissolvedNoise, float fireSize){
	float coord = (dissolvedNoise+fireSize)/fireSize;
    return fireEdge(dissolvedNoise, fireSize)*colorRamp(coord);
}

//gaussian shader

vec3 ACESToneMapping(vec3 color)
{
    
    const float A = 2.51;
    const float B = 0.03;
    const float C = 2.43;
    const float D = 0.59;
    const float E = 0.14;
    return (color * (A * color + B)) / (color * (C * color + D) + E);
}

void main() {
	if (PASSINDEX == 0)	{


	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	   	float totalColumn = 4.0;
	    float wid = 1.0/totalColumn;
	    float column = floor(uv.x/wid)+1.0;
	    float c = wid*column;
	    gl_FragColor = vec4(c,0,0,1.0);
	}
	else if (PASSINDEX == 1)	{


	    // Normalized pixel coordinates (from 0 to 1)
	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	    float dissolveAmount = cos(TIME)*0.5+0.5;
	    float noiseScale = 0.5;
	    vec2 speed = vec2(0,0);
	    float fireSize = 0.3;
	    fireSize = min(1.0-dissolveAmount, fireSize);
	    
	    vec4 noise = noiseMask(uv, noiseScale, speed);
	    vec4 dissolvedNoise = noise-dissolveAmount;
	    float visible = step(0.0, dissolvedNoise.x);
	    vec4 outColor = mainTex(uv)*visible+fire(dissolvedNoise.x, fireSize);
	    // Output to screen
	    gl_FragColor = outColor;
	}
	else if (PASSINDEX == 2)	{


	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	    
	    gl_FragColor = vec4(0.0,0.0,1.0,1.0);
	}
	else if (PASSINDEX == 3)	{


	    // Normalized pixel coordinates (from 0 to 1)
	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	    vec4 outColor = IMG_NORM_PIXEL(BufferB,mod(uv,1.0));
	    //outColor = vec4(ACESToneMapping(outColor.rgb), outColor.a);
	    gl_FragColor = outColor;
	}

}

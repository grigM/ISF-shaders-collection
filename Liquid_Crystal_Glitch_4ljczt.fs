/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/4ljczt by MacroMachines.  One time my iPhone 5 screen broke, and I spent hours recording the glitching liquid RGB smearing.",
    "IMPORTED": {
        "iChannel1": {
            "NAME": "iChannel1",
            "PATH": "8de3a3924cb95bd0e95a443fff0326c869f9d4979cd1d5b6e94e2a01f5be53e9.jpg"
        },
        "iChannel1_2": {
            "NAME": "iChannel1_2",
            "PATH": "fb918796edc3d2221218db0811e240e72e340350008338b0c07a52bd353666a6.jpg"
        },
        "iChannel2": {
            "NAME": "iChannel2",
            "PATH": "52d2a8f514c4fd2d9866587f4d7b2a5bfa1a11a0e772077d7682deb8b3b517e5.jpg"
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
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferB"
        },
        {
        }
    ]
}

*/


#define rate 1.5
#define push_RGB 10.09
#define billow 9.10
#define push_RGB2 1.9209
#define startFrame 100
#define leak 1.50
#define scale 0.201
#define didYOUeatALLthatACID 1.09729
#define feedback 0.45

#define rate 1.5
#define push_RGB 1.09
#define billow 2.10
#define push_RGB2 2.9209
#define startFrame 10
#define leak 1.90
#define scale 1.201
#define didYOUeatALLthatACID 1.19729
#define feedback 0.45

void main() {
	if (PASSINDEX == 0)	{
	    
	    
	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    
	    vec4 flow0  = IMG_NORM_PIXEL(BufferA,mod(uv,1.0));
	    vec4 flowIn = IMG_NORM_PIXEL(iChannel1,mod(uv,1.0));
	    vec4 flow2  = IMG_NORM_PIXEL(BufferB,mod(flow0.xy*0.1+uv,1.0));
	    
	    //if ( fract(TIME*0.2)>0.5 ) {gl_FragColor = mix(flow0,flow2,0.1*flow0.b) ;}
	    gl_FragColor *= IMG_NORM_PIXEL(BufferA,mod(uv,1.0)) * IMG_NORM_PIXEL(iChannel1,mod(uv,1.0));
	    if ( FRAMEINDEX < startFrame ) {gl_FragColor = IMG_NORM_PIXEL(iChannel1,mod(uv,1.0));} 
	    
	    else {
	        
	        vec2 vUv = gl_FragCoord.xy / RENDERSIZE.xy;
	        vec2 texel = rate / RENDERSIZE.xy;
	        vec3 uv = IMG_NORM_PIXEL(BufferA,mod(vUv*(1.0 + leak * 0.005),1.0)).xyz;
	        
	        float gt = mod(TIME*vUv.x*vUv.y, billow * 6.1415)*scale;
	        
	        vec2 d1 = vec2(uv.x * vec2(texel.x*cos(gt * uv.z), texel.y*sin(gt*uv.y)));
	        vec2 d2 = vec2(uv.y * vec2(texel.x*cos(gt * uv.x), texel.y*sin(gt*uv.y)));
	        vec2 d3 = vec2(uv.z * vec2(texel.x*cos(gt * uv.y), texel.y*sin(gt*uv.y)));
	        
	        float bright = (uv.x+uv.y+uv.z)/ push_RGB + push_RGB2;
	        
	        float r = IMG_NORM_PIXEL(BufferA,mod(vUv+ d1 * bright,1.0)).x;
	        float g = IMG_NORM_PIXEL(BufferA,mod(vUv+ d2 * bright,1.0)).y;
	        float b = IMG_NORM_PIXEL(BufferA,mod(vUv+ d3 * bright,1.0)).z;
	        //float m = distance(uv-iMouse.xy,0.5);
	        vec3 uvMix = mix(uv, vec3(r,g,b), didYOUeatALLthatACID);
	        
	        vec3 orig = IMG_NORM_PIXEL(iChannel1,mod(vUv,1.0)).xyz;
	        
	        gl_FragColor += vec4(mix(uvMix, orig, 0.50-feedback), 0.5);//1.0);
	        
	        
	        
	        
	    }
	}
	else if (PASSINDEX == 1)	{
	    
	    
	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    vec4 flow0 = IMG_NORM_PIXEL(iChannel2,mod(uv,1.0));
	    
	    if ( fract(TIME*0.2)>0.5 ) {gl_FragColor /= flow0 ;}
	    
	    if ( FRAMEINDEX < startFrame ) {gl_FragColor = IMG_NORM_PIXEL(iChannel1_2,mod(uv,1.0));} 
	    
	    else {
	        
	        vec2 vUv = gl_FragCoord.xy / RENDERSIZE.xy;
	        vec2 texel = rate / RENDERSIZE.xy;
	        vec3 uv = IMG_NORM_PIXEL(BufferA,mod(vUv*(1.0 + leak * 0.005),1.0)).xyz;
	        
	        float gt = mod(TIME*vUv.x*vUv.y, billow * 6.1415)*scale;
	        
	        vec2 d1 = vec2(uv.x * vec2(texel.x*cos(gt * uv.z), texel.y*sin(gt*uv.y)));
	        vec2 d2 = vec2(uv.y * vec2(texel.x*cos(gt * uv.x), texel.y*sin(gt*uv.y)));
	        vec2 d3 = vec2(uv.z * vec2(texel.x*cos(gt * uv.y), texel.y*sin(gt*uv.y)));
	        
	        float bright = (uv.x+uv.y+uv.z)/ push_RGB + push_RGB2;
	        
	        float r = IMG_NORM_PIXEL(BufferA,mod(vUv+ d1 * bright,1.0)).x;
	        float g = IMG_NORM_PIXEL(BufferA,mod(vUv+ d2 * bright,1.0)).y;
	        float b = IMG_NORM_PIXEL(BufferA,mod(vUv+ d3 * bright,1.0)).z;
	        
	        vec3 uvMix = mix(uv, vec3(r,g,b), didYOUeatALLthatACID);
	        
	        vec3 orig = IMG_NORM_PIXEL(iChannel1_2,mod(vUv,1.0)).xyz;
	        
	        gl_FragColor = vec4(mix(uvMix, orig, 0.50-feedback), 1.0);
	        
	    }
	}
	else if (PASSINDEX == 2)	{
	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    gl_FragColor = IMG_NORM_PIXEL(BufferA,mod(uv,1.0)) * IMG_NORM_PIXEL(BufferB,mod(uv,1.0));
	}

}

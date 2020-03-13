/*{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "CREDIT": null,
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/wt2XWW by edo_m18.  This is simplest way to create a sampling like wave that using just sin wave.",
    "INPUTS" : [
    	{
     		"TYPE" : "image",
      		"NAME" : "inputImage"
    	},
    	
    	
    	{
      		"NAME" : "wave",
      		"TYPE" : "float",
      		"MAX" : 40.0,
      		"DEFAULT" : 30.0,
      		"MIN" : 0.0
    	},
    	{
      		"NAME" : "speed",
      		"TYPE" : "float",
      		"MAX" : 30.0,
      		"DEFAULT" : 10.0,
      		"MIN" : 0.0
    	},
    
  	],
    "IMPORTED": {
        "iChannel0": {
            "NAME": "iChannel0",
            "PATH": "52d2a8f514c4fd2d9866587f4d7b2a5bfa1a11a0e772077d7682deb8b3b517e5.jpg"
        }
    },
    
    "ISFVSN": "2",
    "VSN": null
}
*/



float getHeight(vec2 uv)
{
    float len = length(uv);
    return sin(len * wave - TIME * speed) * pow(1.0 - len, 2.0);
}

void main() {



    //vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE.xy) / RENDERSIZE.x;
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
     
	vec3 col = vec3(0);
    
    const float e = 0.01;
    vec2 shiftX = vec2(0.5, 0.5);
    vec2 shiftY = vec2(0.5, 0.5);
    float hX = getHeight(uv + shiftX);
    float hx = getHeight(uv - shiftX);
    float hY = getHeight(uv + shiftY);
    float hy = getHeight(uv - shiftY);
    
    float yu = (hX - hx) * 0.5;
    float yv = (hY - hy) * 0.5;
    vec3 du = vec3(1.0, yu, 0.0);
    vec3 dv = vec3(0.0, yv, 1.0);
    
    vec3 n = normalize(cross(dv, du));
    
    uv.xy += n.xz;
    
    //gl_FragColor = IMG_NORM_PIXEL(inputImage,mod(uv,2.0));
    
    
   
	
	gl_FragColor = IMG_NORM_PIXEL(inputImage,uv);
}

/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "8de3a3924cb95bd0e95a443fff0326c869f9d4979cd1d5b6e94e2a01f5be53e9.jpg"
    },
    {
      "NAME" : "iChannel1",
      "PATH" : "f735bee5b64ef98879dc618b016ecf7939a5756040c2cde21ccb15e69a6e1cfb.png"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WsySzc by Veggiebob.  doing a little color shift from the center",
  "INPUTS" : [
	{
     		"NAME" : "inputImage",
      		"TYPE" : "image"
    },
    {
			"LABEL":"step_size",
			"NAME": "step_size",
			"TYPE": "float",
			"DEFAULT": 0.01,
			"MIN": 0.0,
			"MAX": 0.05
		},
  ]
}
*/


void main() {



    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    float disp = 0.01 + 0.005 * sin((atan((uv.y-0.5)/(uv.x-0.5))+TIME*0.1) * 20.);
    //float step_size = 0.01;
    vec2 d_out = (uv-0.5)/max(length(uv-0.5), smoothstep(-0.2, 0.2, sin(TIME))*0.6+0.7);
    float tr = IMG_NORM_PIXEL(inputImage,mod(uv - (disp+1.*step_size)*d_out,1.0)).r;
    float tg = IMG_NORM_PIXEL(inputImage,mod(uv - (disp+2.*step_size)*d_out,1.0)).g;
    float tb = IMG_NORM_PIXEL(inputImage,mod(uv - (disp+3.*step_size)*d_out,1.0)).b;
    vec3 col = vec3(tr, tg, tb);
    //col = IMG_NORM_PIXEL(iChannel0,mod(uv,1.0)).rgb;
    // Output to screen
    gl_FragColor = vec4(col,1.0);
}

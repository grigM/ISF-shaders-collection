/*
{
  
  "CATEGORIES" : [
    "shift",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MlcXRB by panda1234lee.  Easy split shift [similar effect: https:\/\/www.shadertoy.com\/view\/4tdXzj]",
   "INPUTS": [
		{
      		"TYPE" : "image",
      		"NAME" : "inputImage"
    	},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MAX": 5.0,
			"MIN": 0.0
		},
		{
			"NAME": "ofset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MAX": 2.0,
			"MIN": -2.0
		},
		{
			"NAME": "band",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MAX": 1.0,
			"MIN": 0.0
		}
      ]
}
*/


void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    //float ratio = RENDERSIZE.x / RENDERSIZE.y;
    float offset = 0.;
    
    // Benefited by iq's advice, Thanks very much!
    // ------------------
    //vec2 duvdx = dFdx(uv);
    //vec2 duvdy = dFdy(uv);
    // ------------------
    
    
    float t1 = uv.x - uv.y;
    
    if(t1 < -1.*band)
    {
    	offset = -1. * (sin(TIME*speed)-ofset);
    }
    else if(t1 > -1.*band && t1 < 0.)
    {
    	offset = 1.*(sin((TIME*speed)-ofset));
    }
    if(t1 > 0. && t1 < 1.*band)
    {
        offset = -1.*sin((TIME*speed)-ofset);
    }
    else if(t1 >band && t1 < 2.*band)
    {
    	offset = 1. * sin((TIME*speed)-ofset);
    }

	vec4 col0 = IMG_NORM_PIXEL(inputImage,mod(uv + vec2(offset, offset),1.0),-10.);
    
    vec4 col1 = vec4(smoothstep(0.,0.015, abs(t1 - 1.*band)));
    vec4 col2 = vec4(smoothstep(0.,0.015, abs(t1 + 0.*band)));
    vec4 col3 = vec4(smoothstep(0.,0.015, abs(t1 + 1.*band)));
    
    gl_FragColor = mix(col0, vec4(1.), (1.- col1)+(1.-col2)+(1.-col3));
    
    
/*
#if 0
    if(gl_FragCoord.x / gl_FragCoord.y > ratio)
    {
    	offset = 1. * sin(TIME);
    }
    else
    {
    	offset = -1.*sin(TIME);
    }
#else   
    
    float t1 = gl_FragCoord.x - ratio *gl_FragCoord.y;
    float t2 = RENDERSIZE.x - RENDERSIZE.y;
    float band = (RENDERSIZE.x - RENDERSIZE.y);
    if(t1 > t2)
    {
    	offset = sin(.5 *TIME);
    }
    else if( ((t1 + 1.*band) > t2) || ((t1 + 2.*band) < t2))
    {
    	offset = -sin(.5 *TIME);
    }
    else if( ((t1 + 2.*band) > t2) || ((t1 + 3.*band) < t2))
    {
    	offset = sin(.5 *TIME);
    }
    
    //gl_FragColor = vec4(t1 + 1.*band);
    
    
#endif    

    // My Test
    //gl_FragColor = IMG_NORM_PIXEL(iChannel0,mod(uv + offset,1.0));
    
    // ☆ Benefited by iq's advice, Thanks very much!
    // ------------------
    //gl_FragColor = texture2DGradEXT(iChannel0, uv + offset, duvdx, duvdy);
    // ------------------
    
    // ☆ Benefited by hornet's advice, Thanks very much!
    // ------------------
    vec4 col0 = IMG_NORM_PIXEL(iChannel0,mod(uv+offset,1.0),-10.0);
    // ------------------
    
    vec4 col1 = vec4(smoothstep(0.,0.015, abs(t1 + 1.*band)/RENDERSIZE.x));
    vec4 col2 = vec4(smoothstep(0.,0.015, abs(t1 + 0.*band)/RENDERSIZE.x));
    vec4 col3 = vec4(smoothstep(0.,0.015, abs(t1 - 1.*band)/RENDERSIZE.x));
    gl_FragColor = mix(col0, vec4(1.), (1.- col1)+(1.-col2)+(1.-col3));
 */
    
}
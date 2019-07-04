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
    "noise",
    "simple",
    "snow",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MdjSD3 by inferno.  just a quick test to get layered snow. Try different textures in channel1 to get large snowflakes, which looks more like a feather ;)\n\nbased on https:\/\/www.shadertoy.com\/view\/Xd2SDc",
   "INPUTS": [
		{
      		"TYPE" : "image",
      		"NAME" : "inputImage"
    	},
    	{
			"NAME": "showOneChanel",
			"TYPE": "bool",
			"DEFAULT": true
			
			
		},
		{
			"NAME": "intensity",
			"TYPE": "float",
			"DEFAULT": 1.12,
			"MIN": 1.0,
			"MAX": 1.5
			
		},
		{
			"NAME": "brightnes",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 1.0,
			"MAX":	3.0
			
		},
		{
			"NAME": "amount",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": 1.0,
			"MAX": 10.0
			
		},
		{
			"NAME": "rot",
			"TYPE": "float",
			"DEFAULT": 15.0,
			"MIN": -30.0,
			"MAX": 30.0
			
		},
		
		{
			"NAME": "showFloor",
			"TYPE": "bool",
			"DEFAULT": false
			
			
		},
		
		{
			"NAME": "sLevel",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.5,
			"MAX": 6.0
			
		}
      ]
}
*/




void main() {




	vec2 uv = (gl_FragCoord.xy / RENDERSIZE.xy);

	float imgRot = radians(0.0);
    
    //float imgRot = radians(TIME * 45.0);
   	uv-=.5;
    
    mat2 m = mat2(cos(imgRot), -sin(imgRot), sin(imgRot), cos(imgRot));
    
    uv  = m * uv;
    uv+=.5;
    
    //uv.y = uv.y * -1.0; 
    vec3 col;
    if(showOneChanel){
 	   col=IMG_NORM_PIXEL(inputImage,mod(uv,1.0)).rrr;
    }else{
    	col=IMG_NORM_PIXEL(inputImage,mod(uv,1.0)).xyz;
    }

    col = vec3(col.r , col.g, col.b )* brightnes;
    uv.y = uv.y * (6.0-sLevel); 
    
    
    if(showFloor){
    	float ts=uv.y-.2+sin(uv.x*4.0+7.4*cos(uv.x*10.0))*0.005;
    	col=mix(col,vec3(1.5* (.85+TIME* 0.02)-uv.y),smoothstep(0.45,0.0,ts));
    }
    
    float c=cos(radians(0.0)+rot*0.01),si=sin(radians(0.0)+rot*0.01);
    uv=(uv-0.5)*mat2(c,si,-si,c);	
    
    float s=IMG_NORM_PIXEL(iChannel1,mod(uv * 1.01 +vec2(TIME)*vec2(0.02,0.501),1.0)).r;
    col=mix(col,vec3(1.0),smoothstep(0.9,1.0, s * .9 * intensity));
    
    if(amount>2.0){
 	    s=IMG_NORM_PIXEL(iChannel1,mod(uv * 1.07 +vec2(TIME)*vec2(0.02,0.501),1.0)).r;
		col=mix(col,vec3(1.0),smoothstep(0.9,1.0, s * 1. * intensity));
    }
    
    if(amount>3.0){
    	s=IMG_NORM_PIXEL(iChannel1,mod(uv+vec2(TIME)*vec2(0.05,0.5),1.0)).r;
    	col=mix(col,vec3(1.0),smoothstep(0.9,1.0, s * .98 * intensity));
	}
	
	if(amount>4.0){
		s=IMG_NORM_PIXEL(iChannel1,mod(uv * .9 +vec2(TIME)*vec2(0.02,0.51),1.0)).r;
    	col=mix(col,vec3(1.0),smoothstep(0.9,1.0, s * .99 * intensity));
	}
	
	if(amount>5.0){
		s=IMG_NORM_PIXEL(iChannel1,mod(uv * .75 +vec2(TIME)*vec2(0.07,0.493),1.0)).r;
    	col=mix(col,vec3(1.0),smoothstep(0.9,1.0, s * 1. * intensity));
	}
	if(amount>6.0){
		s=IMG_NORM_PIXEL(iChannel1,mod(uv * .5 +vec2(TIME)*vec2(0.03,0.504),1.0)).r;
    	col=mix(col,vec3(1.0),smoothstep(0.94,1.0, s * 1. * intensity));
	}
	if(amount>7.0){
		s=IMG_NORM_PIXEL(iChannel1,mod(uv * .3 +vec2(TIME)*vec2(0.02,0.497),1.0)).r;
 	   col=mix(col,vec3(1.0),smoothstep(0.95,1.0, s * 1. * intensity));
	}
	if(amount>8.0){
		s=IMG_NORM_PIXEL(iChannel1,mod(uv * .1 +vec2(TIME)*vec2(0.0,0.51),1.0)).r;
    	col=mix(col,vec3(1.0),smoothstep(0.96,1.0, s * 1. * intensity));
	}
	if(amount>9.0){
		s=IMG_NORM_PIXEL(iChannel1,mod(uv * .03 +vec2(TIME)*vec2(0.0,0.523),1.0)).r;
    	col=mix(col,vec3(1.0),smoothstep(0.99,1.0, s * 1. * intensity));
	}
	gl_FragColor = vec4(col,1.0);
}

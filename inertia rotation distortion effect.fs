/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "ISFVSN": "2",
  "INPUTS" : [
{
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
		{
			"NAME": "deform_amp",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 2.0
			
		},
		
		
		{
			"NAME": "deform_freq",
			"TYPE": "float",
			"DEFAULT": 4.0,
			"MIN": 1.0,
			"MAX": 20.0
			
		},
		
		{
			"NAME": "deform_speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 6.0
			
		},
		
			
		{
			"NAME": "zoom",
			"TYPE": "float",
			"MIN": 0.5,
			"MAX": 5.0,
			"DEFAULT": 1.0
		},
		{
			"NAME": "center",
			"TYPE": "point2D",
			"DEFAULT": [
				0.5,
				0.5
			]
		},
		
		{
      "LABELS" : [
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6"
      ],
      "NAME" : "distant_type",
      "TYPE" : "long",
      "DEFAULT" : 0,
      "VALUES" : [
        0,
        1,
        2,
        3,
        4,
        5,
        6
       ]
    },
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36074.0"
}
*/

#define PI 3.14159265359

#ifdef GL_ES
precision mediump float;
#endif

// inertia rotation --joltz0r

#define TIME 0.2*TIME


void main( void ) {


	vec2		p;
	vec2		modifiedCenter;
	
	p = isf_FragNormCoord;
	modifiedCenter = center / RENDERSIZE;
	
	
	
	
	
	//vec2 p = gl_FragCoord.xy / RENDERSIZE.xy;
	
	//vec2 p = ((gl_FragCoord.xy / RENDERSIZE) - 0.5) * 2.0;
	//p.x *= RENDERSIZE.x/RENDERSIZE.y;
	//p.y *= RENDERSIZE.x/RENDERSIZE.y;
	
	
	//p.x += x_ofset;
	//p.y += y_ofset;
	//p.y /= RENDERSIZE.x/RENDERSIZE.y;	


	//inertia towards the edges
	float t = sin((TIME*deform_speed)*10. - deform_freq*distance(p, vec2(0.0)))*deform_amp;
	
	if(distant_type==0){
		p *= mat2(cos(t), -sin(t), sin(t),  cos(t));
	}else if(distant_type==1){
		p *= mat2(exp(t), -atan(t), -tan(t),  exp(t));
	}else if(distant_type==2){
		p *= mat2(exp(t), atan(t), -tan(t),  exp(t));
	}else if(distant_type==3){
		p *= mat2(exp(t), -sin(t), sin(t),  exp(t));
	}else if(distant_type==4){
		p *= mat2(acos(t), -asin(t), asin(t),  atan(t));
		
	}else if(distant_type==5){
		p *= mat2(cos(t), -sin(t), -sin(t),  cos(t));
	}else if(distant_type==6){
		p *= mat2(exp2(t), -exp(t), -log(t),  exp2(t));
		p.x = p.x+0.5;
		p.y = p.y-0.5;
	}
	
	
	p.x = (p.x - modifiedCenter.x)*(1.0/zoom) + modifiedCenter.x;
	p.y = (p.y - modifiedCenter.y)*(1.0/zoom) + modifiedCenter.y;
	
	
	/*
	if ((p.x < 0.0)||(p.y < 0.0)||(p.x > 1.0)||(p.y > 1.0))	{
		gl_FragColor = vec4(0.0);
	}
	else	{
		gl_FragColor = IMG_NORM_PIXEL(inputImage,p);
	}*/
	gl_FragColor = IMG_NORM_PIXEL(inputImage,p);
}
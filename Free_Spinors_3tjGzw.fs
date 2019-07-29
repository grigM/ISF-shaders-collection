/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/3tjGzw by wyatt.  Continuous polar field with quantized spin ",
    "IMPORTED": {
    },
    "INPUTS": [
        {
            "NAME": "iMouse",
            "TYPE": "point2D"
        },
         {
            "NAME": "reset",
            "TYPE": "event"
        },
        
         {
		"NAME": "buff_zoom_speed",
		"TYPE": "float",
		"DEFAULT": 0.998,
		"MIN": 0.95,
		"MAX": 0.998
	},
	
	
        {
		"NAME": "dot_size",
		"TYPE": "float",
		"DEFAULT": 20.0,
		"MIN": 0.0,
		"MAX": 150.0
	},

        {
		"NAME": "zoom",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": -600.0,
		"MAX": 600.0
	},
	{
		"NAME": "zoomDif",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 20.0
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


///#define R RENDERSIZE.xy
//#define A(U) IMG_NORM_PIXEL(iChannel0, (U)/R)
#define A(U) IMG_NORM_PIXEL(BufferA, (U)/RENDERSIZE.xy)
#define C(U) IMG_NORM_PIXEL(BufferB, (U)/RENDERSIZE.xy)
#define D(U) IMG_NORM_PIXEL(BufferC, (U)/RENDERSIZE.xy)

#define I 5.
#define M .1
#define O 0.5/1.

vec4 hash (vec2 p)
{
	vec4 p4 = fract(vec4(p.xyxy) * vec4(.1031, .1030, .0973, .1099));
    p4 += dot(p4, p4.wzxy+19.19);
    return fract((p4.xxyz+p4.yzzw)*p4.zywx);

}
void main() {
	
	float scale = RENDERSIZE.y / (zoom + sin((TIME*1.0) * 0.4) * zoomDif);
	 
	if (PASSINDEX == 0)	{
		
		
   		//vec2 p = (gl_FragCoord.xy - RENDERSIZE / 2.0) / scale;
    
    
    
		vec2 p = gl_FragCoord.xy;
	
		//p.xy *= zoom;
	    //p.xy += 0.5;
	    p.xy -= 0.5*RENDERSIZE.xy;
	    p.xy *= buff_zoom_speed;
	    p.xy += 0.5*RENDERSIZE.xy;
	    
	    
	    gl_FragColor = A(p.xy);
	   
	    vec4 d = D(p.xy);
	    float o = gl_FragColor.w;
	    float c = cos(o), si = sin(o);
	    gl_FragColor.w -= dot(gl_FragColor.xy,vec2(-d.y,d.x));
	    gl_FragColor = mix(gl_FragColor,D(p.xy),0.05);
	    gl_FragColor.xy *= mat2(c,-si,si,c);
	    if (length(gl_FragColor.xy)==0.||FRAMEINDEX < 1 ||((length(p.xy-(iMouse.xy))<dot_size))) {
	        
	        gl_FragColor = hash(p.xy)*2.-1.;
	        gl_FragColor.w = 2.;
	    }	    
	    
	    
	   
	    
	     if (length(gl_FragColor.xy)>0.) gl_FragColor.xy = normalize(gl_FragColor.xy);
	    	gl_FragColor.w += .2*(floor(0.5+gl_FragColor.w*7.)/7.-gl_FragColor.w);
	    	
	    
	
	}
	
	else if (PASSINDEX == 1)	{


	    gl_FragColor = vec4(0);
	    for (float i = -I; i <= I; i++) {
	     gl_FragColor += M*exp(-O*i*i)*A(gl_FragCoord.xy+vec2(i,0));
	    }
	}
	else if (PASSINDEX == 2)	{


	    gl_FragColor = vec4(0);
	    for (float i = -I; i <= I; i++) {
	    	gl_FragColor += M*exp(-O*i*i)*C(gl_FragCoord.xy+vec2(0,i));
	    }
	}
	else if (PASSINDEX == 3)	{
		
		//vec2 p = (gl_FragCoord.xy - RENDERSIZE / 2.0) / scale;
    	vec2 p = gl_FragCoord.xy;
    
    
		//p.xy += 0.5;
	    p.xy -= 0.5*RENDERSIZE.xy;
	    //p.xy *= 0.998;
	    //p.xy += zoom;
	    
	    p.xy += 0.5*RENDERSIZE.xy;
		


		gl_FragColor = A(p.xy);
	    
	    
	    gl_FragColor.z = -gl_FragColor.x;
	    float a = 3.7, c = cos(a), s = sin(a);
	    gl_FragColor.xy *= mat2(c,-s,s,c);
	    gl_FragColor.yz *= mat2(c,-s,s,c);
	    
	    
	    
	    
	}

}

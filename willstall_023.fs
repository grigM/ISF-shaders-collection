/*{
	"DESCRIPTION": "",
	"CREDIT": "",
	"ISFVSN": "2",
	"CATEGORIES": [
		"XXX"
	],
	"INPUTS": [
		
		{
			"NAME": "boolInput",
			"TYPE": "bool",
			"DEFAULT": 1.0
		},
		
		{
			"NAME": "floatInput",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		}
	],
	 "PASSES" : [
    {
      "TARGET" : "Buffer0",
      "PERSISTENT" : true
    },
    {
      "TARGET" : "Buffer1",
      "PERSISTENT" : true
    },
  ]
	
	
}*/
/*

    daily: 015
    author: Will Stallwood
    insta: https://www.instagram.com/willstall/
    
*/

#ifdef GL_ES
    precision mediump float;
#endif

#define PI 3.14159265359
#define HALF_PI 1.57079632675
#define TWO_PI 6.283185307

uniform vec2 u_mouse;



float sdEquilateralTriangle( in vec2 p )
{
    float k = sqrt(3.0);
    
    p.x = abs(p.x) - 1.0;
    p.y = p.y + 1.0/k;
    if( p.x + k*p.y > 0.0 ) p = vec2( p.x - k*p.y, -k*p.x - p.y )/2.0;
    p.x -= clamp( p.x, -2.0, 0.0 );
    return -length(p)*sign(p.y);
}

mat2 rotate(float angle)
{
    return mat2( cos(angle),-sin(angle),sin(angle),cos(angle) );
}

vec2 center(vec2 st)
{
    float aspect = RENDERSIZE.x/RENDERSIZE.y;
    st.x = st.x * aspect - aspect * 0.5 + 0.5;
    return st;
}

 void main()
 {
    // timing, if using cos+sin times are doubled
    float seconds = 10.0;
    float t = fract(TIME/seconds);

    // space
    vec2 st = gl_FragCoord.xy / RENDERSIZE.xy;
    // st = center( st );

    // vars
    vec3 buffer,color;
    vec3 sdf = vec3( 0.0, 0.0, 0.0 );

    if (PASSINDEX == 0)	{
    
        st = center( st );
       
		buffer = IMG_NORM_PIXEL(Buffer1, st).rgb;
		
					
        vec2 pos = vec2(0.5);
            pos.x += .3 * sin(TWO_PI*t);
            pos.y += .13 * cos(TWO_PI*t);

        // sdf.x = length(st-pos)-0.05;
        // sdf.x = 1.0 - smoothstep(0.0,0.001,sdf.x);

        st -= 0.5;
        st *= rotate(TWO_PI*t*3.0);
        st += 0.5;

        float base = .3 + .6 * sin(TWO_PI*3.0*t);
        sdf.x = sdEquilateralTriangle((st-pos)/base)*base;
        sdf.x = abs(sdf.x+.03)-.001;
        sdf.x = 1.0 - smoothstep(0.0,0.003,sdf.x);

        color = vec3(sdf.x, sdf.x, sdf.x );
        gl_FragColor = vec4(color, 1.0);

    }else if (PASSINDEX == 1)	{
    
    	vec3 buffer_1 = IMG_NORM_PIXEL(Buffer1, st).rgb;
        
        buffer = IMG_NORM_PIXEL(Buffer0, st).rgb;
        
        // buffer *= 0.999999;
        //color = mix(buffer_1,buffer,buffer*.003);        
        //float f = mod(120.0,60.0);
        
        color = mix(buffer,buffer_1, (1.0-buffer)*0.9);

        // color = vec3((1.0/u_fps));
        // (1.0-buffer)*4000000.9999999);
            // (1.0-buffer)*(1.0-mod(u_time,.075)* (1.0-mod(u_fps,60.0)) ));
        gl_FragColor = vec4(color, 1.0);
    
    }else{
    
        
		buffer =  IMG_NORM_PIXEL(Buffer0, st).rgb;
        color = vec3(0.07);
        color = mix(color,vec3(1.0),buffer.y);
        // color = vec3(length(buffer));
        // color = mix(color,buffer,0.3);
        gl_FragColor = vec4(color, 0.0);
    
    }
}
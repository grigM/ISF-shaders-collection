/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
  	{
		"NAME": "move_speed",
		"TYPE": "float",
		"DEFAULT": 1.01,
		"MIN": 0.01,
		"MAX": 2.0
		
	},
	{
		"NAME": "color_speed",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 5.0
		
	},
	
	{
		"NAME": "mono_color",
		"TYPE": "bool",
		"DEFAULT": false,
				
	},
	
  	{
		"NAME": "blur_enable",
		"TYPE": "bool",
		"DEFAULT": true,
				
	},
  	{
		"NAME": "blur_repeat_amnt",
		"TYPE": "float",
		"DEFAULT": 12.0,
		"MIN": 5,
		"MAX": 100.0
		
	},
	
  	{
		"NAME": "blur_amp",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0,
		"MAX": 2.0
		
	},
	
	
	{
		"NAME": "circ_scale",
		"TYPE": "float",
		"DEFAULT": 0.18,
		"MIN": 0.0,
		"MAX": 0.9
		
	},
	
	{
		"NAME": "circ_line_width",
		"TYPE": "float",
		"DEFAULT": 0.01,
		"MIN": 0.0,
		"MAX": 0.2
		
	},
	{
		"NAME": "circ_move_amp",
		"TYPE": "float",
		"DEFAULT": 0.025,
		"MIN": 0.0,
		"MAX": 0.4
		
	},
	
	
	{
		"NAME": "line_scale",
		"TYPE": "float",
		"DEFAULT": 0.05,
		"MIN": 0.0,
		"MAX": 0.29
		
	},
	{
		"NAME": "p3_scale",
		"TYPE": "float",
		"DEFAULT": 0.03,
		"MIN": 0.0,
		"MAX": 0.6
		
	},
	{
		"NAME": "p4_scale",
		"TYPE": "float",
		"DEFAULT": 0.05,
		"MIN": 0.0,
		"MAX": 0.6
		
	},
	{
		"NAME": "p5_scale",
		"TYPE": "float",
		"DEFAULT": 0.04,
		"MIN": 0.00,
		"MAX": 0.6
		
	},
	{
		"NAME": "p6_scale",
		"TYPE": "float",
		"DEFAULT": 0.03,
		"MIN": 0.00,
		"MAX": 0.6
		
	},
	
	{
		"NAME": "p1_scale",
		"TYPE": "float",
		"DEFAULT": 0.05,
		"MIN": 0.00,
		"MAX": 0.6
		
	},
	

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#45330.1"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/XlBGRz
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy globals

#define iResolution RENDERSIZE

// --------[ Original ShaderToy begins here ]---------- //
#define MOTION_BLUR

float lineSegDist(vec2 uv, vec2 lineDir, vec2 linePoint, float r) {
    vec2 ba = -lineDir * r;
    vec2 pa = uv - linePoint + ba;
    ba *= 2.0;
    return length(pa - ba*clamp( dot(pa, ba)/dot(ba, ba), 0.0, 1.0));
}

float aa(float dist, float threshold)
{
    float pixelSize = 1.0 / iResolution.y;
	return dist < threshold-pixelSize ? 0.0 : min(1.0, 1.0-(threshold-dist)/pixelSize);
}

float scene(vec2 uv, float t)
{
    vec2 p = vec2(sin(t*40.3), cos(t*10.0)*0.4);

    vec2 v = normalize(vec2(sin(t*8.0), cos(t*8.0)));
	vec2 p2 = vec2(sin(1.0+t*50.0), cos(0.7+t*24.0)*0.4);
    vec2 p3 = vec2(sin(t*37.3+2.0)*1.2, sin(t*2.0)*0.2+cos(1.0+t*21.0)*0.4);

    float r = (0.3);
    vec2 p4 = vec2(cos(t*60.0), sin(t*64.0)) * r*2.5;
    vec2 p5 = vec2(-cos(t*60.0+3.14159*2.0*0.3333), sin(t*65.0+3.14159*2.0*0.3333)) * r*2.0;
    vec2 p6 = vec2(cos(t*50.0+3.14159*2.0*0.6666), sin(t*55.0+3.14159*2.0*0.6666)) * r;
    vec2 p7 = vec2(cos(t*181.0)*circ_move_amp, cos(t*81.4)*circ_move_amp+sin(t*42.0)*circ_move_amp);

    return min(aa(length(uv-p6), p6_scale*(1.0+0.5*sin(t*50.0+3.14159*2.0*0.6666))),
           min(aa(length(uv-p5), p5_scale*(1.0+0.5*-sin(t*60.0+3.14159*2.0*0.3333))),
           min(aa(length(uv-p4),  p4_scale*(1.0+0.5*sin(t*60.0))),
           min(aa(length(uv-p3), p3_scale),
           min(aa(length(uv-p), p1_scale),
          	min(aa(abs(length(uv-p7)-circ_scale-0.01*cos(t*150.0)), circ_line_width+0.0001*tan(0.1*t)),
           //min(aa(abs(length(uv-p7)-circ_scale-0.0*cos(t*0.0)), circ_line_width+0.00*tan(0.0*t)),
               aa(lineSegDist(uv, v, p2, 0.5), line_scale)
              ))))));
}

float hash( vec2 v ) {
    return fract(sin(mod(dot(v.xy,vec2(12.9898,78.233)),3.14) * 43758.5453));
    //return texture(iChannel0, v, 0.0).r;
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	vec2 uv = vec2(iResolution.x/iResolution.y, 1.0) * (-1.0 + 2.0*fragCoord.xy / iResolution.xy);
	vec4 color;
	
	if(mono_color){
	    color = vec4(1.0,1.0,1.0,1.0);
	}else{
		color = vec4(abs(uv)+0.3,0.5+0.5*sin(20.0*(TIME*color_speed)),1.0);
	}
	    
	if(blur_enable){
	    fragColor = vec4(0.0);
	    
        for (float i=0.0; i<blur_repeat_amnt; i++) {
            float r = hash(mod(fragCoord+vec2(i,0.0), 64.0) / 64.0);
            fragColor += (1.0-scene(uv, (TIME*move_speed)+(blur_amp/58.5)*((i+r)/12.0))) * color;
        }
        fragColor /= 12.0;
	}else{
		fragColor = (1.0-scene(uv, (TIME*move_speed))) * color;
	}
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
  mainImage(gl_FragColor, gl_FragCoord.xy);
}
/*{
  "CREDIT": "by mojovideotech",
  "CATEGORIES": [
    "2d",
    "twist",
    "columns",
    "Automatically Converted"
  ],
  "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/Xl2GRc by iq.",
  "INPUTS": [
    {
      "NAME": "iChannel0",
      "TYPE": "image"
    },
    {
      "NAME": "UVSpace",
      "TYPE": "bool",
      "DEFAULT": 1
    },
    {
      "NAME": "phase2",
      "TYPE": "float",
      "DEFAULT": 0.2,
      "MIN": 0.01,
      "MAX": 4.0
    },
    {
      "NAME": "phase3",
      "TYPE": "float",
      "DEFAULT": 5,
      "MIN": 2,
      "MAX": 9
    },
    {
      "NAME": "zoom",
      "TYPE": "float",
      "DEFAULT": 6,
      "MIN": 1,
      "MAX": 20
    },
    {
      "NAME": "color",
      "TYPE": "float",
      "DEFAULT": 2.0,
      "MIN": 0.5,
      "MAX": 3.5
    },
    {
      "NAME": "X",
      "TYPE": "float",
      "DEFAULT": 0.5,
      "MIN": 0.0,
      "MAX": 1.2
    },
    {
      "NAME": "Y",
      "TYPE": "float",
      "DEFAULT": 0.5,
      "MIN": 0.1,
      "MAX": 1.0
    },
    {
      "NAME": "NumOfCircles",
      "TYPE": "float",
      "DEFAULT": 1.0,
      "MIN": 1.0,
      "MAX": 6.0
    },
    {
      "NAME": "SHADOW",
      "TYPE": "float",
      "DEFAULT": 6.28,
      "MIN": 3.14,
      "MAX": 18.0
    },
    {
      "NAME": "ValT",
      "TYPE": "float",
      "DEFAULT": 1.0,
      "MIN": 0.0,
      "MAX": 2.0
    }


    
  ]
}*/

// TotallyTwistedColumns by mojovideotech
// source : www.shadertoy.com/view/Xl2GRc
// created by IQ : www.iquilezles.org/
// interactive mods by DoctorMojo : www.mojovideotech.com/

///////////////////////////////////

// Created by inigo quilez - iq/2015
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

// Based on Flyguy's "Ring Teister" https://www.shadertoy.com/view/Xt23z3. I didn't write 
// this effect since around 1999. Only now it's antialiased, motion blurred, texture 
// filtered and high resolution. 

float TT = clamp (TIME, 0.0, 5.0);
vec4 segment( float x0, float x1, vec2 uv, float id, float time, float f )
{
    float u = (uv.x - x0)/(x1 - x0);
    float v =-1.0*(id+0.5)*time+2.0*uv.y/3.141593 + f*2.0;
    float w = (x1 - x0);
    
    vec3 col = IMG_NORM_PIXEL(iChannel0,mod(vec2(u,v),1.0)).xyz;
    col += color*sin( 2.0*f + 2.0*id + vec3(0.0,1.0,2.0) );

//col *= mix( 1.0, smoothstep(-0.95,-0.94, sin(8.0*6.2831*v + 3.0*u + 2.0*f)), smoothstep(0.4,0.5,sin(f*13.0)) );
//col *=mix( 1.0, smoothstep(-0.95,-0.94, sin(8.0*6.2831*v + 3.0*u + 2.0*f)), smoothstep(0.9,0.867,sin(f*17.0)) );
//col *= mix( 1.0, smoothstep(-0.8,-0.7, sin(80.0*v)*sin(20.0*u) ), smoothstep(0.4,0.5,sin(f*17.0)) );
    
     col *= smoothstep( 0.9, 0.05, 0.5-abs(u-0.5) );
    
    // lighting
    col *= vec3(0.0,0.1,0.3) + w*vec3(0.7,0.6,0.5);
    col *= mix(1.0-u,1.0,w*w*w*0.8);
    
    float edge = 1.0-smoothstep( 0.5,0.5+0.02/w, abs(u-0.5) );
    return vec4(col,  edge * step(x0,x1) );
    
}

const int numSamples = 4;

void main()
{
	vec2 uv = (-RENDERSIZE.xy+2.0*gl_FragCoord.xy) / max(RENDERSIZE.x,RENDERSIZE.y);

    uv *= 21.0-zoom;
    
    vec2 st = vec2( NumOfCircles *length(uv), atan(uv.y, uv.x) );
    st = (UVSpace==false) ? uv : st;  
    // cartersian coordinates

    float id = floor((st.x)/2.0);
    
    vec3 tot = vec3(0.0);
    for( int j=0; j<numSamples; j++ )
    {
        float h = float(j)/float(numSamples);
        float time = (TIME + h*(1.0/30.0))*0.1;
        
        vec3 col = vec3(0.2)*(1.0-0.08* (8.0*st.x));

        vec2 uvr = vec2( mod( st.x, X ) - Y, st.y );

        float a = uvr.y + (id+0.25*2.0) * 2.0*TT + phase2*sin(3.0*uvr.y)*sin(8.0*time);
        float r = 0.9;
        
        float x0 = r*sin(a);
        for(int i=0; i<5; i++ )
        {
            float f = float(i+1)/phase3;
            float x1 = r*sin((TT* ValT)*a + SHADOW*f );

            vec4 seg = segment(x0, x1, uvr, id, time, f );
            col = mix( col, seg.rgb, seg.a ); 
            
            x0 = x1;
        }
        col *= (1.6-0.1*st.x);
        tot += col;
    }
    
    tot = tot / float(numSamples);
    
 	gl_FragColor = vec4( tot, 1.0);
}
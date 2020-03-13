/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/4lXyRM by aferriss.  work in progress...experimenting with growing images with feedback and some simple rules. Somewhat hindered by the difficulty of doing frame differencing on shadertoy\n\nImage resets every 1000 frames",
    "IMPORTED": {
        "iChannel1": {
            "NAME": "iChannel1",
            "PATH": "0c7bf5fe9462d5bffbd11126e82908e39be3ce56220d900f633d58fb432e56f5.png"
        }
    },
    "INPUTS": [
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


const float punch = 0.1;
const float odds = 0.4;

float rand(float n){return fract(sin(n) * 43758.5453123);}

float noise(float p){
    float fl = floor(p);
    float fc = fract(p);
    return mix(rand(fl), rand(fl + 1.0), fc);
}

const float timeMod = 0.01;
const float amp = 0.8;

float rand(vec2 co){
     float a = 12.9898;
     float b = 78.233;
     float c = 43758.5453;
     float dt= dot(co.xy ,vec2(a,b));
     float sn= mod(dt,3.14);
    return fract(sin(sn) * c);
}

float noise(vec2 n) {
    const vec2 d = vec2(0.0, 1.0);
    vec2 b = floor(n), f = smoothstep(vec2(0.0), vec2(1.0), fract(n));
    return mix(mix(rand(b), rand(b + d.yx), f.x), mix(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}


void main() {
	if (PASSINDEX == 0)	{


	    vec2 tc = gl_FragCoord.xy / RENDERSIZE.xy;
	    
	    vec2 res = RENDERSIZE.xy;
	    float time = TIME*50.0;
	    vec2 offs = 1.0 / res;
	    
	    vec4 tex = IMG_NORM_PIXEL(BufferA,mod(tc,1.0));
	    vec4 n = IMG_NORM_PIXEL(BufferA,mod(tc + vec2(0.0,-offs.y),1.0));
	    vec4 e = IMG_NORM_PIXEL(BufferA,mod(tc + vec2(offs.x, 0.0),1.0));
	    vec4 s = IMG_NORM_PIXEL(BufferA,mod(tc + vec2(0.0, offs.y),1.0));
	    vec4 w = IMG_NORM_PIXEL(BufferA,mod(tc + vec2(-offs.x, 0.0),1.0));
	    
	    vec4 ne = IMG_NORM_PIXEL(BufferA,mod(tc + vec2(offs.x,-offs.y),1.0));
	    vec4 nw = IMG_NORM_PIXEL(BufferA,mod(tc + vec2(-offs.x, -offs.y),1.0));
	    vec4 se = IMG_NORM_PIXEL(BufferA,mod(tc + vec2(offs.x, offs.y),1.0));
	    vec4 sw = IMG_NORM_PIXEL(BufferA,mod(tc + vec2(-offs.x, offs.y),1.0));
	
	
	    vec2 pos = tc * res;
	    float p = pos.y * res.x + pos.x;
	    p /= (res.x*res.y);
	
	    n = mix(vec4(0.0), n, floor(noise(1000.0+time*0.05 + tc.x*res.x*punch)+odds));
	    e = mix(vec4(0.0), e, floor(noise(123.0+time*0.05 + tc.y*res.y*punch)+odds));
	    s = mix(vec4(0.0), s, floor(noise(-78.0+time*0.05 + tc.x*res.x*punch)+odds));
	    w = mix(vec4(0.0), w, floor(noise(42.0+time*0.05 + tc.y*res.y*punch)+odds));
	    
	//    n = mix(vec4(0.0), n, floor(noise(1000.0+time*0.05 + p*punch)+odds));
	//    e = mix(vec4(0.0), e, floor(noise(123.0+time*0.05 + p*punch)+odds));
	//    s = mix(vec4(0.0), s, floor(noise(-78.0+time*0.05 + p*punch)+odds));
	//    w = mix(vec4(0.0), w, floor(noise(42.0+time*0.05 + p*punch)+odds));
	
	    ne = mix(vec4(0.0), ne, floor(noise(1000.0+time*0.05 + tc.x*res.x*punch+tc.y)+odds));
	    nw = mix(vec4(0.0), nw, floor(noise(123.0+time*0.05 + tc.y*res.y*punch+tc.x)+odds));
	    se = mix(vec4(0.0), se, floor(noise(-78.0+time*0.05 + tc.x*res.x*punch+tc.y)+odds));
	    sw = mix(vec4(0.0), sw, floor(noise(42.0+time*0.05 + tc.y*res.y*punch+tc.x)+odds));
	
	    
	    tex += (n + e + s + w);
	    tex += (ne + nw + se + sw);
	
	    if(FRAMEINDEX < 1 || FRAMEINDEX % 1500 == 0){
	        if(tc.x >0.49 && tc.x < 0.5 && tc.y > 0.49 && tc.y <0.5){
	            tex = vec4(1.0);
	        } else {
	            tex = vec4(0.0,0.0,0.0,1.0);
	        }
	    }
	    
	    gl_FragColor = tex;
	    gl_FragColor.a =1.0;
	  
	}
	else if (PASSINDEX == 1)	{


	    vec2 tc = gl_FragCoord.xy / RENDERSIZE.xy;
	    
	    vec4 tex = IMG_NORM_PIXEL(BufferA,mod(tc,1.0));
	    vec4 past = IMG_NORM_PIXEL(BufferB,mod(tc,1.0));
	    
	    if(FRAMEINDEX < 1 || FRAMEINDEX % 1500 == 0){
	         if(tc.x >0.49 && tc.x < 0.5 && tc.y > 0.49 && tc.y <0.5){
	            gl_FragColor = vec4(1.0);
	        } else {
	            gl_FragColor = vec4(0.0,0.0,0.0,1.0);
	        }
	    } else {
	        gl_FragColor = abs(tex - past);
	    }
	    
	    
	    
	}
	else if (PASSINDEX == 2)	{
	    
	    vec2 tc = gl_FragCoord.xy / RENDERSIZE.xy;
		vec2 res = RENDERSIZE.xy;
	    float time = TIME*20.0;
	    
	    vec4 diff = IMG_NORM_PIXEL(BufferB,mod(tc,1.0));
	    vec4 draw = IMG_NORM_PIXEL(BufferC,mod(tc,1.0));
	
	    if(diff.r >= 0.75){
	
	        vec2 offs = 1.0 / res;
	
	        vec4 n = IMG_NORM_PIXEL(BufferC,mod(tc + vec2(0.0,-offs.y),1.0))*amp;
	        vec4 e = IMG_NORM_PIXEL(BufferC,mod(tc + vec2(offs.x, 0.0),1.0))*amp;
	        vec4 s = IMG_NORM_PIXEL(BufferC,mod(tc + vec2(0.0, offs.y),1.0))*amp;
	        vec4 w = IMG_NORM_PIXEL(BufferC,mod(tc + vec2(-offs.x, 0.0),1.0))*amp;
	        
	
	        vec2 pos = tc * res;
	        float p = pos.y * res.x + pos.x;
	        p /= (res.x*res.y);
	
	        draw.r = sin(n.r + w.r + e.r + s.r + (noise(402.0 + tc *p  + time*timeMod*1.01)));
	        draw.g = sin(e.r + w.r + s.r + n.r + (noise(10.4 + tc *p  + time*timeMod*2.1)));
	        draw.b = sin(s.r + w.r + n.r + e.r + (noise(32.0 + tc *p  + time*timeMod)));
	    }
	    
	    //float mask = smoothstep(0.15,0.151,dot(draw.rgb, vec3(0.3333)));//, 1.0);//vec4(smoothstep(vec2(0.1),vec2(0.9),draw.rg), 0.5, 1.0) ;//*0.1;//vec4(abs(angle), 0.0, 1.0);
	    if(FRAMEINDEX < 1 || FRAMEINDEX % 1500 == 0){
	             if(tc.x >0.49 && tc.x < 0.5 && tc.y > 0.49 && tc.y <0.5){
	                gl_FragColor = vec4(1.0);
	            } else {
	                gl_FragColor = vec4(0.0,0.0,0.0,1.0);
	            }
	        } else {
	        gl_FragColor = draw;
	        gl_FragColor.a = 1.0;
	    }
	}
	else if (PASSINDEX == 3)	{
	    gl_FragColor = IMG_NORM_PIXEL(BufferC,mod(gl_FragCoord.xy / RENDERSIZE.xy,1.0));
	}

}

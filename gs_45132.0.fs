/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0,
		"MAX": 10.0
		
	},
	{
		"NAME": "TIME_OFFSET",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0,
		"MAX": 50.0
		
	},
	{
		"NAME": "iter",
		"TYPE": "float",
		"DEFAULT": 5.0,
		"MIN": 0,
		"MAX": 20.0
		
	},
	{
		"NAME": "mod1",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0,
		"MAX": 0.9
		
	}
	,
	{
		"NAME": "circScale",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0,
		"MAX": 1.2
		
	},
	{
		"NAME": "circPos",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0.0,
		"MAX": 1.0
		
	}
	,
	{
		"NAME": "showSelection",
		"TYPE": "bool",
		"DEFAULT": false
		
	}
	,
	{
		"NAME": "selectionNumb",
		"TYPE": "float",
		"DEFAULT": 3.0,
		"MIN": 0.0,
		"MAX": 10.0
		
	}
	,
	{
		"NAME": "selectionIsCirc",
		"TYPE": "bool",
		"DEFAULT": false
		
	},
	{
		"NAME": "circPart",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": 1.0,
		"MAX": 4.0
		
	}

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#45132.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif
#extension GL_OES_standard_derivatives : enable


float hash( float n )
{
    return fract(sin(n)*4358.5453);
}
 
float fade(float t)
{
     //return t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
     return t * t * (3.0 - 2.0 * t);
}
		    
// 1d noise
float noise(float x)
{
   float p = floor(x);
   float f = fract(x);
   f = fade(f);
   float r = mix(hash(p), hash(p+1.0), f);
   r = r*r;
   return r;
}

// 2d noise
float noise( in vec2 x )
{
    vec2 p = floor(x);
    vec2 f = fract(x);
    f = f*f*(3.0-2.0*f);

    float n = p.x + p.y*57.0;
    float res = mix(mix( hash(n+  0.0), hash(n+  1.0),f.x),
                    mix( hash(n+ 57.0), hash(n+ 58.0),f.x), f.y);
    res = res*res;
    return res;
}

float r(float x){
	return fract(sin((x+6.351)*2315.12)*1264.61);
}
float r(vec2 p){
	return fract(sin(dot(p+vec2(53.52,-652.53),vec2(1315.12,5311.63)))*1264.61);
}
vec3 c(vec2 u,float n){
	float f=length(u-circPos)<circScale?1.:0.;
	
	//f += 1.- smoothstep(0.01, 0.1, abs(position.y + func(position.x/i*2.)));
	vec3 color;
	if(showSelection){
		if(int(n)==int(selectionNumb)){
			if(selectionIsCirc){
				color = vec3(f, 0.0, 0.0);
			}else{	
				color = vec3(1.0, 0.0, 0.0);
			}
		}else{
			color =  vec3( f  );
		}
	}else{
		color =  vec3( f  );
	}
	return color;
}

void main(void){
	vec2 uv=gl_FragCoord.xy/RENDERSIZE;
	vec2 a=fract(uv*float(int(circPart)));
	float b=0.;
	
	for(float i=1.;i<iter;i++){
		if(fract(r(floor(uv*pow(2.,i))+r(vec2(i*1.6,i+73.)))+r(i+floor((TIME*speed)+TIME_OFFSET)))>mod1){
			a=fract(a*2.0);
			b++;
		}else{
			break;
		}
	}
	
	gl_FragColor=vec4(c(a,b),1.);
}
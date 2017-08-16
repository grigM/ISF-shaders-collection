/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40690.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define iGlobalTime TIME
#define iResolution RENDERSIZE

//https://www.shadertoy.com/view/lsffD4
//a sphere intersection function from iq
//https://www.shadertoy.com/view/4djSDy
vec2 sphIntersect( in vec3 ro, in vec3 rd)
{
	
	float b = dot( ro, rd );
	float c = dot( ro, ro )-81.0;
	float h = b*b - c;
	if( h<0.0 ) return vec2(-1.0,-2.0);
	return vec2(-b - sqrt( h ),-b+sqrt(h));
}

vec4 insidebox(vec3 pos, vec3 dir) {
    vec3 lens = (1.0-pos*sign(dir))/abs(dir)*step(abs(pos),vec3(1.0));
    float len = min(min(lens.x,lens.y),lens.z);
    return vec4(len,lens);
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	vec2 uv = (fragCoord.xy * 2.0 - iResolution.xy) / iResolution.y;
    
    vec3 pointlight = vec3(4.0,0.0,-10.0);
    
    vec3 pos = 1.-1./(vec3(cos(iGlobalTime)*3.0,sin(iGlobalTime)*3.0,-20.0));
    vec3 dir = normalize(vec3(uv,1.5));
    
    vec2 len = sphIntersect(pos,dir);
   	pos += mix(len.x,len.y,sin(iGlobalTime)*0.5+0.5)*dir;
    vec3 normal = normalize(pos);
    vec4 len2 = insidebox(mod(pos,2.4)-1.2, dir);
    if (len2.x > 0.0) {
        normal = -step(len2.yzw,len2.xxx)*sign(dir);
    }
    if(len.y > len.x) {
        fragColor = vec4(abs(normal),1.0);
        
        vec3 lightnorm = (pointlight-pos);
        float lightdist = length(lightnorm);
        lightnorm /= lightdist;
        
        fragColor = vec4(max(dot(normal,lightnorm),0.1));
        
    } else {
        
		fragColor = vec4(0.0,0.0,uv.y*0.5+0.5,1.0);
        return;
    }
    
	//fragColor = vec4(fract(pos+0.001),1.0);
}

void main() {
	mainImage( gl_FragColor, gl_FragCoord.xy );
}
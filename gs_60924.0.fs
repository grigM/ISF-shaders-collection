/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60924.0",
  "INPUTS" : [

  ],
  "PERSISTENT_BUFFERS" : [
    "buf"
  ],
  "PASSES" : [
    {
      "TARGET" : "buf",
      "PERSISTENT" : true
    }
  ]
}
*/


#ifdef GL_FRAGMENT_PRECISION_HIGH
precision highp float;
#else
//precision mediump float;
#endif


#define PI 3.14159265

//#extension GL_OES_standard_derivatives : enable


vec3 bary(vec3 a, vec3 b, vec3 c, vec2 p) {
    vec2 v0 = b.xy - a.xy, v1 = c.xy - a.xy, v2 = p - a.xy;
    float inv_denom = 1.0 / (v0.x * v1.y - v1.x * v0.y);
    float v = (v2.x * v1.y - v1.x * v2.y) * inv_denom;
    float w = (v0.x * v2.y - v2.x * v0.y) * inv_denom;
    float u = 1.0 - v - w;
    vec3 bc = abs(vec3(u,v,w));
    if (bc.x + bc.y + bc.z > 1.00009) {
        return vec3(0.0);
    } else {
    	return bc;
    }
}

float drawLine (vec3 p1, vec3 p2, vec2 uv, float a) {
    float one_px = 1.0 / RENDERSIZE.x;
    float d = distance(p1.xy, p2.xy);
    float d_uv = distance(p1.xy, uv);
    float r = 1.0-floor(1.0-(a*one_px)+ distance(mix(p1.xy, p1.xy, clamp(d_uv/d, 0.0, 1.0)), uv));
    return r;
}

vec4 texture2D_bicubic(sampler2D tex, vec2 uv)
{
	vec2 ps = 1./RENDERSIZE;
	vec2 uva = uv+ps*.5;
	vec2 f = fract(uva*RENDERSIZE);
	vec2 texel = uv-f*ps;
#define bcfilt(a) (a<2.?a<1.?((3.*a-6.)*a*a+4.)/6.:(((6.-a)*a-12.)*a+8.)/6.:0.) 
	vec4 fxs = vec4(bcfilt(abs(1.+f.x)), bcfilt(abs(f.x)),
			bcfilt(abs(1.-f.x)), bcfilt(abs(2.-f.x)));
	vec4 fys = vec4(bcfilt(abs(1.+f.y)), bcfilt(abs(f.y)),
			bcfilt(abs(1.-f.y)), bcfilt(abs(2.-f.y)));
#undef bcfilt
	vec4 result = vec4(0);
	for (int r = -1; r <= 2; ++r)
	{
		vec4 tmp = vec4(0);
		for (int t = -1; t <= 2; ++t)
			tmp += IMG_NORM_PIXEL(tex,mod(texel+vec2(t,r)*ps,1.0)) * fxs[t+1];
		result += tmp * fys[r+1];
	}
	return result;
}

void main(void) {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy - 0.5;
    float obj_x = TIME - RENDERSIZE.x;
    float str = 1.0;
    
    vec3 p1 = vec3(sin(obj_x)       *0.2, cos(obj_x+PI)    *0.1-0.2, sin(obj_x));
    vec3 p3 = vec3(sin(obj_x+PI)    *0.2, cos(obj_x)       *0.1-0.2, sin(obj_x+PI));
    vec3 p2 = vec3(sin(obj_x+PI/2.0)*0.2, cos(obj_x-0.5*PI)*0.1-0.2, sin(obj_x+PI/2.0)*0.2);
    vec3 p4 = vec3(sin(obj_x-PI/2.0)*0.2, cos(obj_x+0.5*PI)*0.1-0.2, sin(obj_x-PI/2.0)*0.2);
    vec3 p5 = vec3(0.0 , 0.25, 0.0);

    float lines = drawLine(p1, p2, uv, str)
        	+ drawLine(p2, p3, uv, str)
        	+ drawLine(p3, p4, uv, str)
        	+ drawLine(p4, p1, uv, str)
        	+ drawLine(p5, p1, uv, str)
        	+ drawLine(p5, p2, uv, str)
        	+ drawLine(p5, p3, uv, str)
        	+ drawLine(p5, p4, uv, str);
    
    vec3 bc1 = bary(p1, p2, p5, uv);
    vec3 bc2 = bary(p2, p3, p5, uv);
    vec3 bc3 = bary(p3, p4, p5, uv);
    vec3 bc4 = bary(p4, p1, p5, uv);
    vec3 bc5 = bary(p1, p2, p3, uv);
    vec3 bc6 = bary(p1, p4, p3, uv);
    
	gl_FragColor = vec4(bc1 + bc2 + bc3 + bc4 + bc5 + bc6 + lines, 1.0) + texture2D_bicubic(buf,(uv+0.505)*0.99)*length(uv*PI);
}
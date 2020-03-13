/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60144.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec3 random3(vec3 c) {
	float j = 4096.0*sin(dot(c,vec3(17.0, 59.4, 15.0)));
	vec3 r;
	r.z = fract(512.0*j);
	j *= .125;
	r.x = fract(512.0*j);
	j *= .125;
	r.y = fract(512.0*j);
	return r-0.5;
}

const float F3 = 0.3333333;
const float G3 = 0.1666667;

float simplex3d(vec3 p) {
	 vec3 s = floor(p + dot(p, vec3(F3)));
	 vec3 x = p - s + dot(s, vec3(G3));
	 
	 vec3 e = step(vec3(0.0), x - x.yzx);
	 vec3 i1 = e*(1.0 - e.zxy);
	 vec3 i2 = 1.0 - e.zxy*(1.0 - e);
	 	
	 vec3 x1 = x - i1 + G3;
	 vec3 x2 = x - i2 + 2.0*G3;
	 vec3 x3 = x - 1.0 + 3.0*G3;
	 
	 vec4 w, d;
	 
	 w.x = dot(x, x);
	 w.y = dot(x1, x1);
	 w.z = dot(x2, x2);
	 w.w = dot(x3, x3);
	 
	 w = max(0.6 - w, 0.0);
	 
	 d.x = dot(random3(s), x);
	 d.y = dot(random3(s + i1), x1);
	 d.z = dot(random3(s + i2), x2);
	 d.w = dot(random3(s + 1.0), x3);
	 
	 w *= w;
	 w *= w;
	 d *= w;
	 
	 return dot(d, vec4(52.0));
}

float noise(vec3 m) {
    return 0.5333333*simplex3d(m)
	+0.2666667*simplex3d(2.0*m)
	+0.1333333*simplex3d(4.0*m)
	+0.0666667*simplex3d(8.0*m);
}

void main( void ) {
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;    
	uv = uv * 2. -1.;  
	
	vec2 p = gl_FragCoord.xy/RENDERSIZE.x;
	vec3 p3 = vec3(p, TIME*0.4);    
	
	float intensity = noise(vec3(p3*12.0+12.0));
			  
	float t = clamp((uv.x * -uv.x * 0.16) + 0.15, 0., 1.);                         
	float y = abs(intensity * -t + uv.y);
	
	float g = pow(y, 0.2);
			  
	vec3 col = vec3(1.0, 1.48, 1.78);
	col = col * -g + col;                    
	col = col * col;
	col = col * col;
			  
	gl_FragColor.rgb = col;                          
	gl_FragColor.w = 1.;  

}
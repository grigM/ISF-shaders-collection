/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54141.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float sdBox( vec3 p, vec3 b )
{
  vec3 d = abs(p) - b;
  return length(max(d,0.0))
         + min(max(d.x,max(d.y,d.z)),0.0); // remove this line for an only partially signed sdf 
}


vec3 roty(in vec3 p, float ang) { return vec3(p.x*cos(ang)-p.z*sin(ang),p.y,p.x*sin(ang)+p.z*cos(ang)); }
float scene(in vec3 p)
{

	p = roty(p,TIME);
	p.yxz = roty(p.yxz,TIME);
	return sdBox(p, vec3(0.5,0.1,0.3)) ;
}

vec3 get_normal(in vec3 p)
{

	vec3 eps = vec3(0.001,0,0); 
	float nx = scene(p+eps.xyy)-scene(p-eps.xyy);
	float ny = scene(p+eps.yxy)-scene(p-eps.yxy);
	float nz = scene(p+eps.yyx)-scene(p-eps.yyx);
	return normalize(vec3(nx,ny,nz)); 
}
vec3 m2( in vec2 p, vec3 offs ) {
	
	vec3 ro = vec3(0,0,1)+offs; 
	vec3 rd = normalize(vec3(p.x,p.y,-1.0));
	
	vec3 col = vec3(0);
	
	vec3 pos = ro; 
	float dist = 0.0, d; 
	for (int i = 0 ; i < 64; i++) {
		d = scene(pos); 
		pos += d*rd; 		
	}
	dist = length(pos-ro);
	if (dist < 100.0) {
		col = vec3(1); 	
		vec3 n = get_normal(pos);
		vec3 l = normalize(vec3(1,1,1)); 
		float d = clamp(dot(n,l), 0.0, 1.0); 
		col = vec3(0.3)+vec3(1)*d;
	}
	else {
		p.x = mod(p.x, 0.1);
		p.y = mod(p.y, 0.1);
		if (abs(p.x) < 0.01) col = vec3(1); 
		if (abs(p.y) < 0.01) col = vec3(1); 
	
	}
	return col; 
}

void main(void)
{

	vec2 p = 2.0*( gl_FragCoord.xy / RENDERSIZE.xy ) -1.0;
	p.x *= RENDERSIZE.x/RENDERSIZE.y; 

	vec3 col = vec3(0);
	col += m2(p,vec3(-1,0,0)*mouse.x)*vec3(1,0,0); 
	col += m2(p,vec3(+1,0,0)*mouse.x)*vec3(0,0,1); 
	   
	gl_FragColor = vec4(col, 1.0); 
}
/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "GLSLSandbox"
    ],
    "DESCRIPTION": "Automatically converted from http://glslsandbox.com/e#58556.0",
    "INPUTS": [
    ]
}

*/





#ifdef GL_ES
precision mediump float;
#endif
 
 
float bayer( vec2 rc )
{
	float sum = 0.0;
	for( int i=0; i<2; ++i )
	{
		vec2 bsize;
		if ( i == 0 ) { bsize = vec2(2.0); } else if ( i==1 ) { bsize = vec2(4.0); } else if ( i==2 ) { bsize = vec2(8.0); };
		vec2 t = mod(rc, bsize) / bsize;
		int idx = int(dot(floor(t*2.0), vec2(1.0,1.0)));
		float b = 0.0;
		if ( idx == 0 ) { b = 0.0; } else if ( idx==1 ) { b = 2.0; } else if ( idx==2 ) { b = 3.0; } else { b = 1.0; }
		if ( i == 0 ) { sum += b * 16.; } else if ( i==1 ) { sum += b * 4.; } else if ( i==2 ) { sum += b * 1.; };
	}
	return sum / 64.0;
}
 
vec3 rotatex(in vec3 p, float ang) {    
    return vec3(p.x, p.y*cos(ang) - p.z*sin(ang), p.y*sin(ang) + p.z*cos(ang)); 
}
vec3 rotatey(in vec3 p, float ang) {    
    return vec3(p.x*cos(ang) - p.z*sin(ang), p.y, p.x*sin(ang) + p.z*cos(ang)); 
}
vec3 rotatez(in vec3 p, float ang) {    
    return vec3(p.x*cos(ang) - p.y*sin(ang), p.x*sin(ang) + p.y*cos(ang), p.z); 
}
float scene(vec3 p)
{
    p = rotatey(p, 1.1*TIME);
    p = rotatex(p, 1.3*TIME);
    p = rotatez(p, 1.2*TIME);
    float d0 = length(max(abs(p) - 0.5, 0.0))- 0.01  ; 
    float d1 = length(p) - 0.5; 
    return mix(d0,d0, 0.0);
//  return sin(min(d0,d1)*1.6); 
}
 
vec3 get_normal(vec3 p)
{
    vec3 eps = vec3(0.01, 0.0, 0.0); 
    float nx = scene(p + eps.xyy) - scene(p - eps.xyy); 
    float ny = scene(p + eps.yxy) - scene(p - eps.yxy); 
    float nz = scene(p + eps.yyx) - scene(p - eps.yyx); 
    return normalize(vec3(nx,ny,nz)); 
}
void main( void ) 
{
    vec3 color = vec3(0); 
    vec2 p = 2.0*gl_FragCoord.xy/RENDERSIZE - 1.0; 
    p.x *= RENDERSIZE.x/RENDERSIZE.y; 
 
    if (abs(p.y) < 0.7) {
    vec3 ro = vec3(0.0, 0.0, 1.8); 
    vec3 rd = normalize(vec3(p.x, p.y, -1.4)); 
    
    color = (1.0 - vec3(length(p*0.1)))*0.2;   
 
    vec3 pos = ro; 
    float dist = 0.0; 
    for (int i = 0; i < 36; i++) {
        float d = scene(pos); 
        pos += rd*d; 
        dist += d; 
    }
    
    if (dist < 10.0) {
        vec3 n = get_normal(pos);
        float diff  = 1.0*clamp(dot(n, normalize(vec3(1,1,1))), 0.0, 1.0); 
        color = diff*vec3(0,1,1);
    }
    }
	float threshold = bayer(gl_FragCoord.xy);
	float pr = step(threshold+0.1, float(color.r));
	float pg = step(threshold+0.1, float(color.g));
	float pb = step(threshold+0.1, float(color.b));
    
    gl_FragColor = vec4(pr,pg,pb, 1.0);  
}
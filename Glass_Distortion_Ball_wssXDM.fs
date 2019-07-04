/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "e6e5631ce1237ae4c05b3563eda686400a401df4548d0f9fad40ecac1659c46c.jpg"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wssXDM by kubiak.  Magnifying and distorting to give a weird glass effect.",
  "INPUTS" : [
  
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
  ]
}
*/


vec3 calcNormal(vec2 center, vec2 pos, float radius) {
    vec2 rpos = pos - center;
    rpos = rpos / radius;
    float z = sqrt(1.0 - rpos.x*rpos.x + rpos.y*rpos.y);
    
    return -vec3(rpos.x, rpos.y, z);
}


vec2 hash( vec2 p ) {
	p = vec2(dot(p,vec2(127.1,311.7)), dot(p,vec2(269.5,183.3)));
	return -1.0 + 2.0*fract(sin(p)*43758.5453123);
}


float noise( in vec2 p ) {
    const float K1 = 0.366025404; // (sqrt(3)-1)/2;
    const float K2 = 0.211324865; // (3-sqrt(3))/6;
	vec2 i = floor(p + (p.x+p.y)*K1);	
    vec2 a = p - i + (i.x+i.y)*K2;
    vec2 o = (a.x>a.y) ? vec2(1.0,0.0) : vec2(0.0,1.0); //vec2 of = 0.5 + 0.5*vec2(sign(a.x-a.y), sign(a.y-a.x));
    vec2 b = a - o + K2;
	vec2 c = a - 1.0 + 2.0*K2;
    vec3 h = max(0.5-vec3(dot(a,a), dot(b,b), dot(c,c) ), 0.0 );
	vec3 n = h*h*h*h*vec3( dot(a,hash(i+0.0)), dot(b,hash(i+o)), dot(c,hash(i+1.0)));
    return dot(n, vec3(70.0));	
}

vec4 quatForAxisAngle(float angle, vec3 axis) {
    vec4 q;
    
    float half_angle = angle/2.0;
    q.x = axis.x * sin(half_angle);
    q.y = axis.y * sin(half_angle);
    q.z = axis.z * sin(half_angle);
    q.w = cos(half_angle);
    return q;
}

bool intersectsBall(vec2 p2, vec2 center, float radius, out float edginess) {
    float n = noise(p2 * 5.0 + TIME) * 0.015;
    radius += n;
    vec2 to = p2 - center;
    float l = length(to);
    edginess = 0.0;
    
    float edgeWidth = 0.05;
    if(l < radius && l > (radius - edgeWidth)) {
        edginess = 1.0 - (radius - l) / edgeWidth;
    }
    return l < radius;
}


void main() {



	vec2 p = gl_FragCoord.xy / RENDERSIZE.xy;
	p = p;
    
    vec2 p2 = p * 1.0;
    float aspect = RENDERSIZE.x / RENDERSIZE.y;
	p2.x *= aspect;
    p2.y = 1.0 - p2.y;
    
    float s = sin(TIME)*0.5 + 0.5 * aspect;
    //s = 1.0;
    
    vec2 center = vec2(s, 0.5);
    float radius = 0.25;
    
    vec4 col;
    
    vec3 normal = calcNormal(center, p2, radius);
    
    // Rotation
    vec4 q = quatForAxisAngle(0.0, vec3(0.0, 1.0, 0.0));
    vec3 temp = cross(q.xyz, normal) + q.w * normal;
    normal = normal + 2.0*cross(q.xyz, temp);
    
    float edginess = 0.0;
    if(!intersectsBall(p2, center, radius, edginess)) {
     
        col = vec4(0.0);
        col = IMG_NORM_PIXEL(inputImage,mod(p,1.0));
    }
    else 
    {
        vec3 r = reflect(vec3(0.0, 0.0, 1.0), normalize(normal));
        //r = normal;
        vec3 t = (r * 0.5 + 0.5) * 0.5;
        
        t += noise(t.xy * 10.0 + TIME) * 0.05;
        t.x -= s;
        col = IMG_NORM_PIXEL(inputImage,mod(t.xy,1.0));
        
        // Brighten
        col = col * (1.5) + vec4(edginess * edginess);
        //col = vec4(vec3(t.x), 1.0);
		//col = vec4(normal, 1.0);
    }
    // Output to screen
    gl_FragColor = col;
}

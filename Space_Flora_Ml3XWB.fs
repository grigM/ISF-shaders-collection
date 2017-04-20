/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "fractal",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Ml3XWB by zackpudil.  Space Maintenance 2.  Sometimes the sequel is better than the original.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


float time = TIME;

float hash(float n) {
	return fract(sin(n)*43578.5453);
}

float noise(float n) {
	float p = floor(n);
	float f = fract(n);
	f = f*f*(3.0 - 2.0*f);
	
	return mix(hash(p), hash(p + 1.0), f);
}

float formula(vec2 p) {
  float at = 1.3 + time*0.05;
  p = 0.07*p - vec2(0.86*sin(at), 0.7*cos(at));
  float d = 100.0;
  
  for(int i = 0; i < 10; i++) {
    p = abs(p)/dot(p, p) - vec2(0.5);
    d = min(d, abs(2.0*p.x)); 
  }
  
  return d;
}

vec3 bump(vec2 p, float e) {
  vec2 h = vec2(e, 0.0);
  vec3 g = vec3(
    formula(p + h.xy) - formula(p - h.xy),
    formula(p + h.yx) - formula(p - h.yx),
    -0.3)/e;
  
  return g;
}

float lde(vec2 p) {
    if(iMouse.x <= 0.0) {
        float c = floor(time);
        c = 3.14159*noise(time);

        p *= mat2(cos(c), sin(c), -sin(c), cos(c));
        p.y += 2.0*cos(time);
    } else {
        p -= (-RENDERSIZE.xy + 2.0*iMouse.xy)/RENDERSIZE.y;
    }
	return length(p) - 0.005;
}

vec3 light(vec2 p, float d) {
	vec2 h = vec2(max(d, 0.3), 0.0);
	vec3 l = normalize(vec3(
		lde(p + h) - lde(p - h),
		lde(p + h.yx) - lde(p - h.yx),
		0.3));
	
	return -l;
}

void main(){
	vec2 p = (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy)/RENDERSIZE.y;
	vec3 col = vec3(0);
    
	vec3 rd = normalize(vec3(p, 1.97));
	vec3 sn = normalize(bump(p, 0.01));  
	vec3 re = reflect(rd, sn);
    
	float dis = lde(p);
	vec3 lig = light(p, dis);
	float att = 1.0/(1.0 + 0.0*dis + 50.0*dis*dis);
	col += att*vec3(0.2, 1.0, 0.3)*clamp(dot(lig, sn), 0.0, 1.0);
    
	col += 1.0*pow(clamp(dot(-rd, re), 0.0, 1.0), 8.0);
	col *= 3.0*formula(p);
    
	col = mix(vec3(0.2, 1.0, 0.3), col, smoothstep(0.0, 0.04, lde(p)));
    
	col = pow(col, vec3(1.0/2.2));
	gl_FragColor = vec4(col, 1);

}
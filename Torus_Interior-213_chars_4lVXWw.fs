/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "raymarch",
    "torus",
    "short",
    "2tc",
    "codegolf",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4lVXWw by GregRostami.  I've been trying to reduce one of my favorite shaders from fb39ca4 down to two-tweets (2TC):\n[url]https:\/\/www.shadertoy.com\/view\/MsX3Wj[\/url]\nFellow code golfers ... Please HELP reduce this further.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


// 213 chars - Another 3 chars lie dead behind Fabrice's path ...
/**/
void main()
{
    vec3 R = RENDERSIZE, 
         p = R-R; p.z = 4.;
    
    for (int i = 0; i < 64; i++)
        p +=  vec3((u+u-R.xy)/R.x,.5) 
            * (length(vec2(o.a=length(p.xz)-4.,p.y))-3.);
    
    o += 9.*sin(16.*(atan(p.y, o.a) - atan(p.z,p.x) - iDate.w)) -o;                 
}
/**/

// 222 chars - Greg added fade to black for depth.
/**
void main()
{
    vec3 R = RENDERSIZE, 
         p = R-R; p.z = 4.;
    
    for (int i = 0; i < 64; i++)
        p +=  vec3((u+u-R.xy)/R.x,.5) 
            * (length(vec2(o.a=length(p.xz)-4.,p.y))-3.);
    
    o += .2*(p.z+6.)*sin(16.*(atan(p.y, o.a) - atan(p.z,p.x) - iDate.w)) -o;                 
}
/**/

// 216 chars - Code Golf Master F (Fabrice) schooled me AGAIN!!
/**
void main()
{
    vec3 p = RENDERSIZE;
    u = (u+u-p.xy)/p.x;
    p -=p; p.z = 4.;
    for (int i = 0; i < 64; i++)
        p += vec3(u,.5) * (length(vec2(o.a=length(p.xz)-4.,p.y))-3.);
    
    o += 9.*sin(16.*(atan(p.y, o.a) + atan(p.z,p.x) + iDate.w)) -o;                
}
/**/

// 280 chars - reduced by Greg Rostami
/**
void main()
{
    vec3 R = RENDERSIZE, p = R-R;
    u = u/R.xy*2.-1.;
    p.z = 4.;
    for (int i = 0; i < 64; i++)
        p += vec3(u.x*.8, u.y*R.y/R.x, .5) * (length(vec2(length(p.xz)-4.,p.y))-3.);
    
    o = vec4( smoothstep(
        o.a = fract(8.*(atan(p.y, length(p.xz) - 4.) + atan(p.z,p.x) + iDate.w)/3.142),
                        o.a, o.a < .6 ? .1 : 1.));
}
/**/

// 1151 chars - Original shader by fb39ca4
/**
//Thank you iquilez for some of the primitive distance functions!


const float PI = 3.14159265358979323846264;


const int MAX_PRIMARY_RAY_STEPS = 64; //decrease this number if it runs slow on your computer

vec2 rotate2d(vec2 v, float a) { 
	return vec2(v.x * cos(a) - v.y * sin(a), v.y * cos(a) + v.x * sin(a)); 
}

float sdTorus( vec3 p, vec2 t ) {
  vec2 q = vec2(length(p.xz)-t.x,p.y);
  return length(q)-t.y;
}

float distanceField(vec3 p) {
	return -sdTorus(p, vec2(4.0, 3.0));
}

vec3 castRay(vec3 pos, vec3 dir, float treshold) {
	for (int i = 0; i < MAX_PRIMARY_RAY_STEPS; i++) {
			float dist = distanceField(pos);
			//if (abs(dist) < treshold) break;
			pos += dist * dir;
	}
	return pos;
}

void main()
{
	vec4 mousePos = (iMouse / RENDERSIZE.xyxy) * 2.0 - 1.0;
	vec2 screenPos = (gl_FragCoord.xy / RENDERSIZE.xy) * 2.0 - 1.0;
	vec3 cameraPos = vec3(0.0, 0.0, -3.8);
	
	vec3 cameraDir = vec3(0.0, 0.0, 0.5);
	vec3 planeU = vec3(1.0, 0.0, 0.0) * 0.8;
	vec3 planeV = vec3(0.0, RENDERSIZE.y / RENDERSIZE.x * 1.0, 0.0);
	vec3 rayDir = normalize(cameraDir + screenPos.x * planeU + screenPos.y * planeV);
	
	//cameraPos.yz = rotate2d(cameraPos.yz, mousePos.y);
	//rayDir.yz = rotate2d(rayDir.yz, mousePos.y);
	
	//cameraPos.xz = rotate2d(cameraPos.xz, mousePos.x);
	//rayDir.xz = rotate2d(rayDir.xz, mousePos.x);
	
	vec3 rayPos = castRay(cameraPos, rayDir, 0.01);
	
	float majorAngle = atan(rayPos.z, rayPos.x);
	float minorAngle = atan(rayPos.y, length(rayPos.xz) - 4.0);
		
	float edge = mod(8.0 * (minorAngle + majorAngle + TIME) / PI, 1.0);
	float color = edge < 0.7 ? smoothstep(edge, edge+0.03, 0.5) : 1.0-smoothstep(edge, edge+0.03, 0.96);
	//float color = step(mod(8.0 * (minorAngle + majorAngle + TIME) / PI, 1.0), 0.5);
	//color -= 0.20 * step(mod(1.0 * (minorAngle + 1.0 * majorAngle + PI / 2.0) / PI, 1.0), 0.2);
	
	gl_FragColor = vec4(color);
}
/**/
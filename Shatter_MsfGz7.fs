
/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "3083c722c0c738cad0f468383167a0d246f91af2bfa373e9c5c094fb8c8413e0.png"
    },
    {
      "NAME" : "iChannel1",
      "PATH" : [
        "grid_512.png",
        "grid_512.png",
        "grid_512.png",
        "grid_512.png",
        "grid_512.png",
        "grid_512.png"
      ],
      "TYPE" : "cube"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MsfGz7 by simesgreen.  more messing about with convex objects. this time, voronoi decomposition of a cube!",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    },
    {
      "NAME": "auto_rotation",
      "TYPE": "bool",
      "DEFAULT": "TRUE"
    },
    
    
    {
      "NAME": "auto_destruct",
      "TYPE": "bool",
      "DEFAULT": "TRUE"
    },

    {
      "VALUES" : [
        0,
        1,
        2,
        3,
        4,
        5,
        6
      ],
      "NAME" : "shading",
      "TYPE" : "long",
      "DEFAULT" : 4,
      "LABELS" : [
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6"
      ]
    },
    {
      "VALUES" : [
        0,
        1,
        2
      ],
      "NAME" : "back",
      "TYPE" : "long",
      "DEFAULT" : 1,
      "LABELS" : [
        "0",
        "1",
        "2"
      ]
    },
    {
      "NAME" : "speed",
      "TYPE" : "float",
      "MAX" : 3,
      "DEFAULT" : 1.0,
      "MIN" : 0
    }
    ,
     {
      "NAME" : "offset",
      "TYPE" : "float",
      "MAX" : 20.95,
      "DEFAULT" : 0.0,
      "MIN" : 0
    },
    
    {
      "NAME" : "numPoints",
      "TYPE" : "float",
      "MAX" : 50.0,
      "DEFAULT" : 35.0,
      "MIN" : 0
    },
    
    
    {
      "NAME" : "back_opacity",
      "TYPE" : "float",
      "MAX" : 1.0,
      "DEFAULT" : 0.5,
      "MIN" : 0
    },
    
    {
      "NAME" : "camera_distance",
      "TYPE" : "float",
      "MAX" : 50.0,
      "DEFAULT" : 5.0,
      "MIN" : 0
    },
    
    

  ],
  "ISFVSN" : "2"
}
*/


// Voronoi shatter
// @simesgreen 4/12/2012

const float texelSize = 1.0 / 64.0;
//const int numPoints = 35;

// transforms
vec3 rotateX(vec3 p, float a)
{
    float sa = sin(a);
    float ca = cos(a);
    return vec3(p.x, ca*p.y - sa*p.z, sa*p.y + ca*p.z);
}

vec3 rotateY(vec3 p, float a)
{
    float sa = sin(a);
    float ca = cos(a);
    return vec3(ca*p.x + sa*p.z, p.y, -sa*p.x + ca*p.z);
}

float getScale()
{
	if(auto_destruct){
		return 6.0 - cos(((TIME*speed)-offset)*0.3)*5.0;
	}else{
		return 1.0;
	}
}

// get random vertex position from texture
vec3 getVertex(int i)
{
	float j = texelSize*((TIME*speed)-offset)*0.2;	// animate
	//float j = 0.5;	
	vec3 v = IMG_NORM_PIXEL(iChannel0,mod(vec2(float(i)*texelSize, j),1.0)).xyz*2.0-1.0;
	v *= getScale();
	return v;
}

// intersect ray against half-plane
void intersectPlane(vec3 ro, vec3 rd, vec3 n, float d, inout float t1, inout float t2, inout vec3 hitn, inout bool hit)
{
	float denom = dot(n, rd);
	float dist = d - dot(n, ro);
	float t = dist / denom;
	if (denom < 0.0) {
		// entering halfspace
		if (t > t1) {
			t1 = t;
			hitn = n;
		}
	} else  {
		// leaving halfspace
		if (t < t2) {
			t2 = t;
		}
	}
	if (t1 > t2) {
		hit = false;
	}						
}

// intersect ray against convex polyhedron defined by half-planes
bool intersectConvex(vec3 ro, vec3 rd,
					 int j,
					 float s,
					 out float t1,
					 out vec3 hitn)
{
	bool hit = true;
	t1 = 0.0;
	float t2 = 1e10;
	
	vec3 p = getVertex(j);
	
	// look at all other vertices
	for(int i=0; i<int(numPoints); i++) {
		if (i!=j) {
			vec3 p2 = getVertex(i);
			// define plane halfway between this and other vertex
			vec3 n = normalize(p2 - p);
			vec3 mid = p + (p2 - p)*s;
			
			intersectPlane(ro - mid, rd, n, 0.0, t1, t2, hitn, hit);
		}
	}

	// intersect with box sides
	float d = getScale();
	//float d = 1.0;
	intersectPlane(ro, rd, vec3(-1, 0, 0), d, t1, t2, hitn, hit);
	intersectPlane(ro, rd, vec3(1, 0, 0), d, t1, t2, hitn, hit);	
	intersectPlane(ro, rd, vec3(0, -1, 0), d, t1, t2, hitn, hit);		
	intersectPlane(ro, rd, vec3(0, 1, 0), d, t1, t2, hitn, hit);			
	intersectPlane(ro, rd, vec3(0, 0, -1), d, t1, t2, hitn, hit);		
	intersectPlane(ro, rd, vec3(0, 0, 1), d, t1, t2, hitn, hit);			
	
	return hit;
}

float fresnel(vec3 n, vec3 v, float minr)
{
	return minr + (1.0-minr)*pow(1.0 - clamp(dot(n, v), 0.0, 1.0), 2.0);
}

// lighting
vec3 shade(vec3 pos, vec3 n, vec3 eyePos, vec3 c)
{
    const float shininess = 80.0;
	const vec3 l = vec3(0.577, 0.577, 0.577);
	//const vec3 c = vec3(1.0);
	
    vec3 v = normalize(eyePos - pos);
    vec3 h = normalize(v + l);
	
    float diff = dot(n, l);
    float spec = pow(max(0.0, dot(n, h)), shininess) * float(diff > 0.0);
    //diff = max(0.0, diff);
    diff = 0.5+0.5*diff;
	
	// reflection
	vec3 R = reflect(-v, n);
	vec3 Rcol = textureCube(iChannel1,R).xyz;
	//Rcol *= 1.0 + smoothstep(0.5, 1.0, R.x)*2.0;
	
	// refraction
	//const float eta = 1.0 / 1.4;	// air-glass
	const float eta = 1.0 / 1.1;	// air-glass
		
#if 0
	vec3 T = normalize(-v + n*0.1);
	//vec3 T = refract(-v, n, eta);
	vec3 Tcol = textureCube(iChannel1,T).xyz;
#else
	// dispersion
	vec3 T = refract(-v, n, eta);
	vec3 Tcol;
	Tcol.r = textureCube(iChannel1,T).r;
	T = refract(-v, n, eta + 0.01);
	Tcol.g = textureCube(iChannel1,T).g;	
	T = refract(-v, n, eta + 0.02);
	Tcol.b = textureCube(iChannel1,T).b;	
#endif
	
    float f = fresnel(n, v, 0.2);
	
	if (shading == 0){
		return c;	
	}else if (shading == 1){
		return diff*c + vec3(spec);
	}else if (shading == 2){
		return Rcol;
	}else if (shading == 3){
		return Rcol*f;
	}else if (shading == 4){
		return Tcol;
	}else if (shading == 5){
		return n*0.5+0.5;
	}else if (shading == 6){
		return pos*0.5+0.5;
	}else if (shading == 7){
		return mix(Tcol, Rcol, f);
	}else if (shading == 8){
		return diff*c + Rcol*f;
	}
	
	
}


vec3 background(vec3 rd)
{
	if(back==0){
		return mix(vec3(0.0), vec3(0.0, 0.0, 0.0), rd.y);
	}else if(back==1){
		return textureCube(iChannel1,rd).xyz;
	}else if(back==2){
		return mix(vec3(1.0), vec3(0.0, 0.25, 1.0), rd.y);
	}
}

void main() {



    vec2 pixel = (gl_FragCoord.xy / RENDERSIZE.xy)*2.0-1.0;
    // compute ray origin and direction
    float asp = RENDERSIZE.x / RENDERSIZE.y;
    vec3 rd = normalize(vec3(asp*pixel.x, pixel.y, -2.0));
    vec3 ro = vec3(0.0, 0.0, camera_distance);
	vec2 mouse = iMouse.xy / RENDERSIZE.xy;
	float roty;
	float rotx;
	if (auto_rotation) {
		rotx = -sin(((TIME*speed)-offset)*0.3)*0.5;
		roty = ((TIME*speed)-offset)*0.1;	
	} else {
		rotx = (mouse.y-0.5)*3.0;
		roty = -(mouse.x-0.5)*6.0;
	}
	
    rd = rotateX(rd, rotx);
    ro = rotateX(ro, rotx);
		
    rd = rotateY(rd, roty);
    ro = rotateY(ro, roty);
		
	//float sep = 0.4;	
	float sep = 0.5 / getScale();
	
    // trace ray
	vec3 p;
    vec3 n;
	float tmin = 1e10;
	bool hit = false;
	vec4 col;
	
	for(int i=0; i<int(numPoints); i++) {
		float t;
		vec3 cn;
		bool chit = intersectConvex(ro, rd, i, sep, t, cn);
		if (chit  && (t < tmin)) {
			tmin = t;
			n = cn;
			col = IMG_NORM_PIXEL(iChannel0,mod(vec2(0.0, float(i)*texelSize),1.0));;
		}
		hit = hit || chit;		
	}
	p = ro + tmin*rd;	
	
    vec3 rgb;
    if(hit) {
        // shade
        rgb = shade(p, n, ro, col.xyz);
	} else {
		rgb = background(rd)*back_opacity;
	}
    
	// vignetting
    rgb *= 0.5+0.5*smoothstep(2.0, 0.5, dot(pixel, pixel));
	
    gl_FragColor=vec4(rgb, 1.0);
}
/*
"94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_1.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_2.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_3.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_4.png",
        "94284d43be78f00eb6b298e6d78656a1b34e2b91b34940d02f1ca8b22310e8a0_5.png"
*/
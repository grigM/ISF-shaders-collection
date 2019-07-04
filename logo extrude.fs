/*{
  "CREDIT": "by You",
  "DESCRIPTION": "",
  "CATEGORIES": [],
  "INPUTS": [
    {
      "NAME": "heightmap",
      "TYPE": "image"
    },
    {
      "NAME": "j",
      "TYPE": "float",
      "DEFAULT": 0.15,
      "MIN": 0,
      "MAX": 1
    },
    {
      "NAME": "k",
      "TYPE": "float",
      "DEFAULT": 0.5,
      "MIN": 0,
      "MAX": 1
    },
    {
      "NAME": "c0",
      "TYPE": "float",
      "DEFAULT": 0,
      "MIN": 0,
      "MAX": 8
    },
    {
      "NAME": "c1",
      "TYPE": "float",
      "DEFAULT": 0,
      "MIN": 0,
      "MAX": 8
    },
    {
      "NAME": "c2",
      "TYPE": "float",
      "DEFAULT": 0,
      "MIN": 0,
      "MAX": 8
    }
  ]
}*/

vec3   iResolution = vec3(RENDERSIZE, 1.0);
float  iGlobalTime = TIME;
const int MAX_ITER = 30;


float height(float x,  float y) {
	vec2 tex_space = (vec2(x,y)/ 1.5 + 0.5);
	if ( tex_space.x < 0.0 || tex_space.x >1.0 || tex_space.y < 0.0 || tex_space.y > 1.0 )
		return 0.0;
		
	return texture2D(heightmap, tex_space).r *j ;
}

float sdBox( vec3 p, vec3 b ){
  vec3 d = abs(p) - b;
  return min(max(d.x,max(d.y,d.z)),0.0) +
         length(max(d,0.0));
}
mat3 RotationMatrix(vec3 axis, float angle){
    axis = normalize(axis);
    float s = sin(angle);
    float c = cos(angle);
    float oc = 1.0 - c;
    
    return mat3(oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,
                oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,
                oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c);
}

vec2 rotate(in vec2 v, in float a) {
	return vec2(cos(a)*v.x + sin(a)*v.y, -sin(a)*v.x + cos(a)*v.y);
}

vec3 pal( in float t, in vec3 a, in vec3 b, in vec3 c, in vec3 d ){
    return a + b*cos( 6.28318*(c*t+d +c1) );
}

vec3 palette(float f ) {
	return pal(f, vec3(0.8,0.5,0.4),vec3(0.2,0.4,0.2),vec3(2.0,1.0,1.0),vec3(0.0,0.25,0.25) );
	// return pal(f, vec3(0.5,0.5,0.5),vec3(0.5,0.5,0.5),vec3(1.0,1.0,1.0),vec3(0.0,0.10,0.20) );
}

float DistToBoundingBox(in vec3 p){
	p = p * RotationMatrix(vec3(0.,1.,0.0),  TIME/10. * 3.14 * 2. + 3.14);
	return sdBox(p, vec3(1.0, 1.0, 0.2));
}

vec3 hsv(in float h, in float s, in float v) {
	return mix(vec3(1.0), clamp((abs(fract(h + vec3(3, 2, 1) / 3.0) * 6.0 - 3.0) - 1.0), 0.0 , 1.0), s) * v;
}

// vec3 gradient( in vec3 pos )
// {
// 	vec3 eps = vec3( 0.001, 0.0, 0.0 );
// 	vec3 nor = vec3(
// 	    DE(pos+eps.xyy) - DE(pos-eps.xyy),
// 	    DE(pos+eps.yxy) - DE(pos-eps.yxy),
// 	    DE(pos+eps.yyx) - DE(pos-eps.yyx) );
// 	return normalize(nor);
// }

vec3 intersect(in vec3 rayOrigin, in vec3 rayDir){
	float total_dist = 0.0;
	vec3 p = rayOrigin;
	float d = 1.;
	float iter = 0.0;

	bool hit = false;
	for(int i = 0; i <MAX_ITER; i ++ ) {
		p += d * rayDir;
		d = DistToBoundingBox(p);
		if ( d < 0.01) {
			hit = true;
			break;
		}
		total_dist += d;
		iter++;
	}
	
	if(hit)
		iter = 0.0;

	if ( hit) {
		hit = false;
		for(int i = 0; i <MAX_ITER; i ++ ) {
			p += 0.02 * rayDir;
			vec3 p_w = p * RotationMatrix(vec3(0., 1.0,0.0), TIME/10.* 3.14 * 2. + 3.14);
			float h = height(p_w.x, p_w.y);
			
			if ( h > 0.1 && abs(h - p_w.z) < 0.1) {
				hit = true;
				break;
			}
			iter++;
		}
	}
	
	vec3 color = vec3(0.0);
	float x = (iter/float(MAX_ITER));

	float shade = 1.0 - x;
	
	color = vec3(shade) ;
	return color;
}

void main(){
	// vec3 cameraDir = vec3(1., 0., 0.);
	vec3 camera = vec3(0., 0., 2.);

	vec2 screenPos = -1.0 + 2.0 * gl_FragCoord.xy / iResolution.xy;
	screenPos.x *= iResolution.x / iResolution.y;
	vec3 ray = normalize(vec3(screenPos, -1.0));
	
	gl_FragColor = vec4(intersect(camera,  ray), 1.0);
} 
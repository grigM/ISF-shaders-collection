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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#52260.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable




const vec3 cPos = vec3(0.0, 5.0, 5.0);
const vec3 cDir = vec3(0.0, -0.707, -0.707);
const vec3 cUp  = vec3(0.0,  0.707, -0.707);

vec3 light = vec3(2.0,0.8,0.3);
const vec3 lightDir = vec3(-0.57, 0.57, 0.57);
vec3 lightColor = vec3(0.324,0.533,0.443);

float shadow = 0.0;

// torus distance 
float distFuncTorus(vec3 p){
    p.xz -= mouse * 2.0 - 1.0;
    vec2 t = vec2(3.0, 1.0);
    vec2 r = vec2(length(p.xz) - t.x, p.y);
    return length(r) - t.y;
}

// floor distance
float distFuncFloor(vec3 p){
    return dot(p, vec3(0.0, 1.0, 0.0)) + 1.0;
}

// distance 
float distFunc(vec3 p){
    float d1 = distFuncTorus(p);
    float d2 = distFuncFloor(p);
    return min(d1, d2);
}

vec3 genNormal(vec3 p){
    float d = 0.0001;
    return normalize(vec3(
        distFunc(p + vec3(  d, 0.0, 0.0)) - distFunc(p + vec3( -d, 0.0, 0.0)),
        distFunc(p + vec3(0.0,   d, 0.0)) - distFunc(p + vec3(0.0,  -d, 0.0)),
        distFunc(p + vec3(0.0, 0.0,   d)) - distFunc(p + vec3(0.0, 0.0,  -d))
    ));
}

float genShadow(vec3 ro,vec3 rd)
{
	float h = 0.0;
	float c = 0.001;
	float r = 1.0;
	float shadowCoef = 0.5;
	for(float t = 0.0;t < 50.0;++t)
	{
		h = distFunc(ro + rd * c);
		if(h < 0.001)
		{
			return shadowCoef;
		}
		r = min(r,h * 16.0 / c);
		c += h;
	}
	return 1.0 - shadowCoef + r * shadowCoef;
}
void main(void){
    // fragment position
    vec2 p = (gl_FragCoord.xy * 2.0 - RENDERSIZE) / max(RENDERSIZE.x, RENDERSIZE.y);
    
    //move
    lightColor += vec3(0.8 * sin(TIME * pow(2.0,0.33)), 0.7 * cos(TIME * 0.54),sin(TIME * 0.63) * 0.7);
    light += vec3(0.8 * cos(3.4 * TIME * radians(50.0)),2.0 + 0.2 * sin(0.7 * TIME * radians(-30.0)),sin(TIME * radians(34.9)) + 0.0);
   
   // camera and ray
    vec3 cSide = cross(cDir, cUp);
    float targetDepth = 1.0;
    vec3 ray = normalize(cSide * p.x + cUp * p.y + cDir * targetDepth);
    
    float tmp, dist;
    tmp = 0.0;
    vec3 dPos = cPos;
    for(int i = 0; i < 256; i++){
        dist = distFunc(dPos);
        if(dist < 0.001){break;}
        tmp += dist;
        dPos = cPos + tmp * ray;
    }
    
    // hit check
    vec3 color;
    if(abs(dist) < 0.001){
        // normal
        vec3 normal = genNormal(dPos);
        
	//light
	vec3 halfLE = normalize(light - ray);
	float diff = clamp(dot(light,normal),0.1,1.0);
	float spec = pow(clamp(dot(halfLE,normal),0.0,1.0),50.0);
	    
	// shadow
	shadow = genShadow(dPos + normal * 0.001,light);
	    
        float u = 1.0 - floor(mod(dPos.x, 2.0));
        float v = 1.0 - floor(mod(dPos.z, 2.0));
        if((u == 1.0 && v < 1.0) || (u < 1.0 && v == 1.0)){
            diff *= 0.7;
        }
        
        color = vec3(1.0, 1.0, 1.0) * diff * lightColor + vec3(spec);
    }else{
        color = vec3(0.0);
    }
    gl_FragColor = vec4(color * max(0.5,shadow), 1.0);
}
/*{
	"CREDIT": "by mojovideotech",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"generator",
		"5d",
		"tiles"
	],
"INPUTS": [
	{
		"NAME" : 		"scale",
		"TYPE" : 		"float",
		"DEFAULT" : 	1.5,
		"MIN" : 		0.5,
		"MAX" : 		3.0
	},
	{
		"NAME" : 		"rate",
		"TYPE" : 		"float",
		"DEFAULT" : 	0.01,
		"MIN" : 		-0.5,
		"MAX" : 		0.5
	},
	{
   		"NAME" : 		"flip",
     	"TYPE" : 		"bool",
     	"DEFAULT" : 	false
   	},
	{
   		"NAME" : 		"rot",
     	"TYPE" : 		"bool",
     	"DEFAULT" : 	true
   	},
   	{
   		"NAME" : 		"invert",
     	"TYPE" : 		"bool",
     	"DEFAULT" : 	true
   	}
	]
}*/




////////////////////////////////////////////////////////////
// 5dTiles   by mojovideotech
//
// based on : 
// shadertoy.com/\ltdSz4  by David Crooks
//
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0
////////////////////////////////////////////////////////////

struct vec5 {
    vec4 a;
    float v;   
};
    
vec5 plane5(vec5 origin, vec5 u, vec5 v, vec2 p){
	return vec5(origin.a + p.x*u.a + p.y*v.a,
                origin.v + p.x*u.v + p.y*v.v);
}

vec5 mult5(vec5 p, float multiplier) {
    p.a *=  multiplier;
    p.v *= multiplier;
    return p;
}

vec5 mod5(vec5 p, float m) { return vec5(mod(p.a,m),mod(p.v,m)); }

vec3 hsv2rgb(vec3 c) {
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 0.5), c.y);
}

bool dualTileZoneTest(vec5 p , float value) {
    bool down  = all(lessThanEqual(vec4(value),p.a)) && value <= p.v && all(lessThanEqual(vec4(value),vec4(1.0)-p.a)) && value <= (1.0-p.v);
    bool up  = all(greaterThanEqual(vec4(value),p.a)) && value >= p.v && all(greaterThanEqual(vec4(value),vec4(1.0)-p.a)) && value >= (1.0-p.v);
    return down || up;
}

vec3 pattern(vec5 p ){
    
    float hueDelta = 0.8;
    
    p = mod5(p,1.0);
    
    if(dualTileZoneTest(p , p.a.x)){
        return  vec3(0.2, 0.0, 0.7);
    }
    else if(dualTileZoneTest(p,  p.a.y)){
         return vec3(0.1, 0.2, 0.1);
    }
    else if(dualTileZoneTest(p, p.a.z)){
         return vec3(0.7, 0.6, 0.6);
    }
    else if(dualTileZoneTest(p, p.a.w)){
         return vec3(0.0, 0.2, 0.9);
    }
    else if(dualTileZoneTest(p,  p.v)){
         return vec3(0.7, 0.2, 0.0);
    }
    else {
          return vec3(0.0);
    }   
}

void main()
{
    vec2 p = (gl_FragCoord.xy - 0.5*RENDERSIZE.xy) / RENDERSIZE.y;
   	if (flip) { p *= -1.0; }
	if (rot) { p.xy = -p.yx; }
    p /=scale;
    float T = rate*TIME;

    vec5 origin = vec5(vec4(T),-T);

    vec5 u = vec5(vec4(-0.511667,0.19544,0.19544,-0.511667),0.632456) ;
    vec5 v = vec5(vec4(-0.371748,0.601501,-0.601501,0.371748), 0.0);
    
    vec5 plane = plane5(origin,u,v,p);
    plane = mult5(plane,5.0);

    vec3 color = pattern(plane);
    if (invert) { color = 1.0 - color; }
    gl_FragColor =  vec4(color, 1.0);
}
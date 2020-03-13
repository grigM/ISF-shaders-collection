/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel1",
      "PATH" : [
      
      	"gas_pano_cube_backw.jpg",
        "gas_pano_cube_forward.jpg",
        "gas_pano_cube_botom.jpg",
        "gas_pano_cube_botom.jpg",
        "gas_pano_cube_left.jpg",
        "gas_pano_cube_right.jpg"
        
        
        
      ],
      "TYPE" : "cube"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3sS3Wc by jaszunio15.  Experimenting with refraction - trying to create infinite falling water drop.\nCode was abandoned, it doesn't produce exact effect I wanted to make. And source code is quite messy, I was learning. \n\nSong: https:\/\/soundcloud.com\/thisisklyne\/klyne-water-flow",
  "INPUTS" : [
    
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    },
    
    {
            "NAME": "Beat",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": 0.0,
            "MAX": 10.0
    },
    
     {
            "NAME": "CAMERA_ROTATION_SPEED",
            "TYPE": "float",
            "DEFAULT": 2.0,
            "MIN": 0.0,
            "MAX": 10.0
    },
    
    {
            "NAME": "AUTO_CAMERA_ROTATION",
            "TYPE": "bool",
            "DEFAULT": false,
            
    },
    
    {
            "NAME": "DROP_BRIGHTNESS",
            "TYPE": "float",
            "DEFAULT": -0.1,
            "MIN": -0.5,
            "MAX": 0.2
    },
    
    {
            "NAME": "DROP_RADIUS",
            "TYPE": "float",
            "DEFAULT": 1.8,
            "MIN": 0.2,
            "MAX": 3.0
    },
    
    {
            "NAME": "WATER_EFFECT_MULTIPLIER",
            "TYPE": "float",
            "DEFAULT": 0.15,
            "MIN": 0.0,
            "MAX": 0.5
    },
    
    
    {
            "NAME": "REFLECTIONS",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 2.0
    },
    
    
    {
            "NAME": "REFRACTIVE_INDEX",
            "TYPE": "float",
            "DEFAULT": 1.1,
            "MIN": 0.0,
            "MAX": 2.0
    },


	
    {
            "NAME": "DISTORTION_DENSITY",
            "TYPE": "float",
            "DEFAULT": 3.0,
            "MIN": 0.0,
            "MAX": 6.0
    },
    {
            "NAME": "EFFECT_SPEED",
            "TYPE": "float",
            "DEFAULT": 40.0,
            "MIN": 0.0,
            "MAX": 100.0
    },
    
    
    {
      "NAME": "showBackground",
      "TYPE": "bool",
      "DEFAULT": 0
    },
  ]
}
*/


#define RAY_STEPS_COUNT 50
#define NEAR_PLANE 1.3
//#define DROP_BRIGHTNESS 0.1

//#define DROP_RADIUS 1.8
//#define WATER_EFFECT_MULTIPLIER 0.15
//#define REFLECTIONS 0.5
//#define REFRACTIVE_INDEX 1.1
//#define DISTORTION_DENSITY 3.0
//#define EFFECT_SPEED 40.0

#define TIME (TIME * 0.2)



#define PI 3.1415926
#define EPSILON 0.01

float hash13(vec3 v)
{
    return fract(sin(dot(v, vec3(11.51721, 67.12511, 9.7561))) * 1551.4172);  
}

vec3 hash33(vec3 v)
{
 	return fract(sin(v * mat3(11.51721, 67.12511, 9.7561,
                              85.1741, 7.4751, 9.4371,
                              7.01641, 9.7561, 1.48141)) * 1551.4172);   
}

vec3 getNoise33(vec3 v)
{
	vec3 rootV = floor(v);
    vec3 f = smoothstep(0.0, 1.0, fract(v));
    
    vec3 n000 = hash33(rootV);
    vec3 n001 = hash33(rootV + vec3(0,0,1));
    vec3 n010 = hash33(rootV + vec3(0,1,0));
    vec3 n011 = hash33(rootV + vec3(0,1,1));
    vec3 n100 = hash33(rootV + vec3(1,0,0));
    vec3 n101 = hash33(rootV + vec3(1,0,1));
    vec3 n110 = hash33(rootV + vec3(1,1,0));
    vec3 n111 = hash33(rootV + vec3(1,1,1));
    
    vec3 n00 = mix(n000, n001, f.z);
    vec3 n01 = mix(n010, n011, f.z);
    vec3 n10 = mix(n100, n101, f.z);
    vec3 n11 = mix(n110, n111, f.z);
    
    vec3 n0 = mix(n00, n01, f.y);
    vec3 n1 = mix(n10, n11, f.y);
    
    return mix(n0, n1, f.x);
}

//sphere -> drop
void dropifyPoint(inout vec3 v)
{
    float dropRadius = pow(dot(v.xz, v.xz), 0.25) - (0.6 + sin(0. * 2.0) * 0.2);
    float dropY = v.y + 0.0;
    dropY *= 0.7;
    dropY += (v.y + 5.0) * dropRadius * 0.3;
    v.y = mix(v.y, dropY, smoothstep(-1.0, 1.0, v.y));
    v.xz *= 2.0;
}

// x)
float dropDistance(vec3 v, float time, float beat)
{
    vec3 modifier = getNoise33(v * DISTORTION_DENSITY * (0.9 + beat * 0.1) - time * EFFECT_SPEED * vec3(0, 1, 0) + beat * 0.2);
    dropifyPoint(v);
    v += (modifier.rgb - 0.5) * WATER_EFFECT_MULTIPLIER;
 	float dist = sqrt(dot(v, v));
    dist = -DROP_RADIUS + dist;
    
    return dist;
}

vec3 getDropNormal(vec3 point, float time, float beat)
{
 	float p000 = dropDistance(point, time, beat);
    float p001 = dropDistance(point + vec3(0,0,EPSILON), time, beat);
    float p010 = dropDistance(point + vec3(0,EPSILON,0), time, beat);
    float p100 = dropDistance(point + vec3(EPSILON,0,0), time, beat);
    
    return normalize(vec3(p100 - p000, p010 - p000, p001 - p000));
}
  
float dropMarchInside(vec3 rayStart, vec3 rayDirection, float beat, inout vec3 resultPoint, inout vec3 resultNormal)
{
    vec3 point = rayStart;
    float dropDist = 0.0;
    
 	for (int i = 0; i < RAY_STEPS_COUNT * 1 && abs(dropDist) < 50.0; i++)
    {
        dropDist = -dropDistance(point, TIME, beat);
        point += rayDirection * dropDist * 0.3;
    }
    
    resultNormal = -getDropNormal(point, TIME, beat);  
    resultPoint = point;
    
    return dropDist;
}

float dropMarch(vec3 rayStart, vec3 rayDirection, float beat, inout vec3 resultPoint, inout vec3 resultNormal)
{
    vec3 point = rayStart;
    float dropDist = 0.0;
    
 	for (int i = 0; i < RAY_STEPS_COUNT && abs(dropDist) < 50.0; i++)
    {
        dropDist = dropDistance(point, TIME, beat);
        point += rayDirection * dropDist * 0.3;
    }
    
    resultNormal = getDropNormal(point, TIME, beat);  
    resultPoint = point;
    
    return dropDist;
}


float getBeat(float offset, float bandStep, float bandsCount)
{
 	float beat = 0.0;
    for (float i = 0.0; i < bandsCount; i++)   
    {
        float bandCoord = offset + i * bandStep;
        bandCoord *= bandCoord;
        beat += Beat;
        
     }
    
    return beat /= bandsCount;
}

void main() {



    vec2 res = RENDERSIZE.xy;
    vec2 uv = 2.0 * gl_FragCoord.xy - res;
    uv /= min(res.x, res.y);
    
    vec2 mouse;
    if(length(iMouse.xy) >= 1.0) mouse = (2.0 * iMouse.xy - RENDERSIZE.xy) / RENDERSIZE.xy;
    else mouse = vec2(0, 0.2);
    
    
    float beat = getBeat(0.01, 0.01, 64.0);
    beat *= beat;
    beat = smoothstep(0.35, 0.6, beat) * 2.0;
    
    float angleY = -mouse.x * 3.0 * PI + sin(TIME * CAMERA_ROTATION_SPEED * 0.2) * 5.0;
    float angleX = -mouse.y * PI * 0.4 + PI * 0.5 + cos(TIME * CAMERA_ROTATION_SPEED * 0.97) * 0.1 - 0.6;
    float angleZ = PI * 0.5 + 0. * 0.2; //PI * 0.5 + sin(TIME * CAMERA_ROTATION_SPEED * 0.5 * 0.891) * 0.2;
   
    mat3 rotation =   mat3(sin(angleY),  0, cos(angleY),
                           0, 			 1, 0,
                           -cos(angleY), 0, sin(angleY))
        			* mat3(1, 0, 			0,
                           0, sin(angleX),  cos(angleX),
                           0, -cos(angleX), sin(angleX))//
        			* mat3(sin(angleZ),  cos(angleZ), 0,
                           -cos(angleZ), sin(angleZ), 0,
                           0, 			 0, 		  1);
    vec3 cameraCenter = rotation * vec3(sin(TIME * 0.3) * 1.0, sin(TIME * 0.6) * 1.0, 5.0 + cos(TIME * 0.9));
    
    vec3 rayDirection = normalize(rotation * vec3(uv.x, uv.y, -NEAR_PLANE + sin(TIME * 1.0) * 0.2 * (1.0 + beat * 0.15)));
	if(!AUTO_CAMERA_ROTATION){
		cameraCenter = rotation * vec3(sin(0. * 0.3) * 1.0, sin(0. * 0.6) * 1.0, 5.0 + cos(0. * 0.9));
		rayDirection = normalize(rotation * vec3(uv.x, uv.y, -NEAR_PLANE + sin(0.0 * 1.0) * 0.2 * (1.0 + beat * 0.15)));
	}
	
	
	
    
    float rayOriginShift = max(length(cameraCenter) - DROP_RADIUS * 2.0, 0.0);
    vec3 rayStart = cameraCenter + rayDirection * rayOriginShift;
    
    rayDirection = normalize(rayDirection);
    
    vec3 col = vec3(0);
    float al = 0.0;
     
    //Optimisation - bounding cylinder
    vec3 toCamera = normalize(vec3(cameraCenter.x, 0.0, cameraCenter.z));
    vec3 r = cross(toCamera, vec3(0, 1, 0));
    vec3 f = r * DROP_RADIUS * 0.57;
    float cosAlpha = dot(-toCamera, normalize(f - vec3(cameraCenter.x, 0.0, cameraCenter.z)));
    
    if(dot(-toCamera, normalize(vec3(rayDirection.x, 0, rayDirection.z))) > cosAlpha)
    {
        vec3 rayStartDirection = rayDirection;
        vec3 resultNormal = vec3(0);
        vec3 resultPoint = vec3(0);
        
        //marching to drop
        float dist = dropMarch(rayStart, rayDirection, beat, resultPoint, resultNormal);
        
        //1 - drop hit, 0 - no hit
        float drop = smoothstep(0.1, 0.099998, abs(dist));
        
        if (drop > 0.1)
        {
            //Environment reflections on surface
            vec3 reflectVec = reflect(rayDirection, resultNormal);
            vec3 reflections = textureCube(iChannel1,reflectVec).rgb * smoothstep(0.4, 0.0, abs(dist)) * REFLECTIONS;
            reflections = reflections * (1.0 - dot(-rayDirection, resultNormal));
            col += reflections * drop * (1.0 + beat);
            
            //Spherical lighting
            vec3 sphereLight = textureCube(iChannel1,resultNormal).rgb;
            col += sphereLight * DROP_BRIGHTNESS * drop * (1.0 + beat);
            al = 1.0;
 
            
            //refractionVec
            rayDirection = mix(rayDirection, refract(rayDirection, resultNormal, 1.0 / REFRACTIVE_INDEX), drop);
            //marching inside the drop (I don't know how to make it properly, just experimenting)
            vec3 offsetVec = rayDirection * EPSILON * 0.09;
            dist = dropMarchInside(resultPoint + offsetVec, rayDirection, beat, resultPoint, resultNormal);
            rayDirection = mix(rayDirection, refract(rayDirection, resultNormal, REFRACTIVE_INDEX), drop); 
        }
        col += textureCube(iChannel1,rayDirection).rgb * drop;
        //al = 1.0;
        if(showBackground){
        	col += textureCube(iChannel1,rayStartDirection).rgb * (1.0 - drop);
        	al = 1.0;
        }
    }
    if(showBackground){
        
     col = textureCube(iChannel1,rayDirection).rgb;// * 0.0;
     al = 1.0;
    }
    //Some postprocess
   	col = smoothstep(-0.05, 1.0, col);
   
    /*
    if(col.r > 0.01 || col.g > 0.01 || col.b > 0.01){
    	al = 1.0;
    }*/
    
    
    gl_FragColor = vec4(col,  al);
    //gl_FragColor = smoothstep( .00001, 0.9999, vec4(col, al));  
}

/*

"585f9546c092f53ded45332b343144396c0b2d70d9965f585ebc172080d8aa58.jpg",
        "585f9546c092f53ded45332b343144396c0b2d70d9965f585ebc172080d8aa58_1.jpg",
        "585f9546c092f53ded45332b343144396c0b2d70d9965f585ebc172080d8aa58_2.jpg",
        "585f9546c092f53ded45332b343144396c0b2d70d9965f585ebc172080d8aa58_3.jpg",
        "585f9546c092f53ded45332b343144396c0b2d70d9965f585ebc172080d8aa58_4.jpg",
        "585f9546c092f53ded45332b343144396c0b2d70d9965f585ebc172080d8aa58_5.jpg"
        
    
        */
        
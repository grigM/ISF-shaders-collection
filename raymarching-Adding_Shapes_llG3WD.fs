/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "raymarching",
    "primitives",
    "cross",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llG3WD by harrisscott.  simple ray marching example with help from reddit, adding boxes to create a primitive cross shape.\ncredit to this post for the fundamental raymarching principals: https:\/\/www.reddit.com\/r\/twotriangles\/comments\/1hy5qy\/tutorial_1_writing_a_simple_distance_",
  "INPUTS" : [
	{
			"NAME": "EPSILON",
			"TYPE": "float",
			"MIN": 0.0001,
			"MAX": 2.0,
			"DEFAULT": 0.001
	},
	{
			"NAME": "side_1_h",
			"TYPE": "float",
			"MIN": 0.5,
			"MAX": 10.0,
			"DEFAULT": 2.0
	},{
			"NAME": "side_1_w",
			"TYPE": "float",
			"MIN": 0.5,
			"MAX": 1.0,
			"DEFAULT": 0.5
	},{
			"NAME": "side_2_h",
			"TYPE": "float",
			"MIN": 0.5,
			"MAX": 10.0,
			"DEFAULT": 2.0
	},{
			"NAME": "side_3_h",
			"TYPE": "float",
			"MIN": 0.5,
			"MAX": 10.0,
			"DEFAULT": 2.0
	}
  ]
}
*/


/* ***************************************************** */
float sphere(vec3 pos, float radius)
{
    return length(pos) - radius;
}

/* ***************************************************** */
float Box(vec3 pos, vec3 b)
{
    return length(max(abs(pos)-b,0.0));
}

/* ***************************************************** */
float Torus( vec3 p, vec2 t )
{
  vec2 q = vec2(length(p.xz)-t.x,p.y);
  return length(q)-t.y;
}

/* ***************************************************** */
float AddTheseTwoShapes(float firstShape, float secondShape)
{
   return min(firstShape, secondShape);
}

/* ***************************************************** */
float BoxCross(vec3 pos)
{
    return AddTheseTwoShapes(AddTheseTwoShapes(Box(pos, vec3(side_1_h,side_1_w,0.5)),Box(pos, vec3(0.5,0.5,side_3_h))),Box(pos, vec3(0.5,side_2_h,0.5)));
}

/* ***************************************************** */
float displacement(vec3 p)
{
   return sin(p.x)*sin(p.y)*sin(p.z);
}

/* ***************************************************** */
float opDisplace( vec3 p )
{
    float d1 = Torus(p, vec2(1.0,1.0));
    float d2 = displacement(p);
    return d1+d2;
}

float opRep(vec3 p, vec3 c)
{
    vec3 q = mod(p,c)-0.5*c;
    return opDisplace(q);
}
               

/* ***************************************************** */
float distfunc(vec3 pos)
{
    return BoxCross(pos);
}

/* ***************************************************** */
void main()
{
    vec3 cameraOrigin = vec3(4.0, 2.0*cos(TIME), 2.0*sin(TIME));
    vec3 cameraTarget = vec3(0.0, 0.0, 0.0);
    vec3 upDirection = vec3(0.0, 1.0, 0.0);
    
    vec3 cameraDir = normalize(cameraTarget - cameraOrigin);
    
    vec3 cameraRight = normalize(cross(upDirection, cameraOrigin));
    vec3 cameraUp = cross(cameraDir, cameraRight);
    
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    
    vec2 screenPos = -1.0 + 2.0 * gl_FragCoord.xy / RENDERSIZE.xy; // screenPos can range from -1 to 1
screenPos.x *= RENDERSIZE.x / RENDERSIZE.y; // Correct aspect ratio
    
    vec3 rayDir = normalize(cameraRight * screenPos.x + cameraUp * screenPos.y + cameraDir);
    
    const int MAX_ITER = 100; // 100 is a safe number to use, it won't produce too many artifacts and still be quite fast
const float MAX_DIST = 20.0; // Make sure you change this if you have objects farther than 20 units away from the camera
//const float EPSILON = 0.001; // At this distance we are close enough to the object that we have essentially hit it

    
    
    float totalDist = 0.0;
vec3 pos = cameraOrigin;
float dist = EPSILON;

for (int i = 0; i < MAX_ITER; i++)
{
    // Either we've hit the object or hit nothing at all, either way we should break out of the loop
    if (dist < EPSILON || totalDist > MAX_DIST)
        break; // If you use windows and the shader isn't working properly, change this to continue;

    dist = distfunc(pos); // Evalulate the distance at the current point
    totalDist += dist;
    pos += dist * rayDir; // Advance the point forwards in the ray direction by the distance
}
    if (dist < EPSILON)
{
    // Lighting code
}
else
{
    gl_FragColor = vec4(0.0);
}
    vec2 eps = vec2(0.0, EPSILON);
vec3 normal = normalize(vec3(
    distfunc(pos + eps.yxx) - distfunc(pos - eps.yxx),
    distfunc(pos + eps.xyx) - distfunc(pos - eps.xyx),
    distfunc(pos + eps.xxy) - distfunc(pos - eps.xxy)));
    
    float diffuse = max(0.0, dot(-rayDir, normal));
    float specular = pow(diffuse, 35.0);
    
    vec3 color = vec3(diffuse + specular);
gl_FragColor = vec4(color, 1.0);
    
	gl_FragColor = vec4(color,1.0);
}
/* ***************************************************** */

/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#49867.1"
}
*/


#ifdef GL_ES //http://glsl.heroku.com/e#9215.0 
precision mediump float;
#endif 


const float EPSILON = 0.00005;

float SoftMax(float a, float b, float k)
{
	return log(exp(k*a)+exp(k*b))/k;
}

float SoftMin(float a, float b, float k)
{
	return -(log(exp(k*-a)+exp(k*-b))/k);
}

struct Camera
{
	vec3 position;
	vec3 dir;
	vec3 up;
	vec3 rayDir;
};

struct MapResult
{
	float dist;
	vec3 color;
};
	
struct MarchResult
{
	vec3 position;
	float dist;
	vec3 color;
};

float smoothcurve(float f) {
	return 0.5*(1.0+cos(3.14*f));
}

mat4 rotationMatrix(vec3 axis, float angle)
{
	axis = normalize(axis);
	float s = sin(angle);
	float c = cos(angle);
	float oc = 1.0 - c;
	return mat4(oc * axis.x * axis.x + c, oc * axis.x * axis.y - axis.z * s, oc * axis.z * axis.x + axis.y * s, 0.0,
		    oc * axis.x * axis.y + axis.z * s, oc * axis.y * axis.y + c, oc * axis.y * axis.z - axis.x * s, 0.0,
		    oc * axis.z * axis.x - axis.y * s, oc * axis.y * axis.z + axis.x * s, oc * axis.z * axis.z + c, 0.0,
		    0.0, 0.0, 0.0, 1.0);
}

vec3 opCheapBend(vec3 p, float x, float y)
{
    float c = cos(x*p.y);
    float s = sin(y*p.y);
    mat2  m = mat2(c,-s,s,c);
    vec3  q = vec3(m*p.xy,p.z);
    return q;
}

vec3 opTwist(vec3 p, float x, float y)
{
    float c = cos(x*p.y);
    float s = sin(y*p.y);
    mat2  m = mat2(c,-s,s,c);
    vec3  q = vec3(m*p.xz,p.y);
    return q;
}

MapResult map_cube(vec3 position)
{
	MapResult result;
	
	
	float sphere = length(position) - 1.2;
	float TIME = TIME - position.y;

	
	float c = 10.0;
	
	
	position = (rotationMatrix(vec3(0,0,1), TIME) * vec4(position, 1.0)).xyz;
	
	
	position = (rotationMatrix(vec3(0,1,0), TIME) * vec4(position, 1.0)).xyz;
	float cube = length(max(abs(position) - vec3(0.5), 0.0)) - 0.1;
	
	position = (rotationMatrix(vec3(1,0,0), TIME) * vec4(position, 1.0)).xyz;
	
	
	vec2 q = vec2(length(position.xz) - 2.5, position.y);
	float torus= length(q) - 0.2;
	
	
	float d = SoftMax(cube, -sphere, 7.5);
	//float d = max(cube, -sphere);
	
	vec3 color3 = vec3(.5, 0.0, 0.0);
	vec3 color2 = vec3(0.0, 0.5, 0.0);
	vec3 color1 = vec3(0.0, 0.0, 0.5);
	
	d =+ SoftMin(d, torus, 1.);
	
	result.color = mix(color1, mix(color3, color2, cube), torus) * 4.;
	
	result.dist = d;
	return result;
}

MapResult map_torus(vec3 position)
{
	MapResult result;
	result.color = vec3(0.0, 0.5, 0.8);
	
	position = (rotationMatrix(vec3(0,0,1), TIME) * vec4(position, 1.0)).xyz;
	position = (rotationMatrix(vec3(0,1,0), TIME) * vec4(position, 1.0)).xyz;
	position = (rotationMatrix(vec3(1,0,0), TIME) * vec4(position, 1.0)).xyz;
	
	vec2 q = vec2(length(position.xz) - 2.5, position.y);
	result.dist = length(q) - 0.2;
		
	return result;
}

MapResult map(vec3 position)
{
	MapResult result;
	
	MapResult cube = map_cube(position);
	

	result = cube;
	
	return result;
}

vec3 getColor(const in Camera cam, const in vec3 position, const in float dist, const in vec3 color)
{	
	vec3 eps = vec3(EPSILON, 0, 0);
	
	vec3 normal=normalize(
		   vec3(dist - map(position-eps.xyy).dist,
			dist - map(position-eps.yxy).dist,
			dist - map(position-eps.yyx).dist));
	
	float lambert = dot(normal, -cam.rayDir);
	
	return lambert * color;
}

MarchResult raymarch(const in Camera cam)
{
	MarchResult result;
	result.color = vec3(0);
	
	const int MAX_ITERATIONS = 128;
	const float MAX_DEPTH = 52.0;
	
	float depth = 0.0;
	MapResult mapping;
	for(int i = 0; i < MAX_ITERATIONS; ++i)
	{
		result.position = cam.position + cam.rayDir * depth;
		mapping = map(result.position);
		
		if(mapping.dist <= EPSILON)
		{
			break;
		}
		
		depth += mapping.dist;
				
		if(depth > MAX_DEPTH)
		{
			break;
		}
	}
	
	result.dist = mapping.dist;
	
	if(depth < MAX_DEPTH)
		result.color = getColor(cam, result.position, result.dist, mapping.color);

	return result;
}
	
Camera getCamera()
{
	Camera cam;
  	cam.dir = vec3(0,0,0);
	float t = (TIME+1.0) * 0.15;
	cam.position = vec3(sin(t + 1.0)*4.0, 4, cos(t)*4.0);
	cam.up = vec3(0,1,0);
  	vec3 forward = normalize(cam.dir - cam.position);
  	vec3 left = cross(forward, cam.up);
 	cam.up = cross(left, forward);
 
	vec3 screenOrigin = (cam.position+forward);
	vec2 screenPos = 2.0*gl_FragCoord.xy/RENDERSIZE.xy - 1.0;
 	float screenAspectRatio = RENDERSIZE.x/RENDERSIZE.y;
	vec3 screenHit = screenOrigin + screenPos.x * left * screenAspectRatio + screenPos.y * cam.up;
  	cam.rayDir = normalize(screenHit-cam.position);
	
	const float pi = 3.14159;
	if(true){ // `360` view from center
		cam.position = vec3(0);
		float a1 = (RENDERSIZE.x-gl_FragCoord.x-1.)*pi*2./(RENDERSIZE.x-1.) + pi/2.;
		float a2 = (RENDERSIZE.y-gl_FragCoord.y-1.)*pi/(RENDERSIZE.y-1.);
		cam.rayDir = vec3(cos(a1)*sin(a2), sin(a1)*sin(a2), cos(a2));
		
	}
	return cam;
}

void main() 
{	
	Camera cam = getCamera();
	MarchResult result = raymarch(cam);
		
	vec3 color = result.color; //AddMore(zOrder);
	
	gl_FragColor = vec4(color, 1.0);
}
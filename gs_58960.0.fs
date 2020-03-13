/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#58960.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/wsGSRz
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE

// Emulate some GLSL ES 3.x
#define mat2x2 mat2
#define mat3x3 mat3

// --------[ Original ShaderToy begins here ]---------- //
#define RAYMARCH_ITERATIONS 60.0
#define TIME (iTime * 0.4)
#define LINE_LENGTH 1.0
#define LINE_SPACE 1.0
#define LINE_WIDTH 0.007
#define BOUNDING_CYLINDER 1.8
#define INSIDE_CYLINDER 0.32
#define EPS 0.0001
#define FOG_DISTANCE 30.0

#define FIRST_COLOR vec3(1.2, 0.5, 0.2) * 1.2
#define SECOND_COLOR vec3(0.2, 0.8, 1.1)

float hash12(vec2 x)
{
 	return fract(sin(dot(x, vec2(42.2347, 43.4271))) * 342.324234);   
}

vec2 hash22(vec2 x)
{
 	return fract(sin(x * mat2x2(23.421, 24.4217, 25.3271, 27.2412)) * 342.324234);   
}

vec3 hash33(vec3 x)
{
 	return fract(sin(x * mat3x3(23.421, 24.4217, 25.3271, 27.2412, 32.21731, 21.27641, 20.421, 27.4217, 22.3271)) * 342.324234);   
}


mat3x3 rotationMatrix(vec3 angle)
{
 	return 	mat3x3(cos(angle.z), sin(angle.z), 0.0,
                 -sin(angle.z), cos(angle.z), 0.0,
                 0.0, 0.0, 1.0)
        	* mat3x3(1.0, 0.0, 0.0,
                    0.0, cos(angle.x), sin(angle.x),
                    0.0, -sin(angle.x), cos(angle.x))
        	* mat3x3(cos(angle.y), 0.0, sin(angle.y),
                    0.0, 1.0, 0.0,
                    -sin(angle.y), 0.0, cos(angle.y));
}

vec3 castPlanePoint(vec2 fragCoord)
{
 	vec2 uv = (2.0 * fragCoord - iResolution.xy) / iResolution.x;
    return vec3(uv.x, uv.y, -1.0);
}

float planeSDF(vec3 point)
{
 	return point.y;
}

//source https://www.iquilezles.org/www/articles/distfunctions/distfunctions.htm
float boxSDF( vec3 point, vec3 bounds )
{
    vec3 q = abs(point) - bounds;
    return length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0);
}

//rgb - colors
//a - sdf
vec4 repeatBoxSDF(vec3 point)
{
    vec3 rootPoint = floor(vec3(point.x / LINE_SPACE, point.y / LINE_SPACE, point.z / LINE_LENGTH)); 
    rootPoint.z *= LINE_LENGTH;
    rootPoint.xy *= LINE_SPACE;
    float minSDF = 10000.0;
    vec3 mainColor = vec3(0.0);
    
    for (float x = -1.0; x <= 1.1; x++)
    {
        for (float y = -1.0; y <= 1.1; y++)
        {
			for (float z = -1.0; z <= 1.1; z++)
            {
				vec3 tempRootPoint = rootPoint + vec3(x * LINE_SPACE, y * LINE_SPACE, z * LINE_LENGTH);
                
                vec3 lineHash = hash33(tempRootPoint);
                lineHash.z = pow(lineHash.z, 10.0);
                
                float hash = hash12(tempRootPoint.xy) - 0.5;
                tempRootPoint.z += hash * LINE_LENGTH;
                
                vec3 boxCenter = tempRootPoint + vec3(0.5 * LINE_SPACE, 0.5 * LINE_SPACE, 0.5 * LINE_LENGTH);
                boxCenter.xy += (lineHash.xy - 0.5) * LINE_SPACE;
                vec3 boxSize = vec3(LINE_WIDTH, LINE_WIDTH, LINE_LENGTH * (1.0 - lineHash.z));
                
                vec3 color = FIRST_COLOR;
                if(lineHash.x < 0.5) color = SECOND_COLOR;
                
                float sdf = boxSDF(point - boxCenter, boxSize);
                if (sdf < minSDF)
                {
                    mainColor = color;
                    minSDF = sdf;
                }
            }
        }
    }
    
    return vec4(mainColor, minSDF);
}

float cylinderSDF(vec3 point, float radius)
{
 	return length(point.xy) - radius;
}

float multiplyObjects(float o1, float o2)
{
 	return max(o1, o2);   
}

vec3 spaceBounding(vec3 point)
{
 	return vec3(sin(point.z * 0.15) * 5.0, cos(point.z * 0.131) * 5.0, 0.0); 
}

//rgb - color,
//a - sdf
vec4 objectSDF(vec3 point)
{
    point += spaceBounding(point);
    
    vec4 lines = repeatBoxSDF(point);
    float cylinder = cylinderSDF(point, BOUNDING_CYLINDER);
    float insideCylinder = -cylinderSDF(point, INSIDE_CYLINDER);
    
    float object = multiplyObjects(lines.a, cylinder);
    object = multiplyObjects(object, insideCylinder);
 	return vec4(lines.rgb, object);
}


vec3 rayMarch(vec3 rayOrigin, vec3 rayDirection, out vec3 color)
{
    color = vec3(0.0);
    float dist = 0.0;
 	for (float i = 0.0; i < RAYMARCH_ITERATIONS; i++)
    {
     	vec4 sdfData = objectSDF(rayOrigin);
        color += sdfData.rgb * sqrt(smoothstep(0.8, 0.0, sdfData.a)) * pow(smoothstep(FOG_DISTANCE * 0.6, 0.0, dist), 3.0) * 0.2;
        rayOrigin += rayDirection * sdfData.a * 0.7;
        dist += sdfData.a;
        if (length(rayOrigin.xy) > BOUNDING_CYLINDER + 10.0) break;
    }


    return rayOrigin;
}
 
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	vec3 cameraCenter = vec3(0.0, 0.0, -TIME * 10.0);
    cameraCenter -= spaceBounding(cameraCenter);
    vec3 cameraAngle = vec3(0.0, 0.0, 0.0);
    
    vec3 prevCameraCenter = vec3(0.0, 0.0, -(TIME - 0.01) * 10.0);
    prevCameraCenter -= spaceBounding(prevCameraCenter);
    vec3 nextCameraCenter = vec3(0.0, 0.0, -(TIME + 0.4) * 10.0);
    nextCameraCenter -= spaceBounding(nextCameraCenter);
    
    vec3 velocityVector = -normalize(nextCameraCenter - prevCameraCenter);
    vec3 cameraUp = -normalize(cross(velocityVector, vec3(1.0, 0.0, 0.0)));
    vec3 cameraRight = -normalize(cross(velocityVector, cameraUp));
    
    mat3x3 cameraRotation = rotationMatrix(cameraAngle);
    
    cameraRotation = mat3x3(cameraRight, cameraUp, velocityVector);
    
    vec3 rayOrigin = cameraCenter;
    vec3 rayDirection = normalize(castPlanePoint(fragCoord) * cameraRotation);
    
    vec3 color = vec3(0.0);
    vec3 hitPoint = rayMarch(rayOrigin, rayDirection, color);
    vec4 sdf = objectSDF(hitPoint);
    
    float vision = smoothstep(0.01, 0.0, sdf.a);
    
    float fog = sqrt(smoothstep(FOG_DISTANCE, 0.0, distance(cameraCenter, hitPoint)));
    
    vec3 ambient = mix(SECOND_COLOR, FIRST_COLOR, pow(sin(TIME) * 0.5 + 0.5, 2.0) * 0.6);
    ambient *= sqrt((sin(TIME) + sin(TIME * 3.0)) * 0.25 + 1.0);
    vec3 bloom = smoothstep(-0.0, 15.0, color);
    
    color = color * vision * 0.07 * fog + bloom + ambient * 0.3;
    color = smoothstep(-0.01, 1.5, color * 1.1);
    
    fragColor = vec4(color, 1.0);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35502.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float map(vec3 point) {
	vec3 pointInCurrentSpace = fract(point) * 2.0 - 1.0;
	
	//float xDifference = abs(pointInCurrentSpace.x) - 0.25;
	//float yDifference = abs(pointInCurrentSpace.y) - 0.25;
	//float zDifference = abs(pointInCurrentSpace.z) - 0.25;
	
	//return max(xDifference, max(yDifference, zDifference));
	
	float yzDifference = length(pointInCurrentSpace.yz) - 0.1;
	float xyDifference = length(pointInCurrentSpace.xy) - 0.25;
	return min(yzDifference, xyDifference);
	
	//return length(pointInCurrentSpace) - 0.25;
}

float trace(vec3 origin, vec3 ray) {
	float pointAlongRay = 0.0;
	for (int i = 0; i < 32; i++) {
		vec3 pointInSpace = origin + (ray * pointAlongRay);
		pointAlongRay += map(pointInSpace) * 0.5;
	}
	return pointAlongRay;
}

void main( void ) {

	vec2 position = gl_FragCoord.xy / RENDERSIZE.xy;
	position -= 0.5;
	position.x *= RENDERSIZE.x / RENDERSIZE.y;
	
	vec3 origin = vec3(0.0, 0.0, TIME);
	vec3 ray = vec3(position, 1.0);
	
	float distanceToIntersectionPoint = trace(origin, ray);
	float pixelIntensity = 1.0 / (1.0 + distanceToIntersectionPoint * distanceToIntersectionPoint * 0.1);
	
	gl_FragColor = vec4(vec3(pixelIntensity), 1.0);

}
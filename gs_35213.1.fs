/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
   "INPUTS" : [
  {
			"NAME": "speedX",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -6.0,
			"MAX": 6.0
	},
  {
			"NAME": "speedY",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": -6.0,
			"MAX": 6.0
	},
  {
			"NAME": "speedZ",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": -6.0,
			"MAX": 6.0
	},    {
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35213.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float map(vec3 point) {
	vec3 pointInCurrentSpace = fract(point) * 2.0 - 1.0;
	
	float halfCubeSideLength = 0.25;
	
	float xDiff = abs(pointInCurrentSpace.x) - halfCubeSideLength;
	float yDiff = abs(pointInCurrentSpace.y) - halfCubeSideLength;
	float zDiff = abs(pointInCurrentSpace.z) - halfCubeSideLength;
	
	return max(max(xDiff, yDiff), zDiff);
	return length(pointInCurrentSpace) - 0.25;
}

float trace(vec3 camPos, vec3 ray) {
	float pointAlongRay = 0.0;
	for (int i = 0; i < 32; i++) {
		vec3 pointInSpace = camPos + (ray * pointAlongRay);
		pointAlongRay += map(pointInSpace) * 0.5;
	}
	return pointAlongRay;
}

void main( void ) {

	vec2 myPos = (gl_FragCoord.xy / RENDERSIZE.xy);
	myPos *= 2.0;
	myPos -= 1.0;
	myPos.x *= RENDERSIZE.x / RENDERSIZE.y;

	vec3 camPos = vec3(TIME*speedX, TIME*speedY, TIME*speedZ);
	
	vec3 ray = normalize(vec3(myPos, 1.0));
	
	float radiansToRotateX = (mouse.y / RENDERSIZE.y) * 6.28;
	ray.yz *= mat2(cos(radiansToRotateX), -sin(radiansToRotateX), sin(radiansToRotateX), cos(radiansToRotateX));
	
	
	float distanceToIntersection = trace(camPos, ray);
	
	float pixelBrightness = 1.0 / (1.0 + distanceToIntersection * distanceToIntersection * 0.1);
	
	vec3 pixelColor = vec3(pixelBrightness);

	gl_FragColor = vec4(pixelColor, 1.0);

}
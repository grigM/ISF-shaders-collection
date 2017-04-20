/*
{
  "CATEGORIES" : [
    "Automatically Converted"
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35233.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float map(vec3 point) {
	point.y += sin(point.z*0.4 + TIME);
	point.x += cos(point.z*0.4 + TIME);
	vec3 pointInCurrentSpace = fract(point) * 2.0 - 1.0;
	
	return length(pointInCurrentSpace) - 0.20;
}

float trace(vec3 camPos, vec3 ray) {
	float pointAlongRay = 0.0;
	for (int i = 0; i < 132; i++) {
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
	
	vec3 camPos = vec3(0.0, 0.0, TIME);
	
	vec3 ray = normalize(vec3(myPos, 1.0));
	
	float radiansToRotateAlongYAxis = (mouse.x / RENDERSIZE.x) * 6.28;
	ray.xz *= mat2(cos(radiansToRotateAlongYAxis), -sin(radiansToRotateAlongYAxis), sin(radiansToRotateAlongYAxis), cos(radiansToRotateAlongYAxis));
	
	float distanceToIntersectionPoint = trace(camPos, ray);
	
	float brightnessValueOfPixel = 1.0 / (1.0 + distanceToIntersectionPoint * distanceToIntersectionPoint * 0.1);
	
	vec3 pixelColor = vec3(brightnessValueOfPixel);
	
	gl_FragColor = vec4(pixelColor, 1.0);

}
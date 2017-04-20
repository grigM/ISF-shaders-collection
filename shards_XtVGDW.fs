/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex16.png"
    }
  ],
  "CATEGORIES" : [
    "noise",
    "purple",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtVGDW by mahalis.  some colors I like, moving around a bit",
  "INPUTS" : [

  ]
}
*/


const float M_PI_F = 3.14159;

vec2 noiseOffset(float time, vec2 uv, vec3 angleParamsX, vec3 angleParamsY, vec2 angleParamsTime, float baseDistance, vec3 distanceParamsX, vec3 distanceParamsY) {
	float angle = M_PI_F * (sin(angleParamsX.x * uv.x + angleParamsX.y * time + angleParamsX.z) + sin(angleParamsY.x * uv.y + angleParamsY.y * time + angleParamsY.z) + sin(angleParamsTime.x * time + angleParamsTime.y));
	float distance = baseDistance + baseDistance * (sin(distanceParamsX.x * uv.x + distanceParamsX.y * time + distanceParamsX.z) + sin(distanceParamsY.x * uv.y + distanceParamsY.y * time + distanceParamsY.z));
	return vec2(cos(angle), sin(angle)) * distance;
}

void main()
{
    vec2 centeredCoords = ((gl_FragCoord.xy / RENDERSIZE.xy) - 0.5) * 1.2;
    centeredCoords.y *= RENDERSIZE.y / RENDERSIZE.x;
	float centerDistance = length(centeredCoords);
	vec2 noiseUV1 = centeredCoords;
	noiseUV1 += noiseOffset(TIME, noiseUV1, vec3(2.3, 0.51, 0.0), vec3(-1.9, 0.47, 1.0), vec2(0.4, 0.4), 0.02, vec3(3.1, 0.44, 1.3), vec3(3.4, 0.53, 0.0));
	noiseUV1 *= (1.0 - 0.3 * pow(centerDistance * 2.0, 1.2));
	noiseUV1.y -= TIME * 0.05;
	noiseUV1.x += 0.3 * sin(TIME * 0.13);
	noiseUV1 *= 0.7; // scale it so we get larger specks
	
	vec2 noiseUV2 = centeredCoords;
	noiseUV2 += noiseOffset(TIME, noiseUV2, vec3(-1.62, 0.3, 2.1), vec3(-1.83, 0.42, 0.3), vec2(-0.2, 0.19), 0.01, vec3(4.28, 0.23, 0.5), vec3(3.93, 0.19, 1.1));
	noiseUV2 *= (1.0 - 0.4 * pow(centerDistance * 2.0, 1.3));
	noiseUV2.x += 0.2 * sin(TIME * 0.22 + 0.5);
	noiseUV2.y -= TIME * 0.02;
	noiseUV2 *= 0.83;

	float edgeAttenuation = pow(centerDistance, 2.0);

	float visibilitySample1 = IMG_NORM_PIXEL(iChannel0,mod(noiseUV1,1.0)).r;
	float colorSample1 = IMG_NORM_PIXEL(iChannel0,mod(vec2(noiseUV1.x + TIME * 0.005, noiseUV1.y),1.0)).g;
	float visibility1 = smoothstep(0.8, 0.9, visibilitySample1 - edgeAttenuation * 0.5);

	float visibilitySample2 = IMG_NORM_PIXEL(iChannel0,mod(noiseUV2,1.0)).b;
	float colorSample2 = IMG_NORM_PIXEL(iChannel0,mod(vec2(noiseUV2.x - TIME * 0.009, noiseUV2.y),1.0)).r;
	float visibility2 = smoothstep(0.85, 0.9, visibilitySample2 - edgeAttenuation * 0.4);
    
	gl_FragColor = vec4(mix(vec3(1.0,0.2,0.9), vec3(0.1,0.4,1.0), float(smoothstep(0.0, 1.0, colorSample2))) * visibility2 + mix(vec3(1.0, 0.1, 0.6), vec3(0.2, 0.8, 1.0), float(smoothstep(0.0, 1.0, colorSample1))) * visibility1, 1.0);
}
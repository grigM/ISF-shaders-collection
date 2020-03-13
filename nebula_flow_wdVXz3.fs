/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "3871e838723dd6b166e490664eead8ec60aedd6b8d95bc8e2fe3f882f0fd90f0.jpg"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wdVXz3 by mahalis.  Project for Nodevember day 24, “Nebula”.",
  "INPUTS" : [
		{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 3.0
	},
	{
		"NAME": "speed_2",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 3.0
	}
  ]
}
*/


// license: CC BY-NC https://creativecommons.org/licenses/by-nc/4.0/

vec2 r(vec2 p, float a) {
    float c = cos(a);
    float s = sin(a);
    return vec2(c * p.x - s * p.y, s * p.x + c * p.y);
}

const float twoPi = 6.283;

void main() {

    vec2 uv = (gl_FragCoord.xy - RENDERSIZE.xy / 2.) / RENDERSIZE.y;
    uv.x += 0.2 * sin(uv.y * 3.0 + (TIME) * 0.23) * uv.y; // slight distortion of the overall space
    float angle = atan(uv.y, uv.x);
    float normalizedAngle = angle / twoPi;
    
    float baseRadius = length(uv);
    
    float radius1 = baseRadius + sin(angle * 3.0 + (TIME*speed)) * 0.01 + sin(angle * 4.0) * 0.01;
    float radius2 = baseRadius + sin(angle * 4.0 + (TIME*speed) * 1.1 + 1.7) * 0.02 + sin(angle * 4.0 + 1.3 + (TIME*speed) * -0.9) * 0.01;
    
    float mask1 = IMG_NORM_PIXEL(iChannel0,mod(vec2(normalizedAngle + sin(radius1 * 4.0) * 0.03, pow(max(1.0 - (radius1 - 0.4), 0.0), 0.5) * 0.8 + (TIME*speed_2) * 0.2),1.0)).r;
    float mask2 = IMG_NORM_PIXEL(iChannel0,mod(vec2(normalizedAngle * 2.0 + 2. + sin(radius2 * 3.0) * 0.05 + sin(radius1 * 4.3) * 0.02, (radius2 - 0.4) * 0.2 - (TIME*speed) * 0.1),1.0)).g;
    
    float mainMask = mask1 * mask2 * 2.2; // arbitrary scaling factor, brighten things up a bit
    
    // fade out the center
    mainMask *= smoothstep(0.05, 0.2, radius1 + 0.005 * sin(angle * 5.0 - (TIME*speed) * 0.4));
    float outerMask = IMG_NORM_PIXEL(iChannel0,mod(vec2(normalizedAngle + (TIME*speed) * -0.02, baseRadius * 0.05 - (TIME*speed) * 0.1),1.0)).b;
    outerMask *= IMG_NORM_PIXEL(iChannel0,mod(vec2(normalizedAngle + 1.3 + (TIME*speed) * 0.03, baseRadius * 0.4 + (TIME*speed) * -0.05),1.0)).b * 2.0;
    
    // adjust the amount of masking applied based on the distance from center
    outerMask = smoothstep(1.4 - baseRadius, 0.7 - baseRadius, outerMask);
    
    mainMask *= outerMask;
    
    float colorMask = IMG_NORM_PIXEL(iChannel0,mod(vec2(normalizedAngle * 2.0 + (TIME*speed) * 0.11, baseRadius - (TIME*speed) * 0.3 + sin(baseRadius * 0.6) * 0.4),1.0)).r;
    
    const vec3 mainColor1 = vec3(1.2,0.3,0.7);
    const vec3 mainColor2 = vec3(0.3,1.0,1.0);
    
    vec3 color = mix(mainColor2, mainColor1, colorMask);
    
    // hide the color pattern a bit
    color -= vec3(0.4,0.2,0.2) * IMG_NORM_PIXEL(iChannel0,mod(vec2(normalizedAngle * 3.0 - (TIME*speed_2) * 0.13, baseRadius - (TIME*speed_2) * 0.23),1.0)).g;
    
    color = pow(color * mainMask, vec3(2.0)) * 3.0;
    
    gl_FragColor = vec4(color,1.0);
}

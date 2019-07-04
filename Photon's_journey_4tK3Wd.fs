/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "52d2a8f514c4fd2d9866587f4d7b2a5bfa1a11a0e772077d7682deb8b3b517e5.jpg"
    }
  ],
  "CATEGORIES" : [
    "raycast",
    "light",
    "effect",
    "volumetric",
    "photon",
    "journey",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tK3Wd by zguerrero.  first time trying volumetric effect, followed this amazing tutorial : http:\/\/www.blog.sirenix.net\/blog\/realtime-volumetric-clouds-in-unity, then started digressing to get this king of magical light effect.",
  "PASSES" : [
    {
      "TARGET" : "BufferA",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {
      "TARGET" : "BufferB",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {
      "TARGET" : "BufferC",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {

    }
  ],
  "INPUTS" : [

  ]
}
*/


float densityPow = 4.0;
float densityMultiplier1 = 300.0;
float densityMultiplier2 = 3.0;
float viewDistance = 10.0;
float fade = 0.5;
float fadeVariation = 0.1;
float twist = 1.0;
vec3 sinusSpaceDistortion = vec3(0.5, 0.5, 1.5);
float noiseZDistortion = 50.0;
float noiseZDistortionScale = 0.005;
float distortionTwist = 10.0;
vec3 noiseScale = vec3(0.75,0.75,0.25);
vec3 noiseSpeed = vec3(0.5,0.25,0.00);
vec3 cameraOrigin = vec3(0.0,0.0,0.0);
float moveSpeed = 2.0;
vec3 skyColor1 = vec3(0.0,0.0,0.1);
vec3 skyColor2 = vec3(1.25,1.15,1.0);
vec3 skyColor3 = vec3(0.15,0.0,0.1);
vec3 color1 = vec3(0.1,0.03,0.0);
vec3 color2 = vec3(0.25,0.25,1.0);
vec2 colorsPlacement = vec2(0.075,1.0);

//https:www.shadertoy.com/view/XsBXWt
mat2 rot(float a) 
{
	return mat2(cos(a),sin(a),-sin(a),cos(a));	
}

float noise(vec3 x) 
{ 
    vec3 s = noiseSpeed*TIME;
    x *= noiseScale.xyz;
    x.xy *= rot(x.z*distortionTwist);
    float texZ = IMG_NORM_PIXEL(iChannel0,mod(vec2(x.z*noiseZDistortionScale, s.z),1.0)).x;
    float tex = IMG_NORM_PIXEL(iChannel0,mod(x.xy + vec2(texZ-0.5)*noiseZDistortion + s.xy,1.0)).x;   
    return tex;
}

vec3 rayCast(vec3 rayDir, vec3 cameraOrigin)
{
    const int ITER = 100;
    float iterations = float(ITER);
	vec3 p = cameraOrigin;
    vec3 density = vec3(0.0);
    for(int i = 0; i < 70; i++)
	{
        p.x += sin(p.z*sinusSpaceDistortion.y)*sinusSpaceDistortion.x;
        p.y += sin(p.z*sinusSpaceDistortion.y + sinusSpaceDistortion.z)*sinusSpaceDistortion.x;
        p.xy *= rot(p.z*twist);
        float ifloat = float(i);
		float f = ifloat / iterations;
        vec3 c = mix(color1, color2, smoothstep(colorsPlacement.x, colorsPlacement.y, f));
        float clouds1 = noise(p + vec3(0.0,0.0,TIME*moveSpeed));
		float clouds2 = pow(clouds1, densityPow);
        float dfade = smoothstep(1.0, 0.0, length(p.xy*(fade+sin(TIME)*fadeVariation)));
		density += clouds1*clouds1*c*densityMultiplier2 + clouds2*dfade*densityMultiplier1*c;
		p = cameraOrigin + rayDir * f * viewDistance;
	}
    return density/iterations;
}

vec3 skyBox(vec3 rayDir)
{
    float c = clamp(1.0 - length(rayDir.xy),0.0, 1.0);
    
    return mix(mix(skyColor1, skyColor3, rayDir.y), skyColor2, c);
}

//https://www.shadertoy.com/view/Xds3zN
mat3 setCamera( in vec3 ro, in vec3 ta, float cr )
{
	vec3 cw = normalize(ta-ro);
	vec3 cp = vec3(sin(cr), cos(cr), 0.0);
	vec3 cu = normalize( cross(cw,cp) );
	vec3 cv = normalize( cross(cu,cw) );
    return mat3( cu, cv, cw );
}

float sampleDistance = 0.05;



void main() {
	if (PASSINDEX == 0)	{
   

	    vec3 cameraTarget = vec3(sin(TIME),cos(TIME),4.0);
	    cameraOrigin += vec3(sin(TIME*0.7)*1.5,0.0,0.0);
	        
		vec2 screenPos = (gl_FragCoord.xy/RENDERSIZE.xy)*2.0-1.0;
		screenPos.x *= RENDERSIZE.x/RENDERSIZE.y;
	    
		mat3 cam = setCamera(cameraOrigin, cameraTarget, TIME*0.5);
	    
	    vec3 rayDir = cam* normalize( vec3(screenPos.xy,0.25) );
	    vec3 effect = rayCast(rayDir, cameraOrigin);
	    
	    vec3 sky = skyBox(rayDir);
	    vec3 res = sky + effect;
	
		gl_FragColor = vec4(res, 1.0);
	}
	else if (PASSINDEX == 1)	{


	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy - vec2(0.5);
		
	    vec4 col = vec4(0.0);
	    for(int i = 0; i < 4; i++)
	    {
	        
	        col += IMG_NORM_PIXEL(BufferA,mod(uv/(1.0 + float(i)*sampleDistance) + vec2(0.5),1.0));
	    }
	    
	    col /= 4.0;
	    
	    gl_FragColor = col;
	}
	else if (PASSINDEX == 2)	{


	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy - vec2(0.5);
		
	    vec4 col = vec4(0.0);
	    for(int i = 0; i < 4; i++)
	    {
	        
	        col += IMG_NORM_PIXEL(BufferB,mod(uv/(1.0 + float(i)*sampleDistance) + vec2(0.5),1.0));
	    }
	    
	    col /= 4.0;
	    
	    vec4 oldFrame = IMG_NORM_PIXEL(BufferC,mod(gl_FragCoord.xy/RENDERSIZE.xy,1.0));
	    
	    gl_FragColor = mix(col, oldFrame, 0.75);
	}
	else if (PASSINDEX == 3)	{
   

	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
		vec4 col = IMG_NORM_PIXEL(BufferC,mod(uv,1.0));
	    
	    gl_FragColor = col;
	}
}

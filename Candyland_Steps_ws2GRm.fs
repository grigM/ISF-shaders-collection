/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ws2GRm by Elyxian.  Raymarching a stepped noise function. Because the heightmap has infinitely steep sections, a maximum step size is enforced on the raymarching, with some interval bisection stuff to prevent overstepping. This gives pretty good (but imperfect) results.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


#define STEPHEIGHT 1.0
#define HSCALE 0.5
#define VSCALE 8.0

//#define STEPHEIGHT 0.2
//#define HSCALE 0.3
//#define VSCALE 2.0

float saturate(float t) {return clamp(t, 0.0, 1.0);}
mat2 mm2(in float a){float c = cos(a), s = sin(a);return mat2(c,s,-s,c);}

// RGB and HSV Conversion from https://stackoverflow.com/questions/15095909/from-rgb-to-hsv-in-opengl-glsl

vec3 increaseSaturation(vec3 c) {
	
    float strength = 1.15;
    
    // Convert to hsv
    vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
    vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    vec3 hsv = vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
    
    // Increase saturation
    hsv.y *= strength;
    saturate(hsv.y);
    
    // Convert to rgb
    vec4 K2 = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p2 = abs(fract(hsv.xxx + K2.xyz) * 6.0 - K2.www);
    vec3 rgb = hsv.z * mix(K2.xxx, clamp(p2 - K2.xxx, 0.0, 1.0), hsv.y);
    
    return rgb;

}

// Hash function by Dave Hoskins: https://www.shadertoy.com/view/4djSRW

float hash12(vec2 p) {
    
	vec3 p3  = fract(vec3(p.xyx) * 0.1031);
    p3 += dot(p3, p3.yzx + 19.19);
    return fract((p3.x + p3.y) * p3.z);
    
}

// Standard value noise function

float valueNoise(vec2 p) {
    
	vec2 i = floor(p);
    vec2 f = fract(p);
    
    f = f*f*f*(f*(f*6.0-15.0)+10.0);
    
    vec2 add = vec2(1.0,0.0);
    float res = mix(
        mix(hash12(i), hash12(i + add.xy), f.x),
        mix(hash12(i + add.yx), hash12(i + add.xx), f.x),
        f.y);
    return res;
        
}

// The heightmap is 2d value noise but filtered into 'steps'

float heightMap(vec2 p) {
	
    float height = valueNoise(p * HSCALE) * VSCALE;
    height = floor(height / STEPHEIGHT) * STEPHEIGHT;
    return height;
    
}

float raytrace(vec3 ro, vec3 rd) {
	
    // Parameters
    int maxSteps = 500;
    float maxStepDist = 0.12;
    float maxDist = 100.0;
    int maxIterations = 10;
    float eps = 0.01;
    
    // Initial Raymarching Steps
    bool didHit = false;
    float beforeDist = -1.0;
    float afterDist = 0.0;
    float t = 0.0;
    for (int i = 0; i < maxSteps && t < maxDist; i++) {
        
        beforeDist = afterDist;
        afterDist = t;
    	vec3 p = ro + t * rd;
        float height = heightMap(p.xz);
        if (p.y - height < eps) {
			didHit = true;
            break;
        }
        
        // Normally raymarching would just be done using 'p.y - height', however this does not
        // take into account the proximity to the nearest cliff. To account for this, a maximum
        // step size is imposed. Additionally, if the ray is currently above the maximum height
        // of the heightmap, it can safely move forward by the distance to the top of the heightmap
        //t += min(p.y - height, maxStepDist);
        t += max(min(p.y - height, maxStepDist), p.y - VSCALE);
        
    }
    if (!didHit) {
    	return -1.0;
    }
    
    // Use the interval bisection method to find a closer point, as moving forward by a fixed step
    // size may have embedded the ray in a cliff
    for (int i = 0; i < maxIterations; i++) {
    	float midVal = (beforeDist + afterDist) / 2.0;
        vec3 p = ro + midVal * rd;
        if (p.y < heightMap(p.xz)) {
        	afterDist = midVal;
        }
        else {
        	beforeDist = midVal;
        }
    }
    
    // Return the midpoint of the closest point before the terrain, and the closest point after it
    return (beforeDist + afterDist) / 2.0;
    

}

/*vec3 getNormal(vec3 p) {
    
    // Central differences method to generate a normal vector
    // This is imperfect for this non-continuous heightmap, and I believe
    // this is the cause of the bad normal lighting
    vec2 eps = vec2(0.005, 0.00);
    vec3 normal = vec3(
    	heightMap(p.xz + eps.xy) - heightMap(p.xz - eps.xy),
		2.0 * eps.x,
		heightMap(p.xz + eps.yx) - heightMap(p.xz - eps.yx)
    );
    normal = normalize(normal);
    return normal;
    
}*/

vec3 getNormal(vec3 p) {
    
    // This is a somewhat hacky way of getting the normal for this heightmap
    // The above getNormal function is imperfect due to the non-continous heightmap
    
    // Get the normal of the value noise (not the stepped heightmap)
    
    vec2 eps = vec2(0.005, 0.00);
    vec3 normal = vec3(
    	(valueNoise((p.xz + eps.xy) * HSCALE) * VSCALE) - (valueNoise((p.xz - eps.xy) * HSCALE) * VSCALE),
		2.0 * eps.x,
		(valueNoise((p.xz + eps.yx) * HSCALE) * VSCALE) - (valueNoise((p.xz - eps.yx) * HSCALE) * VSCALE)
    );
    
    // Work out whether the point is in a horizontal or vertical section
    float stepEps = 0.012;
    float stepSection = mod(p.y / STEPHEIGHT, 1.0);
    bool isOnCliff = true;
    if (stepSection < stepEps || stepSection > (1.0 - stepEps)) {
    	isOnCliff = false;
    }
    
    // Adjust the normal accordingly
    if (isOnCliff) {
    	normal = vec3(normal.x, 0.0, normal.z);
    }
    else {
    	normal = vec3(0.0, 1.0, 0.0);
    }
    
    // Normalise and return the normal
    normal = normalize(normal);
    return normal;
    
}

vec3 getDiffuse(vec3 p) {
    
    // Get which band the point belongs to
    float eps = 0.01;
    p.y += eps;
    float height = floor(p.y / STEPHEIGHT) * STEPHEIGHT;
    
    // Color function from iq: https://www.shadertoy.com/view/Xds3zN
    vec3 col = 0.45 + 0.35*sin(vec3(0.15,0.08,0.10)*(height*124.2));
    
    // Increase saturation
    col = increaseSaturation(col);
    
    // Darken the lower colors
    col = mix(vec3(0.0), col, 0.1 + 0.9 * (height / VSCALE));
    
    return col;
    
}

vec3 getColor(vec3 p) {
	
    // Directional light source
    vec3 lightDir = normalize(vec3(0.8, 1.0, -0.8));
    
    // The intensity/color of light (all three values are the same for white light)
    vec3 lightCol = vec3(1.0);
    
    // Applies the 'base color' of the light
    vec3 baseLightCol = vec3(1.0, 1.0, 1.0);
    lightCol *= baseLightCol;
    
    // Applies normal-based lighting
    vec3 normal = getNormal(p);
    float normalLight = max(0.05, saturate(dot(normal, lightDir)));
    //float normalLight = max(0.1, step(0.5, dot(normal, lightDir)));
    lightCol *= normalLight;
    
    // Gets the diffuse lighting
    vec3 diffuse = getDiffuse(p);//vec3(0.368, 0.372, 0.901);
    
    // Get the final color
    vec3 col = lightCol * diffuse;
    return col;
    
}

vec3 render(vec3 ro, vec3 rd) {
	
    vec3 skyCol = vec3(0.9, 0.9, 0.9);
    float fogFalloff = 0.1;
    float fogDist = 90.0;
    
    float t = raytrace(ro, rd);
    
    vec3 col = vec3(0.0);
    
    // If the terrain is hit
    if (t >= 0.0) {
    	
        // Get the color of the terrain
        col = getColor(ro + t * rd);
        
        // Add the fog (better fog functions exist though)
        float fogAmount = exp(fogFalloff * (t - fogDist));
        col = mix(col, skyCol, fogAmount);
        
    }
    
    // If the terrain is not hit
    else {
        col = skyCol;
    }
    
    return col;

}


void main() {

    
    // Normalises the fragCoord
    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    vec2 p = uv - 0.5;
    p.x *= RENDERSIZE.x/RENDERSIZE.y;
    
    // Gets the direction of the ray and the origin
    vec3 ro = vec3(0.0, VSCALE + 2.0, 0.0) + vec3(1.0, 0.0, 1.0) * TIME * 0.5;
    vec3 rd = normalize(vec3(p, 1.4));
    // Rotates the ray depending on the mouse position. I lifted this from
    // https://www.shadertoy.com/view/XtGGRt, but it seems to be the common approach
    vec2 mo = iMouse.xy / RENDERSIZE.xy-.5;
    mo = (mo==vec2(-.5))?mo=vec2(0.16, -0.1):mo; // Default position of camera
    mo.x *= RENDERSIZE.x/RENDERSIZE.y;
    mo *= 3.0;
    rd.yz *= mm2(mo.y);
    rd.xz *= mm2(mo.x);
    
    // Render and output the ray to screen
    vec3 col = render(ro, rd);
    float gamma = 2.2;
    col = pow(col, vec3(1.0 / gamma));
    gl_FragColor = vec4(col,1.0);
    
}

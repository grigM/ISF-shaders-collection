/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "procedural",
    "3d",
    "grid",
    "voxel",
    "marching",
    "reusable",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tlfDn by mhnewman.  Simple to reuse, fast voxel engine. Create your scene by filling in setCamera(), voxelHit(), and voxelColor().",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


// Simple to reuse, fast voxel engine.
// Create your scene by filling in setCamera(), voxelHit(), and voxelColor().
// The engine uses a Z-up right-handed coordinate system.
// The following constants should also be set.
const vec3 backgroundColor = vec3(0.6, 0.7, 0.8);
const vec3 lightDir = vec3(0.36, 0.48, 0.8);
const float shadow = 0.7;
const int maxIter = 100;

#define CAST_SHADOW

// Set Camera will position and aim the camera.
//   eye := The location of the camera.
//   center := The location at which the camera is looking.
//   Return value := Camera focal length.
float setCamera(out vec3 eye, out vec3 center) {
    
    // Fill in this function.
    vec2 m = vec2(0.1 * TIME, 0.7);
    if (iMouse.x > 0.0)
        m = iMouse.xy / RENDERSIZE.xy;
    m *= 6.283185 * vec2(1.0, 0.25);    
    float dist = 20.0;
    center = vec3(0.5);
    eye = center + vec3(dist * sin(m.x) * sin(m.y), dist * cos(m.x) * sin(m.y), dist * cos(m.y));
    return 3.0;

}

// Voxel Hit returns true if the voxel at pos should be filled.
bool voxelHit(vec3 pos) {
    
    // Fill in this function.
    vec3 hash = fract(pos * vec3(5.3983, 5.4427, 6.9371));
    hash += dot(hash, hash.yzx + 19.19);
    return length(pos) + 2.0 * fract((hash.x + hash.y) * hash.z) < 6.0 + 0.5 * sin(3.0 * TIME);

}

// Voxel Color returns the color at pos with normal vector norm.
vec3 voxelColor(vec3 pos, vec3 norm) {
    
    // Fill in this function.
    return mix(vec3(0.3, 0.5, 0.8), vec3(1.0, 0.4, 0.0), 0.5 * (length(floor(pos)) - 4.0));

}

////////////////////////////////////////////////////////////////////////////////
// Fill in the functions above.
// The engine below does not need to be modified.
////////////////////////////////////////////////////////////////////////////////

float castRay(vec3 eye, vec3 ray, out float dist, out vec3 norm) {
    vec3 pos = floor(eye);
    vec3 ri = 1.0 / ray;
    vec3 rs = sign(ray);
    vec3 ris = ri * rs;
    vec3 dis = (pos - eye + 0.5 + rs * 0.5) * ri;
    
    vec3 dim = vec3(0.0);
    for (int i = 0; i < maxIter; ++i) {
        if (voxelHit(pos)) {
            dist = dot(dis - ris, dim);
            norm = -dim * rs;
            return 1.0;
        }
    
        dim = step(dis, dis.yzx);
		dim *= (1.0 - dim.zxy);
        
        dis += dim * ris;
        pos += dim * rs;
    }

	return 0.0;
}

void main() {

    vec3 eye, center;
    float zoom = setCamera(eye, center);
    
    vec3 forward = normalize(center - eye);
    vec3 right = normalize(cross(forward, vec3(0.0, 0.0, 1.0)));
    vec3 up = cross(right, forward);
    vec2 xy = 2.0 * gl_FragCoord.xy - RENDERSIZE.xy;
    vec3 ray = normalize(xy.x * right + xy.y * up + zoom * forward * RENDERSIZE.y);
    
    float dist;
    vec3 norm;
    float hit = castRay(eye, ray, dist, norm);
    vec3 pos = eye + dist * ray;
    vec3 color = voxelColor(pos - 0.001 * norm, norm);
    float shade = dot(norm, lightDir);
    
#ifdef CAST_SHADOW
    float illuminated = 1.0 - castRay(pos + 0.001 * norm, lightDir, dist, norm);
    float light = (1.0 + shadow * (illuminated * max(shade, 0.0) - 1.0)) * (1.0 - max(-shade, 0.0));
#else
    float light = (1.0 + shadow * (max(shade, 0.0) - 1.0)) * (1.0 - max(-shade, 0.0));    
#endif
    
    gl_FragColor = vec4(mix(backgroundColor, light * color, hit), 1.0);
}

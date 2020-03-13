/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59198.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/Wtd3zn
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE

// Emulate some GLSL ES 3.x
mat4 mat4x3(vec3 a, vec3 b, vec3 c, vec3 d)
{
    return mat4(a.x, a.y, a.z, 0,
                b.x, b.y, b.z, 0,
                c.x, c.y, c.z, 0,
                d.x, d.y, d.z, 0);
}

// --------[ Original ShaderToy begins here ]---------- //
//////////////////// Re-oriented 8-point BCC noise by KdotJPG ////////////////////
//////////////////// Output: vec4(dF/dx, dF/dy, dF/dz, value) ////////////////////

// Borrowed from Stefan Gustavson's noise code
vec4 permute(vec4 t) {
    return t * (t * 34.0 + 133.0);
}

// Gradient set is a normalized expanded rhombic dodecahedron
vec3 grad(float hash) {
    
    // Random vertex of a cube, +/- 1 each
    vec3 cube = mod(floor(hash / vec3(1.0, 2.0, 4.0)), 2.0) * 2.0 - 1.0;
    
    // Random edge of the three edges connected to that vertex
    // Also a cuboctahedral vertex
    // And corresponds to the face of its dual, the rhombic dodecahedron
    vec3 cuboct = cube;
    float v = hash / 16.0;
    if (v < 1.0)
        cuboct.x = 0.0;
    else if (v < 2.0)
        cuboct.y = 0.0;
    else
        cuboct.z = 0.0;
    
    // In a funky way, pick one of the four points on the rhombic face
    float type = mod(floor(hash / 8.0), 2.0);
    vec3 rhomb = (1.0 - type) * cube + type * (cuboct + cross(cube, cuboct));
    
    // Expand it so that the new edges are the same length
    // as the existing ones
    vec3 grad = cuboct * 1.22474487139 + rhomb;
    
    // To make all gradients the same length, we only need to shorten the
    // second type of vector. We also put in the whole noise scale constant.
    // The compiler should reduce it into the existing floats. I think.
    grad *= (1.0 - 0.08404082057734576 * type) * 3.4105641006663014;
    
    return grad;
}

// BCC lattice split up into 2 cube lattices
vec4 bccNoiseDerivativesPart(vec3 X) {
    vec3 b = floor(X);
    vec4 i4 = vec4(X - b, 2.5);
    
    // Pick between each pair of oppposite corners in the cube.
    vec3 v1 = b + floor(dot(i4, vec4(.25)));
    vec3 v2 = b + vec3(1, 0, 0) + vec3(-1, 1, 1) * floor(dot(i4, vec4(-.25, .25, .25, .35)));
    vec3 v3 = b + vec3(0, 1, 0) + vec3(1, -1, 1) * floor(dot(i4, vec4(.25, -.25, .25, .35)));
    vec3 v4 = b + vec3(0, 0, 1) + vec3(1, 1, -1) * floor(dot(i4, vec4(.25, .25, -.25, .35)));
    
    // Gradient hashes for the four vertices in this half-lattice.
    vec4 hashes = permute(mod(vec4(v1.x, v2.x, v3.x, v4.x), 289.0));
    hashes = permute(mod(hashes + vec4(v1.y, v2.y, v3.y, v4.y), 289.0));
    hashes = mod(permute(mod(hashes + vec4(v1.z, v2.z, v3.z, v4.z), 289.0)), 48.0);
    
    // Gradient extrapolations & kernel function
    vec3 d1 = X - v1; vec3 d2 = X - v2; vec3 d3 = X - v3; vec3 d4 = X - v4;
    vec4 a = max(0.75 - vec4(dot(d1, d1), dot(d2, d2), dot(d3, d3), dot(d4, d4)), 0.0);
    vec4 aa = a * a; vec4 aaaa = aa * aa;
    vec3 g1 = grad(hashes.x); vec3 g2 = grad(hashes.y);
    vec3 g3 = grad(hashes.z); vec3 g4 = grad(hashes.w);
    vec4 extrapolations = vec4(dot(d1, g1), dot(d2, g2), dot(d3, g3), dot(d4, g4));
    
    // Derivatives of the noise
    vec4 derivative = -8.0 * mat4x3(d1, d2, d3, d4) * (aa * a * extrapolations)
        + mat4x3(g1, g2, g3, g4) * aaaa;
    
    // Return it all as a vec4
    return vec4(derivative.xyz, dot(aaaa, extrapolations));
}

// Rotates domain, but preserve shape. Hides grid better in cardinal slices.
// Good for texturing 3D objects with lots of flat parts along cardinal planes.
vec4 bccNoiseDerivatives_XYZ(vec3 X) {
    X = dot(X, vec3(2.0/3.0)) - X;
    
    vec4 result = bccNoiseDerivativesPart(X) + bccNoiseDerivativesPart(X + 144.5);
    
    return vec4(dot(result.xyz, vec3(2.0/3.0)) - result.xyz, result.w);
}

// Gives X and Y a triangular alignment, and lets Z move up the main diagonal.
// Might be good for terrain, or a TIME varying X/Y plane. Z repeats.
vec4 bccNoiseDerivatives_PlaneFirst(vec3 X) {
    
    // Not a skew transform.
    mat3 orthonormalMap = mat3(
        0.788675134594813, -0.211324865405187, -0.577350269189626,
        -0.211324865405187, 0.788675134594813, -0.577350269189626,
        0.577350269189626, 0.577350269189626, 0.577350269189626);
    
    X = orthonormalMap * X;
    vec4 result = bccNoiseDerivativesPart(X) + bccNoiseDerivativesPart(X + 144.5);
    
    return vec4(result.xyz * orthonormalMap, result.w);
}

//////////////////////////////// End noise code ////////////////////////////////


void mainImage(out vec4 fragColor, in vec2 fragCoord) {
    
    // Normalized pixel coordinates (from 0 to 1 on largest axis)
    vec2 uv = fragCoord / max(iResolution.x, iResolution.y) * 8.0;
    
    // Initial input point
    vec3 X = vec3(uv, mod(iTime, 578.0) * 0.8660254037844386);
    
    // 8 steps of gradient ascent
    vec4 noiseResult;
    for (int i = 0; i < 8; i++) {
    	noiseResult = bccNoiseDerivatives_PlaneFirst(X);
        X += noiseResult.xyz / 16.0;
    }
    
    //Get the final value
    float value = noiseResult.w;

    // Time varying pixel color
    vec3 col = vec3(value);

    // Output to screen
    fragColor = vec4(col, 1.0);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
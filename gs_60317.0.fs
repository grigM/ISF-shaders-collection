/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60317.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/3lGGR1
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE

// --------[ Original ShaderToy begins here ]---------- //
// from https://shadertoyunofficial.wordpress.com/2019/01/02/programming-tricks-in-shadertoy-glsl/
#define hash21(p) fract(sin(dot(p, vec2(12.9898, 78.233))) * 43758.5453)

// from https://www.shadertoy.com/view/Xt2BDc
#define hash31(p) fract(sin(dot(p, vec3(17, 1527, 113))) * 43758.5453123)

#define blend(dest, source) dest = mix(dest, vec4(source.rgb, 1.0), source.a);
#define add(dest, source) dest += source * source.a;

const float glow_level = 0.7;
const float glow_attenuation = 6.0;
const float zoom = 2.5;
const float tile_radius = 0.45;
const float frame_level = 0.05;
const float lit_tile_threshold = 0.75;
const float unlit_tile_level = 0.15;

// from https://www.shadertoy.com/view/MsS3Wc
vec3 hsv2rgb(in vec3 c)
{
    vec3 rgb = clamp(abs(mod(c.x * 6.0 + vec3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
    return c.z * mix(vec3(1.0), rgb, c.y);
}

// from http://www.iquilezles.org/www/articles/distfunctions2d/distfunctions2d.htm
float sdBox(in vec2 p, in vec2 b)
{
    vec2 d = abs(p) - b;
    return length(max(d, vec2(0.0))) + min(max(d.x, d.y), 0.0);
}

void get_tile_colour(in vec2 tile_id, out vec3 tile_hsv)
{
    // hue
    tile_hsv.x = hash21(tile_id);
    // saturation
    tile_hsv.y = 1.0;
    // value
    float modTime = mod(iTime, 100.0);
    float TIME = floor(modTime * 2.0);
    float level = step(lit_tile_threshold, hash31(vec3(tile_id, TIME)));
    float lastLevel = step(lit_tile_threshold, hash31(vec3(tile_id, TIME - 1.0)));
    level = max(level, lastLevel * (1.0 - fract(modTime * 2.0) * 1.5));
    tile_hsv.z = level;
}

#define ADD_GLOW(tile) get_tile_colour(tile_id + tile, tile_hsv); add(fragColor, vec4(hsv2rgb(tile_hsv), pow(1.0 - sdBox(tile - tile_coord, vec2(tile_radius)), glow_attenuation) * glow_level * tile_hsv.z));

void mainImage(out vec4 fragColor, in vec2 fragCoord)
{
    // normalise the coordinates
    vec2 R = iResolution.xy, U = ((2. * fragCoord.xy) - R) / min(R.x, R.y) * zoom, FU = fract(U);

    // unique ID for the tile
    vec2 tile_id = floor(U);
    // local tile coords [-0.5, 0.5]
    vec2 tile_coord = FU - 0.5;
    // distance from edge of light
    float tile_dist = sdBox(tile_coord, vec2(tile_radius));

    // render the frame
    fragColor = vec4(vec3(frame_level), 1.0);

    // get tile's colour
    vec3 tile_hsv;
    get_tile_colour(tile_id, tile_hsv);

    // calculate a vignette to apply to the saturation (from https://www.shadertoy.com/view/lsKSWR)
    vec2 vignette_coord = FU * (1.0 - FU.yx);
    float vignette = pow(vignette_coord.x * vignette_coord.y * 5.0, 0.5);

    // render the tile
    vec3 light_colour = hsv2rgb(vec3(tile_hsv.x, 1.0 - (tile_hsv.z * vignette), max(tile_hsv.z, unlit_tile_level)));
    blend(fragColor, vec4(light_colour, (1.0 - step(0.0, tile_dist))));
    // render the tile's own glow on the frame
    add(fragColor, vec4(hsv2rgb(tile_hsv), pow(1.0 - max(tile_dist, 0.0), glow_attenuation) * glow_level * tile_hsv.z));

    // get vector to the three nearest neighbours (round tile_coord away from 0)
    vec2 neighbours = step(vec2(0.0), tile_coord) * 2.0 - 1.0;
    // render the neighbours' glows
    ADD_GLOW(neighbours)
    ADD_GLOW(vec2(neighbours.x, 0.0))
    ADD_GLOW(vec2(0.0, neighbours.y))
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
    gl_FragColor.a = 1.0;
}
/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "cloud",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4dXczH by mende.  clouds",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


float hash( float n )
{
    return fract(tan(n)*43758.5453);
}

float getNoise( vec3 x )
{
    x *= 50.0;
    // The noise function returns a value in the range -1.0f -> 1.0f

    vec3 p = floor(x);
    vec3 f = fract(x);

    f       = f*f*(3.0-2.0*f);
    float n = p.x + p.y*57.0 + 113.0*p.z;

    return mix(mix(mix( hash(n+0.0), hash(n+1.0),f.x),
                   mix( hash(n+57.0), hash(n+58.0),f.x),f.y),
               mix(mix( hash(n+113.0), hash(n+114.0),f.x),
                   mix( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
}

float getLayeredNoise(vec3 seed)
{
	return (0.5 * getNoise(seed * 0.05)) +
           (0.25 * getNoise(seed * 0.1)) +
           (0.125 * getNoise(seed * 0.2)) +
           (0.0625 * getNoise(seed * 0.4));
}
float _smooth(float inVal)
{
    return inVal * inVal * (3.0 - (2.0 * inVal));
}


void main() {

    vec2 size = vec2(35.0, 20.0);
    vec2 coord = gl_FragCoord.xy / RENDERSIZE.xy * size.xy;
    vec2 wind = iMouse.xy / RENDERSIZE.xy - 0.5;
	vec3 uv3 = vec3(floor(coord.xy) / size.xy + wind * TIME * 0.26, TIME * 0.05);
    uv3.xy *= 2.0;
 	float pixel = (_smooth(_smooth(getLayeredNoise(uv3) + 0.2)) - 0.88) * 12.0;
    vec2 loc = abs(fract(coord) - 0.5) * 2.0;
    float value = (loc.x < pixel && loc.y < pixel) ? 1.0 : 0.0;
	gl_FragColor = vec4(vec3(value), 1.0);
}

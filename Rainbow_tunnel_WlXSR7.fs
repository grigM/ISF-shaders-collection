/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WlXSR7 by krwq.  tunnel",
  "INPUTS" : [

  ]
}
*/


#define DARKNESS 0.8
#define DEPTH 0.5

#define pi 3.14159265359

// perhaps will do butterfly spin-off one day
//#define BONUS

// ----------------------------------------------------------------

// hash, noise, hsv2rgb_smooth borrowed from iq (MIT)
float hash(vec3 p)
{
    p  = fract( p*0.3183099+.1 );
               p *= 17.0;
    return fract( p.x*p.y*p.z*(p.x+p.y+p.z) );
}

float noise( in vec3 x )
{
    vec3 p = floor(x);
    vec3 f = fract(x);
    f = f*f*(3.0-2.0*f);
               
    return mix(mix(mix( hash(p+vec3(0,0,0)), 
                        hash(p+vec3(1,0,0)),f.x),
                  mix( hash(p+vec3(0,1,0)), 
                        hash(p+vec3(1,1,0)),f.x),f.y),
               mix(mix( hash(p+vec3(0,0,1)), 
                        hash(p+vec3(1,0,1)),f.x),
                   mix( hash(p+vec3(0,1,1)), 
                        hash(p+vec3(1,1,1)),f.x),f.y),f.z);
}

vec3 hsv2rgb_smooth( in vec3 c )
{
    vec3 rgb = clamp( abs(mod(c.x*6.0+vec3(0.0,4.0,2.0),6.0)-3.0)-1.0, 0.0, 1.0 );

    rgb = rgb*rgb*(3.0-2.0*rgb); // cubic smoothing 

    return c.z * mix( vec3(1.0), rgb, c.y);
}

vec3 colorful_pattern(vec2 p)
{
    float shift = TIME;
    p.x += shift;
    float lightInvScale = .8;
    float light = shift + 2. * (1.0 + sin(TIME * 2. * pi / 10.));
    float n = (0.6 * noise(vec3(p * 23.0, 0.0)) + 0.4 * noise(vec3(p * 3.0, 0.0)));
    float lightInfluence = smoothstep(0.0, 1.0, abs(light - p.x) * lightInvScale);
    vec3 rainbowPattern = hsv2rgb_smooth(vec3(sin(p.x + p.y * 0.1 + sin(2. * p.y + p.x)), 1.0, 1.0));
    vec3 lightColor = hsv2rgb_smooth(vec3(0.5 * (1. + sin(TIME * 2. * pi / 111.)), 1.0, 1.0));
    vec3 rainbowWithLight = mix(lightColor, rainbowPattern, lightInfluence);
    vec3 patternSecondColor = vec3(0.2 * (1. - lightInfluence));
    return clamp(mix(rainbowWithLight, patternSecondColor, n), 0.0, 1.0);
}

vec3 tex(vec2 p)
{
    return colorful_pattern(p);
}

mat2 rot2d(float angle)
{
    float s = sin(angle);
    float c = cos(angle);
    return mat2(c, -s, s, c);
}

vec3 pixel(vec2 uv)
{
    float angle = 2. * pi * TIME / 40.0;
    uv = rot2d(angle) * uv;
    
    // this is rather a hack to make equations a bit simpler
    // because of that we can assume wall is in x = 0
    uv += vec2(0.5, 0.);
    float eyeX = 0.6 + 0.2 * sin(2. * pi * TIME / 37.);
    vec3 eye = vec3(eyeX, 0.0, -DEPTH);
    
    vec2 p = uv;
    // x-mir: (0-.5, 0-1)
    p = vec2(0.5 - abs(p.x - 0.5), p.y);
    // WALL = EYE + (P - EYE) * c
    // now, we need to find c
       
    vec3 p3 = vec3(p, 0.0);
    vec3 peye = p3 - eye;
    float c = - eye.x / peye.x;
    
    // put everything in the equation
    vec3 wall = peye + c * peye;
    
    // map wall coords into tex coords
    //   both y's align
    //   z aligns with tex's x
    //   x is always 0 because wall is flat so it needs to be 0 somewhere
    //     we could have chosen something more complex where wall goes diagonal but why complicate life
    p = vec2(wall.z, wall.y);
    
  	float bonus = 0.0;
#ifdef BONUS
    bonus = sin(2. * pi * TIME);
#endif
    p *= 0.6 + 0.3 * bonus;

    return tex(p) *  pow(abs(0.5 - uv.x) / 0.5, DARKNESS);
}

// ----------------------------------------------------------------

void main() {



    vec2 uv = (gl_FragCoord.xy - RENDERSIZE.xy / 2.0)/RENDERSIZE.y;
    
    vec3 col = pixel(uv);
    
    gl_FragColor = vec4(col, 1.0);
}

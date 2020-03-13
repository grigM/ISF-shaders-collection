/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/WdtGWf by PixelPhil.  A design inspired by scales patterns found on traditional japanese fabric and indian mandala art.\nIt also features motion blur. Best viewed in full screen.\nTry staring at it for a minute ;)",
    "IMPORTED": {
    },
    "INPUTS": [
        {
            "NAME": "iChannel0",
            "TYPE": "audio"
        }
    ]
}

*/


//
// Seigaiha Mandala by Philippe Desgranges
// Email: Philippe.desgranges@gmail.com
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
//

#define S(a,b,c) smoothstep(a,b,c)


// blends a pre-multiplied src onto a dst color (without alpha)
vec3 premulMix(vec4 src, vec3 dst)
{
    return dst.rgb * (1.0 - src.a) + src.rgb;
}

// blends a pre-multiplied src onto a dst color (with alpha)
vec4 premulMix(vec4 src, vec4 dst)
{
    vec4 res;
    res.rgb = premulMix(src, dst.rgb);
    res.a = 1.0 - (1.0 - src.a) * (1.0 - dst.a);
    return res;
}

// compute the round scale pattern and its mask
// output rgb is premultiplied by alpha
vec4 roundPattern(vec2 uv)
{
    float dist = length(uv);
    
    // Resolution dependant Anti-Aliasing for a prettier thumbnail
    // Thanks Fabrice Neyret & dracusa for pointing this out.
    float aa = 8. / RENDERSIZE.x;

    // concentric circles are made by thresholding a triangle wave function
    float triangle = abs(fract(dist * 11.0 + 0.3) - 0.5);
    float circles = S(0.25 - aa * 10.0, 0.25 + aa * 10.0, triangle);

    // a light gradient is applied to the rings
    float grad = dist * 2.0;
    vec3 col = mix(vec3(0.0, 0.5, 0.6),  vec3(0.0, 0.2, 0.5), grad * grad);
    col = mix(col, vec3(1.0), circles);
    
    // border and center are red
    vec3 borderColor = vec3(0.7, 0.2, 0.2);
    col = mix(col, borderColor, S(0.44 - aa, 0.44 + aa, dist));
    col = mix(col, borderColor, S(0.05 + aa, 0.05 - aa, dist));
    
    // computes the mask with a soft shadow
    float mask = S(0.5, 0.49, dist);
    float blur = 0.3;
    float shadow = S(0.5 + blur, 0.5 - blur, dist);
   
    return vec4(col * mask, clamp(mask + shadow * 0.55, 0.0, 1.0)); 
}


//computes the scales on a ring of a given radius with a given number of scales
vec4 ring(vec2 uv, float angle, float angleOffet, float centerDist, float numcircles, float circlesRad)
{
    // polar space is cut in quadrants (one per scale)
    float quadId = floor(angle * numcircles + angleOffet);
    
    // computes the angle of the center of the quadrant
    float quadAngle = (quadId + 0.5 - angleOffet) * (6.283 / numcircles);
    
    // computes the center point of the quadrant on the circle
    vec2 quadCenter = vec2(cos(quadAngle), sin(quadAngle)) * centerDist;
    
    // return to color of the scale in the quadrant
    vec2 circleUv = (uv + quadCenter) / circlesRad;
    return roundPattern(circleUv);
}

// computes a ring with two layers of overlapping patterns
vec4 dblRing(vec2 uv, float angle, float centerDist, float numcircles, float circlesRad, float t)
{
    // Odd and even scales dance up and down
    float s = sin(t * 3.0 + centerDist * 10.0) * 0.05;
    float d1 = 1.05 + s;
    float d2 = 1.05 - s;
    
    // the whole thing spins with a sine perturbation
    float rot = t * centerDist * 0.4 + sin(t + centerDist * 5.0) * 0.2;
    
    // compute bith rings
    vec4 ring1 = ring(uv, angle, 0.0 + rot, centerDist * d1, numcircles, circlesRad);
    vec4 ring2 = ring(uv, angle, 0.5 + rot, centerDist * d2, numcircles, circlesRad);
    
    // blend the results
    vec4 col = premulMix(ring1, ring2);
    
    // add a bit of distance shading for extra depth
    col.rgb *= 1.0 - (centerDist * centerDist) * 0.4;
    
    return col;
}

// computes a double ring on a given radius with a number of scales to fill the circle evenly
vec4 autoRing(vec2 uv, float angle, float centerDist, float t)
{
    float nbCircles = 1.0 + floor(centerDist * 23.0);
    return dblRing(uv, angle, centerDist, nbCircles, 0.23, t);
}

// Computes the pixel color for the full image at a givent time
vec3 fullImage(vec2 uv, float angle, float centerDist, float t)
{
    vec3 col;
    
    // the screen is cut in concentric rings
    float space = 0.1;
    
    // determine in which ring the pixel is
    float ringRad = floor(centerDist / space) * space;
    
	// computes the scales in the previous, current and next ring
	vec4 ringCol1 = autoRing(uv, angle, ringRad - space, t);
 	vec4 ringCol2 = autoRing(uv, angle, ringRad, t);
    vec4 ringCol3 = autoRing(uv, angle, ringRad + space, t);
    
    // blends everything together except in the center
    if (ringRad > 0.0)
    {
        col.rgb = ringCol3.rgb;
        col.rgb = premulMix(ringCol2, col.rgb);
        col.rgb = premulMix(ringCol1, col.rgb);
    }
	else
    {
        col.rgb = ringCol2.rgb; 
    }

    return col;
}

// A noise function that I tried to make as gaussian-looking as possible
float noise21(vec2 uv)
{
    vec2 n = fract(uv* vec2(19.48, 139.9));
    n += sin(dot(uv, uv + 30.7)) * 47.0;
    return fract(n.x * n.y);
}


void main() {



    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = (gl_FragCoord.xy - .5 * RENDERSIZE.xy) / RENDERSIZE.y; 
    
    uv *= 0.9;
    
    // Computes polar cordinates
    float angle = atan(uv.y, uv.x) / 6.283 + 0.5;
    float centerDist = length(uv);
    
    vec3 col = vec3(0.0);
    
	// average 4 samples at slightly different times for motion blur
    float noise = noise21(uv + TIME);
    for (float i = 0.0; i < 4.0; i++)
    {
        col += fullImage(uv, angle, centerDist, TIME - ((i + noise) * 0.03));
    }
    col /= 4.0;
 
    // Output to screen
    gl_FragColor = vec4(col,1.0);
}

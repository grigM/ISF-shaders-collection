/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3tXSz7 by Flopine.  This is a shader I made during a live session on Twitch, about teaching people the basics of shadercoding. You can watch them here: https:\/\/www.twitch.tv\/flopine",
  "INPUTS" : [

  ]
}
*/


// Code by Flopine
// Thanks to wsmind, leon, XT95, lsdlive, lamogui, Coyhot, Alkama and YX for teaching me
// Thanks LJ for giving me the love of shadercoding :3

// Thanks to the Cookie Collective, which build a cozy and safe environment for me 
// and other to sprout :)  https://twitter.com/CookieDemoparty


// This was a Twitch session where I gave people a course about the basics
// of shadercoding.
// Sorry Fabrice Neyret for not using smoothstep function for anti-aliasing ^^"

#define fGlobalTime TIME

// rotation matrice function
mat2 rot (float a)
{return mat2 (cos(a), sin(a), -sin(a),cos(a));}

// split the screen in two in the x axis
float banding (vec2 uv)
{
    // 3 ways of doing it:
    // 1.
    //if(uv.x < 0.) return 0.;
    //else return 1.;
	// 2.
    //return (uv.x<0.) ? 0.: 1.;
	// 3.
    return smoothstep(0., 0.1,uv.x);

}

float triangle (vec2 uv, float size)
{
    // symmetry in the y axis
    uv.x = abs(uv.x);
    return step(max(-uv.y,dot(uv,vec2(1.,0.6))), size);
}

float square (vec2 uv, float size)
{
    uv = abs(uv);
    return step(max(uv.x, uv.y),size);
}


float circle (vec2 uv, float size)
{return step(length(uv), size);}

void main() {



    vec2 uv = 2.*(gl_FragCoord.xy/RENDERSIZE.xy)-1.;
    uv.x /= RENDERSIZE.y / RENDERSIZE.x;
	// scale the uv
    uv *= 5.;
    // repeating the uv into a grid in 2 ways
    // 1.
    //vec2 guv = mod(uv,1)-.5;
    // 2.
    vec2 guv = fract(uv)-.5;
    // id of each cells
    vec2 id = floor(uv);
    guv *= rot(fGlobalTime*(length(id)+0.8)*0.3);
    
    float b = banding(uv);
    // clamp the value is a security if you want to use those shapes as 
    // mask for example
    float t = clamp(triangle(guv,0.15)-triangle(guv,0.05),0.,1.);
    float s = clamp(square(guv, 0.35) - square(guv, 0.31),0.,1.);
    float c = clamp(circle(uv,.8) - circle(uv,.7), 0., 1.);
	
    // also necessary here because when the square and triangle intersect
    // the color value is 2
    vec3 col = clamp(vec3(s+t),0.,1.); 
    gl_FragColor = vec4(col,1.);
}

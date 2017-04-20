/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "gameboy",
    "cga",
    "4colors",
    "2bit",
    "gamemakerstudio",
    "gamemaker",
    "paletteswapper",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MdlyDH by Heavybrush.  A CGA palette swapper + gameboy bonus",
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    }
  ]
}
*/


//   ▄████████    ▄██████▄     ▄████████ 
//  ███    ███   ███    ███   ███    ███ 
//  ███    █▀    ███    █▀    ███    ███ 
//  ███         ▄███          ███    ███ 
//  ███        ▀▀███ ████▄  ▀███████████ 
//  ███    █▄    ███    ███   ███    ███ 
//  ███    ███   ███    ███   ███    ███ 
//  ████████▀    ████████▀    ███    █▀  
// 

/*

////only for game maker studio////

varying vec2 v_vTexcoord;
varying vec4 v_vColour;

varying vec2 fragCoord;
//you have to put varying vec2 fragCoord also in the vertex shader
//after write in the last row of the local scope of the vertex shader: fragCoord = in_Position.xy

uniform vec2 RENDERSIZE;
uniform float TIME;

//palette 0 is not cga but gameboy, I put it as bonus
uniform int palette;
uniform float gamma;

*/

// rgb to float
// rgb to float = rgb / 255

// 0 = 0.0
// 85 = 0.333
// 170 = 0.666
// 255 = 1.0

void main() {



    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    vec3 c = vec3(0.0);
    float alpha = 1.0;
    
    //only for shadertoy
    int palette = 2; //the number between 0 and 6 change palette
    float gamma = 1.5; //the gamma change the threshold of the palette swapper
    
    //c = texture2D(gm_BaseTexture,uv).rgb;
    
    
    //IMG_NORM_PIXEL(inputImage,mod(uv - dr,1.0))
    
    
    c = texture(inputImage,uv).rgb;
    
    c.r = pow(abs(c.r),gamma);
    c.g = pow(abs(c.g),gamma);
    c.b = pow(abs(c.b),gamma);
    
    vec3 col1 = vec3(0.0);
    vec3 col2 = vec3(0.0);
    vec3 col3 = vec3(0.0);
    vec3 col4 = vec3(0.0);
    
    if(palette == 0) {
        col1 = vec3(0.612,0.725,0.086);
        col2 = vec3(0.549,0.667,0.078);
        col3 = vec3(0.188,0.392,0.188);
        col4 = vec3(0.063,0.247,0.063);
    }
    if(palette == 1) {
        col1 = vec3(0.0);
        col2 = vec3(0.0,0.666,0.666);
        col3 = vec3(0.666,0.0,0.666);
        col4 = vec3(0.666,0.666,0.666);
    }
    if(palette == 2) {
        col1 = vec3(0.0);
        col2 = vec3(0.333,1.0,1.0);
        col3 = vec3(1.0,0.333,1.0);
        col4 = vec3(1.0);
    }
    if(palette == 3) {
        col1 = vec3(0.0);
        col2 = vec3(0.0,0.666,0.0);
        col3 = vec3(0.666,0.0,0.0);
        col4 = vec3(0.666,0.333,0.0);
    }
    if(palette == 4) {
        col1 = vec3(0.0);
        col2 = vec3(0.333,1.0,0.333);
        col3 = vec3(1.0,0.333,0.333);
        col4 = vec3(1.0,1.0,0.333);
    }
    if(palette == 5) {
        col1 = vec3(0.0);
        col2 = vec3(0.0,0.666,0.666);
        col3 = vec3(0.666,0.0,0.0);
        col4 = vec3(0.666,0.666,0.666);
    }
    if(palette == 6) {
        col1 = vec3(0.0);
        col2 = vec3(0.333,0.666,0.666);
        col3 = vec3(1.0,0.333,0.333);
        col4 = vec3(1.0);
    }
    float dist1 = length(c - col1);
    float dist2 = length(c - col2);
    float dist3 = length(c - col3);
    float dist4 = length(c - col4);
    float d = min(dist1,dist2);
    d = min(d,dist3);
    d = min(d,dist4);
    if(d == dist1) {
        c = col1;
    }
    else if(d == dist2) {
        c = col2;
    }
    else if(d == dist3) {
        c = col3;
    }
    else {
        c = col4;
    }
    
    
    //gl_FragColor = vec4(c,alpha).rgba;
    gl_FragColor = vec4(c,alpha).rgba;
    //gl_FragColor = IMG_NORM_PIXEL(inputImage,mod(uvec2,1.0));
}

/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#45065.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


/*


	Minimal Hexagonal Grid
	----------------------

	This is not exactly a cutting edge example, but I'd almost say getting comfortable
	with a hexagonal grid is mandatory when it comes to creating interesting patterns.
	Therefore, I've put this up - as an additional alternative to the other beginner 
	references - for anyone who wants to get a start.
	
	In regard to tesselating a grid with a single regular polygon, you have a choice 
	between a triangle, a square or a hexagon (unless I'm overlooking one). The hexagon 
	has the most sides, which means it provides the most interesting combinations. A
	great example of this would be BigWings's "Hexagonal Truchet Weaving" shader.

	There are a few different methods used to construct a hexagonal grid. Everyone
	has their preference, but I like to obtain the nearest hexagon center with the least
	amount of effort (fewest operations), then render a	2D isosurface around it.

	If the 2D surface happens to be a hexagon, the return value will represent the 
	nearest	edge distance - since isosurfaces give boundary distances by design. 
	Rendering a circle around the cell center will be analogous to a Euclidian hexagon 
	center distance.

    It's also trivial to take the nearest hexagonal center point to produce a relative 
	position for the cell and a unique hexagonal grid cell ID.

	Anyway, I've explained the process in more detail below. It's one of the those 
	things that is easy to do, but less easy to explain. However, I believe the code 
	should make it more clear.

	By the way, if anyone spots any errors, or knows of ways to improve the operation 
	count in the "getHex" function, then feel free to let me know.

	Related references:
	
	// Uses the same principle. I stuck with my own code (adapted from my "Hexagonal 
    // Blocks" example), but I liked the simpler way Iomateron compared distances, so 
	// I've adopted that portion of his code.
	Iomateron - simple hexagonal tiles
	https://www.shadertoy.com/view/MlXyDl

	// You can't do a hexagonal grid example without referencing this. :) Very stylish.
	Hexagons - distance - iq
	https://www.shadertoy.com/view/Xd2GR3

*/

#define SQRT3 1.7320508

// Helper vector. If you're doing anything that involves regular triangles or hexagons, the
// 30-60-90 triangle will be involved in some way, which has sides of 1, sqrt(3) and 2.
//const vec2 s = vec2(1, 1.7320508);

const vec2 s = vec2(1.0, SQRT3);

// Standard vec2 to float hash - Based on IQ's original.
float hash21(vec2 p){ return fract(sin(dot(p, vec2(141.13, 289.97)))*43758.5453); }


// The 2D hexagonal isosuface function: If you were to render a horizontal line and one that
// slopes at 60 degrees, mirror, then combine them, you'd arrive at the following. As an aside,
// the function may be a bound - as opposed to a Euclidean distance representation, but either
// way, the result is hexagonal boundary lines.
float hex(in vec2 p){
    
    p = abs(p);
    
    // Below is equivalent to:
    //return max(p.x*.5 + p.y*.866025, p.x); 

    return max(dot(p, s*.5), p.x); // Hexagon.
    
}

// This function returns the hexagonal grid coordinate for the grid cell, and the corresponding 
// hexagon cell ID - in the form of the central hexagonal point. That's basically all you need to 
// produce a hexagonal grid.
//
// When working with 2D, I guess it's not that important to streamline this particular function.
// However, if you need to raymarch a hexagonal grid, the number of operations tend to matter.
// This one has minimal setup, one "floor" call, a couple of "dot" calls, a ternary operator, etc.
// To use it to raymarch, you'd have to double up on everything - in order to deal with 
// overlapping fields from neighboring cells, so the fewer operations the better.
vec4 getHex(vec2 p){
    
    // The hexagon centers: Two sets of repeat hexagons are required to fill in the space, and
    // the two sets are stored in a "vec4" in order to group some calculations together. The hexagon
    // center we'll eventually use will depend upon which is closest to the current point. Since 
    // the central hexagon point is unique, it doubles as the unique hexagon ID.
    vec4 hC = floor(vec4(p, p - vec2(.5, 1))/s.xyxy) + .5;
    
    // Centering the coordinates with the hexagon centers above.
    vec4 h = vec4(p - hC.xy*s, p - (hC.zw + .5)*s);
    
    // Nearest hexagon center (with respect to p) to the current point. In other words, when
    // "h.xy" is zero, we're at the center. We're also returning the corresponding hexagon ID -
    // in the form of the hexagonal central point. Note that a random constant has been added to 
    // "hC.zw" to further distinguish it from "hC.xy."
    //
    // On a side note, I sometimes compare hex distances, but I noticed that Iomateron compared
    // the squared Euclidian version, which seems neater, so I've adopted that.
    return dot(h.xy, h.xy)<dot(h.zw, h.zw) ? vec4(h.xy, hC.xy) : vec4(h.zw, hC.zw + 9.73);
    
}

void main(void){
	
    // Aspect correct screen coordinates.
    vec2 u = (gl_FragCoord.xy - RENDERSIZE.xy*.5)/RENDERSIZE.y;
    
    // Scaling, translating, then converting it to a hexagonal grid cell coordinate and
    // a unique coordinate ID. The resultant vector contains everything you need to produce a
    // pretty pattern, so what you do from here is up to you.
    	
	vec4 h = getHex(u*5. + vec2(1.0,0.0)*TIME/2.);
     	gl_FragColor = vec4(h.xy, 0.0,1.0);
	//return;
	
	
	
    // The beauty of working with hexagonal centers is that the relative edge distance will simply 
    // be the value of the 2D isofield for a hexagon.
    //
    float eDist = hex(h.xy); // Edge distance.
    float cDist = dot(h.xy, h.xy); // Relative squared distance from the center.

    
    // Using the idetifying coordinate - stored in "h.zw," to produce a unique random number
    // for the hexagonal grid cell.
    float rnd = hash21(h.zw);
    rnd = sin(rnd*6.283 + TIME*1.5)*.5 + .5; // Animating the random number.
    
    // It's possible to control the randomness to form some kind of repeat pattern.
    //rnd = mod(h.z + h.w, 4.)/3.;
    
    
    
    // Initiate the background to an off white color.
    vec3 col = vec3(1, .95, .9);

    
    // Using the random number associated with the hexagonal grid cell to provide some color
    // and some smooth blinking. The coloring was made up, but it's worth looking at the 
    // "blink" line which smoothly blinks the cell color on and off.
    //
    float blink = smoothstep(0., .125, rnd - .666); // Smooth blinking transition.
    float blend = dot(sin(u*3.14159*2. - cos(u.yx*3.14159*2.)*3.14159), vec2(.25)) + .5; // Screen blend.
    col = max(col - mix(vec3(0, .4, .6), vec3(0, .3, .7), blend)*blink, 0.); // Blended, blinking orange.
    col = mix(col, col.xzy, dot(sin(u*6. - cos(u*3. + TIME)), vec2(.4/2.)) + .4); // Orange and pink mix.
    
    // Uncomment this if you feel that greener shades are not being fairly represented. :)
    //col = mix(col, col.yxz, dot(cos(u*6. + sin(u*3. - iTime)), vec2(.35/2.)) + .35); // Add some green.

    
    // Using the edge distance to produce some repeat contour lines. Standard stuff.
    float cont = clamp(cos(eDist*6.283*12.)*1. + .95, 0., 1.);
    cont = mix(cont, clamp(cos(eDist*6.283*12./2.)*1. + .95, 0., 1.), .125);
    col = mix(col, vec3(0), (1.-cont)*.95);
    
    // Putting in some dark borders.
    col = mix(col, vec3(0), smoothstep(0., .03, eDist - .5 + .04));
  
    // Using the two distance variables to give the pattern a bit of highthing.
    col *= max(1.25 - eDist*1.5, 0.);
    col *= max(1.25 - cDist*2., 0.);
    
    // Rough gamma correction.    
    gl_FragColor = vec4(sqrt(max(col, 0.)), 1);
}
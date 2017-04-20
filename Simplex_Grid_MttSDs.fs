/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "rhombus",
    "skew",
    "transformations",
    "rhombi",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MttSDs by ptrgags.  Learned how to skew space from squares into sets of two equilateral triangles.\n\nClick and drag to pan around.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


#define PI 3.1415

/* 
 * skew spacee from squares into rhombi.
 * Essentially we are rotating the y-axis
 * 60 degrees counterclockwise while keeping
 * the x-axis still. 
 * 
 * Transform derivation: screen(x, y) -> skewed(x', y')
 * x' = x - ycos(60°)
 * y' =     ysin(60°)
 * 
 * Inverse (the transform we want): skewed(x', y')
 * y = y'csc(60°)
 * x = x' + ycos(60°)
 *   = x' + y'cos(60°)csc(60°)
 *   = x' + y'cot(60°)
 */

vec2 rhombi(vec2 point) {
    mat2 transform = mat2(
        1.0, 0.0,
        //cot(60°), csc(60°)
        1.0 / tan(PI / 3.0), 1.0 / sin(PI / 3.0)
    );
    return transform * point;
}

void main() {



    vec2 mouse_uv = iMouse.xy / RENDERSIZE.xy;
    
    //screen space -> uv space
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    //Fix aspect ratio
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    
    //Pan around with the mouse
    uv += mouse_uv;
    
    //How many rhombus-shaped cells to make
    float num_boxes = 10.0;
    
    //skew space
    uv = rhombi(uv);
    
    //Scale up space from [0.0, 1.0] -> [0.0, num_boxes]
    uv *= num_boxes;
    
    //Get the (x, y) indices of the box.
    //box as in container. these 'boxes' are rhombi!
    vec2 box = floor(uv);
    
    //Get the UV coordinates within the current rhombus
    vec2 box_uv = fract(uv);
    //One triangle in the rhombus is full color, the other
    //is dark.
    float brightness = step(box_uv.x, box_uv.y);
    
    //Color each box a different color
	gl_FragColor = brightness * vec4(box / num_boxes, 0.0, 1.0);
}

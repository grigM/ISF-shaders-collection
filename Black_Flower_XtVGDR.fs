/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "circles",
    "flower",
    "animation",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtVGDR by Zeliss.  I felt like making something interesting with just black and white.",
  "INPUTS" : [

  ]
}
*/


const float PI = 3.14159265358979; //Probably enough precision?
const float TAU = 2.0 * PI;

//Returns how much coord is within a circle centered at circle_pos. [0, 1]
float circle(vec2 circle_pos, vec2 coord)
{
    float circ_rad = 70.0;
    float circ_blur = 1.0;
    float dist = distance(circle_pos, coord);
    return smoothstep(circ_rad + circ_blur, circ_rad - circ_blur, dist); 
}

void main()
{
    vec4 black = vec4(0.0, 0.0, 0.0, 1.0);
    vec4 white = vec4(1.0, 1.0, 1.0, 1.0);
    
    vec2 center = RENDERSIZE.xy * vec2(0.5);
    float t = TIME;
    float d = 70.0*sin(t/PI); //Distance of each circle from the center. (Can be pos/neg)
    
    const float MAX_CIRCLES = 9.0; //Max number of circles.
    float num_circles = MAX_CIRCLES/2.0 + MAX_CIRCLES/2.0*cos(t/PI) + 0.01; //Current number of circles.
    float circles = 0.0; //The total number of circles this pixel is in.
    
    for (float f = 0.0; f < MAX_CIRCLES; f++) {
        if (f < num_circles) { //Wish I could have non-const number of loop iterations :(
        	float ap = t + f/num_circles * TAU; //Angular position of each circle, [0, tau]
        	vec2 newCirclePos = center + d*vec2(cos(ap), sin(ap));
        	circles += circle(newCirclePos, gl_FragCoord.xy); //If this pixel is in the new circle, circles increases.
        }
    }
    
    circles = (cos(circles*PI)+1.0)/2.0; //Smoothly interpolate between odd and even values.
    gl_FragColor = mix(black, white, circles);
}
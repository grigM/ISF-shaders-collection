/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XlcSDf by ptrgags.  Inspired by the color diffraction from https:\/\/www.shadertoy.com\/view\/XsXXDn",
  "INPUTS" : [

  ]
}
*/


// Bell-shaped curve with maximum value 1.0
float bell(float x, float center, float stretch) {
    return exp(-pow((x - center) / stretch, 2.0));
}

void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    //Center the origin
    uv -= 0.5; 
    
    //Fix aspect ratio
    float aspect_ratio = RENDERSIZE.x / RENDERSIZE.y;
    uv.x *= aspect_ratio;
    
    //Distance from center
    float dist_center = length(uv);
    
    // Iterate over the color components
    for (int i = 0; i < 3; i++) {
        //Each
        float phase_shift = float(i) * 0.13;
        
    	//wrap the time value to a range [0.0, 1.5]
    	//this is slightly greater than size of the window
    	//so it makes the animation smoother. This will be
    	//used for the radius of animated pulses
    	float pulse_radius = mod(TIME + phase_shift, 1.5);
    
    	//how much to stretch the pulse (which takes the form of a bell-shaped
    	//curve with respect to distance from center.
    	float pulse_stretch = 0.2;
     
        //Make a bell curve pulsing outwards from the center of the screen.
        gl_FragColor[i] = bell(dist_center, pulse_radius, pulse_stretch);
    }
    gl_FragColor.a = 1.0;
}
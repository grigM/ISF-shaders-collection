/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "metaball",
    "learning",
    "gooey",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MtyGR3 by twitchingace.  Messing around with positive and negative coloured 2D metaballs.",
  "INPUTS" : [

  ]
}
*/


///////////////////////////////////////////////////////////
// Simple messing around with 2D metaballs.
// Exploring colourization as well as negative metaballs.
// Use ISTHRESHOLD to determine if a zeroing threshold
//   should be applied.
// if ISTHRESHOLD is active, use ISSOLID to limit the balls
// to their outlines.
///////////////////////////////////////////////////////////

#define ISSOLID 1
#define ISTHRESHOLD 0
#define posnum 7
#define negnum 2

float metaball(in vec3 ball, in vec2 testPoint){
	return ball.z/length(ball.xy - testPoint);
}

void main()
{
    
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xx;
    // Set up positive metaballs
    vec3 points[posnum];
    vec3 colours[posnum];
    points[0] = vec3(abs(0.1 + sin(TIME)), 0.2 + 0.2 * cos(TIME), .15);
    colours[0] = vec3(1., 0., 0.);
    
    points[1] = vec3(0.5, abs(0.3 + .2 * sin(TIME * 0.7 + 5.)), .1);
    colours[1] = vec3(0., 1., 0.);
    
    points[2] = vec3(abs(0.4 + sin(TIME)), 0.4 - 0.2 * cos(TIME), .3);
    colours[2] = vec3(0., 0., 1.);
    
    points[3] = vec3(0.43,0.25 + .1* cos(TIME), .15);
    colours[3] = vec3(1., 1., 0.);
    
    points[4] = vec3(0.42 + .4* sin(TIME),0.32 - .1 * sin(TIME * .2), .2);
    colours[4] = vec3(1., 0., 1.);
    
    points[5] = vec3(abs(0.12 + .3 * sin(TIME - 10.)),0.52 + .25* cos(TIME), .07);
    colours[5] = vec3(0., 1., 1.);
    
    points[6] = vec3(0.2+ .25* cos(TIME),0.6 - .1 * cos(TIME + 5.), .2);
	colours[6] = vec3(1., 1., 1.);        
    
    
    // Set up negative metaballs
    vec3 negPoints[negnum];
    vec3 negColours[negnum];
    
    negPoints[0] = vec3(0.64, 0.25, .2);
    negColours[0] = vec3(1.2);
    negPoints[1] = vec3(0.34 + .3 * sin(TIME * 2.), 0.25 + .2 * sin(TIME* 0.1), .15);
    negColours[1] = vec3(1.);
    vec3 rgb;
    
    // apply positive metaballs
    for (int i = 0; i < posnum; i++){  
    	rgb += colours[i] * metaball(points[i], uv);    
    }
    
    // negative metaball application
    for (int i = 0; i < negnum; i++){  
    	rgb -= negColours[i] * metaball(negPoints[i], uv);    
    }
    
    // Manage effects
    #if ISTHRESHOLD == 1
    float threshold = 0.65;
    if (rgb.x/2. <= threshold){
        rgb.x = 0.;
    }
    if (rgb.y/2. <= threshold){
    	rgb.y = 0.;
    }
    if (rgb.z/2. <= threshold){
    	rgb.z = 0.;
    }
   
    
    #if ISSOLID == 0
    float border = 0.03;
    if (rgb.x/2. >= threshold + border || 
        	rgb.y/2. >= threshold + border || 
        	rgb.z/2. >= threshold + border){
        rgb = vec3(0.0);
    }
    #endif
    #endif
    
	gl_FragColor = vec4(rgb/2.,1.0);
}
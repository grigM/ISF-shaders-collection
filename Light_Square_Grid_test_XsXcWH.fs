/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "lightsquares",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XsXcWH by SalikSyed.  A grid of light squares",
  "INPUTS" : [

  ]
}
*/




float integrate_montecarlo(vec2 start, vec2 end, vec2 pt) {
    if (length(pt - (start*0.5 + end*0.5)) > 0.5) {
     	return 0.0;   
    }
    float sum = 0.0;
    for(int i  = 0; i <= 15; i++) {
     	float alpha = float(i) / 15.0;
        float d = length((alpha * start + (1.0 - alpha) * end) - pt);
        sum += 1.0 / (pow(d, 1.44));
    }
    return 0.005*sum;
}    




float integrate(vec2 start, vec2 end, vec2 pt) {
	return integrate_montecarlo(start, end, pt); 
}


float compute_light_from_square(vec2 outerMin, vec2 outerMax, vec2 pos) {
   	float d = 0.0;
    // compute the light contribution from each fluorescent segment
    d += integrate(outerMin, vec2(outerMin.x, outerMax.y), pos);
    d += integrate(vec2(outerMax.x, outerMin.y), outerMin, pos);
    d += integrate(vec2(outerMin.x, outerMax.y), outerMax, pos);
    d += integrate(vec2(outerMax.x, outerMin.y), outerMax, pos);
    return d;
}    


void main() {



    gl_FragColor = vec4(0.0);
    vec2 pos = gl_FragCoord.xy/RENDERSIZE.xy;
    float aspect = RENDERSIZE.x/RENDERSIZE.y;
    pos.x *=aspect;
    float PADDING_X = 0.1;
    float PADDING_Y = 0.1;
    int NUM_BOXES = 5;
    float BOX_PADDING = 0.009;
    float boxWidth = (1.0-2.0*PADDING_X-0.5*float(NUM_BOXES-1)*BOX_PADDING)/float(NUM_BOXES);
    float boxHeight = (1.0-2.0*PADDING_X-0.5*float(NUM_BOXES-1)*BOX_PADDING)/float(NUM_BOXES);
    float d = 0.0;
    for(int i = 0; i < NUM_BOXES; i++) {
        for(int j = 0; j < NUM_BOXES; j++) {
			vec2 outerMin = vec2(float(i) * boxWidth + PADDING_X, float(j) * boxHeight + PADDING_Y);
            vec2 outerMax = outerMin + vec2(boxWidth, boxHeight);
            outerMin += vec2(BOX_PADDING, BOX_PADDING);
            outerMax -= vec2(BOX_PADDING, BOX_PADDING);
			d += compute_light_from_square(outerMin, outerMax, pos);
        }
    }
    vec4 mixColor =  vec4(19.0/255.0, 98.0/255.0, 128.0/255.0, 1.0);
    gl_FragColor =  mix(0.00028 *vec4(pow(d, 1.77 + 0.43*(sin(TIME*5.0)*0.5 + 0.5) )), mixColor, 0.4);
}

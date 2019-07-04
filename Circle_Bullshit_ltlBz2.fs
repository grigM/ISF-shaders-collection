/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "beginner",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ltlBz2 by lookezen.  learning stuff",
  "INPUTS" : [

  ]
}
*/


bool cirOL(vec2 center, float rad)
{
    return(length(center) < rad);
}

void main() {



	vec4 col1 = vec4(1., 1., 1., 1.);
    vec4 col2 = vec4(0., 0., 0., 1.);
    float f = abs(sin(TIME));
    int counter = 0;
    
    vec2 r =  2.0*vec2(gl_FragCoord.xy - 0.5*RENDERSIZE.xy)/RENDERSIZE.y;
    float c2 = 0.;
    for(float i = -2.; i < 2.; i += 0.1)
    { 	
        c2+= 2.;
        vec2 temp = r;
        temp.x += i;
        temp.y += sin(c2 * (TIME / 16.));
        if(cirOL(temp, f)){ counter += 1; }
    }
    
    if(mod(float(counter), 2.0) == 0.0)
    {
        gl_FragColor = col1;
    }
    else
    {
        gl_FragColor = col2;
    }
    
}

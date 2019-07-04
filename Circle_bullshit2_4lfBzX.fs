/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "beginner",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4lfBzX by lookezen.  learning stuff",
  "INPUTS" : [

  ]
}
*/


bool cirOL(vec2 center, float rad)
{
    return(length(center) < rad);
}

void main() {



	vec4 col1 = vec4( 0., 0.4, 1., 1.);
    vec4 col2 = vec4( 0., 0.2, 1., 1.);
    vec4 col3 = vec4( 0., 0.,  1., 1.);
    vec4 col4 = vec4( 0., 0.6, 1., 1.);
    vec4 col5 = vec4( 0., 0.8, 1., 1.);
    vec4 col6 = vec4( 0., 0.10, 1., 1.);
    vec4 col7 = vec4( 0., 0.1, 1., 1.);
   
    float f = abs(sin(TIME * 0.02));
    int counter = 0;
    
    vec2 r =  2.0*vec2(gl_FragCoord.xy - 0.5*RENDERSIZE.xy)/RENDERSIZE.y;
    float c2 = 0.;
    for(float i = -2.; i < 2.; i += 0.05)
    { 	
        c2+= 10.;
        vec2 temp = r;
        temp.x += i;
        temp.y += sin(c2 * (TIME /500.));
        if(cirOL(temp, f)){ counter += 1; }
    }
    
		
    if(counter == 0)
    {
        gl_FragColor = vec4(0., 0., 0., 1.);
    }
    
    else if(mod(float(counter), 2.0) == 0.0)
    {   
        gl_FragColor = col3;
    }
    
    else if(mod(float(counter), 3.0) == 0.0)
    {
		gl_FragColor = col1;
    }
    
    else if(mod(float(counter), 7.0) == 0.0)
    {
        gl_FragColor = col4;
    }
    else if(mod(float(counter), 11.0) == 0.0)
    {
        gl_FragColor = col5;
    }
    
    else if(mod(float(counter), 13.0) == 0.0)
    {
        gl_FragColor = col5;
    }
    else if(mod(float(counter), 17.0) == 0.0)
    {
        gl_FragColor = col5;
    }
    
    else
    {
        gl_FragColor = col2;
    }
    
}

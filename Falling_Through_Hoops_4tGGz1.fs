/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "raymarching",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tGGz1 by joe_thomas490.  Raymarching practice",
  "INPUTS" : [

  ]
}
*/


//Distance field functions found here : http://www.iquilezles.org/www/articles/distfunctions/distfunctions.htm

//-----------------Operation functions--------------------

//Union operation (d1 + d2)
float opU(float d1, float d2)
{
  	return min(d1,d2);   
}

//Subtraction operation (d1 - d2)
float opS(float d1, float d2)
{
   return max(-d1,d2);   
}

//Intersection operation (only shows the primitives that intersect)
float opI(float d1, float d2)
{
    return max(d1,d2);
}

//Changes the colour based on it's distance from the camera
float opFog(float p)
{
  return 1.0 / (1.0 * p * p * 0.25);   
}

//Repeats the primitive across the coordinate space (p = point along ray, c = dimensions of repetition)
vec3 opRepeat(vec3 p, vec3 c)
{
    vec3 q = mod(p,c) - 0.5 * c;
    return q;
}


//-----------------Distance field functions--------------------

//Distance field function for a sphere (p = point along ray, r = radius of sphere)
float rSphere(vec3 p, float r)
{
    vec3 q = fract(p) * 2.0 - 1.0;
    
   	return length(q) - r;
}

//Distance field function for a rounded box (p = point along ray, d = dimensions of box, r = radius of roundness)
float rRoundedBox(vec3 p, vec3 b, float r)
{
    vec3 q = fract(p) * 2.0 - 1.0;
    
    return length(max(abs(q)-b,0.0))-r;
}

//Distance field function for a torus primitive (p = point along ray, t = dimesions of torus (inner radius, outer radius))
float rTorus(vec3 p, vec2 t)
{
    vec3 q = fract(p) * 2.0 - 1.0;
    
    vec2 s = vec2(length(q.xz) - t.x,q.y);
    return length(s)-t.y;
}


//-----------------Main functions--------------------

//Main tracing function that maps the distances of each pixel
float trace(vec3 ro, vec3 rt)
{
    float t = 0.0;
    
    //Loop through (in this case 32 times)
    for(int i = 0; i < 32; ++i)
    {
        //Get the point along the ray
        vec3 p = ro + rt * t;
        
        //Get the value for the distance field
        
        //float d = rSphere(p, (sin(TIME) + 1.0) * 0.25);
        //float d = rRoundedBox(p, vec3(0.25,0.1,0.25), 0.1);
        float d = rTorus(p, vec2(1.0,(sin(TIME)+ 1.25) * 0.1));
        
        t += d * 0.5;
    }
    
    return t;
}

void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    //Make the coordinate space between -1 and 1
    uv = uv * 2.0 - 1.0;
    
    //Sort out the aspect ratio so the shapes aren't deformed
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    
    //Create the ray
    vec3 r = normalize(vec3(uv, 1.0));
    
    float the = 90.;
        
    r.yz *= mat2(cos(the),sin(the),-sin(the),cos(the));
    
    //Create where the ray is going towards
    vec3 o = vec3(0.0, TIME + (sin(TIME)+1.0),0.0);
    
    //Call the main trace function and get a value
    float t = trace(o,r);
    
    //Call the fogging function to apply some depth to the image
    t = opFog(t);
    
    gl_FragColor = vec4(t,0.0,0.0,1.0);
}
/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "inscatterfog",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tyGDV by pocketmoon.  Calculates atmospheric illumination (mist, fog etc) by integrating in-scatter along the eye\/fragment line for multiple light source.",
  "INPUTS" : [

  ]
}
*/



// Air Lights by Rob James  "Flying Streetlights in Fog"
// Resuurecting my demo's from 2004/2006 
// initial set up for inscatter calcs.
// Integrates in-scatter along the eye/fragment line for a number of light sources.
 vec3 LightPosition[5];


void main()
{

	float itotal = 0.0;
    
    float brightness = 0.3;
    
    LightPosition[0]=vec3(0.01,0,-110.0);
    LightPosition[1]=vec3(0.06,0,-110.0);
    LightPosition[2]=vec3(0,0,-110.0);
    
    LightPosition[0].x += 0.02* sin(3.32 * TIME); 
    LightPosition[0].y += 0.01* sin(1.37 * TIME); 
    LightPosition[0].z += 30.315* sin(4.22 * TIME); 
    
    LightPosition[1].x -= 0.01* sin(2.11 * TIME); 
    LightPosition[1].y += 0.01* sin(2.132 * TIME); 
    LightPosition[1].z += 25.315* sin(1.33 * TIME); 
    
    LightPosition[2].x += 0.01* sin(2.21 * TIME); 
    LightPosition[2].y-= 0.02* sin(3.12 * TIME); 
    LightPosition[2].z -= 40.315* sin(2.08 * TIME); 
    
    float hres = RENDERSIZE.y/2.0;
    vec3 surf = vec3((gl_FragCoord.x-hres)/RENDERSIZE.y, (gl_FragCoord.y-hres)/RENDERSIZE.y, -1000.0);
    
	float fEye ;   
    float U ;
    vec3 Pi ;
    float rDist ;
    float ooDp ;

    float mag =dot (surf,surf);//dot prod of v with itself is length squared
 
    for (int i = 0 ; i < 3; i++)
    {

        U =dot( surf, LightPosition[i])/mag;

        //Gives us the point along the eye/frag line that is closest to light soure.
        Pi =  surf * U;

        //The distance between pI and light
        rDist = length(Pi - LightPosition[i]);

        ooDp = 1.0/rDist;

        //Approximate Estimate of Accumulated in-scatter
        fEye = ooDp * atan(ooDp * length(Pi));

      //  itotal = itotal + fEye*0.3*(-1.5/LightPosition[i].z);
         itotal = itotal + (brightness*fEye)/-LightPosition[i].z;
      
    }
    
   gl_FragColor =vec4 (vec3(itotal), 1.0);    
  
}
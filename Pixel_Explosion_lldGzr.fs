/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "explosion",
    "pixel",
    "fireworks",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/lldGzr by Leukbaars.  improved version of my fireworks. Better spread, added gravity and flicker\nTo be used as retro effect shader.\n",
  "INPUTS" : [
	{
			"NAME": "particles",
			"TYPE": "float",
			"DEFAULT": 80.5,
			"MIN": 5.0,
			"MAX": 500.0
	},
	{
			"NAME": "res",
			"TYPE": "float",
			"DEFAULT": 32.5,
			"MIN": 32.5,
			"MAX": 200.0
	}
  ]
}
*/


float seed = 0.32; //----------------------------------------------------------starting seed
//const float particles = 32.0; //----------------------------------------------change particle count
//float res = 32.0; //-----------------------------------------------------------pixel resolution
float gravity = 0.72; //-------------------------------------------------------set gravity

void main()
{
	vec2 uv = (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy) / RENDERSIZE.y;
   	float clr = 0.0;  
    float timecycle = TIME-floor(TIME);  
    seed = (seed+floor(TIME));
    
    //testing
    float invres=1.0/res;
    float invparticles = 1.0/particles;

    
    for( float i=0.0; i<particles; i+=1.0 )
    {
		seed+=i+tan(seed);
        vec2 tPos = (vec2(cos(seed),sin(seed)))*i*invparticles;
        
        vec2 pPos = vec2(0.0,0.0);
        pPos.x=((tPos.x) * timecycle);
		pPos.y = -gravity*(timecycle*timecycle)+tPos.y*timecycle+pPos.y;
        
        pPos = floor(pPos*res)*invres; //-----------------------------------------comment this out for smooth version 

    	vec2 p1 = pPos;
    	vec4 r1 = vec4(vec2(step(p1,uv)),1.0-vec2(step(p1+invres,uv)));
    	float px1 = r1.x*r1.y*r1.z*r1.w;
        float px2 = smoothstep(0.0,200.0,(1.0/distance(uv, pPos+.015)));//added glow
        px1=max(px1,px2);
        
	    clr += px1*(sin(TIME*20.0+i)+1.0);
    }
    
	gl_FragColor = vec4(clr*(1.0-timecycle))*vec4(4, 0.5, 0.1,1.0);
}
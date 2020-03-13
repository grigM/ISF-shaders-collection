/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "3d",
    "plane",
    "holodeck",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XlKSzw by cacheflowe.  Test of a borrowed 3d perspective technique",
  "INPUTS" : [
    {
      "NAME" : "drawVert",
      "TYPE" : "bool",
      "DEFAULT" : true
    },
    {
      "NAME" : "drawHoriz",
      "TYPE" : "bool",
      "DEFAULT" : true
    },
    {
      "NAME" : "fade",
      "MAX" : 20,
      "TYPE" : "float",
      "DEFAULT" : 10,
      "MIN" : 0
    },
    {
      "NAME" : "oscFreq",
      "MAX" : 30,
      "TYPE" : "float",
      "DEFAULT" : 12,
      "MIN" : 0
    },
    {
      "NAME" : "oscAmp",
      "MAX" : 0.20000000000000001,
      "TYPE" : "float",
      "DEFAULT" : 0.029999999999999999,
      "MIN" : 0
    },
    {
      "NAME" : "displaceAmp",
      "MAX" : 1,
      "TYPE" : "float",
      "DEFAULT" : 0.10000000000000001,
      "MIN" : 0
    },
    {
      "NAME" : "displaceFreq",
      "MAX" : 10,
      "TYPE" : "float",
      "DEFAULT" : 2.5,
      "MIN" : 0
    }
  ],
  "ISFVSN" : "2"
}
*/


void main()
{
    float time = TIME;
    vec2 uv = 0.5 * (2. * gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.y;
    
    // warp uv pre-perspective shift
    //float displaceAmp = 0.1;
    //float displaceFreq = 2.5;
    uv += vec2(displaceAmp * sin(time + uv.x * displaceFreq));
    
    // 3d params
    // 3d plane technique from: http://glslsandbox.com/e#37557.0 
    float horizon = 0.1 * cos(time); 
    float fov = 0.35 + 0.15 * sin(time); 
	float scaling = 0.2;
    // create a 2nd uv with warped perspective
	vec3 p = vec3(uv.x, fov, uv.y - horizon);      
	vec2 s = vec2(p.x/p.z, p.y/p.z) * scaling;
    
    // wobble the perspective-warped uv 
    //float oscFreq = 12.;
    //float oscAmp = 0.03;
    s += vec2(oscAmp * sin(time + s.x * oscFreq));
	
	// normal drawing here
    // lines/lattice
    
    float verLine = 0.0;
    float horLine = 0.0;
    
    if(drawVert){
    	verLine = smoothstep(0.2, 0.8, 1. - pow(sin(s.x * 100.), 0.28));
    }
    if(drawHoriz){
    	horLine = smoothstep(0.2, 0.8, 1. - pow(sin(s.y * 100.), 0.28));
    }
    
    
    float color = max(
        horLine, 
        verLine
    );

	// fade into distance
	color *= p.z * p.z * fade;
    // create holodeck yellow with color value
	gl_FragColor = vec4( vec3(color, color, color), 1.0 );
}
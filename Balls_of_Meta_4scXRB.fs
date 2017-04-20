/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "sdf",
    "blackwhite",
    "metabals",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4scXRB by IO.  Last nights meta balls.\n\ni know the code is still shitty, but it's going on. \nIf you have any advise or comment feel free to share it.\n\nHave a inspiring day.\nIO_",
  "INPUTS" : [

  ]
}
*/


// Thanks to hughsk for the smin function 
// and the Inspiration.
// https://www.shadertoy.com/view/XsyGRW

float smin(float a, float b, float k) {
  float h = clamp(0.5 + 0.5 * (b - a) / k, 0.0, 1.0);
  return mix(b, a, h) - k * h * (1.0 - h);
}

void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    vec3 fColor = vec3(0.0);
   
    uv = 2.0 * uv - 1.0; // moves uv to [-1,1]
    uv.x *= RENDERSIZE.x / RENDERSIZE.y; // Fix aspect ratio. Extend uv.x to [-1*aspectRatio,1*aspecRatio]
   
    float aaFactor = 0.01; // Antialising Factor for smoothstep
      
   	//Sphere Positions
    vec2 oPos00 = vec2(sin(TIME*0.2+0.07),sin(TIME*0.8-0.07)*0.2);
    vec2 oPos01 = vec2(cos(TIME*0.8-0.07),cos(TIME*0.8-0.07)*0.1);
    vec2 oPos10 = vec2(cos(TIME*0.2),0.0);
    vec2 oPos11 = vec2(sin(TIME*0.8),0.05);
    
    //Scene Buildup
    float mat0 = length(oPos00 - uv) - 0.2;			  // Distance field of a sphere
    mat0 = smin(mat0,length(oPos01 - uv) - 0.35,0.3); // smin for merching the objects in the Scene 
    mat0 = smin(mat0,length(oPos10 - uv) - 0.2,0.3);  // 0.3 factor for archiving 
    mat0 = smin(mat0,length(oPos11 - uv) - 0.25,0.3); // noticable lerping between objects

    // object/scene Mask for coloring
    float object0 = 1.0 - smoothstep(0.0,aaFactor, mat0);
	float scene = 1.0 - object0;
    
    //Coloring
    vec3 sceneColor = vec3(1.0,1.0,1.0);
    vec3 objectColor0 = vec3(0.0,0.0,0.0);

    //Uncomment it for eye cancer color
    //sceneColor = vec3(1.0,0.0,0.65);
    //objectColor0 = vec3(0.0,1.0,1.0);
    
    //Mix objectcolor with scenebackgroundcolor
    fColor = vec3(mix(objectColor0,sceneColor,scene));

	gl_FragColor = vec4(fColor,1.0);
}
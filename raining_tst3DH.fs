/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tst3DH by Netgfx.  from https:\/\/www.shadertoy.com\/view\/4sKfDz",
  "INPUTS" : [

  ]
}
*/



float rand(float Seed){
    
    return fract(sin(Seed*4124213.)*37523.);
}

/*float rand(vec2 Seed){
    
    //return fract(sin(dot(Seed.xy ,vec2(12.9898,78.233))) * 43758.5453);
    return fract(sin(vec2(dot(Seed.xy*1.0,vec2(127.1,311.7)),dot(Seed.xy*1.0,vec2(269.5,183.3))))*43758.5453);
}
*/

float rand(vec2 co)
{
    float a = 12.9898;
    float b = 78.233;
    float c = 43758.5453;
    float dt= dot(co.xy ,vec2(a,b));
    float sn= mod(dt,3.14);
    return fract(sin(sn) * c);
}

float randInRange(float id, vec2 range){
    
    id = rand(id);
    return range.x + id * (range.y-range.x);
}


void AddPannedGrid( inout vec4 FragCol, vec2 uv, vec4 GridColor,
                   vec2 panningSpeedRange, float gridSize, float seedDiff){
    

    float verticalStretch = (6.5);
    vec2 st = uv;
    
    // This value tells you the index of the columns and rows. You might have 
    // to recalculate it if you pan something
    vec2 gridID = floor ( vec2(uv.x*rand(seedDiff) * gridSize, uv.y *verticalStretch));
    
    
    // Paning the texture
    
    uv.y+= rand(gridID.x+0. + seedDiff)+randInRange(gridID.x+seedDiff, panningSpeedRange)*TIME;
    
    // divide the uv so that it creates the grid
    uv = fract ( vec2(uv.x*rand(cos(3.14)) * gridSize , uv.y *verticalStretch));
   
    
    // Shading the grids
    float diagonal = smoothstep(0., 0.05, uv.y - uv.x*.1*st.y);

    vec3 temp = vec3((1.-uv.x)*pow((1.0-uv.y)*1.1,4.))*diagonal;
    
    
    // Applying the alpha and the fin color addtiv on top
    FragCol.xyz += GridColor.a * GridColor.xyz*temp;
    
}


void main() {



    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    // Compensating for aspect ration
    uv.x *= RENDERSIZE.x/RENDERSIZE.y;
    
    vec4 finalColor = vec4(0.,0.,0.,1.0);
    
    // Adding the efect Additiv
	AddPannedGrid(finalColor, uv, vec4(0.1,0.7,1.,0.7), vec2(1.1,.4), 100., 9.);
    //AddPannedGrid(finalColor, uv, vec4(0.0,0.7,1.,0.9), vec2(.1,.7), 100.,2.0);
    AddPannedGrid(finalColor, uv, vec4(0.5,0.7,1.,0.5), vec2(.5,.9), 90.,1.50);
    
    // Output to screen
    gl_FragColor = finalColor;
}

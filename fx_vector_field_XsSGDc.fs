/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "f735bee5b64ef98879dc618b016ecf7939a5756040c2cde21ccb15e69a6e1cfb.png"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XsSGDc by FabriceNeyret2.  the noise color is interpreted as a vector.\nF: toggle flow or gradient field\nC: toggle curves or stick vectors\nA: iterated offset along the field",
  "INPUTS" : [
  	{
     	"NAME" : "inputImage",
      	"TYPE" : "image"
    },
    {
      		"NAME" : "speed",
      		"TYPE" : "float",
      		"DEFAULT" : 1.0,
      		"MAX" : 2,
      		"MIN" : 0.0
    	},
    	{
      		"NAME" : "offset",
      		"TYPE" : "float",
      		"DEFAULT" : 1.0,
      		"MAX" : 2,
      		"MIN" : 0.0
    	},
    	
    {
      "NAME" : "FLOW",
      "TYPE" : "bool",
      "IDENTITY" : 0,
      "DEFAULT" : 0
    },
    {
      "NAME" : "CONT",
      "TYPE" : "bool",
      "DEFAULT" : 0
    },
    
    {
      		"NAME" : "R_SIZE",
      		"TYPE" : "float",
      		"MAX" : 20.0,
      		"DEFAULT" : 4.0,
      		"MIN" : 0.0
    	},
    	
    	{
      		"NAME" : "ZOOM_P",
      		"TYPE" : "float",
      		"MAX" : 512.0,
      		"DEFAULT" : 256.0,
      		"MIN" : 128.0
    	},
    	{
      		"NAME" : "V_ADV",
      		"TYPE" : "float",
      		"MAX" : 2,
      		"DEFAULT" : 1.0,
      		"MIN" : 0.0
    	},
    	{
      		"NAME" : "V_BOIL",
      		"TYPE" : "float",
      		"MAX" : 1,
      		"DEFAULT" : 0.5,
      		"MIN" : 0.0
    	},
    	
  ],
  "ISFVSN" : "2"
}
*/


#define SIZE (RENDERSIZE.x/R_SIZE) // cell size in texture coordinates
#define ZOOM (2. *ZOOM_P/RENDERSIZE.x)
float STRIP  = 1.;    // nbr of parallel lines per cell
//float V_ADV  = 1.;    // velocity
//float V_BOIL = .5;    // change speed
float t;


//bool CONT , FLOW ,ATTRAC; // A: draw field or attractor ?

vec3 flow( vec2 uv) {
   	vec2 iuv = floor(SIZE*(uv)+.5)/SIZE;
	vec2 fuv = 2.*SIZE*(uv-iuv);
	
	vec2 pos = .01*V_ADV*vec2(cos(t)+sin(.356*t)+2.*cos(.124*t),sin(.854*t)+cos(.441*t)+2.*cos(.174*t));	if (CONT) iuv=uv;
	vec3 tex = 2.*IMG_NORM_PIXEL(iChannel0,mod(iuv/(ZOOM*SIZE)-pos,1.0)).rgb-1.;
	float ft = fract(t*V_BOIL)*3.;
	if      (ft<1.) tex = mix(tex.rgb,tex.gbr,ft);
	else if (ft<2.) tex = mix(tex.gbr,tex.brg,ft-1.);
	else            tex = mix(tex.brg,tex.rgb,ft-2.);
	return (FLOW) ? vec3(tex.y,-tex.x,tex.z): tex;
}

void main() {



    t = ((TIME/2.)*speed)-offset;
 	
    vec2 uv =(gl_FragCoord.xy / RENDERSIZE.xy);
	vec3 col;
    
    //if (ATTRAC) {
    	vec2 tex = uv;
    	for(int i=0; i<15; i++) 
        	tex = tex+.03*flow(tex).xy;
        	
    		col = IMG_NORM_PIXEL(inputImage,tex ).rgb;
    /*} else {    
   		vec2 iuv = floor(SIZE*(uv)+.5)/SIZE;
		vec2 fuv = 2.*SIZE*(uv-iuv);
    	vec3 tex = flow(uv);
   		float v = fuv.x*tex.x+fuv.y*tex.y;     
		// v = length(fuv);
		v = sin(STRIP*v);
		col = vec3(1.-v*v*SIZE) * mix(tex,vec3(1.),.5);
    }*/
	// col = tex;
	gl_FragColor = vec4(col,1.0);
}

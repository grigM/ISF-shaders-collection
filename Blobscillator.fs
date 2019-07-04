/*{
	"CREDIT": "by mojovideotech",
  	"CATEGORIES" : [
  		"generator",
    	"blobs",
    	"distance",
    	"noise"
  ],
  	"DESCRIPTION" : "based on https:\/\/www.shadertoy.com\/view\/MlKXWm by cacheflowe.  A wannabe reaction-diffusion, but not at all :-P ",
  	"ISFVSN" : "2",
	"INPUTS" : [
	{
		"NAME": 	"scale",
		"TYPE": 	"float",
		"DEFAULT": 	3.5,
		"MIN": 		0.0,
		"MAX": 		10
	},
	{
		"NAME": 	"rate",
		"TYPE": 	"float",
		"DEFAULT": 	0.125,
		"MIN": 		0.0,
		"MAX": 		1.0
	},
	{
		"NAME": 	"loops",
		"TYPE": 	"float",
		"DEFAULT":	33.0,
		"MIN": 		1.0,
		"MAX": 		100.0
	},
	{
		"NAME": 	"center",
		"TYPE": 	"point2D",
		"DEFAULT":	[ 0, 0 ],
		"MAX" : 	[ 1.0, 1.0 ],
     	"MIN" : 	[ -1.0, -1.0 ]
	},
	{
		"NAME": 	"freq1",
		"TYPE": 	"float",
		"DEFAULT": 	0.95,
		"MIN": 		0.005,
		"MAX": 		1.0
	},
	{
		"NAME": 	"freq2",
		"TYPE": 	"float",
		"DEFAULT": 	3.0,
		"MIN": 		0.5,
		"MAX": 		10.0
	},
	{
     	"NAME" :	"seed1",
     	"TYPE" : 	"float",
     	"DEFAULT" :	233,
     	"MIN" : 	89,
     	"MAX" :		1597
	},
    {
     	"NAME" :	"seed2",
      	"TYPE" :	"float",
     	"DEFAULT" :	13,
     	"MIN" :		5,
     	"MAX" :		55
    }
  ]
}
*/

////////////////////////////////////////////////////////////
// Blobscillator  by mojovideotech
//
// based on :
// shadertoy.com\/view\/MlKXWm  
//
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0
////////////////////////////////////////////////////////////

float hash (float a) { return floor(cos(a)*seed1+sin(a*seed2));  }

void main() {
	
    vec2 uv = (2.0 * gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.y;	
    uv -= center.xy;
    uv *= 10.5-scale;
    float C = sin(TIME * rate) * freq1, dist = 0.0;												
    for(float i=10.0; i < 90.0; i++) {								
        float R = C + i;									
        vec2 N = vec2(sin(R), cos(R));				
        N *= abs(hash(R)) * freq2;							
        dist += sin(i + loops * distance(uv, N));				
    }
	gl_FragColor = vec4(vec3(dist),1.0);
}

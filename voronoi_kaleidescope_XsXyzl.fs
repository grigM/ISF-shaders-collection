/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel1",
      "PATH" : "3083c722c0c738cad0f468383167a0d246f91af2bfa373e9c5c094fb8c8413e0.png"
    },
    {
      "NAME" : "iChannel0",
      "PATH" : "f735bee5b64ef98879dc618b016ecf7939a5756040c2cde21ccb15e69a6e1cfb.png"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XsXyzl by chadmiral.  Brute force (i.e. slow-ass) voronoi convolution filter.\nWould be much faster with a fast NN query (like KD tree, for example)",
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    },
    
    {
			"NAME": "Num_cells",
			"TYPE": "float",
			"MIN": 64.0,
			"MAX": 512.0,
			"DEFAULT": 256.0
		},
		
		 {
			"NAME": "circle_mask",
			"TYPE": "bool",
			"DEFAULT": false
		},
  ]
}
*/


#define SMOOTH_MIN


float random(float seed, float t)
{
    return IMG_NORM_PIXEL(iChannel0,mod(vec2(t, seed),1.0)).y;
}

// polynomial smooth min
float poly_smin(float a, float b, float k)
{
    float h = clamp(0.5 + 0.5 * (b - a) / k, 0.0, 1.0);
    return mix(b, a, h) - k * h * (1.0 - h);
}

void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    float min_d2 = 100.0;
    float cell_id = -1.0;
    for(int i = 0; i < int(Num_cells); i++)
    {
        float seed = float(i) / float(Num_cells);
        float x = random(seed, 0.0 + 0.001 * TIME);
        float y = random(seed, 0.4 + 0.001 * TIME);
        
        //distance squared (faster than actual distance)
        float d2 = (uv.x - x) * (uv.x - x) + (uv.y - y) * (uv.y - y);
        
		if(circle_mask){
        	if(cell_id < 0.0) { cell_id = seed; }
        		float k = 0.05;
				d2 = poly_smin(d2, min_d2, k);
        		//cell_id = poly_smin(cell_id, seed, k);
        	if(d2 < min_d2) { 
        		cell_id = seed; min_d2 = d2;
        	}
		}else{
   
        	if(d2 < min_d2 || cell_id < 0.0)
        	{
            	cell_id = seed;
            	min_d2 = d2;
        	}
		}
        
        //plate_id = d2;
        
    }
    
    cell_id = clamp(cell_id, 0.0, 1.0);
    vec4 cell_color = IMG_NORM_PIXEL(iChannel1,mod(vec2(cell_id, 0.25),1.0));
    
    gl_FragColor = IMG_NORM_PIXEL(inputImage, mod(uv  * (cell_color.xy*2.0),1.0));
    //gl_FragColor = cell_color;
}

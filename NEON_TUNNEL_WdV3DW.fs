/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WdV3DW by alro.  Tunnel with 2D shapes and SDF glow on multiple layers. Distance functions from [url]https:\/\/www.iquilezles.org\/www\/articles\/distfunctions2d\/distfunctions2d.htm[\/url]",
  "INPUTS" : [
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 1.615,
		"MIN": 0.0,
		"MAX": 3.0
	},
	{
		"NAME": "intensity",
		"TYPE": "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 3.0
	},
	{
		"NAME": "radius",
		"TYPE": "float",
		"DEFAULT": 0.1,
		"MIN": 0.0,
		"MAX": 3.0
	},
	
	{
		"NAME": "scale",
		"TYPE": "float",
		"DEFAULT": 500,
		"MIN": 50.0,
		"MAX": 800.0
	},
	
	
	{
		"NAME": "layers",
		"TYPE": "float",
		"DEFAULT": 15,
		"MIN": 0.0,
		"MAX": 20.0
	},
	{
      "VALUES" : [
        0,
        1,
        2,
        3
      ],
      "NAME" : "draw_mode",
      "TYPE" : "long",
      "DEFAULT" : 0,
      "LABELS" : [
      	"mix",
        "only_circle",
        "only_rect",
        "only_triangle"
      ]
    },
    
    
	{
		"NAME": "mix_draw_circ",
		"TYPE": "bool",
		"DEFAULT": true
	},
	
	{
		"NAME": "mix_draw_rect",
		"TYPE": "bool",
		"DEFAULT": true
	},
	
	{
		"NAME": "mix_draw_triang",
		"TYPE": "bool",
		"DEFAULT": true
	},
	
	
	
  ]
}
*/


//Base values modified with depth later

//Distance functions from 
//https://www.iquilezles.org/www/articles/distfunctions2d/distfunctions2d.htm
float triangleDist(vec2 p){ 
    float k = sqrt(3.0);
    p.x = abs(p.x) - 1.0;
    p.y = p.y + 1.0/k;
    if( p.x+k*p.y>0.0 ) p=vec2(p.x-k*p.y,-k*p.x-p.y)/2.0;
    p.x -= clamp( p.x, -2.0, 0.0 );
    return -length(p)*sign(p.y);
}

float boxDist(vec2 p){
    vec2 d = abs(p)-1.0;
    return length(max(d,vec2(0))) + min(max(d.x,d.y),0.0);
}

float circleDist( vec2 p){
  return length(p) - 1.0;
}

//https://www.shadertoy.com/view/3s3GDn
float getGlow(float dist, float radius, float intensity){
    return pow(radius/dist, intensity);
}

void main() {

    
	vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    float widthHeightRatio = RENDERSIZE.x/RENDERSIZE.y;
    vec2 centre;
    vec2 pos;
	
    float t = (TIME * 0.05)*speed;
    
    float dist;
    float glow;
    vec3 col = vec3(0);
    
    //The spacing between shapes
    //float scale = 500.0;
    //Number of shapes
    //float layers = 15.0;
    
    float depth;
    vec2 bend;
    
    vec3 purple = vec3(0.611, 0.129, 0.909);
    vec3 green = vec3(0.133, 0.62, 0.698);
    
    float angle;
    float rotationAngle;
    mat2 rotation;
    
    //For movement of the anchor point in time
    float d = 2.5*(sin(t) + sin(3.0*t));
    //Create an out of frame anchor point where all shapes converge to    
    vec2 anchor = vec2(0.5 + cos(d), 0.5 + sin(d));
    
    //Create light purple glow at the anchor loaction
    pos = anchor - uv;
    pos.y /= widthHeightRatio;
    dist = length(pos);
    glow = getGlow(dist, 0.35, 1.9);
    col += glow * vec3(0.7,0.6,1.0);
    
	for(float i = 0.0; i < layers; i++){
        
        //Time varying depth information depending on layer
        depth = fract(i/layers + t);
        //Move the focus of the camera in a circle
        centre = vec2(0.5 + 0.2 * sin(t), 0.5 + 0.2 * cos(t));
        
        //Position shapes between the anchor and the camera focus based on depth
        bend = mix(anchor, centre, depth);
     	
        pos = bend - uv;
    	pos.y /= widthHeightRatio;
        //Rotate shapes
       	rotationAngle = 3.14 * sin(depth + fract(t) * 6.28) + i;
        rotation = mat2(cos(rotationAngle), -sin(rotationAngle), 
                        sin(rotationAngle),  cos(rotationAngle));
        
        pos *= rotation;
        
        //Position shapes according to depth
    	pos *= mix(scale, 0.0, depth);
    	
        float m = mod(i, 3.0);
        if(draw_mode==0){
        	if(m == 0.0){
        		if(mix_draw_rect){
        			dist = abs(boxDist(pos));
        		}
    	    }else if(m == 1.0){
        		if(mix_draw_triang){
        			dist = abs(triangleDist(pos));
        		}
      	  }else{
        		if(mix_draw_circ){
        			dist = abs(circleDist(pos));
        		}
        	}
        }else if(draw_mode==1){
        	if(m == 0.0){
        		dist = abs(circleDist(pos));
        	}else if(m == 1.0){
        		dist = abs(circleDist(pos));
			}else if(m == 2.0){
				dist = abs(circleDist(pos));
			}
        	
        }else if(draw_mode==2){
        	
        	if(m == 0.0){
        		dist = abs(boxDist(pos));
        	}else if(m == 1.0){
        		dist = abs(boxDist(pos));
			}else if(m == 2.0){
				dist = abs(boxDist(pos));
			}
        }else if(draw_mode==3){
        	
        	if(m == 0.0){
        		dist = abs(triangleDist(pos));
        	}else if(m == 1.0){
        		dist = abs(triangleDist(pos));
			}else if(m == 2.0){
				dist = abs(triangleDist(pos));
			}
        	
        	
        }
       
        //Get glow from base radius and intensity modified by depth
    	glow = getGlow(dist, radius+(1.0-depth)*2.0, intensity + depth);
        
        //Find angle along shape and map from [-PI; PI] to [0; 1]
        angle = (atan(pos.y, pos.x)+3.14)/6.28;
        //Shift angle depending on layer and map to [1...0...1]
		angle = abs((2.0*fract(angle + i/layers)) - 1.0);
        
        //White core
    	//col += 10.0*vec3(smoothstep(0.03, 0.02, dist));
        
        //Glow according to angle value
     	col += glow * mix(green, purple, angle);
	}
    
    //Tone mapping
    col = 1.0 - exp(-col);
    
    //Output to screen
    gl_FragColor = vec4(col,1.0);
}

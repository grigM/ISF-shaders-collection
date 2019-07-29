/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/Xst3Dj by cornusammonis.  Fluid-like continuous cellular automata.",
    "IMPORTED": {
        "iChannel1": {
            "NAME": "iChannel1",
            "PATH": "f735bee5b64ef98879dc618b016ecf7939a5756040c2cde21ccb15e69a6e1cfb.png"
        }
    },
    "INPUTS": [
   		 {
            "NAME": "reset",
            "TYPE": "event"
        },
        
        
        {
            "NAME" : "_K0",
            "TYPE" : "float",
            "DEFAULT" : -3.3333333333,
            "MAX" : 1.0,
            "MIN" : -4.0
		},
        {
            "NAME" : "_K1",
            "TYPE" : "float",
            "DEFAULT" : 0.66667,
            "MAX" : 0.9,
            "MIN" : 0.2
        },
		{
            "NAME" : "_K2",
            "TYPE" : "float",
            "DEFAULT" : 0.166666667,
            "MAX" : 0.2,
            "MIN" : 0.1
		},
		
		{
      		"NAME" : "ls",
      		"TYPE" : "float",
      		"MAX" : 0.4,
      		"DEFAULT" : 0.24,
      		"MIN" : 0.0
    	},
    	{
      		"NAME" : "cs",
      		"TYPE" : "float",
      		"DEFAULT" : 0.25,
      		"MAX" : 0.5,
      		"MIN" : -0.5
   		},

		{
      		"NAME" : "ps",
      		"TYPE" : "float",
      		"MAX" : 0.1,
      		"DEFAULT" : -0.06,
      		"MIN" : -0.1
    	},
		{
      		"NAME" : "ds",
      		"TYPE" : "float",
      		"MAX" : 2.98999999999999999,
      		"DEFAULT" :  -0.08,
     		 "MIN" : -2.0
    	},
    
    	
        {
      		"NAME" : "pwr",
      		"TYPE" : "float",
      		"MAX" : 1.5,
      		"DEFAULT" : 0.2,
      		"MIN" : 0.005
    	},
    	{
     		"NAME" : "amp",
      		"TYPE" : "float",
      		"MAX" : 2.0,
      		"DEFAULT" : 1.0,
      		"MIN" : 0.8
    	},
    
    	{
      		"NAME" : "sq2",
      		"TYPE" : "float",
      		"MAX" : 1.3999999999999999,
     		 "DEFAULT" : 0.7,
      		"MIN" : 0
    	},
    	
    	{
      		"NAME" : "texel_size",
      		"TYPE" : "float",
      		"DEFAULT" : 1.0,
      		"MAX" : 5.0,
      		"MIN" : 0.2
    	},
    	
    	
    	
     {
      "NAME" : "scale",
      "TYPE" : "float",
      "DEFAULT" : 1.0,
      "MAX" : 10.0,
      "MIN" : 0.0
    },
    	
     

    ],
    "PASSES": [
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferA"
        },
        {
        }
    ]
}

*/



/*
     
     
    
    
    {
      "NAME" : "pass2_ds",
      "TYPE" : "float",
      "DEFAULT" : 0.4,
      "MAX" : 1.0,
      "MIN" : -1.0
    },
    
    {
      "NAME" : "pass2_darken",
      "TYPE" : "float",
      "DEFAULT" : 0.01,
      "MAX" : 1.0,
      "MIN" : -1.0
    },
    
    
    {
      "NAME" : "pass2_D1",
      "TYPE" : "float",
      "DEFAULT" : 0.2,
      "MAX" : 1.0,
      "MIN" : -1.0
    }
    ,{
      "NAME" : "pass2_D2",
      "TYPE" : "float",
      "DEFAULT" : 0.05,
      "MAX" : 0.2,
      "MIN" : -0.2
    }
    */
    
void main() {
	if (PASSINDEX == 0)	{


	    //const float _K0 = -20.0/6.0; // center weight
	    //const float _K1 = 4.0/6.0; // edge-neighbors
	    //const float _K2 = 1.0/6.0; // vertex-neighbors
	    //const float cs = 0.25; // curl scale
	    //const float ls = 0.24; // laplacian scale
	    //const float ps = -0.06; // laplacian of divergence scale
	    //const float ds = -0.08; // divergence scale
	    //const float pwr = 0.2; // power when deriving rotation angle from curl
	    //const float amp = 1.0; // self-amplification
	    //const float sq2 = 0.7; // diagonal weight
	
	    vec2 vUv = gl_FragCoord.xy / RENDERSIZE.xy;
	    vec2 texel = texel_size / RENDERSIZE.xy;
	    
	    // 3x3 neighborhood coordinates
	    float step_x = texel.x;
	    float step_y = texel.y;
	    vec2 n  = vec2(0.0, step_y);
	    vec2 ne = vec2(step_x, step_y);
	    vec2 e  = vec2(step_x, 0.0);
	    vec2 se = vec2(step_x, -step_y);
	    vec2 s  = vec2(0.0, -step_y);
	    vec2 sw = vec2(-step_x, -step_y);
	    vec2 w  = vec2(-step_x, 0.0);
	    vec2 nw = vec2(-step_x, step_y);
	
	    vec3 uv =    IMG_NORM_PIXEL(BufferA,mod(vUv,1.0)).xyz;
	    vec3 uv_n =  IMG_NORM_PIXEL(BufferA,mod(vUv+n,1.0)).xyz;
	    vec3 uv_e =  IMG_NORM_PIXEL(BufferA,mod(vUv+e,1.0)).xyz;
	    vec3 uv_s =  IMG_NORM_PIXEL(BufferA,mod(vUv+s,1.0)).xyz;
	    vec3 uv_w =  IMG_NORM_PIXEL(BufferA,mod(vUv+w,1.0)).xyz;
	    vec3 uv_nw = IMG_NORM_PIXEL(BufferA,mod(vUv+nw,1.0)).xyz;
	    vec3 uv_sw = IMG_NORM_PIXEL(BufferA,mod(vUv+sw,1.0)).xyz;
	    vec3 uv_ne = IMG_NORM_PIXEL(BufferA,mod(vUv+ne,1.0)).xyz;
	    vec3 uv_se = IMG_NORM_PIXEL(BufferA,mod(vUv+se,1.0)).xyz;
	    
	    // uv.x and uv.y are our x and y components, uv.z is divergence 
	
	    // laplacian of all components
	    vec3 lapl  = _K0*uv + _K1*(uv_n + uv_e + uv_w + uv_s) + _K2*(uv_nw + uv_sw + uv_ne + uv_se);
	    float sp = ps * lapl.z;
	    
	    // calculate curl
	    // vectors point clockwise about the center point
	    float curl = uv_n.x - uv_s.x - uv_e.y + uv_w.y + sq2 * (uv_nw.x + uv_nw.y + uv_ne.x - uv_ne.y + uv_sw.y - uv_sw.x - uv_se.y - uv_se.x);
	    
	    // compute angle of rotation from curl
	    float sc = cs * sign(curl) * pow(abs(curl), pwr);
	    
	    // calculate divergence
	    // vectors point inwards towards the center point
	    float div  = uv_s.y - uv_n.y - uv_e.x + uv_w.x + sq2 * (uv_nw.x - uv_nw.y - uv_ne.x - uv_ne.y + uv_sw.x + uv_sw.y + uv_se.y - uv_se.x);
	    float sd = ds * div;
	
	    vec2 norm = normalize(uv.xy);
	    
	    // temp values for the update rule
	    float ta = amp * uv.x + ls * lapl.x + norm.x * sp + uv.x * sd;
	    float tb = amp * uv.y + ls * lapl.y + norm.y * sp + uv.y * sd;
	
	    // rotate
	    float a = ta * cos(sc) - tb * sin(sc);
	    float b = ta * sin(sc) + tb * cos(sc);
	    
	    // initialize with noise
	    if(FRAMEINDEX<10 || reset) {
	        gl_FragColor = -0.5 + IMG_NORM_PIXEL(iChannel1,mod(gl_FragCoord.xy / RENDERSIZE.xy,1.0));
	    } else {
	        gl_FragColor = clamp(vec4(a,b,div,1), -1., 1.);
	    }
	    
	
	}
	else if (PASSINDEX == 1)	{


	    vec2 texel = 1. / RENDERSIZE.xy;
	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    vec3 components = IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).xyz;
	    vec3 norm = normalize(components);
	    gl_FragColor = vec4(0.5 + norm.z);
	}

}

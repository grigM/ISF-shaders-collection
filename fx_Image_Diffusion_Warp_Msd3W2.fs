/*
{
  "IMPORTED" : {
    
    "iChannel1" : {
      "NAME" : "iChannel1",
      "PATH" : "f735bee5b64ef98879dc618b016ecf7939a5756040c2cde21ccb15e69a6e1cfb.png"
    }
  },
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Msd3W2 by cornusammonis.  Image warping using diffusion.",
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    },
    {
    		"NAME" : "show_bueffer_a",
      		"TYPE" : "bool",
      		"DEFAULT": false,
    },
    {
      "NAME" : "reset",
      "TYPE" : "event"
    },
    {
      "NAME" : "amp",
      "TYPE" : "float",
      "MAX" : 1.1,
      "DEFAULT" : 1,
      "MIN" : 0.9
    },
    {
      "NAME" : "pwr",
      "TYPE" : "float",
      "MAX" : 1.5,
      "DEFAULT" : 1,
      "MIN" : 0.5
    },
    {
      "NAME" : "sq2",
      "TYPE" : "float",
      "MAX" : 1.3999999999999999,
      "DEFAULT" : 0.69999998807907104,
      "MIN" : 0
    },
    {
      "NAME" : "is",
      "TYPE" : "float",
      "MAX" : 0.10000000000000001,
      "DEFAULT" : 0.0099999997764825821,
      "MIN" : -0.10000000000000001
    },
    {
      "NAME" : "ds",
      "TYPE" : "float",
      "MAX" : 0.98999999999999999,
      "DEFAULT" : 0.05000000074505806,
      "MIN" : 0
    },
    {
      "NAME" : "ps",
      "TYPE" : "float",
      "MAX" : 0.10000000000000001,
      "DEFAULT" : -0.05000000074505806,
      "MIN" : -0.10000000000000001
    },
    {
      "NAME" : "ls",
      "TYPE" : "float",
      "MAX" : 0.4,
      "DEFAULT" : 0.3,
      "MIN" : 0.001
    },
    {
      "NAME" : "cs",
      "TYPE" : "float",
      "DEFAULT" : -0.1,
      "MAX" : 0.5,
      "MIN" : -0.5
    },
    
    {
      "NAME" : "_K2",
      "TYPE" : "float",
      "DEFAULT" : 0.166666667,
      "MAX" : 0.2,
      
      "MIN" : 0.1
    },
     {
      "NAME" : "_K1",
      "TYPE" : "float",
      "DEFAULT" : 0.66667,
      "MAX" : 0.7,
      "MIN" : 0.62
    },
     {
      "NAME" : "_K0",
      "TYPE" : "float",
      "DEFAULT" : -3.3333333333,
      "MAX" : -3.2,
      "MIN" : -3.4
    },
     {
      "NAME" : "sobel_size_x",
      "TYPE" : "float",
      "DEFAULT" : 3.0,
      "MAX" : 100.0,
      "MIN" : -100.0
    },
     {
      "NAME" : "sobel_size_y",
      "TYPE" : "float",
      "DEFAULT" : 3.0,
      "MAX" : 100.0,
      "MIN" : -100.0
    },
    
    
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
    
    
  ],
  "PASSES" : [
    {
      "FLOAT" : true,
      "PERSISTENT" : true,
      "TARGET" : "BufferA"
    },
    {
      "FLOAT" : true,
      "PERSISTENT" : true,
      "TARGET" : "BufferB"
    },
    {

    }
  ],
  "ISFVSN" : "2"
}
*/


void main() {
	if (PASSINDEX == 0)	{


	    //const float _K0 = -3.3333333333;//-20.0/6.0; // center weight
	    //const float _K1 = 4.0/6.0; // edge-neighbors
	    //const float _K2 = 0.166666667;//1.0/6.0; // vertex-neighbors
	    //const float cs = -0.1; // curl scale
	    //const float ls = 0.3; // laplacian scale
	    //const float ps = -0.05; // laplacian of divergence scale
	    //const float ds = 0.05; // divergence scale
	    //const float is = 0.01; // image derivative scale
	    //const float pwr = 1.0; // power when deriving rotation angle from curl
	    //const float amp = 1.0; // self-amplification
	    //const float sq2 = 0.7; // diagonal weight
	
	    vec2 vUv = gl_FragCoord.xy / RENDERSIZE.xy;
	    vec2 texel = 1. / RENDERSIZE.xy;
	    
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
	    
	    // sobel filter
	    vec3 im = IMG_NORM_PIXEL(BufferB,mod(vUv,1.0)).xyz;
	    vec3 im_n = IMG_NORM_PIXEL(BufferB,mod(vUv+n,1.0)).xyz;
	    vec3 im_e = IMG_NORM_PIXEL(BufferB,mod(vUv+e,1.0)).xyz;
	    vec3 im_s = IMG_NORM_PIXEL(BufferB,mod(vUv+s,1.0)).xyz;
	    vec3 im_w = IMG_NORM_PIXEL(BufferB,mod(vUv+w,1.0)).xyz;
	    vec3 im_nw = IMG_NORM_PIXEL(BufferB,mod(vUv+nw,1.0)).xyz;
	    vec3 im_sw = IMG_NORM_PIXEL(BufferB,mod(vUv+sw,1.0)).xyz;
	    vec3 im_ne = IMG_NORM_PIXEL(BufferB,mod(vUv+ne,1.0)).xyz;
	    vec3 im_se = IMG_NORM_PIXEL(BufferB,mod(vUv+se,1.0)).xyz;
	
	    float dx = sobel_size_x * (length(im_e) - length(im_w)) + (length(im_ne) + length(im_se) - length(im_sw) - length(im_nw));
	    float dy = sobel_size_y * (length(im_n) - length(im_s)) + (length(im_nw) + length(im_ne) - length(im_se) - length(im_sw));
	
	    // vector field neighbors
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
	    float ta = amp * uv.x + ls * lapl.x + norm.x * sp + uv.x * sd + is * dx;
	    float tb = amp * uv.y + ls * lapl.y + norm.y * sp + uv.y * sd + is * dy;
	
	    // rotate
	    float a = ta * cos(sc) - tb * sin(sc);
	    float b = ta * sin(sc) + tb * cos(sc);
	    
	    // initialize with noise
	    if(FRAMEINDEX<5 || reset==true) {
	        gl_FragColor = -0.5 + IMG_NORM_PIXEL(inputImage,mod(gl_FragCoord.xy / RENDERSIZE.xy,1.0));
	    } else {
	        gl_FragColor = clamp(vec4(a,b,div,1), -1., 2.);
	    }
	    
	
	}
	else if (PASSINDEX == 1)	{


	    //const float ds = pass2_ds;// 0.4; // diffusion rate
	    float ds = pass2_ds;
	    
	    float darken = pass2_darken;// 0.01; // darkening
	    float D1 = pass2_D1;//0.2;  // edge neighbors
	    float D2 = pass2_D2;//0.05; // vertex neighbors
	    
	    vec2 vUv = gl_FragCoord.xy / RENDERSIZE.xy;
	    vec2 texel = 1. / RENDERSIZE.xy;
	    
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
	    
	    vec3 components = IMG_NORM_PIXEL(BufferA,mod(vUv,1.0)).xyz;
	    
	    float a = components.x;
	    float b = components.y;
	    
	    vec3 im =    IMG_NORM_PIXEL(BufferB,mod(vUv,1.0)).xyz;
	    vec3 im_n =  IMG_NORM_PIXEL(BufferB,mod(vUv+n,1.0)).xyz;
	    vec3 im_e =  IMG_NORM_PIXEL(BufferB,mod(vUv+e,1.0)).xyz;
	    vec3 im_s =  IMG_NORM_PIXEL(BufferB,mod(vUv+s,1.0)).xyz;
	    vec3 im_w =  IMG_NORM_PIXEL(BufferB,mod(vUv+w,1.0)).xyz;
	    vec3 im_nw = IMG_NORM_PIXEL(BufferB,mod(vUv+nw,1.0)).xyz;
	    vec3 im_sw = IMG_NORM_PIXEL(BufferB,mod(vUv+sw,1.0)).xyz;
	    vec3 im_ne = IMG_NORM_PIXEL(BufferB,mod(vUv+ne,1.0)).xyz;
	    vec3 im_se = IMG_NORM_PIXEL(BufferB,mod(vUv+se,1.0)).xyz;
	
	    float D1_e = D1 * a;
	    float D1_w = D1 * -a;
	    float D1_n = D1 * b;
	    float D1_s = D1 * -b;
	    float D2_ne = D2 * (b + a);
	    float D2_nw = D2 * (b - a);
	    float D2_se = D2 * (a - b);
	    float D2_sw = D2 * (- a - b);
	
	    vec3 diffusion_im = -darken * length(vec2(a, b)) * im + im_n*D1_n + im_ne*D2_ne + im_e*D1_e + im_se*D2_se + im_s*D1_s + im_sw*D2_sw + im_w*D1_w + im_nw*D2_nw;
	
	    // initialize with image
	    //if(FRAMEINDEX<10 || reset==true) {
	    if(FRAMEINDEX<5 || reset==true) {
	        gl_FragColor = IMG_NORM_PIXEL(inputImage,mod(vUv,1.0));
	    } else {
	        gl_FragColor = vec4(clamp(im + ds * diffusion_im, 0.0, 1.0), 0.0);
	    }
	}
	else if (PASSINDEX == 2)	{


	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    vec3 im = IMG_NORM_PIXEL(BufferB,mod(uv,1.0)).xyz;
	    if(show_bueffer_a){
	    	gl_FragColor = 0.5 + 0.5 * IMG_NORM_PIXEL(BufferA,mod(uv,1.0));
	    }else{
	    	gl_FragColor = vec4(im, 1.0);
	    }
	    
	}

}

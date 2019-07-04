/*{
	"CREDIT": "by joshpbatty",
	"DESCRIPTION": "Feedback GLSL",
	"CATEGORIES": [
		"Joshua Batty"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "bInvert",
			"TYPE": "bool",
			"DEFAULT": 1.0
		},
		{
			"NAME": "bRotate",
			"TYPE": "bool",
			"DEFAULT": 0.0
		},
		{
			"NAME": "rotate_speed",
			"TYPE": "float",
			"DEFAULT": 0.61,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 0.26,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "x_offset",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "y_offset",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "feedback_area",
			"TYPE": "float",
			"DEFAULT": 0.99,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "feedback_sin_speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "feedback_sin_amp",
			"TYPE": "float",
			"DEFAULT": 0.01,
			"MIN": 0.0,
			"MAX": 1.0
		}
		
	],
	"PASSES": [
    {
      "TARGET": "RenderBufferA",
      "PERSISTENT": true,
      "WIDTH": "$WIDTH",
      "HEIGHT": "$HEIGHT",
      "DESCRIPTION": "Pass 0"
    },
    {
      "TARGET": "RenderBufferB",
      "PERSISTENT": true,
      "WIDTH": "$WIDTH",
      "HEIGHT": "$HEIGHT",
      "DESCRIPTION": "Pass 1"
    },
    {
      "TARGET": "RenderBufferC",
      "PERSISTENT": true,
      "WIDTH": "$WIDTH",
      "HEIGHT": "$HEIGHT",
      "DESCRIPTION": "Pass 2"
    }
	]
}*/

//bool bInvert = false;
//bool bRotate = false;

//float feedback_sin_speed = 1.0;
//float feedback_sin_amp = 0.01;
//float zoom = .260;
//float feedback_area = 0.99496;
//float rotate_speed = 0.61;
//float x_offset = 0.5 + sin(TIME*0.4)*0.25;
//float y_offset = 0.5 + cos(TIME*0.3)*0.25;

float remap( float value, float inMin, float inMax, float outMin, float outMax ) {
    return ( (value - inMin) / ( inMax - inMin ) * ( outMax - outMin ) ) + outMin; 
}

mat2 rotate(float angle) {
    return mat2(cos(angle), -sin(angle),
                sin(angle), cos(angle));
}

void main() {
	
		if (PASSINDEX==0) {
			///---------------- Grab a texture and remove green screen 
			vec2 uv = isf_FragNormCoord.xy;
    		vec4 bg = vec4(sin(TIME+uv.x),sin((TIME+uv.x)*2.),sin((TIME+uv.x)*3.),1.);
    		vec4 texture1 = IMG_NORM_PIXEL(inputImage, uv);
    		float greens = clamp(clamp(((texture1.g-texture1.r)-texture1.b),0.,1.)*10.,0.,1.);
    		bg = vec4(1.0,1.0,1.0,1.0);
			gl_FragColor= mix(texture1,bg,greens);
		}
		 else if (PASSINDEX==1) {
			///---------------- FEEDBACK CODE
    		float offset_x = remap(x_offset,0.0,1.0,-0.05,0.05);
	  	    float offset_y = remap(y_offset,0.0,1.0,-0.05,0.05);
    	    vec2 uv = (isf_FragNormCoord.xy) * 2.0 - vec2(1.0 - offset_x, 1.0 - offset_y);
    		vec2 uv2 = isf_FragNormCoord.xy * 1.0; // Original non transfored coords
    		uv.x *= RENDERSIZE.x / RENDERSIZE.y;
  	 		// uv.x += sin(uv.y*10.0 + iGlobalTime*2.135)/100.0;
    		if(bRotate) uv *= rotate(TIME * (rotate_speed*0.1));
    		vec2 uv_b = uv*0.5;
  	 		// uv_b += sin(uv*10.0 + iGlobalTime)/10.0;
    		vec4 col;
    		if(length(uv_b) < feedback_area) {
        		vec2 coord = uv;
        		coord *= 1.09 - (0.1 + (remap(zoom,0.0,1.0,-0.20,0.30) * 0.5)) + sin(TIME*feedback_sin_speed)*feedback_sin_amp;
    			coord.x /= RENDERSIZE.x / RENDERSIZE.y;
        		coord = (coord + 1.0)/2.0;
	    		col = IMG_NORM_PIXEL(RenderBufferB, coord, 0.1) + 0.01;
             
        		// Our buffer which has the shape
	    		col *= IMG_NORM_PIXEL(RenderBufferA, uv2);

		        float color_shift = TIME*0.04;
        		//col.xy *= rotate(1.8 + color_shift);
        		//col.yz *= rotate(-0.3 + color_shift);
    			//col.zx *= rotate(1.6 + color_shift);
        		//col = pow(abs(col),vec4(1.001));
    		} else {
        		// Our buffer which has the shape
	    		col = IMG_NORM_PIXEL(RenderBufferA, uv2);

		        //col = vec4(pow(abs(length(uv_b)-0.61),0.1));
        		//col *= vec4(0.5,1.0,0.2,1.0);
    		}
		    if(!bInvert){
    			gl_FragColor = col;
    		} else {
			 	gl_FragColor = 1.0-col; //Insane feedback! 
    		}
		 } 
  	 	 else if (PASSINDEX==2) {
			///---------------- FINAL OUTPUT IMAGE
		 	gl_FragColor = pow(abs(IMG_NORM_PIXEL(RenderBufferB, isf_FragNormCoord.xy)),vec4(0.7));
	  	 }

}
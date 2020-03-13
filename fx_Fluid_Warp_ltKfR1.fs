/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/ltKfR1 by wyatt.  Voronoi particle system advected through my trusty fluid algorithm :)\n\n(each pixel knows about the closest particle and asks its neighbors about new closest particles)\n\npretty fun with webcam",
    "IMPORTED": {
    },
    "INPUTS": [
    	{
      "TYPE" : "image",
      "NAME" : "inputImage"
    	},
    	
        
    ],
    "PASSES": [
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferA"
        },
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferB"
        },
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferC"
        },
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferD"
        },
        {
        }
    ]
}

*/


vec2 R;

vec4 T_B ( vec2 U ) {return IMG_NORM_PIXEL(BufferB,mod(U/R,1.0));}
vec4 T_A ( vec2 U ) {return IMG_NORM_PIXEL(BufferA,mod(U/R,1.0));}

vec4 T_D ( vec2 U ) {return IMG_NORM_PIXEL(BufferD,mod(U/R,1.0));}




float X_B (vec2 U, vec2 u, inout vec4 Q, in vec2 r) {
    vec2 v = T_B(U+r).xy;
    float P = T_B(U+r-v).z;
    Q.xy -= 0.25*r*(P-Q.z);
    return (0.5*(length(r-v+u)-length(r+v-u))+P);
}




float X_A (vec2 U, vec2 u, inout vec4 Q, in vec2 r) {
    vec2 v = T_A(U+r).xy;
    float P = T_A(U+r-v).z;
    Q.xy -= 0.25*r*(P-Q.z);
    return (0.5*(length(r-v+u)-length(r+v-u))+P);
}

// Voronoi based particle tracking
// Change BufferC texture filtering to nearest to see
//   the tracked particles
//   With linear filtering, the particles can actually multiply
//   this way the space is always filled

float N;

vec4 P_C ( vec2 U ) {return IMG_NORM_PIXEL(BufferC,mod(U/R,1.0));}
vec4 P_D ( vec2 U ) {return IMG_NORM_PIXEL(BufferD,mod(U/R,1.0));}

void swap (vec2 U, inout vec4 Q, vec2 u) {
    vec4 p = P_C(U+u);
    if (length(U-Q.xy) > length(U-p.xy)) Q = p;
}


//Render particles








void main() {
	if (PASSINDEX == 0)	{
    
     	R = RENDERSIZE.xy;

	 	vec2 u = T_B(gl_FragCoord.xy).xy, e = vec2(1,0);
	 	float P = 0.; 
	 	gl_FragColor = T_B(gl_FragCoord.xy-u);
	 	gl_FragColor.z = 0.25*(
	       X_B (gl_FragCoord.xy,u,gl_FragColor, e)+
	 	   X_B (gl_FragCoord.xy,u,gl_FragColor,-e)+
	 	   X_B (gl_FragCoord.xy,u,gl_FragColor, e.yx)+
	 	   X_B (gl_FragCoord.xy,u,gl_FragColor,-e.yx));
	 	if (FRAMEINDEX < 1) gl_FragColor = vec4(0);
	    if (gl_FragCoord.x < 1.||gl_FragCoord.y < 1.||R.x-gl_FragCoord.x < 1.||R.y-gl_FragCoord.y < 1.) gl_FragColor.xy *= 0.;
	 	if ( mod(float(FRAMEINDEX),float(1000))<50.0 && length(gl_FragCoord.xy-vec2(0.5,0.1)*R) < 0.04*R.y) {gl_FragColor.xy= gl_FragColor.xy*.99+.01*vec2(0,0.1*R.y);; gl_FragColor.w = 1.;}
	 	if ( mod(float(FRAMEINDEX+500),1000.0)<50.0 && length(gl_FragCoord.xy-vec2(0.5,0.9)*R) < 0.04*R.y) {gl_FragColor.xy= gl_FragColor.xy*.99+.01*vec2(0,-0.001*R.y);; gl_FragColor.w = 1.;}
	
	}else if (PASSINDEX == 1)	{
   		R = RENDERSIZE.xy;

	 	vec2 u = T_A(gl_FragCoord.xy).xy, e = vec2(1,0);
	 	float P = 0.; 
	 	gl_FragColor = T_A(gl_FragCoord.xy-u);
	 	
	 	gl_FragColor.z = 0.25*(
	       X_A (gl_FragCoord.xy,u,gl_FragColor, e)+
	 	   X_A (gl_FragCoord.xy,u,gl_FragColor,-e)+
	 	   X_A (gl_FragCoord.xy,u,gl_FragColor, e.yx)+
	 	   X_A (gl_FragCoord.xy,u,gl_FragColor,-e.yx));
	 	if (FRAMEINDEX < 1) gl_FragColor = vec4(0);
	    if (gl_FragCoord.x < 1.||gl_FragCoord.y < 1.||R.x-gl_FragCoord.x < 1.||R.y-gl_FragCoord.y < 1.) gl_FragColor.xy *= 0.;
	 	if ( mod(float(FRAMEINDEX),1000.0)<50.0 && length(gl_FragCoord.xy-vec2(0.5,0.1)*R) < 0.04*R.y) {gl_FragColor.xy= gl_FragColor.xy*.99+.01*vec2(0,0.1*R.y);; gl_FragColor.w = 1.;}
	 	if ( mod(float(FRAMEINDEX)+500.0,1000.0)<50.0 && length(gl_FragCoord.xy-vec2(0.5,0.9)*R) < 0.04*R.y) {gl_FragColor.xy= gl_FragColor.xy*.99+.01*vec2(0,-0.001*R.y);; gl_FragColor.w = 1.;}
	}
	else if (PASSINDEX == 2)	{
   		R = RENDERSIZE.xy;
		vec2 uv = gl_FragCoord.xy;
		
	 	uv.xy -= T_A(gl_FragCoord.xy).xy;
	 	uv.xy -= T_A(gl_FragCoord.xy).xy;
	 	
	 	gl_FragColor = P_C(uv.xy);
	 	swap(uv.xy,gl_FragColor,vec2(1,0));
	 	swap(uv.xy,gl_FragColor,vec2(0,1));
	 	swap(uv.xy,gl_FragColor,vec2(0,-1));
	 	swap(uv.xy,gl_FragColor,vec2(-1,0));
	 
	 	gl_FragColor.xy = fract((gl_FragColor.xy + T_A(gl_FragColor.xy).xy)/R)*R;
	 	gl_FragColor.xy = fract((gl_FragColor.xy + T_A(gl_FragColor.xy).xy)/R)*R;
	 	if (FRAMEINDEX < 1) {
	        vec2 u = uv.xy;
	 		gl_FragColor = vec4(u,u);
	 	}
	}
	else if (PASSINDEX == 3)	{
		R = RENDERSIZE.xy;

	    gl_FragColor = P_C(gl_FragCoord.xy);
		vec2 
	        n = P_C(gl_FragCoord.xy+vec2(0,1)).zw,
	        e = P_C(gl_FragCoord.xy+vec2(1,0)).zw,
	        s = P_C(gl_FragCoord.xy-vec2(0,1)).zw,
	        w = P_C(gl_FragCoord.xy-vec2(1,0)).zw;
	 	 
	 	 
	 	 
    
	
	
	 	gl_FragColor = vec4(T_D(gl_FragCoord.xy).xyz*0.1+0.9*IMG_NORM_PIXEL(inputImage,gl_FragColor.zw/R).xyz*smoothstep(2.,1.,length(gl_FragColor.xy-gl_FragCoord.xy)),
	       (length(n-gl_FragColor.zw)-1.+
	        length(e-gl_FragColor.zw)-1.+
	        length(s-gl_FragColor.zw)-1.+
	        length(w-gl_FragColor.zw)-1.)/R.y*20. 
	            );
	 	if(FRAMEINDEX < 1) gl_FragColor = vec4(0);
	}
	else if (PASSINDEX == 4)	{


	    R = RENDERSIZE.xy;
	    gl_FragColor = P_D(gl_FragCoord.xy);
	    gl_FragColor = gl_FragColor*(0.5+0.5*cos(gl_FragColor.w*vec4(1,2,3,4)));
	}

}

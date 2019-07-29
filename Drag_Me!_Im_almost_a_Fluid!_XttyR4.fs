

/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/XttyR4 by wyatt.  It has some weird behavior, but it also has some fluidic behavior",
    "IMPORTED": {
    },
    "INPUTS": [
        {
            "NAME": "iMouse",
            "TYPE": "point2D",
             "DEFAULT": [
				0,
				0
			]
        },
        
        
    
    
        
      	{
			"NAME": "autoStartDraw",
			"TYPE": "bool",
			"DEFAULT": true
		},
			
        {
            "NAME": "reset",
            "TYPE": "event"
        },
        
        {
			"NAME": "isDrag",
			"TYPE": "bool",
			"DEFAULT": true
		},
		{
      		"NAME": "ne_size",
      		"MAX": 0.5,
      		"MIN": 0.05,
      		"DEFAULT": 0.25,
      		"TYPE": "float"
    	}
    	,
		{
      		"NAME": "velocity_mix_1",
      		"MAX": 2.2,
      		"MIN": 0.0,
      		"DEFAULT": 0.06,
      		"TYPE": "float"
    	},
    	
    	{
      		"NAME": "velocity_mix_2",
      		"MAX": 2.2,
      		"MIN": 0.0,
      		"DEFAULT": 0.06,
      		"TYPE": "float"
    	},
    	{
      		"NAME": "velocity_mix_3",
      		"MAX": 2.0,
      		"MIN": 0.0,
      		"DEFAULT": 1.0,
      		"TYPE": "float"
    	},
    	{
      		"NAME": "line_segment_lenth",
      		"MAX": 30.0,
      		"MIN": 0.0,
      		"DEFAULT": 1.0,
      		"TYPE": "float"
    	},
    	
    	{
      		"NAME": "line_intens",
      		"MAX": 1.0,
      		"MIN": 0.02,
      		"DEFAULT": 0.03,
      		"TYPE": "float"
    	},
    	
    	{
      		"NAME": "line_velocity",
      		"MAX": 40.0,
      		"MIN": 0.0,
      		"DEFAULT": 20.0,
      		"TYPE": "float"
    	},
    	
    	
    	{
      		"NAME": "line_gradient_repeat",
      		"MAX": 10.0,
      		"MIN": 0.0,
      		"DEFAULT": 2.0,
      		"TYPE": "float"
    	}
    	,
    	{
      		"NAME": "glow_rg_hue",
      		"MAX": 255.0,
      		"MIN": 0.0,
      		"DEFAULT": 20.0,
      		"TYPE": "float"
    	},
    	
    	
    	{
      		"NAME": "ripple",
      		"MAX": 2.0,
      		"MIN": 0.5,
      		"DEFAULT": 0.6,
      		"TYPE": "float"
    	}, 
    	
    	
    	{
      		"NAME": "mix_0",
      		"MAX": 0.8,
      		"MIN": -0.09,
      		"DEFAULT": 0.0,
      		"TYPE": "float"
    	},
      	
    	
    	
    	
    	{
			"NAME": "backIsAlpha",
			"TYPE": "bool",
			"DEFAULT": false
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








/*
///// TMP
{
            "NAME": "speed",
            "TYPE": "float",
            "DEFAULT": 0.4,
            "MIN": 0.0,
            "MAX": 3.0
    },
    {
            "NAME": "rad_ofset",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": 0.0,
            "MAX": 3.0
    },
    {
            "NAME": "max_rad",
            "TYPE": "float",
            "DEFAULT": 0.4,
            "MIN": 0.05,
            "MAX": 3.0
    },

*/

// FLUID PART

vec2 ur;
vec2 U;

float ln (vec2 p, vec2 a, vec2 b) {
    return length(p-a-(b-a)*clamp(dot(p-a,b-a)/dot(b-a,b-a),0.0,line_segment_lenth));
}
float area (vec2 a, vec2 b, vec2 c) { // area formula of a triangle from edge lengths
    float A = length(b-c), B = length(c-a), C = length(a-b), s = 0.5*(A+B+C);
    return sqrt(s*(s-A)*(s-B)*(s-C));
}



vec4 t_BufferD (vec2 v, int a, int b) {return IMG_NORM_PIXEL(BufferD,mod(fract((v+vec2(a,b))/ur),1.0));}
vec4 t_BufferD_2 (vec2 v) {return IMG_NORM_PIXEL(BufferD,mod(fract(v/ur),1.0));}

// MOUSE
// FLUID PART



vec4 t_BufferA (vec2 v, int a, int b) {return IMG_NORM_PIXEL(BufferA,mod(fract((v+vec2(a,b))/ur),1.0));}
vec4 t_BufferA_2 (vec2 v) {return IMG_NORM_PIXEL(BufferA,mod(fract(v/ur),1.0));}

// FLUID PART



vec4 t_BufferC (vec2 v, int a, int b) {return IMG_NORM_PIXEL(BufferC,mod(fract((v+vec2(a,b))/ur),1.0));}
vec4 t_BufferC_2 (vec2 v) {return IMG_NORM_PIXEL(BufferC,mod(fract(v/ur),1.0));}

/*

	So a litte bit ago I made this:	
		https://www.shadertoy.com/view/lsVfRd

	but I forgiot the convective term...

	I didnt realize the velocity advects through itself
	kinda trippy tbh

	fluids are like multi-dimensional infinitesimal newton's cradles
	
	


*/



void main() {
	
	
	U = gl_FragCoord.xy;
	ur = RENDERSIZE.xy;
	if (PASSINDEX == 0)	{


	    
	    if (FRAMEINDEX < 1 || reset) {
	        // INIT
	        if(autoStartDraw){
	        	float q = length(U-0.5*ur);
	        	// make a small right pointing velocity in the middle
	        	gl_FragColor = vec4(0.1*exp(-0.01*q*q),0,0,2.*exp(-0.01*q*q));
	        }
	    } else {
	        // start where the pixel is and make a box around it
	        vec2 v = U,
	             A = v + vec2( 1, 1),
	             B = v + vec2( 1,-1),
	             C = v + vec2(-1, 1),
	             D = v + vec2(-1,-1);
	        // ADVECT TO LEARN FROM THE PAST
	        for (int i = 0; i < 5; i++) {
	            // add the velocity at each position to the position
	            v -= t_BufferD_2(v).xy;
	            A -= t_BufferD_2(A).xy;
	            B -= t_BufferD_2(B).xy;
	            C -= t_BufferD_2(C).xy;
	            D -= t_BufferD_2(D).xy;
	        }
	        // find out where and what the pixel is now and what its neighbors were doing last frame
	        vec4 me = t_BufferD(v,0,0);
	        vec4 n = t_BufferD(v,0,1),
	            e = t_BufferD(v,1,0),
	            s = t_BufferD(v,0,-1),
	            w = t_BufferD(v,-1,0);
	        //average the neighbors to allow values to blend
	        vec4 ne = ne_size*(n+e+s+w);
	        // mix the velocity and pressure from neighboring cells
			me = mix(t_BufferD_2(v),ne,vec4(velocity_mix_1,velocity_mix_2,velocity_mix_3, mix_0));
	        // add the change in the area of the advected box to the pressure
	        me.z  = me.z  - ((area(A,B,C)+area(B,C,D))-4.);
			
	        // PRESSURE GRADIENT
	            vec4 pr = vec4(e.z,w.z,n.z,s.z);
	        	// add the pressure gradient to the velocity
	            me.xy = me.xy + vec2(pr.x-pr.y, pr.z-pr.w)/ur;
	        // MOUSE MOVEMENT
	        	vec4 mouse = IMG_NORM_PIXEL(BufferB,mod(vec2(0.5),1.0));
	            float q = ln(U,mouse.xy,mouse.zw);
	            vec2 m = mouse.xy-mouse.zw;
	            float l = length(m);
	            if (l>0.) m = min(l,10.)*m/l;
	        	// add a line from the mouse to the velocity field and add some color
	            me.xyw += line_intens*exp(-6e-2*q*q*q)*vec3(m,line_velocity);
	        gl_FragColor = me;
	        gl_FragColor.xyz = clamp(gl_FragColor.xyz, 0.0-ripple, ripple);
	    }
	}
	else if (PASSINDEX == 1)	{


	    //
	 /*   
	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	
    vec2 coords = (uv-iMouse);						//Remap uv to cartesian coordinates. -1 to 1
    coords.x *= RENDERSIZE.x / RENDERSIZE.y;			//Account for aspect ratio
	
	
	//vec2 offsetMoon = vec2(iMouse.x,iMouse.y);
	//color -= fill(circleSDF(st-offsetMoon),.5);
	
	

	gl_FragColor = vec4(0.0,1.0,0.0,1.0);					//Set the background to green

	float l_time = fract((TIME / 0.75)*speed);			//Calculate a 0-1 time ... In this case it repeats every 0.75 seconds
    
	if(length(coords) > (l_time * max_rad)+rad_ofset)  			//Set to black if less than 
    {
		gl_FragColor = vec4(0.0,0.0,0.0,1.0);
    }    
    */
    
    	
    vec4 p = IMG_NORM_PIXEL(BufferB,mod(gl_FragCoord.xy/RENDERSIZE.xy,10.0));
	    if (isDrag)  {
	        if (p.z>0.){
	        	gl_FragColor =  vec4(iMouse.xy,p.xy);
	        }else{
	        	 gl_FragColor =  vec4(iMouse.xy,iMouse.xy);
	        }
	    } else{
	     	gl_FragColor = vec4(-RENDERSIZE.xy,-RENDERSIZE.xy);
	    }
	    
	   	}
	else if (PASSINDEX == 2)	{


	   
	    if (FRAMEINDEX < 1 || reset) {
	        // INIT
	        if(autoStartDraw){
	        	float q = length(U-0.5*ur);
	        	// make a small right pointing velocity in the middle
	        	gl_FragColor = vec4(0.1*exp(-0.01*q*q),0,0,2.*exp(-0.01*q*q));
	        }
	    } else {
	        // start where the pixel is and make a box around it
	        vec2 v = U,
	             A = v + vec2( 1, 1),
	             B = v + vec2( 1,-1),
	             C = v + vec2(-1, 1),
	             D = v + vec2(-1,-1);
	        // ADVECT TO LEARN FROM THE PAST
	        for (int i = 0; i < 5; i++) {
	            // add the velocity at each position to the position
	            v -= t_BufferA_2(v).xy;
	            A -= t_BufferA_2(A).xy;
	            B -= t_BufferA_2(B).xy;
	            C -= t_BufferA_2(C).xy;
	            D -= t_BufferA_2(D).xy;
	        }
	        // find out where and what the pixel is now and what its neighbors were doing last frame
	        vec4 me = t_BufferA(v,0,0);
	        vec4 n = t_BufferA(v,0,1),
	            e = t_BufferA(v,1,0),
	            s = t_BufferA(v,0,-1),
	            w = t_BufferA(v,-1,0);
	        //average the neighbors to allow values to blend
	        vec4 ne = ne_size*(n+e+s+w);
	        // mix the velocity and pressure from neighboring cells
			me = mix(t_BufferA_2(v),ne,vec4(velocity_mix_1,velocity_mix_2,velocity_mix_3,mix_0));
	        // add the change in the area of the advected box to the pressure
	        me.z  = me.z  - ((area(A,B,C)+area(B,C,D))-4.);
			
	        // PRESSURE GRADIENT
	            vec4 pr = vec4(e.z,w.z,n.z,s.z);
	        	// add the pressure gradient to the velocity
	            me.xy = me.xy + vec2(pr.x-pr.y, pr.z-pr.w)/ur;
	        // MOUSE MOVEMENT
	        	vec4 mouse = IMG_NORM_PIXEL(BufferB,mod(vec2(0.5),1.0));
	            float q = ln(U,mouse.xy,mouse.zw);
	            vec2 m = mouse.xy-mouse.zw;
	            float l = length(m);
	            if (l>0.) m = min(l,10.)*m/l;
	        	// add a line from the mouse to the velocity field and add some color
	            me.xyw += line_intens*exp(-6e-2*q*q*q)*vec3(m,line_velocity);
	        gl_FragColor = me;
	        gl_FragColor.xyz = clamp(gl_FragColor.xyz,  0.0-ripple, ripple);
	    }
	}
	else if (PASSINDEX == 3)	{


	   
	    if (FRAMEINDEX < 1 || reset) {
	        // INIT
	        if(autoStartDraw){
	        	float q = length(U-0.5*ur);
	        	// make a small right pointing velocity in the middle
	       		gl_FragColor = vec4(0.1*exp(-0.01*q*q),0,0,2.*exp(-0.01*q*q));
	        }
	    } else {
	        // start where the pixel is and make a box around it
	        vec2 v = U,
	             A = v + vec2( 1, 1),
	             B = v + vec2( 1,-1),
	             C = v + vec2(-1, 1),
	             D = v + vec2(-1,-1);
	        // ADVECT TO LEARN FROM THE PAST
	        for (int i = 0; i < 5; i++) {
	            // add the velocity at each position to the position
	            v -= t_BufferC_2(v).xy;
	            A -= t_BufferC_2(A).xy;
	            B -= t_BufferC_2(B).xy;
	            C -= t_BufferC_2(C).xy;
	            D -= t_BufferC_2(D).xy;
	        }
	        // find out where and what the pixel is now and what its neighbors were doing last frame
	        vec4 me = t_BufferC(v,0,0);
	        vec4 n = t_BufferC(v,0,1),
	            e = t_BufferC(v,1,0),
	            s = t_BufferC(v,0,-1),
	            w = t_BufferC(v,-1,0);
	        //average the neighbors to allow values to blend
	        vec4 ne = ne_size*(n+e+s+w);
	        // mix the velocity and pressure from neighboring cells
			me = mix(t_BufferC_2(v),ne,vec4(velocity_mix_1,velocity_mix_2,velocity_mix_3,mix_0));
	        // add the change in the area of the advected box to the pressure
	        me.z  = me.z  - ((area(A,B,C)+area(B,C,D))-4.);
			
	        // PRESSURE GRADIENT
	            vec4 pr = vec4(e.z,w.z,n.z,s.z);
	        	// add the pressure gradient to the velocity
	            me.xy = me.xy + vec2(pr.x-pr.y, pr.z-pr.w)/ur;
	        // MOUSE MOVEMENT
	        	vec4 mouse = IMG_NORM_PIXEL(BufferB,mod(vec2(0.5),1.0));
	            float q = ln(U,mouse.xy,mouse.zw);
	            vec2 m = mouse.xy-mouse.zw;
	            float l = length(m);
	            if (l>0.) m = min(l,10.)*m/l;
	        	// add a line from the mouse to the velocity field and add some color
	            me.xyw += line_intens*exp(-6e-2*q*q*q)*vec3(m,line_velocity);
	        gl_FragColor = me;
	        gl_FragColor.xyz = clamp(gl_FragColor.xyz, 0.0-ripple, ripple);
	    }
	}
	else if (PASSINDEX== 4)	{


	   	vec4 g = IMG_NORM_PIXEL(BufferA,mod(gl_FragCoord.xy/RENDERSIZE.xy,1.0));
	    vec2 d = vec2(
	    	IMG_NORM_PIXEL(BufferA,mod((gl_FragCoord.xy+vec2(1,0))/RENDERSIZE.xy,1.0)).w-IMG_NORM_PIXEL(BufferA,mod((gl_FragCoord.xy-vec2(1,0))/RENDERSIZE.xy,1.0)).w,
	    	IMG_NORM_PIXEL(BufferA,mod((gl_FragCoord.xy+vec2(0,1))/RENDERSIZE.xy,1.0)).w-IMG_NORM_PIXEL(BufferA,mod((gl_FragCoord.xy-vec2(0,1))/RENDERSIZE.xy,1.0)).w
	    );
	    vec3 n = normalize(vec3(d,0.1));
	    float a = acos(dot(n,normalize(vec3(1))))/3.141593;
		g.w = line_gradient_repeat*sqrt(g.w);
	    vec3 color = 1.3*(.5+0.5*sin(abs(g.xyz)*vec3(glow_rg_hue,glow_rg_hue,1)+2.*g.w*vec3(sin(g.w),cos(g.w*2.),3)))*(abs(a)*.5+0.5);
	    if(backIsAlpha){
	    	gl_FragColor = vec4(color*color*1.2, color);
	    }else{
	    	gl_FragColor = vec4(color*color*1.2, 1);
	    }
	}

}

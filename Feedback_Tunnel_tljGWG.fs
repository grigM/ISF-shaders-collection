/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/tljGWG by TekF.  Unwinding after some stress by making something simple and trippy.",
    "IMPORTED": {
    },
    "INPUTS": [
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


void main() {
	if (PASSINDEX == 0)	{


	    // read previous frame with a perspective twist
	    vec2 aspect = RENDERSIZE.xy/sqrt(RENDERSIZE.x*RENDERSIZE.y);
	    vec2 uv = (gl_FragCoord.xy/RENDERSIZE.xy-.5)*aspect;
	    
	    // ray trace a plane at z=0, and map uv on that
	    // this way I can move the camera to control the feedback shape
	    // without adding redundant params for controlling the plane
	    vec3 ray = normalize(vec3(uv,1));
	
	    // adjust camera rotation
	    vec3 rot = .02*sin(TIME*vec3(1.618,1.,.382)*.3);
	    vec2 s = vec2(-1,1);
		#define rot(a) mat2(cos(a),-sin(a),sin(a),cos(a))
	    ray.xy *= rot(rot.z);
	    ray.yz *= rot(rot.x);
	    ray.zx *= rot(rot.y);
	    
		// position camera (position of <0,0,-1> and 0 rotation = direct image)
	    vec3 camPos = vec3(rot.yx*vec2(-1.1,1.1),-1.01);
	    float t = (0.-camPos.z)/ray.z;
	    vec3 pos = camPos + ray*t;
	    uv = pos.xy;
	    
	    gl_FragColor = IMG_NORM_PIXEL(BufferA,mod(uv/aspect+.5,1.0));
	    
	    // draw things on top
	    
	    // box / tunnel wall
	    gl_FragColor.rgb = mix( gl_FragColor.rgb, step(0.,sin(TIME*vec3(16.18,20,1.618*.618*10.))), smoothstep(.02,.015,abs(max(abs(uv.x)-.5*aspect.x,abs(uv.y)-.5*aspect.y))) );
	    // alternative colours:
	//    gl_FragColor.rgb = mix( gl_FragColor.rgb, sin(floor(TIME*vec3(16.18,20,1.618*.618*10.)))*.8+.2, smoothstep(.02,.015,abs(max(abs(uv.x)-.5*aspect.x,abs(uv.y)-.5*aspect.y))) );
	
	    // circle / snake
	    gl_FragColor.rgb = mix( gl_FragColor.rgb, sin(TIME*vec3(1,1.382,1.618)*3.)*.7+.5, smoothstep(.03,.025,length( uv-vec2(.45)*aspect*mix(sin(TIME*vec2(.618,1.382)),sin(TIME*vec2(1.618,2.)),.3) )/(abs(sin(TIME))+.5)) );
		// grey version:
	//    gl_FragColor.rgb = mix( gl_FragColor.rgb, sin(TIME*vec3(10))*.5+.5, smoothstep(.03,.025,length( uv-vec2(.5)*aspect*mix(sin(TIME*vec2(.618,1.382)),sin(TIME*vec2(1.618,2.)),.3) )) );
	    
	    gl_FragColor.rgb = clamp(gl_FragColor.rgb,0.,1.);
	}
	else if (PASSINDEX == 1)	{


		gl_FragColor = IMG_NORM_PIXEL(BufferA, isf_FragNormCoord.xy);
	
	    // linear -> gamma
	    //gl_FragColor.rgb = pow( gl_FragColor.rgb, vec3(1./2.2) );
	}

}

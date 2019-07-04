/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
  {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    },
	{
			"NAME": "size",
			"TYPE": "float",
			"DEFAULT": 15.0,
			"MIN": 0.0,
			"MAX": 30.0
			
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 3.0
			
		},
		{
			"NAME": "anim_ofset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.55
			
		},
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54343.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/3tlGRr
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE

// --------[ Original ShaderToy begins here ]---------- //
//#define SIZE 15.0 
#define HPI 1.5707963 
#define COL1 vec3(0, 0, 0) / 255.0 
#define COL2 vec3(255, 255, 255) / 255.0 
 
void mainImage(out vec4 fragColor, in vec2 fragCoord)
 { 
    vec2 uv = ((fragCoord.xy - iResolution.xy * 0.5) / iResolution.x)-mouse;
    float hsm = 1.5 / iResolution.y * size * 0.5; // Half-Smooth factor
        
    uv *= size; // Make grid
    vec2 id = floor(uv);
    uv = fract(uv) - 0.5;
    
    float angle = iTime*speed-anim_ofset; // Prepare rotation matrix    
    
    float phase = mod(floor(angle / HPI), 2.0); // Determine what phase is right now
    
    float mask = 0.0;
    for(float y =- 1.0; y <= 1.0; y++ ) { // Loop to draw neighbour cells
        for(float x =- 1.0; x <= 1.0; x++ ) {
            vec2 ruv = uv + vec2(x, y);
            vec2 rid = id + vec2(x, y);
                        
            // Golfed Rotation https://www.shadertoy.com/view/XlsyWX
            ruv *= mat2(cos( angle + vec4(0,33,11,0)));
            
            vec2 maskXY = smoothstep(0.5 + hsm, 0.5 - hsm, abs(ruv));            
            float maskI = maskXY.x*maskXY.y;  
            
            vec2 idm = mod(rid, 2.0);
            float draw = abs(idm.x*idm.y + (1.-idm.x)*(1.-idm.y) - phase); // Flip depending on phase            
            
            mask += maskI * draw;
        }
    }
    
    vec3 col = vec3(1.0);
    col = mix(COL1*.0, COL2, abs(mask - phase)); // Color flip depending on phase
    
    fragColor = vec4(col, 1.0);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
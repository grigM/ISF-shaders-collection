/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "3d",
    "plane",
    "holodeck",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XlKSzw by cacheflowe.  Test of a borrowed 3d perspective technique",
  "INPUTS" : [

  ]
}
*/


void main()
{
    float time = TIME;
    vec2 uv = 0.5 * (2. * gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.y;
    
    // warp uv pre-perspective shift
    float displaceAmp = 0.1;
    float displaceFreq = 2.5;
    uv += vec2(displaceAmp * sin(time + uv.x * displaceFreq));
    
    // 3d params
    // 3d plane technique from: http://glslsandbox.com/e#37557.0 
    float horizon = 0.1 * cos(time); 
    float fov = 0.35 + 0.15 * sin(time); 
	float scaling = 0.2;
    // create a 2nd uv with warped perspective
	vec3 p = vec3(uv.x, fov, uv.y - horizon);      
	vec2 s = vec2(p.x/p.z, p.y/p.z) * scaling;
    
    // wobble the perspective-warped uv 
    float oscFreq = 12.;
    float oscAmp = 0.03;
    s += vec2(oscAmp * sin(time + s.x * oscFreq));
	
	// normal drawing here
    // lines/lattice
    float color = max(
        smoothstep(0.2, 0.8, 1. - pow(sin(s.y * 100.), 0.28)), 
        smoothstep(0.2, 0.8, 1. - pow(sin(s.x * 100.), 0.28))
    );

	// fade into distance
	color *= p.z * p.z * 10.0;
    // create holodeck yellow with color value
	gl_FragColor = vec4( vec3(color, color, 0), 1.0 );
}
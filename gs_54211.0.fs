/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54211.0"
}
*/


#ifdef GL_ES
precision highp float;
#endif


// Biting_PacMan_AA.glsl         2017-10-27

// animated PacMan figure with antialiasing
// todo: - walking across the screen
//       - eat pills
//       - adding eye ?  
// play...   https://www.shadertoy.com/view/Ms3XWN
// biting... https://www.shadertoy.com/view/XsVXRh

const vec4 PacManColor = vec4(1.0, 1.0, 0.0, 1.0);
const vec4 PillColor = vec4(1.0, 0.85, 0.67, 1.0);

float PacMan (vec2 uv, float radius) 
{
  // get distance of current coordinate
  float d = length(uv);
	
  // smooth paint if coordinate is inside circle and outside the mouth
  float mouthIntensity = smoothstep(uv.x/d, uv.x/d+0.1, 0.2*sin(TIME*12.)+0.84);
  return smoothstep(d,d+0.01, radius) * mouthIntensity;
}

void main( void ) 
{
float ss = 32.0*sin(TIME);
	vec2 gg = gl_FragCoord.xy;
	gg = ceil(gg / ss) * ss;
		
  // transform pixel to viewport coordinate
  vec2 uv = (2.0 * gg - RENDERSIZE) / RENDERSIZE.y;	
	
	
  // get pacman color intensity
  float intensity = PacMan (uv, 0.7);
	
  // paint pacman
  gl_FragColor = PacManColor * intensity;
}
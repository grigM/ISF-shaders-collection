/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "projection",
    "checker",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XldGzr by BigotedSJW.  [insert tired Flippy from Starfox meme here]",
  "INPUTS" : [

  ]
}
*/


float checker( in vec2 p, in float s );
void main()
{
   	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy - 0.5;
	uv.x *= RENDERSIZE.x / RENDERSIZE.y; //square cooridinated, 0,0 at center
	
    
    float ang = 1.5*TIME;
    vec2 rot=vec2(sin(ang),cos(ang));
    mat2 rm =mat2(rot.x,rot.y,-rot.y,rot.x);
    uv=rm*uv; //rotate input coordinates

    //projection
    vec3 project_in =  vec3(uv.x,  0.5, abs(uv.y));
	vec2 project_out = project_in.xy/ project_in.z + (vec2(0.0,4.0*TIME));
    
    //sample (procedural) texture (poorly, hence artifacting at the 'horizon')
    float tex = checker(project_out,4.0);
    
	gl_FragColor = vec4(vec3(tex),1.0); 
}

float checker( in vec2 p, in float s )
{
	//ivec2 state = ivec2(p * s); //interger binary operations not supported :(
   	vec2 state=floor(s*p);
    
    //float xodd = mod(state.x,2.0);
    //float yeve = mod(state.y + 1.0,2.0);
    return mod(state.x+state.y,2.0); //per IQ, may try FN2's ideas some other day
    
}
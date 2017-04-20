/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35204.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


void main( void ) 
{
	vec2 pos = ( gl_FragCoord.xy / RENDERSIZE.xy ) - vec2(0.5,0.5);	
	//vec2 pos = vv_FragNormCoord;
        float horizon = cos(TIME/ 3.)/3.0; 
        float fov = 0.5; 
	float scaling = 0.1;
	
	vec3 p = vec3(pos.x, fov, pos.y - horizon);      
	vec2 s = vec2(p.x/p.z, p.y/p.z) * scaling;
	
	mat2 rot = mat2(cos(TIME/ 3.), -sin(TIME/ 3.), sin(TIME/ 3.), cos(TIME/ 3.));
	s *= rot;
	
	s.x += TIME / 21. * sign(pos.y - horizon);
	s.y -= TIME / 33. * sign(pos.y - horizon);
	
	float g = sign((mod(s.x, 0.1) - 0.02) * (mod(s.y, 0.1) - 0.02));
	
	gl_FragColor = vec4( vec3(g, g, g) *p.z*p.z*11.0, 1.0 );
}
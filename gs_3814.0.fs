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
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#3814.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


// Fake DOF spheres by Kabuto
// Move mouse vertically to control focal plane

void main( void ) {

	vec3 pos = vec3(0,0,-10);
	vec3 dir = normalize(vec3((gl_FragCoord.xy - RENDERSIZE.xy*.5) / RENDERSIZE.x, 1.));

	vec3 color = vec3(0.,.15,.64);
	for (float y = 6.; y >= -6.; y--) {
		for (float x = -5.; x <= 5.; x++) {
			// 
			vec3 s = vec3(x+sin(TIME+y*.7),sin(TIME+x*.5+y*.5),y+sin(TIME-x*.7));
			float t = dot(s-pos,dir);
			vec3 diff = (pos+t*dir-s)*3.;
			float dist = length(diff);
			// fake depth of field
			float dof = abs(1./(pos.z-s.z)+1./(5.+10.*mouse.y))*4.;
			float dofdist = (length(diff)-1.)/dof;
			dofdist = max(-1.,min(1.,dofdist));
			dofdist = sign(dofdist)*(1.-pow(1.-abs(dofdist),1.5));
			float invalpha = dofdist*.5+.5;
			vec3 normal = normalize(pos+t*dir-vec3(-(mouse.x-.5)*.5*(5.+10.*mouse.y),-5.,-5.+10.*mouse.y));
			
			float color1 = dot(normalize(diff+vec3(0,0,1.)),normal)*.5+.5;
			color1 = pow(color1,2.);
			color = color*invalpha + color1*(1.-invalpha);
		}
	}
	
	
	
	gl_FragColor = vec4(sqrt(color), 1.0 );

}
/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#14157.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


//@mattdesl - first attempts at raymarching, W.I.P. 

float sdTorus( vec3 p, vec2 t )
{
  vec2 q = vec2(length(p.xz)-t.x,p.y);
  return length(q)-t.y;
}

float sdFloor(vec3 p, vec3 b) {
  return length(max(abs(p)-b,0.0));
}

float sdCylinder( vec3 p, vec3 c )
{
  return length(p.xz-c.xy)-c.z;
}

void main()
{


	vec2 coords = gl_FragCoord.xy / RENDERSIZE;
	
	vec3 ray_dir = normalize( vec3( coords.x-sin(TIME*0.5)*0.1, coords.y - 0.5, -1.0 +sin(TIME*0.5)*0.1) );
	vec3 ray_orig = vec3(-20.0+sin(TIME*0.5)*20.0,5.0,100.0);
	float offs = 0.0;
	float j;
	for( float i = 0.0; i < 50.0;i += 1.0 ) {
		vec3 pos = vec3(ray_orig+ray_dir*offs);
			
		vec3 c = vec3(100.0,100.0,100.0);
		vec3 q = mod(pos,c)-0.5*c;
		
		float dist = sdCylinder(q, vec3(0.0,0.0,5.0));
		
		dist = min(dist, sdFloor(q, vec3(50.0,1.0,50.0)));
		
		offs+=dist;
	        j = i;
		if(abs(dist)<0.0001) break;
		
	}
	
	float c=j/50.0;
	gl_FragColor=vec4(vec3(c), 1.0);
	
}
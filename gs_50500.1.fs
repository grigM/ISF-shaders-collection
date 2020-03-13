/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#50500.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec3 light_dir = vec3( 0.0,1.0,-1.0);

struct Ray{
	vec3 dir;
	vec3 pos;
};
	
mat3 rot = mat3(
		cos(TIME/3.0),0.0,sin(TIME/3.0),
		0.0,1.0,0.0,
		-sin(TIME/3.0),0,cos(TIME/3.0)
);

	

float calc_dist(vec3 pos){
	float displacement = sin(pos.x*400.0)*sin(1.0*pos.y+TIME)*sin(200.0*pos.z)*sin(5.0*TIME)*2.0;

	return mod(length(pos) - 2.0 + displacement,4.0)-2.0;		
}
vec3 calc_norm(vec3 pos){
	float diff = 0.001;
	return normalize(vec3(
		calc_dist( vec3(pos.x + diff, pos.y, pos.z)) - calc_dist(vec3(pos.x - diff, pos.y,pos.z)),
		calc_dist( vec3(pos.x , pos.y+diff, pos.z)) - calc_dist(vec3(pos.x , pos.y-diff,pos.z)),
		calc_dist( vec3(pos.x , pos.y, pos.z+diff)) - calc_dist(vec3(pos.x , pos.y,pos.z-diff))
		));
}

void main( void ) {

	vec2 pos = (gl_FragCoord.xy * 2.0 - RENDERSIZE.xy) / min(RENDERSIZE.x, RENDERSIZE.y);
	vec3 camera_pos = vec3(0.0, 0.0,- 5.0);
	vec3 camera_up = vec3(0.0,1.0,0.0);
	vec3 camera_dir = vec3(0.0,0.0,1.0);
	vec3 camera_side = cross(camera_up, camera_dir);
	
	Ray ray;
	ray.pos = camera_pos;
	float focus = 1.0;
	ray.dir = normalize( camera_side * pos.x + camera_up * pos.y + focus * camera_dir);
	
	float dist;
	for( int  i = 0; i < 2; i++){
		dist = calc_dist(ray.pos);
		if( dist < 0.01){
			break;
		}else{
			ray.pos += dist * ray.dir;
		}
	}
	if( dist < 0.01){
		vec3 norm = calc_norm(ray.pos);
		float light_str = dot(light_dir, norm);
		gl_FragColor = vec4(vec3(light_str),1.0);
	}else{
		gl_FragColor = vec4(vec3(0.0),1.0);
	}

}
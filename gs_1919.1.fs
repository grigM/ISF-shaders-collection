/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#1919.1"
}
*/


/*
 * Following iQ live coding tutorial on writing a basic raytracer http://www.youtube.com/watch?v=9g8CdctxmeU
 * @blurspline 
  * ... this forked version mostly just scrunched up to fit in less space without obfuscating too much. 
 */

#ifdef GL_ES
precision mediump float;
#endif


// normals(?)
vec3 nSphere( in vec3 pos, in vec4 sph ) { return ( pos - sph.xyz )/ sph.w; } 
vec3 nPlane( in vec3 pos ) {	return vec3 (0.0, 1.0, 0.0); }
vec4 sph1 = vec4( 0.0, 1.0, 0.0, 1.0); // sphere, w is size

// intersections
float iPlane( in vec3 ro, in vec3 rd ) { return -ro.y / rd.y; }

float iSphere( in vec3 ro, in vec3 rd, in vec4 sph ) {
	vec3 oc = ro - sph.xyz; // looks like we are going place sphere from an offset from ray origin, which is = camera
	//float r = 1.0;
	float b = 2.0*dot( oc, rd );
	float c = dot(oc, oc) - sph.w * sph.w; // w should be size
	float h = b*b - 4.0 *c;
	if (h<0.0 ) return -1.0;
	float t = (-b - sqrt(h)) / 2.0;
	return t;
}

float intersect( in vec3 ro, in vec3 rd, out float resT ) {
	resT = 10000.;
	float id = -1.0;
	float tmin = -1.0;
	float tsph = iSphere( ro, rd, sph1 ); 
	float tpla = iPlane( ro, rd ); 
	
	if ( tsph>0.0 ) {
		id = 1.0;
		resT = tsph;
	}
	
	if (tpla > 0.0 && tpla <resT ) {
		id = 2.0;
		resT = tpla;
	}
		
	return id;
}


void main( void ) {
	vec3 light = normalize( vec3(0.5,0.6,0.4) );		// ok, here we come, GLSL raytacing!
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );  // this is the pixel coords
	sph1.xz = vec2 ( 0.5 * cos( TIME ),  0.5 * sin( TIME )); // move sphere around
	
	vec3 ro = vec3( .1, 0.2, 3.0 ); //ray origin
	vec3 rd = normalize( vec3( -1.0 + 2.*uv* vec2(RENDERSIZE.x/RENDERSIZE.y, 1.0), -1.0 ) ); // ray destination
 	
	float t;
	float id = intersect( ro, rd, t ); // we intersect ray with 3d scene
	vec3 col = vec3(0.65);
	
	if ( id>0.5 && id<1.5 ) {
		// sphere		
		vec3 pos = ro + t * rd;
		vec3 nor = nSphere( pos, sph1 );
		float dif = clamp( dot( nor, light ), 0.0, 1.0); // diffuse light
		float ao = 0.5 + 0.5 * nor.y;
		col = vec3( 0.8, 0.8, 0.6) * dif * ao + vec3(0.1, 0.2, 0.4) * ao;
	} else if ( id>1.5 ) {
		// plane
		vec3 pos = ro + t * rd;
		vec3 nor = nPlane( pos );
		float dif = clamp( dot(nor, light), 0.0, 1.0 );
		float amb = smoothstep( 0.0, sph1.w * 2.0, length(pos.xz - sph1.xz) ); // shadows under the sphere
		col = vec3 (amb * 0.9);
	}
		
	col = sqrt(sqrt(col));
	float f = (mod(gl_FragCoord.x * 5.0 + gl_FragCoord.y, 8.0) < col.r * 8.0) ? 1.0 : 0.0;
	gl_FragColor = vec4( f, f, f, 1.0 );

}
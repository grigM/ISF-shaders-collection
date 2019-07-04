/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#45936.0"
}
*/


// original by nimitz https://www.shadertoy.com/view/lsSGzy#, slightly modified, and gigatron for glslsandbox
// added computed noise from https://gist.github.com/patriciogonzalezvivo/670c22f3966e662d2f83
// instead textured noise ; etc // 
#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


 
	#define ray_brightness 10.
	#define gamma 5.
	#define ray_density 4.5
	#define curvature 15.
	#define red   4.
	#define green 1.0
	#define blue  0.99 

 

#define SIZE 0.2

 
// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

float rand(vec2 n) { 
	return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453);
}

float noise(vec2 p){
	vec2 ip = floor(p);
	vec2 u = fract(p);
	u = u*u*(3.0-2.0*u);
	
	float res = mix(
		mix(rand(ip),rand(ip+vec2(1.0,0.0)),u.x),
		mix(rand(ip+vec2(0.0,1.0)),rand(ip+vec2(1.0,1.0)),u.x),u.y);
	return res*res;
}




// FLARING GENERATOR, A.K.A PURE AWESOME
mat2 m2 = mat2( 0.80,  0.60, -0.60,  0.80 );
float fbm( in vec2 p )
{	
	float z=8.;       // EDIT THIS TO MODIFY THE INTENSITY OF RAYS
	float rz = -0.08; // EDIT THIS TO MODIFY THE LENGTH OF RAYS
	p *= 0.425;        // EDIT THIS TO MODIFY THE FREQUENCY OF RAYS
	for (int i= 1; i < 6; i++)
	{
		rz+= abs((noise(p)-0.5)*2.)/z;
		z = z*2.;
		p = p*2.*m2;
	}
	return rz;
}

void main()
{
	float t = -TIME*.33; 
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy-0.5;
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
	uv*= curvature* SIZE;
	
	float r = sqrt(dot(uv,uv)); // DISTANCE FROM CENTER, A.K.A CIRCLE
	float x = dot(normalize(uv), vec2(.5,0.))+t;
	float y = dot(normalize(uv), vec2(.0,.5))+t;
 
        float val=0.0;
        val = fbm(vec2(r+ y * ray_density, r+ x * ray_density)); // GENERATES THE FLARING
	val = smoothstep(gamma*.02-.1,ray_brightness+(gamma*0.02-.1)+.001,val);
	val = sqrt(val); // WE DON'T REALLY NEED SQRT HERE, CHANGE TO 15. * val FOR PERFORMANCE
	
	vec3 col =  val/ vec3(red,green,blue);
	col = 1.-col; // WE DO NOT NEED TO CLAMP THIS LIKE THE NIMITZ SHADER DOES!
        float rad= 35. ; // MODIFY THIS TO CHANGE THE RADIUS OF THE SUNS CENTER
	col = mix(col,vec3(1.), rad - 266.667 * r); // REMOVE THIS TO SEE THE FLARING
	// for glslsandbox pic-frame visibility... gigatron
	vec4 cfinal =  mix(vec4(col,1.0),vec4(0.0,0.0,.0,1.0),0.05);
	
	gl_FragColor = vec4(cfinal);
}
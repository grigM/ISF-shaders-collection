/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#42915.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

//more hexagon stuff :D



const float pi=3.14159265359;

 
vec2 hexify(vec2 p,float hexCount){
	p*=hexCount;
	vec3 p2=floor(vec3(p.x/0.86602540378,p.y+0.57735026919*p.x,p.y-0.57735026919*p.x));
	float y=floor((p2.y+p2.z)/3.0);
	float x=floor((p2.x+(1.0-mod(y,2.0)))/2.0);
	return vec2(x,y)/hexCount;
}

 

void main( void ) {
	
	vec2 p = ( gl_FragCoord.xy * 2. - RENDERSIZE.xy ) / min(RENDERSIZE.x, RENDERSIZE.y);
	p /= dot(p,p);
	
	p=hexify(p,2.0 );
	
	
	float vr = 0.5*sin(40.* ( p.y+p.x*0.2)+TIME*2.)+0.5 ;
	
	float vg = 0.5*sin(30.* ( p.y+p.x*0.3)+TIME*3.)+0.5 ;
	
	float vb = 0.5*sin(20.* ( p.y+p.x*0.4)+TIME*4.)+0.5 ;
	  	
	gl_FragColor = vec4(vr,vg,vb,1);
}
/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#4805.0"
}
*/


// @mod*, colored by rotwang

#ifdef GL_ES
precision highp float;
#endif


float sphere(vec3 p)
{
	return length(p)-5.;
}

float displacement(vec3 p)
{
	return sin(p.x)*sin(p.y)*sin(p.z);
}

float opDisplace( vec3 p )
{
	float d1 = sphere(p);
    	float d2 = displacement(p);
    	return d1+d2;
}

void main( void ) {

	vec2 p = -1. + 2.*gl_FragCoord.xy / RENDERSIZE.xy;
	p.x *= RENDERSIZE.x/RENDERSIZE.y;
	
	//Camera animation
  vec3 vuv=vec3(0,1,0);//Change camere up vector here
  vec3 vrp=vec3(0,1,0); //Change camere view here
  vec3 prp=vec3(sin(TIME)*8.0,4,cos(TIME)*8.0); //Change camera path position here

  //Camera setup
  vec3 vpn=normalize(vrp-prp);
  vec3 u=normalize(cross(vuv,vpn));
  vec3 v=cross(vpn,u);
  vec3 vcv=(prp+vpn);
  vec3 scrCoord=vcv+p.x*u+p.y*v;
  vec3 scp=normalize(scrCoord-prp);

  //Raymarching
  const vec3 e=vec3(0.1,0,0);
  const float maxd=16.0; //Max depth

  float s=0.1;
  vec3 c,p1,n;

  float f=1.0;
  for(int i=0;i<30;i++){
   // if (abs(s)<.01||f>maxd) break;//eliminating break so I can try out w/ core image.
    f+=s;
    p1=prp+scp*f;
    s=opDisplace(p1);
  }
  	
	//replacing if/else with ternary to try out with apple's "core image"
	c=vec3(.2,0.6,0.8);
    	n=normalize(
      	vec3(s-opDisplace(p1-e.xyy),
           s-opDisplace(p1-e.yxy),
           s-opDisplace(p1-e.yyx)));
    	float b=dot(n,normalize(prp-p1));
    	vec4 tex=vec4((b*c+pow(b,128.0))*(1.0-f*.01),1.0);
	tex += vec4(0.99,0.66,0.2,1.0)/f*2.0;
	vec4 background=vec4(0.0,0,0,1);
	
	vec4 Color=(f<maxd)?tex:background;
	
  	/*if (f<maxd){
      	c=vec3(.3,0.5,0.8);
    	n=normalize(
      	vec3(s-opDisplace(p1-e.xyy),
           s-opDisplace(p1-e.yxy),
           s-opDisplace(p1-e.yyx)));
    	float b=dot(n,normalize(prp-p1));
   	gl_FragColor=vec4((b*c+pow(b,8.0))*(1.0-f*.01),1.0);
  	}
  	else gl_FragColor=vec4(0,0,0,1);
	*/
//to use with core image, just replace with "return Color"
gl_FragColor=Color;

}
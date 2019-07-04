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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#16385.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif



vec4 circle(float r,vec2 p0,vec2 p1,vec3 color){
	return vec4(color,step(distance(p0,p1),r));
}

vec4 cutcircle1(float r,vec2 p0,vec2 p1,vec3 color,float cut){
	vec4 c=circle(r,p0,p1,color);
	return vec4(c.xyz,c.w*step(cut,((p1.y-p0.y)+r)/(2.0*r)));
}

vec4 cutcircle2(float r,vec2 p0,vec2 p1,vec3 color,float cut){
	vec4 c=circle(r,p0,p1,color);
	return vec4(c.xyz,c.w*step(cut,1.0-((p1.y-p0.y)+r)/(2.0*r)));
}

vec3 smiley(vec2 p){
	p=fract(p);
	vec3 c=vec3(0);
	//yellow
	vec4 temp=circle(0.45,vec2(0.5),p,vec3(0.9,0.8,0.2));
	c=mix(c,temp.xyz,temp.w);
	//mouth border
	temp=cutcircle2(0.34,vec2(0.47,0.42),p,vec3(0),0.45);
	c=mix(c,temp.xyz,temp.w);
	//mouth inside
	temp=cutcircle2(0.3,vec2(0.47,0.4),p,vec3(0.5,0.0,0.2),0.45);
	c=mix(c,temp.xyz,temp.w);
	//eye1 border
	temp=cutcircle1(0.15,vec2(0.25,0.62),p,vec3(0),0.2);
	c=mix(c,temp.xyz,temp.w);
	//eye1 inside
	temp=cutcircle1(0.12,vec2(0.25,0.63),p,vec3(1),0.2);
	c=mix(c,temp.xyz,temp.w);
	//eye2 border
	temp=cutcircle1(0.15,vec2(0.63,0.62),p,vec3(0),0.2);
	c=mix(c,temp.xyz,temp.w);
	//eye2 inside
	temp=cutcircle1(0.12,vec2(0.63,0.63),p,vec3(1),0.2);
	c=mix(c,temp.xyz,temp.w);
	//eye1 pupil
	temp=circle(0.05,vec2(0.69,0.69),p,vec3(0));
	c=mix(c,temp.xyz,temp.w);
	//eye2 pupil
	temp=circle(0.05,vec2(0.31,0.69),p,vec3(0));
	c=mix(c,temp.xyz,temp.w);
	return c;
}
void main( void ) {
        vec2 position = ( gl_FragCoord.xy / min(RENDERSIZE.x,RENDERSIZE.y) );
	vec2 m = mouse * RENDERSIZE.xy / min(RENDERSIZE.x,RENDERSIZE.y)  ;
	
	
	position = fract(sin(2.*position*distance(m,position)));
	
	gl_FragColor = vec4( smiley(position), 1.0 );

}
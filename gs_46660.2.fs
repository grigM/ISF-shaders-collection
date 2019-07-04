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
    },
    {
			"NAME": "VECTOR_SIZE",
			"TYPE": "float",
			"DEFAULT": 24,
			"MIN": 10,
			"MAX": 100.0
			
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#46660.2"
}
*/


#ifdef GL_ES
precision highp float;
#endif

uniform vec2 surfaceSize;



float distLine(vec2 p0,vec2 p1,vec2 uv)
{
	vec2 dir = normalize(p1-p0);
	uv = (uv-p0) * mat2(dir.x,dir.y,-dir.y,dir.x);
	return distance(uv,clamp(uv,vec2(0),vec2(distance(p0,p1),0)));   
}

float distVector(vec2 vec, vec2 uv)
{
	vec = normalize(vec);
	
	float dist = 1e6;
	
	uv -= 0.5;
	uv *= mat2(vec.x,vec.y,-vec.y,vec.x);
	
	dist = min(dist, distLine(vec2( 0.4, 0.0),vec2(-0.4, 0.0),uv));
	dist = min(dist, distLine(vec2( 0.4, 0.0),vec2( 0.2, 0.2),uv));
	dist = min(dist, distLine(vec2( 0.4, 0.0),vec2( 0.2,-0.2),uv));
	
	return dist;
}

void main( void ) 
{
	vec2 aspect = surfaceSize*0.5;//vec2(RENDERSIZE.xy/RENDERSIZE.y);
	vec2 uv = vv_FragNormCoord;//(gl_FragCoord.xy / RENDERSIZE.y);
	
	//uv.x -= (aspect.x-1.0)/2.0;
	
	vec2 mo = (mouse * 2.0 - 1.0)/uv;
	//* aspect;
	//mo.x -= (aspect.x-1.0)/2.0;
	
	float color = 0.0;
	
	float vectors = floor(RENDERSIZE.y / VECTOR_SIZE);
	
	vec2 rep = mod(uv,vec2(1.0/vectors))*vectors;
	
	vec2 pos = (floor(uv*vectors)+0.5)/vectors;
	
	color = distVector(normalize(mo-pos),rep);
	
	float scale = 1.0/RENDERSIZE.y * vectors;
	
	color = smoothstep(1.5*scale,0.0,color);
	
	gl_FragColor = vec4( vec3( color ), 1.0 );

}
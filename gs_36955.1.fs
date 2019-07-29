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
		"NAME": "p2_x",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": 0.0,
		"MAX": 10.0
	},
	{
		"NAME": "p2_y",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 5.0
	},
	{
		"NAME": "p3_x",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 10.0
	},
	{
		"NAME": "p3_y",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 5.0
	},
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36955.1"
}
*/



#extension GL_OES_standard_derivatives : enable
#ifdef GL_ES
precision mediump float;
#endif


// Comment this out to use a euclidian distance
#define USE_MANHATTAN

// Comment this out to stop drawing the point
#define DRAW_POINT

// How big points should be
#define POINT_SIZE .1



#ifdef USE_MANHATTAN
	#define DISTANCE_FUNC manhattan
	#define POINT_SIZE_ACTUAL POINT_SIZE
#else
	#define DISTANCE_FUNC euclid
	//we do fast pythagorean so pow2 the normal size
	#define POINT_SIZE_ACTUAL POINT_SIZE*POINT_SIZE
#endif

float euclid(vec2 a, vec2 b){
	vec2 delta = a-b;
	vec2 delta2=delta*delta;
	//Skip the sqrt for just comparisons
	return delta2.x+delta2.y;
}

float manhattan(vec2 a, vec2 b){
	vec2 delta = abs(a-b);
	return delta.x+delta.y;
}

void main( void ) {
	//So GLSL100 doesn't support initilizer lists, so make a shitty macro to do it
	#define p(i,x,y,r,g,b) points[i]=vec2(x,y);point_colors[i]=vec4(float(r)/255.0,float(g)/255.0,float(b)/255.0,1);
	
	const int num_points = 6;
	vec2 points[num_points];
	vec4 point_colors[num_points];
	//index, X, Y (0-10), R, G, B (0-255)
	
	p(0,(13.*mouse.x),13.*mouse.y,255,0,0);
	p(1,2.*p2_x,2.*p2_y,0,255,0);
	p(2,4.*p3_x,2.*p3_y,0,0,255);
	p(3,8,4,255,255,0);
	p(4,4,6,0,255,255);
	p(5,9,8,255,0,255);
	/*
	p(0,1,3,255,0,0);
	p(1,2,3,0,255,0);
	p(2,3,3,0,0,255);
	p(3,4,4,255,255,0);
	p(4,5,3,0,255,255);
	p(5,6,3,255,0,255);
	*/
	//adjust for aspect ratio and center
	float minres = min(RENDERSIZE.x,RENDERSIZE.y);
	vec2 offset = (RENDERSIZE-minres)/2.0;
	vec2 position = (( gl_FragCoord.xy - offset) / minres ) * 10.0;
	
	//iteratively find the closest
	vec4 closest_color;
	float closest_distance=99999.0;
	
	for(int i=0;i<num_points;i++){
		float current_distance = DISTANCE_FUNC(position,points[i]);
#ifdef DRAW_POINT
		if(false){//current_distance < POINT_SIZE){ // draw a point
			closest_color = vec4(0,0,0,0);
			closest_distance = 0.0;
		}else 
#endif
		if(current_distance < closest_distance){
			closest_distance = current_distance+.01;
			closest_color = point_colors[i];
		}
	}
	
	gl_FragColor = closest_color;

}
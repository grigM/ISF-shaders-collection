/*{
	"CREDIT": "by mojovideotech",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"generator"
	],
	"INPUTS": [
		{
			"NAME": "sizeX",
			"TYPE": "float",
			"DEFAULT": 300,
			"MIN": 6,
			"MAX": 600
		},	{
			"NAME": "sizeY",
			"TYPE": "float",
			"DEFAULT": 10,
			"MIN": 6,
			"MAX": 600
		},
		{
			"NAME": "rate",
			"TYPE": "float",
			"DEFAULT": 2.5,
			"MIN": 0.1,
			"MAX": 5.0
		},
		{
			"NAME": "hue",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -0.9,
			"MAX": 0.9
		},
		{
			"NAME": "colorcycle",
			"TYPE": "point2D",
		   "DEFAULT": [
				0.2,
				0.8
	  		],
      		"MAX" : [
        		1.0,
        		1.0
      		],
      		"MIN" : [
        		0.1,
        		0.1
      		]
    	}
	]
}*/


// WorkinOnTheBlockChainGang by mojovideotech
// mod of :
// glslsandbox.com/e#30939.0

#ifdef GL_ES
precision mediump float;
#endif

# define sizeXY vec2(sizeX,sizeY)

float ranomize(vec2 coords)
{
		float a = 1.282427;
    	float b = 41.49865;
    	float c = 57721.56649;
    	float dt = dot(coords.xy ,vec2(a,b));
    	float sn = mod(dt,2.685452);
    	return fract(sin(sn) * c);
}

vec3 getColor(vec2 coords)
{
	coords.x = coords.x-mod(coords.x, sizeX);
	coords.y = coords.y-mod(coords.y, sizeY);
	float r = ranomize(coords.xx*colorcycle);
	float g = ranomize(coords.yy/colorcycle);
	float b = ranomize(vec2(r,g));
	return vec3(r-hue,g,b+hue);
}

float triangleWave(float x)
{
	x = mod(x,2.0);
	if (x > 1.0) x = -x+2.0;
	return x;
}

bool inSize(vec2 coords)
{
	vec2 box = coords.xy-mod(coords.xy, sizeXY);
	vec2 center = box+(sizeXY*0.5);
	float tsize = (triangleWave((TIME * rate)+(ranomize(box*box)*2.0))/2.0)*max(sizeX,sizeY);
	return (abs(coords.x-center.x) < tsize && abs(coords.y-center.y) < tsize);
}

void main( void ) 
{
	vec3 color;
	if (inSize(gl_FragCoord.xy)) color += getColor(gl_FragCoord.xy);
	gl_FragColor = vec4( color, 1.0 );

}
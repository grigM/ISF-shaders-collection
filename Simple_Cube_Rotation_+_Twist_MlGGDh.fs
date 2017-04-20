/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "cube",
    "rotation",
    "twist",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MlGGDh by cacheflowe.  I'm trying to learn more. Sorry so basic! Suggestions are welcome.",
  "INPUTS" : [
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 3.0
		},
		{
			"NAME": "bend",
			"TYPE": "float",
			"DEFAULT": 1.2,
			"MIN": -4.0,
			"MAX": 4.0
		},
		{
			"NAME": "rotMat1x",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -10.0,
			"MAX": 10.0
		},
		{
			"NAME": "rotMat2x",
			"TYPE": "float",
			"DEFAULT": -3.0,
			"MIN": -5.0,
			"MAX": 5.0
		},
		{
			"NAME": "rotMat3x",
			"TYPE": "float",
			"DEFAULT": 0.7,
			"MIN": -5.0,
			"MAX": 5.0
		},
		{
			"NAME": "SDF_THRESHOLD",
			"TYPE": "float",
			"DEFAULT": 0.0001,
			"MIN": 0.0001,
			"MAX": 0.1
		},
		{
			"NAME": "ITERATIONS",
			"TYPE": "float",
			"DEFAULT": 64,
			"MIN": 0.0,
			"MAX": 64
		},
		{
			"NAME": "scale",
			"TYPE": "float",
			"DEFAULT": 0.6,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "cubeZ",
			"TYPE": "float",
			"DEFAULT": -3.0,
			"MIN": -5.0,
			"MAX": -2.0
		},
		
		
	{
      "NAME" : "color_1",
      "TYPE" : "color",
      "DEFAULT" : [
        0.0,
        0.0,
        0.0,
        1
      ],
      "LABEL" : ""
    },
    
    
    {
			"NAME": "cube_color_type",
			"TYPE": "long",
			"VALUES": [
				0,
				1

			],
			"LABELS": [
				"solid",
				"gradient"
		
			],
			"DEFAULT": 0
		},
		
		{
      "NAME" : "color_2",
      "TYPE" : "color",
      "DEFAULT" : [
        1.0,
        1.0,
        1.0,
        1
      ],
      "LABEL" : ""
    },
    {
			"NAME": "graColorSpeed",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 8.0
		},
    	{
			"NAME": "graColor1",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "graColor2",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "graColor3",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
    
    
  ]
}
*/


//#define ITERATIONS 64
//#define SDF_THRESHOLD 0.0001

//#define BG_COLOR vec3(1,1,1)
#define PI 3.141592653589793238462643383

// --------------------------------------------------------
// http://www.neilmendoza.com/glsl-rotation-about-an-arbitrary-axis/
// updated by @stduhpf to be 3x3 - thank you!
// also thanks to @FabriceNeyret2 for code optimizations.
// --------------------------------------------------------

mat3 rotationMatrix(vec3 m,float a) {
    m = normalize(m);
    float c = cos(a),s=sin(a);
    return mat3(c+(1.-c)*m.x*m.x,
                (1.-c)*m.x*m.y-s*m.z,
                (1.-c)*m.x*m.z+s*m.y,
                (1.-c)*m.x*m.y+s*m.z,
                c+(1.-c)*m.y*m.y,
                (1.-c)*m.y*m.z-s*m.x,
                (1.-c)*m.x*m.z-s*m.y,
                (1.-c)*m.y*m.z+s*m.x,
                c+(1.-c)*m.z*m.z);
}

// --------------------------------------------------------
// http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
// --------------------------------------------------------

float udBox( vec3 p, vec3 b ) {
  return length(max(abs(p)-b,0.0));
}

float udBoxTwisted( vec3 p, vec3 b, float twist )
{
    float c = cos(twist*p.y);
    float s = sin(twist*p.y);
    mat2  m = mat2(c,-s,s,c);
    vec3  q = vec3(m*p.xz,p.y);
    return udBox(q, b);
}
 
void main()
{
    // basic raymarching template from @nicoptere: https://www.shadertoy.com/view/ldtGD4
    // 1 : get fragment's coordinates
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv -= 0.5;									// Move to center
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;		// Correct for aspect ratio
    
    // 2 : camera position and ray direction
    //float cubeZ = -3.;
	vec3 pos = vec3( 0, 0, cubeZ);
	vec3 dir = normalize( vec3( uv.x, uv.y, 1.) );
 
	// 3 : ray march loop. ip will store where the ray hits the surface
	vec3 ip;
 
	// variable step size
	float t = 0.0;
	int found = 0;
    int last_i = 0;
    float time = TIME*speed;
    
	for(int i=0; i < int(ITERATIONS); i++) {
		last_i = i;

        //update position along path
        ip = pos + dir * t;
 
        // gets the shortest distance to the sdf shape. break the loop if the distance was too small. this means that we are close enough to the surface
    	vec3 ipRotated = ip * rotationMatrix(vec3(rotMat1x,rotMat2x,rotMat3x), 3.3 * sin(time));
        // float temp = udBox( ipRotated, vec3(CUBE_SIZE) );
        float temp = udBoxTwisted( ipRotated, vec3(scale), -sin(PI*0.5 + time) * bend );
		if( temp < SDF_THRESHOLD ) {
			
			if(cube_color_type==0){
				ip = vec3(color_2);
			}else if(cube_color_type==1){
			ip = vec3(
				//cube colors
                graColor1  * sin(graColorSpeed + time + ip.x),
                graColor2  * sin((graColorSpeed*2.0) + time + ip.y),
                graColor3 * sin((graColorSpeed*3.0) + time + ip.z)
            );
			}
			found = 1;
			break;
		}
		
		//increment the step along the ray path
		t += temp;
	}
	
	// make background black if no shape was hit
	if(found == 0) ip = vec3(color_1);
 
	// 4 : apply color to this fragment
	gl_FragColor = vec4(ip, 1.0);
	
}
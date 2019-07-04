/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "noise",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Mdlyz2 by Jops.  noise",
  "INPUTS" : [
    
    	{
			"NAME": "noise_amp",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 1.0,
			"DEFAULT": 0.06
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 4.0,
			"DEFAULT": 1.0
		},
		{
			"NAME": "fract_count",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 100.0,
			"DEFAULT": 10.0
		},
		{
			"NAME": "fract_speed",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 15.0,
			"DEFAULT": 2.5
		},
		{
			"NAME": "HORIZONTAL",
			"TYPE": "bool",
			"DEFAULT": 0.0
		}
		

  ]
}
*/


#define PI 3.14159265359
#define TWO_PI 6.28318530718
float rand(vec2 uv)
{
    //return fract(sin(dot(uv, vec2(12.9898,78.233)))*10000.*TIME);
	//return fract(sin(dot(uv, iMouse.xy))*10000.);
	//return (fract(sin(dot(uv, vec2(12., 70.)))*100000.));
    return (fract(sin(dot(uv, vec2(12., 70.)))*43758.5453123));
}


// Value noise
float noise(in vec2 st) {
    vec2 i = floor(st);
    vec2 f = fract(st);

    // Four corners in 2D of a tile
    float a = rand(i);
    float b = rand(i + vec2(1.0, 0.0));
    float c = rand(i + vec2(0.0, 1.0));
    float d = rand(i + vec2(1.0, 1.0));

    // Smooth Interpolation

    // Cubic Hermine Curve.  Same as SmoothStep()
    vec2 u = f*f*(3.0-2.0*f);
    // u = smoothstep(0.,1.,f);

    // Mix 4 coorners porcentages
    return mix(a, b, u.x) + 
            (c - a)* u.y * (1.0 - u.x) + 
            (d - b) * u.x * u.y;
}

float createPoly(int corners, vec2 uv, vec2 pos, float rotate)
{
    uv-=pos;
    //pos.x *= ratio;
// Number of sides of your shape
	int N = corners;

// Angle and radius from the current pixel
	float a = atan(uv.x,uv.y)+PI + rotate;
	float r = TWO_PI/float(N);
  
// Shaping function that modulate the distance
    float dist = cos(floor(.5+a/r)*r-a)*length(uv);
    
    return dist;
}

float createRectangle(vec2 uv, vec2 pos, vec2 size)
{
   
    
    float testH = step(pos.x, uv.x ) - step(pos.x + (size.x), uv.x );
    float testV = step(pos.y, uv.y ) - step(pos.y + (size.y), uv.y );
    
    return testV * testH;
}

float createCircle(vec2 uv, vec2 pos, float rad)
{
    
    float testV;
    
    testV = 1.-step(rad, distance(pos, uv));
    //testV = 1.-step(rad, distance(pos, uv) + distance(uv, pos + vec2(0.05)));
    
   	//testV = step(distance(pos, uv), rad);
    //testV = step(distance(uv,pos), rad);
  	//testV = step(distance(uv, pos), rad);
   	//testV = step(distance(uv, pos) * distance(uv, pos), rad);
    //testV += step(distance(uv, pos ) * distance(uv, pos), rad);
    //testV = step(max(distance(uv,pos + vec2(0.6)), distance(uv, pos)), rad);


      
    //testV = fract(distance(uv,pos)) * (distance(uv, pos));
    
    return testV;
}

void main() {



    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    uv.x *= ratio;
    
    vec2 pos = vec2(0.5 * ratio, 0.5);
    float offset = noise(uv * 6. + (TIME)) * noise_amp;
    
  
    ////////////////////////////////////////////////////////////////////////////
    //vec2 shapePos = vec2(.5*ratio, .5);
    //float dist = distance(shapePos, uv)*2.;
    
    //uv -= shapePos;
    //float angle = atan(uv.y, uv.x);
    //float radius = cos(3.*angle);    
    ////////////////////////////////////////////////////////////////////////////
    float value;// = step(0.4 + offset, uv.x) - step(0.5 + offset, uv.x);
    vec3 randc = vec3(uv * 0.2-0.9*sin(TIME), value) ;
    //value = createRectangle(uv + offset, vec2(0.5 * ratio, 0.1), vec2(0.01, 0.6));
    
   	//value += createRectangle(uv + offset, vec2(0.45 * ratio, 0.3), vec2(0.01, .5));
    //value += createRectangle(uv + offset, vec2(0.55 * ratio, 0.3), vec2(0.01, .5));
    
    //value += createRectangle(uv + offset, vec2(0.3 * ratio, 0.7), vec2(0.1));
    //value += createRectangle(uv + offset, vec2(0.355 * ratio, 0.8), vec2(0.1));
    
    //value += createRectangle(uv + offset, vec2(0.41 * ratio, 0.9), vec2(0.3,0.1));
    
    //value += createRectangle(uv + offset, vec2(0.63 * ratio, 0.7), vec2(0.1));
    //value += createRectangle(uv + offset, vec2(0.578 * ratio, 0.8), vec2(0.1));
    
    //value += step(createPoly(4, uv + offset, vec2(0.88,0.8), 0.8), 0.1);
    
    //value += createRectangle(uv + offset, vec2(0.31 * ratio, 0.7), vec2(0.6,0.1));
    
  	//value += step(createPoly(10, uv + offset, vec2(0.61,0.8), 0.8), 0.1);
  	//value += step(createPoly(10, uv + offset, vec2(0.68,0.9), 0.8), 0.1);
    
    //value += step(createPoly(10, uv + offset, vec2(1.12,0.8), 0.8), 0.1);
  	//value += step(createPoly(10, uv + offset, vec2(1.09,0.9), 0.8), 0.1);
    //////////////////////////////////////////////////////////////////////////////////
    //value *= fract(sin(uv.x * uv.y * 10000.)*100000.);
    
    float id;
    float r; 
    
    if(HORIZONTAL){
    	
    	id = floor(uv.y*fract_count);
    	r = rand(vec2(id));
    
    	uv.x += r*fract_speed * -(TIME*speed) / 2. ;
   		uv.x = fract(uv.x);
   		
    }else{
    	
    	
    
    	id = floor(uv.x*fract_count);
    	r = rand(vec2(id));
    
    	uv.y += r*fract_speed * -(TIME*speed) / 2. ;
   		uv.y = fract(uv.y);
    }
    
    
    value = createCircle(uv + offset , vec2(0.5 *  ratio * abs(sin(TIME) + 1.0),      0.5), 0.1);
    value += createCircle(uv + offset, vec2(0.5 * ratio * abs(sin(TIME -.11) + 1.0), 0.4), 0.1);
    value += createCircle(uv + offset, vec2(0.5 * ratio * abs(sin(TIME - .22) + 1.0), 0.3), 0.1);
    value += createCircle(uv + offset, vec2(0.5 * ratio * abs(sin(TIME - .33) + 1.0), 0.2), 0.1);
    
    value += createRectangle(uv + offset, vec2(0.4 * ratio * abs(sin(TIME) + 0.95), 0.5 ) , vec2(0.6, 0.01));
    value += createRectangle(uv + offset, vec2(0.4 * ratio * abs(sin(TIME  -.11 ) + 0.95), 0.4 ) , vec2(0.6, 0.01));
    value += createRectangle(uv + offset, vec2(0.4 * ratio * abs(sin(TIME  -.22) + 0.95), 0.3 ) , vec2(0.6, 0.01));
    value += createRectangle(uv + offset, vec2(0.4 * ratio * abs(sin(TIME  -.33) + 0.95), 0.2 ) , vec2(0.6, 0.01));
    
    
    //uv.y *= 3.;    
    //vec2 tileIdx = floor(uv);
   	//uv = fract(uv);
    //value = rand(tileIdx / TIME) * sin(5.*angle);
    //value = rand(tileIdx ) * sin(5.*angle);
    //value = rand(tileIdx);
    
    //value += step(0.4, uv.x ) - step(0.2+.3*.9 * r, uv.x)  ;
    //value +=  createCircle(uv + offset , vec2(0.5 * ratio,      0.5 * r), 0.1);
 	
    
    
	
	gl_FragColor = vec4(vec3(value) ,1.0);
}

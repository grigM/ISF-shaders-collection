/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MdsBR4 by nshelton.  raymarch triangle tunnel",
  "PASSES" : [
    {
      "TARGET" : "BufferA",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {

    }
  ],
  "INPUTS" : [

  ]
}
*/


float sdTriPrism( vec3 p, vec2 h )
{
    vec3 q = abs(p);
    return max(q.z-h.y,max(q.x*0.866025+p.y*0.5,-p.y)-h.x*0.5);
}

float opS( float d1, float d2 )
{
    return max(-d1,d2);
}

mat3 rotationMatrix(vec3 axis, float angle)
{
    axis = normalize(axis);
    float s = sin(angle);
    float c = cos(angle);
    float oc = 1.0 - c;
    
    return mat3(oc * axis.x * axis.x + c,           oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,
                oc * axis.x * axis.y + axis.z * s,  oc * axis.y * axis.y + c,           oc * axis.y * axis.z - axis.x * s,
                oc * axis.z * axis.x - axis.y * s,  oc * axis.y * axis.z + axis.x * s,  oc * axis.z * axis.z + c);
}

float map(vec3 p)
{
    vec3 q = p;
    vec3 c = vec3(0.5);
    p.z = mod(p.z,c.z)-0.5*c.z;
    
    p = rotationMatrix(vec3(0,0,1), sin(floor(q.z / 0.5) / 2.0) * sin(TIME/5.0) * 1.0) * p;
    
    float rad = sin(q.z* 6.0) * 0.3 + 1.0;
	float outer = sdTriPrism(p, vec2(rad, 0.1));
    float inner = sdTriPrism(p, vec2((sin(TIME + q.z * 10.0) * 0.2 + 0.6) * rad, 0.6));
    
    return opS(inner, outer);
}

void getCamPos(inout vec3 ro, inout vec3 rd)
{
    ro.z = TIME;
}

void main() {
	if (PASSINDEX == 0)	{


	
	    vec2 _p = (-RENDERSIZE.xy + 2.0*gl_FragCoord.xy) / RENDERSIZE.y;
	    vec3 ray = normalize(vec3(_p, 1.0));
	    vec3 cam = vec3(0.0, 0.0, 0.0);
	    
	    getCamPos(cam, ray);
	    
	    float depth = 0.0, d = 0.0, iter = 0.0;
	    vec3 p;
	    
	    for( int i = 0; i < 50; i ++)
	    {
	    	p = depth * ray + cam;
	        d = map(p);
	                  
	        if (d < 0.001)
				break;
	                   
			depth += d * 0.5;
			iter++;
	                   
	    }
	    
	    vec3 col = vec3(1.0 - iter / 50.0);
	    gl_FragColor = vec4(col, 1.0);
	    
	}
	else if (PASSINDEX == 1)	{


	
	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    
	    vec4 col;
	    vec2 delta = vec2(uv - 0.5)* 0.5 * sin(TIME);
	    
	    col.r = IMG_NORM_PIXEL(BufferA,mod(uv + delta,1.0)).r;
	    col.g = IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).g;
	    col.b = IMG_NORM_PIXEL(BufferA,mod(uv - delta,1.0)).b;
	    col.a  = 1.0;
		gl_FragColor = sqrt(col);
	
	    
	}
}

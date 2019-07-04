/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "raymarch",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llXBRH by macbooktall.  lol",
  "PASSES" : [
    {
      "TARGET" : "BufferA",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {
      "TARGET" : "BufferB",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {

    }
  ],
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


#define MAXDIST 150.
#define GIFLENGTH 1.570795

struct Ray {
	vec3 ro;
    vec3 rd;
};
    
void pR(inout vec2 p, float a) {
	p = cos(a)*p + sin(a)*vec2(p.y, -p.x);
}

float length6( vec3 p )
{
	p = p*p*p; p = p*p;
	return pow( p.x + p.y + p.z, 1.0/6.0 );
}

float fractal(vec3 p)
{
    p=p.yxz;
    p.x += 6.;

    float scale = 1.25;
   
    const int iterations = 30;

    float time = TIME;
    float a = time;
    
	float l = 0.;
	float len = length(p);
    //vec2 m = iMouse.xy / RENDERSIZE.xy;
    vec2 f = vec2(0.1,0.1);
	vec2 m = vec2(.525,0.6);

    pR(p.yz,.5);
    
    pR(p.yz,m.y*3.14);
    
    for (int i=0; i<iterations; i++) {
		p.xy = abs(p.xy);
		p = p*scale + vec3(-3.,-1.5,-.5);
        
		pR(p.yz,m.y*3.14 + sin(TIME*4. + len)*f.x);

        pR(p.xy,m.x*3.14 + cos(TIME*4. + len)*f.y);
		
        l=length6(p);
	}
	return l*pow(scale, -float(iterations))-.25;
}

vec2 map(vec3 pos) {

    return vec2(fractal(pos), 0.);
}

vec2 march(Ray ray) 
{
    const int steps = 90;
    const float prec = 0.001;
    vec2 res = vec2(0.);
    
    for (int i = 0; i < steps; i++) 
    {        
        vec2 s = map(ray.ro + ray.rd * res.x);
        
        if (res.x > MAXDIST || s.x < prec) 
        {
        	break;    
        }
        
        res.x += s.x;
        res.y = s.y;
        
    }
   
    return res;
}

vec3 vmarch(Ray ray, float dist, vec3 normal)
{   
    vec3 p = ray.ro;
    vec2 r = vec2(0.);
    vec3 sum = vec3(0);
  	vec3 c = vec3(1.+dot(ray.rd,normal));
    for( int i=0; i<20; i++ )
    {
        r = map(p);
        if (r.x > .01) break;
        p += ray.rd*.005;
        vec3 col = c;
        col.rgb *= smoothstep(.0,0.1,-r.x);
        sum += abs(col);
    }
    return sum;
}


vec3 calcNormal(vec3 pos) 
{
	const vec3 eps = vec3(0.005, 0.0, 0.0);
                          
    return normalize(
        vec3(map(pos + eps).x - map(pos - eps).x,
             map(pos + eps.yxz).x - map(pos - eps.yxz).x,
             map(pos + eps.yzx).x - map(pos - eps.yzx).x ) 
    );
}

vec3 render(Ray ray) 
{
    vec3 col = vec3(0.);
	vec2 res = march(ray);
   
    if (res.x > MAXDIST) 
    {
        return col;
    }
    
    vec3 p = ray.ro+res.x*ray.rd;
    vec3 normal = calcNormal(p);
    vec3 pos = p;
    ray.ro = pos;
   	col = vec3(1.+dot(ray.rd,normal))*1.2;
    
    col = mix(col, vec3(0.), clamp((res.x*res.x)/80., 0., 1.));
   	return col;
}
mat3 camera(in vec3 ro, in vec3 rd, float rot) 
{
	vec3 forward = normalize(rd - ro);
    vec3 worldUp = vec3(sin(rot), cos(rot), 0.0);
    vec3 x = normalize(cross(forward, worldUp));
    vec3 y = normalize(cross(x, forward));
    return mat3(x, y, forward);
}

// This buffer is the feedback loop

vec3 hue(vec3 color, float shift) {

    const vec3  kRGBToYPrime = vec3 (0.299, 0.587, 0.114);
    const vec3  kRGBToI     = vec3 (0.596, -0.275, -0.321);
    const vec3  kRGBToQ     = vec3 (0.212, -0.523, 0.311);

    const vec3  kYIQToR   = vec3 (1.0, 0.956, 0.621);
    const vec3  kYIQToG   = vec3 (1.0, -0.272, -0.647);
    const vec3  kYIQToB   = vec3 (1.0, -1.107, 1.704);

    // Convert to YIQ
    float   YPrime  = dot (color, kRGBToYPrime);
    float   I      = dot (color, kRGBToI);
    float   Q      = dot (color, kRGBToQ);

    // Calculate the hue and chroma
    float   hue     = atan (Q, I);
    float   chroma  = sqrt (I * I + Q * Q);

    // Make the user's adjustments
    hue += shift;

    // Convert back to YIQ
    Q = chroma * sin (hue);
    I = chroma * cos (hue);

    // Convert back to RGB
    vec3    yIQ   = vec3 (YPrime, I, Q);
    color.r = dot (yIQ, kYIQToR);
    color.g = dot (yIQ, kYIQToG);
    color.b = dot (yIQ, kYIQToB);

    return color;
}
float hash( float n )
{
    return fract(sin(n)*43758.5453123);
}

float noise( in vec2 x )
{
    vec2 p = floor(x);
    vec2 f = fract(x);

    f = f*f*(3.0-2.0*f);

    float n = p.x + p.y*157.0;

    return mix(mix( hash(n+  0.0), hash(n+  1.0),f.x),
               mix( hash(n+157.0), hash(n+158.0),f.x),f.y);
}

void main() {
	if (PASSINDEX == 0)	{


		vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    uv = uv * 2.0 - 1.0;
	    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
	    
	    vec3 camPos = vec3(0. + sin(TIME*4.)*0.045, .5, 10.+ cos(TIME*4.)*0.055);
	    vec3 camDir = camPos + vec3(-.1, .1 + cos(TIME*4.)*0.015, -1. );
	    mat3 cam = camera(camPos, camDir, 0.);
	    
	        vec2 polarUv = (uv * 2.0 - 1.0);
	
	    float angle = atan(polarUv.y, polarUv.x);
	    
	    vec3 rayDir = cam * normalize( vec3(uv, 1. + cos(TIME*4.)*0.05) );
	    
	    Ray ray;
	    ray.ro = camPos;
	    ray.rd = rayDir;
	    
	    vec3 col = render(ray);
	    
		gl_FragColor = vec4(col,1.0);
	}
	else if (PASSINDEX == 1)	{


	   	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    
	    float time = mod(TIME, 1.570795);
	    float val1 = noise(uv*2. + time)*0.0025;
	    float val2 = noise(uv*2. + time - 1.570795)*0.0025;
	    
		
	    // Convert the uv's to polar coordinates to scale up  
	    vec2 polarUv = (uv * 2.0 - 1.0);
	
	    float angle = atan(polarUv.y, polarUv.x);
	    
	    // Scale up the length of the vector by a noise function feeded by the angle and length of the vector
	    float llr = length(polarUv)*0.495;
	    float llg = length(polarUv)*0.4965;
	    float llb = length(polarUv)*0.498;
	 
	    vec3 base = IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).rgb;
	
	    vec2 offsR = vec2(cos(angle)*llr + 0.5, sin(angle)*llr + 0.5);
	    vec2 offsG = vec2(cos(angle)*llg + 0.5, sin(angle)*llg + 0.5);
	    vec2 offsB = vec2(cos(angle)*llb + 0.5, sin(angle)*llb + 0.5);
	    
	    // sample the last texture with uv's slightly scaled up
	    vec3 overlayR = IMG_NORM_PIXEL(BufferB,mod(offsR,1.0)).rgb;
		vec3 overlayG = IMG_NORM_PIXEL(BufferB,mod(offsG,1.0)).rgb;
		vec3 overlayB = IMG_NORM_PIXEL(BufferB,mod(offsB,1.0)).rgb;
		vec3 overlay = vec3(overlayR.r, overlayG.g, overlayB.b);
	
	    // Additively blend the colors together
	    vec4 col = vec4(base + overlay*0.55, 1.0);
	    
	    gl_FragColor = col;
	}
	else if (PASSINDEX == 2)	{


	   	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    vec2 uuv = uv * 2.0 - 1.0;
	    
	    vec4 tex = IMG_NORM_PIXEL(BufferB,mod(uv,1.0));
	    vec4 col = min(vec4(.795), pow(tex, vec4(3.75)));
	    gl_FragColor = mix(vec4(0.), col, 1.-smoothstep(0.,1.,length(uuv)*0.7));
	}
}

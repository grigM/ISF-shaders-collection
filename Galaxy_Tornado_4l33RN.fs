/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "galaxytornado",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4l33RN by vox.  Galaxy Tornado",
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

  ]
}
*/




//-----------------CONSTANTS MACROS-----------------

#define PI 3.14159265359
#define E 2.7182818284
#define GR 1.61803398875
#define MAX_DIM (max(RENDERSIZE.x,RENDERSIZE.y))

//-----------------UTILITY MACROS-----------------

#define time ((sin(float(__LINE__))/PI/GR+1.0/GR/PI/E)*TIME+1000.0)
#define saw(x) (acos(cos(x))/PI)
#define sphereN(uv) (clamp(1.0-length(uv*2.0-1.0), 0.0, 1.0))
#define clip(x) (smoothstep(0.25, .75, x))
#define TIMES_DETAILED (1.0)
#define angle(uv) (atan(uv.y, uv.x))
#define angle_percent(uv) ((angle(uv)/PI+1.0)/2.0)
#define hash(p) (fract(sin(vec2( dot(p,vec2(127.5,313.7)),dot(p,vec2(239.5,185.3))))*43458.3453))

#define flux(x) (vec3(cos(x),cos(4.0*PI/3.0+x),cos(2.0*PI/3.0+x))*.5+.5)
#define rormal(x) (normalize(sin(vec3(time, time/GR, time*GR)+seedling)*.25+.5))
#define rotatePoint(p,n,theta) (p*cos(theta)+cross(n,p)*sin(theta)+n*dot(p,n) *(1.0-cos(theta)))


//-----------------SEEDLINGS-----------------------
float seedling = 0.0;
float stretch = 1.0;
vec2 offset = vec2(0.0);
float last_height = 0.0;
float scale = 1.0;
float extraTurns = 0.0;
float aspect = 1.0;

//-----------------AUDIO ALGORITHM-----------------

float lowAverage()
{
    const int iters = 32;
    float product = 1.0;
    float sum = 0.0;
    
    float smallest = 0.0;
    
    for(int i = 0; i < iters; i++)
    {
        float sound = IMG_NORM_PIXEL(BufferB,mod(vec2(float(i)/float(iters), 0.5),1.0)).r;
        smallest = 
        
        product *= sound;
        sum += sound;
    }
    return max(sum/float(iters), pow(product, 1.0/float(iters)));
}

//-----------------SIMPLEX ALGORITHM-----------------

vec3 random3(vec3 c) {
    float j = 4096.0*sin(dot(c,vec3(17.0, 59.4, 15.0)));
    vec3 r;
    r.z = fract(512.0*j);
    j *= .125;
    r.x = fract(512.0*j);
    j *= .125;
    r.y = fract(512.0*j);
    return r-0.5;
}


float simplex3d(vec3 p) {
    const float F3 =  0.3333333;
    const float G3 =  0.1666667;
    
    vec3 s = floor(p + dot(p, vec3(F3)));
    vec3 x = p - s + dot(s, vec3(G3));
    
    vec3 e = step(vec3(0.0), x - x.yzx);
    vec3 i1 = e*(1.0 - e.zxy);
    vec3 i2 = 1.0 - e.zxy*(1.0 - e);
    
    vec3 x1 = x - i1 + G3;
    vec3 x2 = x - i2 + 2.0*G3;
    vec3 x3 = x - 1.0 + 3.0*G3;
    
    vec4 w, d;
    
    w.x = dot(x, x);
    w.y = dot(x1, x1);
    w.z = dot(x2, x2);
    w.w = dot(x3, x3);
    
    w = max(0.6 - w, 0.0);
    
    d.x = dot(random3(s), x);
    d.y = dot(random3(s + i1), x1);
    d.z = dot(random3(s + i2), x2);
    d.w = dot(random3(s + 1.0), x3);
    
    w *= w;
    w *= w;
    d *= w;
    
    return dot(d, vec4(52.0));
}

//-----------------BASE IMAGE--------------------------

#define R(r)  fract( 4e4 * sin(2e3 * r) )  // random uniform [0,1[
vec4 stars(vec2 uv)
{
    vec4 stars = vec4(0.0);
    for (float i = 0.; i < 32.0; i ++)
    {
        float r = R(i)/ 256.0         // pos = pos(0)  +  velocity   *  t   ( modulo, to fit screen )
        / length( saw( R(i+vec2(.1,.2)) + (R(i+vec2(.3,.5))-.5) * time ) 
                 - saw(uv) );
        stars += r*vec4(flux(r*PI+i), 1.0);
    }
    
    return stars-1.0/16.0;
}

vec4 galaxy(vec2 uv)
{
    vec2 uv0 = uv;
    float r = length(uv);
	uv *= 5.0*(GR);
    
    
    float r1 = log(length(uv)+1.)*2.0;
    float r2 = pow(log(length(uv)+1.)*3.0, .5);
    
    float rotation = TIME*PI*2.0;
    
    float theta1 = atan(uv.y, uv.x)-r1*PI+rotation*.5+seedling;
    float theta2 = atan(uv.y, uv.x)-r2*PI+rotation*.5+seedling;
    
    vec4 color = vec4(flux((seedling*GR+1.0/GR)*time*PI*4.0), 1.0);
    
    vec4 final = (acos(1.0-(cos(theta1)*cos(theta1)+sqrt(cos(theta1+PI)*cos(theta1+PI)))/2.0)*(1.0-log(r1+1.))
              + cos(1.0-(cos(theta2)*cos(theta2)+cos(theta2+PI/2.)*cos(theta2+PI/2.))/2.0)*(1.25-log(r2+1.)))*color;
         
    final.rgba += color;
    
    final /= r1;
    
	final = (clamp(final, 0.0, 1.0));
    
    float weight = clamp(length(clamp(final.rgb, 0.0, 1.0)), 0.0, 1.0);
    return final*smoothstep(0.0, 1.0/GR/PI/E, 1.0-r);
}

//-----------------IMAGINARY TRANSFORMATIONS-----------------

vec2 cmul(vec2 v1, vec2 v2) {
	return vec2(v1.x * v2.x - v1.y * v2.y, v1.y * v2.x + v1.x * v2.y);
}

vec2 cdiv(vec2 v1, vec2 v2) {
	return vec2(v1.x * v2.x + v1.y * v2.y, v1.y * v2.x - v1.x * v2.y) / dot(v2, v2);
}

vec2 mobius(vec2 uv, vec2 multa, vec2 offa, vec2 multb, vec2 offb)
{
    
    //numerator /= (abs(denominator)+1.0);
    
    vec2 quotient = vec2(0.0);
    const int bends = 2;
    for(int i = 0; i < bends; i++)
    {
       	float iteration = float(i)/float(bends);
        vec2 numerator = cmul(uv, multa+sin(vec2(time+seedling-2.0*PI*sin(-iteration+time/GR), time/GR+seedling-2.0*PI*sin(iteration+time)))) + offa
            +sin(vec2(time+seedling-2.0*PI*sin(-iteration+time/GR), time/GR+seedling-2.0*PI*sin(iteration+time)));
        vec2 denominator = cmul(uv, multb+sin(vec2(time+seedling-2.0*PI*sin(-iteration+time/GR), time/GR+seedling-2.0*PI*sin(iteration+time)))) + offb
            +sin(vec2(time+seedling-2.0*PI*sin(-iteration+time/GR), time/GR+seedling-2.0*PI*sin(iteration+time)));
        quotient += (cdiv(numerator, denominator));
    }
    
    vec2 next = quotient;


    float denom = length(fwidth(uv));//max(fwidth(uv.x),fwidth(uv.y));
    denom += 1.0-abs(sign(denom));

    float numer = length(fwidth(next));//min(fwidth(next.x),fwidth(next.y));
    numer += 1.0-abs(sign(numer));

    stretch = denom/numer;
    
    return quotient;
}

//-----------------ITERATED FUNCTION SYSTEM-----------------

vec2 iterate(vec2 uv, vec2 dxdy, out float magnification, vec2 multa, vec2 offa, vec2 multb, vec2 offb)
{
    uv += offset;
    
    vec2 a = uv+vec2(0.0, 		0.0);
    vec2 b = uv+vec2(dxdy.x, 	0.0);
    vec2 c = uv+vec2(dxdy.x, 	dxdy.y);
    vec2 d = uv+vec2(0.0, 		dxdy.y);//((fragCoord.xy + vec2(0.0, 1.0)) / RENDERSIZE.xy * 2.0 - 1.0) * aspect;

    vec2 ma = mobius(a, multa, offa, multb, offb);
    vec2 mb = mobius(b, multa, offa, multb, offb);
    vec2 mc = mobius(c, multa, offa, multb, offb);
    vec2 md = mobius(d, multa, offa, multb, offb);
    
    float da = length(mb-ma);
    float db = length(mc-mb);
    float dc = length(md-mc);
    float dd = length(ma-md);
    
    magnification = stretch;
    
    vec2 final = mobius(uv, multa, offa, multb, offb);
    
    seedling = (floor(final.x)+floor(final.y));
    
    return final;
}
    
void main() {
	if (PASSINDEX == 0)	{


	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    float scale = E;
	    uv = uv*scale-scale/2.0;
	    
	    float aspect = RENDERSIZE.x/RENDERSIZE.y;
	    
	    uv.x *= aspect;
	    
	    vec2 uv0 = uv;
	    
		const int max_iterations = 2;
	    int target = max_iterations;//-int(saw(spounge)*float(max_iterations)/2.0);
	    vec2 multa, multb, offa, offb;
	    
	    float antispeckle = 1.0; 
	    float magnification = 1.0;
	  
		vec4 color = vec4(0.0);
	    float border = 1.0;
	    
	    seedling = 0.0;
	    
	        
	    offset = sin(vec2(time+seedling,
	                      -time-seedling))*(.5/E);
	    
	    border *= (1.0-color.a);//*antispeckle;
	    
	    for(int i = 0; i < max_iterations; i++)
	    {
	        float iteration = float(i)/float(max_iterations);
	
	        multa = cos(vec2(time*1.1, time*1.2)+iteration*PI*4.0);
	        offa = cos(vec2(time*1.3, time*1.4)+iteration*PI*4.0);
	        multb = cos(vec2(time*1.5, time*1.6)+iteration*PI*4.0);
	        offb = cos(vec2(time*1.7, time*1.8)+iteration*PI*4.0);
	
	        seedling = float(i);
	        extraTurns = float(i*i+1);
	
	        uv = iterate(uv0, .5/RENDERSIZE.xy, magnification, multa, offa, multb, offb);
	        
	        antispeckle = stretch;
	
	        stretch = smoothstep(0.0, 1.0/PI/GR, stretch);
	
	        float draw = border*(1.0-color.a);
	
	        float skip = saw(seedling*PI)*stretch;
	
	
	        vec3 p = vec3(saw(uv*PI), sphereN(saw(uv*PI)));
	        
	        
	        color = clamp(color + galaxy((p.xy)*2.0-1.0)*draw*skip+stars(p.xy)*draw, 0.0, 1.0);
	        border *= draw;//*antispeckle;
	
	    }
	
	    color += stars(uv0*2.0-1.0);
	    
	    vec2 o = 0.0*vec2(cos(time), sin(time));
	
	    float a = simplex3d(vec3(uv0, time));
	    vec2 d = vec2(cos(a), sin(a))/MAX_DIM;
	    
	    vec4 back = IMG_NORM_PIXEL(BufferB,mod(gl_FragCoord.xy/RENDERSIZE.xy+d,1.0));
	    float skew = (120.0+64.0)/255.0;
	    if(color.a <= skew)
	        gl_FragColor = clamp((back*(1.0-color.a)/skew+color*color.a/skew), 0.0, 1.0);
	    else
	    	gl_FragColor = vec4(color)*GR+(1.0-color.a)*back;
	}
	else if (PASSINDEX == 1)	{


		vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
		gl_FragColor = clamp(IMG_NORM_PIXEL(BufferA,mod(uv,1.0))-1.0/60.0, 0.0, 1.0);
	}
	else if (PASSINDEX == 2)	{


		vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
		gl_FragColor = IMG_NORM_PIXEL(BufferA,mod(uv,1.0));
	}
}

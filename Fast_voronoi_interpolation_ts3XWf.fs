/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/ts3XWf by michael0884.  Mipmap blur for the win\n[url]JAVASCRIPT: H=location.host;E=(H?window:opener).Effect;P=E.prototype;if(!E.P)E.P=P.Paint;P.Paint=function(...A){for(i=0;i<8;i++)E.P.apply(this,A);};if(!H)close();[/url]",
    "IMPORTED": {
    },
    "INPUTS": [
        {
            "NAME": "iMouse",
            "TYPE": "point2D"
        }
    ],
    "PASSES": [
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferA"
        },
        {
        },
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferB"
        },
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferC"
        },
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferD"
        },
        {
        }
    ]
}

*/


//voronoi particle tracking 

void Check(inout vec4 U, vec2 pos, vec2 dx)
{
    vec4 Unb = SAMPLE(BufferA, pos+dx, size);
    vec2 rpos1 = mod(pos-Unb.xy+size*0.5,size) - size*0.5;
    vec2 rpos2 = mod(pos-U.xy+size*0.5,size) - size*0.5;
    //check if the stored neighbouring particle is closer to this position 
    if(length(rpos1) < length(rpos2))
    {
        U = Unb; //copy the particle info
    }
}

vec4 B(vec2 pos)
{
   return 5.*SAMPLE(BufferD, pos, size);
}

#define size RENDERSIZE.xy
#define SAMPLE(a, p, s) texture((a), (p)/s)

float gauss(vec2 x, float r)
{
    return exp(-pow(length(x)/r,2.));
}
#define SPEED
#define BLASTER
   
#define PI 3.14159265

#ifdef SPEED
//high speed
    #define dt 8.5
    #define P 0.007
#else
//high precision
 	#define dt 2.
    #define P 0.05
#endif

//how many particles per pixel, 1 is max
#define particle_density 1.
#define minimal_density 0.02

const float radius = 2.0;
vec4 B(vec2 pos)
{
   return SAMPLE(BufferD, pos, size);
}


const vec2 damp = vec2(0.000,0.01);
const vec2 ampl = vec2(0.1,1.);

float weight(float t, float log2radius, float gamma)
{
    return exp(-gamma*pow(log2radius-t,2.));
}

//mipmap blur https://www.shadertoy.com/view/WsVGWV
vec4 sample_blured(vec2 uv, float radius, float gamma)
{
    vec4 pix = vec4(0.);
    float norm = 0.001;
    //weighted integration over mipmap levels
    for(float i = 0.; i < 5.; i += 0.5)
    {
        float k = weight(i, log2(1. + radius), gamma);
        pix += k*IMG_NORM_PIXEL(BufferA,mod(uv,1.0),i); 
        norm += k;
    }
    //nomalize
    return pix/norm;
}

//voronoi interpolation
vec4 voronopolation(vec2 pos, float radius)
{
    vec4 particle_param = SAMPLE(BufferA, pos, size);
    float dist = length(mod(pos-particle_param.xy+size*0.5,size) - size*0.5);
    //blur the voronoi texture with a radius proportional to the closest particle distance
    return sample_blured(pos/size,radius*dist,0.25);
}

vec2 V(vec2 pos)
{
    return voronopolation(pos, 1.).zw;
}

vec4 B(vec2 pos)
{
   return SAMPLE(BufferB, pos, size);
}


const vec2 damp = vec2(0.000,0.01);
const vec2 ampl = vec2(0.1,1.);

float weight(float t, float log2radius, float gamma)
{
    return exp(-gamma*pow(log2radius-t,2.));
}

//mipmap blur https://www.shadertoy.com/view/WsVGWV
vec4 sample_blured(vec2 uv, float radius, float gamma)
{
    vec4 pix = vec4(0.);
    float norm = 0.001;
    //weighted integration over mipmap levels
    for(float i = 0.; i < 5.; i += 0.5)
    {
        float k = weight(i, log2(1. + radius), gamma);
        pix += k*IMG_NORM_PIXEL(BufferA,mod(uv,1.0),i); 
        norm += k;
    }
    //nomalize
    return pix/norm;
}

//voronoi interpolation
vec4 voronopolation(vec2 pos, float radius)
{
    vec4 particle_param = SAMPLE(BufferA, pos, size);
    float dist = length(mod(pos-particle_param.xy+size*0.5,size) - size*0.5);
    //blur the voronoi texture with a radius proportional to the closest particle distance
    return sample_blured(pos/size,radius*dist,0.25);
}

vec2 V(vec2 pos)
{
    return voronopolation(pos, 1.).zw;
}

vec4 B(vec2 pos)
{
   return SAMPLE(BufferC, pos, size);
}


const vec2 damp = vec2(0.000,0.01);
const vec2 ampl = vec2(0.1,1.);

float weight(float t, float log2radius, float gamma)
{
    return exp(-gamma*pow(log2radius-t,2.));
}

//mipmap blur https://www.shadertoy.com/view/WsVGWV
vec4 sample_blured(vec2 uv, float radius, float gamma)
{
    vec4 pix = vec4(0.);
    float norm = 0.001;
    //weighted integration over mipmap levels
    for(float i = 0.; i < 5.; i += 0.5)
    {
        float k = weight(i, log2(1. + radius), gamma);
        pix += k*IMG_NORM_PIXEL(BufferA,mod(uv,1.0),i); 
        norm += k;
    }
    //nomalize
    return pix/norm;
}

//voronoi interpolation
vec4 voronopolation(vec2 pos, float radius)
{
    vec4 particle_param = SAMPLE(BufferA, pos, size);
    float dist = length(mod(pos-particle_param.xy+size*0.5,size) - size*0.5);
    //blur the voronoi texture with a radius proportional to the closest particle distance
    return sample_blured(pos/size,radius*dist,0.25);
}

vec2 V(vec2 pos)
{
    return voronopolation(pos, 1.).zw;
}

// Fork of "Lava blaster" by michael0884. https://shadertoy.com/view/WdtXzs
// 2019-11-05 21:20:41

const int KEY_UP = 38;
const int KEY_DOWN  = 40;

float weight(float t, float log2radius, float gamma)
{
    return exp(-gamma*pow(log2radius-t,2.));
}

//mipmap blur https://www.shadertoy.com/view/WsVGWV
vec4 sample_blured(vec2 uv, float radius, float gamma)
{
    vec4 pix = vec4(0.);
    float norm = 0.001;
    //weighted integration over mipmap levels
    for(float i = 0.; i < 10.; i += 0.5)
    {
        float k = weight(i, log2(1. + radius), gamma);
        pix += k*IMG_NORM_PIXEL(BufferA,mod(uv,1.0),i); 
        norm += k;
    }
    //nomalize
    return pix/norm;
}

//voronoi interpolation
vec4 voronopolation(vec2 pos, float radius)
{
    vec4 particle_param = SAMPLE(BufferA, pos, size);
    float dist = length(mod(pos-particle_param.xy+size*0.5,size) - size*0.5);
    //blur the voronoi texture with a radius proportional to the closest particle distance
    return sample_blured(pos/size,radius*dist,0.25);
}

vec4 B(vec2 pos)
{
   return SAMPLE(BufferD, pos, size);
}

//density and velocity
vec3 pdensity(vec2 pos)
{
   vec4 particle_param = SAMPLE(BufferA, pos, size);
   return vec3(particle_param.zw,gauss(pos - particle_param.xy, 0.7*radius));
}

vec2 V(vec2 pos)
{
    return voronopolation(pos, 1.).zw;
}

void main() {
	if (PASSINDEX == 0)	{


	    gl_FragColor = SAMPLE(BufferA, gl_FragCoord.xy, size);
	    
	    //check neighbours 
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(-1,0));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(1,0));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(0,-1));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(0,1));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(-1,-1));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(1,1));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(1,-1));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(1,-1));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(-2,0));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(2,0));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(0,-2));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(0,2));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(-8,0));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(8,0));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(0,-8));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(0,8));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(-32,0));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(32,0));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(0,-32));
	    Check(gl_FragColor, gl_FragCoord.xy, vec2(0,32));
	    gl_FragColor.xy = mod(gl_FragColor.xy,size); //limit the position to the texture
	    
	    //make new particles by diverging existing ones
	 	if(length(mod(gl_FragCoord.xy-gl_FragColor.xy+size*0.5,size) - size*0.5) > 1./minimal_density)
	    {
	        gl_FragColor.xy = gl_FragCoord.xy;
	    }
	
	    vec2 ppos = gl_FragColor.xy;
	
	    vec2 pressure = vec2(B(ppos+vec2(1,0)).z - B(ppos+vec2(-1,0)).z, B(ppos+vec2(0,1)).z - B(ppos+vec2(0,-1)).z);
	    //mouse interaction
	    if(iMouse.z>0.)
	    {
	        float k = gauss(ppos-iMouse.xy, 25.);
	        gl_FragColor.zw = gl_FragColor.zw*(1.-k) + k*0.2*vec2(cos(0.02*TIME*dt), sin(0.02*TIME*dt));
	    }
		
	    #ifdef BLASTER
	     gl_FragColor.zw += 0.002*vec2(cos(0.01*TIME*dt), sin(0.01*TIME*dt))*gauss(ppos-size*vec2(0.5,0.5),8.)*dt;
	    #endif
	    
	    //update the particle
	    gl_FragColor.zw = gl_FragColor.zw*0.9995; // decrease velocity with time
	    gl_FragColor.zw += P*pressure*dt;
	    //smooth velocity
	    vec2 velocity = 0.*B(ppos).xy + gl_FragColor.zw;
	    gl_FragColor.xy += dt*velocity;
	    gl_FragColor.xy = mod(gl_FragColor.xy,size); //limit the position to the texture
	    
	    
	    if(FRAMEINDEX < 1)
	    {
	      	if(mod(gl_FragCoord.xy, vec2(1./particle_density)).x < 1. && mod(gl_FragCoord.xy, vec2(1./particle_density)).y < 1.)
	           gl_FragColor = vec4(gl_FragCoord.xy,0.,0.);
	      
	    }
	}
	else if (PASSINDEX == 1)	{
	}
	else if (PASSINDEX == 2)	{


	    vec4 prev_u = SAMPLE(BufferD, gl_FragCoord.xy, size);
	    
	    vec4 particle_param = SAMPLE(BufferA, gl_FragCoord.xy, size);
	    gl_FragColor.xy =  particle_param.zw;
	    float div = V(gl_FragCoord.xy+vec2(1,0)).x-V(gl_FragCoord.xy-vec2(1,0)).x+V(gl_FragCoord.xy+vec2(0,1)).y-V(gl_FragCoord.xy-vec2(0,1)).y;
	    gl_FragColor.zw = (1.-0.001)*0.25*(B(gl_FragCoord.xy+vec2(0,1))+B(gl_FragCoord.xy+vec2(1,0))+B(gl_FragCoord.xy-vec2(0,1))+B(gl_FragCoord.xy-vec2(1,0))).zw;
	    gl_FragColor.zw += ampl*vec2(div,0.);
	}
	else if (PASSINDEX == 3)	{


	    vec4 prev_u = SAMPLE(BufferB, gl_FragCoord.xy, size);
	    
	    vec4 particle_param = SAMPLE(BufferA, gl_FragCoord.xy, size);
	    gl_FragColor.xy =  particle_param.zw;
	    float div = V(gl_FragCoord.xy+vec2(1,0)).x-V(gl_FragCoord.xy-vec2(1,0)).x+V(gl_FragCoord.xy+vec2(0,1)).y-V(gl_FragCoord.xy-vec2(0,1)).y;
	    gl_FragColor.zw = (1.-0.001)*0.25*(B(gl_FragCoord.xy+vec2(0,1))+B(gl_FragCoord.xy+vec2(1,0))+B(gl_FragCoord.xy-vec2(0,1))+B(gl_FragCoord.xy-vec2(1,0))).zw;
	    gl_FragColor.zw += ampl*vec2(div,0.);
	}
	else if (PASSINDEX == 4)	{


	    vec4 prev_u = SAMPLE(BufferC, gl_FragCoord.xy, size);
	    
	    vec4 particle_param = SAMPLE(BufferA, gl_FragCoord.xy, size);
	    gl_FragColor.xy =  particle_param.zw;
	    float div = V(gl_FragCoord.xy+vec2(1,0)).x-V(gl_FragCoord.xy-vec2(1,0)).x+V(gl_FragCoord.xy+vec2(0,1)).y-V(gl_FragCoord.xy-vec2(0,1)).y;
	    gl_FragColor.zw = (1.-0.001)*0.25*(B(gl_FragCoord.xy+vec2(0,1))+B(gl_FragCoord.xy+vec2(1,0))+B(gl_FragCoord.xy-vec2(0,1))+B(gl_FragCoord.xy-vec2(1,0))).zw;
	    gl_FragColor.zw += ampl*vec2(div,0.);
	}
	else if (PASSINDEX == 5)	{


	   vec3 density = pdensity(gl_FragCoord.xy);
	   vec2 velocity = voronopolation(gl_FragCoord.xy, 1.3).zw;
	   vec4 blur = SAMPLE(BufferD, gl_FragCoord.xy, size);
	    float vorticity = V(gl_FragCoord.xy+vec2(1,0)).y-V(gl_FragCoord.xy-vec2(1,0)).y-V(gl_FragCoord.xy+vec2(0,1)).x+V(gl_FragCoord.xy-vec2(0,1)).x;
	   //gl_FragColor = vec4(SAMPLE(iChannel2, gl_FragCoord.xy, size).xyz  + 0.8*vec3(0.4,0.6,0.9)*vorticity,1.0);
	    if(texelFetch( iChannel2, ivec2(KEY_UP,2), 0 ).x < 0.5)
	    {
	        if(mod(TIME,3.) < 1.5)
	        {
	             gl_FragColor = vec4(10.*abs(velocity.xyy) + vec3(0,0,1.)*0.5*abs(blur.z),1.0);
	        }
	        else
	        {
	            gl_FragColor = vec4(10.*abs(density.xyy) + vec3(0,0,1.)*0.5*abs(blur.z),1.0);
	        }
	    }
	    else
	    {
	     	float l1 = 490.*abs(vorticity);
	        float l2 = 1.-l1;
	        gl_FragColor = vec4(vec3(1.,0.3,0.1)*l1 + 0.*vec3(0.1,0.1,0.1)*l2,1.0);
	    }  
	}

}


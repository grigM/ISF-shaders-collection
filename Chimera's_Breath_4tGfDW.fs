/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/4tGfDW by nimitz.  Single pass fluid simulation with vorticity confinement and coloring. Fullscreen recommended (it's fast enough)\n\nclick and drag to add force and color. (mouse only define in common tab)",
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
        },
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferA"
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


//Chimera's Breath
//by nimitz 2018 (twitter: @stormoid)

/*
	The main interest here is the addition of vorticity confinement with the curl stored in
	the alpha channel of the simulation texture (which was not used in the paper)
	this in turns allows for believable simulation of much lower viscosity fluids.
	Without vorticity confinement, the fluids that can be simulated are much more akin to
	thick oil.
	
	Base Simulation based on the 2011 paper: "Simple and fast fluids"
	(Martin Guay, Fabrice Colin, Richard Egli)
	(https://hal.inria.fr/inria-00596050/document)

	The actual simulation only requires one pass, Buffer A, B and C	are just copies 
	of each other to increase the simulation speed (3 simulation passes per frame)
	and Buffer D is drawing colors on the simulated fluid 
	(could be using particles instead in a real scenario)
*/

#define dt 0.15
#define USE_VORTICITY_CONFINEMENT
//#define MOUSE_ONLY

//Recommended values between 0.03 and 0.2
//higher values simulate lower viscosity fluids (think billowing smoke)
#define VORTICITY_AMOUNT 0.11

float mag2(vec2 p){return dot(p,p);}
vec2 point1(float t) {
    t *= 0.62;
    return vec2(0.12,0.5 + sin(t)*0.2);
}
vec2 point2(float t) {
    t *= 0.62;
    return vec2(0.88,0.5 + cos(t + 1.5708)*0.2);
}

vec4 solveFluid(sampler2D smp, vec2 uv, vec2 w, float time, vec3 mouse, vec3 lastMouse)
{
	const float K = 0.2;
	const float v = 0.55;
    
    vec4 data = textureLod(smp, uv, 0.0);
    vec4 tr = textureLod(smp, uv + vec2(w.x , 0), 0.0);
    vec4 tl = textureLod(smp, uv - vec2(w.x , 0), 0.0);
    vec4 tu = textureLod(smp, uv + vec2(0 , w.y), 0.0);
    vec4 td = textureLod(smp, uv - vec2(0 , w.y), 0.0);
    
    vec3 dx = (tr.xyz - tl.xyz)*0.5;
    vec3 dy = (tu.xyz - td.xyz)*0.5;
    vec2 densDif = vec2(dx.z ,dy.z);
    
    data.z -= dt*dot(vec3(densDif, dx.x + dy.y) ,data.xyz); //density
    vec2 laplacian = tu.xy + td.xy + tr.xy + tl.xy - 4.0*data.xy;
    vec2 viscForce = vec2(v)*laplacian;
    data.xyw = textureLod(smp, uv - dt*data.xy*w, 0.).xyw; //advection
    
    vec2 newForce = vec2(0);
    #ifndef MOUSE_ONLY
    #if 1
    newForce.xy += 0.75*vec2(.0003, 0.00015)/(mag2(uv-point1(time))+0.0001);
    newForce.xy -= 0.75*vec2(.0003, 0.00015)/(mag2(uv-point2(time))+0.0001);
    #else
    newForce.xy += 0.9*vec2(.0003, 0.00015)/(mag2(uv-point1(time))+0.0002);
    newForce.xy -= 0.9*vec2(.0003, 0.00015)/(mag2(uv-point2(time))+0.0002);
    #endif
    #endif
    
    if (mouse.z > 1. && lastMouse.z > 1.)
    {
        vec2 vv = clamp(vec2(mouse.xy*w - lastMouse.xy*w)*400., -6., 6.);
        newForce.xy += .001/(mag2(uv - mouse.xy*w)+0.001)*vv;
    }
    
    data.xy += dt*(viscForce.xy - K/dt*densDif + newForce); //update velocity
    data.xy = max(vec2(0), abs(data.xy)-1e-4)*sign(data.xy); //linear velocity decay
    
    #ifdef USE_VORTICITY_CONFINEMENT
   	data.w = (tr.y - tl.y - tu.x + td.x);
    vec2 vort = vec2(abs(tu.w) - abs(td.w), abs(tl.w) - abs(tr.w));
    vort *= VORTICITY_AMOUNT/length(vort + 1e-9)*data.w;
    data.xy += vort;
    #endif
    
    data.y *= smoothstep(.5,.48,abs(uv.y-0.5)); //Boundaries
    
    data = clamp(data, vec4(vec2(-10), 0.5 , -10.), vec4(vec2(10), 3.0 , 10.));
    
    return data;
}
//Chimera's Breath
//by nimitz 2018 (twitter: @stormoid)

//see "Common" tab for fluid simulation code

float length2(vec2 p){return dot(p,p);}
mat2 mm2(in float a){float c = cos(a), s = sin(a);return mat2(c,s,-s,c);}

//Chimera's Breath
//by nimitz 2018 (twitter: @stormoid)

//see "Common" tab for fluid simulation code

float length2(vec2 p){return dot(p,p);}
mat2 mm2(in float a){float c = cos(a), s = sin(a);return mat2(c,s,-s,c);}

//Chimera's Breath
//by nimitz 2018 (twitter: @stormoid)

//see "Common" tab for fluid simulation code

float length2(vec2 p){return dot(p,p);}
mat2 mm2(in float a){float c = cos(a), s = sin(a);return mat2(c,s,-s,c);}

//Chimera's Breath
//by nimitz 2018 (twitter: @stormoid)

//see "Common" tab for fluid simulation code

mat2 mm2(in float a){float c = cos(a), s = sin(a);return mat2(c,s,-s,c);}

//shader incoming relating to this palette
vec3 getPalette(float x, vec3 c1, vec3 c2, vec3 p1, vec3 p2)
{
    float x2 = fract(x/2.0);
    x = fract(x);   
    mat3 m = mat3(c1, p1, c2);
    mat3 m2 = mat3(c2, p2, c1);
    float omx = 1.0-x;
    vec3 pws = vec3(omx*omx, 2.0*omx*x, x*x);
    return clamp(mix(m*pws, m2*pws, step(x2,0.5)),0.,1.);
}

vec4 pal(float x)
{
    vec3 pal = getPalette(-x, vec3(0.2, 0.5, .7), vec3(.9, 0.4, 0.1), vec3(1., 1.2, .5), vec3(1., -0.4, -.0));
    return vec4(pal, 1.);
}

vec4 pal2(float x)
{
    vec3 pal = getPalette(-x, vec3(0.4, 0.3, .5), vec3(.9, 0.75, 0.4), vec3(.1, .8, 1.3), vec3(1.25, -0.1, .1));
    return vec4(pal, 1.);
}

// Chimera's Breath
// by nimitz 2018 (twitter: @stormoid)
// https://www.shadertoy.com/view/4tGfDW
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License
// Contact the author for other licensing options

/*
	Simulation code is in the "common" tab (and extra defines)

	The main interest here is the addition of vorticity confinement with the curl stored in
	the alpha channel of the simulation texture (which was not used in the paper)
	this in turns allows for believable simulation of much lower viscosity fluids.
	Without vorticity confinement, the fluids that can be simulated are much more akin to
	thick oil.
	
	Base Simulation based on the 2011 paper: "Simple and fast fluids"
	(Martin Guay, Fabrice Colin, Richard Egli)
	(https://hal.inria.fr/inria-00596050/document)

	The actual simulation only requires one pass, Buffer A, B and C	are just copies 
	of each other to increase the simulation speed (3 simulation passes per frame)
	and Buffer D is drawing colors on the simulated fluid 
	(could be using particles instead in a real scenario)
*/

void main() {
	if (PASSINDEX == 0)	{
	}
	else if (PASSINDEX == 1)	{


	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	    vec2 w = 1.0/RENDERSIZE.xy;
	    
	    vec4 lastMouse = texelFetch(BufferC, ivec2(0,0), 0);
	    vec4 data = solveFluid(BufferC, uv, w, TIME, iMouse.xyz, lastMouse.xyz);
	    
	    if (FRAMEINDEX < 20)
	    {
	        data = vec4(0.5,0,0,0);
	    }
	    
	    if (gl_FragCoord.y < 1.)
	        data = iMouse;
	    
	    gl_FragColor = data;
	    
	}
	else if (PASSINDEX == 2)	{


	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	    vec2 w = 1.0/RENDERSIZE.xy;
	    
	    vec4 lastMouse = texelFetch(BufferA, ivec2(0,0), 0);
	    vec4 data = solveFluid(BufferA, uv, w, TIME, iMouse.xyz, lastMouse.xyz);
	    
	    if (FRAMEINDEX < 20)
	    {
	        data = vec4(0.5,0,0,0);
	    }
	    
	    if (gl_FragCoord.y < 1.)
	        data = iMouse;
	    
	    gl_FragColor = data;
	    
	}
	else if (PASSINDEX == 3)	{


	    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
	    vec2 w = 1.0/RENDERSIZE.xy;
	    
	    vec4 lastMouse = texelFetch(BufferB, ivec2(0,0), 0);
	    vec4 data = solveFluid(BufferB, uv, w, TIME, iMouse.xyz, lastMouse.xyz);
	    
	    if (FRAMEINDEX < 20)
	    {
	        data = vec4(0.5,0,0,0);
	    }
	    
	    if (gl_FragCoord.y < 1.)
	        data = iMouse;
	    
	    gl_FragColor = data;
	    
	}
	else if (PASSINDEX == 4)	{


	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    vec2 mo = iMouse.xy / RENDERSIZE.xy;
	    vec2 w = 1.0/RENDERSIZE.xy;
	    
	    vec2 velo = textureLod(BufferA, uv, 0.).xy;
	    vec4 col = textureLod(BufferD, uv - dt*velo*w*3., 0.); //advection
	    if (gl_FragCoord.y < 1. && gl_FragCoord.x < 1.)
	        col = vec4(0);
	    vec4 lastMouse = texelFetch(BufferD, ivec2(0,0), 0).xyzw;
	    
	    if (iMouse.z > 1. && lastMouse.z > 1.)
	    {
	        float str = smoothstep(-.5,1.,length(mo - lastMouse.xy/RENDERSIZE.xy));   
	        col += str*0.0009/(pow(length(uv - mo),1.7)+0.002)*pal2(-TIME*0.7);
	    }
	    
	    #ifndef MOUSE_ONLY
	    col += .0025/(0.0005+pow(length(uv - point1(TIME)),1.75))*dt*0.12*pal(TIME*0.05 - .0);
	    col += .0025/(0.0005+pow(length(uv - point2(TIME)),1.75))*dt*0.12*pal2(TIME*0.05 + 0.675);
	    #endif
	    
	    
	    if (FRAMEINDEX < 20)
	    {
	        col = vec4(0.);
	    }
	    
	    col = clamp(col, 0.,5.);
	    col = max(col - (0.0001 + col*0.004)*.5, 0.); //decay
	    
	    if (gl_FragCoord.y < 1. && gl_FragCoord.x < 1.)
	        col = iMouse;
	
	    gl_FragColor = col;
	    
	}
	else if (PASSINDEX == 5)	{


	    vec4 col = textureLod(BufferD, gl_FragCoord.xy/RENDERSIZE.xy, 0.);
	    if (gl_FragCoord.y < 1. || gl_FragCoord.y >= (RENDERSIZE.y-1.))
	        col = vec4(0);
	    gl_FragColor = col;
	}

}

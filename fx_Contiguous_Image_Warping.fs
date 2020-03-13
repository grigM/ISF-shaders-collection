/*
{
  "IMPORTED" : [
    
    {
      "NAME" : "iChannel0",
      "PATH" : "f735bee5b64ef98879dc618b016ecf7939a5756040c2cde21ccb15e69a6e1cfb.png"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wlj3DK by TimoKinnunen.  Testing image warping controlled by a grid of movable points.",
  "INPUTS" : [
  		{
     		"NAME" : "inputImage",
      		"TYPE" : "image"
    	},
    	
    	{
			"NAME": "warp_grid_size",
			"TYPE": "float",
			"DEFAULT": 200.0,
			"MIN": 64.0,
			"MAX": 1000.0
		},
		
    	{
     		"NAME" : "pixelOffset_dist_sin_speed",
      		"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 16.0
    	},
    	
    	{
     		"NAME" : "pixelOffset_dist_sin_phase_offset",
      		"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.5,
			"MAX": 2.4
    	},
    	
    	
    	
		{
     		"NAME" : "animate_distortion",
      		"TYPE" : "bool",
      		"DEFAULT": true,
    	},
    	
    	
    	
    	
    	
		{
			"NAME": "dist_grid_sin_move_speed",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 2.0
		},
		
		{
			"NAME": "dist_grid_sin_amp",
			"TYPE": "float",
			"DEFAULT": 6.2831853,
			"MIN": 0.0,
			"MAX": 10.0
		},

		

	    {
    		"NAME" : "showGrid",
      		"TYPE" : "bool",
      		"DEFAULT": false,
    	}
  ]
}
*/


// Testing grid-based linear image warping / distortion.

// Based on Faster Voronoi Edge Distance https://www.shadertoy.com/view/llG3zy by tomkh

// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// by Tomasz Dobrowolski' 2016

// Based on https://www.shadertoy.com/view/ldl3W8 by Inigo Quilez
// And his article: http://www.iquilezles.org/www/articles/voronoilines/voronoilines.htm

#define ANIMATE

// How far cells can go off center during animation (must be <= .5)
//#define ANIMATE_D 1.0

// Points cannot be closer than sqrt(EPSILON)
#define EPSILON .00001

vec2 hash2(vec2 p)
{
   
       // Dave Hoskin's hash as in https://www.shadertoy.com/view/4djSRW
       //vec3 p3 = fract(vec3(p.xyx) * vec3(.1031, .1030, .0973));
       //p3 += dot(p3, p3.yzx+19.19);
       //vec2 o = fract(vec2((p3.x + p3.y)*p3.z, (p3.x+p3.z)*p3.y));
    
       // Texture-based
       vec2 o = IMG_NORM_PIXEL(iChannel0,mod((p+0.5)/256.0,1.0),-100.0).xy;
    
    if(animate_distortion){
       o = 0.5 + dist_grid_sin_move_speed*sin( TIME + o* dist_grid_sin_amp );
    }
   return o;
}
vec2 mg;
vec2 mo;
float me1;
float me2;
//---------------------------------------------------------------
// 5x5 scan in both passes = most accurate
//---------------------------------------------------------------

vec3 voronoi( in vec2 x )
{
    vec2 n = floor(x);
    vec2 f = fract(x);

    //----------------------------------
    // first pass: regular voronoi
    //----------------------------------
	vec2 mr;

    float md0 = 80.0;
    for( int j=-2; j<=2; j++ )
    for( int i=-2; i<=2; i++ )
    {
        vec2 g = vec2(i,j);
        vec2 o = hash2( n + g );
        vec2 r = g + o - f;
        float d = dot(r,r);

        if( d<md0 )
        {
            md0 = d;
            mr = r;
            mg = g;
            mo = o;
        }
    }

    //----------------------------------
    // second pass: distance to borders
    //----------------------------------
    me1 = 80.0;
    me2 = 80.0;
    for( int j=-2; j<=2; j++ )
    for( int i=-2; i<=2; i++ )
    {
        vec2 g = mg + vec2(i,j);
        vec2 o = hash2( n + g );
        vec2 r = g + o - f;

        if( dot(mr-r,mr-r)>EPSILON ) { // skip the same cell
            float d = dot( 0.5*(mr+r), normalize(r-mr) );
	        me2 = min(me2, max(me1, d));
	        me1 = min(me1, d);
        }
    }

    return vec3( me1, mr );
}

vec3 plot(vec2 U)
{
    mg = vec2(-1);
    mo = vec2(-1);
    me1 = 10000.0;
    me2 = 10000.0;
    float size = warp_grid_size;
    vec2 p = U/size;
    vec2 R = RENDERSIZE.xy;
    vec3 c = voronoi( p );
    // TODO: cleanup here
    vec2 n = floor(p);
    vec2 f = fract(p);
    vec2 cn = floor(p)+mg;
    vec2 cf = mo;
    vec2 ro = (cn+cf)*size;
    vec2 pixelOffset = ((cf-0.5)/R*size)*size*0.25;

    pixelOffset = (hash2(-cn)-0.5)*size*(.5+.5*sin((TIME*pixelOffset_dist_sin_speed)+pixelOffset_dist_sin_phase_offset));

    // Distances ...
    float d = length(c.yz);

    // ... to center
    float dc1 = clamp(exp2(-d),0.,1.);
    float dc2 = smoothstep(0.75,1.0,dc1);
    float dc3 = clamp(4.*dot(c.yz,c.yz),0.,1.);
    float dc4 = 1.-clamp(1.75*d-0.25,0.,1.);
    float dc5 = clamp(1.125-1.5*d,0.,1.);

    // ... to edges
    float de1 = clamp(c.x*2.,0.,1.);
    float de2 = clamp(c.x*64.-0.0625,0.,1.);
    float de3 = clamp(c.x*32.-0.0625,0.,1.);
    float de4 = smoothstep(-0.2*0.1,0.5*0.0625,c.x);
    float de5 = smoothstep(-0.2*0.4,0.5*0.125 ,c.x);
    float de6 = smoothstep(-0.2*0.6,0.5*0.25  ,c.x);
    float de7 = clamp(de2*de3*de4*de5*de6,0.,1.);

    // ... to both
    float d10 = dc5*de7;

    // ... to corner
    float df1 = clamp(2.-4.*sqrt(sqrt(me2- me1)),0.,1.);
    df1 *= df1;df1 *= df1;
    
    // Coordinates
    vec2 poff = ro*1.;
    vec2 uv0 = (U+mod(poff,21.)-10.)/R;
    vec2 p1 = p;
    vec2 uv1 = U/R;
    vec2 p2 = p1 - 0.5 * (0.5 - uv0);
    vec2 uv2 = (U+pixelOffset)/R;
    vec2 p3 = mix(p1,p2,d10);
    vec2 uv3 = mix(uv1,uv2,de1);

	
	
    // Colors:
    vec3 interior = vec3(.2,.8,1.);
    vec3 border = vec3(.2,.8,.4);
    vec3 point = vec3(1.,.7,0.);
    vec3 pattern = c.x*2.*(sin(c.x*120.)*.1+.9)*interior;
    //vec3 col = IMG_NORM_PIXEL(inputImage,mod(uv3,1.0)).xyz;
    
    
    
    vec3 col = IMG_NORM_PIXEL(inputImage, uv3).xyz;
    
    
        
    if (showGrid) // show grid
    {
    	
		vec2 g = abs(fract(p)-.5);
        col = mix(clamp(col,0.,1.),vec3(.8),smoothstep(.5-1.5/size,.5,max(g.x,g.y)));
        col = mix(col,point,smoothstep(2.,1.5,distance(U,ro)));
        col = mix(col,border,vec3(1.-de2));
	    col = mix(col,point,vec3(df1));
	    
    }
    return col;
}

void main() {



    gl_FragColor = vec4(plot(gl_FragCoord.xy), 1.);
}

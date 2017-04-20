/*
{
  "CATEGORIES" : [
    "Automatically Converted"
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
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34710.2"
}
*/



#extension GL_OES_standard_derivatives : enable


// port from https://www.shadertoy.com/view/lsySzd
// "Volumetric explosion" by Duke
//-------------------------------------------------------------------------------------
// Based on "Supernova remnant" (https://www.shadertoy.com/view/MdKXzc) 
// and other previous shaders 
// otaviogood's "Alien Beacon" (https://www.shadertoy.com/view/ld2SzK)
// and Shane's "Cheap Cloud Flythrough" (https://www.shadertoy.com/view/Xsc3R4) shaders
// Some ideas came from other shaders from this wonderful site
// License: Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License
//-------------------------------------------------------------------------------------
// modified by @kruhft

//-------------------
#define pi 3.14159265
#define R(p, a) p*cos(a)*p*sin(a)*vec2(p.y, -p.x)

float t = 0.0;
float magic = 10000000.;
float noise()
{
    t = t + magic;
    return sin(t);
}

float fbm( vec3 p )
{
   return noise();
}

float Sphere( vec3 p, float r )
{
   return length(p)*(.00000005*r);
}

//==============================================================
// otaviogood's noise from https://www.shadertoy.com/view/ld2SzK
//--------------------------------------------------------------
// This spiral noise works by successively adding and rotating sin waves while increasing frequency.
// It should work the same on all computers since it's not based on a hash function like some other noises.
// It can be much faster than other noise functions if you're ok with some repetition.
const float nudge = 8.8;	// size of perpendicular vector
float normalizer = 1.0 / sqrt(2.0 + nudge*nudge);	// pythagorean theorem on that perpendicular to maintain scale
float SpiralNoiseC(vec3 p)
{
    float n = -mod(TIME * 0.2,-3.); // noise amount
    float iter = 1.0;
    for (int i = 0; i < 9; i++)
    {
        // add sin and cos scaled inverse with the frequency
        n += -abs(sin(p.y*iter) + cos(p.x*iter)) / iter;	// abs for a ridged look
        // rotate by adding perpendicular and scaling down
        p.xy += vec2(p.y, -p.x) * nudge;
        p.xy *= normalizer;
        // rotate on other axis
        p.xz += vec2(p.z, -p.x) * nudge;
        p.xz *= normalizer;
        // increase the frequency
        iter *= 1.73453;
    }
    return n;
}

float VolumetricExplosion(vec3 p)
{
    float final = Sphere(p,1.);
    final += fbm(p);
    final += SpiralNoiseC(p.zxy*0.4132+3.)*7.0; //1.25;

    return final;
}

float map(vec3 p) 
{
	R(p.xz, mouse.x*0.008*pi+TIME*TIME*0.1);

	float VolExplosion = VolumetricExplosion(p/0.5)*0.5; // scale
    
	return VolExplosion;
}

//--------------------------------------------------------------

// assign color to the media
vec3 computeColor( float density, float radius )
{
	// color based on density alone, gives impression of occlusion within
	// the media
	vec3 result = mix( vec3(1.0,0.5,0.8), vec3(0.4,0.5,0.9), density*density );
	
	// color added to the media
	vec3 colCenter = 2.*vec3(0.1,1.0,1.0);
	vec3 colEdge = .1*vec3(0.48,0.5,0.5);
	result *= mix( colCenter, colEdge, min( (radius+.9)/.9, 10.25 ) );
	
	return result;
}

bool RaySphereIntersect(vec3 org, vec3 dir, out float near, out float far)
{
	float b = dot(dir, org);
	float c = dot(org, org) - 8.;
	float delta = b*b - c;
	if( delta < 0.0) 
		return false;
	float deltasqrt = sqrt(delta);
	near = -b - deltasqrt;
	far = -b + deltasqrt;
	return far > 0.0;
}

void main( void )
{  

    	vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    
	// ro: ray origin
	// rd: direction of the ray
	vec3 rd = normalize(vec3((gl_FragCoord.xy-0.5*RENDERSIZE.xy)/RENDERSIZE.y, 1.0));
	vec3 ro = vec3(0., 0., -3.);
    	
	// Dithering
	vec2 seed = uv + fract(TIME);
	
	// ld, td: local, total density 
	// w: weighting factor
	float ld=0., td=0., w=0.;

	// t: length of the ray
	// d: distance function
	float d=1000., t=0.;
    
    	const float h = 0.3;
   
	vec4 sum = vec4(0.0);
   
    	float min_dist=0.0, max_dist=10.0;

   	if(RaySphereIntersect(ro, rd, min_dist, max_dist))
    	{
       
	t = min_dist*step(t,min_dist);
   
	// raymarch loop
    	for (int i=0; i<170; i++)
	{ 
	    vec3 pos = ro + t*rd;
  
	    // Loop break conditions.
	    if(td>0.9 || d<0.05*t || t>10. || sum.a > 0.99 || t>max_dist) break;
        
	    // evaluate distance function
	    float d = map(pos);
        
	    if (uv.x<1.0)
	    {
            	d = abs(d)+0.07;    
	    }
        
	    // change this string to control density 
	    d = max(d,0.1);
        
	    // point light calculations
	    vec3 ldst = vec3(0.0)-pos;
	    float lDist = max(length(ldst), 0.1);
	    
	    // the color of light 
	    vec3 lightColor=vec3(1.0,.5,0.25);
        
	    sum.rgb+=(lightColor/exp(lDist*lDist*lDist*.008)/3.5); // bloom
        
	    if (d<h) 
	    {
	    // compute local density 
	    ld = h - d;
            
	    // compute weighting factor 
	    w = (1.1 - td) * ld;
     
	    // accumulate density
	    td += w + 1./210.;
		
	    vec4 col = vec4( computeColor(td,lDist), td );
            
            // emission
            sum += sum.a * vec4(sum.rgb, 0.0) * 0.6 / lDist;	
            
	    // uniform scale density
	    col.a *= 0.2;
	    // colour by alpha
	    col.rgb *= col.a;
	    // alpha blend in contribution
	    sum = sum + col*(1.0 - sum.a);  
       
	    }
      
	    td += 1./70.;
		
	    // Dithering
	    d=abs(d)*(.8+0.2*noise());
	    
	    // trying to optimize step siz
	    t += max(d * 0.04 * max(min(length(ldst),d),2.0), 0.01);   
	}
    
	// simple scattering
	sum *= 1.7 / exp( ld * 0.1 ) * 0.05;
	
   	sum = clamp( sum, 0.0, 1.0 );
   
	sum.xyz = sum.xyz*sum.xyz*(3.0-2.0*sum.xyz);
    
	}
   
	gl_FragColor = vec4(sum.xyz,1.0);

}
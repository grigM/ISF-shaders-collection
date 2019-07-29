/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36715.0"
}
*/


// raytracer for shader-studio

#ifdef GL_ES
precision mediump float;
#endif




vec3 global_color_hack = vec3(0.);

#define ASPECT		RENDERSIZE.x/RENDERSIZE.y
#define EPSILON		.001
#define FOV 		1.6
#define FARPLANE	12.
#define ITERATIONS	128

#define VIEWPOSITION	vec3(0., .0, -7.)
#define VIEWTARGET	vec3(0., -.1, 0.)

#define LIGHTPOSITION	vec3(-4., 4., -8.)
#define LIGHTCOLOR	vec3(.95, 0.95,  0.86)

#define PI		(4.*atan(1.)) 		
#define TAU		(8.*atan(1.)) 		




struct ray
{
	vec3 origin;
	vec3 position;
	vec3 direction;
	vec2 material_range;
}; 
	
struct surface
{
	vec4 color;
	vec3 normal;
	float range;
};	

struct light
{
	vec3 color;
	vec3 position;
	vec3 direction;
	vec3 ambient;
};	

struct material
{
	vec3  color;
	float refractive_index;
	float roughness;
};	
	


	
ray 		emit(ray r);
ray 		view(in vec2 uv);
vec2 		map(in vec3 position);
vec3 		derive(in vec3 p);

material	assign_material(in float material_index);
surface		shade(in ray r, in surface s,  in material m, in light l);
float		fresnel(in float i, in float hdl);	
float		geometry(in float i, in float ndl, in float ndv, in float hdn, in float hdv, in float hdl);
float		distribution(in float r, in float ndh);
float		ambient_occlusion(vec3 p, vec3 n);
float		shadow(vec3 p, vec3 d);

vec3 		sphericalharmonic(vec3 n, in vec4 c[7]);
void 		shcdusk(out vec4 c[7]);
void 		shcday(out vec4 c[7]);

float 		sphere(vec3 position, float radius);
float 		cube(vec3 position, vec3 scale);
float 		kaleidoscopic_ifs(vec3 position, vec3 rotation);

mat2 		rmat(in float r);
mat3 		rmat(in vec3 r);




void main( void ) 
{
	vec2 uv		= gl_FragCoord.xy/RENDERSIZE.xy;

	ray r		= view(uv);
	
	r		= emit(r);
	
	vec4 result	= vec4(0.);
	
	if(r.material_range.x != 0. && r.material_range.y < EPSILON)
	{		
		surface	s	= surface(vec4(0.), vec3(0.), 0.);
		s.color		= result;
		s.range		= distance(r.position, r.origin);
		s.normal 	= derive(r.position);

		material m	= assign_material(r.material_range.x);
		
		light l		= light(vec3(0.), vec3(0.), vec3(0.), vec3(0.));
		l.color		= LIGHTCOLOR;	
		l.position	= LIGHTPOSITION;
		l.direction	= normalize(l.position-r.position);
		
		vec4 c[7];
		shcdusk(c);
		//shcday(c);
		
		l.ambient	= sphericalharmonic(s.normal, c);
		
		
		s		= shade(r, s, m, l);
		
		
		result		= s.color;
	}
	else
	{
		vec4 c[7];
		shcdusk(c);
		//shcday(c);
		
		
		result.xyz 	=  sphericalharmonic(r.direction, c);
		result.w 	= 0.;
	}
	
	
	gl_FragColor = result;
}// sphinx




//// SCENES

vec2 map(in vec3 position)
{
	float k = 1e1;
	vec3 p = position;
	for(float i = 0.; i <= 1.; i += 1./12.0){
		k = min(k, cube(position+vec3(0., -i-(3.0*cos(TIME))*cos(TIME+p.x/2.-0.25*p.z), -(3.0*cos(TIME))), vec3(3., .025, 3.)));
	}
	
	return vec2(1., k);
}

//(these functions are what gets rendered - swap them out)
#ifdef KALISET_SCENE
vec2 map(in vec3 position)
{
	vec3 rotation		= vec3(.09, .21, .14) * TIME;	
	float k			= kaleidoscopic_ifs(position, rotation);
	
	vec2 material_range 	= vec2(0.);
	
	material_range.x	= 3.;
	material_range.y	= k;
	
	return material_range;
}
#endif


#ifdef SPHERE_SCENE
vec2 map(in vec3 position)
{
	vec2 material_range 	= vec2(0.);
	
	float s 		= sphere(position+vec3(0., -.25 + cos(TIME) * .05, 0.), 2.);
	float c 		= cube(position+vec3(0., 2., 0.), vec3(3., .025, 3.));
	
	material_range.x	= s < c ? 1. : 2.;
	material_range.y	= min(c, s);

	return material_range;
}
#endif
//// SCENES




//// RENDERING
//emit rays to map the scene, stepping along the direction of the ray by the  of the nearest object until it hits or goes to far
ray emit(ray r)
{
	float range = 0.;
	float threshold = EPSILON * 1./float(ITERATIONS);
	for(int i = 0; i < ITERATIONS; i++)
	{
		r.material_range = map(r.position);
		
		if(r.material_range.y < threshold || range > FARPLANE)
		{
			break;	
		}
            
		range		+= r.material_range.y * .9;

		threshold  	*= 1.03;  
		r.position 	= r.origin + r.direction * range;

	}
	
	return r;
}


//transform the pixel positions into rays 
ray view(in vec2 uv)
{ 
	uv			= uv * 2. - 1.;
	uv.x 			*= RENDERSIZE.x/RENDERSIZE.y;
    	
	vec3 w			= normalize(VIEWTARGET-VIEWPOSITION);
	vec3 u			= normalize(cross(w,vec3(0.,1.,0.)));
	vec3 v			= normalize(cross(u,w));
    
	ray r 			= ray(vec3(0.), vec3(0.), vec3(0.), vec2(0.));
	r.origin		= VIEWPOSITION;
	r.position		= VIEWPOSITION;
	r.direction		= normalize(uv.x*u + uv.y*v + FOV*w);;
	r.material_range	= vec2(0.);
	
	return r;
}	


//find the normal by comparing offset samples on each axis as a partial derivative
vec3 derive(in vec3 p)
{
	vec2 offset 	= vec2(0., EPSILON);

	vec3 normal 	= vec3(0.);
	normal.x 	= map(p+offset.yxx).y-map(p-offset.yxx).y;
	normal.y 	= map(p+offset.xyx).y-map(p-offset.xyx).y;
	normal.z 	= map(p+offset.xxy).y-map(p-offset.xxy).y;
	
	return normalize(normal);
}
//// RENDERING




//// SHADING
surface shade(in ray r, in surface s,  in material m, in light l)
{
	//http://simonstechblog.blogspot.com/2011/12/microfacet-brdf.html
	
	//view and light vectors
	vec3 view_direction	= normalize(VIEWPOSITION-VIEWTARGET);		//direction into the view
	vec3 half_direction	= normalize(view_direction+l.direction);	//direction halfway between view and light
	
	
	//exposure coefficients
    	float light_exposure   	= dot(s.normal, l.direction);			//ndl
	float view_exposure	= dot(s.normal, view_direction);		//ndv
	
	float half_view   	= dot(half_direction, view_direction);		//hdn	
	float half_normal  	= dot(half_direction, s.normal);		//hdv
	float half_light 	= dot(half_direction, l.direction);		//hdl
   	
    
	//lighting coefficient
	float f     		= fresnel(m.refractive_index, half_light);
	float g     		= geometry(m.roughness, light_exposure, view_exposure, half_normal, half_view, half_light);
	float d     		= distribution(m.roughness, half_normal);

	
	//shadow and occlusion projections
	float occlusion		= ambient_occlusion(r.position, s.normal);
    	float fshadow		= shadow(r.position, l.direction);

	
	//bidrectional reflective distribution function
    	float brdf  		= (f*g*d)/(4.*light_exposure*view_exposure);
	
	
	vec3 ambient_light	= l.ambient * (1.-view_exposure) * (1.-f);
	
	
	s.color.xyz		=  m.color * l.color + brdf * l.color;
	s.color.xyz		*= occlusion * fshadow;
	s.color.xyz 		+= occlusion * ambient_light * .5 * m.color;
	s.color.w		= 1.;
	
	return s;
}


float fresnel(in float i, in float hdl)
{   
	return i + (1.33-i) * pow(1.-max(hdl, 0.), 5.);
}


float geometry(in float i, in float ndl, in float ndv, in float hdn, in float hdv, in float hdl)
{
	float k         = i * sqrt(2./PI);
	float ik        = 1. - k;
	ndv 		= max(0., ndv);
	ndl 		= max(0., ndl);
	return (ndv / (ndv * ik + k)) * (ndl / (ndl * ik + k));
}


float distribution(in float r, in float ndh)
{  
	float m     = 2./(r*r) - 2.;
	return (m+2.) * pow(max(ndh, 0.0), m) / TAU;
}


float ambient_occlusion(vec3 p, vec3 n)
{
	const int iterations = 8;
	float a       = 1.;    //occlusion  
	const float r = .025;   //range
	float d       = 1.-r/float(iterations);
	for(int i = 0; i < iterations; i++ )
	{
        	float hr = r + r * float(i);
	        vec3  op = n * hr + p;
        	float e  = map(op).y;
	        a 	 += (e-hr) * d;
       		d	 *= d;
    }
    return clamp(a, 0., 1. );
}


float shadow(vec3 p, vec3 d)
{
	const int iterations	= 16;
 	float e       		= EPSILON;
	const float u		= 1.;  		   
    	float s 		= 1.;         
    	for( int i=0; i < iterations; i++ )
    	{
    		float l = map(p + d * e).y;
    		s 	= mix(s, min(s, u*l/e),1.15/float(1+i));
    	    	e 	+= .125;
    	}
	return clamp(s, 0.0, 1.);
}


vec3 sphericalharmonic(vec3 n, in vec4 c[7])
{     
    	vec4 p = vec4(n, 1.);
   
    	vec3 l1 = vec3(0.);
    	l1.r = dot(c[0], p);
	l1.g = dot(c[1], p);
	l1.b = dot(c[2], p);
	
	vec4 m2 = p.xyzz * p.yzzx;
	vec3 l2 = vec3(0.);
	l2.r = dot(c[3], m2);
	l2.g = dot(c[4], m2);
	l2.b = dot(c[5], m2);
	
	float m3 = p.x*p.x - p.y*p.y;
	vec3 l3 = vec3(0.);
	l3 = c[6].xyz * m3;
    	
	vec3 sh = vec3(l1 + l2 + l3);
	
	return clamp(sh, 0., 1.);
}

//sh light coefficients
void shcdusk(out vec4 c[7])
{
    c[0] = vec4(0.2, .77, 0.2, 0.45);
	c[1] = vec4(0.2, .63, 0.2, 0.25);
	c[2] = vec4(0.0, .13, 0.1, 0.15);
	c[3] = vec4(0.1, -.1, 0.1, 0.0);
	c[4] = vec4(0.1,-0.1, 0.1, 0.0);
	c[5] = vec4(0.2, 0.2, 0.2, 0.0);
	c[6] = vec4(0.0, 0.0, 0.0, 0.0);
}


void shcday(out vec4 c[7])
{
    c[0] = vec4(0.0, 0.5, 0.0, 0.4);
	c[1] = vec4(0.0, 0.3, .05, .45);
	c[2] = vec4(0.0, 0.3, -.3, .85);
	c[3] = vec4(0.0, 0.2, 0.1, 0.0);
	c[4] = vec4(0.0, 0.2, 0.1, 0.0);
	c[5] = vec4(0.1, 0.1, 0.1, 0.0);
	c[6] = vec4(0.0, 0.0, 0.0, 0.0);   
}
//// SHADING





////MATERIALS
material assign_material(in float material_index)
{
	material m;
	if(material_index == 1.)
	{
		m.color			= vec3(.9, .125, .125);
		m.refractive_index	= .5;
		m.roughness		= .1;
	}
	else if(material_index == 2.)
	{
		
		m.color			= vec3(1.);
		m.refractive_index	= .5;
		m.roughness		= .5;	
	}
	else if(material_index == 3.)
	{
		m.color			= global_color_hack;
		m.refractive_index	= .95;
		m.roughness		= .5;	
	}
	else
	{
		m.color			= vec3(.5);
		m.refractive_index	= .5;
		m.roughness		= .5;	
	}
	return m;
}
////
	



//// DISTANCE FIELD FUNCTIONS
float sphere(vec3 position, float radius)
{
	return length(position)-radius;	
}

float cube(vec3 p, vec3 s)
{
	vec3 d = (abs(p) - s);
  	return min(max(d.x,max(d.y,d.z)),0.0) + length(max(d,0.0));
}

//kaliset fractal
float kaleidoscopic_ifs(vec3 position, vec3 rotation)
{
	float translation	= 1.81;
	float dialation		= 1.;
	
	mat3 rot 		= rmat(rotation);
	rot     		*= translation; 
	
	const int iterations = 10;
	for (int i = 0; i < iterations; i++) 
	{
    		position   *= rot;
		position   = abs(position)-translation;
		dialation  *= translation;
		global_color_hack = position;
        }

	return cube(position, vec3(translation))/dialation;
}
//// DISTANCE FIELD FUNCTIONS

	


//// ROTATION MATRICES
mat2 rmat(in float r)
{
    float c = cos(r);
    float s = sin(r);
    return mat2(c, s, -s, c);
}


//3d rotation matrix
mat3 rmat(in vec3 r)
{
	vec3 a = vec3(cos(r.x)*cos(r.y),sin(r.y),sin(r.x)*cos(r.y));
	
	float c = cos(r.z);
	float s = sin(r.z);
	vec3 as  = a*s;
	vec3 ac  = a*a*(1.- c);
	vec3 ad  = a.yzx*a.zxy*(1.-c);
	mat3 rot = mat3(
		c    + ac.x, 
		ad.z - as.z, 
        	ad.y + as.y,
		ad.z + as.z, 
		c    + ac.y, 
		ad.x - as.x,
		ad.y - as.y, 
		ad.x + as.x, 
		c    + ac.z);
	return rot;	
}
//// ROTATION MATRICES
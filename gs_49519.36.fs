/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#49519.36"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


//rhombic triacontahedron
//hrrrm
//sphinx

#define BRIGHTNESS	1.5
#define GAMMA		.45

#define FLIP_VIEW	(mouse.y < .5 ? 1. : -1.)
#define TARGET_RANGE	30.

#define VIEW_POSITION 	(normalize(vec3(sin((mouse.x-.5)*TAU), sin((mouse.y-.25)*TAU*.5)-.25, -cos((mouse.x-.5)*TAU+TAU*.5))) * TARGET_RANGE) //orbit cam
//#define VIEW_POSITION 	(normalize(vec3(.5, .3, .0)) * TARGET_RANGE)
//#define VIEW_POSITION 	(normalize(vec3(1., .01, 0.)) * TARGET_RANGE) //x
//#define VIEW_POSITION 	(normalize(vec3(0.01, 1., 0.)) * TARGET_RANGE)//-y
//#define VIEW_POSITION 	(normalize(vec3(0.01, 0., 1.)) * TARGET_RANGE)	//z

#define VIEW_TARGET 	vec3(0., 0., 0.)

#define ROTATE		(mouse.x > .8)
#define CUTAWAY		(mouse.y > .8)
#define OUTER		false//(mouse.y < .2)

#define MAX_FLOAT 	(pow(2., 128.)-1.)
	
#define TAU 		(8. * atan(1.))
#define PHI 		((sqrt(5.)+1.)*.5)
#define PHI2 		(PHI*PHI)
#define PHI3 		(PHI*PHI*PHI)

vec4 g_ray		= vec4(0., 0., 0., 1.);
vec3 e_color		= vec3(0., 0., 0.);
bool get_material	= false;

mat2 rmat(float t);

vec3 hsv(in float h, in float s, in float v);

float squaresum(in vec3 v); 
float sum(in vec3 v);
float max_component(vec3 v);
float smoothmin(float a, float b, float x);
float hash(float x);

float edge(vec3 p, vec3 a, vec3 b);
float plane(vec3 p, vec3 n, float d); 
	
float icosahedron(vec3 p);
float dodecahedron(vec3 p);
float rhombictriacontahedron(vec3 p, float r);
float trucatedicosahedron(vec3 p);

vec4 derivative(in vec3 position, in float range);
vec4 derivative2(in vec3 position, in float range);
float exp2fog(float depth, float density);
float shadow(vec3 origin, vec3 direction, float mint, float maxt, float k);
float ambient_occlusion(vec3 position, vec3 normal, float delta, float t, float f);
vec3 gamma_correction(vec3 color, float brightness, float gamma);


float plane(vec3 p, vec3 n, float d) 
{
	return dot(p, n) + d;
}


float icosahedron(vec3 p)
{
	vec4 q 	= vec4(.30901699437, .5, .80901699437, 0.); 	
	p 			= abs(p);
	return max(max(max(dot(p,q.wxz), dot(p, q.yyy)),dot(p,q.zwx)), dot(p,q.xzw));
}


float dodecahedron(vec3 p)
{
	vec3 q 	= vec3(0., .5, .80901699437);	
	p 	= abs(p);	
	return max(max(dot(p, q.yxz), dot(p, q.zyx)),dot(p, q.xzy));
}


float rhombictriacontahedron(vec3 p)
{
	vec3 q 	= vec3(.30901699437, .5,.80901699437);	
	p 	= abs(p);	
	return  max(max(max(max(max(p.x, p.y), p.z), dot(p, q.zxy)), dot(p, q.xyz)), dot(p, q.yzx));
}

float rhombictriacontahedron_edges(vec3 p, float r)
{
	vec4 v 		= vec4(normalize(vec3(PHI3, PHI2, PHI)), 0.) * r;
	vec3 o		= vec3(0., 0., 0.);
	
	float e = MAX_FLOAT;
	p 		= abs(p);
	e		= min(e, edge(p, o, v.yyy)); //center
	e 		= min(e, edge(p, o, v.xyw)); //right
	e 		= min(e, edge(p, o, v.xwz)); //bottom right
	e 		= min(e, edge(p, o, v.ywx)); //bottom center
	e 		= min(e, edge(p, o, v.wzx)); //left
	e 		= min(e, edge(p, o, v.wxy)); //top left
	e 		= min(e, edge(p, o, v.zxw)); //top right
		
	return e;
}

vec3 gcol		=  vec3(0.,0.,0.);
float map(in vec3 position)
{	
	vec3 vp		= position;
	vp.xz		*= rmat(TIME*.1);
	float cut_plane	= MAX_FLOAT;
	if(ROTATE)
	{
		position.xz	*= rmat(TIME*.1);
	}
	
	if(CUTAWAY)
	{
		cut_plane 	= position.z-.1;
	}
	

	
	//position	= mod(abs(position-m*.5), m)-m*.5;
	
	//outer!
	float rtc	= rhombictriacontahedron(position);
//	float tic	= trucatedicosahedron(position);	
	float ddc	= dodecahedron(position.yzx);
	float ico	= icosahedron(position);
	float range	= MAX_FLOAT;

	float k		= 5.;
	ddc		-= k;
	ico		-= k-.01;
	rtc		-= k + 1.2;
	
	float outer	= max(min(ddc, ico), -rtc);
	
	////
	
	
	//inner - again!
	float ddc2	= dodecahedron(position.yzx);
	float ico2	= icosahedron(position);
	float rtc2	= rhombictriacontahedron(position);
	
	ddc2		-= k / PHI2; 
	ico2		-= k / PHI2;
	rtc2		-= k / PHI2;

	float inner 	= max(min(ddc2-.05, ico2), -rtc2+.49);

	////

	float spokes 	= max(-rtc2+.5, rhombictriacontahedron_edges(position, 7.55));
 
	
	if(OUTER)
	{
		range		= outer;
	}
	else
	{	
		
		range		= min(inner, outer);
		range 		= min(range, spokes);
	}
	
	vec3 g 	= vec3(1.4, .21, .1) * .5;
	vec3 s 	= vec3(.1, .4, .9) * .85;
	vec3 t 	= g;
	float ts =  TIME * 2.;
	float fk = ico;
	if(range != spokes)
	{
		
		
		if(inner < outer)
		{
			float f = cos(fk+.125/abs(ddc+ico));
			g 	= mix(g, s, 1.-f);
			s 	= mix(t, s, f);
			g_ray.xyz	= ddc2 > ico2 + .0125 ? g : s;	
			
			g_ray.xyz	*= !CUTAWAY ?  .35 : .5;
		}
		else
		{
			float f = cos(fk+.125/abs(ddc2+ico2));
			g 	= mix(g, s, 1.-f);
			s 	= mix(t, s, f);
			g_ray.xyz	=  ddc > ico+.0125 ? g : s;
			g_ray.xyz	*= 1.25;
		}
	} 
	else
	{
		vec3 p 	= abs(position);
		vec4 v 	= vec4(normalize(vec3(PHI3, PHI2, PHI)), 0.) * 32.;
		float e = edge(p, vec3(0.,0.,0.), v.wxy);
		e 		= min(e, edge(p, vec3(0.,0.,0.), v.yzw));
	
		
		float f = cos(fk+.125/abs(ico-ddc));
		g 	= mix(g, s, 1.-f);
		s 	= mix(t, s, f);
		g_ray.xyz 	= abs(spokes-e) < .05 ? g : s;
		range 		= min(range, e);
	}
	
	gcol+=g_ray.xyz*.5;

	range -= .05;
	if(CUTAWAY)
	{
		return max(cut_plane+.1, range);
	}
	else
	{
		return range;
	}
}


float map2(in vec3 position)
{	
	
	if(ROTATE)
	{
		position.xz	*= rmat(TIME*.1);
	}
	float k 		= 5.;

	float rtc_outer 	= rhombictriacontahedron(position) - k - 1.309;
	
	float range		= MAX_FLOAT;
	range 			= rtc_outer;
	if(CUTAWAY)
	{
		
		float rtc_inner 	= rhombictriacontahedron(position) - k + PHI - 1.;
	
		float outer 		= max(max(position.z-.05, -rtc_inner + PHI), rtc_outer);
		
		float rtc_inner2 	= rhombictriacontahedron(position) - PHI2+.5;
		float rtc_outer2 	= rhombictriacontahedron(position) - PHI2+.15;
		
		float outer2 		= max(max(position.z-.05, -rtc_inner2), rtc_outer2);
		
		if(OUTER)
		{
			range		= outer;
		}
		else
		{
			range 		= min(outer, outer2);
		}
	}
	
	
	return range-.025;
}



void main( void ) 
{
	vec2 aspect			= RENDERSIZE.xy/RENDERSIZE.yy;
	vec2 uv 			= gl_FragCoord.xy/RENDERSIZE.xy;
	vec2 screen			= (uv - .5) * aspect;
	
	vec2 m				= (mouse-.5) * aspect;
	
	
	float field_of_view		= PHI;
	
	vec3 w          		= normalize(VIEW_POSITION-VIEW_TARGET);
	vec3 u          		= normalize(cross(w,vec3(0.,1.,0.)));
	vec3 v          		= normalize(cross(u,w));

	vec3 direction     		= normalize(screen.x * u + screen.y * v + field_of_view * -w);	
	vec3 origin			= VIEW_POSITION;
	vec3 position			= origin;
	

	vec3 position2			= origin;
	
	//sphere trace	
	float minimum_range		= 2./max(RENDERSIZE.x, RENDERSIZE.y);
	float surface_threshold		= minimum_range;
	float max_range			= 512.;
	float range			= max_range;

	float total_range		= -1.;
	float steps 			= 1.;
	float range2			= range;
	
	float glass_range		= 0.;
	bool hit			= false;
	const float iterations		= 256.;
	vec3 light_position 		= VIEW_POSITION+vec3(-PHI3, PHI3, PHI) * 256.;
	vec3 accum			= vec3(0.,0.,0.);
	for(float i = 1.; i < iterations; i++)
	{
		if(range > surface_threshold && total_range < max_range)
		{
			float f			= i/iterations;
			
			range 			= map(position);
			range			*= .85;
			surface_threshold	*= 1.01;
		
			total_range		+= range;			
		
			position 		= origin + direction * total_range;	
			
			
			steps++;			
		}
	
		//glass pass
		range2			= map2(position2);	
		if(range2 < .002)
		{
			if(!hit)
			{
				float light		= 0.;
				vec3 light_direction	= normalize(light_position - position2);	
				vec4 gradient2		= derivative2(position2, .005);
				vec3 normal		= normalize(gradient2.xyz);
				float light_incident 	= max(dot(normal, light_direction), 0.);
				vec3 reflection 	= reflect(direction, normal.xyz);
				float light_specular 	= pow(max(dot(reflection, light_direction), 0.), 12.);
				float light_bounce 	= pow(max(dot(-reflection, light_direction), 0.), .5);
				light			+= light_incident * .5;
				light 			+= light_specular * 6.;
				light 			+= light_bounce * 1.5;
				accum			+= .05 * light + abs(dot(normal, reflection)) * .25;
				float shadows		= shadow(position2, light_direction, .05, 256., 228.)*.25+.75;
				float occlusion		= ambient_occlusion(position2, normal, 5., .0125, .25);

				g_ray.w			+= (light * occlusion + light * shadows) * .25;
				
				glass_range		= length(position2-origin);

			}
			hit		= true;
			position2 	+= .00125 * direction;	
			g_ray.w		-= .00125;
		}
		else
		{			
			
			position2 	+= range2 * direction;		
		}
		g_ray.w		-= .00125;
		//					
	}
	vec3 light_direction	= normalize(light_position - position);
	
	
	
	
	
	//shade		
	vec3 color 			= vec3(0., 0., 0.);

	
	if(steps < iterations-1. && total_range < max_range)
	{		

		vec4 gradient		= derivative(position, .0025);
		
		vec3 surface_direction 	= normalize(gradient.xyz);
	
		
		
		vec4 light_color	= vec4( .97, .95, .93, 1.);
		
			
		vec3 reflection 	= reflect(direction, surface_direction);
		float light_direct	= clamp(dot(surface_direction, light_direction), .5, 1.);
		float light_specular 	= pow(max(dot(light_direction, reflection), -1.), 9.5);
		float light_bounce 	= pow(max(dot(-light_direction, reflection), 0.), 2.);
		float light_ambient	= .5;
		

		float fog 		= exp2fog(max_range/(1.+total_range), .25);
		float shadows		= shadow(position, light_direction, .05, 32., 32.) *.125 +.65;
		float occlusion		= ambient_occlusion(position, surface_direction, .25, .5, .125) * .75 + .25;


		get_material		= true;				
		float k 		= map(position - surface_direction.xyz * range);
		
		light_color.xyz		+= light_specular * light_color.xyz;		
		light_color.xyz		+= light_direct   * light_color.xyz;		
		light_color.xyz		+= light_bounce	  * light_color.xyz;
		light_color.xyz		*= light_color.w;
	
		
		color			= g_ray.xyz * .65;		
		color			+= light_direct * .45;		
		color			+= light_specular * .5;
		color			+= light_bounce * .125;
		color			+= fog * light_ambient;

		color			*= occlusion * shadows * light_ambient;
		color			+= gradient.w * .5 * light_specular * light_direct;
		color			*= g_ray.xyz*.5+.5;		
		gradient		= derivative(position, .0025);
		color			-= abs(gradient.w * .5) * light_specular * light_direct;
		
		g_ray.w			*= float(total_range < glass_range+0.05);
		color			+= g_ray.w * color;
	}
	else
	{
		color			= vec3(0., 0., 0.);
		color			+= g_ray.w * direction.z * .5 + direction.y * .25;
		color			= max(color, 0.);
	
	}
	
	color.xyz 			+= accum;
	
	color				= gamma_correction(color, BRIGHTNESS, GAMMA);
	
	gl_FragColor 			= vec4(color, 1.);
}//sphinx



float squaresum(in vec3 v) 
{ 
	return dot(v,v); 
}



float sum(in vec3 v) 
{ 
	return dot(v, vec3(1., 1., 1.)); 
}


float smoothmin(float a, float b, float k)
{
//	const float k = 5.8;
        return -log(exp(-k*a)+exp(-k*b))/k;
}


float max_component(vec3 v)
{
	return max(max(v.x, v.y), v.z);
}


vec3 hsv(in float h, in float s, in float v)
{
    return mix(vec3(1.),clamp((abs(fract(h+vec3(3.,2.,1.)/3.)*6.-3.)-1.),0.,1.),s)*v;
}

float hash(in float x)
{
	float k = x * 65537.618034;   	
	return fract(fract(k * x) * k);
}

mat2 rmat(float t)
{
	float c = cos(t);
	float s = sin(t);
	return mat2(c, s, -s, c);
}


float edge(vec3 p, vec3 a, vec3 b)
{
	vec3 pa = p - a;
	vec3 ba = b - a;
	float h = clamp(dot(pa, ba) / dot(ba, ba), .0, 1.);
	return max_component(pa - ba * h);
}


vec4 derivative(in vec3 position, in float epsilon)
{
	vec2 offset = vec2(-epsilon, epsilon);
	vec4 simplex = vec4(0.);
	simplex.x = map(position + offset.xyy);
	simplex.y = map(position + offset.yyx);
	simplex.z = map(position + offset.yxy);
	simplex.w = map(position + offset.xxx);
	vec3 grad = offset.xyy * simplex.x + offset.yyx * simplex.y + offset.yxy * simplex.z + offset.xxx * simplex.w;
	return vec4(grad, .2/epsilon*(dot(simplex, vec4(1.)) - 4. * map(position)));
}

vec4 derivative2(in vec3 position, in float epsilon)
{
	vec2 offset = vec2(-epsilon, epsilon);
	vec4 simplex = vec4(0.);
	simplex.x = map2(position + offset.xyy);
	simplex.y = map2(position + offset.yyx);
	simplex.z = map2(position + offset.yxy);
	simplex.w = map2(position + offset.xxx);
	vec3 grad = offset.xyy * simplex.x + offset.yyx * simplex.y + offset.yxy * simplex.z + offset.xxx * simplex.w;
	return vec4(grad, .2/epsilon*(dot(simplex, vec4(1.)) - 4. * map(position)));
}

float exp2fog(float depth, float density)
{
	float f = pow(2.71828, depth * density);
	return 1./(f * f);
}



float shadow(vec3 origin, vec3 direction, float mint, float maxt, float k) 
{
	float sh = 1.0;
	float t = mint;
	float h = t;
	for (int i = 0; i < 32; i++) 
	{
		if (t < maxt)
		{
			h 		= map(origin + direction * t);
			sh 		= smoothmin(abs(k * h/t), sh, k);
			t 		+= min(h, maxt);
		}
	}
	return clamp(sh, .0, 1.);
}



float ambient_occlusion(vec3 position, vec3 normal, float delta, float t, float f)
{	   
	float occlusion = 0.0;
	for (float i = 1.; i <= 9.; i++)
	{
	    occlusion	+= t * (i * delta - map(position + normal * delta * i));
	    t 		*= f;
	}
 	
	const float k 	= 4.0;
	return 1.0 - clamp(k * occlusion, 0., 1.);
}



vec3 gamma_correction(vec3 color, float brightness, float gamma)
{
	return pow(color * brightness, vec3(1., 1., 1.)/gamma);
}	 
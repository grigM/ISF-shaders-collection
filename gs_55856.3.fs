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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#55856.3"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define PHI 		((sqrt(5.)+1.)*.5)
#define TAU 		(8.*atan(1.))

#define LEFT_DISPLAY_WIDTH .15

#define TARGET_RANGE	12.
#define VIEW_X 		(normalize(vec3( 1., -.001,  .0)) * TARGET_RANGE)
#define VIEW_Y 		(normalize(vec3(.0,   1., -.001)) * TARGET_RANGE)
#define VIEW_Z 		(normalize(vec3(.0001, 0.,  -1.)) * TARGET_RANGE)
#define VIEW_PHI	(normalize(vec3(1.,1., 1.)) * TARGET_RANGE)
#define VIEW_ORBIT  	(normalize(vec3(3.*sin((mouse.x-.5)*2.*TAU), -3.*atan((mouse.y-.5) * TAU)*2., 3.*cos((mouse.x-.5)*2.*TAU+TAU*.5))) * -TARGET_RANGE) //orbit cam
#define VIEW_ORIGIN     (mouse.x < LEFT_DISPLAY_WIDTH ? (mouse.y < .75 ? (mouse.y < .5 ? (mouse.y < .25 ? VIEW_Z : VIEW_Y) : VIEW_X) : VIEW_PHI) : VIEW_ORBIT)


/*
vec3 foldY(vec3 P, float n)
{
	float r = length(P.xz);
	float a = atan(P.z, P.x);
	float c = 3.14159265358979 / n;

	a = mod(a, 2.0 * c) - c; 

	P.x = r * cos(a);
	P.z = r * sin(a);

	return P;
}

// Optimized case for 4 repetitions
vec3 foldY4(vec3 p)
{
	p.xz = vec2(p.x + p.z, p.z - p.x) * sin45deg;
	p.xz = abs(p.x) > abs(p.z) ? p.xz * sign(p.x) : vec2(p.z,-p.x) * sign(p.z);
	return p;
}


float capsule( vec3 p, vec3 a, vec3 b, float r )
{
	vec3 pa = p - a;
	vec3 ba = b - a;
	float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
	
	return length( pa - ba*h ) - r;
}
float sphere(vec3 rp, vec3 sp, float r){
	return length(rp - sp)-r;		
}

vec3 derivate(vec3 p){
	vec3 n;
	vec2 d = vec2(0., .01);
	n.x = map(p+d.yxx)-map(p-d.yxx);
	n.y = map(p+d.xyx)-map(p-d.xyx);
	n.z = map(p+d.xxy)-map(p-d.xxy);
//    return n/.03;
    return normalize(n);
}
*/

float binary(float n, float e)
{
	return n/exp2(e+1.);
}


float gray(float n, float e)
{
//	return binary(n,e);
	return binary(n,e+1.)+.25;
}


float step_bit(float b)
{
	return step(.5, fract(b));
}


float rcp(float x)
{
	return x == 0. ? x : 1./x;	
}

float sprite(float n, vec2 p)
{
	p 		= ceil(p);
	float bounds 	= float(all(bvec2(p.x < 3., p.y < 5.)) && all(bvec2(p.x >= 0., p.y >= 0.)));
	return step_bit(binary(n, (2. - p.x) + 3. * p.y)) * bounds;
}

				
float digit(float n, vec2 p)
{	
	     if(n == 0.) { return sprite(31599., p); }
	else if(n == 1.) { return sprite( 9362., p); }
	else if(n == 2.) { return sprite(29671., p); }
	else if(n == 3.) { return sprite(29391., p); }
	else if(n == 4.) { return sprite(23497., p); }
	else if(n == 5.) { return sprite(31183., p); }
	else if(n == 6.) { return sprite(31215., p); }
	else if(n == 7.) { return sprite(29257., p); }
	else if(n == 8.) { return sprite(31727., p); }
	else             { return sprite(31695., p); }
}

				
float print(float n, vec2 position)
{	
	float result = 0.;
	for(int i = 0; i < 8; i++)
	{
		float place = pow(10., float(i));
		
		if(n >= place || i == 0)
		{
			result	 	+= digit(floor(mod(floor(n/place), 10.)), position);		
			position.x	+= 4.;
		}				
	}
	return floor(result+.5);
}


vec3 hsv(in float h, in float s, in float v)
{
    	return mix(vec3(1.),clamp((abs(fract(h+vec3(3.,2.,1.)/3.)*6.-3.)-1.),0.,1.),s)*v;
}


float contour(float x, float r)
{
	return 1.-clamp(x*(dot(vec2(r),RENDERSIZE)), 0., 1.);
}



float edge(vec2 p, vec2 a, vec2 b)
{
	vec2 q	= b - a;	
	float u = dot(p - a, q)/dot(q, q);
	u 	= clamp(u, 0., 1.);

	return distance(p, mix(a, b, u));
}


float line(vec2 p, vec2 a, vec2 b, float r)
{
	vec2 q	= b - a;	
	float u = dot(p - a, q)/dot(q, q);
	u 	= clamp(u, 0., 1.);

	return contour(edge(p, a, b), r);
}


mat2 rmat(float t)
{
	float c = cos(t);
	float s = sin(t);
	return mat2(c, s, -s, c);
}



mat3 projection_matrix(in vec3 origin, in vec3 target) 
{	
	vec3 w          	= normalize(origin-target);
	vec3 u         		= normalize(cross(w,vec3(0.,1.,0.)));
	vec3 v          	= -normalize(cross(u,w));
	return mat3(u, v, w);
}


mat3 phack;
vec3 project(vec3 origin, vec3 v)
{
	v 	-= origin;
	v 	*= phack*1.0;	
	v.z 	= v.z-1.;	
	
	if(gl_FragCoord.x < LEFT_DISPLAY_WIDTH * RENDERSIZE.x)
	{
		v.xy /= TARGET_RANGE;
	}
	else
	{
		if(mouse.x < LEFT_DISPLAY_WIDTH)
		{
			v.xy *= rcp(TARGET_RANGE);
		}
		else
		{
			v.xy *= rcp(v.z+1.);
		}
	}
	
	return v;
}


vec3 h46cube(float i)
{	
	//135024
	float x = step_bit(gray(i, 1.));
	float y = step_bit(gray(i, 3.));
	float z = step_bit(gray(i, 5.));
	float u = step_bit(gray(i, 0.));
	float v = step_bit(gray(i, 2.));
	float w = step_bit(gray(i, 4.));
	
	float t = abs(fract(TIME * .125)-.5)*2.;
	t 	= t * t * t;
	float p = mix(1., PHI, clamp(t*4.-1.,0.,1.));

	vec3 r = vec3(x * p - u * p + y + v, y * p - v * p + z + w, z * p - w * p + x + u);	
	return r - 1.;
	
	
}

float fold(float i)
{
//	float m = 32.;//floor(mouse.y*64.);
	//i = mod(floor(i * rcp(32.)), 2.) == 0. ? i + 32. : i - 32.;
//	i = mod(floor(i * rcp(16.)), 2.) == 0. ? i + 16. : i - 16.;
//	i = mod(floor(i * rcp(8.)), 2.) == 0. ? i - 8. : i + 8.;
	//i = mod(floor(i * rcp(4.)), 2.) == 0. ? i + 4. : i - 4.;
	//i = mod(floor(i * rcp(2.)), 2.) == 0. ? i + 2. : i - 2.;
	//i = mod(floor(i * m), 2.) == 0. ? i - m : i + m;
//	i = mod(floor(i * rcp(m)), 2.) == 0. ? i - m : i + m;
//	i = mod(i, 64.);
	return i;
}


float icosahedron(vec3 p, float r)
{
	vec4 q 	= (vec4(.30901699437, .5, .80901699437, 0.)); 	
	p 	= abs(p);
	return max(max(max(dot(p,q.wxz), dot(p, q.yyy)),dot(p,q.zwx)),dot(p,q.xzw))-r+(PHI-1.);
}


float dodecahedron(vec3 p, float r)
{
	vec3 q 	= normalize(vec3(0., .5,.80901699437));	
	p 	= abs(p);	
	return max(max(dot(p, q.yxz), dot(p, q.zyx)),dot(p, q.xzy))-r+(PHI-1.);
}

float rhombictriacontahedron(vec3 p, float r)
{
	vec3 q = vec3(.30901699437, .5,.80901699437);	
	p = abs(p);	
	return  max(max(max(max(max(p.x, p.y), p.z), dot(p, q.zxy)), dot(p, q.xyz)), dot(p, q.yzx)) - r;
}


float trucatedicosahedron(vec3 p, float r)
{
	vec4 q	= vec4(.30901699437, .5,.80901699437, 0.);	
	//p = abs(p);
	float d = 0.;

	p	= abs(p);
	d	= max(max(max(max(max(p.x, p.y), p.z), dot(p, q.zxy)), dot(p, q.xyz)), dot(p, q.yzx));	
	d 	= max(max(max(dot(p, q.ywz), dot(p, q.zyw)),dot(p, q.wzy)), d - .125);			
	d	-= r - .125;
	return  d;
}

float exp2fog(vec3 position, vec3 view_origin, float density)
{
	float f = pow(2.71828, distance(position, view_origin) * density);
	return 1./(f * f);
}

float map(vec3 position)
{
	 return rhombictriacontahedron(position, 3.);
}

vec4 derivative(in vec3 position, in float epsilon)
{
	vec2 offset 	= vec2(-epsilon, epsilon);
	vec4 simplex 	= vec4(0.);
	simplex.x 	= map(position + offset.xyy);
	simplex.y 	= map(position + offset.yyx);
	simplex.z 	= map(position + offset.yxy);
	simplex.w 	= map(position + offset.xxx);
	vec3 grad 	= offset.xyy * simplex.x + offset.yyx * simplex.y + offset.yxy * simplex.z + offset.xxx * simplex.w;	
	return vec4(grad, .2/epsilon*(dot(simplex, vec4(1.)) - 4. * map(position)));
}


void main( void ) 
{
	vec2 aspect			= RENDERSIZE.xy/min(RENDERSIZE.x, RENDERSIZE.y);
	vec2 uv 			= gl_FragCoord.xy/RENDERSIZE.xy;
	
	bool left_display_panels	= uv.x < LEFT_DISPLAY_WIDTH; 
	bool mouse_on_left		= mouse.x < LEFT_DISPLAY_WIDTH;
	float display_panel		= left_display_panels ? floor(uv.y * 4.) + 1. : 0.;
	vec2 display_uv			= left_display_panels ? fract(uv * vec2(4., 4.)) + vec2(.125, -.0625) : uv;
	
	vec2 p				= (display_uv - .5) * aspect;
	p				+= left_display_panels ? .125 : 0.;
	
	vec3 origin			= display_panel == 0. ? VIEW_ORIGIN : 
					  display_panel == 1. ? VIEW_Z : 
					  display_panel == 2. ? VIEW_Y : 
					  display_panel == 3. ? VIEW_X : 
				    	  VIEW_PHI;
	
	vec3 view_position		= origin;
	vec3 target			= vec3(0., 0., 0.);
	
	mat3 projection			= projection_matrix(vec3(0.,0.,0.), origin);
	phack				= projection;
	vec3 view			= normalize(vec3(p, 1.61));

	float x				= floor((1.-uv.x)*64.+1.);	
	float y				= floor(uv.y*64.);	
	y 				= fold(y);

	
	float bits			= step_bit(gray(y*2., x));
	
	float width			= 1.;
	vec3 path			= vec3(0., 0., 0.);
	vec3 vertex[2];
	vertex[0]			= h46cube(fold(63.));
	vertex[1]			= h46cube(fold(0.));
	vec3 v_projection[14];
	v_projection[0]	= project(origin, vec3(1., 1., 1.) * vertex[0].xyz);
	v_projection[1]	= project(origin, vec3(1.,-1.,-1.) * vertex[0].xyz);
	v_projection[2]	= project(origin, vec3(-1., 1., -1.) * vertex[0].yzx);
	v_projection[3]	= project(origin, vec3(1., -1., 1.) * vertex[0].yzx);
	v_projection[4]	= project(origin, vec3(-1., 1., -1.) * vertex[0].zxy);
	v_projection[5]	= project(origin, vec3(1., 1.,  1.) * vertex[0].zxy);
	v_projection[6]	= project(origin, vec3(-1.,-1., -1.) * vertex[0].zxy);
	
	float v_weight[7];
	path				+= bits * .0125;
	float animation_speed		= 5.;
	float animation_step 		= TIME * animation_speed;
	float cutoff			= mod(animation_step, 128.);
	bool reverse 			= cutoff > 64.;
	float animation_interpolant	= reverse ? fract(animation_step) : fract(1.-animation_step) ;
	cutoff				= abs(cutoff-64.);
	
	if(mod(animation_step, 256.) > 128.)
	{
		cutoff			= 64.;
		animation_interpolant	= 1.;
	}
	

	float id_print			= 0.;	
	vec3 bit_hue			= vec3(0.,0.,0.);
	vec3 bit_display		= vec3(0., 0., 0.);
	float v				= 0.;
	float f				= .5;
	for(float i = 0.; i < 64.; i++)
	{			
		v 				= fold(i);

		vertex[0]			= h46cube(v);
		
			
		bool last_vert			= i == floor(cutoff);	

		float saturation		= float(v < cutoff) - float(last_vert) * animation_interpolant;
		float brightness		= v < cutoff ? 1. : .5;
		vec3 color			= hsv(floor(v)*rcp(64.), saturation, brightness);

		
		float extent 			= length(vertex[0]);
		bool dodecahedron		= extent < 1.7;
		bool icosahedron		= extent > 1.6 && extent < 1.74;	
	
		if(i == y)
		{
			bit_display		= max(bit_display, bits * color);
		}
			

		v_projection[7]	= v_projection[0];
		v_projection[8]	= v_projection[1];
		v_projection[9]	= v_projection[2];
		v_projection[10]= v_projection[3];
		v_projection[11]= v_projection[4];
		v_projection[12]= v_projection[5];		
		v_projection[13]= v_projection[6];		
	
		
		v_projection[0]	= project(origin, vec3(1., 1., 1.) * vertex[0].xyz);
		v_projection[1]	= project(origin, vec3(1.,-1.,-1.) * vertex[0].xyz);
		v_projection[2]	= project(origin, vec3(-1., 1., -1.) * vertex[0].yzx);
		v_projection[3]	= project(origin, vec3(1., -1., 1.) * vertex[0].yzx);
		v_projection[4]	= project(origin, vec3(-1., 1., -1.) * vertex[0].zxy);
		v_projection[5]	= project(origin, vec3(1., 1.,  1.) * vertex[0].zxy);
		v_projection[6]	= project(origin, vec3(-1.,-1., -1.) * vertex[0].zxy);
		
		float f2 	= .95;
		float f3	= .025;
		v_weight[0]	= f3+f2*view.z*rcp(max(v_projection[0].z, v_projection[7].z));
		v_weight[1]	= f3+f2*view.z*rcp(max(v_projection[1].z, v_projection[8].z));
		v_weight[2]	= f3+f2*view.z*rcp(max(v_projection[2].z, v_projection[9].z));
		v_weight[3]	= f3+f2*view.z*rcp(max(v_projection[3].z, v_projection[10].z));
		v_weight[4]	= f3+f2*view.z*rcp(max(v_projection[4].z, v_projection[11].z));
		v_weight[5]	= f3+f2*view.z*rcp(max(v_projection[5].z, v_projection[12].z));		
		v_weight[6]	= f3+f2*view.z*rcp(max(v_projection[6].z, v_projection[13].z));
		float l 	= 0.;
		if(i < cutoff)
		{
			
			l		= max(l, line(view.xy, v_projection[0].xy,  v_projection[7].xy, .01/v_weight[0]) * v_weight[0]);
			/*
			l		= max(l, line(view.xy, v_projection[1].xy,  v_projection[8].xy, .01/v_weight[1]) * v_weight[1]);
			l		= max(l, line(view.xy, v_projection[2].xy,  v_projection[9].xy, .01/v_weight[2]) * v_weight[2]);
			l		= max(l, line(view.xy, v_projection[3].xy, v_projection[10].xy, .01/v_weight[3]) * v_weight[3]);
			l		= max(l, line(view.xy, v_projection[4].xy, v_projection[11].xy, .01/v_weight[4]) * v_weight[4]);
			l		= max(l, line(view.xy, v_projection[5].xy, v_projection[12].xy, .01/v_weight[5]) * v_weight[5]);
			l		= max(l, line(view.xy, v_projection[6].xy, v_projection[13].xy, .01/v_weight[6]) * v_weight[6]);
			
			*/
			l 		*= 25.;
			l		= pow(l, 3.)*.0425;
			path		= max(path, l * color * .85+  l * color * (v_weight[0]*1.5));
			l 		= 0.;
		
		}
		
		l		= max(l, line(view.xy, v_projection[0].xy,  v_projection[7].xy, .01/v_weight[0]) * v_weight[0]);
		l		= max(l, line(view.xy, v_projection[1].xy,  v_projection[8].xy, .01/v_weight[1]) * v_weight[1]);
		l		= max(l, line(view.xy, v_projection[2].xy,  v_projection[9].xy, .01/v_weight[2]) * v_weight[2]);
		l		= max(l, line(view.xy, v_projection[3].xy, v_projection[10].xy, .01/v_weight[3]) * v_weight[3]);
		l		= max(l, line(view.xy, v_projection[4].xy, v_projection[11].xy, .01/v_weight[4]) * v_weight[4]);
		l		= max(l, line(view.xy, v_projection[5].xy, v_projection[12].xy, .01/v_weight[5]) * v_weight[5]);
		l		= max(l, line(view.xy, v_projection[6].xy, v_projection[13].xy, .01/v_weight[6]) * v_weight[6]);
		l 		*= 16.;
		l		= pow(l, 3.)*.0625;	
		path		= max(path, clamp(l * vec3(1.,1.,1.),0.,1.));
	
		
		vertex[1] 	= vertex[0];		
	
	}

	id_print 		+= print(floor(gl_FragCoord.y*64.*rcp(RENDERSIZE.y)), ceil(vec2(gl_FragCoord.x-RENDERSIZE.x+85., mod(floor(gl_FragCoord.y-1.), 8.))));;
	
	
	vec3 result 		= vec3(0., 0., 0.);	
	
	result 			+= bit_display;
	result 			+= path;
	result			+= id_print;
	result.xyz		= pow(result.xyz, vec3(1.,1.,1.) * 1.6);
	
	gl_FragColor.xyz	= result;
	gl_FragColor.w 		= 1.;
}//sphinx
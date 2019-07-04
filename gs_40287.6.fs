/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40287.6"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


/****** CONFIG *************************************/

/*
 * NOTICE:
 *   Set the scale drop down above to 2 or 1,
 *   and uncommen exactly one of these to match.
 */
#define AASCALE 2
//#define AASCALE 1

/*
 * reduce this value if the
 * color spinnning is too hectic
 */
#define COLOR_SPEED 0.8
//#define COLOR_SPEED 0.6
//#define COLOR_SPEED 0.3

/*
 * "What good is a disco ball if it's not spinning?
 *  Spin tht sonnavabitch!"
 */
//#define DO_ROTATE

/***************************************************/

#if (AASCALE == 2)
# define SIZE (7.0)
#else
# if (AASCALE == 1)
#  define SIZE (11.0)
# else
#  error AASCALE must equal 1 or 2
# endif
#endif


#define TILECOUNT (SIZE * SIZE)

#define TAU 6.283185307179586

#define PX (vec2(1.0) / RENDERSIZE)
#define nsin(x) ((sin(x) + 1.0) / 2.0)
#define ncos(x) ((cos(x) + 1.0) / 2.0)


vec2 tile_pos;

vec2 rotate(in vec2 point, in float rads)
{
	float cs = cos(rads);
	float sn = sin(rads);
	return point * mat2(cs, -sn, sn, cs);
}

vec4 permute(vec4 x){return mod(((x*34.0)+1.0)*x, 289.0);}
vec4 taylorInvSqrt(vec4 r){return 1.79284291400159 - 0.85373472095314 * r;}

float snoise(vec2 v2, float sizemod){
  vec3 v = vec3(v2/(9.0 + sizemod), TIME / 2.5333);

  const vec2  C = vec2(1.0/6.0, 1.0/3.0) ;
  const vec4  D = vec4(0.0, 0.5, 1.0, 2.0);

// First corner
  vec3 i  = floor(v + dot(v, C.yyy) );
  vec3 x0 =   v - i + dot(i, C.xxx) ;

// Other corners
  vec3 g = step(x0.yzx, x0.xyz);
  vec3 l = 1.0 - g;
  vec3 i1 = min( g.xyz, l.zxy );
  vec3 i2 = max( g.xyz, l.zxy );

  //  x0 = x0 - 0. + 0.0 * C 
  vec3 x1 = x0 - i1 + 1.0 * C.xxx;
  vec3 x2 = x0 - i2 + 2.0 * C.xxx;
  vec3 x3 = x0 - 1. + 3.0 * C.xxx;

// Permutations
  i = mod(i, 289.0 ); 
  vec4 p = permute( permute( permute( 
             i.z + vec4(0.0, i1.z, i2.z, 1.0 ))
           + i.y + vec4(0.0, i1.y, i2.y, 1.0 )) 
           + i.x + vec4(0.0, i1.x, i2.x, 1.0 ));

// Gradients
// ( N*N points uniformly over a square, mapped onto an octahedron.)
  float n_ = 1.0/7.0; // N=7
  vec3  ns = n_ * D.wyz - D.xzx;

  vec4 j = p - 49.0 * floor(p * ns.z *ns.z);  //  mod(p,N*N)

  vec4 x_ = floor(j * ns.z);
  vec4 y_ = floor(j - 7.0 * x_ );    // mod(j,N)

  vec4 x = x_ *ns.x + ns.yyyy;
  vec4 y = y_ *ns.x + ns.yyyy;
  vec4 h = 1.0 - abs(x) - abs(y);

  vec4 b0 = vec4( x.xy, y.xy );
  vec4 b1 = vec4( x.zw, y.zw );

  vec4 s0 = floor(b0)*2.0 + 1.0;
  vec4 s1 = floor(b1)*2.0 + 1.0;
  vec4 sh = -step(h, vec4(0.0));

  vec4 a0 = b0.xzyw + s0.xzyw*sh.xxyy ;
  vec4 a1 = b1.xzyw + s1.xzyw*sh.zzww ;

  vec3 p0 = vec3(a0.xy,h.x);
  vec3 p1 = vec3(a0.zw,h.y);
  vec3 p2 = vec3(a1.xy,h.z);
  vec3 p3 = vec3(a1.zw,h.w);

//Normalise gradients
  vec4 norm = taylorInvSqrt(vec4(dot(p0,p0), dot(p1,p1), dot(p2, p2), dot(p3,p3)));
  p0 *= norm.x;
  p1 *= norm.y;
  p2 *= norm.z;
  p3 *= norm.w;

// Mix final noise value
  vec4 m = max(0.6 - vec4(dot(x0,x0), dot(x1,x1), dot(x2,x2), dot(x3,x3)), 0.0);
  m = m * m;
  return 0.14 + 37.0 * dot( m*m, vec4( dot(p0,x0), dot(p1,x1), 
                                dot(p2,x2), dot(p3,x3) ) );
}

void main(void)
{

	vec2 position = ((gl_FragCoord.xy / RENDERSIZE.xy ) * 2.0) - 1.0;

	position.y *= RENDERSIZE.y/RENDERSIZE.x;

#ifdef DO_ROTATE
	vec2 rposition = rotate(position, TIME/-11.0);
	vec2 tposition = rposition + vec2(cos(TIME / 13.0), sin(TIME/17.0));
#else
# define rposition position
# define tposition position
#endif

	vec3 c = vec3(0.0);
	
	vec2 position_size = tposition * SIZE; // rotate(tposition * S, TIME / 11.0);

	tile_pos = ceil(position_size);

	vec2 sw_pos = rotate(tile_pos, (TIME * 0.05) + tile_pos.x - tile_pos.y);
	vec2 secwave = 1.0 / abs(cos(((sw_pos * 0.15) + (TIME *1.5)) * 0.5 ));
	secwave -= 1.0;
	secwave = pow(secwave, vec2(0.36));
	secwave *= 0.4;

	vec2 inner_pos = (fract(position_size) * 2.0) - 1.0;
	vec2 tile_px = PX * SIZE;

	vec2 ntscale = tile_pos;
	vec2 scale_r = vec2( 0.7,  1.2) * ntscale;
	vec2 scale_g = vec2( 1.3, -1.3) * ntscale.yx;
	vec2 scale_b = vec2(-1.2,  0.9) * ntscale;


	vec3 phase = vec3(snoise(scale_r, 3.37),
			  snoise(scale_g, 2.13),
			  snoise(scale_b, 5.751));

	vec2 border_size = abs(tile_px * 4.0);
	vec2 border_limit = 1.0 - border_size;
	vec2 aip = abs(inner_pos);

	float mag = max(phase.x, max(phase.y, phase.z));
	
	float sw = max(secwave.x, secwave.y);
	//float sw = secwave.x;

	mag = mix(mag, 1.0, sw);

	float corner_limit = sqrt(2.0) - 2.5 *length(border_size);
	if ((aip.x < border_limit.x) &&
	    (aip.y < border_limit.y) &&
	    (length(inner_pos) < corner_limit)) {
		c = vec3(phase.xzy);        
		c += mix(c, vec3(1.0), sw);
		
		float fadepow = mix(1.5, 3.0, mag);
		float fade = pow(length(aip), fadepow);
		vec3 cfade = mix(c, vec3(0.33 + (nsin(TIME*3.0 + length(tile_pos)) * 0.118)), 0.66);
		vec3 cnorm = mix(c, vec3(0.96), 0.37);
		c = mix(cnorm, cfade, fade);
		c = mix(vec3(0.03), c, mag);
	}

	gl_FragColor = vec4(c, 1.0);
}
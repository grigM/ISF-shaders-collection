/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wljGDc by badjano.  Voronoi with random scaling distance",
  "INPUTS" : [

  ]
}
*/


float hash1( float n ) {
    return fract(sin(n)*43758.5453);
}
vec2  hash2( vec2  p ) {
    p = vec2(
        dot(p,vec2(127.1,311.7)),
        dot(p,vec2(269.5,183.3))
    );
    return fract(sin(p)*43758.5453);
}
vec3 hash3( vec3  p ) {
    p = vec3(
        dot(p,vec3(421.9,137.2,159.5)),
        dot(p,vec3(127.1,311.7,753.7)),
        dot(p,vec3(269.5,183.3,459.3))
    );
    return fract(sin(p)*43758.5453);
}
vec3 hsv2rgb(vec3 c) {
  vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
  vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
  return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

vec4 voronoi( in vec2 x)
{
    vec2 n = floor( x );
    vec2 f = fract( x );

    const int _size = 4;
	vec4 m = vec4( 0.0, 0.0, 0.0, 1.0 );
    for( int j=-_size; j<=_size; j++ )
    for( int i=-_size; i<=_size; i++ )
    {
        vec2 g = vec2( float(i),float(j) );
        vec2 o1 = hash2( n + g );
		
		// animate
        vec2 o = 0.5 + 0.5*sin( TIME*2. + 6.2831*o1 );

        // distance to cell		
		float d = length(g - f + o)*pow(sin(TIME + o1.x * 1000.)+1.5,3.);
		
        // do the smoth min for colors and distances		
		vec3 col = hsv2rgb(vec3(o,1.));
		float h = smoothstep( 0.0, 1.0, 0.5 + 0.5*(m.w-d)/pow(sin(TIME+o.x)*.1+.1,1.1) );
		
	    m.w   = mix( m.w,     d, h );
		m.xyz = mix( m.xyz, col, h );
    }
	
	return m;
}

void main() {



    vec2 p = gl_FragCoord.xy/RENDERSIZE.xy - vec2(.5);
    p.x *= RENDERSIZE.x/RENDERSIZE.y;
	
    vec4 v = voronoi( (sin(TIME)*2.+13.)*p );
    gl_FragColor = vec4(v.xyz,1.);
}

/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "voronoi",
    "hexagons",
    "polygons",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MdlcDB by qmuntada.  Simple addition to Iq shader : https:\/\/www.shadertoy.com\/view\/MslGD8",
  "INPUTS" : [

  ]
}
*/


// Created by inigo quilez - iq/2013
// Modified by qmuntada (2017)
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

vec2 hash( vec2 p ) { p=vec2(dot(p,vec2(127.1,311.7)),dot(p,vec2(269.5,183.3))); return fract(sin(p)*18.5453); }

// return distance, and cell id
vec2 voronoi( in vec2 x )
{
    vec2 n = floor(x);
    vec2 f = fract(x);

	vec3 m = vec3(8.0);
    for(int j=-1; j<=1; j++)
    for(int i=-1; i<=1; i++)
    {
        vec2  g = vec2(float(i), float(j));
        vec2  o = hash(n + g) * (cos(TIME) + 1.0) / 2.0;
        
        if (mod(n.x, 2.0) - float(i) == -1.0 || mod(n.x, 2.0) - float(i) == 1.0)
            o.y += 0.5;
        
       	vec2  r = g - f + o;
        
		float d = dot(r, r);
        if(d < m.x)
            m = vec3(d, o);
    }

    return vec2( sqrt(m.x), m.y+m.z );
}

void main() {



    vec2 p = gl_FragCoord.xy/max(RENDERSIZE.x,RENDERSIZE.y);
    
    // computer voronoi patterm
    vec2 c = voronoi( (14.0+6.0*sin(0.2*TIME))*p );
    // colorize
    vec3 col = 0.5 + 0.5 * cos(c.y * 3.0 + vec3(0.0,1.0,2.0) );	
    col *= clamp(1.0 - 0.5 * c.x * c.x, 0.0, 1.0);
    col -= (1.0-smoothstep( 0.05, 0.05, c.x));
	
    gl_FragColor = vec4( col, 1.0 );
}

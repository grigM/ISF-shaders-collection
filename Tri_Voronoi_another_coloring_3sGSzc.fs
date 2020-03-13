/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3sGSzc by aiekick.  Triangular Voronoi another coloring",
  "INPUTS" : [

  ]
}
*/


// Created by Stephane Cuillerdier - Aiekick/2019 (twitter:@aiekick)
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// Tuned via NoodlesPlate (https://github.com/aiekick/NoodlesPlate/releases)

const vec3 color0 = vec3(0.2,0.1,0.3);
const vec3 color1 = vec3(0.6,0.1,1.9);
const vec3 lightColor = vec3(0.7,1.6,0.6);

vec3 shape(vec2 g)
{
	float c = 9.0;
    	
    float t = TIME;
	float t1 = t * 0.1;
	
	vec2 p = vec2(0), sp = p;
	
    g *= 2.0;
    
    for(int x=-2;x<=2;x++)
    for(int y=-2;y<=2;y++)
    {	
        p = vec2(x,y);
        p += .5 + .5*sin( t1 * 10. + 9. * fract(sin((floor(g)+p)*mat2(2,5,5,2)))) - fract(g);
        p *= mat2(cos(t1), -sin(t1), sin(t1), cos(t1));
        float d = max(abs(p.x)*.866 - p.y*.5, p.y);
        if (d < c)
        {
            c = d;
            sp = p;
        }
    }

    return vec3(c,sp);
}

void main() {



    vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE.xy)/RENDERSIZE.y;
	
	float t = TIME * 0.5;
	
    float k = 0.01;
	vec3 f = shape(uv);
	float fx = shape(uv + vec2(k,0.0)).x-f.x;
	float fy = shape(uv + vec2(0.0, k)).x-f.x;
	
    vec3 n = normalize(vec3(fx, 0.1, fy) );
	
	vec3 col = mix( color0, color1, f.x );
    float r = sin(t + f.y)*cos(t + f.z) * .5 + .5;
	col = mix(col.xyz, mix(col.zxy, col.yzx, r), 1.-r);
    
	col += .4 * pow(max(dot( n, vec3(0,1,0)), 0.), 100.) * lightColor;
	
	gl_FragColor = vec4( col, 1.0 );
}

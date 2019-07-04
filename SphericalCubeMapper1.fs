/*{
	"CREDIT": "by mojovideotech",
"CATEGORIES" : [
    "3d",
    "uv",
    "mapping",
    "quadcube"
  ],
  "DESCRIPTION" : "SphericalCubeMapper1 from https:\/\/www.shadertoy.com\/view\/4sjXW1 by nimitz. Implementation of procedural cubemaps",
  "INPUTS" : [
    {
      "NAME" : "SprCoords",
      "TYPE" : "point2D",
        "DEFAULT": [
		0.0,
		0.0
	  ],
      "MAX" : [
        1000,
        1000
      ],
      "MIN" : [
        -1000,
        -1000
      ]
    },
    	{
			"NAME": "UX",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.5,
			"MAX": 1.5
		},
		{
			"NAME": "UY",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.5,
			"MAX": 1.5
		},
		{
			"NAME": "UZ",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "RX",
			"TYPE": "float",
			"DEFAULT": -3.1365,
			"MIN": -3.1365,
			"MAX": 3.1365
		},
		{
			"NAME": "RY",
			"TYPE": "float",
			"DEFAULT": 2.8,
			"MIN": -3.1365,
			"MAX": 3.1365
		},
		{
			"NAME": "origin",
			"TYPE": "float",
			"DEFAULT": 1.5,
			"MIN": -3.0,
			"MAX": 3.0
		},
		{
			"NAME": "scale",
			"TYPE": "float",
			"DEFAULT": 4.0,
			"MIN": 1.0,
			"MAX": 10.0
		},
		{
			"NAME": "maptype",
			"TYPE": "long",
			"VALUES": [
				0,
				1,
				2,
				3
			],
			"LABELS": [
				"cube",
				"quad",
				"trip",
				"sphr"
			],
			"DEFAULT": 0
		},
		{
			"NAME": "mixer",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		}
		
  ]
}
*/

// SphericalCubeMapper1 by mojovideotech
// based on:
// www.shadertoy.com/view/4sjXW1
// Sphere mapping by nimitz

mat2 mm2(in float a){float c = cos(a), s = sin(a);return mat2(c,-s,s,c);}

vec3 hsv2rgb( in vec3 c )
{
    vec3 rgb = clamp( abs(mod(c.x*6.0+vec3(0.0,4.0,2.0),6.0)-3.0)-1.0, 0.0, 1.0 );
	return c.z * mix( vec3(1.0), rgb, c.y);
}

vec3 tex(in vec2 p)
{
    float frq =50.3;
    p += 0.405;
    return vec3(1.)*smoothstep(.9, 1.05, max(sin((p.x)*frq),sin((p.y)*frq)));
}

vec3 cubeproj(in vec3 p)
{
    vec3 x = tex(p.zy/p.x);
    vec3 y = tex(p.xz/p.y);
    vec3 z = tex(p.xy/p.z);
    x *= vec3(1,0,0)*abs(p.x) + p.x*vec3(0,1,0);
    y *= vec3(0,1,0)*abs(p.y) + p.y*vec3(0,0,1);
    z *= vec3(0,0,1)*abs(p.z) + p.z*vec3(1,0,0);
    p = abs(p);
    if (p.x > p.y && p.x > p.z) return x;
    else if (p.y > p.x && p.y > p.z) return y;
    else return z;
}

vec3 healpix(vec3 p)
{
	float a = atan(p.z, p.x) * 0.63662; 
	float h = 3.*abs(p.y);
	float h2 = .75*p.y;
	vec2 uv = vec2(a + h2, a - h2);
	h2 = sqrt(3. - h);
	float a2 = h2 * fract(a);
    uv = mix(uv, vec2(-h2 + a2, a2), step(2., h));    
    vec3 col = tex(uv);
    col.x = a*0.5;
    return hsv2rgb(vec3(col.x,.8,col.z));
}

vec3 tpl(in vec3 p)
{
	vec3 x = tex(p.yz);
	vec3 y = tex(p.zx);
	vec3 z = tex(p.xy);
    x *= vec3(1,0,0)*abs(p.x) + p.x*vec3(0,1,0);
    y *= vec3(0,1,0)*abs(p.y) + p.y*vec3(0,0,1);
    z *= vec3(0,0,1)*abs(p.z) + p.z*vec3(1,0,0);
    p = normalize(max(vec3(0),abs(p)-.6));
    return x*p.x + y*p.y + z*p.z;
}

vec3 sphproj(in vec3 p)
{
    vec2 sph = vec2(acos(p.y/length(p)), atan(p.z,p.x));
    vec3 col = tex(sph*.9);
    col.x = sph.x*0.4;
    return hsv2rgb(vec3(col.x,.8,col.z));
}

float iSphere(in vec3 ro, in vec3 rd)
{
    vec3 oc = ro;
    float b = dot(oc, rd);
    float c = dot(oc,oc) - 1.;
    float h = b*b - c;
    if(h <0.0) return -1.;
    return -b - sqrt(h);
}

void main()
{	
	vec2 p = gl_FragCoord.xy/RENDERSIZE.xy - 0.5;
	p.x*=RENDERSIZE.x/RENDERSIZE.y;
	vec2 um = SprCoords*p / RENDERSIZE.xy - 0.5;
	um.x *= RENDERSIZE.x/RENDERSIZE.y;
    p*= scale; p.x += UX; p.y += UY;
	
	vec3 ro = vec3(0.0,0.0,UZ);
    vec3 rd = normalize(vec3(p,-2.5*origin));
    mat2 mx = mm2((RX-0.4)+um.x*5.);
    mat2 my = mm2((RY-0.3)+um.y*5.); 
    ro.xz *= mx;rd.xz *= mx;
    ro.xy *= my;rd.xy *= my;
    
    float sel = float(maptype);
    float t = iSphere(ro,rd);
    vec3 col = vec3(0);
    if (sel == 0.) col = cubeproj(rd)*1.1;
    else if (sel == 1.) col = tpl(rd)*1.2;
    else if (sel == 2.) col = healpix(rd);
    else col = sphproj(rd);
    
	vec3 sol = vec3(0.0);
    vec3 pos = ro+rd*t;
    if (sel == 0.) sol = cubeproj(pos)*1.1;
    else if (sel == 1.) sol = tpl(pos)*1.2;
    else if (sel == 2.) sol = healpix(pos);
    else sol = sphproj(pos);
        
    vec3 sc = mix(col,sol,mixer);
    sc *= 1.5-sqrt(pow(sc,vec3(3.33)));
    
	gl_FragColor = vec4(sc, 1.0);
}
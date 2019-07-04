/*{
  "CREDIT": "by mojovideotech",
  "DESCRIPTION": "",
  "CATEGORIES": [
    "generator",
    "2d",
    "iterations"
  ],
  "INPUTS": [
    {
      "NAME": "center",
      "TYPE": "point2D",
      "DEFAULT": [
        -1.25,
        -1.75
      ],
      "MAX": [
        2,
        2
      ],
      "MIN": [
        -2,
        -2
      ]
    },
    {
      "NAME": "rate",
      "TYPE": "float",
      "DEFAULT": 0.37,
      "MIN": -3,
      "MAX": 3
    },
    {
      "NAME": "vectorX",
      "TYPE": "float",
      "DEFAULT": 2.92,
      "MIN": 0,
      "MAX": 6.2831853
    },
    {
      "NAME": "vectorY",
      "TYPE": "float",
      "DEFAULT": 0.14,
      "MIN": -1,
      "MAX": 1
    },
    {
      "NAME": "rot",
      "TYPE": "float",
      "DEFAULT": 4.11,
      "MIN": 0,
      "MAX": 6.2831853
    },
    {
      "NAME": "width",
      "TYPE": "float",
      "DEFAULT": 2.5,
      "MIN": 0.005,
      "MAX": 2.5
    },
    {
      "NAME": "push",
      "TYPE": "float",
      "DEFAULT": 1.87,
      "MIN": -10,
      "MAX": 10
    },
    {
      "NAME": "zoom",
      "TYPE": "float",
      "DEFAULT": -19.1,
      "MIN": -200,
      "MAX": 1
    },
    {
      "NAME": "detail",
      "TYPE": "float",
      "DEFAULT": 10.61,
      "MIN": 10,
      "MAX": 19.9
    },
    {
      "NAME": "depth",
      "TYPE": "float",
      "DEFAULT": 19.18,
      "MIN": 7,
      "MAX": 36
    },
    {
      "NAME": "color",
      "TYPE": "float",
      "DEFAULT": 0.6,
      "MIN": 0.025,
      "MAX": 1
    },
    {
      "NAME": "hue",
      "TYPE": "float",
      "DEFAULT": 1.59,
      "MIN": 1,
      "MAX": 10
    }
  ]
}*/

///////////////////////////////////////////
// TextileMutationLattice  by mojovideotech
//
// mod of:
// LaceLikeLattice2  by mojovideotech
// interactiveshaderformat.com/\1281
//
// based on :
// String Theory by nimitz 
// shadertoy.com/\XdSSz1
//
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
///////////////////////////////////////////



float hash (vec2 xy) { return (log(pow(xy.x,xy.y)*tan(317811.0))); }

mat2 mm2 (in float a) {
	float c = cos(a), s = sin(a);
	return mat2(c,-s,s,c);
}

float f (vec2 p, float t) {
	p.y = sin(p.y*1.+t*1.23)*cos(t+p.y*0.05);	
    p += sin(p.y*0.05)*hash(p);
    return smoothstep(-0.01,width,abs(p.x));
}

vec3 g (in vec3 p, in vec3 t) { return vec3(dot(cos(t),p)); }

void main()
{
	float aspect = RENDERSIZE.x/RENDERSIZE.y;
	float time = TIME * rate * 10.0;
	vec2 uv = vec2(gl_FragCoord.xy/RENDERSIZE.xy*2.0-center.xy);
	vec2 p = uv.yx;
	p.y *= RENDERSIZE.x/RENDERSIZE.y;
	p *= mm2(rot)*zoom;	
	p.y = abs(p.y);
	vec3 col = vec3(0.0);
	float counter = depth;
	float k = hash(p);
	for(float i=0.;i<36.;i++)
	{
        p.y -= 20.0 - detail;
        p.x -= hash(vec2(time*0.125,push));
		p*= mm2(i*vectorY+vectorX);
        vec2 pa = vec2(abs(p.y-1.5),abs(p.x));
        vec2 pb = vec2(p.x,abs(p.y));
        p = mix(pa,pb,smoothstep(0.5,.1,f(vec2(push,detail*0.5),time)));
        col /= g(col,vec3(pa.x,pb));
        vec3 col2 = (sin(vec3(13.0*hue,21.,34.0/hue)+i*color)*color+(1.0-color)+0.125)*(1.-f(p,time));
		col += mix(col,col2,1.0/i);
		p += f( sin( col.rg ) + exp( -sin( col2.gb )),i);
        counter -= 1.0;
        if (counter<1.0)  break; 
	}
	col += g(col,col);
	gl_FragColor = vec4(col,1.0);
}
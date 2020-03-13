/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MllGRl by guil.  Lightning bolt",
  "INPUTS" : [

  ]
}
*/


// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// Created by S.Guillitte

#define time  TIME*5.

float hash( in vec2 p ) 
{
    return fract(sin(p.x*15.32+p.y*35.78) * 43758.23);
}

vec2 hash2(vec2 p)
{
	return vec2(hash(p*.754),hash(1.5743*p.yx+4.5891))-.5;
}


vec2 add = vec2(1.0, 0.0);

vec2 noise2(vec2 x)
{
    vec2 p = floor(x);
    vec2 f = fract(x);
    f = f*f*(3.0-2.0*f);
    
    return mix(mix( hash2(p),          hash2(p + add.xy),f.x),
                    mix( hash2(p + add.yx), hash2(p + add.xx),f.x),f.y);
    
}


float dseg( vec2 ba, vec2 pa )
{	
	float h = clamp( dot(pa,ba)/dot(ba,ba), 0., 1.5 );	
	return length( pa - ba*h );
}

float arc(vec2 x,vec2 p, vec2 dir)
{
    vec2 r = p;
    float d=10.;
    for (int i = 0; i < 5; i++)
    {
        vec2 s= noise2(r+time)+dir;
        d=min(d,dseg(s,x-r));
        r +=s;      
    }
    return d*3.;
    
}



float thunderbolt(vec2 x)
{
    vec2 r = vec2(8.,1.);
    float d=1000.;
    if((x).y<abs((x-r).x))return d;    
    for (int i = 0; i < 19; i++)
    {
        if(r.y>x.y+.5)break;
        vec2 s= noise2(r+time)+vec2(0.,.7);
        d=min(d,dseg(s,x-r));
        r +=s;
        if(i-(i/5)*5==0){
            if(i-(i/10)*10==0)d=min(d,arc(x,r,vec2(.3,.5)));
            else d=min(d,arc(x,r,vec2(-.3,.5)));
        }
    }
    return d;
   
}

vec2 fbm2(vec2 x)
{
    vec2 r = x; 
    for (int i = 0; i < 9; i++)
    {
        vec2 s= .5*noise2(r+time);
        r +=s;
             
    }
    return r-x;
   
}



void main() {



	vec2 uv = 2.*gl_FragCoord.xy / RENDERSIZE.yy;
    uv*=5.;
    float c = thunderbolt(uv+.02*fbm2(5.*uv));
    c=exp(-5.*c);
    vec3 col;
    col=clamp(1.7*vec3(0.8,.7,.9)*c,0.,1.);
	gl_FragColor = vec4(col,1.0);
}






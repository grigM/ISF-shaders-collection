/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/WlBXDD by Artleet.  Playing around with Perlin noise and hue shift",
    "IMPORTED": {
    },
    "INPUTS": [
    ]
}

*/


#define R RENDERSIZE

vec3 h(vec3 c, float s){
    vec3 m=vec3(cos(s),s=sin(s)*.5774,-s);
    return c*mat3(m+=(1.-m.x)/3.,m.zxy,m.yzx);
}

float n(vec2 u){
    vec4 d=vec4(.106,5.574,7.728,3.994),q=u.xyxy+TIME*.1,p=floor(q);
    q-=p;--q.zw;++p.zw;p=fract(p*d.xyxy);d=p+d.wzwz;
	d=p.xxzz*d.ywyw+p.ywyw*d.xxzz;p=fract((p.xxzz+d)*(p.ywyw+d));
    p=cos(p*=TIME+6.)*q.xxzz+sin(p)*q.ywyw;q*=q*(3.-2.*q);
    p=mix(p,p.zwzw,q.x);return mix(p.x,p.y,q.y);
}

void main() {

    vec2 i,u=gl_FragCoord.xy/R.xy;u.x*=R.x/R.y;i=floor(u*=10.);u-=i+.5;    
    vec3 a=vec3(n(i*.05),n(i*.1),n(i*.15)),b=a*6.,r=u.x*sin(b)+u.y*cos(b);
	gl_FragColor=vec4(h(step(r,vec3(a.y*.5)),a.z*20.),1.);    
}

/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#41248.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

vec2 rotate(vec2 a, float b){
    float c=length(a),r=atan(a.x,a.y)+b;
    return vec2(cos(r),sin(r))*c;
}
float box(vec3 p,vec3 b) {
    return length(max(abs(p)-b,0.));
}
float smin( float a, float b)
{
    float k = 0.1;
    float h = clamp( 0.5+0.5*(b-a)/k, 0.0, 1.0 );
    return mix( b, a, h ) - k*h*(1.0-h);
}
float map(vec3 o) {
    float f = 1e8;
	float th = sin(TIME*3.14/2. + o.y*1.0 + 20.0)+sin(TIME);
	//th -= atan(o.x, o.z)*0.25+o.y*.5;
	o.xz *= mat2(cos(th), sin(th), -sin(th), cos(th));
	o.x += sin(TIME+o.y)*.5 + sin(TIME*2.)*.3;
	f =  box( o, vec3(.5, 4., .5) );
	
	float mu = 10.;
	float add = (sin(o.x*mu)+sin(o.z*mu))*.001;
	f += add;
    return f;
}
vec3 normal( in vec3 x)
{
    vec2 e = vec2( .01, 0.0 );
    return normalize( vec3( map(x+e.xyy) - map(x-e.xyy),
                            map(x+e.yxy) - map(x-e.yxy),
                            map(x+e.yyx) - map(x-e.yyx) ) );
}

float trace(vec3 o, vec3 d){float t=0.;for(int i=0;i<32;i++){t+=map(o+d*t)*.7;}return t;}
void main() {
    vec2 u = gl_FragCoord.yx/RENDERSIZE.yx * vec2(0.2, 1.0) + vec2(0.4, 0);
    vec3 mc = vec3(0,0.2,.2),
	    c=mc;
    vec3 o = vec3(0,3,-5), d=vec3(u*2.-1.,2.);
    d.y-=.7;
    d.x*=RENDERSIZE.x/RENDERSIZE.y;
    d=normalize(d);

    float t = trace(o, d);
    vec3 h = o+d*t;
    float b = map(h);
    if(b<.01){
        float m = abs(dot(normal(h),normalize(h)));
        c=vec3(max(m,.1));
        c=mix(mc,c,dot(normal(h),-d));
        float s = (length(normal(h))/10.-map(h+normal(h)/10.))*1.;
        c-=s;
    }
    c=mix(c,vec3(0),length(u-.5));
    gl_FragColor = vec4(pow(c + vec3(1,3,3) * 0.01, vec3(0.7)),1);
}
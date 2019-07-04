/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#53619.0",
  "PASSES" : [
    {
      "TARGET" : "backbuffer",
      "PERSISTENT" : true
    }
  ],
  "ISFVSN" : "2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


mat2 rotate(float a) {
	float c = cos(a);
	float s = sin(a);
	return mat2(c, s, -s, c);
}

// http://glslsandbox.com/e#36217.0
float hash(float x)
{
  return fract(21654.6512 * sin(385.51 * x));
}
float hash(in vec2 p)
{
  return fract(sin(p.x*15.32+p.y*35.78) * 43758.23);
}

vec2 hash2(vec2 p)
{
  return vec2(hash(p*.754),hash(1.5743*p.yx+4.5891))-.5;
}

vec2 hash2b(vec2 p)
{
  return vec2(hash(p*.754),hash(1.5743*p+4.5476351));
}

vec2 add = vec2(1.0, 0.0);

vec2 noise2(vec2 x)
{
  vec2 p = floor(x);
  vec2 f = fract(x);
  f = f*f*(3.0-2.0*f);
  vec2 res = mix(mix( hash2(p),          hash2(p + add.xy),f.x),
                 mix( hash2(p + add.yx), hash2(p + add.xx),f.x),f.y);
  return res;
}

vec2 fbm2(vec2 x)
{
  vec2 r = vec2(0.0);
  float a = 1.0;
    
  for (int i = 0; i < 8; i++)
  {
    r += abs(noise2(x)+.5 )* a;
    x *= 2.;
    a *= .5;
  }
  return r;
}

mat2 m2 = mat2( 0.80, -0.60, 0.60, 0.80 );

vec2 fbm3(vec2 x)
{
  vec2 r = vec2(0.0);
  float a = 1.0;
  for (int i = 0; i < 6; i++)
  {
    r += m2*noise2((x+r)/a)*a; 
    r =- .8*abs(r);
    a *= 1.7;
  }     
  return r;
}

vec3 storm(vec2 x)
{
  float t = .5*TIME;
  float st = sin(t), ct = cos(t);
  m2 = mat2(ct,st,-st,ct);
  x = fbm3(x+0.5*TIME)+2.;
  x *= .35;
        
  float c = length(x);
  c = c*c*c;
  vec3 col=vec3(0.6-.1*x.x,0.7,0.8-.1*x.y)*c*x.y;   
  return clamp(col,0.,1.);
}

float dseg(vec2 ba, vec2 pa)
{
  float h = clamp( dot(pa,ba)/dot(ba,ba), -0.2, 1. );	
  return length( pa - ba*h );
}

float arc(vec2 x, vec2 p, vec2 dir)
{
  vec2 r = p;
  float d = 10.;
  for (int i = 0; i < 5; i++)
  {
    vec2 s = noise2(r+TIME)+dir;
    d = min(d,dseg(s,x-r));
    r += s;      
  }
  return d*3.0; 
}

float thunderbolt(vec2 x, vec2 tgt)
{
  vec2 r = tgt;
  float d = 1000.;
  float dist = length(tgt-x);
     
  for (int i = 0; i < 19; i++)
  {
    if(r.y > x.y+.5)break;
    vec2 s = (noise2(r+TIME)+vec2(0.,.7))*2.;
    dist = dseg(s,x-r);
    d = min(d,dist);
    r += s;
    if(i-(i/5)*5==0)
    {
      if(i-(i/10)*10==0)
           d = min(d,arc(x,r,vec2( .3,.5)));
      else d = min(d,arc(x,r,vec2(-.3,.5)));
    }
  }
  return exp(-5.*d)+.2*exp(-1.*dist);
}
///

float noise(vec2 p){
	vec2 ip = floor(p);
	vec2 u = fract(p);
	u = u*u*(3.0-2.0*u);
	
	float res = mix(
		mix(hash(ip),hash(ip+vec2(1.0,0.0)),u.x),
		mix(hash(ip+vec2(0.0,1.0)),hash(ip+vec2(1.0,1.0)),u.x),u.y);
	return res*res;
}


float layer(vec2 uv, float fz) {
	float col = (0.);
	
	
	vec2 f = 2. * fract(uv) - 1.;
	vec2 i = floor(uv);
	
	
	vec2 t = hash2(i) * .8;
	float d = length(f - t);
	
	
	float r = hash(i) * .01 + .001;
	col += smoothstep(r + fz, r, d);
	col += .0008 / d;
	
	
	return col;
}

vec3 lightening(vec2 p) {
	
  vec2 tgt = vec2(1., -8.);
  float c = 0.;
  vec3 col = vec3(0.);
  float t = hash(floor(5.*TIME));
  tgt += 8.*hash2b(tgt+t);
  if(hash(t+2.3) > 0.8)
  {
    c = thunderbolt(p*10.+2.*fbm2(5.*p),tgt);	
    col += clamp(1.7*vec3(0.8,.7,.9)*c,0.,1.);	
  }
	return col;
}

void main() {
	
	vec2 uv = (2. * gl_FragCoord.xy - RENDERSIZE) / RENDERSIZE.y;
	vec2 u = uv;
	vec3 col = vec3(0.);

	uv *= 10.;
	//col = layer(uv, .04);
	
	float r = .0;
	col += lightening(u);
		
	for (float i = 0.; i < 1.; i += .02) {
		float r = hash(vec2(i)) * 1.28 - .786;
		uv.y += TIME / 10.;
		uv *= rotate(.2 + r);
		//uv.y += TIME / 10.;
		float t = fract(i);
		float s = smoothstep(1., 0., t);
		float fz = (1. - i) / 10.;
		uv *= s;
		uv += hash2(vec2(i, i * 2.));
		col += smoothstep(.3, 1., noise(uv + TIME)) / 50. * u.y;
		col += layer(uv, fz) * .4;
		col += .2* (1. -u.y) * .0000001;
		
		
		
	}
	col += lightening(u + hash(floor(TIME))) * .5;

	col += lightening(hash(floor(TIME * 34.)) + u * 2.) * .2;
		
	//col *= .5 + .5 * cos(uv.x + col + TIME + vec3(23, 21, 0));
	
	
	col = col * .5 +  IMG_NORM_PIXEL(backbuffer,mod(gl_FragCoord.xy / RENDERSIZE,1.0)).rgb *.8;
	gl_FragColor = vec4(col, 1.);

}
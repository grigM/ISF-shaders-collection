/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#6406.8"
}
*/


#ifdef GL_ES
precision mediump float;
#endif
// by Tonnor
// トーンカーブフィルタとか試験的にかけてみたが？ 2013/02/03修正

vec3 Hue( float hue )
{
	vec3 rgb = fract(hue + vec3(0.0,2.0/3.0,1.0/3.0));

	rgb = abs(rgb*2.0-1.0);
		
	return clamp(rgb*3.0-1.0,0.0,1.0);
}

vec3 HSV2RGB( vec3 hsv ) { return ((Hue(hsv.x)-1.0)*hsv.y+1.0) * hsv.z; }

vec3 tonereplace( vec3 rgb )//Rを持ち上げ、Gは0.5以上を膨らませて0.5以下を絞る、BはGの逆のかけ方という典型的なトーン置き換えフィルタ 
{ 
 vec3 tone;
 vec3 tcp = vec3(0.4,0.2,0.5);	
	tone.x = pow(rgb.x,1.0-tcp.x);
	tone.z = (pow(rgb.z,1.0+(1.0-(abs(rgb.z-0.5)*2.0))*sign(rgb.z-0.5)*tcp.z) );
	tone.y = (pow(rgb.y,1.0-(1.0-(abs(rgb.y-0.5)*2.0))*sign(rgb.y-0.5)*tcp.y) );	
 return tone;
	 }


vec2 trans(vec2 p)
{
  float sec;
  sec = TIME*1.016;
  return vec2(p.x+cos(sec), p.y+sin(sec));
}

float sq(vec2 p)
{
	vec2 p2 = floor(p*5.)*0.2;
	p2 = mod(p2*8.0, 4.0)-2.0;
	p2.x = 0.2+sin(p2.x*2.)*p2.x;	
	p2.y = 0.2+cos(p2.y*2.)*p2.y;
	float r2 = (p2.x*p2.y);	
	return r2;
}

void main( void ) {

 
  vec2 uv  = -1.0 + 2.0 * (gl_FragCoord.xy/RENDERSIZE.xy) +sin(TIME*.4)*0.8;
  vec3 D   = vec3(uv.x * 1.25, uv.y, 0.85);
  vec3 p   = vec3(TIME * 1.2, 0.25, TIME*4.);
  vec3 pdel = vec3 (0,pow(sin(uv.x*.3+uv.y*4.+TIME * 2.1),3.),sin(TIME * 4.0)*cos(TIME*3.)*2.);	
  vec3 g   = p;
  for(int i = 0 ; i < 50; i++) {
    float k = 3.0 - dot(abs(g), vec3(0, 1, 0)) +pdel.y* sin(g.x) * cos(g.z+pdel.z)*2.;
    g += D * k * 0.24;
  }	  
	float s = 0.3;
	float v = sq(trans(g.xz * 0.2))*0.20 +0.50;
	float h = (gl_FragCoord.x / RENDERSIZE.x);

	h =sq(trans(g.xz * 0.2));
	
  float c = length(g - p) * 0.03;

	
	gl_FragColor = vec4(tonereplace(HSV2RGB(vec3(h,s,v))),1.0)+c;
}
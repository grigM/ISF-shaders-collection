/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#3641.5"
}
*/


//IQ Version - http://pouet.net/topic.php?which=7931
// las style mod - might be a good alternative in some situations
// rotwang: @mod* some smoothsteps @mod* aspect
#ifdef GL_ES
precision highp float;
#endif


float HexPrism( vec2 p, float h )
{
    vec2 q = abs(p);
    return max(dot(q, vec2(0.866025, 0.5)), q.y)-h;
}

float udBox( vec3 p, vec3 b )
{
  return length(max(abs(p)-b,0.0));
}

float sdBox( vec3 p, vec3 b )
{
  vec3  di = abs(p) - b;
  float mc = max(max(di.x, di.y), di.z);
  return min(mc, length(max(di,0.0)));
}

float fbox(vec3 p, vec3 s) { // las variant
        vec3 d = abs(p) - s;
        float m = max(d.x,max(d.y, d.z));
        return mix(m, length(max(d, 0.0)),step(0.,m));
}

float sdBox1( vec3 p, vec3 b )
{
    vec3 d = abs(p) - b;
    return length(max(d, 0.0)) + min(max(d.x,max(d.y, d.z)), 0.);
}

float sdBox2( vec3 p, vec3 b )
{
  vec3  di = abs(p) - b;
  return max(max(di.x, di.y), di.z);
}

vec4 distance2Color(float d) {
	vec4 c = vec4(1,1,1,1);
	//vec4 c = mix(vec4(1,0,0,1), vec4(0,1,0,1), sign(d) * 0.5 + 0.5);
	c *= abs(floor(sin(d*20.)));
	//c *= 1. - smoothstep(abs(d), 0.0, 0.25);
	return c;
}	

void main(void)
{
    vec2 p = -1.0 + 2.0 * gl_FragCoord.xy / RENDERSIZE.xy;
    p.x *= RENDERSIZE.x / RENDERSIZE.y;
    p *= 1.5;
	
   
    
    float d7 = sdBox2(vec3(p, 0.), vec3(1.)); // SIGNED - NOT EUCLID - LAS
	

    gl_FragColor = distance2Color(floor((d7*5.0)*(cos(TIME*5.0)))*2.0);
}
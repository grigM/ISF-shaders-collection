/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xl3SzB by lomateron.  click to change pattern",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


//iq color palette
vec3 pal(float t, vec3 a, vec3 b, vec3 c, vec3 d )
{
    return a + b*cos( 6.28318*(c*t+d) );
}
void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv = uv*2.-1.;
    uv.x*=RENDERSIZE.x/RENDERSIZE.y;
    
    float e = TIME*.125;
    float d = 128.*iMouse.x/RENDERSIZE.x+8.5;
    
    float zoom = 16.;
    vec2 g = uv*zoom;
    uv = d*(floor(g)+.5)/zoom;
    g = fract(g)*2.-1.;
    
    float f = dot(uv,uv)-e;
    
    vec4 c = vec4(
        pal( f*.5 + e,
            vec3(0.5,0.5,0.5),
            vec3(0.5,0.5,0.5),
            vec3(1.0,1.0,1.0),
            vec3(0.0,0.10,0.20)),1.);
    
	gl_FragColor = c*(1.-dot(g,g))*.2/abs((fract(f)-.5)*8.);
}
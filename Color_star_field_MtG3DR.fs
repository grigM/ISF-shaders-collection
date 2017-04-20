/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "3d",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MtG3DR by frutbunn.  Click mouse to move center.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


#define PI 3.14159265358979323846
#define TIMER(sec, min, max) (((mod(TIME, (sec)) * ((max) - (min))) / (sec)) + (min))
mat2 mm2(in float a) { float c = cos(a), s = sin(a); return mat2(c, s, -s, c); }

void main()
{
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy - vec2(.5);
    uv.x *= RENDERSIZE.x/RENDERSIZE.y;
    
    vec2 mouse = iMouse.xy;    
    if (mouse != vec2(0.)) {
        mouse = vec2(mouse.x/RENDERSIZE.x -.5, mouse.y/RENDERSIZE.y -.5);
    	mouse.x *= RENDERSIZE.x/RENDERSIZE.y;
    }
        
    vec3 color = vec3(0.);
    
    vec3 ray = vec3(uv-mouse, .75);
    ray.xy *= mm2(TIMER(15. ,0., -PI*2.));
    vec3 s = ray/max(abs(ray.x), abs(ray.y))*.4;

    vec3 p = s;
    for(int i=0; i<5; i++) {
        vec2 nos1 = vec2(floor(p.xy*30.334) );
        const vec2 nos2 = vec2(12.9898, 78.233);
        const float nos3 = 43758.5453;

        vec3 nc = vec3( fract(sin(dot(nos1, nos2))*nos3), fract(sin(dot(nos1, nos2*.5))*nos3), fract(sin(dot(nos1, nos2*.25))*nos3) );
        float n = fract(sin(dot(nos1, nos2*2.) )*nos3);       
        float z = fract(cos(n)-sin(n)-TIME*.2);
     
        float d = (1.-abs(30.*z-p.z) );

        float sz = 1./s.z;
        vec3 c = vec3(sin( max(0., d*(sz*nc.r)) ), sin( max(0., d*(sz*nc.g)) ), sin( max(0., d*(sz*nc.b)) ) );

        color += (1.-z)*c;
        p += s;
    }    
    
    gl_FragColor = vec4( max(vec3(0.), min(vec3(1.), color) ), 1.);
}
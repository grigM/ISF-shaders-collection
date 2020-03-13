/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#59344.0"
}
*/


// Lightning
// By: Brandon Fogerty
// bfogerty at gmail dot com
// xdpixel.com



// By: Brandon Fogerty
// bfogerty at gmail dot com
// xdpixel.com


// EVEN MORE MODS BY 27


#ifdef GL_ES
precision lowp float;
#endif


// EVEN MORE MODS BY 27




#ifdef GL_ES
precision lowp float;
#endif




const float count = 27.0;
const float speed = 2.7;


float Hash( vec2 p, in float s)
{
    vec3 p2 = vec3(p.xy,27.0 * abs(sin(s)));
    return fract(sin(dot(p2,vec3(27.1,61.7, 12.4)))*273758.5453123);
}


float noise(in vec2 p, in float s)
{
    vec2 i = floor(p);
    vec2 f = fract(p);
    f *= f * (3.0-2.0*f);
    
    
    return mix(mix(Hash(i + vec2(0.,0.), s), Hash(i + vec2(1.,0.), s),f.x),
               mix(Hash(i + vec2(0.,1.), s), Hash(i + vec2(1.,1.), s),f.x),
               f.y) * s;
}


float fbm(vec2 p)
{
    float v = - noise(p * 02., 0.25);
    v += noise(p * 01.1, 0.5) - noise(p * 01.1, 0.25);
    v += noise(p * 02.1, 0.25) - noise(p * 02.1, 0.125);
    v += noise(p * 04.1, 0.125) - noise(p * 08.1, 0.0625);
    v += noise(p * 08.1, 0.0625) - noise(p * 16., 0.03125);
    v += noise(p * 16.1, 0.03125);
    return v;
}


void main( void )
{
    float worktime = TIME * speed;
    
    vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy ) * 2.0 - 1.0;
    uv.x *= RENDERSIZE.x/RENDERSIZE.y;
    
    
    vec3 finalColor = vec3( 0.0, 0.0, 0.0 );
    for( float i = 1.0; i < count; i++ )
    {
        float t = abs(1.0 / ((uv.x + fbm( uv + worktime / i )) * (i * 100.0)));
        finalColor +=  t * vec3( i * 0.075, 0.7, 2.0 );
    }
    
    gl_FragColor = vec4( finalColor, 1.0 );
    
    
}
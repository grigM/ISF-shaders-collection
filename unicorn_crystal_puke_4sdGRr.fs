/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4sdGRr by nicoptere.  one has to start somewhere...",
  "INPUTS" : [

  ]
}
*/


#define PI 3.1415926535897932384626433832795

float hash(in float n)
{
    return fract(sin(n)*43758.5453123);
}

void main() {



    float time = TIME;
    
    //"squarified" coordinates 
	vec2 xy = ( 2.* gl_FragCoord.xy -RENDERSIZE.xy ) / RENDERSIZE.y ;
    //rotating light 
    vec3 lightDir = vec3( sin( time ), 1., cos( time * .5 ) );
    
    const float count = 200.;
    
    vec3 pp = vec3(0.);
    float length = 1e10;
    for( float i = 0.; i < count; i+=1. )
    {
        //random cell
        float an = sin( time * PI * .00001 ) - hash( i ) * PI * 2.;
        float ra = sqrt( hash( an ) );
    	vec2 p = vec2( lightDir.x + cos( an ) * ra, lightDir.z + sin( an ) * ra );
        //finds the closest cell center from XY coords
        float di = distance( xy, p );
        length = min( length, di );
        if( length == di )
        {
            pp.xy = p;
            pp.z = i / count * ( xy.y*xy.x );
        }
    }
    //shimmy shake
    gl_FragColor = vec4( pp + vec3( 1.) * ( 1.- max( 0.0, dot( pp, lightDir)) ), 1. );
}

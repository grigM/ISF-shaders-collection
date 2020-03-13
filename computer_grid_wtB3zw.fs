/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wtB3zw by tristanwhitehill.  grid",
  "INPUTS" : [

  ]
}
*/


vec2 hash( vec2 x ) 
{
    const vec2 k = vec2( 0.3183099, 0.3678794 );
    x = x*k + k.yx;
    return -1.0 + 2.0*tan( (TIME*.004)*130.0 * k*fract( cos (x.x/x.y/(x.x*x.y))) );
}

float noise( in vec2 p )
{
    vec2 i = floor( p );
    vec2 f = fract( p*  floor(TIME*.4)*10. );
	
	vec2 u = f*f*floor(1.0-8.0*f);

    return mix( mix( dot( hash( i + vec2(0.0,0.0) ), f - vec2(0.0,0.0) ), 
                     dot( hash( i + vec2(1.0,0.0) ), f - vec2(1.0,0.0) ), u.x),
                mix( dot( hash( i + vec2(0.0,1.0) ), f - vec2(0.0,1.0) ), 
                     dot( hash( i + vec2(1.0,1.0) ), f - vec2(1.0,1.0) ), u.x), u.y);
}
void main() {



   
	vec2 r = vec2( gl_FragCoord.xy / RENDERSIZE.xy );
    float mathy =cos(TIME*.07)*mod(r.y,TIME);
     float n= floor(noise(10.*r));
	vec3 backgroundColor = vec3(0.0);
	vec3 axesColor = vec3(0.0, 0.0, 1.0);
	vec3 gridColor = vec3(0.0,1.0,.3);
    
	vec3 pixel = backgroundColor;
	
	const float tickWidth = .005;
	for(float i=0.0; i<1.0; i+=tickWidth) {
	
		if(fract(sin((TIME+.05)+r.x + i))<0.002) pixel = gridColor;
		if(fract(cos((TIME+.03)+r.y - i))<0.002) pixel = gridColor;
    }
  
	gl_FragColor = vec4(pixel/(n/r.x), 1.0);
}

/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 6.0,
		"MIN": 0,
		"MAX": 20.0
		
	},
	{
		"NAME": "ofset",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0,
		"MAX": 2.0
		
	},
	{
		"NAME": "line_glow",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": 0.5,
		"MAX": 2.0
		
	}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#40423.0"
}
*/


// Trinity
// By: Brandon Fogerty
// bfogerty at gmail dot com
// xdpixel.com


//"For God so loved the world, that he gave his only Son, 
// that whoever believes in him should not perish but have eternal life." (John 3:16)
	
// "Go therefore and make disciples of all nations, baptizing them in 
// the name of the Father and of the Son and of the Holy Spirit, 
// teaching them to observe all that I have commanded you. 
// And behold, I am with you always, to the end of the age." - King Jesus (Matthew 28:19-20)

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable



#define Resolution				RENDERSIZE


float line( vec2 a, vec2 b, vec2 p )
{
	vec2 aTob = b - a;
	vec2 aTop = p - a;
	
	float t = dot( aTop, aTob ) / dot( aTob, aTob);
	
	t = clamp( t, 0.0, 1.0);
	
	float d = length( p - (a + aTob * t) );
	d = .01 / d;
	
	return clamp( d, 0.0, 1.0 );
}


void main( void ) {
	float aspectRatio = RENDERSIZE.x / RENDERSIZE.y;
	
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	
	vec2 signedUV = uv * 2.0 - 1.0;
	signedUV.x *= aspectRatio;
	signedUV.y += 0.2;
	
	vec3 finalColor = vec3( 0.0 );
	float a = floor(mod((TIME*speed)+ofset,3.0)) * 2.0943951 + 1.5707693;
	float t = line( vec2(cos(a),sin(a)), vec2(cos(a+2.0943951),sin(a+2.0943951)), signedUV);
	finalColor += vec3(pow(t,line_glow));
	


	gl_FragColor = vec4( finalColor, 1.0 );

}
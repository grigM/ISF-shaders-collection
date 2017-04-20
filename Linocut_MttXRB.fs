/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "engraving",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MttXRB by spite.  simulating the result of a linocut picture",
  
  
  
   "INPUTS": [
		{
      		"TYPE" : "image",
      		"NAME" : "inputImage"
    	},
		{
			"NAME": "scale",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MAX": 5.0,
			"MIN": -2.0
		}
      ]
}
*/


float luma(vec3 color) {
  return dot(color, vec3(0.299, 0.587, 0.114));
}

float luma(vec4 color) {
  return dot(color.rgb, vec3(0.299, 0.587, 0.114));
}

 
void main()
{
	float radius = .5;
    vec2 center = vec2(RENDERSIZE.x/2., RENDERSIZE.y/2.);
    vec2 uv = gl_FragCoord.xy;
    
    
    vec2 d = uv - center;
    float r = length(d)/1000.;
    float a = atan(d.y,d.x) + scale*(radius-r)/radius;
    //a += .1 * TIME;
    vec2 uvt = center+r*vec2(cos(a),sin(a));
    
	vec2 uv2 = gl_FragCoord.xy / RENDERSIZE.xy;
    float c = ( .75 + .25 * sin( uvt.x * 1000. ) );
    vec4 color = IMG_NORM_PIXEL(inputImage,mod(uv2,1.0));
    float l = luma( color );
    float f = smoothstep( .5 * c, c, l );
	f = smoothstep( 0., .5, f );
    
	gl_FragColor = vec4( vec3( f ),.0);
}
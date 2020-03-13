/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57937.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

uniform vec2 surfaceSize;

// visualization of gradation with dither pattern created by bayer-matrix

float bayer( int iter, vec2 rc )
{
	float sum = 0.0;
	for( int i=0; i<4; ++i )
	{
		if ( i >= iter ) break;
		vec2 bsize = vec2(pow(2.0, float(i+1)));
		vec2 t = mod(rc, bsize) / bsize;
		int idx = int(dot(floor(t*2.0), vec2(2.0,1.0)));
		float b = 0.0;
		if ( idx == 0 ) { b = 0.0; } else if ( idx==1 ) { b = 2.0; } else if ( idx==2 ) { b = 3.0; } else { b = 1.0; }
		sum += b * pow(4.0, float(iter-i-1));
	}
	float phi = pow(4.0, float(iter))+1.0;
	return (sum+1.0) / phi;
}

void main( void )
{
	vec2 position = vv_FragNormCoord;
	vec2 m = vec2(1.,0);

	float alpha = fract(dot(vv_FragNormCoord,vv_FragNormCoord)+TIME);
	float threshold = bayer( int(mix(1.0, 5.0, 1.0-m.y)), gl_FragCoord.xy-vec2(0.5) );
	float p = step(threshold, alpha);
	
	if (p<=0.)
	{
		discard;
	}
	
	gl_FragColor = vec4( p, p, p, 1.0 );

}
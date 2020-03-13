/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#58545.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


uniform vec2 surfaceSize;

// visualization of gradation with dither pattern created by bayer-matrix

float bayer( vec2 rc )
{
	float sum = 0.0;
	for( int i=0; i<3; ++i )
	{
		vec2 bsize;
		if ( i == 0 ) { bsize = vec2(2.0); } else if ( i==1 ) { bsize = vec2(4.0); } else if ( i==2 ) { bsize = vec2(8.0); };
		vec2 t = mod(rc, bsize) / bsize;
		int idx = int(dot(floor(t*2.0), vec2(2.0,1.0)));
		float b = 0.0;
		if ( idx == 0 ) { b = 0.0; } else if ( idx==1 ) { b = 2.0; } else if ( idx==2 ) { b = 3.0; } else { b = 1.0; }
		if ( i == 0 ) { sum += b * 16.; } else if ( i==1 ) { sum += b * 4.; } else if ( i==2 ) { sum += b * 1.; };
	}
	return sum / 64.;
}

void main( void )
{
	vec2 position = vv_FragNormCoord;
	vec2 m = vec2(1.,0);

	float alpha = fract(dot(vv_FragNormCoord,vv_FragNormCoord)+TIME/122.);
	float threshold = bayer(gl_FragCoord.xy-vec2(0.5) );
	float p = step(threshold, alpha);
	
	gl_FragColor = vec4( p, p, p, 1.0 );

}
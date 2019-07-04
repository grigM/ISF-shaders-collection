/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
      "NAME" : "x_iter",
      "TYPE" : "float",
      "MAX" : 20.0,
      "DEFAULT" : 10.0,
      "MIN" : 0.0
    },
    {
      "NAME" : "y_iter",
      "TYPE" : "float",
      "MAX" : 20.0,
      "DEFAULT" : 10.0,
      "MIN" : 0.0
    },
    {
      "NAME" : "speed",
      "TYPE" : "float",
      "MAX" : 3.0,
      "DEFAULT" : 1.0,
      "MIN" : 0.0
    },
    {
      "NAME" : "radius",
      "TYPE" : "float",
      "MAX" : 0.5,
      "DEFAULT" : 0.259,
      "MIN" : 0.0
    },
    {
      "NAME" : "x_radius",
      "TYPE" : "float",
      "MAX" : 30.0,
      "DEFAULT" : 7.0,
      "MIN" : 0.0
    },
    {
      "NAME" : "y_radius",
      "TYPE" : "float",
      "MAX" : 30.0,
      "DEFAULT" : 7.0,
      "MIN" : 0.0
    },
    {
      "NAME" : "z_cam_rot",
      "TYPE" : "float",
      "MAX" : 50.0,
      "DEFAULT" : 25.0,
      "MIN" : 0.0
    },
	{
      "NAME" : "y_cam_rot",
      "TYPE" : "float",
      "MAX" : 50.0,
      "DEFAULT" : 10.0,
      "MIN" : 0.0
    },
    {
      "NAME" : "smoothstep_max",
      "TYPE" : "float",
      "MAX" : 2.3,
      "DEFAULT" : 1.1953075,
      "MIN" : 0.3
    },
    {
      "NAME" : "glow",
      "TYPE" : "float",
     "DEFAULT" : 9.0,
      "MAX" : 9.7,
      "MIN" : 0.0
    },


    

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#2101.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

// metablobgalaxthing


float df( vec2 v, float r )
{
	float d = length(v);
	return 1.0 / d;
}

void main( void )
{
	vec2 pos = ( gl_FragCoord.xy );

	
	vec2 ctr = RENDERSIZE.xy / 2.0;
	//vec2 c0 = ctr +  vec2( 43.0 * cos(TIME*speed), 16.0 * sin(TIME*speed) );
	//vec2 c1 = ctr +  vec2( 50.0 * sin(TIME*speed), 52.0 * cos(1.3*(TIME*speed)) );
	//vec2 c2 = ctr +  vec2( 45.0 * sin(TIME*speed), 30.0 * cos(1.6*(TIME*speed)+sin(2.0*(TIME*speed))) );
	
	vec2 c[100];
	vec2 d[100];
	const float r = 30.0;
	float col = 0.0;
	
	for (float x = 0.0; x < x_iter; x++)
	for (float y = 0.0; y < y_iter; y++)
	{
	 c[int(x + x*y)] = ctr + radius*vec2( (z_cam_rot + (x + x*y)/1.0) * cos(((TIME*speed)*1.0) + x + x*y)*x_radius, (y_cam_rot + (x + x*y)/1.0)*sin((TIME*speed) + x + x*y)*y_radius);
	 d[int(x + x*y)] = c[int(x + x*y)] - pos;
	 col += df( d[int(x + x*y)], r);
	}
	
	
	
	
	//vec2 d0 = c0 - pos;
	//vec2 d1 = c1 - pos;
	//vec2 d2 = c2 - pos;
	
	
	//col += df( d0, r );
	//col += df( d1, r );
	//col += df( d2, r );
	
	float t = smoothstep_max;
	col = smoothstep( 0.0, 1.0, (col-t)/t );
	col = pow( col, 10.0-glow );
	
	gl_FragColor = vec4( vec3(col), 1.0 );

}
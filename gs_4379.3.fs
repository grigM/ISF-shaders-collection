/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#4379.3"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

//Hacky dithering using matrices


mat4 b0 = mat4( 
	0,0,0,0,
	0,0,0,0,
	0,0,0,0,
	1,0,0,0);

mat4 b1 = mat4(
	0,0,0,1,
	0,0,0,0,
	0,0,0,0,
	1,0,0,0);

mat4 b2 = mat4(
	0,0,0,0,
	0,0,1,0,
	0,1,0,0,
	1,0,0,0);

mat4 b3 = mat4(
	0,0,0,1,
	0,0,1,0,
	0,1,0,0,
	1,0,0,0);

mat4 b4 = mat4(
	0,0,0,1,
	1,0,1,0,
	0,1,0,0,
	1,0,1,0);

mat4 b5 = mat4(
	0,1,0,1,
	1,0,1,0,
	0,1,0,1,
	1,0,1,0);

float getPattern(float x,float y,float b)
{
	int ix = int(mod(x,4.0));
	int iy = int(mod(y,4.0));
	
	
	for(int x = 0; x < 5 ; ++x)
	{
		for(int y = 0; y < 5 ; ++y)
		{
			if(x == ix && y == iy)
			{
				float col = 0.0;
				if(b <= 1.0)
				{
					col = 1.0;

				}
				if(b <= 0.99)
				{
					col = b5[x][y];

				}
				if(b <= 0.95)
				{
					col = b4[x][y];

				}
				if(b <= 0.85)
				{
					col = b3[x][y];

				}
				if(b <= 0.70)
				{
					col = b2[x][y];

				}
				if(b <= 0.5)
				{
					col = b1[x][y];

				}
				if(b <= 0.25)
				{
					col = b0[x][y];
				}
				if(b <= 0.05)
				{
					col = 0.0;
				}
				
				return col;
			}
		}
	}
	return 0.0;
}

void main( void ) {

	vec2 p = ( gl_FragCoord.xy );

	float color = 0.0;
	
	color = distance(p,RENDERSIZE/2.)/32.;
	
	color = sin(color+TIME+(p.x/RENDERSIZE.x));

	color = getPattern(p.x,p.y,color);
	

	gl_FragColor = vec4( vec3( color), 1.0 );

}
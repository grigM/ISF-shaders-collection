/*{
	"CREDIT": "by isak.burstrom",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"XXX"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "size",
			"TYPE": "float",
			"DEFAULT": 0.27,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "sides",
			"TYPE": "float",
			"DEFAULT": 7.0,
			"MIN": 1.0,
			"MAX": 12.0
		},
		{
			"NAME": "rotation",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 6.283185307
		},
		{
			"NAME": "linewidth",
			"TYPE": "float",
			"DEFAULT": 0.04,
			"MIN": 0.0,
			"MAX": 0.22
		}
	]
}*/

/*
	Psychedeliscope
	===============

	1-05-16	
	Recreating an old java applet in a shader.	
	
	I first encountered "star polygons" in Robert Dixon's 1987 book,
	'Mathographics' ( pg. 126 ). 
	
	One of the formulas in the "Computer Drawings" chapter shows how a star 
	polygon of a given number of points can be drawn, working in polar
    coordinates. The formula is roughly this :

	For a polygon of N points and S sides:
 
	for( S = 0; S <= N; S++ )
                                             
    Angle = 360 * S * M / N; // where M is a whole number between 1 and N
                             // which shares no common factor with N. 

	The angle increment from one star point to the next is determined by this
	equation, above, and then the polar coordinates ( angle, radius ) are
	converted to Cartesian with these equations:
	
                       x = R * Cos( Angle );
                       y = R * Sin( Angle );
                       
	If values of M and N are chosen which do share a common factor, the angle 
	increment is still 360 * M / N, but since M / N can be reduced by a common 
	factor, the number of sides in the polygon is the reduced value of N, and 
	that polygon is drawn over itself the reduced value of M times.
    
    For example, if M = 8 and N = 12, 8 / 12 = 2 / 3, and a 3-sided polygon is
    drawn in the exact same position, 2 times.
 		
*/
//==============================================================================

// cosine based palette, 4 vec3 params
// http://www.iquilezles.org/www/articles/palettes/palettes.htm


vec3 resolution = vec3(RENDERSIZE, 1.);
float thickness = linewidth * size;


float maxPolyRadius = 2.25,
      polyRadius = maxPolyRadius * size;

const int maxSides = 28;

// Constant numRevs for sharp lines. 17 is lowest prime that avoids 
// drawing straight horizontal lines	
const int numRevs = 17; 
float _numRevs = float(numRevs);
    
	                                                           
//------------------------------------------------------------------------------
float drawLine( vec2 p1, vec2 p2, vec2 uv, float thickness ) 
{
	float a = abs( distance( p1, uv ) ),
	      b = abs( distance( p2, uv ) ),
	      c = abs( distance( p1, p2 ) );
	
	if ( a >= c || b >=  c ) return 0.0;
	
	float p = (a + b + c) * 0.5;
	
	// median to ( p1, p2 ) vector
	float h = 2.0 / c * sqrt( p * ( p - a ) * ( p - b ) * ( p - c ) );
	
	return mix( 1.0, 0.0, step( 0.5 * thickness, h ) );
}
    
// POLYGON DRAWING LOOP. ( Sets the value of 'clr' to non-zero in the 
// drawLine() function to indicate the current pixel is part of a line,
// if it is. The pixel's color is not set until this loop completes. )
// ---------------------
float draw_star(float numSides, float rotate) {
	float angle = 0.0,
      oldx = 0.0,
      oldy = 0.0,
      firstx = 0.0,
      firsty = 0.0,
      clr = 0.0;
      
	vec2 curPix = ( -resolution.xy + 2.0 * gl_FragCoord.xy ) / resolution.y;
	
	float x = curPix.x,
	      y = curPix.y,
	      ang = rotation - 1.570796327 + rotate;
	      
	curPix.x = x * cos( ang ) - y * sin( ang );
	curPix.y = x * sin( ang ) + y * cos( ang );	
	
    for( int curSide = 0; curSide < maxSides; curSide++ ) {
    	float _curSide = float(curSide);
    	
    	if(numSides == 7.0 || numSides == 11.0) {
    		angle = (1.0 - (4.0 / float(numSides))) * 360.0 * _curSide * _numRevs;
    	} else {
        	angle = 360.0 * _curSide * _numRevs / numSides;
    	}
                                                    
        x = polyRadius * cos( radians( angle ) );
        y = polyRadius * sin( radians( angle ) );
        
        if ( curSide == 0 ) {
        	//Store the first coord
        	firstx = x;
        	firsty = y;
        }
        else {
        	clr += drawLine( vec2( oldx, oldy ), vec2( x, y ), curPix, thickness );
        	
        	// Connect the last vertex to the first.
        	if ( curSide == int(numSides) - 1 ) {
        		clr += drawLine( vec2( firstx, firsty ), vec2( x, y ), curPix, thickness );	
				break;
            }
		}        		                                           
        	
        oldx = x;
        oldy = y;
        	
	}
	
	return clr;
}

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

float PI = 3.141592654;

void main()
{
	vec4 fragColor;
	float clr = 0.0;
	
	int numSides = int( sides );

	if(numSides == 6) {
		clr = draw_star(3.0, 0.0);
		
		if(clr == 0.0) {
			clr = draw_star(3.0, PI);	
		}
		
	} else if (numSides == 8) {
		clr = draw_star(4.0, 0.0);
		if(clr == 0.0) {
			clr = draw_star(4.0, PI/4.);	
		}
	} else if (numSides == 9) {
		clr = draw_star(3.0, 0.0);
		
		if(clr == 0.0) {
			clr = draw_star(3.0, PI/9. * 2.);	
		}
		
		if(clr == 0.0) {
			clr = draw_star(3.0, PI/9. * 4.);	
		}
	} else if (numSides == 12) {
		clr = draw_star(4.0, 0.0);
		
		if(clr == 0.0) {
			clr = draw_star(4.0, PI/3. * 2.);	
		}
		
		if(clr == 0.0) {
			clr = draw_star(4.0, PI/3.);	
		}
	} else {
		clr = draw_star(float(numSides), 0.0);	
	}
	
	// end polygon drawing loop
	if ( clr != 0.0 ) {
		fragColor = vec4( vec3( clr ), 1.0 );
	}
    
	gl_FragColor = fragColor;

}

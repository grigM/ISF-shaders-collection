/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#48892.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

//map from xy to ulam spiral and inverse 

//mouse down to zoom out and see error 






//functions 
float ulam_spiral(vec2 position);
vec2 inverse_ulam(float index);

float grid(float scale, vec2 coordinate);
float extract_bit(float n, float b);
float sprite(float n, vec2 p);
float digit(float n, vec2 p);
float print_number(float index, vec2 position);
float print_ulam_numbers(float n, float scale, vec2 print_coordinate);
float print_ulam_inverse(vec2 i, float scale, vec2 print_coordinate);






//maps coordinate p.xy to an ulam spiral number
float ulam_spiral(vec2 p)
{
	float x = abs(p.x);
	float y	= abs(p.y);
	bool q	= x > y;
	
	x	= max(x, y);
	y	= q ? abs(p.x+p.y) : abs(p.x - p.y);
	y 	= y + 4. * (x * x);
	x 	*= 2.;
	
	if(abs(p.x) <= abs(p.y))
	{
		return p.y > 0. ? y - x : y + x;
	}
	else
	{
	 	return p.x > 0. ? y - x - x : y;
	}
}





//maps the ulam spiral number n to an xy coordinate
vec2 inverse_ulam(float n)
{
	float r	= sqrt(n + .375);
	float s = 3. - fract(r) * 4.;	
	r	*= mod(r, 2.) > 1. ? .5 : -.5;
	
	return s > 1. ? vec2(r, 2. * r - r * s) : vec2(r * s, r);
}





void main( void ) 
{
	float scale		= 96. * mouse.y;

	vec2 coordinate		= floor(gl_FragCoord.xy - .5 * RENDERSIZE);
	
	vec2 field		= coordinate/scale;
	
	float spiral		= ceil(ulam_spiral(ceil(field)));
	vec2 inverse		= ceil(inverse_ulam(ceil(spiral)));
	
	
	vec4 result		= vec4(0.);	
	float opacity		= pow(clamp(mouse.y, 0., 1.), .25); 		//fade out when zooming
	float error		= float(inverse != ceil(field));		//draw red where the inverse fails to return the correct coordinates (zoom out to see)

	
	vec2 animation_position	= inverse_ulam(mod(TIME * 64., 512.));
	float animated_x	= float(inverse.x - animation_position.x == 0.);
	float animated_y	= float(inverse.y - animation_position.y == 0.);	
	float animated_line	= float(abs(spiral - ceil(ulam_spiral(ceil(animation_position)))) < 8.);	
	
	result			+= grid(scale, coordinate); 				
	result			+= print_ulam_numbers(spiral, scale, coordinate-vec2(2.)) * opacity;
	result			+= print_ulam_inverse(ceil(inverse), scale, coordinate-vec2(2., 9.)) * .5 * opacity;
	result			*= opacity * opacity;
	result 			+= (animated_x + animated_y) * .125 + animated_line * .5;
	result.x		+= error;
		
	result.w		= 1.;

	gl_FragColor		= result;
}//sphinx




////The rest of this code draws the grid and numbers


//draws a grid
float grid(float scale, vec2 coordinate)
{
	return max(float(mod(coordinate.x, scale) < 1.), float(mod(coordinate.y, scale) < 1.));
}


//returns the bit 1 or 0 from number n at power b
float extract_bit(float n, float p)
{
	n = floor(n);
	p = floor(p);
	p = floor(n/pow(2.,p));
	return float(mod(p,2.) == 1.);
}


//draws the bits of n into a 3 by 5 sprite over p
float sprite(float n, vec2 p)
{
	p 		= floor(p);
	float bounds 	= float(all(lessThan(p, vec2(3., 5.))) && all(greaterThanEqual(p,vec2(0,0))));
	return extract_bit(n, (2. - p.x) + 3. * p.y) * bounds;
}


//maps input n to the corrosponding float representing it's sprite graphic
float digit(float n, vec2 p)
{
	n = mod(floor(n), 10.0);
	if(n == 0.) return sprite(31599., p);
	else if(n == 1.) return sprite( 9362., p);
	else if(n == 2.) return sprite(29671., p);
	else if(n == 3.) return sprite(29391., p);
	else if(n == 4.) return sprite(23497., p);
	else if(n == 5.) return sprite(31183., p);
	else if(n == 6.) return sprite(31215., p);
	else if(n == 7.) return sprite(29257., p);
	else if(n == 8.) return sprite(31727., p);
	else if(n == 9.) return sprite(31695., p);
	else return 0.0;
}


//prints a number n
float print_number(float number, vec2 position)
{	
	float result	= 0.;
	result 		+= number < 0. ? sprite(24., position + vec2(4., 0.)) : 0.;		
	for(int i = 8; i >= 0; i--)
	{
		float place = pow(10., float(i));
		if(number >= place || float(i) < 1.)
		{
			result	 	+= digit(abs(number/place), position);
			position.x 	-= 4.;
		}
	}
	return result;
}


//prints all ulam numbers over the grid
float print_ulam_numbers(float n, float scale, vec2 print_coordinate)
{
	print_coordinate.x	= mod(print_coordinate.x, scale);
	print_coordinate.y	= mod(print_coordinate.y, scale);
	
	return print_number(n, print_coordinate);	
}


//prints all ulam numbers over the grid
float print_ulam_inverse(vec2 i, float scale, vec2 print_coordinate)
{
	print_coordinate.x	= mod(print_coordinate.x, scale);
	print_coordinate.y	= mod(print_coordinate.y, scale);
	
	return max(print_number(i.x, print_coordinate), print_number(i.y, print_coordinate - vec2(8., 0.)));	
}

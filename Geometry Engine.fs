/*{
	"DESCRIPTION": "Your shader description",
	"CREDIT": "by you",
	"CATEGORIES": [
		"Your category"
	],
	"INPUTS": [
	{
			"LABEL": "DotSize",
			"NAME": "DotSize",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"LABEL": "LineWidth",
			"NAME": "LineWidth",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": 0.0,
			"MAX": 1.0
		}
	]
}*/

vec3 iResolution = vec3(RENDERSIZE, 1.);
float iGlobalTime = TIME;
float twoPi = 6.28318531;



// Based on "Time Coordinates" by burito (Daniel Burke): https://www.shadertoy.com/view/Xd2XWR
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// Special thanks IÃ±igo Quilez!


// THIS SECTION DEFINES THE GEOMETRIC PRIMITIVE FUNCTIONS --------------------------------------

// NOTE: When I ported this I took the liberty of adding versions of the function calls that
// take POLAR COORDINATES as inputs. It should make some effects much easier to define, as well
// as adding a barbell function that draws a line with endcaps.

// Generalized rotation formula ----------------------------------------------------------------
	vec2 rot(vec2 p, float a) // 
	{
    	float c = cos(a);
    	float s = sin(a);
    	return vec2(p.x*c + p.y*s,
	             -p.x*s + p.y*c);
	}

// Draw circles based on CARTESIAN coordinates -------------------------------------------------
	float circle(vec2 pos, float radius)  
	{
    	return clamp(((.99+LineWidth/100.-abs(length(pos)-radius))-0.99)*500., 0.0, 1.0);
	}
	
// Draw circles based on POLAR coordinates -----------------------------------------------------
	float circleRad(in vec2 p, in float ang, in float radius, in float radius2)  
	{
		ang = ang * twoPi;
		vec2 pos = vec2 (radius * sin (ang), radius * cos(ang)) + p;
		
    	return clamp(((.99+LineWidth/100.-abs(length(pos)-radius2))-0.99)*500., 0.0, 1.0);
	}

// Draw filled circlesbased on CARTESIAN coordinates -------------------------------------------
	float circleFill(vec2 pos, float radius) 
	{
    	return clamp(((.99-(length(pos)-radius))-0.99)*500.0, 0.0, 1.0); // Mutiplication factor term affects sharpness of blend. Higher values are more distinct (default 500)  
	}
	
// Draw filled circles based on POLAR coordinates ----------------------------------------------
	float circleFillRad(in vec2 p, in float ang, in float radius, in float radius2)  
	{
		ang = ang * twoPi;
		vec2 pos = vec2 (radius * sin (ang), radius * cos(ang)) + p;
		
    	return clamp(((.99-(length(pos)-radius2))-0.99)*500., 0.0, 1.0); // Mutiplication factor term affects sharpness of blend. Higher values are more distinct (default 500)  
	}	

// Draw lines based on CARTESIAN coordinates -------------------------------------------------
	float line( in vec2 p, in vec2 a, in vec2 b ) 
	{
    	vec2 pa = -p - a;
    	vec2 ba = b - a;
    	float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    	float d = length( pa - ba*h );
    
    	return clamp(((.99+LineWidth/100. - d)-0.99)*500., 0.0, 1.0); 
	}
	
// Draw lines based on POLAR angle and radius coordinates -----------------------------------
	float lineRad( in vec2 p, in float ang1, in float radius1, in float ang2, in float radius2) 
	{
		ang1 = ang1 * twoPi;
		ang2 = ang2 * twoPi;
		
		vec2 a = vec2 (radius1 * sin (ang1), radius1 * cos(ang1));
		vec2 b = vec2 (radius2 * sin (ang2), radius2 * cos(ang2));
		
    	vec2 pa = -p - a;
    	vec2 ba = b - a;
    	float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    	float d = length( pa - ba*h );
    
    	return clamp(((.99+LineWidth/100. - d)-0.99)*500., 0.0, 1.0); 
	}
	
// barbell - Draw lines with endpoint disks based on CARTESIAN coordinates ------------------

	float barbell (in vec2 p, in vec2 a, in vec2 b, in float size ) 
	{
    	vec2 pa = -p - a;
    	vec2 ba = b - a;
    	float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    	float d = length( pa - ba*h );
    
    	float result = clamp(((.99+LineWidth/100. - d)-0.99)*500., 0.0, 1.0);
    	
    	result += circleFill(p+a, size);
    	result += circleFill(p+b, size);
    	
    	return result;
	}
	
// barbellRad - POLAR - Draw lines with endpoint disks based on angle and radius.--------------------

	float barbellRad (in vec2 p, in float ang1, in float radius1, in float ang2, in float radius2, in float size) 
	{
		ang1 = ang1 * twoPi;
		ang2 = ang2 * twoPi;
		
		vec2 a = vec2 (radius1 * sin (ang1), radius1 * cos(ang1));
		vec2 b = vec2 (radius2 * sin (ang2), radius2 * cos(ang2));
		
    	vec2 pa = -p - a;
    	vec2 ba = b - a;
    	
    	float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    	float d = length( pa - ba*h );
    
    	float result = clamp(((.99+LineWidth/100. - d)-0.99)*500., 0.0, 1.0);
    	
    	result += circleFill(p+a, size);
    	result += circleFill(p+b, size);
    	
    	return result;
	}

// END DEFINITION OF PRIMITIVE FUNCTIONS -----------------------------



void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	vec2 uv = fragCoord.xy / iResolution.xy;
    vec2 p = 1.0 - 2.0 * uv; // Coordinate system is centered on screen, Cartesian style. Up/Right is positive, Down/Left is negative. Origin = p
    p.x *= iResolution.x / iResolution.y; // Aspect correction
    
    
    // p = rot(p,sin(TIME)); // This uses the rot transform to rotate the origin 'p'. All function calls currently reference 'p'.
 
    vec3 colour = vec3(0.); // Initialize colour variable
    vec3 white = vec3(1.,1.,1.); // Initialize geometry colour
    float DotSize = DotSize/50.; // Scaling factor determines range of Dot Sizes (Default 50.)
    
// PLACE ALL GEOMETRY CALLS HERE -----------------------
    
    float c = circle(p, 0.2); // First geometry call sets up the variable that all other calls are written to as additions
    
    c += circle(p, 0.81);
    c += circle(p, 0.18);
    c += circleFill(p, 0.0305);
    c += circleFill(p+rot(vec2(-.5,-.5),sin(TIME)/4.), 0.001);
    c += line(p, vec2(0.000, -0.5), vec2(0.000, 0.5));
    c += circleFill(p+vec2(0.000, -0.5), DotSize);
    c += circleFill(p+vec2(0.000, 0.5), DotSize);
    
    c += lineRad (p, .125, .5, .35, .0);
    c += circleRad (p, .125, .5, .05);
    c += circleFillRad (p, .125, .5, DotSize);
    c += barbell(p, vec2(-.5,.5), vec2(.5,.5), DotSize);

	// This section shows an example of how to handle animations -----------------

	for ( int i=0; i < 10; i++ )
	{
		c += barbell(p, rot(vec2(-.5,-.5),sin(iGlobalTime+float(i))/4.), vec2(.0,.0), DotSize);
	}
		
	for ( int i=0; i < 20; i++ )
	{
		float fi = float(i);
		c += barbellRad(p, .225+TIME/10.+fi/20., .25+sin(TIME), .225+TIME/20.+fi/20., .5, DotSize);
		c += circleRad (p, .225+TIME/10.+fi/20., .25+sin(TIME), sin(fi+TIME)/10.);
	}
	
 // THIS IS THE END OF GEOMETRY CALLS ------------------
    
    c = clamp(c, 0.0, 1.0); // Final calculation. Modifying "c" term can make transparent layering.
    
    colour = white * c; // Colours all geometry with pre-defined "white" colour. White can be anything.
    
    fragColor = vec4(colour, 1.0);
}

void main(void) {
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
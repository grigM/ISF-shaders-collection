/*{
	"CREDIT": "by joshpbatty",
	"DESCRIPTION": "Port from Shanes https://www.shadertoy.com/view/4lBSzW",
	"CATEGORIES": [
		"XXX"
	],
	  "INPUTS": [
    {
      "NAME": "inputImage",
      "TYPE": "image"
    },
   	{
		"NAME": "hue_shift",
		"TYPE": "float",
		"DEFAULT": 0.5,
		"MIN": 0.0,
		"MAX": 1.0
	},
   	{
		"NAME": "size",
		"TYPE": "float",
		"DEFAULT": 5.0,
		"MIN": 1.0,
		"MAX": 20.0
	},
 	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 2.0,
		"MIN": 1.0,
		"MAX": 5.0
	},
	{
		"NAME": "vignette",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 1.0
	},
	{
		"NAME": "saturation",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 2.0
	},
	{
		"NAME": "mirrorX",
		"TYPE": "bool",
		"DEFAULT": 1.0
	},
	{
		"NAME": "mirrorY",
		"TYPE": "bool",
		"DEFAULT": 1.0
	}
  ]
}*/

#define PI 3.14159265359

//---------------------------------------------------------
// Cheers to macbooktail for the hue code https://www.shadertoy.com/view/MlSXWd
vec3 hue(vec3 color, float shift, float chroma_amp) {

    const vec3  kRGBToYPrime = vec3 (0.299, 0.587, 0.114);
    const vec3  kRGBToI     = vec3 (0.596, -0.275, -0.321);
    const vec3  kRGBToQ     = vec3 (0.212, -0.523, 0.311);

    const vec3  kYIQToR   = vec3 (1.0, 0.956, 0.621);
    const vec3  kYIQToG   = vec3 (1.0, -0.272, -0.647);
    const vec3  kYIQToB   = vec3 (1.0, -1.107, 1.704);

    // Convert to YIQ
    float   YPrime  = dot (color, kRGBToYPrime);
    float   I      = dot (color, kRGBToI);
    float   Q      = dot (color, kRGBToQ);

    // Calculate the hue and chroma
    float   hue     = atan (Q, I);
    float   chroma  = sqrt (I * I + Q * Q) * chroma_amp;

    // Make the user's adjustments
    hue += shift;

    // Convert back to YIQ
    Q = chroma * sin (hue);
    I = chroma * cos (hue);

    // Convert back to RGB
    vec3    yIQ   = vec3 (YPrime, I, Q);
    color.r = dot (yIQ, kYIQToR);
    color.g = dot (yIQ, kYIQToG);
    color.b = dot (yIQ, kYIQToB);

    return color;
}

// Cheers the pallette code IQ ;) 
//--------------------------------------------------------
vec3 pal( in float t, in vec3 a, in vec3 b, in vec3 c, in vec3 d )
{
    return a + b*cos( 6.28318*(c*t+d) );
}

// Perform our colour grading 
//----------------------------------------------------------
vec4 lut(float pos){
    float hueAjust = hue_shift * (PI*2.0);
    float chromaAjust = .1920;
	return vec4(pal( pos, vec3(0.5,0.5,0.5),vec3(0.5,0.5,0.5),vec3(0.5,0.5,0.5),hue(vec3(0.0,1.0,1.0),hueAjust,chromaAjust) ),1.0);
}

// Standard 2x2 hash algorithm.
vec2 hash22(vec2 p) { 

    // Faster, but probably doesn't disperse things as nicely as other ways.
    float n = sin(dot(p,vec2(41, 289))); 
    p = fract(vec2(8.0*n, n)*262144.);
    return sin(p*6.2831853 + TIME*speed);
}

float Voronoi3Tap(vec2 p){
	// Simplex grid stuff.
    //
    vec2 s = floor(p + (p.x+p.y)*0.3660254); // Skew the current point.
    p -= s - (s.x+s.y)*0.2113249; // Use it to attain the vector to the base vertice (from p).

    // Determine which triangle we're in. Much easier to visualize than the 3D version.
    float i = step(0.0, p.x-p.y); 
    
    // Vectors to the other two triangle vertices.
    vec2 p1 = p - vec2(i, 1.0-i) + 0.2113249, p2 = p - 0.5773502; 

    // Add some random gradient offsets to the three vectors above.
    p += hash22(s)*0.125;
    p1 += hash22(s +  vec2(i, 1.0-i))*0.125;
    p2 += hash22(s + 1.0)*0.125;
    
    // Determine the minimum Euclidean distance. You could try other distance metrics, 
    // if you wanted.
    float d = min(min(dot(p, p), dot(p1, p1)), dot(p2, p2))/0.425;
   
    // That's all there is to it.
    return sqrt(d); // Take the square root, if you want, but it's not mandatory.
}

void main() {

	//-------------------------------------------------------------------------------- START VORONOI
	// Screen coordinates.
	//vec2 uv = (fragCoord.xy - iResolution.xy*0.5)/ iResolution.y;
   	vec2 uv =  isf_FragNormCoord.xy; // fragCoord.xy / iResolution.xy;
    uv = uv*2.-1.;
    
    if(mirrorX) uv.x = abs(uv.x);
    if(mirrorY) uv.y = abs(uv.y);
   // uv =abs(uv);
    
    // Take two 3-tap Voronoi samples near one another.
    float c = Voronoi3Tap(uv*size);
    float c2 = Voronoi3Tap(uv*size - 10./RENDERSIZE.y);
   
    // Coloring the cell.
    //
    // Use the Voronoi value, "c," above to produce a couple of different colors.
    // Mix those colors according to some kind of moving geometric patten.
    // Setting "pattern" to zero or one displays just one of the colors.
    float pattern =  cos(uv.x*0.75*3.14159-0.9)*cos(uv.y*1.5*3.14159-0.75)*(0.5+saturation) * 0.5;
     
    // Just to confuse things a little more, two different color schemes are faded in out.
    //
    // Color scheme one - Mixing a firey red with some bio green in a sinusoidal kind of pattern.
    //vec3 v_color = mix(vec3(c*1.3, pow(c, 2.), pow(c, 10.)), vec3(c*c*0.8, c, c*c*0.35), pattern );
    vec3 v_color = mix(vec3 (c*lut(0.0).r, c*pow(lut(0.0).g, 2.), c*pow(lut(0.0).b, 10.0)), 
    					vec3(c*lut(PI).r, c*pow(lut(PI).g, 2.), c*c*pow(lut(PI).b, 10.0)), pattern);
    // Color scheme two - Farbrausch fr-025 neon, for that disco feel. :)
    vec3 v_color2 = mix(vec3(c*1.2, pow(c, 8.), pow(c, 2.)), vec3(c*1.3, pow(c, 2.), pow(c, 10.)), pattern );
    
    // Alternating between the two color schemes.
//    v_color = mix(v_color, v_color2, smoothstep(.4, .6, sin(TIME*.25)*.5 + .5)); // 
    v_color = mix(v_color, v_color2, 0.0); // 
	
	//v_color *= v_color2;
    //col = mix(col.zxy, col, cos(uv.x*2.*3.14159)*cos(uv.y*5.*3.141595)*.25 + .75 );
    
    // Hilighting.
    //
    // Use a combination of the sample difference "c2-c" to add some really cheap, blueish highlighting.
    // It's a directional-derviative based lighting trick. Interesting, but no substitute for point-lit
    // bump mapping. Comment the following line out to see the regular, flat pattern.
    v_color += vec3(0.5, 0.8, 1.)*(c2*c2*c2-c*c*c)*5.;
       
    // Speckles.
    //
    // Adding subtle speckling to break things up and give it a less plastic feel.
    v_color += (length(hash22(uv + TIME))*0.06 - 0.03)*vec3(1., 0.5, 0.);
    

    // Vignette.
    //
    //v_color *= (1.15 - dot(uv, uv)*0.5);//*vec3(1., 0.97, 0.92); // Roundish.
    vec2 p = uv*vec2(RENDERSIZE.y/(RENDERSIZE.x*1.2), 0.5)+0.5; // Rectangular.
    v_color *= smoothstep(0., 0.5, pow( 16.*p.x*p.y*(1.0-p.x)*(1.0-p.y), 0.25 + (vignette * 0.7)))*vec3(1.1, 1.07, 1.01);
    
    
    // Even more color schemes.
    //col = col.xzy; // col.yzx, col.zyx, etc.
    
    // Approximate gamma correction.
	gl_FragColor = vec4(sqrt(clamp(v_color, 0., 1.)), 1);
	
}
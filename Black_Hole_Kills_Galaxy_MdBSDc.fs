/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel1",
      "PATH" : "tex09.jpg"
    },
    {
      "NAME" : "iChannel0",
      "PATH" : "tex16.png"
    }
  ],
  "CATEGORIES" : [
    "galaxy",
    "stars",
    "blackhole",
    "cosmic",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MdBSDc by rakesh111989.  Made based on this information below: Some Black Holes Can Kill Entire Galaxies\nhttp:\/\/www.space.com\/8336-black-holes-kill-entire-galaxies.html",
  "INPUTS" : [

  ]
}
*/


//Changed Galaxy3 by  FabriceNeyret2
//---  Galaxy --- Fabrice NEYRET  august 2013

const float RETICULATION = 3.;  // strenght of dust texture
const float NB_ARMS = 5.;       // number of arms
//const float ARM = 3.;         // contrast in/out arms
const float COMPR = .1;         // compression in arms
const float SPEED = .1;
const float GALAXY_R = 1./2.;
const float BULB_R = 1./2.5;
const vec3 GALAXY_COL = vec3(.9,.9,1.); //(1.,.8,.5);
const vec3 BULB_COL   = vec3(1.,1.0,1.0);
const float BULB_BLACK_R = 1./4.;
const vec3 BULB_BLACK_COL   = vec3(0,0,0);
const vec3 SKY_COL    = .5*vec3(.1,.3,.5);
		
#define Pi 3.1415927
	float t = TIME;

// --- base noise
float tex(vec2 uv) 
{
	float n = IMG_NORM_PIXEL(iChannel0,mod(uv,1.0)).r;
	
#define MODE 3  // kind of noise texture
#if MODE==0         // unsigned
	#define A 2.
	return n;
#elif MODE==1       // signed
	#define A 3.
	return 2.*n-1.;
#elif MODE==2       // bulbs
	#define A 3.
	return abs(2.*n-1.);
#elif MODE==3       // wires
	#define A 1.5
	return 1.-abs(2.*n-1.);
#endif
}


// --- perlin turbulent noise + rotation
float noise(vec2 uv)
{
	float v=0.;
	float a=-SPEED*t,	co=cos(a),si=sin(a); 
	mat2 M = mat2(co,-si,si,co);
	const int L = 7;
	float s=1.;
	for (int i=0; i<L; i++)
	{
		uv = M*uv;
		float b = tex(uv*s);
		v += 1./s* pow(b,RETICULATION); 
		s *= 2.;
	}
	
    return v/2.;
}

bool keyToggle(int ascii) 
{
	return false;//(texture2D(iChannel2,vec2((.5+float(ascii))/256.,0.75)).x > 0.);
}

void main()
{
	vec2 uv = gl_FragCoord.xy/RENDERSIZE.y-vec2(.8,.5);
	vec3 col;
	
	// spiral stretching with distance
	float rho = length(uv); // polar coords
	float ang = atan(uv.y,uv.x);
	float shear = 2.*log(rho); // logarythmic spiral
	float c = cos(shear), s=sin(shear);
	mat2 R = mat2(c,-s,s,c);

	// galaxy profile
	float r; // disk
	r = rho/GALAXY_R; float dens = exp(-r*r);
	r = rho/BULB_R;	  float bulb = exp(-r*r);
	r = rho/BULB_BLACK_R; float bulb_black = exp(-r*r);
	float phase = NB_ARMS*(ang-shear);
	// arms = spirals compression
	ang = ang-COMPR*cos(phase)+SPEED*t;
	uv = rho*vec2(cos(ang),sin(ang));
	// stretched texture must be darken by d(new_ang)/d(ang)
	float spires = 1.+NB_ARMS*COMPR*sin(phase);
	// pires = mix(1.,sin(phase),ARM);
	dens *= .7*spires;	
	
	// gaz texture
	float gaz = noise(.09*1.2*R*uv);
	float gaz_trsp = pow((1.-gaz*dens),2.);

	// stars
	//float a=SPEED*t, co=cos(a),si=sin(a); 
	//mat2 M = mat2(co,-si,si,co);
	// adapt stars size to display resolution
	float ratio = .8*RENDERSIZE.y/_iChannel0_imgSize.y;
	float stars1 = IMG_NORM_PIXEL(iChannel1,mod(ratio*uv+.5,1.0)).r, // M*uv
	      stars2 = IMG_NORM_PIXEL(iChannel0,mod(ratio*uv+.5,1.0)).r,
		  stars = pow(1.-(1.-stars1)*(1.-stars2),5.);
	
	//stars = pow(stars,5.);
	
	// keybord controls (numbers)
	if (keyToggle(49)) gaz_trsp = 1./1.7;
	if (keyToggle(50)) stars = 0.;
	if (keyToggle(51)) bulb = 0.;
	if (keyToggle(52)) dens = .3*spires;
	
	// mix all	
	col = mix(SKY_COL,
			  gaz_trsp*(1.7*GALAXY_COL) + 1.2*stars, 
			  dens);
	col = mix(col, 2.*BULB_COL,1.2* bulb);

	col = mix(col, 1.2*BULB_BLACK_COL, 2.0*bulb_black);

		
	gl_FragColor = vec4(col,1.);
}
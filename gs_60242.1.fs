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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60242.1"
}
*/


// Atmospheric scattering model
// Code adapted from Martins
// http://blenderartists.org/forum/showthread.php?242940-unlimited-planar-reflections-amp-refraction-%28update%29
// Martijn Steinrucken countfrolic@gmail.com
// My name is Will here, making volumetric clouds :) with Martins sky implementation
// William Germany germany.william@gmail.com

#ifdef GL_ES
precision mediump float;
#endif


vec3 wPos;
vec3 sunPos = vec3(1.,10.,0.);

uniform vec3 cameraPos;

vec3 sunDirection = normalize(sunPos);

float turbidity = 1.0;
float rayleighCoefficient = 1.5;

const float mieCoefficient = 0.50;
const float mieDirectionalG = 0.70;


// constants for atmospheric scattering
const float e  = 2.718281828459045235360287471352662497757247;
const float pi = 3.141592653589793238462643383279502884197169;

const float n = 1.0003; // refractive index of air
const float N = 2.545E25; // number of molecules per unit volume for air at
						// 288.15K and 1013mb (sea level -45 celsius)

// wavelength of used primaries, according to preetham
const vec3 primaryWavelengths = vec3(680E-9, 550E-9, 450E-9);

// mie stuff
// K coefficient for the primaries
const vec3 K = vec3(0.686, 0.78, 0.666);
const float v = 4.0;

// optical length at zenith for molecules
const float rayleighZenithLength = 8.4E3;
const float mieZenithLength = 1.25E3;
const vec3 up = vec3(0.0, 0.0, 1.0);

const float sunIntensity = 575.0;
const float sunAngularDiameterCos = 0.9998993194915; // 66 arc seconds -> degrees, and the cosine of that

// earth shadow hack
const float cutoffAngle = pi/1.95;
const float steepness = 1.5;

struct Camera											// A camera struct that holds all relevant camera parameters
{
	vec3 position;
	vec3 lookAt;
	vec3 rayDir;
	vec3 forward, up, left;
};
	
struct Clouds
{
	vec3 Skylighting;
	vec3 Sunlighting;
	vec3 Groundlighting;
	vec3 Inscattering;
	vec3 Density;
	vec3 Wind;
};
	
//Sky funtions

vec3 TotalRayleigh(vec3 primaryWavelengths)
{
	vec3 rayleigh = (8.0 * pow(pi, 3.0) * pow(pow(n, 2.0) - 1.0, 2.0)) / (3.0 * N * pow(primaryWavelengths, vec3(4.0)));   // The rayleigh scattering coefficient
 
    return rayleigh; 

    //  8PI^3 * (n^2 - 1)^2 * (6 + 3pn)     8PI^3 * (n^2 - 1)^2
    // --------------------------------- = --------------------  
    //    3N * Lambda^4 * (6 - 7pn)          3N * Lambda^4         
}

float RayleighPhase(float cosViewSunAngle)
{	 
	return (3.0 / (1.0*pi)) * (1.0 + pow(cosViewSunAngle, 2.0));
}

vec3 totalMie(vec3 primaryWavelengths, vec3 K, float T)
{
	float c = (0.2 * T ) * 10E-18;
	return 0.434 * c * pi * pow((2.0 * pi) / primaryWavelengths, vec3(v - 2.0)) * K;
}

float SchlickPhase(float cosViewSunAngle, float g)
{
	float k = (1.55 * g) - (5.55 * (g * g * g));
	return (1.0 / (4.0 * pi)) * ((1.0 - (k * k)) / ( pow( 1.0 + k * cosViewSunAngle, 2.0)));
}

float SunIntensity(float zenithAngleCos)
{
	return sunIntensity * max(0.0, 1.0 - exp(-((cutoffAngle - acos(zenithAngleCos))/steepness)));
}


//Sun Ray funtion

float rand(int seed, float ray) 
{
	return mod(sin(float(seed)*363.5346+ray*674.2454)*6743.4365, 1.0);
}

vec3 SunRay(in vec2 pos, in vec2 RENDERSIZE)
{	
	float pi = 3.14159265359;
	vec2 position = pos;
	position.y *= RENDERSIZE.y/RENDERSIZE.x;
	position.y += 0.33;
	float ang = atan(position.x, position.y);
	float dist = length(position);
	vec3 col = vec3(1.7, 1.5, 1.0) * (pow(dist, -1.0) * 0.006);
	for (float ray = 0.5; ray < 10.0; ray += 0.097) 
	{
		float rayang = rand(5, ray)*6.2+(TIME*0.02)*20.0*(rand(2546, ray)-rand(5785, ray))-(rand(3545, ray)-rand(5467, ray));
		rayang = mod(rayang, pi*2.0);
		if (rayang < ang - pi) {rayang += pi*2.0;}
		if (rayang > ang + pi) {rayang -= pi*2.0;}
		float brite = 0.3 - abs(ang - rayang);
		brite -= dist * 0.5;
		
		if (brite > 0.0) 
		{
			col += vec3(0.1+1.7*rand(8644, ray), 0.55+1.3*rand(4567, ray), 0.7+0.5*rand(7354, ray)) * brite * 0.025;
		}
	}
	
	return col.rgb;
}

//Cloud funtions

float hash( float n )
{
    return fract(sin(n)*4548.5455);
}


float FractionalBrownianMotion( in vec3 x )
{
    vec3 p = floor(x);
    vec3 f = fract(x);

    f = f*f*(3.0-2.0*f);
    float n = p.x + p.y*57.0 + 113.0*p.z;
    return mix(mix(mix( hash(n+  0.0), hash(n+  1.0),f.x),
                   mix( hash(n+ 57.0), hash(n+ 58.0),f.x),f.y),
               mix(mix( hash(n+113.0), hash(n+114.0),f.x),
                   mix( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
}

vec4 CalculateDensity( in vec3 p, in vec3 diffuse, in vec3 ambient )
{
	//altitude
	float d = 4.0 - p.y;
	//wind
	Clouds Cloud;
	Cloud.Wind =  vec3(-0.2,0.1,0.1) * (TIME * 0.2);
	vec3 q = p - Cloud.Wind;
	
	float f;
    	f  = 0.5000*FractionalBrownianMotion( q ); q = q*2.02;
    	f += 0.2500*FractionalBrownianMotion( q ); q = q*2.03;
    	f += 0.1250*FractionalBrownianMotion( q ); q = q*2.01;
    	f += 0.0625*FractionalBrownianMotion( q );
	
	//0-1
	float cloudPuffiness = 0.7;
	float cloudWispyness = 0.0;
	//f -= 0.12 * d * 0.01;
	f -= 0.09 * FractionalBrownianMotion( q ) * 2.5;
	
	
	f = clamp(f - (1.0 - cloudPuffiness), 0.0, 1.0 - cloudWispyness) / (1.0 - cloudWispyness);
	
	//density
	d += 0.0 * f;

	d = clamp( d, 0.0, 1.0 );
	
	vec4 res = vec4( d );

	// diffuse is here actually
	res.xyz = mix( diffuse * 8.0, vec3(0.5), res.x );
	res.a -= f * 1.0;
	return res;
}

vec4 CalculateClouds( in vec3 ro, in vec3 rd, in vec3 color, in float x )
{
	vec4 sum = vec4(0.0, 0.0, 0.0, 0);
	float cloudCover = 0.15;
	ro.x *= -1.0;
	
	float t = 0.0;
	for(int i=0; i<64; i++)
	{
		if( sum.a > 0.99 ) continue;

		vec3 pos = ro + t * vec3(rd.xz * (0.1 / rd.y), 2.0);
		
		
		//Calculate DirectLighting
		vec3 diffuse = vec3(2.0,1.55,1.0) * 0.5 + 0.3;
		float sunPhase = (SchlickPhase(x - 0.055, mieDirectionalG) * 0.35);
		//Calculate Ambient
		vec3 ambient = exp2(1.0 - sunPhase) * vec3(1.5,1.97,2.38) * 0.1 + 0.8;
		//Calculate GroundLighting
		
		vec4 col = CalculateDensity( pos, diffuse, ambient );
		
		
		#if 1
		float constrast = 1.0;
		col.xyz *= diffuse + mix(ambient, color, clamp((1.0 - sunPhase), 0.0, 1.0 ) * 0.01);
		#endif
		col.a = max(col.a - (1.0 - cloudCover), 0.0);
		col.a *= 0.25;
		//col.rgb *= (sunPhase * diffuse);
		col.rgb += (ambient) * 0.2;
		col.rgb *= col.a;
		col.xyz *= pow(col.xyz, vec3(constrast));
		sum = sum + col*(0.8 - sum.a);
		
        	#if 0
		t += 0.1;
		#else
		t += max(0.1,0.04*t);
		#endif
	}

	sum.xyz /= (0.001+sum.w);

	return clamp( sum, 0.0, 1.0 );
}

float A = 0.15;
float B = 0.50;
float C = 0.10;
float D = 0.20;
float E = 0.02;
float F = 0.30;
float W = 1000.0;

vec3 Uncharted2Tonemap(vec3 x)
{
   return ((x*(A*x+C*B)+D*E)/(x*(A*x+B)+D*F))-E/F;
}

vec3 ToneMap(vec3 color) {
    vec3 toneMappedColor;
    
    toneMappedColor = color * 0.04;
    toneMappedColor = Uncharted2Tonemap(toneMappedColor);
    
    float sunfade = 1.0-clamp(1.0-exp(-(sunPos.z/500.0)),0.0,1.0);
    toneMappedColor = pow(toneMappedColor,vec3(1.0/(1.2+(1.2*sunfade))));
    
    return toneMappedColor;
}

void main() 
{ 
    sunDirection = normalize(vec3(-(mouse.x-0.5)*2., -1, (mouse.y-0.5)*2.));
	
    // General parameter setup
	vec2 vPos = 2.0*gl_FragCoord.xy/RENDERSIZE.xy - 1.0; 					// map vPos to -1..1
	float t = TIME*0.0;									// TIME value, used to animate stuff
	float screenAspectRatio = RENDERSIZE.x/RENDERSIZE.y;					// the aspect ratio of the screen (e.g. 16:9)
	vec3 finalColor = vec3(0.1);								// The background color, dark gray in this case
	
   //Camera setup
	Camera cam;										// Make a struct holding all camera parameters
  	cam.lookAt = vec3(0,0,3);								// The point the camera is looking at
	cam.position = vec3(0, 5, 0);						// The position of the camera
	cam.up = vec3(0,0,1);									// The up vector, change to make the camera roll, in world space
  	cam.forward = normalize(cam.lookAt-cam.position);					// The camera forward vector, pointing directly at the lookat point
  	cam.left = cross(cam.forward, cam.up);							// The left vector, which is perpendicular to both forward and up
 	cam.up = cross(cam.left, cam.forward);							// The recalculated up vector, in camera space
 
	vec3 screenOrigin = (cam.position+cam.forward); 					// Position in 3d space of the center of the screen
	vec3 screenHit = screenOrigin + vPos.x*cam.left*screenAspectRatio + vPos.y*cam.up; 	// Position in 3d space where the camera ray intersects the screen
  
	cam.rayDir = normalize(screenHit-cam.position);						// The direction of the current camera ray

	
	vec2 q = gl_FragCoord.xy / RENDERSIZE.xy;
	vec2 p = -1.0 + 2.0*q;
	p.x *= RENDERSIZE.x/ RENDERSIZE.y;
	vec2 mo = -1.0 + 2.0 / RENDERSIZE.xy;
	
	    // camera
    	vec3 ro = 4.0*normalize(vec3(cos(2.75-3.0*mo.x), 0.7+(mo.y+2.2+0.3*sin(0.2)), sin(2.75-3.0*mo.x)));
	vec3 ta = vec3(1.5, -10.0, 2.0);
    	vec3 ww = normalize( ta - ro);
    	vec3 uu = normalize(cross( vec3(0.0,1.0,0.0), ww ));
    	vec3 vv = normalize(cross(ww,uu));
    	vec3 rd = normalize( p.x*uu*1.7 + p.y*vv*1.4 + 1.0*ww );
	
	
	sunPos = vec3(0, -1, abs(sin(TIME*0.0))*0.9+0.1);
	sunDirection = normalize(sunPos);//vec3(-(mouse.x-0.5)*2.*screenAspectRatio, -1, (mouse.y-0.5)*2.));	
    vec3 viewDir = cam.rayDir;//normalize(wPos - cameraPos);
	
	//Sun rays
	vec2 po = ( (gl_FragCoord.xy / RENDERSIZE.xy) * 2.0 - 1.0 ) - sunPos.xz;
	vec3 sunRay = SunRay(po, RENDERSIZE);
	
	
    // Cos Angles
    float cosViewSunAngle = dot(viewDir, sunDirection);
    float cosSunUpAngle = dot(sunDirection, up);
    float cosUpViewAngle = dot(up, viewDir);
    
    float sunE = SunIntensity(cosSunUpAngle);  // Get sun intensity based on how high in the sky it is

	// extinction (absorbtion + out scattering)
	// rayleigh coefficients
//	vec3 rayleighAtX = TotalRayleigh(primaryWavelengths) * rayleighCoefficient;
    vec3 rayleighAtX = vec3(5.176821E-6, 1.2785348E-5, 2.8530756E-5) * rayleighCoefficient;
    
	// mie coefficients
	vec3 mieAtX = totalMie(primaryWavelengths, K, turbidity) * mieCoefficient;  
    
	// optical length
	// cutoff angle at 90 to avoid singularity in next formula.
	float zenithAngle = max(0.0, cosUpViewAngle);
    
	float rayleighOpticalLength = rayleighZenithLength / zenithAngle;
	float mieOpticalLength = mieZenithLength / zenithAngle;


	// combined extinction factor	
	vec3 Fex = exp(-(rayleighAtX * rayleighOpticalLength + mieAtX * mieOpticalLength));

	// in scattering
	vec3 rayleighXtoEye = rayleighAtX * RayleighPhase(cosViewSunAngle);
	vec3 mieXtoEye = mieAtX *  SchlickPhase(cosViewSunAngle, mieDirectionalG);
     
    vec3 totalLightAtX = rayleighAtX + mieAtX;
    vec3 lightFromXtoEye = rayleighXtoEye + mieXtoEye; 
    
    vec3 somethingElse = sunE * (lightFromXtoEye / totalLightAtX);
    
    vec3 sky = somethingElse * (1.0 - Fex);
    sky *= mix(vec3(1.0),pow(somethingElse * Fex,vec3(0.5)),clamp(pow(1.0-dot(up, sunDirection),5.0),0.0,1.0));

	//vec4 cloud = CalculateClouds(ro, rd, sky, cosViewSunAngle);
    
	// composition + solar disc

    float sundisk = 0.0;//smoothstep(sunAngularDiameterCos,sunAngularDiameterCos+0.00002,cosViewSunAngle);
    vec3 sun = (sunE * 19000.0 * Fex)*sundisk;
    vec3 final = ToneMap(sky+sun) * 1.0;
	//final = mix( final, (cloud.xyz), cloud.w );

	
    gl_FragColor.rgb = pow(final, vec3(1.5)) + sunRay;
    gl_FragColor.a = 1.0;
}
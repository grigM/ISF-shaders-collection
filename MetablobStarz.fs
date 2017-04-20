/*{
	"CREDIT": "by mojovideotech",
  "CATEGORIES" : [
    "generator"
  ],
  "DESCRIPTION" : "",
  "ISFVSN" : "2",
  "INPUTS" : [
        {
			"NAME": "radius1",
			"TYPE": "float",
			"DEFAULT": 60.0,
			"MIN": 10.0,
			"MAX": 100.0
		},
		{
			"NAME": "radius2",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.01,
			"MAX": 1.5
		},
		{
			"NAME": "rate",
			"TYPE": "float",
			"DEFAULT": 0.125,
			"MIN": 0.01,
			"MAX": 0.5
		},
		{
			"NAME": "numblobs",
			"TYPE": "float",
			"DEFAULT":36.0,
			"MIN": 1.0,
			"MAX": 54.0
		},
		{
			"NAME": "blend",
			"TYPE": "float",
			"DEFAULT": 3.5,
			"MIN": 0.5,
			"MAX": 5.0
		},
		{
			"NAME": "freq",
			"TYPE": "float",
			"DEFAULT": 1.5,
			"MIN": 0.1,
			"MAX": 3.0
		},
		{
			"NAME": "edge",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.125,
			"MAX": 1.0
		},
		{
			"NAME": "luminosity",
			"TYPE": "float",
			"DEFAULT": 0.15,
			"MIN": 0.001,
			"MAX": 0.75
		},
    	{
      		"NAME" : "light",
      		"TYPE" : "point2D",
      		"DEFAULT":[ 0.5, 0.1 ],
      		"MAX" : [ 1, 1 ],
      		"MIN" : [ 0, 0 ]
    	}
  ]
}
*/

////////////////////////////////////////////////////////////
// MetablobStarz  by mojovideotech
//
// mod of :
//
// metaballss by pailhead in 2014-Jun-2
// original:  https://www.shadertoy.com/view/4sXXWM
// small modifications & antialiasing by I.G.P. 2016-09-24
//
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0
////////////////////////////////////////////////////////////


#define COUNT 54

float rn(float x) { return  fract(sin(x*43349.4437)* 5142.29); }

void mainImage( out vec4 fragColor, in vec2 fragCoord ) {
	float ts = TIME * rate;   // TIME scaled	
	vec3 COLOR_MASKS[16];  // blob colors
	COLOR_MASKS[0] = vec3(0.30, 0.00, 0.90 );
  	COLOR_MASKS[1] = vec3(0.00, 0.00, 0.90 );
  	COLOR_MASKS[2] = vec3(0.90, 0.00, 0.15 );
  	COLOR_MASKS[3] = vec3(0.00, 0.90, 0.30 );
  	COLOR_MASKS[4] = vec3(0.60, 0.00, 0.30 );
	COLOR_MASKS[5] = vec3(0.90, 0.00, 0.60 );
  	COLOR_MASKS[6] = vec3(0.00, 0.15, 0.60 );
  	COLOR_MASKS[7] = vec3(0.30, 0.90, 0.00 );
  	COLOR_MASKS[8] = vec3(0.90, 0.00, 0.00 );
  	COLOR_MASKS[9] = vec3(0.30, 0.15, 0.90 );
  	COLOR_MASKS[10]= vec3(0.60, 0.90, 0.00 );
  	COLOR_MASKS[11]= vec3(0.15, 0.00, 0.90 );
  	COLOR_MASKS[12]= vec3(0.00, 0.30, 0.90 );
  	COLOR_MASKS[13]= vec3(0.60, 0.00, 0.90 );
  	COLOR_MASKS[14]= vec3(0.90, 0.60, 0.00 );
  	COLOR_MASKS[15]= vec3(0.60, 0.30, 0.00 );

	float aspect = RENDERSIZE.x / RENDERSIZE.y;
	vec2 fragPN = (fragCoord / RENDERSIZE.xy) * 2.0 - 1.0;
	fragPN.x *= aspect;
	float vH = 200.0;   // camera disance - fov
	vec3 vD = normalize(vec3(-fragPN, vH));   // view dir
	vec3 lD = normalize(vec3(cos(ts)+ 0.5, 1.50, 2.0 - sin(ts)));   // light dir
	vec2 mbPos[COUNT];  // blob position
	vec3 nn = vec3(0.0), cc = vec3(0.0);
	float number = floor(numblobs); 
	for(int i=0; i<COUNT; i++) {
		vec3 bC = COLOR_MASKS[i-(i/15)*15]; // blob color
		float rn1 = rn(float(i+47)), rn2 = rn(float(i-21)), rn3 = rn(float(i-52));	
		mbPos[i] = vec2(sin(rn1*3.14 + ts*rn2)*aspect, cos(rn2*5.14 + ts*rn3));  // calc position	
		mbPos[i] = fragPN - mbPos[i]*.8;
		float rr = cos(rn3*6.28 +ts * rn1)*.2 +.5;
		mbPos[i] *= rr * (1000. / radius1);   // blob coord
		float bL = mix(max(abs(mbPos[i].x), abs(mbPos[i].y)), length(mbPos[i]), sin(TIME*freq)-(1.125+radius2));     // blob length
		float bH = exp(-bL*(6.0-blend));
		vec3 bN = vec3(mbPos[i] * 0.3 * bH, bH - 0.01);
		bC *= bH;
		nn += vec3(mbPos[i]*0.5*bH, bH);
		cc += bC;
		number -= 1.0;
		if (number == 0.0) break;
	}
	vec3 n = normalize( vec3(nn.x, nn.y, nn.z-0.1) );
	float aB = smoothstep(0.0, 1.275 - edge, n.z);  // antialiasing edge 
	cc /= nn.z;
	float ndl = 0.3 + 0.7 * dot(n,lD);
	vec3 h = normalize(vD+lD);
	float ndh = 0.5 + 0.5 * dot(n,h);
	ndh = pow(ndh, 1.0 + 2000.0*(light.y))*(0.2 + 0.65*light.x);  // light reflection
	vec3 fc = cc*ndl + ndh;
	float frs = dot(n,vD);
	frs = 1.0 - clamp(frs, 0.0, 1.0);
	frs = pow(frs, 100.0);
	frs = frs*0.4 + luminosity;
	fc += frs;
	fragColor = vec4(fc*aB, 1.0);
}

void main( void ) {
	mainImage( gl_FragColor, gl_FragCoord.xy );
}
/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
      "NAME" : "speed1",
      "TYPE" : "float",
      "DEFAULT" : 1.0,
      "MIN" : 0.0,
      "MAX" : 8.0
    },
    {
      "NAME" : "speed2",
      "TYPE" : "float",
      "DEFAULT" : 1.0,
      "MIN" : 0.0,
      "MAX" : 8.0
    },
    {
      "NAME" : "sin_ofset_1",
      "TYPE" : "float",
      "DEFAULT" : 0.0,
      "MIN" : -2.0,
      "MAX" : 2.0
    },
    {
      "NAME" : "sin_ofset_2",
      "TYPE" : "float",
      "DEFAULT" : 0.0,
      "MIN" : -2.0,
      "MAX" : 2.0
    },
    
    {
      "NAME" : "hue_shift",
      "TYPE" : "float",
      "DEFAULT" : 4.2,
      "MIN" : 0.0,
      "MAX" : 8.0
    },
    {
      "NAME" : "count",
      "TYPE" : "float",
      "DEFAULT" : 7.0,
      "MIN" : 0.0,
      "MAX" : 16.0
    },
    
    {
      "NAME" : "xofset",
      "TYPE" : "float",
      "DEFAULT" : 0.0,
      "MIN" : -2.0,
      "MAX" : 2.0
    },
    {
      "NAME" : "yofset",
      "TYPE" : "float",
      "DEFAULT" : 0.0,
      "MIN" : -2.0,
      "MAX" : 2.0
    },
    	{
    
			"NAME": "zoom_level",
			"TYPE": "float",
			"MIN": 0.01,
			"MAX": 10.0,
			"DEFAULT": 1.0
		}
    
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#27226.2"
}
*/




/*
void main() {
	vec2		loc;
	vec2		modifiedCenter;
	
	loc = isf_FragNormCoord;
	modifiedCenter = center / RENDERSIZE;
	loc.x = (loc.x - modifiedCenter.x)*(1.0/level) + modifiedCenter.x;
	loc.y = (loc.y - modifiedCenter.y)*(1.0/level) + modifiedCenter.y;
	if ((loc.x < 0.0)||(loc.y < 0.0)||(loc.x > 1.0)||(loc.y > 1.0))	{
		gl_FragColor = vec4(0.0);
	}
	else	{
		gl_FragColor = IMG_NORM_PIXEL(inputImage,loc);
	}
}
*/

#ifdef GL_ES
precision mediump float;
#endif


float al_tri(vec2 p, vec2 p0, vec2 p1, vec2 p2) {
    float s = p0.y * p2.x - p0.x * p2.y + (p2.y - p0.y) * p.x + (p0.x - p2.x) * p.y;
    float t = p0.x * p1.y - p0.y * p1.x + (p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y;

    //if ((s < 0.) != (t < 0.)) {
    //return 0.;
    //}

    float A = -p1.y * p2.x + p0.y * (p2.x - p1.x) + p0.x * (p1.y - p2.y) + p1.x * p2.y;
    if (A < 0.0)
    {
        s = -s;
        t = -t;
        A = -A;
    }
	
    return float(s > 0. && t > 0. && (s + t) < A);
}

vec3 HueShift (in vec3 Color, in float Shift)
{
    vec3 P = vec3(0.55735)*dot(vec3(0.55735),Color);
    
    vec3 U = Color-P;
    
    vec3 V = cross(vec3(0.55735),U);    

    Color = U*cos(Shift*6.2832) + V*sin(Shift*6.2832) + P;
    
    return vec3(Color);
}


float tri(vec2 p, vec2 p0, vec2 p1, vec2 p2) {
    float ret = 0.;
    for (int i = 0; i < 16; i++) { 
    vec2 q = vec2(floor(mod(float(i), 4.0)) / 4.0, floor(float(i)) / 16.0);
    vec2 uv = p+(q/600.);
    ret += al_tri(uv,p0,p1,p2);
    }
    return ret/16.;
}




void main( void ) {
	#define PI 3.141592653589793238462
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy );
	
	vec2 suv = ((vec2(uv.x,((uv.y-0.5)*(RENDERSIZE.y/RENDERSIZE.x))+0.5)-.5)*2.)+.5;
	
	vec2		loc;
	vec2		modifiedCenter;
	loc = isf_FragNormCoord;
	modifiedCenter =  RENDERSIZE;
	suv.x = (suv.x - xofset)*(1.0/zoom_level) + xofset;
	suv.y = (suv.y - yofset)*(1.0/zoom_level) + yofset;
	
	
	vec3 colors = vec3(0.);
	
	float atans = (atan(suv.x-0.5,suv.y-0.5)+PI)/(PI*2.);
	
	for (float i = 0.; i < count; i++) {
		vec2 a = (vec2(cos(i*3.),sin((i*3.)+(TIME*speed1)+sin_ofset_1))+1.)/2.;
		vec2 b = (vec2(cos(i+0.5*3.),sin(i+0.5*3.))+1.)/2.;
		vec2 c = (vec2(cos((i+3.)-(TIME*speed2)+sin_ofset_2),sin(i+1.*3.))+1.)/2.;
		colors += vec3(HueShift( vec3(tri(suv,a,b,c),0.,0.) , i/hue_shift ));
	}
	
	
	float alpha = 0.0;
	if(colors.x>0.1 || colors.y>0.1 || colors.z>0.1 ){
		alpha = 1.0;
	}
	vec4 smooth = smoothstep( 0.1, 1.5, abs(vec4(colors, alpha )));
	
	
	gl_FragColor = smooth;

}
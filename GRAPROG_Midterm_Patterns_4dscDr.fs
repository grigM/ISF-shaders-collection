/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "patterns",
    "circle",
    "polygons",
    "shape",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4dscDr by lyradss.  Midterms = No Chill < Passion",
  "INPUTS" : [
	{
			"NAME": "speed",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 8.0,
			"DEFAULT": 2.0
	},
	{
			"NAME": "PATTERN",
			"TYPE": "Int",
			"MIN": 0.0,
			"MAX": 8.0,
			"DEFAULT": 2.0
	}
  ]
}
*/


#define PI 3.14159265359
#define TWO_PI 6.28318530718

#define PATTERN 9

vec3 colorB1 = vec3(0.000, 0.000, 0.400);
vec3 colorR1 = vec3(0.820, 0.031, 0.114);
vec3 colorO = vec3(1.000,0.549,0.000);
vec3 colorY = vec3(1.000,1.000,0.000);
vec3 colorB = vec3(0.000,0.000,1.000);
vec3 colorLB = vec3(0.000,0.749,1.000);
vec3 colorG = vec3(0.000, 1.000, 0.000);
vec3 colorR = vec3(1.000, 0.000, 0.000);
vec3 colorS = vec3(0.647, 0.949, 0.952);
vec3 colorW = vec3(1.000, 1.000, 1.000);
vec3 colorP = vec3(1.000, 0.078, 0.576);
vec3 colorA = vec3(0.831, 0.404, 0.098);

mat2 rotate2d(float angle)
{
    return mat2(cos(angle), -sin(angle), sin(angle), cos(angle)); 
}	

mat2 scale2d(vec2 value)
{
    return mat2(value.x, 0, 0, value.y);
}


float createPolygon(vec2 uv, int sides, float size)
{

    float a = atan(uv.x,uv.y) + PI;
	float r = TWO_PI / float(sides);

	float dist = cos(floor(.5+a/r)*r-a)*length(uv);
    
    float value = 1. - smoothstep(.4, .41, dist * 3.);
    
    
    return value;
}

float createPolygon2(vec2 uv, int sides, float size)
{

    float a = atan(uv.x,uv.y) + PI;
	float r = TWO_PI / float(sides);
  
	float dist = cos(floor(.5+a/r)*r-a)*length(uv);

    float value = 1. - smoothstep(.4, .41, dist * 3.); 
    value +=abs(cos(dist * 500.  ) *0.3) * sin(TIME * 2.) ;
    
    
    return value;
}

float createPolygon3(vec2 uv, int sides, float size)
{

    float a = atan(uv.x,uv.y) + PI;
	float r = TWO_PI / float(sides);

	float dist = cos(floor(.5+a/r)*r-a)*length(uv);
    
    float value = 1. - smoothstep(.4, .41, dist * 3.); 
    value +=abs(cos(dist * 500.  ) *0.3) * sin(TIME * 2.) ;
    value += floor(cos(dist*(sin(TIME)))+.3)* .1;
    value += floor(cos (dist*400.) * 0.2) * sin(TIME * 2.);
    
    
    
    return value;
}

float createCircle1(vec2 uv, vec2 circlepoints, float radius)
{
    
 
    float dist = distance(circlepoints,uv)*2.;
    uv -= circlepoints;
    
    float angle = atan(uv.y, uv.x);
    angle += TIME;
    
    radius = floor(cos (angle*10.) * 0.9) * sin(TIME * 2.);
    radius += floor(cos(angle*abs(sin(TIME)))+.3)* .1;
    
    float value = 1. - step(radius,dist);  
    
   
    return value;
}

float createCircle2(vec2 uv, vec2 circlepoints, float radius)
{
    
 
    float dist = distance(circlepoints,uv)*2.;
    uv -= circlepoints;
    
    float angle = atan(uv.y, uv.x);
    angle += TIME;
    
    radius = floor(cos (angle*10.) * 0.9) * sin(TIME * 2.);
    radius += abs(cos(angle * 40.) *0.4) * sin(TIME * 4.);
    radius += abs(sin(angle * 40. * sin(TIME * 2.)));
    
    float value = 1. - step(radius,dist);  
    
   
    return value;
}

float createCircle3(vec2 uv, vec2 circlepoints, float radius)
{
    
 
    float dist = distance(circlepoints,uv)*2.;
    uv -= circlepoints;
    
    float angle = atan(uv.y, uv.x);
    angle += TIME;
    
    radius = cos(3.*angle - 3.);
    radius += abs(sin(angle * 3.) * sin(TIME * .5));
    radius /= 2.;
    
    float value = 1. - step(radius,dist);  
    
   
    return value;
}


float createCircle4(vec2 circle, vec2 uv, float radius)
{
    
 
    float dist = distance(circle,uv)*2.;
    uv -= circle;
    
    float angle = atan(uv.y, uv.x);
    angle -= TIME;
   	
    radius = cos(10.*angle);
    radius += cos((60.*angle)*0.1);
    radius += floor(cos (angle*400.) * 0.2) * sin(TIME * 2.);
    radius *= abs(cos(angle * 40.) *0.4);
    radius *= abs(sin(angle * 4.) * sin(TIME * .5)); 
    radius += floor(cos(angle*abs(sin(TIME)))+.3)* .1;
    
        
    float value = 1. - step(radius,dist);
    
    
    
    return value;
}

float createCircle5(vec2 circle, vec2 uv, float radius)
{
    
 
    float dist = distance(circle,uv)*2.;
    uv -= circle;
    
    float angle = atan(uv.y, uv.x);
   	angle += TIME * 5.;
    radius = cos(10.*angle);
       
        
    float value = 1. - step(radius,dist);
    
    
    
    return value;
}


float createCircle6(vec2 circle, vec2 uv, float radius)
{
    
 
    float dist = distance(circle,uv)*2.;
    uv -= circle;
    
    float angle = atan(uv.y, uv.x);
   	angle -= TIME * 5.;
    radius = cos(10.*angle);
       
        
    float value = 1. - step(radius,dist);
    
    
    
    return value;
}

float createCircle7(vec2 uv, vec2 circlepoints, float radius)
{
    
 
    float dist = distance(circlepoints,uv)*2.;
    uv -= circlepoints;
    
    float angle = atan(uv.y, uv.x);
    angle += TIME;
    
    radius = sin(4.*angle - 4.);
    radius += abs(sin(angle * 30.) * sin(TIME * .50));
    radius /= 2.;
    
    float value = 1. - step(radius,dist);  
    
   
    return value;
}


void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    
    if (PATTERN == 1 ){
        
    uv*= 5.;
    
    vec2 tileIdx = floor(uv);
 
    float offset = step(1., mod(uv.x, 2.));
    uv.y += offset*TIME;
    
    vec3 color = vec3(0.0);
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    uv = fract(uv);
    uv.x *= ratio;
    
    vec2 shapePos = vec2(.5*ratio, .5);
    uv -= shapePos;
    uv *= rotate2d(sin(TIME * 4.));
    
    color = vec3(createPolygon(uv, 4, 5.)) * colorB1;
    
    if (tileIdx.x == 1. && tileIdx.y == 0. ||
        tileIdx.x == 1. && tileIdx.y == 1. ||
        tileIdx.x == 1. && tileIdx.y == 2. ||
        tileIdx.x == 1. && tileIdx.y == 3. ||
        tileIdx.x == 1. && tileIdx.y == 4. ||
        tileIdx.x == 3. && tileIdx.y == 0. ||
        tileIdx.x == 3. && tileIdx.y == 1. ||
        tileIdx.x == 3. && tileIdx.y == 2. ||
        tileIdx.x == 3. && tileIdx.y == 3. ||
        tileIdx.x == 3. && tileIdx.y == 4.)
        color = vec3(createPolygon(uv, 10, 5.)) * colorR1;
        
    else if (tileIdx.x == 2. && tileIdx.y == 0. ||
        tileIdx.x == 2. && tileIdx.y == 1. ||
        tileIdx.x == 2. && tileIdx.y == 2. ||
        tileIdx.x == 2. && tileIdx.y == 3. ||
        tileIdx.x == 2. && tileIdx.y == 4.)
        color = vec3(createPolygon(uv, 5, 5.)) * colorW;
      
	gl_FragColor = vec4(color,1.0);
    }
    
    
    //
    
    else if (PATTERN == 2 ){
    
    uv*= 7.;
    
    vec2 tileIdx = floor(uv);
  
    float offset = step(1., mod(uv.y, 2.));
    offset -= step (1.,mod(uv.x, 2.));
    uv.x += offset*TIME;
    uv.y += offset*TIME;
      
    vec3 color = vec3(0.0);
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    uv = fract(uv);
    uv.x *= ratio;
    
    vec2 shapePos = vec2(.5*ratio, .5);
    uv -= shapePos;
    uv *= rotate2d(sin(TIME * 4.));
    color = vec3(createPolygon(uv, 5, 5.)) * colorLB;
    
    if (tileIdx.x == 0. && tileIdx.y == 0. ||
        tileIdx.x == 0. && tileIdx.y == 2. ||
        tileIdx.x == 0. && tileIdx.y == 4. ||
        tileIdx.x == 0. && tileIdx.y == 6. ||
        tileIdx.x == 1. && tileIdx.y == 1. ||
        tileIdx.x == 1. && tileIdx.y == 3. ||
        tileIdx.x == 1. && tileIdx.y == 5. ||
        tileIdx.x == 2. && tileIdx.y == 0. ||
        tileIdx.x == 2. && tileIdx.y == 2. ||
        tileIdx.x == 2. && tileIdx.y == 4. ||
        tileIdx.x == 2. && tileIdx.y == 6. ||
        tileIdx.x == 3. && tileIdx.y == 1. ||
        tileIdx.x == 3. && tileIdx.y == 3. ||
        tileIdx.x == 3. && tileIdx.y == 5. ||
        tileIdx.x == 4. && tileIdx.y == 0. ||
        tileIdx.x == 4. && tileIdx.y == 2. ||
        tileIdx.x == 4. && tileIdx.y == 4. ||
        tileIdx.x == 4. && tileIdx.y == 6. ||
        tileIdx.x == 5. && tileIdx.y == 1. ||
        tileIdx.x == 5. && tileIdx.y == 3. ||
        tileIdx.x == 5. && tileIdx.y == 5. ||
        tileIdx.x == 6. && tileIdx.y == 0. ||
        tileIdx.x == 6. && tileIdx.y == 2. ||
        tileIdx.x == 6. && tileIdx.y == 4. ||
        tileIdx.x == 6. && tileIdx.y == 6. )
        color = vec3(createPolygon(uv, 4, 5.)) * colorB;
      
	gl_FragColor = vec4(color,1.0);
    }
    
    
    //
    
    else if (PATTERN == 3 ){
    uv*= 2.;
    
    vec2 tileIdx = floor(uv);
 
    vec3 color = vec3(0.0);
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    uv = fract(uv);
    uv.x *= ratio;
    
    
    vec2 shapePos = vec2(.5*ratio, .5);
    uv -= shapePos;
    uv *= rotate2d(sin(TIME * 10.));
    uv *= scale2d(vec2(abs(sin(TIME ))));
    
    color = vec3(createPolygon2(uv, 8, 5.)) * colorR;
      
	gl_FragColor = vec4(color,1.0);
    }
    
    //
    
    else if (PATTERN == 4 ){
    uv*= 3.;
    
    vec2 tileIdx = floor(uv);
 
    vec3 color = vec3(0.0);
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    uv = fract(uv);
    
    vec2 shapePos = vec2(.5*ratio, .5);
   
    color = createCircle1(uv, vec2(.5,.5), 0.1) * colorG;
      
	gl_FragColor = vec4(color,1.0);
    }
    
    
    //
    
    
    else if (PATTERN == 5 ){
       
    uv*= 2.;
    
    vec2 tileIdx = floor(uv);
 
    vec3 color = vec3(0.0);
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    uv = fract(uv);
    
    vec2 shapePos = vec2(.5*ratio, .5);
   
    color = createCircle2(uv, vec2(.5,.5), 0.1) * colorS;
      
	gl_FragColor = vec4(color,1.0);
    }
    
    
    //
    
     else if (PATTERN == 6 ){
        
    uv*= 4.;
    
    float offset = step(1., mod(uv.x, 2.));
    uv.y += offset*TIME;
    offset += step(0., mod(uv.x, 2.));
    uv.y += offset*TIME/2.;
        
    vec2 tileIdx = floor(uv);
    vec3 color = vec3(0.0);
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    uv = fract(uv);
    vec2 shapePos = vec2(.5*ratio, .5);
   
    color = createCircle3(uv, vec2(.5,.5), 0.1) * mix(colorS, colorP, abs(sin(TIME / 3.0)));
      
	gl_FragColor = vec4(color,1.0);
    }
    
    
    else if (PATTERN == 7 ){
        
    uv*= 4.;
    
    vec2 tileIdx = floor(uv);
        
    vec3 color = vec3(0.0);
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    uv = fract(uv);
    
    vec2 shapePos = vec2(.5*ratio, .5);
    
    color = createCircle4(uv, vec2(.5,.5), 0.1) * mix(colorW, colorR, abs(sin(TIME / 3.0)));
     if(tileIdx.x == 0. && tileIdx.y == 0. ||
        tileIdx.x == 0. && tileIdx.y == 2. ||
        tileIdx.x == 1. && tileIdx.y == 1. ||
        tileIdx.x == 1. && tileIdx.y == 3. ||
        tileIdx.x == 2. && tileIdx.y == 0. ||
        tileIdx.x == 2. && tileIdx.y == 2. ||
        tileIdx.x == 3. && tileIdx.y == 1. ||
        tileIdx.x == 3. && tileIdx.y == 3.)
        
        color = createCircle4(uv, vec2(.5,.5), 0.1) * mix(colorR, colorW, abs(sin(TIME / 3.0)));
      
	gl_FragColor = vec4(color,1.0);
    }
    
    
    //
    
    
    else if (PATTERN == 8 ){
     
    uv*= 6.;
   
    vec2 tileIdx = floor(uv);
    vec3 color = vec3(0.0);
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    uv = fract(uv); 
    
    vec2 shapePos = vec2(.5*ratio, .5);
    
    color = createCircle5(uv, vec2(.5,.5), 0.1) * colorR;
     if(tileIdx.x == 0. && tileIdx.y == 3. ||
        tileIdx.x == 0. && tileIdx.y == 5. ||
        tileIdx.x == 1. && tileIdx.y == 4. ||
        tileIdx.x == 2. && tileIdx.y == 3. ||
        tileIdx.x == 2. && tileIdx.y == 5.)
        
        color = createCircle6(uv, vec2(.5,.5), 0.1) * mix(colorW, colorB, abs(sin(TIME * 3.0)));
        
     else if(tileIdx.x == 0. && tileIdx.y == 4. ||
        tileIdx.x == 1. && tileIdx.y == 3. ||
        tileIdx.x == 1. && tileIdx.y == 5. ||
        tileIdx.x == 2. && tileIdx.y == 4.)
        
        color = createCircle6(uv, vec2(.5,.5), 0.1) * mix(colorB, colorW, abs(sin(TIME * 3.0)));
         
     if(tileIdx.x == 0. && tileIdx.y == 0. ||
        tileIdx.x == 1. && tileIdx.y == 0. ||
        tileIdx.x == 2. && tileIdx.y == 0. ||
        tileIdx.x == 3. && tileIdx.y == 0. ||
        tileIdx.x == 4. && tileIdx.y == 0. ||
        tileIdx.x == 5. && tileIdx.y == 0. ||
        tileIdx.x == 0. && tileIdx.y == 2. ||
        tileIdx.x == 1. && tileIdx.y == 2. ||
        tileIdx.x == 2. && tileIdx.y == 2. ||
        tileIdx.x == 3. && tileIdx.y == 2. ||
        tileIdx.x == 4. && tileIdx.y == 2. ||
        tileIdx.x == 5. && tileIdx.y == 2. ||
        tileIdx.x == 3. && tileIdx.y == 4. ||
        tileIdx.x == 4. && tileIdx.y == 4. ||
        tileIdx.x == 5. && tileIdx.y == 4.)
       
         color = createCircle6(uv, vec2(.5,.5), 0.1) * colorW;
         
	gl_FragColor = vec4(color,1.0);
    }
    
    
    //
    
    
    else if (PATTERN == 9 ){
    uv*= 5.;
    vec2 tileIdx = floor(uv);
    vec3 color = vec3(0.0);
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    uv = fract(uv);
    uv.x *= ratio;
    
    vec2 shapePos = vec2(.5*ratio, .5);
    uv -= shapePos;
    uv *= scale2d(vec2(abs(sin(TIME ))));
    
    color = vec3(createPolygon3(uv, 4, 5.)) * colorS;
      
	gl_FragColor = vec4(color,1.0);
    }
    
    
    //
    
    
    else if (PATTERN == 10 ){
    uv*= 3.;
 
    float offset = step(1., mod(uv.x, 2.));
    uv.y += offset*TIME;
        
    vec2 tileIdx = floor(uv);
    vec3 color = vec3(0.0);
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    uv = fract(uv);
    vec2 shapePos = vec2(.5*ratio, .5);
      
    color = createCircle7(uv, vec2(.5,.5), 0.1) * colorA;
      
	gl_FragColor = vec4(color,1.0);
    }
    
}

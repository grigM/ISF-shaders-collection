/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "smily",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ldXyRs by Pinillya.  Trying a tutorial",
  "INPUTS" : [

  ]
}
*/


float Circle (float rad, vec2 position, vec2 uv, float blur)
{
    float dist = length(uv-position);
   	float c = smoothstep(rad, rad-blur, dist);
    return c;
}

float Band(float t, float start, float end, float blur)   
{
    float step1 = smoothstep(start-blur, start+blur, t);
    float step2 = smoothstep(end+blur, end-blur, t);
    return step1 * step2;
}

float Rect(vec2 uv, float left, float right, float bott, float top, float blur)
{
    float rect = Band(uv.x, left, right, blur);
    rect *= Band(uv.y, bott, top, blur);
    return rect;
}

float remap01(float a, float b, float t)
{
 	return (t-a) / (b-a);
}

float remap(float a, float b, float c, float d, float t)
{
 	return remap01(a, b, t) * (d-c) + c; 
}

float Smily (vec2 uv, float size, vec2 position)
{
    float face = Circle(0.5*size, vec2(.0)+position, uv, 0.03*size);
    
    face -= Circle(0.1*size, vec2(-.13*size, .2*size)+position, uv, 0.01*size);
    face -= Circle(0.1*size, vec2(.13*size, .2*size)+position, uv, 0.01*size);
    
    float mouth = Circle(0.35*size, vec2(.0, .0)+position, uv, 0.01*size);
    mouth -= Circle(0.35*size, vec2(.0, .1*size)+position, uv, 0.01*size);
        
    face -= mouth;
    
  	return face;
}


void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy; // uv.y 1 on top of screen pixel coordiant
    float t = TIME;
    uv -= .5;
    uv.x *= RENDERSIZE.x/RENDERSIZE.y;
    float x = uv.x;
    float y = uv.y;
    
    //vec3 col = vec3(1., 0., 1.);
	//float smily = Smily(uv, 1.1, vec2(.2, .1));
    //col *= mask;
  	//col *= vec3(1., 1., 1.);
    //col *= mask;
    
    //x += y*0.2;
    
    float m = sin(t + x * 8.)* .1;
    y -= m;
    
    float blur = remap(-.5, .5, .01, .25, x);
    //blur = blur*blur;
	
    blur = pow(blur*4., 3.);
    
    float mask = Rect(vec2(x, y), -.5, .5, -.1, .1, blur);
    //mask += Rect(vec2(x, y), -.2 + y *.3, .1 + y * 0.7, -.2, -.0, .01);
    gl_FragColor = vec4(vec3(mask), 1.0);
}

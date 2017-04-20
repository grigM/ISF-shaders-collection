/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "triangle",
    "colors",
    "aimation",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XsXcWS by Ahmidou.  A funky and colorful effect done by taking some parts from other shaders and tweaking a bit",
  "INPUTS" : [

  ]
}
*/


// note that your triangle was drawn for 2*h
bool isInTriangle(vec2 U, float h, float a)
{
    float s = sin(a), c = cos(a);
  	U *= mat2(c,-s,s,c);
    
    return     U.y > - h/3.  // distance from center to each sides
         && .5*U.y - 1.73/2. * U.x < h/3.
         && .5*U.y + 1.73/2. * U.x < h/3.;
}
vec3 hsv2rgb(float h, float s, float v)
{
    h = fract(h);
    vec3 c = smoothstep(2./6., 1./6., abs(h - vec3(0.5, 2./6., 4./6.)));
    c.r = 1.-c.r;

    return mix(vec3(s), vec3(1.0), c) * v;
}

vec3 getRandomColor(float f, float t)
{
    return hsv2rgb(f+t, 0.2+cos(sin(f))*0.3, 0.9);
}

void main() {



    float step = 15.0;
	vec2  R = RENDERSIZE.xy;
    float t = TIME;
    gl_FragColor =  vec4(1.0, 1.0, 1.0, 1.0);
    for(int i = 0; i < int(step); i++) {
        float m = 1.0/(step+1.0)*float(i);
        vec3 color = getRandomColor(float(i) *0.05 + 0.01, t);
        bool test = isInTriangle((gl_FragCoord.xy-.5*R)/R.y+(float(i)*0.01), mod(1000.0 - t, 1000.0/R.y) * 3.0 - m*15.0, (3.1415 * t)+(float(i)*5.0));
        if( test )   
    		gl_FragColor =  vec4(color, 1);
    }
}

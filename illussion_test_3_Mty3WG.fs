/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "illusion",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Mty3WG by brchan_toy.  illusion",
  "INPUTS" : [

  ]
}
*/


vec4 hi_green = vec4(.341,1.,0.,1.);
vec4 hi_blue = vec4(.219,0.,1.,1.);
vec4 white =  vec4(1.,1.,1.,1.);
vec4 black =  vec4(0.,0.,0.,1.);

vec4 red =  vec4(1.,0.,0.,1.);
vec4 green =  vec4(0.,1.,0.,1.);
vec4 cyan =  vec4(0.,1.,1.,1.);

float LINES_NUMBER = 30.;
void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv = uv - vec2 (0.5, 0.5);
    vec2 sc = vec2 (RENDERSIZE.x / RENDERSIZE.y, 1.);
    //
    float set = 0.;
    float shift = TIME * 6.;
    vec4 color = white;
    vec4 color_0 = white;
    vec4 color_1 = white;
    
    //LINES_NUMBER = 40. * (2.-length (uv * sc));
    
    //float angle = degrees (atan(uv.y * sc.y,uv.x * sc.x) ) + 180. + shift*10.;
    
    //if (uv.x<=0.5) shift = -shift+1./LINES_NUMBER;

    //float test = floor((uv.x + shift)*LINES_NUMBER);
    vec4 cl = red;
    if (length ( uv * sc)<0.36){
        shift = -shift;
        //cl = cyan;
    }
    
    float test = floor(length (uv * sc * vec2(1.,1.))*LINES_NUMBER + shift  );
    if (mod( test, 2.) == 1.)
		color_0 = red;
    else
        color_0 = black;
    
    float ln = -abs(uv.x * sc.x) - abs(uv.y * sc.y);
    test = floor(length (ln * vec2(1.,1.))*LINES_NUMBER - shift);
    if (mod( test, 2.) == 1.)
		color_1 = white;
    else{
        color_1 = black;
    }
    
    
    //if (mod(float(iFrame), 2.) == 1.)
   // 	color = color_1;
    //else
        color = color_1 ;
    
    if (abs(uv.x * sc.x) < 0.01 && abs(uv.y * sc.y) < 0.01) color = red;
	gl_FragColor = color;
}
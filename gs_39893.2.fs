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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#39893.2"
}
*/



// Author: Patricio Gonzalez Vivo
// http://glslsandbox.com/e#39877.0


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

#define PI 3.1415926535
#define HALF_PI 1.57079632679



// Title: recoded Fractal Invaders by Jared Tarbell
// http://www.levitated.net/daily/levInvaderFractal.html

vec3 cosPalette(  float t,  vec3 a,  vec3 b,  vec3 c, vec3 d ){
    return a + b*cos( 6.28318*(c*t+d) );
}

vec3 pal( float t ) {
	vec3 a = vec3(0.5);
	vec3 b = a;
	vec3 c = vec3(1.0);
	vec3 d = vec3(0.00,0.33,0.67);
	return ( cosPalette( t, a, b, c, d ) );
}

float random(in float x){ return fract(sin(x)*43758.5453); }
float random(in vec2 st){ return fract(sin(dot(st.xy ,vec2(12.9898,78.233))) * 43758.5453); }

float randomChar(vec2 outer,vec2 inner){
    float grid = 7.;
    vec2 margin = vec2(.15,.15);
    vec2 borders = step(margin,inner)*step(margin,1.-inner);
    vec2 ipos = floor(inner*grid);
    ipos = abs(ipos-vec2(3.,0.));
    return step(.5, random(outer*64.+ipos)) * borders.x * borders.y;
}

void main(){
    vec2 st = gl_FragCoord.st/RENDERSIZE.xy;
    float ratio = RENDERSIZE.x/RENDERSIZE.y;
    st.x *= ratio;
    vec3 color = vec3(0.0);

    float rows = 3.0;
    rows += floor(mod(mouse.y*33.0,64.));
    
    float t = 1.+TIME*1.3;
    vec2 vel = vec2(0.,1.-floor(t));
    
    vec2 ipos = floor(st*rows);
    vec2 fpos = fract(st*rows);
    
    vec3 pct = vec3(1.0);
    vec2 chr = mod(ipos + vel,vec2(mouse.y));
    pct *= vec3( randomChar( chr + mouse.x*0.1,fpos), randomChar( chr * 3.0,fpos), randomChar( chr * 333.0,fpos) );
	
    pct *= randomChar( chr + 33.0,fpos);
	
//    if (ipos.y > 0.0 || ipos.x < rows*ratio) {
        color = vec3(pct);
//    }  
	
//    color += pal(TIME*0.1);

    float n = ( ( ipos.y + ipos.x / rows ));
    gl_FragColor = vec4( color, 1.0);
}
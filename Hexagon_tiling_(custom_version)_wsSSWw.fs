/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wsSSWw by deerfeeder.  adding custom modulations",
  "INPUTS" : [

  ]
}
*/


// Fork of "Hexagon (layout)" by deerfeeder. https://shadertoy.com/view/tsjSDD
// 2019-03-29 19:35:41

float skew = 1.73;
float blur = 0.99;
float scale = 5.0;

float HexDist(vec2 p) {
    // vertical mirror
	p = abs(p);
    
    // hex skew;
    float c = dot(p, normalize(vec2(1,skew)));
    // vertical sides
    c = max(c, p.x);    
    return c;
}


vec4 HexCoords(vec2 uv){
 
   	//skew
    vec2 r = vec2(1,skew);
    vec2 h = r * 0.5;
    vec2 a = mod(uv, r)-h;
	
    
    //basegrid midpoints
    vec2 b = mod(uv-h, r )-h;
    vec2 gv = dot(a,a) < dot(b,b) ? a : b;
    vec2 id = uv - gv;
    float y = atan(gv.x,gv.y)+sin(1.* TIME+id.y);
    float x = atan(gv.x,gv.y)+cos(0.5* TIME+id.x);
    return vec4(x, y,id.x,id.y);
}


void main() {



   	// middle & squared 
    vec2 uv = (gl_FragCoord.xy - .5 * RENDERSIZE.xy)/RENDERSIZE.y;
 	scale -= abs((scale*0.75) * sin(TIME*0.4));
    skew += 1. *cos(TIME);
    vec3 col = vec3(0);
	uv = uv * scale;
    vec4 hc = HexCoords(uv);
    
 	col.rgb = hc.yyy*0.2;
   	
    
    // Output to screen
    gl_FragColor = vec4(col,1.);
}

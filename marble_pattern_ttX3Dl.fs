/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ttX3Dl by ankd.  marble pattern",
  "INPUTS" : [

  ]
}
*/


vec2 rotate(in vec2 p, in float r){
	float c=cos(r), s=sin(r);
    return mat2(c, -s, s, c)*p;
}

void main() {



    vec2 p  = (gl_FragCoord.xy*2.0-RENDERSIZE.xy)/min(RENDERSIZE.x, RENDERSIZE.y);
    
    for(int i=0;i<5;i++){
        p = rotate(p, TIME*0.2+sin(length(p)));
		p.x += p.y*sin(p.x*1.97+TIME*0.45 + sin(p.y*1.25+TIME*0.97));
		p.y += p.x*sin(p.y*1.36+TIME*0.45 + sin(p.y*1.82+TIME*0.97));
    }
    
    vec3 col1 = vec3(0.2, 0.5, 0.9);
    vec3 col2 = vec3(0.9, 0.95, 1.0);
    vec3 col = clamp(mix(col1, col2, min(2.0, (0.5*length(p)))), 0., 1.);
    gl_FragColor = vec4(col,1.0);
}

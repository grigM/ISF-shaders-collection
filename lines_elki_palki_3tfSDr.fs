/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3tfSDr by foran.  lines",
  "INPUTS" : [

  ]
}
*/



const float PI=3.1415926535;
void main() {



  vec2 coord=6.*(gl_FragCoord.xy-.05*RENDERSIZE.xy)/min(RENDERSIZE.y,RENDERSIZE.x);//   6.* - zoom
    for(int n=1;n<8;n++){
  float i=float(n);
  //coord += vec2(0.7 / i * sin(i * coord.y + TIME + 0.3 * i) + 0.8, 0.4 / i * sin(coord.x + TIME + 0.3 * i) + 1.6);
  coord+=vec2(.7/i*sin(coord.y+.3*i)+.8,.4/i*sin(coord.x+.3*i)+1.6)*7.;//   *7. - blur
}
//coord -= vec2(0.7 / sin(coord.y + TIME + 0.3) + 0.8, 0.4 / sin(coord.x + TIME + 0.3) + 1.6);
//coord*=vec2(.14/abs(coord.x+TIME+.2),.14/cos(coord.x+TIME+.2)+1.6);
//coord*=vec2(.14/tan(coord.x+TIME+.2),.14/tan(coord.x+TIME+.2)+1.6);
//coord*=vec2(coord.x*1.+TIME+1.2*coord.y+TIME);
//coord-=vec2(coord.x+TIME+sin(2.*PI*coord.y+TIME)*2.);
//coord-=vec2(coord.y+TIME+sin(2.*PI*coord.x+TIME)*2.);
coord-=vec2(1.+(pow((sin(coord.x*1.-115.+TIME)-10.),1.)+pow((cos(coord.y*1.-5.+TIME)-10.),2.)));//1-((x*2-1)^2+(y*2-1)^2)-Checker Circle
vec3 color=vec3(.5*sin(coord.x)+.5,.5*sin(coord.y)+.5,sin(coord.x+coord.y));
  gl_FragColor=vec4(color,1.);
}

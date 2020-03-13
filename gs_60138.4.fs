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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60138.4"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


/*void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy ) + mouse / 4.0;

	float color = 0.0;
	color += sin( position.x * cos( TIME / 15.0 ) * 80.0 ) + cos( position.y * cos( TIME / 15.0 ) * 10.0 );
	color += sin( position.y * sin( TIME / 10.0 ) * 40.0 ) + cos( position.x * sin( TIME / 25.0 ) * 40.0 );
	color += sin( position.x * sin( TIME / 5.0 ) * 10.0 ) + sin( position.y * sin( TIME / 35.0 ) * 80.0 );
	color *= sin( TIME / 10.0 ) * 0.5;

	gl_FragColor = vec4( vec3( color, color * 0.5, sin( color + TIME / 3.0 ) * 0.75 ), 1.0 );
}*/


bool inCircle(vec2 center, float size, vec2 pos) {
  float dist = distance(pos, center);
  return dist <= size;
}

bool ipsoid(vec2 center, float size, float offset, float angle, vec2 pos) {
  vec2 offsetShift = vec2(cos(angle),sin(angle));
  vec2 center1 = center - offset * size * offsetShift;
  vec2 center2 = center + offset * size * offsetShift;
  return inCircle(center1, size, pos) && inCircle(center2, size, pos);
}

vec4 infty(float i, float maxI) {
  float r = 0.45+0.2*sin((i*7.0)/maxI+TIME);
  float g = 0.7+0.2*sin(2.0+(i*10.0)/maxI+TIME);
  float b = 0.55+0.3*sin(2.0+(i*7.0)/maxI+TIME);
  vec4 infty = vec4(r,g,b,1);
  return infty;
}

vec4 infty(int i, int maxI) {
  return infty(float(i), float(maxI));
}

void main(void)
{
  vec2 pos = vec2(gl_FragCoord.x / RENDERSIZE.x, gl_FragCoord.y / RENDERSIZE.y);
  pos -= 0.5;
  pos /= vec2(RENDERSIZE.y / RENDERSIZE.x, 1);

  vec4 background = vec4(0,0.05,0.05,1);
  vec4 foreground = vec4(0,0,0,1);
  
  const int maxI = 25;
  int inside = 0;
  int outSide = 0;
  for (int i = 0; i < maxI; i++) {
    float i_ratio = float(i)*1.0 / float(maxI);
    bool lipsi = ipsoid(vec2(0.2*sin((0.2*TIME+i_ratio)*3.1415)+0.1*sin(2.0+1.5*i_ratio*mouse.x), 0.2*cos(0.1*TIME+123.0)*cos((0.3*TIME+i_ratio*mouse.y)*3.1415)),
                        5.0 - (i_ratio) * 5.1,
                        0.85,
                        3.1415/2.0 + sin(5.0+mouse.x*0.05*(0.09+sin(TIME*.1))*5.*i_ratio*TIME+TIME) + sin(i_ratio*mouse.x) + 0.25*TIME*mouse.y/100.0,
                        pos);
    foreground += float(int(lipsi)) * 1.0 / float(maxI) * infty(i, maxI);
    
    if (!lipsi) {
      break;
    }
  }
  float inside_ratio = float(inside)*1.0 / float(maxI);
  
  gl_FragColor = foreground + background;
}
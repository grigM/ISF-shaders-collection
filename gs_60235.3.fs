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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60235.3"
}
*/


// +japseye
#ifdef GL_ES
precision highp float;
#endif


void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy ) -.5+ mouse ;
	position.y *= dot(position,position);
	
	float color1 = 0.0;
	float color2 = 0.0;
	float color3 = 0.0;
	float color4 = 0.0;
	float color5 = 0.0;
	float color6 = 0.0;
	float equation = 0.0;
	float equation2 = 0.0;
	//float size = 8.0;
	float size = 10.0 *0.4 + .0;
	float posx = size - position.x * size * 2.0;
	float posy = size - position.y * size * 2.0;
	float moux = mouse.x - 0.5;
	float mouy = mouse.y - 0.5;
	float wavy = 0.0;
	
	//---------------------------------------------------------
	wavy = sin((posx+posy)*cos(posx-TIME)-sin(posy-TIME)-cos(posy-TIME));
	equation = sin(TIME*.1)+(posx+posy + wavy);
	
	//equation2 = posx*posy * sin(posx-posy*moux) * sin(posx-posy*mouy);
	equation2 = equation;
	//---------------------------------------------------------
	
	color1 = 0.0 - equation;
	color3 = equation;
	if (equation > 3.0) {
		color1 = 0.5;
		color2 = 0.75;
		color3 = 0.75;
	} else if (equation > 2.0) {
		color1 = equation - 2.0;
		color2 = 1.0;
	} else if (equation > 1.0) {
		color2 = equation - 1.0;
	};
	if (equation < -3.0) {
		color3 = 0.5;
		color2 = 0.75;
		color1 = 0.75;
	} else if (equation < -2.0) {
		color3 = 0.0 - equation - 2.0;
		color2 = 1.0;
	} else if (equation < -1.0) {
		color2 = 0.0 - equation - 1.0;
	};
		
		
	color4 = 0.0 - equation2;
	color6 = equation2;
	if (equation2 > 3.0) {
		color4 = 0.5;
		color5 = 0.75;
		color6 = 0.75;
	} else if (equation2 > 2.0) {
		color4 = equation2 - 2.0;
		color5 = 1.0;
	} else if (equation2 > 1.0) {
		color5 = equation2 - 1.0;
	};
	if (equation2 < -3.0) {
		color6 = 0.5;
		color5 = 0.75;
		color4 = 0.75;
	} else if (equation2 < -2.0) {
		color6 = 0.0 - equation2 - 2.0;
		color5 = 1.0;
	} else if (equation2 < -1.0) {
		color5 = 0.0 - equation2 - 1.0;
	};
	
	//if (RENDERSIZE.x == RENDERSIZE.y) {color2 = 0.75;};
	
	gl_FragColor = vec4( .2-vec3((color1+color4)/4.0,(color2+color5)/1.0, (color3+color6)/23.0 ), 1.0 );

}
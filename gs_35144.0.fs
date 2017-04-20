/*
{
  "CATEGORIES" : [
    "Automatically Converted"
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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35144.0"
}
*/




void main(void){
	vec2 uv = 1.5*(2.0*gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.y;
	vec2 offset = (mouse*2.0-1.0)*1.5;
	offset.x *= RENDERSIZE.x/RENDERSIZE.y;
	//float t = TIME*50.0;
	//vec2 offset = vec2(cos(t),+sin(t))/10.0;
	//how could I make an eclipse?
	
	vec3 light_color = vec3(0.9, 0.7, 0.5);
	float light = 0.0;
	
	light = 0.1 / distance(normalize(uv), uv);
	
	if(length(uv) < 1.0){
		light *= 0.1 / distance(normalize(uv-offset), uv-offset);
	}

	gl_FragColor = vec4(light*light_color, 3.0);
}
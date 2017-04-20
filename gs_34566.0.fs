/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [
	{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 0.09,
			"MIN": 0.0,
			"MAX": 0.4
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34566.0"
}
*/




mat2 rotate = mat2(cos(TIME * speed), -sin(TIME * speed), sin(TIME* speed), cos(TIME* speed));

void main( void ) {
	vec2 aspect = RENDERSIZE.xy / min(RENDERSIZE.x, RENDERSIZE.y);
	vec2 position = (gl_FragCoord.xy / RENDERSIZE.xy) * aspect;
	vec2 center = 0.5 * aspect;
	vec3 color = vec3(0.0);
	float a = 0.0;
	position *= rotate;
	center *= rotate;
	
	vec2 next = vec2(0.2, 0.2) * rotate;
	
	
	for(int i = 45; i > 0; i--) {
		next *= rotate;
		if(distance(position, center + next) < 0.03 + float(i) * 0.005) {
			color = vec3(sin((TIME*speed) + float(i)), sin((TIME*speed) + 4.0 + float(i)), sin((TIME*speed) + 4.0 + float(i)));
			a = 1.0;
		
		}
	}
	
	gl_FragColor = vec4(color,a);

}
/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#34935.0"
}
*/




mat2 rotate = mat2(cos(TIME * 0.09), -sin(TIME * 0.09), sin(TIME* 0.09), cos(TIME* 0.09));

void main( void ) {
	vec2 aspect = RENDERSIZE.xy / min(RENDERSIZE.x, RENDERSIZE.y);
	vec2 position = (gl_FragCoord.xy / RENDERSIZE.xy) * aspect;
	vec2 center = 0.5 * aspect;
	vec3 color = vec3(0.0);
	
	position *= rotate;
	center *= rotate;
	
	vec2 next = vec2(0.2, 0.2) * rotate;
	float alpha = 0.0;
	
	for(int i = 45; i > 0; i--) {
		next *= rotate+sin(1./float(i));
		if(distance(position, center + next) < 0.03 + float(i) * 0.005) {
			color = vec3(sin(TIME + float(i)), sin(TIME + 2.08 + float(i)), sin(TIME + 4.08 + float(i)));
			alpha = 1.0;
		}
	}
	
	gl_FragColor = vec4(color, alpha );

}
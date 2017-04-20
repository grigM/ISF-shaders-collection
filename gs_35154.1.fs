/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35154.1"
}
*/




void main() {
	float theta = 0.1*TIME;
	mat2 rot_theta = mat2(
		cos(theta), -sin(theta),
		sin(theta), cos(theta));
	float psi = -0.05 * TIME;
	mat2 rot_psi = mat2(
		cos(psi), -sin(psi),
		sin(psi), cos(psi));
	
	float alpha = 42.0;
	float a = cos(length(-rot_theta*gl_FragCoord.xy/alpha + rot_theta*vec2(15.0, 15.0)));
	float b = cos(923.0+length(-rot_psi*gl_FragCoord.xy/alpha + rot_theta*vec2(-5.0, -5.0)));
	float c = sin(length(rot_psi*gl_FragCoord.xy/(20.0*cos(0.01*TIME)+28.0) + rot_psi*vec2(16.0, 16.0)));
	
	vec3 color_a = vec3(a, 0.3*a, 0.4*a*cos(TIME));
	vec3 color_b = vec3(0.2*b, sin(TIME)*b, cos(TIME*0.1)*b);
	vec3 color_c = vec3(cos(TIME*0.4)*c, cos(TIME)*c*0.8, c);
	
	vec3 color = color_a + color_b + color_c;
	
	gl_FragColor = vec4(color, 1.0);
}
/*{
  "CREDIT": "by DANtheMAN",
  "DESCRIPTION": "",
  "CATEGORIES": [
  ],
  "INPUTS": [
    {
      "NAME": "pos",
      "TYPE": "float",
      "MAX" : 10,
      "MIN" : 0
    }
  ]
}*/

#define MAX_ITER 12
#define PI 3.14159265359
 
void main( void ) {
 
    vec2 p = gl_FragCoord.xy / RENDERSIZE.x - vec2(0.5);


	vec2 i = p;
 
	float c = 0.0;
 
	float d = length(i);
	float r = atan(i.x, i.y);
	for (int n = 0; n < MAX_ITER; n++) {
		i = i + vec2( 
				1.0/float((n+1) * MAX_ITER/(n+1)) * d * sin(r * i.y * (pow(float(n), 2.0 - (sin(TIME * 0.022))) * float(MAX_ITER)/float(n+1)) + pos*PI),
				1.0/float((n+1) * MAX_ITER/(n+1)) * d * cos(r * i.x * (pow(float(n), 2.0 - (cos(TIME * 0.022))) * float(MAX_ITER)/float(n+1)) + pos*PI)
			);
		
		d = length(i);
	  r = (atan(i.x, i.y));
	}
	
	c = (1.0-d);
	c = pow(c, 2.2);
	gl_FragColor = vec4(c);
}
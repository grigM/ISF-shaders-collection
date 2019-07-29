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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#37086.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define particles_count 0.2
#define k 0.2

#define zmul(a, b) vec2(a.x*b.x-a.y*b.y, a.x*b.y+b.x*a.y)
#define zinv(a) vec2(a.x, -a.y) / dot(a,a*(0.7 + 0.6*sin(TIME)))

void main()
{
	vec2 g = gl_FragCoord.xy;
	vec2 si = RENDERSIZE.xy;
    	vec2 m = mouse.xy/si;
	float t = TIME;
    	vec3 stars = vec3(0);
    
    	vec2 z = (g+g-si)/min(si.x,si.y) * 2.;
    
	vec2 c = vec2(0.96,1.23);
	
   	float r = 0.;
	for (float i=0.;i<2.;i++)
	{
		if (r > 4.) break;
        r = dot(z,z);
		z = zinv( (zmul(z, z) + c));  
        
        vec3 col = mix(vec3(0.5,0,0.5), vec3(-3.*sin(0.7*TIME),4. * cos(0.6*TIME),-3.29), i/(12.*(0.8 + 0.5*sin(0.4*TIME))));
        
        vec3 acc = vec3(0);
        for (float j=0.;j<particles_count;j++)
        {
            float tt = t + j/(3.14159/(particles_count*k));
            vec2 b = vec2(cos(tt), sin(tt)) * 2.;
        	acc += col/r/8./dot(z-b,z-b);
        }
        stars += acc / particles_count / 0.75;
    }
 
    gl_FragColor = vec4(stars * 0.3,1);
}
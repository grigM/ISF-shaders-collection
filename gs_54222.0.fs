/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54222.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


float distFunc(vec3 p)
{
    return length(mod(p+vec3(0,0,mod(-TIME*19.,4.)),4.)-2.)-.4;
}

float quad(float a)
{
	return a * a;
}

float sphere(vec3 M, vec3 O, vec3 d)
{
	float r = 0.2;
	vec3 MO = O - M;
	float root = quad(dot(d, MO))- quad(length(d)) * (quad(length(MO)) - quad(r));
	if(root < 0.001)
	{
		return -1000.0;
	}
	float p = -dot(d, MO);
	float q = sqrt(root);
    return (p - q) > 0.0 ? p - q : p + q;
}

vec3 sphere_color(float t, vec3 mid){
	
    float a = t < 10000.0? 1.0 : 0.0; 
    
    return abs(normalize( mid * vec3( 1.4, 1.0, 0.2))) * a;

}

void main()
{
float ss = 8.0*sin(TIME*0.3);
	vec2 gg = gl_FragCoord.xy;
	gg = ceil(gg / ss) * ss;
	
    vec3 sphereM;
    
    float fov = 100.0;
	float tanFov = tan(fov / 2.0 * 3.14159 / 140.0) /RENDERSIZE.x;
	vec2 p = tanFov * (gg * 2.0 - RENDERSIZE.xy);

	vec3 camP = vec3(3.0, 3.0, 0.0);
	vec3 camDir = normalize(vec3(p.x - 0.3, p.y - 0.2, 1.0));

	float t = 10000.0;
	for(float x = -2.0; x <= 2.0; ++x)
	{
		for(float y = 1.0; y <= 1.0; ++y)
		{
			for(float z = 1.0; z <= 10.0; z+= 0.25)
			{	
                vec3 newSphereMid = vec3(x, y + cos(TIME * 2.0 + z), z);
				float newT = sphere(newSphereMid, camP, camDir);
				if (0.0 < newT && newT < t)
				{	
					t = newT;
                    sphereM = newSphereMid;
				}
			}
		}
	}
    
    vec3 light = normalize(vec3(1.0, 1.0, 1.0));
    
    vec3 intersect_pt = camP + t* camDir;
    
    vec3 intersec_normal = normalize( intersect_pt - sphereM );
                                   
    float lightning = max(0.2, dot(intersec_normal, light));

	gl_FragColor = lightning * vec4(sphere_color(t, sphereM), 1.0) * 1.5;
}
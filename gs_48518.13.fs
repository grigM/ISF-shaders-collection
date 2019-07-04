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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#48518.13"
}
*/


// 
// v 1.1
// parent: http://glslsandbox.com/e#48495.1



//precision highp float;


// generalized distance fields using minkowski space
// https://pdfs.semanticscholar.org/fa9c/b8957468892bf660f3afda3c002f6d468a81.pdf <<origional paper
// http://glslsandbox.com/e#48340.2 < simplified example
// sphinx
#define minkowski(v,m) pow(dot(pow(v, v*0.+m), v*0.+1.), 1./m)


#define pi  (atan(1.0) * 4.0)
#define tau (atan(1.0) * 8.0)

float map(in vec3 p)
{ 

    float tm = -mod(ceil( TIME * 2.0 ), -12.0) + 0.001;  	
   	
    float m = minkowski(abs(p), tm ) - 0.51;
    
    return m;
}


mat3 rmat(float a, float b) 
{	
	float c = cos(a), s = sin(a), c1 = cos(b), s1 = sin(b);
	return mat3(1, 0, 0, 0, c1, -s1, 0, s1, c1) * mat3(c, 0, s, 0, 1, 0, -s, 0, c);
}

// copy +paste https://www.shadertoy.com/view/ll2XRD
// thank you ! 
vec3 computeColor(in vec3 ray, in vec3 m, in vec3 normal, in vec3 light, in vec3 eye, vec3 envmap) {
    
    vec3 lightRay = normalize(m - light);
    float diffuse = dot(normal, -lightRay);
    
    vec3 reflectedLight  = reflect(lightRay, normal);
    float hilight = pow(max(dot(reflectedLight, -ray),0.0), 20.0);
    
    vec3 diffuseComponent = vec3(1.0, 0.0, 0.0) * diffuse;
    vec3 hilightComponent = vec3(1.0, 1.0, 1.0) * hilight;    
    vec3 ambiantComponent = vec3(0.2, 0.0, 0.0);       
    
    vec3 reflectray = reflect(m-eye, normal);
    vec3 sphereColor = diffuseComponent + hilightComponent + ambiantComponent;
    
    return vec3(mix(sphereColor, envmap, 0.29));
}


vec3 computeNormal(in vec3 pos)
{
	vec3 eps = vec3( 0.001, 0.0, 0.0 );
	vec3 nor = vec3(
	     map(pos+eps.xyy) - map(pos-eps.xyy),
	     map(pos+eps.yxy) - map(pos-eps.yxy),
	     map(pos+eps.yyx) - map(pos-eps.yyx));
    
	return normalize(nor);
}
void main()
{
        
	
    // ----------------------- CAMERA
   
    vec2 st = 2.0 * gl_FragCoord.xy / RENDERSIZE.xy - 1.0;
	
    // black bars cant live without 
    // 0.89 PAL/NTSC im a kid of the 90ties
    if( acos( abs(st.y) )  < 0.6777)
    {
    	discard;
    }
    
    float aspect = (RENDERSIZE.x / RENDERSIZE.y);
    
    vec3 ro = vec3( 0.001, 0.001, -3.0);
    
    vec3 lookAt = vec3(0.001);
    vec3 up = vec3(0.001, 1.001, 0.001);
    float fov = 95.001;
    vec3 g = normalize(lookAt - ro.xyz);
    vec3 u = normalize(cross(g, up));
    vec3 v = normalize(cross(u, g));
    u = u * tan(radians(fov * 0.501)); 
    v = v * tan(radians(fov * 0.501)) / aspect;
    vec3 rd = normalize(g + st.x * u + st.y * v);
    
    // mouse and basic rotation
    vec2 mouse = 2.5 * atan(( 3.14 * (mouse.xy) -1.) * vec2(aspect, 1.0) );
    // 
    mat3 rot = rmat(mouse.x , mouse.y );
    rd = rot * rd;
    ro = rot * ro;
	
    vec3 light = vec3(-2.0, 2.0, -2.0); //vec3(-2.0*cos(TIME), 2.0*cos(TIME/3.0), -12.0);
    
    
    // ----------------------- RAYMARCH

    vec3 color = vec3(0.0);
    float t = 0.0; float d = 0.0;
    const float max_dist = 32.0;   
    for (float i= 0.;i < 1.0; i += 0.013) //1/75
    {
        
        if (d >= 0.001 && t > max_dist ) break;
        d = map(ro);
	//d = max(d, 0.003);    
        t += d;
        ro += rd*d;
    }
    
   

    // ----------------------- SHADE
    
    if(t < max_dist)
    {
       
    vec3 m = ro + rd *t;
    vec3 bg = (rd * sin( TIME + 36.0 * (1.0-length(ro) )) ) * 0.7 + vec3(0.20) ;
    vec3 normal = computeNormal(m);
    
    // currently under debug    
    vec3 pre = bg; //computeColor(rd, m, normal, light, ro, bg);
    color = step(.5, pre) - pre;
    }
    else 
    {
     // background color	    
     color = rd * 0.42 +vec3(1.0);
    }
    
    //
    gl_FragColor.rgb = pow(color, vec3(1.0/2.2));
    gl_FragColor.a = 1.0;
}
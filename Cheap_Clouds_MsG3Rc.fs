/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "0c7bf5fe9462d5bffbd11126e82908e39be3ce56220d900f633d58fb432e56f5.png"
    }
  ],
  "CATEGORIES" : [
    "procedural",
    "3d",
    "perlin",
    "cheap",
    "volumetric",
    "xor",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MsG3Rc by Xor.  Cheap 3D hacked-together clouds. The code could be cleaned up, but I just wanted this posted.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


vec3 SUN = normalize(vec3(4,2,1));


float p1(vec3 p)
{
    vec3 S = smoothstep(0.,1.,fract(p))-fract(p)*vec3(1,1,0);
    return mix(IMG_NORM_PIXEL(iChannel0,mod((p.xy+S.xy+floor(p.z)*vec2(123,69))/256.,1.0)).r,
               IMG_NORM_PIXEL(iChannel0,mod((p.xy+S.xy+ceil(p.z)*vec2(123,69))/256.,1.0)).r,S.z);	
}
float f1(vec3 p)
{
 	return p1(p/vec3(7,7,3))*(p1(p/9.+.5)*.6+p1(p/5.)*.2+p1(p/3.+.5)*.1+p1(p)*.08+p1(p*2.+.5)*.02);   
}
float volume(vec3 p)
{
    vec3 n = p+TIME*vec3(1,3,.1);
    //2.-length(max(abs(p)-2.,0.));
    return f1(n);
}

void main() {



    //2D Screen coordinates
	vec2 UV = (gl_FragCoord.xy - RENDERSIZE.xy*.5)/RENDERSIZE.y;
    
    //3D Position and direction
    vec2 A = mix(vec2(TIME*.5,3.1416),iMouse.xy/RENDERSIZE.xy*6.2831,sign(iMouse.x+iMouse.y));
    vec3 P = vec3(cos(A.x)*sin(A.y*.5),sin(A.x)*sin(A.y*.5),cos(A.y*.5))*12.;
    vec3 D = normalize(-P);
    
    //Matrix components
    vec3 X = normalize(D);
    vec3 Y = normalize(cross(X,vec3(0,0,1)));
    vec3 Z = normalize(cross(X,Y));
    
    //Current ray direction and position
    vec3 R = normalize(mat3(X,Y,Z) * vec3(1,UV));
    
    //Color
    vec3 S = mix(vec3(1.,.8,.5),vec3(.5,.8,1.),(max(.1-R.z,0.)))+vec3(8,4,2)*max(dot(R,SUN)*4.-3.9,0.);
    vec3 C = S;
    for(float i = 32.;i>0.;i-=.5)
    {
        vec2 V = vec2(volume(P+R*i),volume(P+R*i-SUN));
        vec3 T = mix(vec3(1.,.98,.95),V.x*vec3(.5,.7,.9),smoothstep(-.05,.05,V.x-V.y));
    	C = mix(C,T,min(pow(V.x+.3,8.),1.));
    }
	gl_FragColor = vec4(C,1);
}

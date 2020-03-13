/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WtXXDf by jeyko.  function visualization technique and directional derivative shadows borrowed from IQ's two tweet derivative shader - https:\/\/www.shadertoy.com\/view\/MsfGzM",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


#define pi acos(-1.)


//#define iMouse.x (iMouse.x + sin(TIME))


mat2 rot(float d){return mat2(cos(d),-sin(d), sin(d), cos(d));}



float r11 (float i){
	return fract(sin(i*12414.124124)*sin(i*124124.125125)*11424.5125);
}


float valueNoise (float z){
    float id = floor(z);
    z = fract(z);
	float r1 = r11(id);
    float r2 = r11(id + 1.);
	return smoothstep(r1,r2,z);
}


vec3 background(vec2 uv){
    vec3 c = vec3(0);
    
    uv += 0.2*iMouse.x/RENDERSIZE.y;
    uv *= 9.;
    
    vec2 id = floor(uv);
    
   	uv = mod(uv, 2.);
    
    float r1 = r11(id.x +id.y*212.4141);
    float r2 = r11(id.x + id.y*51.123124);
    float rT = r11(id.x + floor(TIME));
    
    uv.x += 1.;
    float ratio;
    if (r2 < 0.5)
        ratio = 5.;
    else 
        ratio = 2.;    
    
    float dlines = 0.;
    if (r2 < 0.33) {
        
        if (rT < 0.9010101) {uv.x += TIME*r1*2.;}
        
    	dlines = step(fract(uv.x*4.), 0.6);
        
        c += dlines;
        
    } else if (r1 >= 0.33 && r1 < 0.66) {
        
        if (rT < 0.5010101) {uv.x += TIME;}
        uv *= rot(pi/4.);
        
    	dlines = step(fract(uv.x*5.), 0.75);
        
        c += dlines;
	    	
    } else{
        uv *= rot(pi/1.5);
            
        if (rT < 0.5010101) {uv.x += TIME;}
    	
    	dlines = step(fract(uv.x*ratio), 0.6 + ratio*0.02);
        
        c += dlines;
    }
    
    
	return c;
}



float map(vec3 p){
    #define TIME (TIME * 1.)
	float h = 10e9;
    vec3 q = p;
    p.y += sin(p.z*1.)*0.3;
    p.x -= p.z*(1. + sin(iMouse.x/RENDERSIZE.y)*0.1);
    //p.z += 10.*iMouse.x/RENDERSIZE.y;
    for (int i = 0; i < 2; i++){
    	p.z -= 0.2;
        p.xy *= rot(0.4*pi );
        p.yz *= rot(1.6*pi);
        p.xz = abs(p.xz);
    }
	//p.y += 0.4;
    //h = length(.05*cos(29.*p.y*p.x)+cos(p)-.1*cos(9.*(p.z+.3*p.x-p.y)))-0.4;
    
	h = length(
		//+ atan(p.y)*0.7 
        +cos(p*1.  + cos(TIME*0.75)*0.1 )
        +sin(p.z*0.4  + sin(TIME*0.75)*1.62 )*0.1
        
        -cos(p.y * 1. + sin(TIME)*0.02)
        +cos(p.z)
//        -.2*cos(9.*(p.z+.3*p.x-p.y))
    )-1.;
    
    //h += clamp(q.y*0.1 + 0.3,0., 200.);
    //zh += clamp(-q.x*0.2, 0., 100.);
    //h += clamp(p.y - 30.,0., 100.); 
    
    return h/3.;
}




void main() {



    vec3 c = vec3(0);
    
    vec2 uv = (gl_FragCoord.xy - 0.5*RENDERSIZE.xy)/RENDERSIZE.y;
 
    vec3 ro = vec3(0);
   	vec3 rd= normalize(vec3(uv, 1.));
    
    //ro.z  += 100.*iMouse.x/RENDERSIZE.y;
    ro.z +=30.2;
    
    vec3 p = ro;
    float t = 0.;
    
    #define iters 250.
    vec3 bg =  background(uv);
    //bg = clamp(bg, 0., 1.);
    bg = bg*-1. + 1.;
    for (float i = 0.; i < iters; i++){
    	float h = map(p);
        
        if (h < 0.001){
            float diff = clamp(map(p + normalize(vec3(2,0,1))*0.06)/0.01,0.,1.); 
        	//c -= sin(0.5 + 0.4*vec3(p.z*0.5, p.y*0.3, p.x*0.3 + 0.9))*vec3(2.,0.9,1.8);
            p.x -= sin(p.y)*1.5;
            //p.x += sin(p.z*0.7);
            
            p.z += 1. + valueNoise(p.z*2.)*0.1;
            //p.z += clamp(valueNoise(p.z),0., 0.2);
            //c += step(fract((p.z + 1.)*5.), 0.9);
            
            float st = 0.4;
            p.z = fract(p.z*8.);
            if (p.z > 0.5) p.z =  1. - p.z;
            c += smoothstep(st, st*0.7, p.z);
            //c -= smoothstep(fract(abs((p.z*2. - 0.25))*4.), 0.5, 0.6);
            float ratio = clamp(0.1 * t - 5., 0., 1.); 
            c *= (1. - ratio);
            c -= diff*0.3;
            //c += ratio * bg;
            break;
        } 
        if (t > 15.) {
        	c += bg;
            break;
        }
        t += h;
        p = ro + rd*t;
    }
    
    //c += background(uv);
   
    
    
    
    gl_FragColor = vec4(c,1.0);
}

/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtyGzh by sillsm.  Trees with breeze",
  "INPUTS" : [
	{
      "NAME": "t1_wind_amnt",
      "TYPE": "float",
      "DEFAULT": 0.6,
      "MIN": 0.0,
      "MAX": 1.0
    },
    
	{
      "NAME": "t1_speed",
      "TYPE": "float",
      "DEFAULT": 1.3,
      "MIN": 0.0,
      "MAX": 3.0
    },
    {
      "NAME": "t1_x_pos",
      "TYPE": "float",
      "DEFAULT": -5,
      "MIN": -10.0,
      "MAX": 10.0
    },
    {
      "NAME": "t1_y_pos",
      "TYPE": "float",
      "DEFAULT": 0,
      "MIN": -10.0,
      "MAX": 10.0
    },
    {
      "NAME": "t1_len",
      "TYPE": "float",
      "DEFAULT": 1.3,
      "MIN": 0.0,
      "MAX": 6.0
    },
    
    {
      "NAME": "t2_wind_amnt",
      "TYPE": "float",
      "DEFAULT": 0.6,
       "MIN": 0.0,
      "MAX": 1.0
    },
    {
      "NAME": "t2_speed",
      "TYPE": "float",
     "DEFAULT": 0.5,
      "MIN": 0.0,
      "MAX": 3.0
    },
    {
      "NAME": "t2_x_pos",
      "TYPE": "float",
      "DEFAULT": 0,
      "MIN": -10.0,
      "MAX": 10.0
    },
    {
      "NAME": "t2_y_pos",
      "TYPE": "float",
      "DEFAULT": 0,
      "MIN": -10.0,
      "MAX": 10.0

    },
    {
      "NAME": "t2_len",
      "TYPE": "float",
      "DEFAULT": 1.3,
     "MIN": 0.0,
      "MAX": 6.0
    },
    {
      "NAME": "t3_wind_amnt",
      "TYPE": "float",
      "DEFAULT": 0.6,
       "MIN": 0.0,
      "MAX": 1.0
    },
    {
      "NAME": "t3_speed",
      "TYPE": "float",
     "DEFAULT": 2,
      "MIN": 0.0,
      "MAX": 3.0
    },
    {
      "NAME": "t3_x_pos",
      "TYPE": "float",
      "DEFAULT": 5,
      "MIN": -10.0,
      "MAX": 10.0
    },
    {
      "NAME": "t3_y_pos",
      "TYPE": "float",
      "DEFAULT": 0,
      "MIN": -10.0,
      "MAX": 10.0

    },
    {
      "NAME": "t3_len",
      "TYPE": "float",
      "DEFAULT": 1.3,
      "MIN": 0.0,
      "MAX": 6.0
    },
  ]
}
*/


// Copyright Max Sills 2016, licensed under the MIT license.
//
// Real time lindenmayer systems.
// Reference: http://algorithmicbotany.org/papers/abop/abop-ch1.pdf
// Figures d, e, f from pg. 25. "Bracketed OL systems".
// 
// Inspired by Knighty's using base n encodings to explore
// n-ary IFS. Bounding volumes are trash.
//
// Enormous thanks to iq for numerous bugfixes and optimizations!
#define PI 3.14159
#define MAXDEPTH 7

mat3 Rot (float angle)
{
    float c = cos(angle);
    float s = sin(angle);
    
return  mat3(
        vec3(c, s, 0),
        vec3(-s, c, 0),
        vec3(0, 0, 1)
); 
}

mat3 Disp (vec2 displacement)
{
return  mat3(
        vec3(1, 0, 0),
        vec3(0, 1, 0),
        vec3(displacement, 1)
); 
}

float sdCappedCylinder( vec2 p, vec2 h )
{
  p -= vec2(0.,h.y);
  vec2 d = abs(vec2(length(p.x),p.y)) - h;
  return min(max(d.x,d.y),0.0) + length(max(d,0.0));
}

float left(vec2 pt)
{
    mat3 posR = Rot(-(20./360.)*2.*PI);
    mat3 negR = Rot(20./360.*2.*PI);
    
    const int depth = 6;
    const int branches = 3; 
    int maxDepth = int(pow(float(branches) , float(depth )));
    float len = 1.7;
    float wid = .01;
    pt = pt + vec2(0,2);  
    mat3 m1 = posR*Disp(vec2(0,-len));
    
    float trunk = sdCappedCylinder(pt-vec2(0.,0.), vec2(wid,len));
    float d = 500.;
    
    int c = 0; // Running count for optimizations
    
    for (int count = 0; count <= 100; ++count){
      int off = int(pow(float(branches), float(depth)));
        
      vec2 pt_n = pt;
      for (int i = 1; i <= depth; ++i)
      {
        float l = len/pow(2.,float(i));
         
        //
        off /= branches; 
        int dec = c / off;
        int path = dec - branches*(dec/branches); //  dec % kBranches
          
        mat3 mx;
	    if(path == 0){
		  mx=posR*Disp(vec2(0,-2.*l));
	    }
        else if(path == 1){
          mat3 wind = Rot(0.5*sin(TIME + 2.));
          mx = wind*posR * Disp(vec2(0,-4.*l));
	    }
	    else if(path == 2){
          mat3 wind = Rot(0.2*sin(TIME));
          mx = wind*negR * Disp(vec2(0,-4.*l));
	    }
        pt_n = (mx * vec3(pt_n,1)).xy;
        float y = sdCappedCylinder(pt_n, vec2(wid,l));
        
        // Early bail out. Bounding volume is a noodle of radius
        // 2. * l around line segment.
        if( y-2.0*l > 0.0 ) { c += off-1; break; }
          d = min( d, y );
     }
        
    ++c;
    if (c > maxDepth) break;
    }
   return min(d,trunk);
}

float center(vec2 pt)
{
    
    mat3 posR = Rot(-(25.7/360.)*2.*PI);
    mat3 negR = Rot(25.7/360.*2.*PI);
    
    const int depth = 7;
    const int branches = 3; 
    int maxDepth = int(pow(float(branches) , float(depth )));
    float len = 1.7;
    float wid = .01;
    pt = pt + vec2(0,2);  
    mat3 m1 = posR*Disp(vec2(0,-len));
    
    float trunk = sdCappedCylinder(pt-vec2(0.,0.), vec2(wid,len));
    float d = 500.;
    
    int c = 0; // Running count for optimizations
    for (int count = 0; count <= 100; ++count){
      int off = int(pow(float(branches), float(depth)));
      vec2 pt_n = pt;
      for (int i = 1; i <= depth; ++i)
      {
        float l = len/pow(2.,float(i));
          
        
        off /= branches; 
        int dec = c / off;
        int path = dec - branches*(dec/branches); //  dec % kBranches
          
        mat3 mx;
	    if(path == 0){
		  mx = posR*Disp(vec2(0,-2.*l));
	    }
        else if(path == 1){
          mat3 wind = Rot(0.2*sin(TIME+6.2));
          mx = wind*negR*Disp(vec2(0,-2.*l));
	    }
	    else if(path == 2){
          mat3 wind = Rot(0.2*sin(TIME+1.));
          mx = wind*Disp(vec2(0,-4.*l)) ;
	    }
        
        pt_n = (mx * vec3(pt_n,1)).xy;
        float y = sdCappedCylinder(pt_n, vec2(wid,l));   
          
        // Early bail out. Bounding volume is a noodle of radius
        // 2. * l around line segment.
        if( y-2.0*l > 0.0 ) { c += off-1; break; }
          d = min( d, y );
     }
        
    ++c;
    if (c > maxDepth) break;
    }
   return min(d,trunk); 
}

// Primitive shape for the right l-system.
float right_p(vec2 pt, float wid, float len)
{
    mat3 posR = Rot(-(22.5/360.)*2.*PI);
    
    float t1 = sdCappedCylinder(pt, vec2(wid,len));
    vec2 pt_t2 = (posR*Disp(vec2(0,-2.*len))* vec3(pt,1)).xy;
    float t2= sdCappedCylinder(pt_t2, vec2(wid,len/2.));
    return min(t1, t2);
}

float right(vec2 pt,  float speed, float len, float windAmnt)
{
    mat3 posR = Rot(-(22.5/360.)*2.*PI);
    mat3 negR = Rot(22.5/360.*2.*PI);
    
    const int depth = 4;
    const int branches = 4; 
    int maxDepth = int(pow(float(branches) , float(depth )));
    //float len = 1.3;
    float wid = .01;
    pt = pt + vec2(0,2);  
    mat3 m1 = posR*Disp(vec2(0,-len));
    
    float trunk = right_p(pt, wid, len);
    float d = 500.;
    
    int c = 0; // Running count for optimizations
    for (int count = 0; count <= 110; ++count){
    
    int off = int(pow(float(branches), float(depth)));
    vec2 pt_n = pt;
                
      for (int i = 1; i <= depth; ++i)
      {
        float l = len/pow(2.,float(i));
        
        off /= branches; 
        int dec = c / off;
        int path = dec - branches*(dec/branches); //  dec % kBranches
          
        mat3 mx;
	    if(path == 0){
		  mx = negR*Disp(vec2(0,-2.*l));
	    }
        else if(path == 1){
          mat3 wind = Rot(windAmnt*sin((TIME/2.)*speed));
		  mx = wind*negR*Disp(vec2(0,-4.*l));
	    }
        else if(path == 2){
          // This branch overlaps the first, so you don't
          // immediately see its effects.
          mx = Disp(vec2(0,-2.*l));
	    }  
	    else if(path == 3){
          mx = Disp(vec2(0,-2.*l))*posR*Disp(vec2(0,-4.*l));
	    }

         pt_n = (mx* vec3(pt_n,1)).xy; 
         float y = right_p(pt_n, wid, l); 
          
        // Early bail out. Bounding volume is a noodle of radius
        // 2. * l around line segment.
        if( y-3.0*l > 0.0 ) { c += off-1; break; }
          d = min( d, y );
     }
        
    ++c;
    if (c > maxDepth) break;
    }
   return min(d,trunk);
}


void main() {



    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv = uv * 2.0 - 1.0;		
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    uv *= 5.;
    
    gl_FragColor = vec4(0);
    //float l = left(uv - vec2(-5.,0.));
    //float c = center(uv);
    
    
    float l = right(uv - vec2(t1_x_pos,t1_y_pos),t1_speed, t1_len, t1_wind_amnt);
    float c = right(uv- vec2(t2_x_pos,t2_y_pos), t2_speed, t2_len, t2_wind_amnt);
    float r = right(uv - vec2(t3_x_pos,t3_y_pos), t3_speed, t3_len, t3_wind_amnt);
    
    float d = min(r,min(l, c));
    float t = clamp(d, 0.0, .04) * 2.*12.5;
    
    vec4 bg = vec4(0);
    vec4 fg = vec4(.8);
    gl_FragColor = mix(bg, fg, 1.-t);  
}

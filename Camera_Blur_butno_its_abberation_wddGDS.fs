/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : [
        "488bd40303a2e2b9a71987e48c66ef41f5e937174bf316d3ed0e86410784b919.jpg",
        "488bd40303a2e2b9a71987e48c66ef41f5e937174bf316d3ed0e86410784b919_1.jpg",
        "488bd40303a2e2b9a71987e48c66ef41f5e937174bf316d3ed0e86410784b919_2.jpg",
        "488bd40303a2e2b9a71987e48c66ef41f5e937174bf316d3ed0e86410784b919_3.jpg",
        "488bd40303a2e2b9a71987e48c66ef41f5e937174bf316d3ed0e86410784b919_4.jpg",
        "488bd40303a2e2b9a71987e48c66ef41f5e937174bf316d3ed0e86410784b919_5.jpg"
      ],
      "TYPE" : "cube"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wddGDS by JuliaPoo.  i have nothing better to do but procrastinate on actual stuff i need to do.\n\nalso was too lazy to make commons work so here's arnd 3k chars of code duplicated in the 2 buffers\n\nalso bad code. might fix later.",
  "PASSES" : [
    {
      "TARGET" : "BufferA",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {
      "TARGET" : "BufferB",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {

    }
  ],
  "INPUTS" : [

  ]
}
*/


const float END = 20.;
const float ep = 0.001;

mat2 rot(float a){
	return mat2(cos(a), -sin(a), sin(a), cos(a));
}

float smin(float a, float b, float k){
	float f = clamp(0.5 + 0.5 * ((a - b) / k), 0., 1.);
    return (1. - f) * a + f  * b - f * (1. - f) * k;
}

float cube(vec3 p, float b, float r){
    vec3 d = abs(p) - b;
    return length(max(d,0.0)) - r + min(max(d.x,max(d.y,d.z)),0.0);
}

float sphere(vec3 p, float r){
 	return length(p) - r;  
}

float plane( vec3 p, vec4 n )
{
  	return dot(p,n.xyz) + n.w;
}

float light(vec3 p){
    vec3 move = 1.5*vec3(cos(TIME)*sin(TIME), sin(TIME)*sin(TIME), cos(TIME));
    vec3 p_ = p - move; p_.xy *= rot(TIME * 3.); p_.zy *= rot(TIME * 2.);
	return min(cube(p_, .17, 0.), sphere(p + move, .2)); 
}

float obj(vec3 p){
    vec3 pos = p;
    pos.xy *= rot(TIME * .3);
    float cube1 = cube(pos, 1., 0.);
    pos = p;
    pos.zy *= rot(TIME * .4);
    float cube2 = cube(pos, 1., 0.);
    pos = p;
    pos.zx *= rot(TIME * .7);
    float cube3 = cube(pos, 1., 0.);
	return max(max(cube1, cube2), cube3);   
}

float mirror(vec3 p){
    
    return cube(p + vec3(0., 3.2, 0.), 2., 0.);
}

float SDscene(vec3 p){
   
	float obj = obj(p);
    float mirror = mirror(p);
    float light = light(p);
    float d = min(min(obj, mirror), light);
    
    return d;
}

vec3 SDnormal(vec3 p){
    
    //Calculates the normal vector of SDscene
    
    return normalize(vec3(
    SDscene(vec3(p.x+ep,p.y,p.z))-SDscene(vec3(p.x-ep,p.y,p.z)),
    SDscene(vec3(p.x,p.y+ep,p.z))-SDscene(vec3(p.x,p.y-ep,p.z)),
    SDscene(vec3(p.x,p.y,p.z+ep))-SDscene(vec3(p.x,p.y,p.z-ep))
    ));
}

float depth(vec3 ro, vec3 rd, float sig, inout float min_l){
    
    //Returns depth from ro given raydirection
    
    int max=300;
    vec3 p;
    
    float dist=0., d;
    for (int i=0; i<max; i++){
        p = ro + dist*rd;
    	d = SDscene(p)*sig;
        if (light(p) < min_l){ min_l = light(p);}
    if (abs(d)<ep){
        return dist;
    }
    dist += d;
    if (dist > END){
        return END;
    }
  }
}

void ray_mirror(inout vec3 ro, inout vec3 rd, inout float d, inout float min_l, inout vec3 col, inout vec3 scale){
    
    int Nmax = 20, count = 0;
    while (count < Nmax){
        
        ro += SDnormal(ro)*ep*3.;
        rd = normalize(reflect(rd, SDnormal(ro)));
        d = depth(ro, rd, 1., min_l);
        ro += d*rd;
       
        if (abs(mirror(ro)) > ep){break;}
        
        count += 1;
    }
}

void ray_obj(inout vec3 ro, inout vec3 rd, inout float Dglass, inout float d, inout float min_l, inout vec3 col, inout vec3 scale){
    
    int Nmax = 20, count = 0, count2 = 0;
    vec3 p, rd_;
    while (count < Nmax){
        
        //Go into glass
        rd = normalize(refract(rd, SDnormal(ro), 0.6));
        ro -= SDnormal(ro) * ep*3.;
        d = depth(ro, rd, -1., min_l);
        ro += rd * d;
    	Dglass += d;
        
        //internal refraction
    	rd_ = refract(rd, -SDnormal(ro), 1.5);
       	while (length(rd_) < 0.0001 && count2 < Nmax){
            
            rd = normalize(reflect(rd, -SDnormal(ro)));
            ro -= SDnormal(ro) * ep*3.;
            d = depth(ro, rd, -1., min_l);
            ro += d*rd;
            
            Dglass += d;
            rd_ = refract(rd, -SDnormal(ro), 1.5);
            count2 += 1;
        }
  
        if (length(rd_) > 0.0001){rd = normalize(rd_);}
        ro += SDnormal(ro) * ep*3.;
        d = depth(ro, rd, 1., min_l);
        ro += rd * d;
        
        if (abs(obj(ro)) > ep){break;}
        
        //if (mirror(ro) > ep){ break;}
      	
		count += 1;
    }
  	
    vec3 tint = vec3(exp(Dglass*-0.3),exp(Dglass*-0.9),exp(Dglass*-0.9));
    scale *= tint;
    col += pow(clamp(abs(1./min_l)*0.05, 0., 1.), .7) * scale;
    Dglass = 0.;
}


void fresnel(vec3 ro, vec3 rd, inout float refl, inout float refr){
 	   
   	float b = ((1. - 1.5)/(1. + 1.5));
    float r0 = b*b;
    refl = r0 + (1. - r0)*pow((1. - abs(dot(SDnormal(ro), normalize(rd)))), 5.);
    refr = 1.-refl;
    //refl = .5; refr = .5;
}

vec3 render(vec2 uv){
    vec3 col;
    
    //Camera
    float ScreenSize = 4.;
    float shake = 0.3*sin(.3*TIME);
    
    float zoom = 2.5;
    float k = 0.4;
    float osc = sin(TIME*.3); //3.5 + 2.*osc*osc
  	vec3 ro = 4.*vec3(sin(k*TIME), shake, cos(k*TIME)) + vec3(0.,1.,0.);
  	vec3 lookat = vec3(0,0,0);
    
    
  	vec3 fw = normalize(lookat - ro);
  	vec3 r = normalize(cross(vec3(0,1.,0), fw));
  	vec3 up = normalize(cross(fw,r));
  	vec3 scrC = ro + (zoom)*fw;
  	vec3 scrP = scrC + (uv.x*r + uv.y*up) * ScreenSize;
  	vec3 rd = normalize(scrP - ro);
    
    float Dglass, min_l = END;
    float d = depth(ro, rd, 1., min_l);
    ro += d*rd;
    
    vec3 ro_, rd_, scale_;
    float refl, refr;
    vec3 scale = vec3(1.);
    int Nmax = 15, count;
    while (count < Nmax){
        //hits background
        if (d > END - ep){
            col += textureCube(iChannel0,ro).xyz * scale;
            col += pow(clamp(abs(1./min_l)*0.05, 0., 1.), 2.) * scale;
            break;
        }
        
        //hit light
        else if (light(ro) < ep){
        	col += vec3(1.)*scale;
            break;
        }

        //hit obj
        else if (obj(ro) < ep){
            fresnel(ro, rd, refl, refr);
            ro_ = ro; rd_ = rd; scale_ = scale;
            scale *= refr;
            scale_ *= refl;
            ray_obj(ro, rd, Dglass, d, min_l, col, scale);
            ray_mirror(ro_, rd_, d, min_l, col, scale_);
        }

        //hit mirror
        else if (mirror(ro) < ep){
            ray_mirror(ro, rd, d, min_l, col, scale);
        }
        
        else{d = END;}
        
        col += pow(clamp(abs(1./min_l)*0.05, 0., 1.), 2.) * scale;
        count += 1;
    }
    return col;
}



const float END = 20.;
const float ep = 0.001;

mat2 rot(float a){
	return mat2(cos(a), -sin(a), sin(a), cos(a));
}

float smin(float a, float b, float k){
	float f = clamp(0.5 + 0.5 * ((a - b) / k), 0., 1.);
    return (1. - f) * a + f  * b - f * (1. - f) * k;
}

float cube(vec3 p, float b, float r){
    vec3 d = abs(p) - b;
    return length(max(d,0.0)) - r + min(max(d.x,max(d.y,d.z)),0.0);
}

float sphere(vec3 p, float r){
 	return length(p) - r;  
}

float plane( vec3 p, vec4 n )
{
  	return dot(p,n.xyz) + n.w;
}

float light(vec3 p){
    vec3 move = 1.5*vec3(cos(TIME)*sin(TIME), sin(TIME)*sin(TIME), cos(TIME));
    vec3 p_ = p - move; p_.xy *= rot(TIME * 3.); p_.zy *= rot(TIME * 2.);
	return min(cube(p_, .17, 0.), sphere(p + move, .2)); 
}

float obj(vec3 p){
    vec3 pos = p;
    pos.xy *= rot(TIME * .3);
    float cube1 = cube(pos, 1., 0.);
    pos = p;
    pos.zy *= rot(TIME * .4);
    float cube2 = cube(pos, 1., 0.);
    pos = p;
    pos.zx *= rot(TIME * .7);
    float cube3 = cube(pos, 1., 0.);
	return max(max(cube1, cube2), cube3);   
}

float mirror(vec3 p){
    
    return cube(p + vec3(0., 3.2, 0.), 2., 0.);
}

float SDscene(vec3 p){
   
	float obj = obj(p);
    float mirror = mirror(p);
    float light = light(p);
    float d = min(min(obj, mirror), light);
    
    return d;
}

vec3 SDnormal(vec3 p){
    
    //Calculates the normal vector of SDscene
    
    return normalize(vec3(
    SDscene(vec3(p.x+ep,p.y,p.z))-SDscene(vec3(p.x-ep,p.y,p.z)),
    SDscene(vec3(p.x,p.y+ep,p.z))-SDscene(vec3(p.x,p.y-ep,p.z)),
    SDscene(vec3(p.x,p.y,p.z+ep))-SDscene(vec3(p.x,p.y,p.z-ep))
    ));
}

float depth(vec3 ro, vec3 rd){
    
    //Returns depth from ro given raydirection
    
    int max=300;
    vec3 p;
    
    float dist=0., d;
    for (int i=0; i<max; i++){
        p = ro + dist*rd;
    	d = SDscene(p);
    if (abs(d)<ep){
        return dist;
    }
    dist += d;
    if (dist > END){
        return END;
    }
  }
}

vec3 render(vec2 uv, float focus){
    vec3 col;
    
    //Camera
    float ScreenSize = 4.;
    float shake = 0.3*sin(.3*TIME);
    
    float zoom = 2.5;
    float k = 0.4;
    float osc = sin(TIME*.3); //3.5 + 2.*osc*osc
  	vec3 ro = 4.*vec3(sin(k*TIME), shake, cos(k*TIME)) + vec3(0.,1.,0.);
  	vec3 lookat = vec3(0,0,0);
    
    
  	vec3 fw = normalize(lookat - ro);
  	vec3 r = normalize(cross(vec3(0,1.,0), fw));
  	vec3 up = normalize(cross(fw,r));
  	vec3 scrC = ro + (zoom)*fw;
  	vec3 scrP = scrC + (uv.x*r + uv.y*up) * ScreenSize;
  	vec3 rd = normalize(scrP - ro);
    
    float d = depth(ro, rd);
    
    return vec3((d - focus)/END);
}

void main() {
	if (PASSINDEX == 0)	{


	   	
	    //Shader setup
	    vec2 R = RENDERSIZE.xy;
	    vec2 uv = (gl_FragCoord.xy - 0.5*R)/R.x;
	    vec3 col = render(uv);
	   	gl_FragColor = vec4(col,1.);
	    
	}
	else if (PASSINDEX == 1)	{


		//Shader setup
	    vec2 R = RENDERSIZE.xy;
	    vec2 uv = (gl_FragCoord.xy - 0.5*R)/R.x;
	    
	    float focus = 2.;
	    vec3 col = render(uv, focus);
	   	gl_FragColor = vec4(col,1.);
	}
	else if (PASSINDEX == 2)	{


	    
	    //Shader setup
	    vec2 R = RENDERSIZE.xy;
	    vec2 uv2 = gl_FragCoord.xy/R.xy;
	   	float aberr = length(uv2);
	    float degree = IMG_NORM_PIXEL(BufferB,mod(uv2,1.0)).x/20.;
	    vec2 uv_off = distance(uv2, vec2(.5)) * vec2(aberr,aberr*aberr)*degree * R.x/R.y;
	    gl_FragColor = vec4(1);
	    for(int i = 0; i < 3; i++) {
	        vec2 uv = uv2 + uv_off*(1. - float(i) + .5);
	        vec3 col = IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).xyz;;
	        gl_FragColor[i] = col[i];
	    }
	    
	}
}

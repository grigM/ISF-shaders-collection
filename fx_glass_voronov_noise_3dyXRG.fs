/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel1",
      "PATH" : "8de3a3924cb95bd0e95a443fff0326c869f9d4979cd1d5b6e94e2a01f5be53e9.jpg"
    },
    {
      "NAME" : "iChannel0",
      "PATH" : "cbcbb5a6cfb55c36f8f021fbb0e3f69ac96339a39fa85cd96f2017a2192821b5.png"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3dyXRG by MallSerg.  test mask noise",
  "INPUTS" : [
  	{
     	"NAME" : "inputImage",
      	"TYPE" : "image"
    },
     
    	
     {
      		"NAME" : "noise_size",
      		"TYPE" : "float",
      		"DEFAULT" : 5.0,
      		"MAX" : 30.0,
      		"MIN" : 0.0
    	},
    	{
      		"NAME" : "animate",
      		"TYPE" : "bool",
      		"DEFAULT" : 0
    	},
    	
    	{
      		"NAME" : "animate_speed",
      		"TYPE" : "float",
      		"DEFAULT" : 1.0,
      		"MIN" : 0.0,
      		"MAX" : 3.0
    	},
    	{
      		"NAME" : "blink",
      		"TYPE" : "bool",
      		"DEFAULT" : 1
    	},

    	
     {
      		"NAME" : "refraction",
      		"TYPE" : "float",
      		"DEFAULT" : 0.0810,
      		"MIN" : 0.0,
      		"MAX" : 0.3
    	},

 		{
      		"NAME" : "difraction",
      		"TYPE" : "float",
      		"DEFAULT" : 0.0160,
      		"MIN" : 0.0,
      		"MAX" : 0.2
    	},
    	
    	
    
    {
      		"NAME" : "full_fill",
      		"TYPE" : "bool",
      		"DEFAULT" : 0
    	},
    	{
      		"NAME" : "xpos",
      		"TYPE" : "float",
      		"DEFAULT" : 0.5,
      		"MIN" : 0.0,
      		"MAX" : 1.0
    	},
    	{
      		"NAME" : "fill_width",
      		"TYPE" : "float",
      		"DEFAULT" : 0.6,
      		"MIN" : 0.0,
      		"MAX" : 4.0
    	},
    	
    
    	{
      		"NAME" : "rotation",
      		"TYPE" : "float",
      		"DEFAULT" : 0.0,
      		"MAX" : 1.0,
      		"MIN" : -1.0
    	},
  ]
}
*/



#define PI 3.14159
// Поворот блика
//#define RB 0.19 
// Глубина шума граней
//#define NG 15.0
// koficent vremeni
#define KV 0.012
// Коф преломления
//#define KP 0.0810
// Коф дифракции цветовое смещение
//#define KS 0.0160

vec2 rand( vec2 p ) {
    
    vec2 rnd = fract(
        sin( vec2(
            dot(p,vec2(127.1,311.7)),
            dot(p,vec2(269.5,183.3))
        ))*43758.5453);
    return rnd;
}
vec3 gmain(in vec2 fragCoord) {
	vec2 st = fragCoord.xy/RENDERSIZE.xy;
	st.x *= RENDERSIZE.x/RENDERSIZE.y;
    //vec2 mouse;
    //vec2 mouse = iMouse.xy/RENDERSIZE.xy;
    //mouse = mouse/.5 + vec2(-1.,-1.);
    
    //mouse = translate
    
	vec3 color = vec3(.0);
	st *= noise_size;
	vec2 i_st = floor(st);
	vec2 f_st = fract(st);
	float m_dist = 1.;
	vec2 m_point;
	

	for(int y=-1; y <=1; y++){
		for(int x=-1; x <= 1; x++){
			vec2 neighbor = vec2(float(x), float(y));
			
			vec2 point = rand(i_st + neighbor);
			if(animate){
				point = 0.5 + 0.5*sin(((TIME*.3)*animate_speed) + 6.2831*point);
				//point = 0.5 + 0.5*sin((iMouse.x*.03+(TIME*animate_speed)*KV) + 6.2831*point);
			}
            //fract(TIME*KV);
            point = 0.5 + 0.5*sin(((xpos*RENDERSIZE.x)*.03) + 6.2831*point);
            
			
			vec2 diff = neighbor + point - f_st;
			
            //diff = mix(diff,mouse,.5);
                
            // Distance to the point
			float dist = length(diff)*0.43;
			if(dist < m_dist){
				m_dist = dist;
				m_point = point;
			}
		}
	}
    
    
    
	//color += dot(m_point, vec2(.3,.6));
    vec2 normalPointXY = fragCoord.xy/RENDERSIZE.xy;
    vec2 ki = vec2(0.5,0.5);
    float kj = distance(normalPointXY.x,xpos+(normalPointXY.y-.5)*rotation);
    
    //fract(TIME*KV)
    
    color = vec3( m_point.x,kj/fill_width,m_dist);//m_point);
    if(full_fill){
    	color = vec3( m_point.x,normalize(sin(TIME))-1.,m_dist);//m_point);
    }
    //color = smoothstep();
	//vec2 pz = gl_FragCoord.xy/RENDERSIZE.xy;
	//vec3 pl = vec3(color.x*mouse.x*2.,color.x*mouse.y,color.y );
	//color += vec3(pz.x-iMouse.x,pz.y-iMouse.y,color.z)*.2;
	//color += 1.-step(0.02, m_dist);
	return color;
}

void main() {



  vec4 outcol;
  vec2 xy = gl_FragCoord.xy/RENDERSIZE.xy;
 
 
	vec4 texColor=IMG_NORM_PIXEL(inputImage,mod(xy,1.0));
            vec3 map=gmain(gl_FragCoord.xy);
        float r = IMG_NORM_PIXEL(inputImage,mod(xy,1.0))[0];
        float g = IMG_NORM_PIXEL(inputImage,mod(xy,1.0))[1];
        float b = IMG_NORM_PIXEL(inputImage,mod(xy,1.0))[2];
    float bs = (1.-map.x);
    
        if ( map.y < .3){
            
        	r =IMG_NORM_PIXEL(inputImage,mod(xy +map.x*refraction,1.0))[0];
        	g =IMG_NORM_PIXEL(inputImage,mod(xy +map.x*(refraction+difraction),1.0))[1];
        	b =IMG_NORM_PIXEL(inputImage,mod(xy +map.x*(refraction+difraction+difraction),1.0))[2];
            
        	//r = mix(IMG_NORM_PIXEL(iChannel1,mod(xy,1.0))[0],r,1.-map.y) ;//map.y/43.;
        	//g = mix(IMG_NORM_PIXEL(iChannel1,mod(xy,1.0))[0],g,1.-map.y);
        	//b = mix(IMG_NORM_PIXEL(iChannel1,mod(xy,1.0))[0],b,1.-map.y);//+= map.y/43.;
            
            //if(map.x < 0.1){ r *= 0.5;g *= 0.5; b *= 0.5;};
            //if(map.x > 0.9){ r *= 1.9;g *= 1.9; b *= 1.9;};
            if(map.x > 0.96 && blink){
                bs *= 20.; // окр 1 - 0 
                if (map.y > .2) bs *= 0.2;
                r += (1.-r)*bs ;g += (1.-g)*bs; b += (1.-b)*bs;
                //if (map.y < .2) bs *= .02;
                //	r *= bs ;g *= bs; b *= bs;
            };
            if(map.x < 0.2){    
                
                r -= (r*.5)*(bs*0.5) ;g -= g*.5*(bs*0.5); b -= b*.5*(bs*0.5);
            };
            
        };
        outcol = vec4(vec3(r,g,b)  ,1.0);
    
    
    /*
    if ((texColor.x+texColor.y+texColor.z) < 2.9  ) { // все что не белое или альфа
        
        //outcol = vec4(vec3(r,0.,0.)  ,1.);
        //outcol = vec4(map  ,1.);
        
        
        
    }
    else{
    //outcol =vec4(mouse.x,mouse.y,0.0,1.);
        //outcol=vec4(1.0,1.0,1.0,1.0);
        outcol=IMG_NORM_PIXEL(iChannel1,mod(xy,1.0))*.15;
        
    };
    */
    //texColor *= 0.42;
    
    gl_FragColor=outcol;
}

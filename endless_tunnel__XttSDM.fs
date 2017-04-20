/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "tunnel",
    "good",
    "pow",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XttSDM by nabr.  basics",
  "INPUTS" : [

  ]
}
*/


//nabr
//License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

void main()
{
    vec2 pos =-1.+2.*(gl_FragCoord.xy/RENDERSIZE.y)-vec2(.8,-.1);
      
    	 pos*=214.;
 
    vec2 cen = vec2(0.5)-pos; 
    float angle = atan(cen.y,cen.x);
    float radius = length(cen)*-TIME;
    float color;
    (sin(TIME)/3.<.2)?
    color = distance((angle+300.),radius*5.)/2./length(length(cen))
    : color = distance((angle+600.),radius*125.)/20./length(length(cen));
    
    float newp=0.1; 
    vec2 rot  = vec2(0.1);
    for (float i=1.0; i<=55.0; i+=5.)
		{  
         rot += vec2(6.*cos(i/5.+(TIME)*3.0/2.0),6.*sin(i/15.-cos(TIME)*-3.0/2.0)); 
         
    	 newp +=2.2/distance(pos-rot.yx+150.*cos(TIME),-pos/(rot.x)+105.+sin(TIME)) 
             	+.15/distance(cen+rot.yx,cen/sin(rot.y))*3.;  
         	
         cen.x = (cos(color)-newp)*-(cos(color)/newp)
                    /pow(cos(angle)+newp/tan(TIME),2.);
         cen.y *=fract(sin(color-TIME))/1.2;
		}
   
    gl_FragColor = mix(vec4(vec3(newp),1.),vec4(vec3(cen.y-.7,0.0-.9,cen.x)/4.,1.0),.75);
 
}
/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "circleraymarching",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4ttSWS by micantre.  Spinning circular tunnel made with ray marching",
  "INPUTS" : [

  ]
}
*/


#define ITER 16.
#define SPEED 1.0
#define ALTERNATE_COLORS false
#define TWIST true

bool isInCircle(vec2 center, float radius, vec2 point)
{
    if(distance(center,point) > radius) return true;
    else return false;
}

void main()
{
	vec2 uv = (gl_FragCoord.xy/RENDERSIZE.xy) * 2. - 1.;
    uv.x*=RENDERSIZE.x/RENDERSIZE.y;
    vec3 color = vec3(0.0);
    vec2 ray;
    for(float z=0.;z<ITER;z+=1.){
        ray = uv*z;
        float var;
        if(TWIST){
            var = (SPEED*TIME+float(z));
        }
        else{
            var = (SPEED*TIME);
        }
        ray.x+=sin(var)/1.5;
        ray.y+=cos(var)/2.0;
        if(isInCircle(vec2(0.),1.0,ray)){
            float col;
            if(ALTERNATE_COLORS){
            	if(mod(  z , 2.) == 0.){ col = 0.; } //black
            	else { col = 1. -z/ITER;} //faded white
            }
            else{
            	col = 1. - z/ITER;
            }
            
            color=vec3(col);
            break;
        }
    }
    gl_FragColor = vec4(color,1.0);
}

// credit to: https://www.shadertoy.com/view/Mt3SRf
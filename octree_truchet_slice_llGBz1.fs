/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llGBz1 by abje.  a 2d slice of a 3d octree truchet",
  "INPUTS" : [

  ]
}
*/


//5 is maybe a little too much, 3 is fine
#define limit 3.0

#define HASHSCALE3 vec3(.6531, .5563, .7498)

#define dot2(p) dot(p,p)

//hash function in hash without sine by Dave_Hoskins
//https://www.shadertoy.com/view/4djSRW
float hash13(vec3 p3)
{
	p3  = fract(p3 * HASHSCALE3);
    p3 += dot(p3, p3.yzx + 19.19);
    return fract((p3.x + p3.y) * p3.z*15.3023+0.434);
}

#define checktree(k)                        \
for (j = 0.0; j < k; j++) {             \
    vec3 h = floor(r*exp2(j))*exp2(-j); \
    float rand = hash13(h+exp2(-j-1.0));\
    if (rand >= 0.5) {                 \
        break;                          \
    }                                   \
}

float squarering(vec3 p, float size) {
    vec2 q = vec2(abs(length(p.xy)-0.5),abs(p.z));
    float len = length(max(q-size,0.0))+min(max(q.x,q.y)-size,0.0);
    return len;
}

float truchet(vec3 p, int type) {
    vec3 q = abs(p-0.5);
	
    vec3 q2 = vec3(min(min(q.x,q.y),q.z),0,max(max(q.x,q.y),q.z));
    q = min(q,q.yzx);
    q2.y = max(max(q.x,q.y),q.z);

    float inside = length(abs(q2.yz-vec2(0.0,0.5)))-0.1667;
    float inside2 = length(abs(max(q2.yz-vec2(0.0,0.5),0.0)))-0.1667;
    float outside = length(abs(q2.yz-vec2(0.5,0.5)))-0.3333;
    
    float len;
    //len = min(len, squarering(p.xyz-vec3(0,0,0.5)));
    //len = min(len, squarering(p.yzx-vec3(1,0,0.5)));
    //len = min(len, squarering(p.zxy-vec3(1,1,0.5)));
    if (type == 0) {
        len = -outside;
    } else if (type == 1) {
        len = inside;
    } else if (type == 2) {
        len = inside2;
    } else if (type == 3) {
        float size = 0.1667-0.1+q2.z*0.2;
        len = squarering(p.xyz-vec3(0,0,0.5),size)*0.9;
        len = min(len, squarering(p.yzx-vec3(1,0,0.5),size)*0.9);
        len = min(len, squarering(p.zxy-vec3(1,1,0.5),size)*0.9);
    }
    
    
    return len;
}

float map(vec3 p) {
    
    vec3 fp;
    vec3 lp;
    float len;
    float i;
    
    //r is the truchet cell you want the random 
    vec3 r = p;
    float j;
    checktree(limit);
    i = j;
    float size = exp2(-i);
    //the position in the bottom left corner of the truchet cell
    fp = floor(p/size)*size;

    //the local position on the truchet cell (always 0-1)
    lp = fract(p/size);
    
    int type = int(hash13((fp+size*0.5)*vec3(0.93,0.89,1.23))*4.0);
    len = truchet(lp,type)*size;
    
    while (i <= limit) {
        //the position in the bottom left corner of the truchet cell
        fp = floor(p/size)*size;
        //the local position on the truchet cell (always 0-1)
        lp = fract(p/size);
        //check for the overlapping black dots
        vec3 p2 = p/size;
        vec3 fp2 = floor(p2-0.5);
        for(int x = -0; x <= 1; x++) {
            for(int y = -0; y <= 1; y++) {
                for(int z = -0; z <= 1; z++) {
                    r = (fp2+vec3(x,y,z))*size;
                    //this branch doesn't do anything, but it skips the random() once
                    if (r != fp)
                    {
                        checktree(i);

                        if (i==j) {
                            vec3 q = abs(p2-fp2-vec3(x,y,z)-0.5);
                            
                            vec3 q2 = vec3(min(min(q.x,q.y),q.z),0,max(max(q.x,q.y),q.z));
                            q = min(q,q.yzx);
                            q2.y = max(max(q.x,q.y),q.z);
                            
                            float outside = length(abs(q2.yz-vec2(0.5,0.5)))-0.3333;
                            
                            len = max(-outside*size,len);
                        }
                    }
                }
            }
        }
        size *= 0.5;
        len *= -1.0;
        i++;
    }
    return len;
    
}

void main() {



    
    vec2 uv = (gl_FragCoord.xy*2.0-RENDERSIZE.xy)/RENDERSIZE.y;
    
    
    gl_FragColor = vec4(map(vec3(uv,TIME*0.1))*RENDERSIZE.y*0.5);
    
    gl_FragColor = sqrt(gl_FragColor);
}

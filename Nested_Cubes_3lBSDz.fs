/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/3lBSDz by omeometo.  Inspired by \"Cube in cube in cube in cube\" by TambakoJaguar: https://www.shadertoy.com/view/tlSXzw, but here the structure nested through the faces is not the surface texture but rather cubes in another space.",
    "IMPORTED": {
    },
    "INPUTS": [
    ]
}

*/


vec4 hash4(int n){
    return fract(sin(vec4(532.894,392.843,402.942,837.098)*float(n+1))*4444.5);
}

void qrot(inout vec3 v, in vec4 q){ 
	v += 2.0*cross(cross(v, q.xyz)+q.w*v, q.xyz);
}

vec3 cube_ray_hit(in float r, in vec4 q, in vec3 c, in vec3 ori, in vec3 rd){
    ori-=c;
    qrot(ori, q);
    qrot(rd, q);
    vec3 h0=-(ori+sign(rd)*r)/rd;
    vec3 h1=-(ori-sign(rd)*r)/rd;
    float t0=max(h0.x, max(h0.y, h0.z));
    float t1=min(h1.x, min(h1.y, h1.z));
    if(t1<t0)return vec3(-1);
    if(t0<1e-9)return vec3(-1);
    
    float u=1.0;
    if(h0.y>h0.x)u=2.0;
    if(h0.z>max(h0.x, h0.y))u=3.0;
    
    vec3 hp=abs(ori+rd*t0);
    
    float d=r-min(max(hp.x, hp.y), min(max(hp.y, hp.z), max(hp.z, hp.x)));
    return vec3(t0, u, d);
}

vec3 osc(in vec3 u){
	u=mod(u, 1.0)*4.0;
    return abs(u-2.0)-1.0;
}

void getColor(out vec4 fragColor, in vec3 pos, in vec3 ray){
    int depth=10;
    int idx=0;
    int ri=0;
    for(int d=0;d<depth;d++){
        float col=0.2;
        mat4 back=mat4(0,0,0,1,col,0,0,1,0,col,0,1,0,0,col,1);
        mat4 edge=mat4(1,1,1,1,1,0,0,1,0,1,0,1,0,0,1,1);

        
        float t=TIME*0.2;
        vec4 u=normalize(hash4(ri));
        vec4 v=vec4(hash4(ri+1));
        v=normalize(v-dot(v,u)*u);
        vec4 q=cos(t)*u+sin(t)*v;
        float r=pow(0.5, float(d))*0.5;
		vec3 c=(0.5-r)*osc(t+hash4(ri+2).xyz)*0.3;
        
        vec3 hit=cube_ray_hit(r, q, c, pos, ray);
        if(hit.y<-0.5){fragColor=back[idx];break;}
        if(hit.z<0.1*r){
            fragColor=edge[idx*int(hit.y)];
            break;
        }else if(d==depth-1){
			fragColor=back[idx*int(hit.y)];        	
        }else{
            //idx=int(hit.y);
            idx=int(hit.y);
            
            ri=ri*3+int(hit.y);
        }
    }
}

void main() {



    vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE.xy)/RENDERSIZE.y;
	
    vec3 pos = vec3(0, 0, 3);
    vec3 eye = -normalize(pos);
    vec3 up = vec3(0.0, 1.0, 0.0);
    up-=dot(up, eye)*eye;
    up=normalize(up);
    vec3 right = cross(eye, up);
    float angle=0.3;
    vec3 ray = eye + (uv.x*right+uv.y*up) * angle;
    ray = normalize(ray);
    getColor(gl_FragColor, pos, ray);
}

void mainVR(out vec4 fragColor, in vec2 fragCoord, in vec3 pos, in vec3 ray){
	getColor(fragColor, pos+vec3(0,0,1), ray);
}

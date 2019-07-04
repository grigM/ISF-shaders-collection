/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "raymarching",
    "snow",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MsG3RW by Emil.  I don't even like snow!!",
  "INPUTS" : [

  ]
}
*/


vec4 cellColor = vec4(0.0,0.0,0.0,0.0);
vec3 cellPosition = vec3(0.0,0.0,0.0);
float cellRandom = 0.0, onOffRandom = 0.0;


float random (vec3 i){
	return fract(sin(dot(i.xyz,vec3(4154895.34636,8616.15646,26968.489)))*968423.156);
}

vec4 getColorFromFloat (float i){
    i *= 2000.0;
    return vec4(normalize(vec3(abs(sin(i+radians(45.0))),abs(sin(i+radians(90.0))),abs(sin(i)))),1.0);
}

vec3 getPositionFromFloat (float i){
    i *= 2000.0;
    return vec3(normalize(vec3(abs(sin(i+radians(45.0))),abs(sin(i+radians(90.0))),abs(sin(i)))))-vec3(0.5,0.5,0.5);
}

float map(vec3 p, float radius, float time){
    //p *= 1.0;
    cellRandom = random(floor((p*0.5)+0.0*vec3(0.5,0.5,0.5)));
    onOffRandom = random(vec3(5.0,2.0,200.0)+floor((p*0.5)+0.0*vec3(0.5,0.5,0.5)));
    cellColor = getColorFromFloat(cellRandom);
    cellPosition = getPositionFromFloat(cellRandom);
    p.x = mod(p.x, 2.0);
    p.y = mod(p.y, 2.0);
    p.z = mod(p.z, 2.0);
    p += 1.0*cellPosition.xyz;
    if(onOffRandom>0.92){
    	return length(p-vec3(1.0,1.0,1.0)) - (0.3*radius*(cellRandom+0.3))+0.01*(sin(time*(2.0*onOffRandom+cellRandom*20.0)));
    } else {
        return 0.95;
    }
}

float trace(vec3 o, vec3 r, float radius, float time){
    float t = 0.5;
    const int maxSteps = 62;
    for (int i = 0; i < maxSteps; i++){ 
        vec3 p = o + r * t;
        float d = map(p, radius, time);
        t += d*0.48;
    }
    return t;
}

void main() {



	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv = uv*2.0 -1.0;
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    vec3 r = normalize(vec3(uv, 0.5+(0.2*sin(TIME))));
    uv += 0.05*random(vec3(uv.xy,1.0));
    r = r*0.8+cross(r,vec3(0.0,1.0,0.0));
    r = r+0.2*cross(r,vec3(-1.4*sin(TIME*0.4+sin(0.7*(uv.x*0.5+0.1*TIME))),-20.0*cos(TIME*0.2),uv.y-2.0*sin(-uv.y)));
    vec3 o;
    o.y = 14.5*TIME+sin(TIME)*2.0;
    
    
    float t = trace(o,r, 0.4, TIME*0.2);
    float tn = clamp(t*-0.05+1.4,0.0,2.0);
    o.y += 900.5;
    t = trace(o+vec3(0.5,6.5,1.9),r, 0.8, TIME*0.8 + 10220.0);
    tn += 0.2*clamp(t*-0.05+1.4,0.0,2.0);/*
    o.y += 1.6;
    t = trace(o,r, 0.2, TIME + 40.0);
    tn += clamp(t*-0.05+1.4,0.0,2.0);
    o.y += 1.7;
    t = trace(o,r, 0.3, TIME + 60.0);
    tn += clamp(t*-0.05+1.4,0.0,2.0);*/
    
    
    gl_FragColor = vec4(tn)*0.3*vec4(1.0,1.0,1.4,1.0)+vec4(0.0,0.5*cos(uv.x*0.6)+0.4*cos((uv.y-2.0)*0.6),1.0*cos(uv.x*0.7)+0.4*cos((uv.y-2.0)*0.6),0.0) ;
	//gl_FragColor = vec4(0.2,0.17,0.1,0.0)*0.8*(uv.y*2.8+1.5)+vec4(0.2,0.08,0.0,0.0)+vec4(fc*vec3(28.0,15.0+-1.0*length(uv+vec2(0.0,1.0)),6.4)*1.2/length(uv+vec2(0.0,1.3))*1.0,1.0);
}

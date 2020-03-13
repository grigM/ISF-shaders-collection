/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/3dt3DM by ankd.  volumetric rendering.\nreference : \"Playing marble\" by guil\nhttps:\/\/www.shadertoy.com\/view\/MtX3Ws",
  "INPUTS" : [

  ]
}
*/


float noise(in float t) {
	float s = sin(t+sin(t*1.1+sin(t*1.2)));
    return s;
}

mat2 rotate(in float r) { float c=cos(r),s=sin(r); return mat2(c, s, -s, c); }

vec2 cmul(in vec2 a, in vec2 b) { return vec2(a.x*b.x - a.y*b.y, a.x*b.y + a.y*b.x); }
vec2 csqr(in vec2 a) { return vec2(a.x*a.x - a.y*a.y, 2.0*a.x*a.y); }

// distance function
float sphere(in vec3 p, in float r) { return length(p) - r; }
float box(in vec3 p, in vec3 b) { vec3 d=abs(p)-b; return length(max(d, 0.))+min(max(d.x,max(d.y,d.z)),0.); }

// get geometry
float geometry(in vec3 p) {
    vec3 q = p;
    vec3 c = vec3(1.);
    //q = mod(q, c) - 0.5*c;
    
    float res = box(q, vec3(0.3));
	return res;
}

vec3 normal(in vec3 p) {
    vec2 e = vec2(1., -1.) * 0.001;
    return normalize(
    	e.xyy*geometry(p+e.xyy) +
    	e.yxy*geometry(p+e.yxy) +
    	e.yyx*geometry(p+e.yyx) +
    	e.xxx*geometry(p+e.xxx)
    );
}

// pattern in geometry
float map(in vec3 p) {
	float res = 0.;
    vec3 c = p;
	for (int i = 0; i < 10; ++i) {
        //p =.7*abs(p)/dot(p,p) -.7;
        p =.7*abs(p)/dot(p,p) -.8*noise(TIME*0.3);
        p.yz= csqr(p.yz);
        p=p.zxy;
        res += exp(-19. * abs(dot(p,c)));
	}
	return res/2.;
}


// ray march to get geometry
vec2 getTMinMax(in vec3 ro, in vec3 rd, in vec2 cminmax) {
    vec2 res = vec2(-1.);
    float thr = 0.001;

    // calc tmin
    float t = cminmax.x;
    for(int i=0;i<128;i++) {
    	float tmp = geometry(ro + rd*t);
        tmp = max(tmp, 0.);
        if(tmp<thr || cminmax.y<t) {
            break;
        }
        t += tmp*0.5;
    }
    if(cminmax.y<t) return vec2(-1.);
    res.x = t;
    
    // calc tmax
    //t += 4.*thr;
    t += 0.05;
    for(int i=0;i<128;i++) {
    	float tmp = geometry(ro + rd*t);
        tmp *= -1.;
        if(tmp<thr || cminmax.y<t) {
            break;
        }
        t += tmp*0.5;
    }
    res.y = cminmax.y<t ? cminmax.y : t;
    return res;
}

// 
vec3 rayMarch(in vec3 ro, in vec3 rd, in vec2 tminmax) {
    vec3 col = vec3(0.);
    
	float t = tminmax.x;
	float dt = (tminmax.y-tminmax.x) / 64.;
    //dt = 0.02;
	for(int i=0;i<16;i++) {
		// march t
        t += dt;
		if(tminmax.y<t) break;
        float c = map(ro + rd*t);
        col = 0.99*col + 0.02*vec3(c*c);
    }
    //col = vec3(acc);
    return col;
}

void main() {



    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    vec2 p = (gl_FragCoord.xy*2. - RENDERSIZE.xy) / min(RENDERSIZE.x, RENDERSIZE.y);
	float time = TIME*0.1;
    // camera
    vec3 ro = vec3(cos(time), 0., sin(time));
    //ro = vec3(0., 0., -time);
    vec3 ta = vec3(0., 0., 0.);
    float cr = 0.3*sin(0.1*TIME);
    vec3 cz = normalize(ta-ro);
    vec3 cx = normalize(cross(cz, vec3(sin(cr), cos(cr), 0.)));
    vec3 cy = normalize(cross(cx, cz));
    vec3 rd = normalize(mat3(cx, cy, cz) * vec3(p, 2.));
    
    // get tmin, tmax
    vec2 tmm = getTMinMax(ro, rd, vec2(0., 10.));
        
    // render
    vec3 col = vec3(0.);
    if(tmm.x<0.){
        col = vec3(1.0);
    } else {
	    col = rayMarch(ro, rd, tmm);
    }
    
    col += 0.3*vec3(tmm.y-tmm.x);
    
    
    // Output to screen
    gl_FragColor = vec4(col,1.0);
}

/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60868.1"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/Wty3Dt
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE
const vec4 iMouse = vec4(0.0);

// --------[ Original ShaderToy begins here ]---------- //
// As always, code is messy.
// inspired by a gif on the web
// and this <3 https://www.youtube.com/watch?v=JURh1XtkR-w&t=94s

// ------- IF GLITCHY OR GREEN -------- //
//
// comment these lines at the end
//     col = clamp(col, vec3(0.01), vec3(1.));
//     col = smoothstep(0.,1.,col);
//

#define dmin(a,b) a.x < b.x ? a : b
#define pmod(a,x) mod(a, x) - x*0.5

#define mx (3.*iTime+20.*iMouse.x/iResolution.x)
#define pi acos(-1.)

#define rot(x) mat2(cos(x),-sin(x),sin(x),cos(x))


//check out variations!
//#define VAR1
//#define VAR2
#define VARENDLESS

vec3 getRd(vec3 ro, vec3 lookAt, vec2 uv){
	vec3 dir = normalize(lookAt - ro);
	vec3 right = normalize(cross(vec3(0,1,0), dir));
	vec3 up = normalize(cross(dir, right));
	return dir + right*uv.x + up*uv.y;
}

float sdBox(vec3 p, vec3 s){
	p = abs(p) - s;
	return max(p.x, max(p.z, p.y));
}
vec3 u;
float zid;
float repD = 1. ;
vec2 map(vec3 p){
	vec2 d = vec2(10e5);
	vec3 o = p;
    zid = floor(p.z/repD);
    p.z = pmod(p.z, repD);
    //d = dmin(d, vec2(length(p) - 1., 2.));
    vec3 q = p;
    u = p*(2. + floor(mod(p.z, repD*4.)/(repD*4.)));
    
    p = abs(p);
    
    p.x -= 2.15;
    p.y -= 2.13;
    p = abs(p);
    p.yz *= rot(zid*pi*0.25);
    p.xy *= rot(0.25*pi);
    
    #ifdef VARENDLESS
    	p = abs(p);
    	p.x -= 0.7 + sin(zid);
    #endif
    //p.zx *= rot(0.25*pi*zid);
   	#ifdef VAR2
    	p.yz *= rot(-0.1);
    #endif
    d = dmin(d, vec2(sdBox(p, vec3(1,2,1)), 1.));
    d.x = max(d.x, -u.z  );
    #ifdef VAR1
    d.x = max(d.x, u.z  );
	#endif
    d.x = max(d.x, u.z - 0.4  );
    d.x *= 0.5;
    return d;
}

vec3 getNormal(vec3 p){
	vec2 t = vec2(0.001, 0);
    return normalize(map(p).x - vec3(
    	map(p - t.xyy).x,
    	map(p - t.yxy).x,
    	map(p - t.yyx).x
    ));
    
}
vec3 glow = vec3(0);

vec2 march(vec3 ro, vec3 rd, inout float t,inout bool hit, inout vec3 p){
	vec2 d;
    
	p = ro + rd;
    
    for (int i = 0; i < 270; i++){
    	d = map(p);
        glow += exp(-d.x);
        if(d.x < 0.001){
        	hit = true;
            break;
        }
        //if (t > 60.){break;}
        t += d.x;
        p = ro + rd*t;
    }
    
	return d;
}

#define pal(a,b,c,d,e) (a + b*sin(c*d + e))

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec2 uv = (fragCoord - 0.5*iResolution.xy)/iResolution.y;

    vec3 col = vec3(0);

    vec3 ro = vec3(0.,0.,0. + mx);
    vec3 lookAt = ro + vec3(0,0,1.);
    //lookAt.x += sin(iTime)*0.1;
    vec3 rd = getRd(ro, lookAt, uv);
//    rd.xy *= 1. - dot(uv,uv)*0.2;
    vec3 p = vec3(0.0);
    float t = 0.; bool hit = false;
    vec2 d = march(ro, rd, t, hit, p);
    
    vec3 n = getNormal(ro + rd.x*t);
    
    if(hit){
    	//col += n;
    	//u.xy *= rot(0.15*pi + iTime);
        if (d.y == 1.){
        	vec3 q = p;
            
            float repD=7.;
            float w = 2.8;
            //q.z *= 9.;
            //q.x *= 2.;
            q *= 2.;
            //q.y *= 3.;
    		zid = floor(p.z/repD);
            
            q = abs(q);
            //q.xy -= pow(abs(sin(iTime*0.4 + zid*3.)), 20.)*2. ;
            q.xy *= rot(zid*pi*0.25);
            //q.zy *= rot(0.25*pi*zid);
            float id = floor(max(abs(q.x)*repD, abs(q.y)*repD)*0.95) + 1.;
            //float sqs = abs((mod(max(abs(q.x), max(abs(q.y),q.z)) , 1.) - 0.5)*repD); 
            float sqs = abs((mod(max(abs(q.x), max(abs(q.y),q.z)) , 1.) - 0.5)*repD); 
            
            w += sin(id*9.)*3.;
            //w = min(w, 2.2);
            //w = max(w, 0.8);
        	col += smoothstep(w,w*0.5, sqs)*pal(0.5,0.5,vec3(8,3,4),id ,iTime*1. + zid*0.6 + 9.*id);
        	col -= abs(sin(id*5. + iTime))*0.4;
        }
    }
    col -= pow(glow*0.03,vec3(5.))*0.08;
    //col -= pow(glow*0.09,vec3(3.))*0.1;
    //col = pow(col, vec3(1.75));
    //col = smoothstep();
    
    //col = pow(col, vec3(1.9,1.1,1.7));
    col = pow(col, vec3(1.0,1.9,1.0));
    //col.b *= 0.8;
    col = clamp(col, 0.01, 1.);
    col = smoothstep(0.,1.,col);
    col.b *= 0.4;
    col *= 1.4;
    //col += pow(glow*0.03,vec3(1.))*0.28;
    fragColor = vec4(col,1.0);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
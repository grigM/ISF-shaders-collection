/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57036.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/wljSRc
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE

// Emulate a black texture
#define texture(s, uv) vec4(0.0)

// --------[ Original ShaderToy begins here ]---------- //
#define pi 3.14159265359
mat2 rot(float a)
{
 return mat2(cos(a), -sin(a), sin(a), cos(a));   
}
float rnd(vec2 p)
{
 return fract(sin(dot(p, vec2(12.9898, 78.233)))*45362.23543);   
}
float circle(vec2 p, float r)
{
    
 float ss = 0.00;
    return 1.0-smoothstep(r-ss, r+ss, length(p));
}
float circle2(vec2 p, float r)
{
    vec2 x2 = vec2(0.5, 0.);
 return circle(p-x2, r) + circle(p+x2, r-0.01);
}
float circle4(vec2 p, float r)
{
    p = fract(p*2.)-0.5;
 return circle(p, r);
}
float sideCircle(vec2 p, float r)
{
    p.x-=0.5;
 return circle(p, r);
}
float halfCircle(vec2 p, float r)
{
    
 return circle(p, r)*step(0., p.x);   
}
float ring(vec2 p, float r)
{
    
 return circle(p, r) - circle(p, r-0.1);
}
float ring2(vec2 p, float r)
{
    vec2 x2 = vec2(0.5, 0.);
 return ring(p-x2, r) + ring(p+x2, r-0.01);
}
float square(vec2 p, float b)
{
 	vec2 st = abs(p);
 	float square = (1.0-step(b, st.x))*(1.-step(b, st.y));
	return square;   
}
float squareRing(vec2 p, float b)
{
 	vec2 st = abs(p);
 	float sq1 = (1.0-step(b, st.x))*(1.-step(b, st.y));
 	float sq2 = (1.0-step(b-0.1, st.x))*(1.-step(b-0.1, st.y));

    return sq1-sq2;   
}
float squareRingS(vec2 p, float b, float s)
{
 	vec2 st = abs(p);
 	float sq1 = (1.0-step(b, st.x))*(1.-step(b, st.y));
 	float sq2 = (1.0-step(b-s, st.x))
        *(1.-step(b-s, st.y));

    return sq1-sq2;   
}
float diagonal(vec2 p)
{
 return step(p.x,p.y);   
}
float diagonal2(vec2 p)
{
    vec2 st = p*rot(pi/2.);   
 float d2 = smoothstep(p.y-0.009, p.y+0.009,p.x)/2.+smoothstep(st.y-0.009, st.y+0.009,p.x)/2.;//+step(0., 1.0-p.x*p.y))/2.;   
 
     return d2;
     }
float diagonal3(vec2 p)
{
    vec2 st = p*rot(pi/2.);
 return smoothstep(p.y-0.009, p.y+0.009,p.x)-
     smoothstep(st.y-0.009, st.y+0.009,p.x);//+step(0., 1.0-p.x*p.y))/2.;   
}
float multiStripes(vec2 p, float n)
{
 return ceil((p.x+0.5)*n)/n;   
}
float stripes(vec2 p, float n)
{
    p = p*rot(pi/2.);
    p.x+=0.05;
 return step(0.5, fract(p.x*n));  
}
float rectCross(vec2 p,float b)
{
 p = abs(p);
     float x = step(b, p.x);
    float y = step(b, p.y);
    return 1.0-x*y;
}
float oppCross(vec2 p,float b)
{
 p = abs(p);
     float x = step(b, p.x);
    float y = step(b, p.y);
    return x*y;
}
float shadeCircle(vec2 p, float r, float n)
{
 float ss = 0.00;
 float l = 1.0-length(p);
 float a = atan(p.y, p.x);
 return smoothstep(r-ss, r+ss, l)*floor(l*n)/n;
}
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = fragCoord/iResolution.xy;
    uv = uv*2.0-1.0;
    uv.x*=iResolution.x/iResolution.y;
	
    //uv+=iTime/8.;
    uv.y+=1.;//versteck ein felher
   uv.x+=iTime/2.;
   //uv/=8.;
    vec3 goodblue = vec3(0.2, 0.3, 0.9);
    vec3 goodred = vec3(0.75, 0.1, 0.1)/.9;
    vec3 goodpurple = vec3(0.4, 0.1, 0.9);
    vec3 okgreen = vec3(0.4, 0.9, 0.5);
    vec3 drkpurple = vec3(0.4, 0., 0.5);
    vec3 cyan = vec3(0., 255., 255.)/255.;
    vec3 brown = vec3(139.0,71.0,38.0)/255.0;
    
    vec2 st = uv;
    
    vec2 ipos = ceil(uv*4.);
    float rn = rnd(ipos);
    float f = rnd(floor(uv*4.));
    uv = fract(uv*4.)-0.5;
    uv = uv*rot(pi/2.*step(0.5,rnd(ipos)));
	
	
    float cphase = rn*512.256;
    if(mod(cphase, 7.)>6.){okgreen = goodred;}
    else if(mod(cphase, 7.)>5.){okgreen = goodpurple;}
    else if(mod(cphase, 7.)>4.){okgreen = goodblue;}
    else if(mod(cphase, 7.)>3.){okgreen = okgreen;}
    else if(mod(cphase, 7.)>2.){okgreen = drkpurple;}
    else if(mod(cphase, 7.)>1.){okgreen = cyan;}
    else {okgreen = brown;}
	
    float shape = 0.0;
    //circle p,r circle2 pr, halfCircle pr, 
    //ring pr, diagonal p, diagonal2 p, diagonal3 p
    if(f>0.95){shape = circle(uv, 0.25+rn/5.);}
    else if(f>0.9){shape = circle4(uv, 0.4);}
    else if(f>0.85){shape = circle2(uv, 0.4);}
    else if(f>0.8){shape = halfCircle(uv, 0.4);}
    else if(f>0.75){shape = ring(uv, 0.4+rn/10.);}
    else if(f>0.7){shape = step(0.5,diagonal2(uv));}
    else if(f>0.65){shape = step(0.5,diagonal(uv));}
    else if(f>0.6){shape = step(0.5,diagonal(uv));}
    else if(f>0.55){shape = sideCircle(uv, 0.4+rn/10.);}
    else if(f>0.5)shape = multiStripes(uv, 3.);
    else if(f>0.45)shape = stripes(uv, 5.);
	else if(f>0.4)shape = ring2(uv, 0.35+rn/10.);
    else if(f>0.35)shape = squareRing(uv, 0.2+rn/5.);
    else if(f>0.3)shape = square(uv, 0.15+rn/10.);
    else if(f>0.25)shape = rectCross(uv, 0.1+rn/10.);
    else if(f>0.2)shape = oppCross(uv, 0.05+rn/10.);
    else if(f>0.15)shape = shadeCircle(uv, 0.6+rn/10., 5.);

    else shape = squareRing(uv, 0.4-rn/5.);

        float tx = vec3(texture(iChannel0, st)).x;
        float tx1 = vec3(texture(iChannel1, st)).x;
        float tx2 = vec3(texture(iChannel2, st)).x;
        float tx3 = vec3(texture(iChannel3, st/4.)).x;

        
        vec3 gold = vec3(215., 152., 15.)/195.-tx;
        vec3 red1 = vec3(165.,42.,42.)/255.-tx1;
        vec3 red2 = vec3(139.,26.,26.)/255.-tx1;
        vec3 red3 = vec3(139., 37., 0.)/255.-tx2;
        
        
    shape = shape*(f/2.+0.5)-squareRingS(uv, 0.5, 0.05);
    shape = clamp(shape, 0., 1.);
        shape = rnd(vec2(shape, ipos.y*ipos.x));
        vec3 col = vec3(0.7);
    
        if(shape > 0.75){
            col = mix(col, okgreen/2., shape); 
        }
    else if(shape > 0.5){
            col = mix(col, okgreen/6., shape); 
        }
    else if(shape > 0.25){
            col = mix(col, okgreen/4., shape); 
        }
    else {col = mix(col, red3, shape);}
        	col = mix(col, vec3(0.2),squareRingS(uv, 0.5, 0.02)); 
  	
        //0.5 + 0.5*cos(iTime+uv.xyx+vec3(0,2,4));

    fragColor = vec4(col,1.0);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
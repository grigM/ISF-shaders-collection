/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60702.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/wty3Dm
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy emulation
#define iTime TIME
#define iResolution RENDERSIZE

// --------[ Original ShaderToy begins here ]---------- //
const int MAX_DIST = 1000;
const float EPSI = 0.009;

float random(vec2 p){
	return(fract(sin(p.x*431.+p.y*707.)*7443.));
}

float noise(vec2 uv){
	 vec2 id = floor(uv*10.);
    vec2 lc = smoothstep(0.,1.,fract(uv*10.));
    
    float a = random(id);
    float b = random(id + vec2(1.,0.));
    float c = random(id + vec2(0.,1.));
    float d = random(id + vec2(1.,1.));
    
    float ud = mix(a,b,lc.x);
    float lr = mix(c,d,lc.x);
    float fin = mix(ud,lr,lc.y);
    return fin;
}

float octaves(vec2 uv){
    float amp = 0.5;
    float f = 0.;
    for(int i =1; i<10+1;i++){
    	f+=noise(uv)*amp;
        uv*=2.;
        amp*=0.5;
    }
    return f;
}
float SDF(vec3 p){
    vec3 spherePos = vec3(8,6,25);
    float sphere = length(p-spherePos)-1.;
    float water = p.y+8.+octaves((p.xz/30.)+(iTime/10.)+sin(length(p.xz*2.))*.04);
    float mindst = min(water,sphere);
    return mindst;
}
           
float rayMarcher(vec3 ro, vec3 rd){
	float tot = 0.;
    for(int i=0;i<MAX_DIST;i++){
    	vec3 p = ro+rd*tot;
        float diff = SDF(p);
        tot+=diff;
        if(diff<EPSI || tot>float(MAX_DIST)){
        	tot = float(i)/float(MAX_DIST-500);
            break;
        }
    }
    return tot;
}

void mainImage( out vec4 fragColor, in vec2 fragCoord ){    
    
    vec2 uv = (fragCoord-0.5*iResolution.xy)/iResolution.x;
	
	vec2 vu = vec2(length(uv), atan(uv.x, uv.y));
	vu.x += abs(uv.x)*(tan(vu.y)*cos(abs(uv.x)*15.)*0.2);
	uv = vu.x*vec2(sin(vu.y), cos(vu.y));
	
	
    vec3 ro = vec3(0,0,-8);
    vec3 rd= normalize(vec3(uv,1.)); 
    vec3 col = vec3(rayMarcher(ro,rd)); 
    fragColor = vec4(col,1.0);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    mainImage(gl_FragColor, gl_FragCoord.xy);
}
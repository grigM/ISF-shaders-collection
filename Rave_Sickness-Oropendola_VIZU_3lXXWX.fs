/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/3lXXWX by ttoinou.  [url=https://youtu.be/XYrRWPAB_o4]Youtube Render[/url]\n[url=https://soundcloud.com/bigmamashouse/rave-sickness-oropendola-stig ]Music on Soundcloud[/url]\n[url=https://www.shadertoy.com/view/XsSfWm ]Previous Shader[/url]",
    "IMPORTED": {
    },
    "INPUTS": [
        {
            "NAME": "iMouse",
            "TYPE": "point2D"
        },
        {
            "NAME": "iChannel1",
            "TYPE": "audio"
        }
    ],
    "PASSES": [
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferA"
        },
        {
        }
    ]
}

*/


// https://www.shadertoy.com/view/XtVGDz

#define buffer iChannel0
#define sound  iChannel1
#define soundRes IMG_SIZE(iChannel1)
#define step 1.0/RENDERSIZE.y
#define inputSound iChannel0
#define fftWidth 		1.0		
#define fftMinBass 		0.00196 	// 1.0/512.0

#define render 0

// fft Options
#if render

#define fftSmoothTime   0.08
#define fftSmooth 		1.3			// smoothness coeff
#define fftPreamp		0.65		// pre amp before dynamic
#define fftBoost  		0.14		// dynamic amp
#define fftAmp			0.65		    // final gain
#define noiseLevel      0.1
#define fftTrebles		2.5
#define fftBass			1.0

#else

#define fftSmoothTime   .7
#define fftSmooth 		1.3			// smoothness coeff
#define fftPreamp		0.65		// pre amp before dynamic
#define fftBoost  		0.14		// dynamic amp
#define fftAmp			0.6			// final gain
#define noiseLevel      0.05
#define fftTrebles		3.0
#define fftBass			1.6

#endif

#define fftRadiusR		3.0/512.0
#define fftRadiusG		fftRadiusR*3.0
#define fftRadiusB		fftRadiusR*3.0*3.0
#define fftSamplesR 	4 // number of iteration for fft sampling, increases quality !
#define fftSamplesG 	fftSamplesR*4
#define fftSamplesB 	fftSamplesR*4*4
#define fftGBGain       1.1

#define to01(x) clamp(x,0.0,1.0)

float remapIntensity(float f, float i){
  //return i;
  // noise level
  i = to01( (i - noiseLevel) / (1.0 - noiseLevel) );
  float k = f-1.0;
  i *= ( fftTrebles - fftBass*k*k ) * fftPreamp;
  // more dynamic
  i *= (i+fftBoost);
    
  return i*fftAmp;
  // limiter, kills dynamic when too loud
  //return 1.0 - 1.0 / ( i*4.0 + 1.0 );
}

float remapFreq(float freq){
 // linear scale
 //return clamp(freq,fftMinBass,1.0);
 // log scale
 return clamp(to01(- log(1.0-freq/1.65 + fftMinBass)),fftMinBass,1.0);
}

float fftR(float f){
    float sum = 0.0;
    float val = 0.0;
    float coeff = 0.0;
    float k = 0.0;
    for( int i = 0; i < fftSamplesR ; i++ ){
        k = float(i)/float(fftSamplesR-1)-0.5;
        coeff = exp(-k*k/(fftSmooth*fftSmooth)*2.0);
		val += texture(sound, vec2( remapFreq(f + k * fftRadiusR)*fftWidth, 0.0) ).r * coeff;
        sum += coeff;
    }
    return remapIntensity(f,val/sum);
}

float fftG(float f){
    float sum = 0.0;
    float val = 0.0;
    float coeff = 0.0;
    float k = 0.0;
    for( int i = 0; i < fftSamplesG ; i++ ){
        k = float(i)/float(fftSamplesG-1)-0.5;
        coeff = exp(-k*k/(fftSmooth*fftSmooth)*2.0);
		val += texture(sound, vec2( remapFreq(f + k * fftRadiusG)*fftWidth, 0.0) ).r * coeff;
        sum += coeff;
    }
    return remapIntensity(f,val/sum)*fftGBGain;
}

float fftB(float f){
    float sum = 0.0;
    float val = 0.0;
    float coeff = 0.0;
    float k = 0.0;
    for( int i = 0; i < fftSamplesB ; i++ ){
        k = float(i)/float(fftSamplesB-1)-0.5;
        coeff = exp(-k*k/(fftSmooth*fftSmooth)*2.0);
		val += texture(sound, vec2( remapFreq(f + k * fftRadiusB)*fftWidth, 0.0) ).r * coeff;
        sum += coeff;
    }
    return remapIntensity(f,val/sum)*fftGBGain*fftGBGain;
}

// Fork of "Pinwheel Vizu" by ttoinou. https://shadertoy.com/view/XsBfDm
// 2019-07-09 14:03:16

#define dx iMouse.x/RENDERSIZE.x
#define dy iMouse.y/RENDERSIZE.y

#define PI (3.14159265359)
#define TWOPI (3.14159265359*2.0)

#define sound iChannel0
#define to01(x) clamp(x,0.0,1.0)

vec4 fft(float freq,float time){
    return texture(sound,vec2(freq,time));
}


float repeat(float x,float y){
    x = mod(x,2.0*y);
    if( x > y ){
        x = 2.0*y - x;
    }
    //return x;
    return mod(x+y,y);
}

// segment.x is distance to closest point
// segment.y is barycentric coefficient for closest point
// segment.z is length of closest point on curve, on the curve, starting from A
// segment.a is approximate length of curve
vec4 segment( vec2 p, vec2 a, vec2 b )
{
  a -= p;
  b -= p;
  vec3 k = vec3( dot(a,a) , dot(b,b) , dot(a,b) );
  float t = (k.x - k.z)/( k.x + k.y - 2.*k.z );
  float len = length(b-a);
    
  t = clamp(t,0.0,1.0);
  return vec4( length(a*(1.-t) + b*t) , t , t*len , len );
}

// https://www.shadertoy.com/view/4djSRW
#define ITERATIONS 4


// *** Change these to suit your range of random numbers..

// *** Use this for integer stepped ranges, ie Value-Noise/Perlin noise functions.
#define HASHSCALE1 .1031
#define HASHSCALE3 vec3(.1031, .1030, .0973)
#define HASHSCALE4 vec4(1031, .1030, .0973, .1099)
//----------------------------------------------------------------------------------------
///  3 out, 2 in...
vec3 hash32(vec2 p)
{
	vec3 p3 = fract(vec3(p.xyx) * HASHSCALE3);
    p3 += dot(p3, p3.yxz+19.19);
    return fract((p3.xxy+p3.yzz)*p3.zyx);
}

vec3 hash3point(vec2 p)
{
    //vec3 col = hash32(p);
    vec3 col = 
            hash32(p*1.25672+vec2(.2,.8))
          * hash32(vec2(p.y,p.x)/3.42464-vec2(.5,.0))
          - hash32(vec2(3.0+p.y,1.2))
    ;
    
    return pow(
        (abs(col)+max(col,0.0))/2.0
        , vec3(.6,.5,.4)
    );
}

float smoothFunction(float k)
{
    return 1.0 / ( 1.0 + k*k );
}

vec3 smoothFunction(vec3 k)
{
    return 1.0 / ( 1.0 + k*k );
}


float coeffDistPoint(vec2 uv,vec2 colPoint,float scale)
{    
    //float dist = length(uv - colPoint) * scale;
    //dist = pow(dist,0.25);
    //dist = 1.0 - smoothstep(0.0,1.0,dist);
    
    vec2 uv_ = (uv - colPoint)*scale*24.0;
    float dist = dot(uv_,uv_);
    return  1.0 / ( 1.0 + dist );
}


vec3 mixColorLine(vec2 uv,vec3 currentCol,vec3 colLine,vec2 lineA,vec2 lineB,float scale)
{
    return mix(
        currentCol , 
        colLine ,
        1.0 - smoothstep(0.0,1.0,(sqrt(       max(segment(uv,lineA,lineB).x * scale , .0)     )))
    );
}

// pointA and pointB are on the same side of the half plane delimited by line (lineA,lineB)
bool pointsOnSameSideOfLine(vec2 pointA,vec2 pointB,vec2 lineA, vec2 lineB)
{
    vec2 n = lineB - lineA;
    n = vec2(n.y,-n.x);
    return  dot(pointA-lineA,n)
          * dot(pointB-lineA,n)
    > 0.0;
}


float viewportMagnify = 2.0;
vec2 screenToViewport(vec2 uv)
{
    return (uv - RENDERSIZE.xy/2.0 ) / min(RENDERSIZE.x,RENDERSIZE.y) * viewportMagnify;
}

vec2 viewportToScreen(vec2 uv,vec2 base)
{
    return (uv - base/4.0) / viewportMagnify * min(RENDERSIZE.x,RENDERSIZE.y) +  RENDERSIZE.xy/2.0;
    //return (uv - RENDERSIZE.xy/2.0 ) / min(RENDERSIZE.x,RENDERSIZE.y) * viewportMagnify;
} 

float det22(vec2 a,vec2 b)
{
    return a.x*b.y - a.y*b.x;
}

struct Pinwheel
{
    vec2 A; // Right angle, divided into 1 acute and 1 obtuse
    vec2 B; // Acute angle, stays acute
    vec2 C; // Obtuse angle, stays obtuse
    
    vec2 D; // on GA
    vec2 E; // on AB
    vec2 F; // on BC, close to B
    vec2 G; // on BC, close to C
};
   
vec3 barycentricCoordinate(vec2 P,Pinwheel T)
{
    vec2 PA = P - T.A;
    vec2 PB = P - T.B;
    vec2 PC = P - T.C;
    
    vec3 r = vec3(
        det22(PB,PC),
        det22(PC,PA),
        det22(PA,PB)
    );
    
    return r / (r.x + r.y + r.z);
}

    
#define EQUERRE_COPY(T,Q) \
    T.A = Q.A; \
    T.B = Q.B; \
    T.C = Q.C;
    
#define EQUERRE_COMPUTE_DEFG(T) \
	T.E = (T.A + T.B)/2.0; \
	T.F = (3.0 * T.B + 2.0 * T.C)/5.0; \
	T.G = (T.B + 4.0 * T.C)/5.0; \
	T.D = (T.G + T.A)/2.0;
    
#define EQUERRE_GET1(T,Q) \
	T.A = Q.F; \
    T.B = Q.B; \
    T.C = Q.E;

#define EQUERRE_GET2(T,Q) \
	T.A = Q.F; \
    T.B = Q.G; \
    T.C = Q.E;

#define EQUERRE_GET3(T,Q) \
	T.A = Q.D; \
    T.B = Q.E; \
    T.C = Q.G;

#define EQUERRE_GET4(T,Q) \
	T.A = Q.D; \
    T.B = Q.E; \
    T.C = Q.A;

#define EQUERRE_GET5(T,Q) \
	T.A = Q.G; \
    T.B = Q.A; \
    T.C = Q.C;

#define EQUERRE_COND_12_345(X,T) \
	pointsOnSameSideOfLine(uv,T.F,T.E,T.G)
 
#define EQUERRE_COND_1_2(X,T) \
	pointsOnSameSideOfLine(uv,T.B,T.E,T.F)

#define EQUERRE_COND_34_5(X,T) \
	pointsOnSameSideOfLine(uv,T.E,T.A,T.G)
        
#define EQUERRE_COND_3_4(X,T) \
	pointsOnSameSideOfLine(uv,T.G,T.E,T.D)
        
#define EQUERRE_CENTER(T) ((T.A+T.B+T.C)/3.0)
        

void main() {
	if (PASSINDEX == 0)	{


	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	
	   	// black by default
	    gl_FragColor = vec4(0.0,0.0,0.0,1.0);
	
	     // FRAMEINDEX == 0 => Reset
	    if( FRAMEINDEX != 0 ){
	        // store current fft
	        if( gl_FragCoord.y <= 1.0 ){
	            
	            float freq = uv.x;
	            float i1,i2,i3;
	
	            i1 = fftR(freq);
	            i2 = fftG(freq);
	            i3 = fftB(freq);
		
	            gl_FragColor.rgb = vec3(i1,i2,i3);
	            gl_FragColor.rgb = mix(texture(buffer,vec2(uv.x,uv.y - step)).rgb,gl_FragColor.rgb,fftSmoothTime);
	
	            #if render
	            //vec3 mean = mix(gl_FragColor.rgb,texture(buffer,vec2(uv.x,uv.y - step)).rgb,fftSmoothTime);
	            
	            /*if( length(mean) > length(gl_FragColor.rgb) ){
	               gl_FragColor.rgb = mean.rgb; 
	            }
	            */
	            //gl_FragColor.rgb = max(gl_FragColor.rgb,mean.rgb);
	            #endif
	            //gl_FragColor.rgb = vec3(0.0);
	            gl_FragColor.a = texture(sound,vec2(freq,1.0)).x;
	            
	        // store previous fft
	        } else if( gl_FragCoord.y < RENDERSIZE.y - 1.0 ) {
	            gl_FragColor=texture(buffer,vec2(uv.x,uv.y - step));
	        }
	    }
	}
	else if (PASSINDEX == 1)	{


	    float tWholeMusic = TIME/378.;
	    gl_FragColor = vec4(1.0);
	    
	    int nbIterations = 6;
	    vec2 base = vec2(2.0,1.0);
	    
	    
	    
	    
	    viewportMagnify = mix(.1,.5,tWholeMusic)/3.2;
	    uv *= viewportMagnify;
	    
	    uv *= mat2(cos(TIME/48.+vec4(0.,1.6,-1.6,0.)));
	    
	    uv += base/3.2;
	    
	    // Base Triangle
	    Pinwheel Tri;
	    Pinwheel Tri_TMP;
	    Tri.A = Tri.B = Tri.C = vec2(0.0);
	    Tri.B.x += base.x;
	    Tri.C.y += base.y;
	    int PinwheelID = 0;
	    
	    for(int i = 0 ; i < nbIterations ; i++)
	    {
	        PinwheelID *= 5;
	        EQUERRE_COMPUTE_DEFG(Tri);
	        
	        if( EQUERRE_COND_12_345(uv,Tri) )
	        {
	            if( EQUERRE_COND_1_2(uv,Tri) )
	            {
	            	EQUERRE_GET1(Tri_TMP,Tri);
	            }
	            else
	            {
	            	EQUERRE_GET2(Tri_TMP,Tri);
	                PinwheelID += 1;
	            }
	        }
	        else if( EQUERRE_COND_34_5(uv,Tri) )
	        {
	            if( EQUERRE_COND_3_4(uv,Tri) )
	            {
	            	EQUERRE_GET3(Tri_TMP,Tri);
	                PinwheelID += 2;
	            }
	            else
	            {
	            	EQUERRE_GET4(Tri_TMP,Tri);
	                PinwheelID += 3;
	            }
	        }
	        else 
	        {
	            EQUERRE_GET5(Tri_TMP,Tri);
	            PinwheelID += 4;
	        }
	        
	        EQUERRE_COPY(Tri,Tri_TMP);
	    }
	    
	    
	    //gl_FragColor.rgb = hash3point(EQUERRE_CENTER(Tri));
	    vec3 v = cos(
	             TIME/vec3(63.,54.,69.)/float(nbIterations)/1.2
	              + vec3(.0,.95,1.22)
	             )
	             * vec3(36.,34.,31.)
	             + vec3(25.,19.,42.);
	    vec3 s = vec3( cos(TIME/1.425)*.5+.5 ,dx,dy);
	    gl_FragColor.rgb = mod(vec3(PinwheelID),v)/(v-1.);
	    gl_FragColor.rgb = mod(gl_FragColor.rgb+s,1.);
	    // interesting variation
	    // but needs tuning in color 
	    //gl_FragColor = sqrt( cos(gl_FragColor*3.14*vec4(1.,2.,3.,1.))*.5+.5 );
	    
	
	    
	    float k = 13.;
	    k = mod(float(PinwheelID),k)/(k-1.);
	    
	    
	    float ma = (fft( repeat( k*3. + .32 , 1.) , .0 ).g - .03 )*2.;
	    
	    float ga = fft(.08,.0).r;
	    float gb = fft(.18,.0).r;
	    float gc = fft(.28,.0).r;
	    float gabc =(ga+gb+gc)/3.0;
	    
	    vec4 col = cos( vec4(3.,5.,7.,1.)*k*.75 + TIME*1. + float(PinwheelID)/19. )*.5+.5;
	    // negative sqrt :(((
	    // canot remove it, it looks very good on my computer ! (but undefined behavir on other computers)
	    col *= sqrt( (fft( repeat( k*2. + .05 ,1.) , .0 ).r - .07 ))*4.5+ga*.5;
	    
	    gl_FragColor = mix(
	        gl_FragColor ,
	        col, //vec4(1.)
	        mix(1.5,-1.5,gabc)
	    );
	           
	    float scale = float(nbIterations);
	    scale = pow(2.0,scale)/viewportMagnify/scale*mix(6.,1.,ma*sqrt(ma));
	    
	        
	        
	    vec3 EquerreColor = vec3(0.);
	    #define OPERATION1(x,y) gl_FragColor.rgb = mixColorLine(uv,gl_FragColor.rgb,EquerreColor,x,y,scale);
	    OPERATION1(Tri.A,Tri.B);
	    OPERATION1(Tri.B,Tri.C);
	    OPERATION1(Tri.C,Tri.A);
	    
	    
	    gl_FragColor.rgb = tanh(
	        pow(gl_FragColor.rgb,vec3(mix(1.,2.,tWholeMusic)))
	        *mix(.8,1.5,tWholeMusic)
	        *vec3(1.,.8,.7)
	    );
	}

}

{
    //float tWholeMusic = TIME/378.;
    
    // 2.35 PAR preview
    if( abs(fragCoord.y*2.-RENDERSIZE.y) > RENDERSIZE.x/2.34 )
    {
        //fragColor = vec4(.0);
        //return;
    }
    
    fragColor.a = 1.;
    vec4 fgCout = vec4(.0);
    
	vec2 uv = screenToViewport(fragCoord.xy );
    vec2 newUV;
    vec3 r = vec3(sqrt(dot(uv,uv)));
    vec3 newR = r - r*r*r*vec3(
         .072
        ,.068 - fft(.8,.0).g*.01
        ,.064 - fft(.6,.0).g*.01
    );
    //+ sin(r*r-1.)*.1 + 0.;
    
    
    //r
    newUV = normalize(uv)*newR.r;
    mainImage2( fgCout, newUV );
    fragColor.r = fgCout.r;
    
    //g
    newUV = normalize(uv)*newR.g;
    mainImage2( fgCout, newUV );
    fragColor.g = fgCout.g;
    
    //b
    newUV = normalize(uv)*newR.b;
    
    mainImage2( fgCout, newUV );
    fragColor.b = fgCout.b;
}

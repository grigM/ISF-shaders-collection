/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "joydivision",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MlG3zm by seani.  Tried to recreate the classic Joy Division album art.\n\nhttps:\/\/en.wikipedia.org\/wiki\/Unknown_Pleasures",
  "INPUTS" : [

  ]
}
*/


//By Sean Irby
//sean.t.irby@gmail.com


vec2 wTopLeft = vec2(-0.5, 0.7);
vec2 wBottomRight = vec2(0.5, -0.7);

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

float noise(vec2 p, float freq ){
    float unit = 1.0/freq;
    vec2 ij = floor(p/unit);
    vec2 xy = mod(p,unit)/unit;
    xy = .5*(1.-cos(3.14*xy));
    float a = rand((ij+vec2(0.,0.)));
    float b = rand((ij+vec2(1.,0.)));
    float c = rand((ij+vec2(0.,1.)));
    float d = rand((ij+vec2(1.,1.)));
    float x1 = mix(a, b, xy.x);
    float x2 = mix(c, d, xy.x);
    return mix(x1, x2, xy.y);
}

float pNoise(vec2 p, int res){
    float persistance = .5;
    float n = 0.;
    float normK = 0.;
    float f = 7.;
    float amp = 0.5;
    int iCount = 0;
    for (int i = 0; i<50; i++){
        n+=amp*noise(p, f);
        f*=2.;
        normK+=amp;
        amp*=persistance;
        if (iCount == res) break;
        iCount++;
    }
    float nf = n/normK;
    return nf*nf*nf*nf;
}

float window(float x, float i)
{
	return smoothstep(wTopLeft.x+0.1, -0.1, x)*(1.0-smoothstep(0.1,wBottomRight.x-0.1, x));
}

void main()
{
	vec2 uv =  2.0*vec2(gl_FragCoord.xy - 0.5*RENDERSIZE.xy)/RENDERSIZE.y;
    float y = wTopLeft.y;
    float inc = ((wTopLeft.y - wBottomRight.y) / 60.0);
    float waveYLast = 0.0;
    float waveY;
    float thickness;
    float ret = 0.0;
    
    //draw waveforms
    if(wTopLeft.x < uv.x && uv.x < wBottomRight.x)
    {
        for(float i = 0.0; i < 60.0; i++)
        {
            //generate waveform from audio fft and perlin noise
            waveY = 2.5*texture2D(iChannel0, vec2(i/50.0, 0.25)).x;
            waveY = waveY*pNoise(vec2(uv.x + TIME/5.0, y), 3)/2.0;

            //apply window function
            waveY = waveY*window(uv.x, i);

            //add offset
            waveY = mix(waveY+y, waveY, 0.5+0.5*sin(TIME*0.6));

            //draw waveform
            float thickness = 5./RENDERSIZE.y + abs(dFdx(waveY))*RENDERSIZE.y*0.0025;
            ret = mix(ret, 1.0, 1.0-smoothstep(0.0, thickness, distance(vec2(uv.x, waveY), uv)));

            
            //mask y values below waveform
            ret = ret*(smoothstep(waveY-thickness/2.0, waveY+thickness/2.0, uv.y));

            //update loop vars
            waveYLast = waveY;
            y -= inc;
        }
    }
    
   
    gl_FragColor = vec4(1.0*ret, 1.0*ret, 1.0*ret, 1.0);
}
/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/tdS3DR by granito.  ..",
  "PASSES" : [
    {
      "TARGET" : "BufferA",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {
      "TARGET" : "BufferB",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {

    }
  ],
  "INPUTS" : [
    {
      "TYPE" : "image",
      "NAME" : "inputImage"
    },
    {
      "TYPE" : "image",
      "NAME" : "inputImage_2"
    }
  ]
}
*/


//Generate UV Offset

#ifdef GL_ES
precision mediump float;
#endif

#define F vec3(.2126, .7152, .0722)

float normpdf(in float x, in float sigma)
{
	return 0.39894*exp(-0.5*x*x/(sigma*sigma))/sigma;
}

//Apply UV Offset

#ifdef GL_ES
precision mediump float;
#endif
//#define F vec3(-1., 1., -1.)

#ifdef GL_ES
precision mediump float;
#endif

void main() {
	if (PASSINDEX == 0)	{


	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
		const int mSize = 11;
		const int kSize = (mSize-1)/2;
		float kernel[mSize];
	    float detect;
	    vec2 vector;
		float sigma = 7.0;
		float Z = 0.0;
		for (int j = 0; j <= kSize; ++j)
		{
				kernel[kSize+j] = kernel[kSize-j] = normpdf(float(j), sigma);
		}
	    for (int j = 0; j < mSize; ++j)
	    {
	        Z += kernel[j];
	    }
	    for (int i=-kSize; i <= kSize; ++i)
	    {
	        for (int j=-kSize; j <= kSize; ++j)
	        {
	            vec3 col = IMG_NORM_PIXEL(inputImage,mod((gl_FragCoord.xy + vec2(float(i),float(j))) / RENDERSIZE.xy,1.0)).rgb;
	            float gray = pow(dot(col, F),0.5);
	            detect += kernel[kSize+j]*kernel[kSize+i] * gray;
	        }
	    }
		
		detect = pow(detect/(Z*Z), 5.);
	    vector = vec2(dFdx(detect), dFdy(detect));
		vec2 combine = mix( vector , IMG_NORM_PIXEL(BufferA,mod(uv + vector,1.0)).rg, .9);
	    gl_FragColor = vec4(combine,0.,0.);
	}
	else if (PASSINDEX == 1)	{


	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    vec2 uvoffset = IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).xy; 
	    uvoffset = normalize(uvoffset) * sin(TIME*1.);
	
	    vec3 outcol = mix( IMG_NORM_PIXEL(inputImage_2,mod(uv + uvoffset * 0.005,1.0)),  IMG_NORM_PIXEL(BufferB,mod(uv + uvoffset * 0.0025,1.0)), 0.8).rgb;
	    float alpha =1.-pow( clamp(  dot(IMG_NORM_PIXEL(inputImage_2,mod(uv + uvoffset * 0.005,1.0)).xyz, F) , 0.,1.), 0.5); 
	    gl_FragColor = vec4(outcol, alpha);
	}
	else if (PASSINDEX == 2)	{


	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    gl_FragColor = IMG_NORM_PIXEL(BufferB,mod(uv,1.0));
	}
}

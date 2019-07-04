/*{
  "CREDIT": "by mojovideotech",
  "CATEGORIES": [
    "noise",
    "lightning"
  ],
  "DESCRIPTION": "flash input triggered lightning",
  "INPUTS": [
    {
      "NAME": "sizeX",
      "TYPE": "float",
      "DEFAULT": 30,
      "MIN": 1,
      "MAX": 199
    },
    {
      "NAME": "mash",
      "TYPE": "float",
      "DEFAULT": 0.5,
      "MIN": 0.001,
      "MAX": 1
    },
    {
      "NAME": "rate",
      "TYPE": "float",
      "DEFAULT": 2.5,
      "MIN": -3,
      "MAX": 3
    },
    {
      "NAME": "shift",
      "TYPE": "float",
      "DEFAULT": 0.5,
      "MIN": 0.125,
      "MAX": 1
    },
    {
      "NAME": "noiseX",
      "TYPE": "float",
      "DEFAULT": 0.45,
      "MIN": 0.1,
      "MAX": 0.9
    },
    {
      "NAME": "X2",
      "TYPE": "bool",
      "DEFAULT": 1
    },
    {
      "NAME": "constant",
      "TYPE": "bool",
      "DEFAULT": 0
    },
    {
      "NAME": "flash",
      "TYPE": "event"
    }
  ]
}*/


// LightningFlash by mojovideotech
// based on www.shadertoy.com/view/Mds3W7
// rand,noise,fmb functions from www.shadertoy.com/view/Xsl3zN

float rand(vec2 n) {
    return fract(sin(dot(n, vec2(12.9898, 4.1414))) * 43758.5453*mash);
}

float noise(vec2 n) {
    const vec2 d = vec2(0.0, 1.0);
    vec2 b = floor(n), f = smoothstep(vec2(0.0), vec2(1.0), fract(n));
    return mix(mix(rand(b), rand(b + d.yx), f.x), mix(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}

float fbm(vec2 n) {
    float total = 0.0, amplitude = -0.125;
    if (constant) { amplitude = 0.5; }
    else if (flash) amplitude += 1.133; 
    for (int i = 0; i < 8; i++) {
        total += noise(n) * amplitude;
        n += n;
        amplitude *= noiseX;
    }
    return total;
}

void main()
{
	float diff, diff2, c, c1, c2;
    vec2 uv = gl_FragCoord.xy * 1.0 / RENDERSIZE.xy;
    uv.xy = uv.yx;
    
    vec2 t = uv * vec2(2.0,1.0) - TIME*rate;
    float ycenter = mix( 0.5, 0.25 + 0.25*fbm( t ), uv.x*4.0);
    ycenter = fbm(t)*shift;
    diff = abs(uv.y - ycenter);
    c1 = 1.0 - mix(0.0,1.0,diff*(200.-sizeX));
    if (X2) 
    {
    	vec2 t2 = (vec2(1,-1) + uv) * vec2(2.0,1.0) - TIME*rate; 
    	float ycenter2 = mix( 0.5, 0.25 + 0.25*fbm( t2 ), uv.x*4.0);
		ycenter2= fbm(t2)*shift;
  		diff2 = abs(uv.y - ycenter2);
    	c2 = 1.0 - mix(0.0,1.0,diff2*(200.-sizeX));
    }
    else c2 = 0.01;
    c = max(c1,c2);
    vec4 col = vec4(vec3(c-0.3,c-0.2,c),1.0);
    
    gl_FragColor = col;
}
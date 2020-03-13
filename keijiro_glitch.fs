/*{
    "CATEGORIES": [
        "Glitch"
    ],
    "CREDIT": "by keijiro",
    "DESCRIPTION": null,
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "DEFAULT": 0.1237,
            "LABEL": "Seed",
            "MAX": 1,
            "MIN": 0,
            "NAME": "_Seed",
            "TYPE": "float"
        },
        {
            "DEFAULT": [
                0,
                0
            ],
            "LABEL": "Drift",
            "MAX": [
                1,
                1
            ],
            "MIN": [
                0,
                0
            ],
            "NAME": "_Drift",
            "TYPE": "point2D"
        },
        {
            "DEFAULT": [
                0,
                0
            ],
            "LABEL": "Jitter",
            "MAX": [
                1,
                1
            ],
            "MIN": [
                0,
                0
            ],
            "NAME": "_Jitter",
            "TYPE": "point2D"
        },
        {
            "DEFAULT": [
                0,
                0
            ],
            "LABEL": "Jump",
            "MAX": [
                1,
                1
            ],
            "MIN": [
                0,
                0
            ],
            "NAME": "_Jump",
            "TYPE": "point2D"
        },
        {
            "DEFAULT": 0,
            "LABEL": "Shake",
            "MAX": 1,
            "MIN": 0,
            "NAME": "_Shake",
            "TYPE": "float"
        },
        {
            "DEFAULT": 0,
            "LABEL": "Block Strength",
            "MAX": 1,
            "MIN": 0,
            "NAME": "_BlockStrength",
            "TYPE": "float"
        },
        {
            "DEFAULT": 4,
            "LABEL": "Block Stride",
            "MAX": 10,
            "MIN": 0,
            "NAME": "_BlockStride",
            "TYPE": "float"
        },
        {
            "DEFAULT": 4.12,
            "LABEL": "Block Seed 1",
            "MAX": 10,
            "MIN": 0,
            "NAME": "_BlockSeed1",
            "TYPE": "float"
        },
        {
            "DEFAULT": 7.41,
            "LABEL": "Block Seed 2",
            "MAX": 10,
            "MIN": 0,
            "NAME": "_BlockSeed2",
            "TYPE": "float"
        },
        {
            "DEFAULT": 0,
            "LABEL": "UV Distort",
            "MAX": 1,
            "MIN": 0,
            "NAME": "_uvDistort",
            "TYPE": "float"
        }
    ],
    "ISFVSN": "2",
    "VSN": null
}
*/




//	adapted from https://github.com/keijiro/Kino/blob/master/Packages/jp.keijiro.kino.post-processing/Resources/Glitch.shader
//	https://github.com/keijiro/Kino/blob/master/Packages/jp.keijiro.kino.post-processing/Resources/Glitch.hlsl




vec3 rgb2hsv(vec3 c)	{
	vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	//vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
	//vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));
	vec4 p = c.g < c.b ? vec4(c.bg, K.wz) : vec4(c.gb, K.xy);
	vec4 q = c.r < p.x ? vec4(p.xyw, c.r) : vec4(c.r, p.yzx);
	
	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

vec3 hsv2rgb(vec3 c)	{
	vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

float FRandom(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main()
{

	vec2 uv = isf_FragNormCoord;

	//
	// Block glitch
	//

	float block_size = 32.0;
	float columns = floor((RENDERSIZE.x) / block_size);

	// Block index
	vec2 block_xy = floor(isf_FragNormCoord * RENDERSIZE.xy / float(block_size));
	float block = (block_xy.y) * float(columns) + (block_xy.x);

	// Segment index
	float segment = (block / _BlockStride);

	// Per-block random number
	float r1 = FRandom(vec2(block     + _BlockSeed1,0.42));
	float r3 = FRandom(vec2(block / 3.0 + _BlockSeed2,0.93));
	float seed = (r1 + r3) < 1.0 ? float(_BlockSeed1) : float(_BlockSeed2);
	float rand = FRandom(vec2(segment + seed,0.5113));

	// Block damage (offsetting)
	block += rand * 20000.0 * float((rand < _BlockStrength));

	// Screen space position reconstruction
	vec2 ssp = vec2(mod(block, float(columns)), block / columns) * block_size;
	ssp.x += mod(isf_FragNormCoord.x * RENDERSIZE.x,block_size);
	ssp.y += mod(isf_FragNormCoord.y * RENDERSIZE.y,block_size);

	// UV recalculation
	uv = mix(uv,fract((ssp + 0.5) / RENDERSIZE.xy),_uvDistort);


	//
	// Basic glitch effects

	// Texture space position
	float tx = uv.x;
	float ty = uv.y;
	
	// Jump
	ty = mix(ty, fract(ty + _Jump.x), _Jump.y);

	// Screen space Y coordinate
	float sy = ty;

	// Jitter
	float jitter = FRandom(vec2(sy + _Seed, TIME)) * 2.0 - 1.0;
	tx += jitter * float(_Jitter.x < abs(jitter)) * _Jitter.y;

	// Shake
	tx = fract(tx + (FRandom(vec2(_Seed,TIME)) - 0.5) * _Shake);

	// Drift
	float drift = sin(ty * 2.0 + _Drift.x) * _Drift.y;

	// Input sample
	float sx1 = (tx        );
	float sx2 = (tx + drift);
	vec2 coord = vec2(sx1, sy);
	vec4 c1 = IMG_NORM_PIXEL(inputImage, coord);
	coord = vec2(sx2, sy);
	vec4 c2 = IMG_NORM_PIXEL(inputImage, coord);
	vec4 c = vec4(c1.r, c2.g, c1.b, c1.a);


	// Block damage (color mixing)
	if (fract(rand * 1234.0) < _BlockStrength * 0.1)
	{
		vec3 hsv = rgb2hsv(c.rgb);
		hsv = hsv * vec3(-1.0, 1.0, 0.0) + vec3(0.5, 0.0, 0.9);
		c.rgb = hsv2rgb(hsv);
	}


	gl_FragColor = c;
}
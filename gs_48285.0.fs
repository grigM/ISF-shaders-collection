/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
			"NAME": "charWidth",
			"TYPE": "float",
			"DEFAULT":  0.01,
			"MIN": 0.0053,
			"MAX": 0.02
			
		},
		{
			"NAME": "charHeight",
			"TYPE": "float",
			"DEFAULT":  0.02,
			"MIN": 0.003,
			"MAX": 0.05
			
		},
		{
			"NAME": "charMax",
			"TYPE": "float",
			"DEFAULT": 80.0,
			"MIN": 0.0,
			"MAX": 200.0
			
		},
		{
			"NAME": "tabMax",
			"TYPE": "float",
			"DEFAULT": 9.0,
			"MIN": 0.0,
			"MAX": 20.0
			
		},
		{
			"NAME": "charsPerTab",
			"TYPE": "float",
			"DEFAULT": 4.0,
			"MIN": 0.0,
			"MAX": 15.0
			
		},
		{
			"NAME": "jitter",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 5.0
			
		},
		{
			"NAME": "warp",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
			
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT":  0.2,
			"MIN": 0.1,
			"MAX": 5.0
			
		},
		{
			"NAME": "columns",
			"TYPE": "float",
			"DEFAULT":  5.0,
			"MIN": 1.0,
			"MAX": 5.0
			
		}
		
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#48285.0"
}
*/


/*
 * Original shader from: https://www.shadertoy.com/view/ltSGDW
 */

#ifdef GL_ES
precision mediump float;
#endif

// glslsandbox uniforms

// shadertoy globals
float iTime;
vec3  iResolution;

// --------[ Original ShaderToy begins here ]---------- //
//const float charWidth = 0.0053;

//const float charMax = 200.0;
//const float tabMax = 12.0;
//const float charsPerTab = 4.0;


//const float jitter = 0.;
//const float warp = 0.0;

const mat2 m = mat2(1.616, 1.212, -1.212, 1.616);

//	Hash function adapted from David Hoskins:
//	https://www.shadertoy.com/view/4djSRW

float hash12(vec2 p) {
	p = fract(p * vec2(5.3983, 5.4427));
    p += dot(p.yx, p.xy + vec2(21.5351, 14.3137));
	return fract(p.x * p.y * 95.4337);
}

vec2 hash22(vec2 p) {
	p = fract(p * vec2(5.3983, 5.4427));
    p += dot(p.yx, p.xy +  vec2(21.5351, 14.3137));
	return fract(vec2(p.x * p.y * 95.4337, p.x * p.y * 97.597));
}

float noise12(vec2 p) {
    vec2 i = floor(p);
    vec2 f = fract(p);
	vec2 u = f * f * (3.0 - 2.0 * f);
    return 1.0 - 2.0 * mix(mix(hash12(i + vec2(0.0, 0.0)), 
                               hash12(i + vec2(1.0, 0.0)), u.x),
                           mix(hash12(i + vec2(0.0, 1.0)), 
                               hash12(i + vec2(1.0, 1.0)), u.x), u.y);
}

vec2 noise22(vec2 p) {
    vec2 i = floor(p);
    vec2 f = fract(p);
	vec2 u = f * f * (3.0 - 2.0 * f);
    return 1.0 - 2.0 * mix(mix(hash22(i + vec2(0.0, 0.0)), 
                               hash22(i + vec2(1.0, 0.0)), u.x),
                           mix(hash22(i + vec2(0.0, 1.0)), 
                               hash22(i + vec2(1.0, 1.0)), u.x), u.y);
}

float fbm12(vec2 p) {
    float f = noise12(p); p = m * p;
    f += 0.5 * noise12(p); p = m * p;
    f += 0.25 * noise12(p); p = m * p;
    f += 0.125 * noise12(p); p = m * p;
    f += 0.0625 * noise12(p);
    return f / 1.9375;
}

vec2 fbm22(vec2 p) {
    vec2 f = noise22(p); p = m * p;
    f += 0.5 * noise22(p); p = m * p;
    f += 0.25 * noise22(p); p = m * p;
    f += 0.125 * noise22(p); p = m * p;
    f += 0.0625 * noise22(p);
    return f / 1.9375;
}

void mainImage(out vec4 fragColor, in vec2 fragCoord) {
	
	float screenWidth = iResolution.x / iResolution.y;
    vec2 uv = fragCoord.xy / iResolution.y;
    
    float green = 0.0;
    for (int i = 0; i < int(columns); ++i) {
        float id = float(i);

        float x = uv.x - (screenWidth/columns) * id;
        float y = uv.y - speed * iTime + jitter * fbm12(vec2(0.2 * iTime, id));

        vec2 w = fbm22(0.2 * uv + vec2(0.1 * iTime, 4.0 * id));
        x += warp * w.x;
        y += warp * w.y;

        float charX = mod(x, charWidth) / charWidth;
        float charY = mod(y, charHeight) / charHeight;
        float col = x / charWidth;
        float row = floor(y / charHeight);

        float rowHash = hash12(vec2(row, id));
        float rowWidth = 1.0 + floor(charMax * rowHash * rowHash);
        float rowIndent = charsPerTab * floor(tabMax * abs(fbm12(vec2(0.15 * row, id))));

        float char = -0.5 + 4.0 * noise12(vec2(4.0 * col, 6.0 * y / charHeight));
        char *= smoothstep(0.0, 0.4, charX) * smoothstep(1.0, 0.6, charX);
        char *= smoothstep(0.1, 0.4, charY) * smoothstep(1.0, 0.7, charY);
        char *= step(rowIndent, col);
        char *= step(col, rowIndent + rowWidth);
        
        green += clamp(char, 0.0, 1.0);
    }
    fragColor = vec4(green, green, green, 1.0);
}
// --------[ Original ShaderToy ends here ]---------- //

void main(void)
{
    iTime = TIME;
    iResolution = vec3(RENDERSIZE, 0.0);

    mainImage(gl_FragColor, gl_FragCoord.xy);
}
/*{
	"CREDIT": "by joshpbatty",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"XXX"
	],
	"INPUTS": [
		{
			"NAME": "scrollSpeed",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": -0.3,
			"MAX": 0.3
		},
		{
			"NAME": "dimensions",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 1.0,
			"MAX": 8.0
		},
		{
			"NAME": "fade_amp",
			"TYPE": "float",
			"DEFAULT": 0.05,
			"MIN": 0.05,
			"MAX": 0.9
		}
	]
}*/

//https://www.shadertoy.com/view/4lscz8

#define PI 3.1415926535

float random2d(vec2 n) { 
    return fract(sin(dot(n, vec2(129.9898, 4.1414))) * 2398.5453);
}

vec2 getCellIJ(vec2 uv, float gridDims){
    return floor(uv * gridDims)/ gridDims;
}

vec2 rotate2D(vec2 position, float theta)
{
    mat2 m = mat2( cos(theta), -sin(theta), sin(theta), cos(theta) );
    return m * position;
}

//from https://github.com/keijiro/ShaderSketches/blob/master/Text.glsl
float letter(vec2 coord, float size)
{
    vec2 gp = floor(coord / size * 7.); // global
    vec2 rp = floor(fract(coord / size) * 7.); // repeated
    vec2 odd = fract(rp * 0.5) * 2.;
    float rnd = random2d(gp);
    float c = max(odd.x, odd.y) * step(0.5, rnd); // random lines
    c += min(odd.x, odd.y); // fill corner and center points
    c *= rp.x * (6. - rp.x); // cropping
    c *= rp.y * (6. - rp.y);
    return clamp(c, 0., 1.);
}

void main() {
	vec2 uv = isf_FragNormCoord.xy;
	
	//correct aspect ratio
    uv.x *= RENDERSIZE.x/RENDERSIZE.y;

    float t = TIME;
    //float scrollSpeed = -0.3;
    float dims = dimensions;
    //int maxSubdivisions = 2;
    
    //uv = rotate2D(uv,PI/12.0);
    uv.y -= TIME * scrollSpeed;
    
    float cellRand;
    vec2 ij;

    for(int i = 0; i <= 2; i++) { 
        ij = getCellIJ(uv, dims);
        cellRand = random2d(ij);
        dims *= 2.0;
        //decide whether to subdivide cells again
        float cellRand2 = random2d(ij + 454.4543);
        if (cellRand2 > 0.3){
        	break; 
        }
    }
       
    //draw letters    
    float b = letter(uv, 1.0 / (dims));
	
    //fade in
    float scrollPos = TIME*scrollSpeed + 0.15;
    float showPos = -ij.y + cellRand;
    float fade = smoothstep(showPos ,showPos + fade_amp, scrollPos );
    b *= fade;
    
        
    //hide some
    //if (cellRand < 0.1) b = 0.0;
    
    gl_FragColor = vec4(vec3(b), 1.0);
}
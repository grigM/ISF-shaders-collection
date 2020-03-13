/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/tdd3DS by khoba.  Just a writhing mass of cells",
    "IMPORTED": {
    },
    "INPUTS": [
        {
            "NAME": "iMouse",
            "TYPE": "point2D"
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


#define S 13
#define O 6
#define D 8

const int p[S*S] = int[S*S](
	0,0,0,0,0,7,7,7,0,0,0,0,0,
    0,0,0,7,7,6,6,6,7,7,0,0,0,
    0,0,7,6,6,5,5,5,6,6,7,0,0,
    0,7,6,5,5,4,4,4,5,5,6,7,0,
    0,7,6,5,4,3,3,3,4,5,6,7,0,
    7,6,5,4,3,2,2,2,3,4,5,6,7,
    7,6,5,4,3,2,1,2,3,4,5,6,7,
    7,6,5,4,3,2,2,2,3,4,5,6,7,
    0,7,6,5,4,3,3,3,4,5,6,7,0,
    0,7,6,5,5,4,4,4,5,5,6,7,0,
    0,0,7,6,6,5,5,5,6,6,7,0,0,
    0,0,0,7,7,6,6,6,7,7,0,0,0,
    0,0,0,0,0,7,7,7,0,0,0,0,0
);

int[D] sums(in vec2 fragCoord) {
    int s[D];
    int ij = 0;
    for(int i = -O; i <= O; i++) {
        for(int j = -O; j <= O; j++) {
            float tex = IMG_NORM_PIXEL(BufferA,mod((fragCoord + vec2(i, j)) / RENDERSIZE.xy,1.0)).w;
            s[p[ij++]] += int(ceil(tex));
        }
    }
    return s;
}

vec4 solu(in vec2 fragCoord, in vec2 uv) {
    
    int[8] s = sums(fragCoord);
    bool alive = s[1] > 0;
    
    int s1 = s[2] + s[4];
    int s2 = s[6] + s[7];
    int s3 = s[4] + s[7];
    if(s1 >= 10 && s1 <= 13) {
    	alive = true;
    }
    if(s3 >= 20 || s2 <= 10 || s2 > 23) {
    	alive = false;
    }
    
    vec3 prev = IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).xyz;
    float avg = (4. * float(s[1]) + 2. * float(s[2]) + 10. * float(s[3])) / 32.;
    return vec4(vec3(avg) * .1 + prev.x * .9, float(alive));
}

// <--- Click to refresh
void main() {
	if (PASSINDEX == 0)	{
	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    if(FRAMEINDEX <= 1 || iMouse.z > 0.) {
	        bool alive = fract(sin(gl_FragCoord.x) * cos(gl_FragCoord.y) * 43758.5453123) < .22;
	        gl_FragColor = vec4(0., 0., 0., float(alive));
	    } else {
	        gl_FragColor = solu(gl_FragCoord.xy, uv);
	    }
	}
	else if (PASSINDEX == 1)	{
	    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	    gl_FragColor = vec4(IMG_NORM_PIXEL(BufferA,mod(uv,1.0)).xyz, 1.);
	}

}

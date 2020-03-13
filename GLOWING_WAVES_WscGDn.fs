/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/WscGDn by alro.  Sine waves with glow\n\nSee [url]https://www.shadertoy.com/view/3s3GDn[/url] for the glow effect",
    "IMPORTED": {
    },
    "INPUTS": [
    ]
}

*/


// Glowing sine waves
// See https://www.shadertoy.com/view/3s3GDn for comments on the glow

float time;

float getWaveGlow(vec2 pos, float radius, float intensity, float speed,
                  float amplitude, float frequency, float shift){
    
	float dist = abs(pos.y + amplitude * sin(shift + speed * time + pos.x * frequency));
    dist = 1.0/dist;
    dist *= radius;
    dist = pow(dist, intensity);
    return dist;
}

void main() {

    
    vec2 uv = gl_FragCoord.xy/RENDERSIZE.xy;
    float widthHeightRatio = RENDERSIZE.x/RENDERSIZE.y;
    vec2 centre = vec2(0.5, 0.5);
    vec2 pos = centre - uv;
    pos.y /= widthHeightRatio;
    
    time = TIME * 0.3;
    
    float intensity = 0.9;
    float radius = 0.03;
    
    vec3 col = vec3(0.0);
    float dist = 0.0;
   
    dist = getWaveGlow(pos, radius,intensity, 2.0, 0.01, 3.7, 0.0);
	col += dist * mix(vec3(0.1,0.1,0.5), vec3(0.5,0.1,0.1), 0.5 + 0.5*cos(4.0*time));
 	dist = getWaveGlow(pos, radius, intensity, 4.0, 0.01, 6.0, 2.0);
	col += dist * mix(vec3(0.5,0.1,0.5), vec3(0.5,0.5,0.1), 0.5 + 0.5*cos(3.0*time));
	dist = getWaveGlow(pos, radius*0.5, intensity, -5.0, 0.01, 4.0, 1.0);
	//Use time varying colours from the basic template
	//Add it to vec3(0.1) to always have a bright core
	col += dist * (vec3(0.1) + 0.5 + 0.5*cos(time+vec3(0,2,4)));
    
    //See comments in https://www.shadertoy.com/view/3s3GDn
    col = 1.0 - exp(-col);
    
    // Output to screen
    gl_FragColor = vec4(col, 1.0);
}

/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/3sd3W8 by gal_eon.  Just 2d metaballs over a sinewaves background",
    "IMPORTED": {
    },
    "INPUTS": [
    	{
            "NAME": "speed",
            "TYPE": "float",
           "DEFAULT": 1.0,
            "MIN": 0.0,
            "MAX": 3.0
        },
    ]
}

*/


// Galen Ivanov

void main() {



    vec2 uv = (gl_FragCoord.xy-.5*RENDERSIZE.xy)/RENDERSIZE.y;
    uv *= 2.;
    vec3 col = vec3(.5+.7*sin(90.*(.40*(TIME*speed)+uv.y+cos(.5*TIME+.23*uv.x)*sin(.71*(TIME*speed)+uv.x+uv.y))));
    
    
    vec2 p1 = vec2(.3*sin((TIME*speed)), .3*cos((TIME*speed)));
    vec2 p2 = vec2(.7*sin(2.*(TIME*speed)), -.5*cos(3.*(TIME*speed)));
    vec2 p3 = vec2(.9*cos(1.53*(TIME*speed)), .6*sin(2.*TIME));
    vec2 p4 = vec2(1.1*sin(1.3*(TIME*speed)), .4*cos(2.1*(TIME*speed)));
    
    float mask = length(uv-p1);
    mask *= length(uv-p2);
    mask *= length(uv-p3);
    mask *= length(uv-p4);
    mask *= 5. + 4.*sin(10.*(TIME*speed));
    col = smoothstep(.8, .81, col/mask);
    col *= 0.5 + 0.5*cos((TIME*speed)+uv.xyx+vec3(0,2,4));
   
    gl_FragColor = vec4(col,1.);
}

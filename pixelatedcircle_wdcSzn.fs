/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/wdcSzn by scry.  variable resolution can be fun but I didn't expect a 3d effect! ",
  "INPUTS" : [

  ]
}
*/


void main() {



    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = (gl_FragCoord.xy-.5*RENDERSIZE.xy)/RENDERSIZE.y;
	vec3 col = vec3(0.);
    vec2 p = vec2(0.);
    //uv = floor(uv*120.)/120.;
    //float steps = abs(sin(TIME*0.1)*16.)+4.;
    float steps = 32. + sin(TIME*0.1)*31.;
    //float steps = 64.;
    float s = TIME*01.25;
    for (float i;i<steps;i++) {
        float ii = i*02.102/steps;
        p = vec2(sin(s+ii),cos(s+ii))*(0.3+sin((s+ii)*8.)*0.1);
        ii = ii*ii*15.;
        //ii += 40.;
        //ii = ii*20.;
        //ii = (steps*3.)-ii;
        vec2 pv = floor(uv*ii)/ii;
    	if (length(pv+p) < 0.1*(i/steps)) {
            col = vec3(i/(steps-1.));
            col = col*col*col;
        	//col = vec3((i/steps)+(.025));
        }
    }
    gl_FragColor = vec4(col,1.0);
}

/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35234.3"
}
*/


#ifdef GL_ES
precision highp float;
#endif


float stripe(float x) {
    return float(mod(x, .2) > .1) * floor(x * 10. + 1.) * .1;
}

vec3 degree_to_rgb(float h) {
    float h1 = mod(h, 360.) / 60.;
    float x = 1. - abs(mod(h1, 2.) - 1.);
    
    if (0. <= h1 && h1 < 1.) {
        return vec3(1, x, 0);
    }
    if (1. <= h1 && h1 < 2.) {
        return vec3(x, 1, 0);
    }
    if (2. <= h1 && h1 < 3.) {
        return vec3(0, 1, x);
    }
    if (3. <= h1 && h1 < 4.) {
        return vec3(0, x, 1);
    }
    if (4. <= h1 && h1 < 5.) {
        return vec3(x, 0, 1);
    }
    if (5. <= h1 && h1 < 6.) {
        return vec3(1, 0, x);
    }
    
    return vec3(0);
}

float test(float x, float y) {
    return (floor(abs(.5 - x) * 20.) * .1) * float(mod(y - x, .5) > .3);
}

void main() {
    vec2 st = gl_FragCoord.xy/128.;
    float movetime = TIME * .25;
	
    float r = stripe(mod(st.x + movetime, 1.));
    float g = stripe(mod(st.y + movetime, 1.));
    // float b = (1. - (step(mod(st.y, .2), .1) * float(st.x >= st.y) + step(mod(st.x, .2), .1) * float(st.x <= st.y) * floor(st.y * 10.))) * min(floor(st.x * 10.) * .1, floor(st.y * 10.) * .1);
    float b = test(mod(st.x - movetime * 1.75, 1.), mod(st.y - movetime, 1.)) * 1.5;
    
    vec3 col1 = degree_to_rgb(TIME * 12.) * r;
    vec3 col2 = degree_to_rgb(TIME * 30. + 60.) * g;
    vec3 col3 = degree_to_rgb(TIME * 42. + 180.) * b;
    
    vec3 color = col1 + col2 + col3;
    
    gl_FragColor = vec4(color, 1.);
}
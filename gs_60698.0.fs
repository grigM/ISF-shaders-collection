/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60698.0"
}
*/


// -----------------------------------------------------
// a different home by nabr
// self:http://glslsandbox.com/e#60054.5
// License Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0)
// https://creativecommons.org/licenses/by-nc/4.0/
// -----------------------------------------------------
//precision highp float;

// based on glslsandbox.com/e#30073.3
float tex(in vec3 st)
{
    
    vec3 i = st;
    for (float t = 0.; t < 10.; t += .489)
    {
        i = 5. * st + +vec3(cos(.20 * TIME - t - i.x) + cos(t - i.y), 
			     sin(.25 * TIME + t- i.y) + cos(t + i.x), 6);
	i.xy = st.xz + (0. - (cos(t) * i.xy + sin(t) * vec2(i.y, -i.x)));
    }
    return 6. - sqrt(.001-dot(vec3(0, 5, 4), i));
}

void main()
{
    vec2 st = (gl_FragCoord.xy - .5 * RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
    vec3 o = vec3(.001), d = normalize(vec3(st, -1));
    // sphere
    vec3 sc = vec3(0, 0, 2);
    float sr2 = .5;
    vec3 ld = normalize(vec3(-5, 2, -8));
    // scene/background color	
    vec3 col = vec3(.757),bg = pow(0.125 * vec3(.0, .0, .0), vec3(.454545));	
    // raytrace
    o -= sc;
    float b = dot(o, d), c = dot(o, o) - sr2, det = 0.2;//b * b - c;
	
    vec3 hit = o + (-b + sqrt(det)) * d;
    vec3 n = normalize(hit - sc);
    float spe = pow(max(0., dot(normalize(ld + (-sc)), n)), 32.0);
    col = cos(vec3(1, 1, 1) + (tex(12. * n) * 2.)) * .35 + .5;
    col *= spe * .9 / sqrt(det) * 3.;
    gl_FragColor.rgb = mix(.899 * bg, col, max(0., det));




    gl_FragColor.a = 1.;
}
/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60061.0"
}
*/


// -----------------------------------------------------
// translation by nabr 
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License
// Contact the author for other licensing options
// -----------------------------------------------------
//precision highp float;

// based on e#30073.3
float tex(in vec3 st)
{
    // based on e#30073.3
    vec3 i = st;
    for (float n = 0.; n < 100.; n += 1.)
    {
        float t = (1000.+TIME * 0.0125) * n;
        i = 10. * st + vec3(cos(t - i.x) + cos(t - i.y), sin(t - i.y) + cos(t + i.x) , sin(.25*TIME)-cos(.25 * i.z));
        i.xy = st.xz + (cos(t) * i.xy + sin(t) * vec2(i.y, -i.x));
    }
    return (.45 - min(cos(2. * TIME  + (2. * i.x)) * -.45, (6. - length(i))));
}
// -----------------------------------------------------
// Parallax mapping by nimitz (twitter: @stormoid)
// https://www.shadertoy.com/view/4lSGRh
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License
// Contact the author for other licensing options
// -----------------------------------------------------
vec3 prlpos(in vec3 p, in vec3 n, in vec3 rd)
{
    vec3 tgt = reflect(n, -rd); //(n * dot(rd, n) - rd); 
    tgt /= (abs(dot(tgt, rd))) + 1.;
    p += (tgt * tex(p) * .501);
    return p;
}
void main()
{  
 
  vec2 st = (gl_FragCoord.xy - .5 * RENDERSIZE) / min(RENDERSIZE.x, RENDERSIZE.y);
  //dither by iq - https://www.shadertoy.com/view/3tj3DW
  vec3 dthr = 4.*fract(sin(gl_FragCoord.x*vec3(13,1,11)+gl_FragCoord.y*vec3(1,7,5))*158.391832)/255.0;

  vec3 o = vec3(.001), d = normalize(vec3(st, -1));
	
  // sphere
  vec3 sc = vec3(0, 0, 2);
  float sr2 = 1.;

  vec3 ld = normalize(vec3(3. * sin(2.*TIME), 2. * cos(2.*TIME), -6));

 vec3 col = vec3(.757);
 gl_FragColor.rgb = pow(.25*vec3(.05, .26, .62), vec3(.454545));
	
  // raytrace
  o -= sc;
  float b = dot(o, d), 
	c = dot(o, o) - sr2 , 
	det = b * b - c;
  if (det > 0.)
  {
    vec3 hit = o + (-b + sqrt(det)) * d;
    vec3 n = normalize(hit - sc); 
    // half viewdir
    vec3 hd = normalize(ld + (-sc));
    float spe = pow(max(0., dot(hd, n)), 64.0);
    hit = prlpos(hit, n, d);
    float fc = pow(clamp(1.005 + dot(n, d), 0.0, 1.0), 60.0);
    col = cos(vec3(0,1,1.57) +  distance(tex(.5*hit), tex(12.*n)*2.)) * .35 + .5;
    col *=  spe * .85 / sqrt(det) * 1. + fc;
    gl_FragColor.rgb = dthr+mix(gl_FragColor.xyz, col, max(0., det));
  }
  
  gl_FragColor.a = 1.;
}
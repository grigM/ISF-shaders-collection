/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "0c7bf5fe9462d5bffbd11126e82908e39be3ce56220d900f633d58fb432e56f5.png"
    }
  ],
  "CATEGORIES" : [
    "isometric",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Mdscz8 by MacroMachines.  isometric based on https:\/\/www.shadertoy.com\/view\/Md2XRd by fizzle",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


mat2 z=mat2(1,1,1,-1);
float m(vec2 p)
{
    return step(cos(
        1.4257*(IMG_NORM_PIXEL(iChannel0, 
                          floor(z*p*.1)
                          /64.,-32.).
                r >.5 ? p.y : p.x)),
                .17);
}
void main() {



    vec2 p = z * (gl_FragCoord.xy / RENDERSIZE.y * 25. 
                  + iMouse.xy*.1 
                  + TIME*vec2(1.5,1.08)),
        c = floor(p);
    float s = step(1. - p.x + c.x, p.y - c.y), 
        f = m(c), 
        g = m(c + vec2(-1, 0) + s);
    s/=4.;
    vec2 sz = vec2(0);
	gl_FragColor.rgb = mix(vec3(.3,.3,.25),
                        vec3(1),
                        m(c-z[1])+(g < f ? 
                                   f*(.75-s) : 
                                   g*(.5+s)));
}

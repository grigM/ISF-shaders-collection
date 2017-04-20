/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "fractal",
    "weird",
    "8",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XlV3zm by aiekick.  Variation of (url=https:\/\/www.shadertoy.com\/profile]Weird Fractal 0[\/url]",
  "INPUTS" : [

  ]
}
*/


//based on shader from coyote => https://www.shadertoy.com/view/ltfGzS

void main()
{
    vec4 p=vec4(gl_FragCoord.xy,0.,1.)/RENDERSIZE.x-.5,r=p-p,q=r;p.y+=.25;
    q.zw-=TIME*0.1+1.;
    
    for (float i=1.; i>0.; i-=.01) {

        float d=0.,s=1.;

        for (int j = 0; j < 6; j++)
            r=max(r=abs(mod(q*s+1.,2.)-1.),r.yzxw),
            d=max(d-r.x*0.01,(.3-length(r*0.95)*.3)/s), // #
            s*=3.-r.x*0.6; // #

        q+=p*d;
        
        gl_FragColor = p-p+i;

        if(d<1e-5) break;
    }
}
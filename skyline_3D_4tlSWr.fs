/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "3d",
    "raymarching",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4tlSWr by FabriceNeyret2.  in the spirit of the 2D skylines (e.g., https:\/\/www.shadertoy.com\/view\/4tlXzM )... but in 3D :-)  ( I even reproduced the original temporal aliasing :-D  )\n\nBe sure to wait long enough ;-)",
  "INPUTS" : [

  ]
}
*/


#define T TIME

// --- using the base ray-marcher of Trisomie21: https://www.shadertoy.com/view/4tfGRB#

vec4 bg = vec4(0); // vec4(0,0,.3,0); 

void main() {

    vec4 p = vec4(gl_FragCoord.xy,0,1)/RENDERSIZE.yyxy-.5, d,c; p.x-=.4; // init ray
    // r(p.xz,.13); r(p.yz,.2); r(p.xy,.1);   // camera rotations
    d =p;                                 // ray dir = ray0-vec3(0)
    p.x += 15.*T; p += 2.*d;
    gl_FragColor = bg;
    float l,x=1e9, closest = 999.;
   
    for (float i=1.; i>0.; i-=.01)  {
       
        vec4 u = floor(p/8.), t = mod(p, 8.)-4., ta; // objects id + local frame
      	u.y = 0.; 
        u = sin(78.17*(u+u.yzxw));                     // randomize ids
        
        c = p/p*1.2;
        ta = abs(t);
        x=1e9; 
        if (sin(17.*(u.x+u.y+u.z))>.95) { // 10% of blocks
            ta.y = p.y + 30.*u.x - .3*pow(abs(.03*floor(p.z)),3.) + 35.;
            x = max(ta.x,max(ta.y,ta.z))  -3.; 
         }
        closest = min(closest, p.y+150.); 
        
        // artifacts: passed a object, we might be fooled about dist to next (hidden in next modulo-tile)
#if 1        // if dist to box border is closest to object, go there.  <<< the working solution ! (at mod8 scale)
        vec4 k, k1 = (4.-t)/d ,k2 = (-4.-t)/d, dd; 
        k = min (k1-1e5*sign(k1),k2-1e5*sign(k2))+1e5; // ugly trick to get the min only if positive.
        // 2 less ugly/costly formulations, but less robust close to /0 :
        //   k = mix(k1,k2, .5+.5*sign(k2));
        //   dd = d+.001*clamp(1.-d*d,.999,1.); k = (4.*sign(dd)-t)/dd;
        l = min(k.x,min(k.y,k.z));
        if (l<x) { p+= 1.*d*(l+0.01); continue; }
#endif
        // if (x<.01) c = texture(iChannel0,.1*(p.xy+p.yz));
      
        if(x<.01) // hit !
            { gl_FragColor = mix(bg,c,i*i); break;  }  // color texture + black fog
       
        p += d*x;       // march ray
     }
    //if (length(gl_FragColor)==0.) gl_FragColor = vec4(1,1,.6,0)*smoothstep(.31,.3,length(gl_FragCoord.xy/RENDERSIZE.y-vec2(1.3,.7)));
    gl_FragColor += vec4(1) * exp(-.01*closest)*(.5+.5*cos(1.+T/8.)); // thanks kuvkar ! 
}

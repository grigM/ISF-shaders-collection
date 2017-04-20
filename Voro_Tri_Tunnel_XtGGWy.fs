/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : [
        "cube05_0.png",
        "cube05_1.png",
        "cube05_2.png",
        "cube05_3.png",
        "cube05_4.png",
        "cube05_5.png"
      ],
      "TYPE" : "cube"
    }
  ],
  "CATEGORIES" : [
    "tunnel",
    "tri",
    "voro",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtGGWy by aiekick.  Voro Tri Tunnel",
  "INPUTS" : [

  ]
}
*/


vec4 voro(vec2 uv) // https://www.shadertoy.com/view/ltK3WD
{
	vec2 g = floor(uv);
	vec2 f = fract(uv);
	vec2 p,cp;
	float t = TIME * 0.05;
	float d = 1.,k;;
	for(int x=-2;x<=2;x++)
    for(int y=-2;y<=2;y++)
    {	
        p = vec2(x,y);
		p += .5 + .35*sin( t * 10. + 9. * fract(sin((g+p)*mat2(2,5,5,2)))) - f;
        k = max(abs(p.x)*.866 - p.y*.5, p.y);
        if (k<d)
        {
            d = k;
            cp = p;
        }
	}

	vec3 n = vec3(0);
    
    if ( d == -cp.x*.866 - cp.y*.5) 	n = vec3(1,0,0);
	if ( d == cp.x*.866 - cp.y*.5) 		n = vec3(0,1,0);
	if ( d == cp.y) 					n = vec3(0,0,1);
	
    return sqrt(textureCube(iChannel0, -n)*d*1.5);
}

void main()
{
	vec2 g = gl_FragCoord.xy;
	vec2 si = RENDERSIZE.xy;
	vec2 uv = (g+g-si)/si.y;
	
	uv.x = abs(uv.x);
	
	float a = atan(uv.x, uv.y) * 2.;
	float r = length(uv);
	
	uv = vec2(a,r/dot(uv,uv));
    
	uv.y += TIME;
	
	gl_FragColor = vec4(voro(uv)*(1. - 1./(1. + r*r*8.))); // thanks to shane
}
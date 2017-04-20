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
    "voronoi",
    "triangular",
    "lighted",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ltK3WD by aiekick.  based on [url=https:\/\/www.shadertoy.com\/view\/ltKGWD]Triangular Voronoi[\/url]",
  "INPUTS" : [

  ]
}
*/


// based on Triangular Voronoi https://www.shadertoy.com/view/ltKGWD
void main()
{
	vec2 p = g /= RENDERSIZE.y / 5.; f.x=9.;
    
    float t = TIME * 0.1;
    
    for(int x=-2;x<=2;x++)
    for(int y=-2;y<=2;y++)
    {	
        p = vec2(x,y);
		p += .5 + .35*sin( t * 10. + 9. * fract(sin((floor(g)+p)*mat2(2,5,5,2)))) - fract(g);
        p *= mat2(cos(t), -sin(t), sin(t), cos(t));
        f.y = max(abs(p.x)*.866 - p.y*.5, p.y);
        if (f.y < f.x)
        {
            f.x = f.y;
            f.zw = p;
        }
    }
	
    vec3 n = vec3(0);
    
    if ( f.x == -f.z*.866 - f.w*.5) 	n = vec3(1,0,0);
	if ( f.x == f.z*.866 - f.w*.5) 		n = vec3(0,1,0);
	if ( f.x == f.w) 					n = vec3(0,0,1);
	
    f = sqrt(textureCube(iChannel0, -n)*f.x);
}
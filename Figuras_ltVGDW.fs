/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "triangulo",
    "curculo",
    "rectangulo",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ltVGDW by FranciscoGarcia.  Funciones para crear figuras básicas.",
  "INPUTS" : [

  ]
}
*/


#define PI 3.14159265
#define DPI 6.2831853

float circulo(float r, in vec2 uv, in vec2 pos){
	
    uv = uv-pos;
    return 1.0-step(r,length(uv));
        
}

float rect(in vec2 uv, float a, float b, float c, float d, in vec2 pos){
	
    uv = (uv+vec2(0.5,0.5))-pos;
    float xy = step(a,uv.x);
    xy *= step(b,uv.y);
    xy *= step(c,1.-uv.x);
    xy *= step(d,1.-uv.y);
    return xy;
}

float xFigura(in vec2 uv, float n, float tam, in vec2 pos ){
    
    uv = uv-pos;
    float a = atan(uv.x, uv.y) - PI;
    float r = DPI/n;
    float d = cos(floor(0.5+a/r)*r-a)*length(uv);
    d = 1.0-step(tam,d); 
    return d;
}	


void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    // coordenadas de -1.0 a 1.0
    vec2 p = -1.0 + 2.0*uv;
    p.x *= RENDERSIZE.x/RENDERSIZE.y;
    
    vec3 col = vec3(0.0);
    
    //circulo(radio, coor, posición)
    float c0 = circulo(0.41, p, vec2(-1.2, 0.));
    
    //rect(coor, a,b,c,d, posición)
    float r0 = rect(p,-0.2,0.1,0.2,0.2, vec2(0.2,0.0));
    
    //xFigura(coor, nLados, tamaño, posición)
    float x0 = xFigura(p, 3.0, 0.30, vec2(1.2,-0.1));
    
    col += vec3(c0)*vec3(0.0,0.3,0.6);
    col += vec3(r0)*vec3(1.0,0.8,0.0);
    col += vec3(x0)*vec3(0.6,0.0,0.0);

    
	gl_FragColor = vec4(col,1.0);
}
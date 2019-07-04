/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#49636.0"
}
*/



//KSHIRO51

//precision mediump float;

uniform vec2 resolution1;

void main(void)
{
	float c = 0.0;
	
	vec2 uv = ( gl_FragCoord.xy / RENDERSIZE.xy )-.5;
	vec2 cuv = uv;
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
	
	
	
	c += 0.05/clamp(length(uv), 0.15, 1.0);
	
	c += 0.015/(length(uv));
	
	
	c += 0.005/(length(uv+vec2(+sin(TIME*2.0), 0.0) *0.5)); //derecha
	
	c += 0.005/(length(uv+vec2(0.0, -sin(TIME*2.0)) *0.5)); //arriba
	
	c += 0.005/(length(uv+vec2(sin(TIME*2.0), 0.0) *0.5)); //izquierda
	
	c += 0.005/(length(uv+vec2(0.0, sin(TIME*2.0)) *0.5)); //abajo
	
	
	c += 0.003/(length(uv.x-uv.y));
	
	c += 0.003/(length(uv.x+uv.y));
	
	//for(float i = 0.0; i < 10.0; i++)

	c += 0.010/(length(uv+vec2(-sin(TIME*2.0), -sin(TIME*2.0)) *0.5)); //izquierda abajo
	c += 0.010/(length(uv+vec2(sin(TIME*2.0), -sin(TIME*2.0)) *0.5)); //derecha abajo
	
	c += 0.010/(length(uv+vec2(sin(TIME*2.0), sin(TIME*2.0)) *0.5)); //derecha arriba
	c += 0.010/(length(uv+vec2(-sin(TIME*2.0), sin(TIME*2.0)) *0.5)); //izquierda arriba
	gl_FragColor =  vec4(vec3(c, c, c) * vec3(0.8, 0.75, 1.0), 1.0);

}
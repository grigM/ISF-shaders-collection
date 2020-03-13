/*
{
  "CATEGORIES" : [
    "Generators"
  ],
  "DESCRIPTION" : "Gradients",
  "ISFVSN" : "2",
  "VSN" : ".01",
  "INPUTS" : [
    {
      "NAME" : "circle_size",
      "TYPE" : "float",
      "MAX" : 10,
      "DEFAULT" : 1.5922174453735352,
      "MIN" : 0
    },
    {
      "NAME" : "fill_color",
      "TYPE" : "color",
      "DEFAULT" : [
        0.94757574796676636,
        0.13316583633422852,
        0.18496136367321014,
        1
      ]
    },
    {
      "NAME" : "grad_color",
      "TYPE" : "color",
      "DEFAULT" : [
        0.99536514282226562,
        0.83413654565811157,
        0,
        1
      ]
    }
  ],
  "CREDIT" : "@colin_movecraft"
}
*/

float makeDist(vec2 screenSpace, float multiplier){
	return length(screenSpace*multiplier);
}

mat2 scale(vec2 _scale){
    return mat2(_scale.x,0.0,
                0.0,_scale.y);
}

vec4 ellipseGrad( vec2 screenSpace, float radius, vec3 gradColor, vec3 fillColor ){
	
	screenSpace = scale( vec2(radius) ) * screenSpace;
	
	vec4 color = vec4(0.0,0.0,0.0,1.0);
	
	float gradDist = makeDist(screenSpace , 1.75  );
	float solidDist = makeDist(screenSpace, 1.0);
	
	float gradSDF = 1.0-pow( gradDist , 3.0 );
	float solidSDF = 1.0-smoothstep( .495,.5 , solidDist );
	
	vec3 grad = vec3( gradSDF ) * gradColor;
	vec3 solid = vec3( solidSDF ) * fillColor;
	
	color.rgb += grad;
	
	color.a -= max(1.0-gradSDF,0.0);
	
	color.a += solidSDF;
	color.rgb = 1.0-vec3(solidSDF);
	
	color.rgb *= grad;
	color.rgb += solid;
	
	return color;
}



void main(){
	
	vec4 color = vec4(0.1,0.2,0.1,1.0); 
	vec2 screenSpace = isf_FragNormCoord;
	screenSpace -=.5;
	screenSpace.x *= (RENDERSIZE.x/RENDERSIZE.y);

	color += ellipseGrad( screenSpace, circle_size, grad_color.rgb, fill_color.rgb);
	
		color += ellipseGrad( screenSpace+vec2(cos(TIME)*.1,sin(TIME+3.14)*.1), circle_size, grad_color.rgb, fill_color.rgb);
		color += ellipseGrad( screenSpace-vec2(cos(TIME)*.1,sin(TIME+3.14)*.1), circle_size, grad_color.rgb, fill_color.rgb);
		color += ellipseGrad( screenSpace-vec2(cos(TIME)*.1,sin(TIME)*.1), circle_size, grad_color.rgb, fill_color.rgb);
		
	color.a = 1.0;
	
	gl_FragColor = vec4(color);
}
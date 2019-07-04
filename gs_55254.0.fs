/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#55254.0"
}
*/


// added a bit of burning color

#ifdef GL_ES
precision highp float;
#endif



// Trapped by curiouschettai

void main( void ) {  
	vec2 uPos = ( gl_FragCoord.xy / RENDERSIZE.y );//normalize wrt y axis
	uPos -= vec2((RENDERSIZE.x/RENDERSIZE.y)/2.0, 0.5);//shift origin to center
	
	float multiplier = 0.0005; // Grosseur
	const float step = 0.5; //segmentation
	const float loop = 100.0; //Longueur
	const float timeSCale = 1.05; // Vitesse
	
	vec3 blueGodColor = vec3(0.0);
	for(float i=1.0;i<loop;i++){		
		float t = TIME*timeSCale-step*i;
		vec2 point = vec2(0.75*sin(t), 0.5*sin(t*.02));
		point += vec2(0.75*cos(t*0.1), 0.5*sin(t*1.3));
		point /= 4. * sin(i);
		float componentColor= multiplier/((uPos.x-point.x)*(uPos.x-point.x) + (uPos.y-point.y)*(uPos.y-point.y))/i;
		blueGodColor += vec3(componentColor/6.0, componentColor/4.0, componentColor);
	}
	
	
	vec3 color = vec3(0,0,0);
	color += pow(blueGodColor,vec3(1.1,1.1,0.8));
   
	
	gl_FragColor = vec4(color, 1.0);
}
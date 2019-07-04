/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#4551.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


//some bad lens flare rendering
//by MrOMGWTF
//pls improve this ;_;
//yep, added some gamma correction :)

float point(vec2 p, vec2 p2)
{
	return pow(1.0 / (distance(p, p2)) * 0.02, 0.75);	
}

float bokeh(vec2 p, float r, float smooth)
{
	vec2 q = abs(p);
	float d = dot(q, vec2(0.866024,  0.5));
	float s = max(d, q.y) - r;
	return smoothstep(smooth, -smooth, s);
}
vec3 adjust(vec3 color) {
	return color*color;
}
void main( void ) {

	vec2 p = (gl_FragCoord.xy / RENDERSIZE.xy - vec2(0.5,0.5)) * vec2(1.0, RENDERSIZE.y/RENDERSIZE.x);
	vec3 color;
	vec2 center = vec2(0.0);
	vec2 flare = vec2(-0.2 * sin(TIME), 0.1 * sin(TIME) +  0.1 * cos(TIME) * 0.5);//mouse * 2.0 - 1.0;
	vec2 flaredir = normalize(center - flare);
	color = vec3(0.0);
	//sun
	vec3 sun = vec3(0.0);
	sun += pow(vec3(1.0, 0.9, 0.8) * bokeh(p -flare ,0.05, 0.1) * 1.0, vec3(2.0));
	sun += vec3(1.0, 0.86, 0.8) * point(p, flare);
	
	//ghosts
	vec3 ghosts = vec3(0.0);
	ghosts += adjust(vec3(1.0, 0.9, 0.8) * bokeh(p + flare * 0.3,0.05, 0.004)) * 0.2;
	ghosts += adjust(vec3(1.0, 0.8, 0.7) * bokeh(p + flare * - 0.2,0.03, 0.003)) * 0.3;
	ghosts += adjust(vec3(1.0, 1.0, 0.9) * bokeh(p + flare * 0.6,0.06, 0.010)) * 0.2;
	ghosts += adjust(vec3(1.0, 1.0, 0.9) * bokeh(p + flare * 1.5,0.09, 0.015)) * 0.1;
	
	
	ghosts = pow(ghosts, vec3(2.0)) * 1.75;
	
	color = adjust(sun) + ghosts;
	
	gl_FragColor = vec4(sqrt(color), 1.0 );

}
/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#46075.2"
}
*/


//Peter Capener

#ifdef GL_ES
precision highp float;
#endif

#extension GL_OES_standard_derivatives : enable


float snow(float density, float R, float speed, float seed1, float seed2, float alphaSpeed){
	float scale  = min(RENDERSIZE.x, RENDERSIZE.y) / density;
	
	
	float move = TIME*max(RENDERSIZE.x, RENDERSIZE.y)/speed;//坐标系移动的距离
	vec2 uv = gl_FragCoord.xy - move;
	
	float cellX = floor(uv.x  / scale);
	float cellY = floor(uv.y / scale);
	
	vec2 off = vec2(cos(TIME + cellX) * R, sin(TIME + cellY) * R);//自动移动的距离
	
	float random = sin(cellX*R + cellY * scale);
	float centorOff = R*2.2 + (random  + 1.) / 2. * (scale - 4.4* R);//边缘多留一个半径，再多留每个粒子自由偏离的距离都是（1.0 +1.2）*2*R
	vec2 cellCentor = vec2(cellX * scale + centorOff, cellY * scale + centorOff) + off;
	
	float alpha = sin(TIME / alphaSpeed + centorOff);
	
	float d = length(cellCentor + move - gl_FragCoord.xy);
	float k = 1.-smoothstep(R, R*1.2, d);
	float c = k * alpha;
	return c;
}

void main( void ) {
	float c = snow(2., 10., 10., 3.241231234, 4.12341234, 1.0);
	if(c<0.0001)c+=snow(4., 8., 15., 3.685678567, 4.523463456, 0.8);
	if(c<0.0001)c+=snow(6., 6., 20., 3.345760345, 4.234709853, 1.6);

	gl_FragColor = vec4( vec3(c), max(c, 0.) );
	//gl_FragColor = vec4(-1.0, -1.0, -1.0, 1.);
}
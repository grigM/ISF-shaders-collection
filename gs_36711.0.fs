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
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#36711.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec4 sky()
{
	return vec4(0.0,0.8,0.8,0.0);
}

vec4 Sphere(vec3 pos, float radius)
{
	vec2 screenPos = gl_FragCoord.xy;//(gl_FragCoord.xy / RENDERSIZE.xy );
	
	vec2 toSphere = (pos.xy - screenPos);
	float distToSpherePos = length(toSphere.xy);
	
	//radius >= distToSphere = 0
	//radius < distToSphere = point on sphere
	bool isInSphere = radius < distToSpherePos;
	float pointIsOnSphere = isInSphere ? 0.0 : 1.0;
	
	float alpha = distToSpherePos/ radius;
	
	vec2 pointOnSphere = toSphere.xy/radius;
	vec2 xy;
	xy.x = abs(sin(pointOnSphere.x));
	xy.y = abs(sin(pointOnSphere.y));
	
	vec3 red= vec3(0.6,0.0,0.0);
	vec3 white = vec3(1.0,0.0,0.0);
	vec3 col = mix(red,white,pow(alpha,5.0));
	return vec4(col,pointIsOnSphere);
}

//freq in MHz
vec4 Wave(in vec3 pos, in float amp, in float freq, in float alpha, out bool outUnderWave)
{	
	float waveLength =  300.0 / freq;
	
	float waveAtPos = (sin((gl_FragCoord.x + pos.x) / freq) * amp) + pos.y;
	
	float bend = (gl_FragCoord.x / RENDERSIZE.x);
	bend = cos(TIME + bend * 3.14) * 15.0;
	
	//waveAtPos += bend;
	//bool underWave = (pos.y < waveAtPos);
	//float bend = sin(gl_FragCoord.y) * 4.0;
	outUnderWave = (gl_FragCoord.y > waveAtPos);
	
	float underWave = outUnderWave ? 0.0 : 1.0;
	vec4 result = vec4(0.0);
	
	//color of wave
	vec3 blue = vec3(0.0,0.0,1.0);
	vec3 red = vec3(1.0,0.0,0.0);
	
	
	vec2 screenCenter = RENDERSIZE.xy / 2.0;
	
	//0->1 (alpha 0 -> 0.5)
	
	
	vec3 color = mix(red,blue,alpha);
	
	float yDistToPos = abs(pos.y + bend - gl_FragCoord.y + abs(amp));
	
	yDistToPos /= 80.0;
	yDistToPos = clamp(yDistToPos,0.0,1.0);
	
	color = vec3( pow(1.0 - yDistToPos,3.0)) * color;//vec3(1.0,1.0,1.0) * 0.2;
	
		
	return vec4(color,1.0);
}



void main( void ) {

	vec2 mousePos = mouse * RENDERSIZE.xy;// ( gl_FragCoord.xy / RENDERSIZE.xy ) + mouse / 4.0;

	

	//gl_FragColor = vec4( vec3( color, color * 0.5, sin( color + TIME / 3.0 ) * 0.75 ), 1.0 );
	
	vec2 screenCenter = RENDERSIZE.xy/2.0;
		
	
	float sunRadius = 150.0;
	vec2 sunPos = screenCenter - vec2(0,20);
	//sunPos.y += sin(TIME * 0.5) * 30.0;
	vec4 sphere01 = Sphere(vec3(sunPos,0.0),sunRadius);
	
	
	//vec2 waveMovement = vec2(TIME * 30.0,sin(TIME * 2.0) * 15.0);	
	//vec3 wavePos = vec3(screenCenter + waveMovement - vec2(0.0,50),0.0);	
	//float ampMove = sin(TIME) * 20.0;
	//vec4 wave01 = Wave(wavePos,ampMove, 80.0);
	
	//vec4 wave02 = Wave(wavePos,ampMove/8.0, 1.0);
	
	
	vec4 outCol;// = sphere01.a > 0.0 ? sphere01 : sky();
	vec4 waves;
	const float waveSpacing = 20.0;
	const float numWaves = 30.0;
	float wavesXOffset = 0.0;//-mousePos.x;
	
	float absSinTime = sin(TIME);
	for(float f = 0.0; f <numWaves; f+=1.0)
	{
		float a = f;
		float fPlusOne = f +1.0;
		float alpha = clamp(f / (numWaves - 1.0),0.0,12.0);		
		
		vec2 waveMovement = vec2(TIME * (5.0),-abs(sin(TIME * 0.5)) * 5.0 * 0.0);	
		vec3 wavePos = vec3(screenCenter + vec2(0.0,waveSpacing * -a * 0.2),0.0);// + waveMovement - vec2(0.0,waveSpacing * -a) - vec2(0.0,20.0),0.0);	
		wavePos += screenCenter.y * 0.45;
		wavePos.x += wavesXOffset * (1.0 - alpha);
		
		float ampVar = mod(fPlusOne,3.0) + 10.0;// + 8.0;//mod(f,4.0) + 4.0;
		float ampMove = sin(TIME * (alpha * 1.0)) * (ampVar/8.0) * ampVar;// * cos(gl_FragCoord.x/400.0 + (TIME * 0.2 * f/4.0));
		bool underWave;
		vec4 wave = Wave(wavePos + vec3(TIME/a,0,0),ampMove, 80.0 / (10.0 * (1.0 -alpha)),alpha,underWave);
		
		outCol = underWave ? outCol :wave;
	}
	gl_FragColor = outCol;//sphere01 + waves;
	//gl_FragColor *= 0.2;

}
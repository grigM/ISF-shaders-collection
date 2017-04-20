/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "rings",
    "targe",
    "hypnotic",
    "moving",
    "smoothstep",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xsl3RX by possum.  Feel free to experiment with the constants.",
  "INPUTS" : [

  ]
}
*/


const float rings = 5.0;	//exactly the number of complete white rings at any moment.
const float velocity=4.;	
const float b = 0.003;		//size of the smoothed border

void main()
{
	vec2 position = gl_FragCoord.xy/RENDERSIZE.xy;
    float aspect = RENDERSIZE.x/RENDERSIZE.y;
	position.x *= aspect;
	float dist = distance(position, vec2(aspect*0.5, 0.5));
	float offset=TIME*velocity;
	float conv=rings*4.;
	float v=dist*conv-offset;
	float ringr=floor(v);
	float color=smoothstep(-b, b, abs(dist- (ringr+float(fract(v)>0.5)+offset)/conv));
	if(mod(ringr,2.)==1.)
		color=1.-color;
	gl_FragColor = vec4(color, color, color, 1.);
}
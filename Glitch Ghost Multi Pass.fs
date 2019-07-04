/*
{
  "CATEGORIES" : [
    "Glitch",
    "Blur"
  ],
  "INPUTS" : [
    {
      "NAME" : "inputImage",
      "TYPE" : "image"
    }
  ],
  "PASSES" : [
    {
      "TARGET" : "bufferA",
      "PERSISTENT" : true,
      "WIDTH": "$WIDTH/1.2",
	  "HEIGHT": "$HEIGHT/1.2"
    }
  ],
  "ISFVSN" : "2",
  "DESCRIPTION" : "glitch mixed"
}
*/

const float delta_x = 0.05;
const float delta_y = 0.05;

float extract_bit(float n, float b, float time) {
	n = floor(n);
	b = floor(b);
	b = floor(n/pow(2.,b) - time );
	return float(mod(b,2.) == 1.);
}

void main() {

	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;

	
	vec2 flow;

	vec4 pixel = IMG_NORM_PIXEL(inputImage, uv);
	
	vec3 intensity = vec3(0.99) - pixel.rgb;

	float position = floor(uv.y * 4.),
		number  = floor(uv.x * 128.),
		bits = extract_bit(number, position, TIME);

	float vidSample = dot( vec3(1.0) , pixel.rgb  * bits * .95);
	float vidSampleDx = dot( vec3(1.0), IMG_NORM_PIXEL(inputImage, uv + vec2(delta_x, 0.0)).rgb ),
		vidSampleDy = dot( vec3(1.0), IMG_NORM_PIXEL(inputImage, uv + vec2(0.0, delta_y)).rgb );

	flow = delta_x * bits * vec2(vidSampleDx - vidSample, vidSample - vidSampleDy);

	intensity *= 0.055;

	if ( PASSINDEX == 0){
		intensity += 0.95 * (1.0 - IMG_NORM_PIXEL(bufferA, uv + vec2( delta_x, delta_y) *  flow).rgb);
		gl_FragColor = vec4(1.0 - intensity, 1.0);
	}
	else{
		gl_FragColor = vec4(1.0 - intensity, 1.0);
	}


}

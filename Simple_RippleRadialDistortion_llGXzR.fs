/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex04.jpg"
    }
  ],
  "CATEGORIES" : [
    "distorionripple",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/llGXzR by tkoram20.  Just Random UV distortion tests",
  "INPUTS" : [

  ]
}
*/


float radial(vec2 pos, float radius)
{
    float result = length(pos)-radius;
    result = fract(result*1.0);
    float result2 = 1.0 - result;
    float fresult = result * result2;
    fresult = pow((fresult*5.5),10.0);
    //fresult = clamp(0.0,1.0,fresult);
    return fresult;
}




void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    vec2 c_uv = uv * 2.0 - 1.0;
    vec2 o_uv = uv * 0.80;
    float gradient = radial(c_uv, TIME*0.5);
    vec2 fuv = mix(uv,o_uv,gradient);
    vec3 col = IMG_NORM_PIXEL(iChannel0,mod(fuv,1.0)).xyz;
	gl_FragColor = vec4(col,1.0);
}
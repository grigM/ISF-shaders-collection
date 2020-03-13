/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "8de3a3924cb95bd0e95a443fff0326c869f9d4979cd1d5b6e94e2a01f5be53e9.jpg"
    }
  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4s2GRR by SanchYESS.  Input parameter: \"power\", output effect: fisheye or fisheye correction of input texture. Should work with different proportions.\nOrigins from http:\/\/stackoverflow.com\/questions\/6030814\/add-fisheye-effect-to-images-at-runtime-using-opengl-es",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    },
    {
     		"NAME" : "inputImage",
      		"TYPE" : "image"
    	},
    
  ]
}
*/


//Inspired by http://stackoverflow.com/questions/6030814/add-fisheye-effect-to-images-at-runtime-using-opengl-es
void main() {



	vec2 p = gl_FragCoord.xy / RENDERSIZE.x;//normalized coords with some cheat
	                                                         //(assume 1:1 prop)
	float prop = RENDERSIZE.x / RENDERSIZE.y;//screen proroption
	vec2 m = vec2(0.5, 0.5 / prop);//center coords
	vec2 d = p - m;//vector from center to current fragment
	float r = sqrt(dot(d, d)); // distance of pixel from center
	float power = ( 2.0 * 3.141592 / (2.0 * sqrt(dot(m, m))) ) *
				(iMouse.x / RENDERSIZE.x - 0.5);//amount of effect
	float bind;//radius of 1:1 effect
	if (power > 0.0) bind = sqrt(dot(m, m));//stick to corners
	else {if (prop < 1.0) bind = m.x; else bind = m.y;}//stick to borders
	//Weird formulas
	vec2 uv;
	if (power > 0.0)//fisheye
		uv = m + normalize(d) * tan(r * power) * bind / tan( bind * power);
	else if (power < 0.0)//antifisheye
		uv = m + normalize(d) * atan(r * -power * 10.0) * bind / atan(-power * bind * 10.0);
	else uv = p;//no effect for power = 1.0
	vec3 col = IMG_NORM_PIXEL(inputImage,mod(vec2(uv.x, -uv.y * prop),1.0)).xyz;//Second part of cheat
	           
	                                        //for round effect, not elliptical
	gl_FragColor = vec4(col, 1.0);
}

/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "plasma",
    "colorful",
    "trippy",
    "disco",
    "strange",
    "acid",
    "trip",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/4sfXRB by TomoAlien.  Remixed my shader into something super gnarly.",
  "INPUTS" : [
	{
    	"NAME": "waveFreq",
		"TYPE": "float",
		"MIN": 0.0,
		"MAX": 10.0,
		"DEFAULT": 4.0
    },
    {
    	"NAME": "waveDepth",
		"TYPE": "float",
		"MIN": 0.0,
		"MAX": 1000.0,
		"DEFAULT": 100.0
    }
    ,
    {
    	"NAME": "timeScale",
		"TYPE": "float",
		"MIN": 0.1,
		"MAX": 3.0,
		"DEFAULT": 1.0
    }
  ]
}
*/


void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	float time = TIME;
	float depth = sin(uv.y*2.0+sin(time*timeScale)*1.5+1.0+sin(uv.x*waveFreq*1.5+time*1.2*timeScale))*cos(uv.y*waveFreq+time*timeScale)+sin((uv.x*waveFreq*1.5+time));
	float texey = (uv.x-0.5);
	float xband = sin(sqrt(uv.y/uv.y)*3.0/(depth)+time*1.0);
	float final = (
		sin(texey/abs(depth)*(1000.0 - waveDepth)+time*16.0)*(depth)*xband
	);

	
	gl_FragColor = vec4(-final*abs(sin(time)),(-final*sin(time)*1.2),(final),1.0)*1.5;
}
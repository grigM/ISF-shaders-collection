/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel1",
      "PATH" : "8de3a3924cb95bd0e95a443fff0326c869f9d4979cd1d5b6e94e2a01f5be53e9.jpg"
    },
    {
      "NAME" : "iChannel2",
      "PATH" : "f735bee5b64ef98879dc618b016ecf7939a5756040c2cde21ccb15e69a6e1cfb.png"
    }
  ],
  "CATEGORIES" : [
    "hue",
    "feedback",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtcSWM by aferriss.  Moving texture coordinates in a circle based on hue.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ],
  "PASSES" : [
    {
      "TARGET" : "BufferA",
      "PERSISTENT" : true,
      "FLOAT" : true
    },
    {

    }
  ],
  "ISFVSN" : "2"
}
*/


const float amt = 15.0;

vec3 rgb2hsv(vec3 c)
{
    vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
    vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main() {
	if (PASSINDEX == 0)	{


	    vec2 res = RENDERSIZE.xy;
	    vec2 uv = gl_FragCoord.xy / res;
	    uv = -1.0 + 2.0 * uv;
	    uv *= 0.995;
	    uv = uv *0.5 + 0.5;
	    
	    vec4 rand = texture(iChannel2, uv*0.5 + TIME);
	    vec4 fb = texture(BufferA, uv);
	
	    vec4 colOut = texture(BufferA, uv + vec2(fb.y - fb.x, fb.x - fb.z)*(amt/res));
	    
	    colOut.rgb = rgb2hsv(colOut.rgb);
	    
	    colOut.r += 0.005;
	    colOut.r = mod(colOut.r, 1.0);
	    
	    colOut.g += 0.0025;
	    colOut.g = mod(colOut.g, 1.0);
	    
	    colOut.b += 0.0001;
	    colOut.b = mod(colOut.b, 1.0);   
	  
	    colOut.rgb = hsv2rgb(colOut.rgb);
	    colOut.rgb += rand.rgb *0.01;
	    
	    if(FRAMEINDEX < 10 || iMouse.z > 0.0){
	        gl_FragColor = texture(iChannel1, uv );
	    } else {
	    	gl_FragColor = colOut;
	    }
	}
	else if (PASSINDEX == 1)	{


		vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
		gl_FragColor = texture(BufferA, uv);
	}
}

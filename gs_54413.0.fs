/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#54413.0"
}
*/


//P

//https://www.cnblogs.com/webgl-angela/p/9846990.html

#ifdef GL_ES
precision mediump float; 
#endif

#extension GL_OES_standard_derivatives : enable


float SnowLayer(vec2 uv, float scale, float ttime)
{
  float w = smoothstep(1.0 , 0.0, -uv.y * (scale / 10.0));
  
  if (w < 0.1)
    return 0.0;
	
  uv += ttime / scale;
  uv.x -= ttime  / scale;
  
  uv.x += sin(uv.x - ttime * 0.5) / scale;
  
  uv *= scale;
  vec2 s = floor(uv),f=fract(uv),p;
  
  float k = 3.0;
  float d = 0.0;
  
  p = 0.5 + 0.35 * sin(11.0 * fract(sin((s + p + scale)* mat2(7.0, 3.0, 6.0, 5.0)) * 5.0)) - f;
  
  d = length(p);
	
  k = min(d,k);
	
  k = smoothstep(0.0, k ,sin(f.x + f.y) * 0.03);
  
  return k* w;
}

void main(void)
{
  vec2 uv = (gl_FragCoord.xy * 2.0 - RENDERSIZE.xy) / min(RENDERSIZE.x,RENDERSIZE.y);

  float ttime = mod(TIME * 2.0, RENDERSIZE.y);
  
  float snow = 0.0;
  
  snow += SnowLayer(uv, 10.0, ttime);
  snow += SnowLayer(uv, 8.0, ttime);
  snow += SnowLayer(uv, 6.0, ttime);
  snow += SnowLayer(uv, 5.0, ttime);
  snow += SnowLayer(uv, 4.0, ttime);
	
  gl_FragColor = vec4(vec3(snow), 1.0 );
}
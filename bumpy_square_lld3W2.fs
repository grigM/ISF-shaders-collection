/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "bumping",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/lld3W2 by zardoru.  a square that bumps",
  "INPUTS" : [

  ]
}
*/



float square(vec2 where, vec2 pos, float size) {
    vec2 tr = where + vec2(1.0,0) * size;
    vec2 bl = where + vec2(0,1) * size;
    vec2 br = where + vec2(1.0,1) * size;
    if (bl.x < pos.x && br.x > pos.x && pos.y < br.y && pos.y > tr.y)
        return 1.0;
    else
        return 0.0;
}

void main()
{
    float ratio = RENDERSIZE.x / RENDERSIZE.y;
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy * vec2(ratio, 1.0);
    float size = 0.25;
    float xbounce = 4.5;
    float ybounce = 3.0;
    float maxy = (1.0 - size);
    float maxx = (ratio - size);
    float x = abs(sin(TIME * xbounce)) * maxx;
    float y = abs(sin(TIME * ybounce)) * maxy;
   
    float col = square(vec2(x, y), uv, size);
	gl_FragColor = vec4(col);
}
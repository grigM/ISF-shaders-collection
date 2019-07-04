
/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "bricks",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MlXBzH by zeh.  Speed stripes",
  "INPUTS" : [
	{
            "NAME": "all_speed",
            "TYPE": "float",
            "DEFAULT": 2.0,
            "MIN": 0.0,
            "MAX": 20.0
    },
    {
            "NAME": "numRows",
            "TYPE": "float",
            "DEFAULT": 10.0,
            "MIN": 0.0,
            "MAX": 40.0
    },
    {
            "NAME": "numCols",
            "TYPE": "float",
            "DEFAULT": 10.0,
            "MIN": 0.0,
            "MAX": 40.0
    }
  ]
}
*/


//int numRows = 10;
//int numCols = 10;

   
void main() {



	float rowHeight = 1.0 / float(numRows);
	float colWidth = 1.0 / float(numCols);
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    int row = int(uv.y / rowHeight);
    
    float speed = float(row) / float(numRows);
    float offset = (speed * TIME) * all_speed;
    
    int col = int((uv.x + offset) / colWidth);
    float blurDistance = float(row) * rowHeight;
    float positionInCol = (uv.x + offset - (float(col) * colWidth)) / colWidth;
    float blurGray = positionInCol > blurDistance / 2.0 ? (positionInCol < 1.0 - blurDistance / 2.0 ? 0.0 : 0.5 - ((1.0 - positionInCol) / blurDistance)) : 0.5 - (positionInCol / blurDistance);
    
    gl_FragColor = vec4(0.0 + blurGray, 0.0 + blurGray, 0.0 + blurGray, 1.0);
    
    //gl_FragColor = vec4(positionInCol, positionInCol, positionInCol, 1.0);
    //gl_FragColor = vec4(offset, offset, offset, 1.0);
}

/*
float thickness = 2.0;
float width = 50.0;
float height = 30.0;
//float diamondSize = 10.0;
vec4 colorWhite = vec4(1.0, 1.0, 1.0, 1.0);
vec4 colorBlack = vec4(0.0, 0.0, 0.0, 1.0);
vec4 colorRed = vec4(1.0, 0.0, 0.0, 1.0);

{
    float diamondSize = 10.0 * abs(sin(TIME));
	vec2 uv = fragCoord.xy / RENDERSIZE.xy;
    float fx = mod(fragCoord.x, width);
    float fy = mod(fragCoord.y, height);
    float fax = min(fx, width - fx);
    float fay = min(fy, height - fy);
    if (fax + fay <= diamondSize) {
        if (fax + fay < diamondSize - thickness) {
	        fragColor = colorWhite;
        } else {
	        fragColor = colorBlack;
        }
    } else if (fax <= thickness / 2.0) {
        fragColor = colorBlack;
    } else if (fay <= thickness / 2.0) {
        fragColor = colorBlack;
    } else {
        fragColor = colorWhite;
    }
}
*/

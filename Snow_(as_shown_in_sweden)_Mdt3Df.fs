/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "2d",
    "parallax",
    "snow",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Mdt3Df by Emil.  Looks alot like outdoors right now where I live\n\nFun first try!\nIf I continue working on it I would probably fix the movement (more realistic) and also adjust the shapes so it's not just symmetric blobs as rightnow.",
  "INPUTS" : [

  ]
}
*/


/////// I started playing around with some other things 
/////// and then I looked out the window and felt inspired :)

void main() {



    float snow = 0.0;
    float gradient = (1.0-float(gl_FragCoord.y / RENDERSIZE.x))*0.4;
    float random = fract(sin(dot(gl_FragCoord.xy,vec2(12.9898,78.233)))* 43758.5453);
    for(int k=0;k<6;k++){
        for(int i=0;i<12;i++){
            float cellSize = 2.0 + (float(i)*3.0);
			float downSpeed = 0.3+(sin(TIME*0.4+float(k+i*20))+1.0)*0.00008;
            vec2 uv = (gl_FragCoord.xy / RENDERSIZE.x)+vec2(0.01*sin((TIME+float(k*6185))*0.6+float(i))*(5.0/float(i)),downSpeed*(TIME+float(k*1352))*(1.0/float(i)));
            vec2 uvStep = (ceil((uv)*cellSize-vec2(0.5,0.5))/cellSize);
            float x = fract(sin(dot(uvStep.xy,vec2(12.9898+float(k)*12.0,78.233+float(k)*315.156)))* 43758.5453+float(k)*12.0)-0.5;
            float y = fract(sin(dot(uvStep.xy,vec2(62.2364+float(k)*23.0,94.674+float(k)*95.0)))* 62159.8432+float(k)*12.0)-0.5;
            float randomMagnitude1 = sin(TIME*2.5)*0.7/cellSize;
            float randomMagnitude2 = cos(TIME*2.5)*0.7/cellSize;
            float d = 5.0*distance((uvStep.xy + vec2(x*sin(y),y)*randomMagnitude1 + vec2(y,x)*randomMagnitude2),uv.xy);
            float omiVal = fract(sin(dot(uvStep.xy,vec2(32.4691,94.615)))* 31572.1684);
            if(omiVal<0.08?true:false){
                float newd = (x+1.0)*0.4*clamp(1.9-d*(15.0+(x*6.3))*(cellSize/1.4),0.0,1.0);
                /*snow += d<(0.08+(x*0.3))/(cellSize/1.4)?
                    newd
                    :newd;*/
                snow += newd;
            }
        }
    }
    
    
    gl_FragColor = vec4(snow)+gradient*vec4(0.4,0.8,1.0,0.0) + random*0.01;
}

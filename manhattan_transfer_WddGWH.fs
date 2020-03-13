/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/WddGWH by tristanwhitehill.  bike bike",
  "INPUTS" : [

  ]
}
*/



vec2 hash( vec2 x ) 
{
    const vec2 k = vec2( 0.3183099, 0.3678794 );
    x = x*k + k.yx;
    return -1.0 + 3.0*sin( (TIME*.4)*130.0 * k*fract( x.x*x.y*(x.x+x.y)) );
}

float noise( in vec2 p )
{
    vec2 i = floor( p );
    vec2 f = ceil( p );
	
	vec2 u = f*f*(sin(TIME*.5)*5.-1.0*f);

    return mix( mix( dot( hash( i + vec2(0.0,0.0) ), f - vec2(0.0,0.0) ), 
                     dot( hash( i + vec2(1.0,0.0) ), f - vec2(1.0,0.0) ), u.x),
                mix( dot( hash( i + vec2(0.0,1.0) ), f - vec2(0.0,1.0) ), 
                     dot( hash( i + vec2(1.0,1.0) ), f - vec2(1.0,1.0) ), u.x), u.y);
}
float manhatanDistance(vec2 pointA, vec2 pointB)
{
    return abs(pointA.x - pointB.x) + abs(pointA.y - pointB.y);
}

void main() {

  

    	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    float n= (noise(uv));
   float fm = sin(TIME*.5)*.5;
   float fm2 = sin(TIME*.03)*fm*n;
    //Define points used for partitioning the plane
    vec2 points[10];
    points[0] = vec2(0.048, 0.259-fm);
	points[1] = vec2(0.45+fm2, 0.233-fm2);
	points[2] = vec2(0.386-fm*n, 0.556-fm);
	points[3] = vec2(0.847, 0.868+fm);
	points[4] = vec2(0.437+fm, 0.718-fm2);
	points[5] = vec2(0.095+fm, 0.558);
	points[6] = vec2(0.513-fm2, 0.680-fm2);
	points[7] = vec2(0.013, 0.865-fm);
	points[8] = vec2(0.168+fm2*n, 0.653+fm2);
	points[9] = vec2(0.891-fm, 0.721+fm);
    
    
    //Define the point colors
    vec4 pointColors[10];
    pointColors[0] = vec4(0.,0.,0.0, 1.0);
	pointColors[1] = vec4(255.,255.,255., 1.0);
	pointColors[2] = vec4(0.,0.,0.0, 1.0);
	pointColors[3] = vec4(255.,255.,255., 1.0);
	pointColors[4] = vec4(0.,0.,0.0, 1.0);  
    pointColors[6] = vec4(255.,255.,255., 1.0);
	pointColors[5] = vec4(0.,0.,0.0, 1.0);
	pointColors[7] = vec4(255.,255.,255., 1.0);
	pointColors[8] = vec4(0.,0.,0.0, 1.0);
	pointColors[9] = vec4(255.,255.,255., 1.0);
    
    //Allow the image to be scaled
    //Preserve aspect ratio
    if(RENDERSIZE.x > RENDERSIZE.y)
    {
        uv.x = uv.x / RENDERSIZE.y * RENDERSIZE.x;
        for(int i = 0; i < 10; i++)
        {
            points[i].x = points[i].x / RENDERSIZE.y * RENDERSIZE.x;
        }
    }
    else
    {
        uv.y = uv.y / RENDERSIZE.x * RENDERSIZE.y;
        for(int i = 0; i < 10; i++)
        {
            points[i].y = points[i].y / RENDERSIZE.x * RENDERSIZE.y;
        }
    }
    
    //Find the point closest to the pixel that is beeing colored now
    float minDistance = manhatanDistance(uv, points[0]);
    gl_FragColor = pointColors[0];
    for(int i = 1; i < 10; i++)
    {
        float currentDistance = manhatanDistance(uv, points[i]);
        if(currentDistance < minDistance)
        {
            minDistance = currentDistance;
            gl_FragColor = pointColors[i];
        }
    }
    
    //Color the points
    if(minDistance < 0.0005)
    {
        gl_FragColor = vec4(0.2, 0.2, 0.2, 1.0);
    }
}

/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35115.0"
}
*/




//80s Sci-Fi style thing.



float linstep(float x0, float x1, float xn)
{
	return (xn - x0) / (x1 - x0);
}

float cdist(vec2 v0, vec2 v1)
{
	v0 = abs(v0-v1);
	return max(v0.x,v0.y);
}


void main( void )
{
	//vec2 aspect = (RENDERSIZE.xy / cos(sin(TIME)/2.0) ) / (RENDERSIZE.y / sin(cos(TIME)/2.0) );
	vec2 aspect = (RENDERSIZE.xy ) / (RENDERSIZE.y  );
	vec2 _uv = gl_FragCoord.xy / RENDERSIZE.y;
	vec2 cen = aspect/2.0;

	vec3 color = vec3(0);


	vec2 gruv = _uv-cen;

	gruv = vec2(gruv.x * abs(1.4/gruv.y), abs(1.0/gruv.y) + ( mod(TIME, 1.0 ) ) );
	gruv.y = clamp(gruv.y,-1e2,1e2);

	float grid = 2.0 * cdist(vec2(0.5), mod((gruv)*1.0,vec2(1)));
		
	float gridmix = max(pow(grid,6.0) * 1.2, smoothstep(0.93,0.98,grid) * 3.0);

	vec3 gridcol = (mix(vec3(0.00, 0.00, 0.90), vec3(0.90, 0.00, 0.90), _uv.y*2.0) + 1.2) * gridmix;
	gridcol *= linstep(0.1,1.5,abs(_uv.y - cen.y));

	color = mix(gridcol, gridcol, gridcol);
	gl_FragColor = vec4( color, 1.0 );
}
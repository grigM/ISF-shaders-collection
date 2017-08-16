/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#41734.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec3 toHex(vec2 coords)
{
	float q = (coords.x * sqrt(3.0)/3.0 - coords.y / 3.0) / 50.0;
	float r = (coords.y * 2.0/3.0) / 50.0;
	return vec3(q,r,-q-r);
}

float round(float a)
{
	float t = floor(a);
	if(fract(a)>0.5) t=ceil(a);
	return t;
}



vec3 cube_round(vec3 cube)
{
    float rx = round(cube.x);
    float ry = round(cube.y);
    float rz = round(cube.z);

    float x_diff = abs(rx - cube.x);
    float y_diff = abs(ry - cube.y);
    float z_diff = abs(rz - cube.z);

    if (x_diff > y_diff && x_diff > z_diff)
        rx = -ry-rz;
    else if (y_diff > z_diff)
        ry = -rx-rz;
    else
        rz = -rx-ry;

    return vec3(rx, ry, rz);
}

vec3 edge_coord(vec3 lo)
{
	return vec3(lo.x+lo.y,lo.y+lo.z,lo.z+lo.x);
}

void main( void ) {

	vec2 position = ( gl_FragCoord.xy );

	
	vec3 cell = cube_round((toHex(position)));
	vec3 pc = toHex(position);
	vec3 lo = abs(pc-cell);
	vec3 ec = edge_coord(lo);
	float em = max(ec.x,max(ec.y,ec.z));
	
	float res=0.0;
	res = smoothstep(0.94,0.98,em);
	
	gl_FragColor = vec4(vec3(res),1.0);

}
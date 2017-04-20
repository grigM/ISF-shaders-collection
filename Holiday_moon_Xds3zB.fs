/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex09.jpg"
    }
  ],
  "CATEGORIES" : [
    "2d",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xds3zB by maqflp.  just moon, stars & sky. ",
  "INPUTS" : [

  ]
}
*/


//by maq/floppy
float gauss(float s, float d, float r)
{
		return (1.0/(sqrt(2.0*3.1415)*s))*exp(-pow(d-r,2.0)/(s*s));
}	
float bgauss(float s, float d, float r)
{
	if(d>=r)
		return gauss(s,d,r);
	else
		return gauss(s,0.0,0.0);
}
void main()
{
	vec2 uv = (gl_FragCoord.xy / RENDERSIZE.xy-0.5);
	uv.y*=0.66;
	vec2 point = vec2(-0.13,-0.03);
	float s=0.03;
	float r=0.1;
	// moon	
	float d = length(uv-point);
	d=bgauss(s,d,r);
	// earth shadow
	vec2 point2 = point+vec2(-0.1+sin(TIME*0.02),0.01);
	float d2 = length(uv-point2);
	d2=bgauss(s,d2,r);
	d=min(d,1.0-d2);
	// moon texture
	d*=(1.0+2.0*length(uv-point));
	d=d*(0.9+(0.3-d)*0.3*IMG_NORM_PIXEL(iChannel0,mod(uv+TIME*0.01,1.0)).y);

	//stars
	d=max(d,bgauss(0.0005,length(uv-vec2(0.01,0.112)),0.0)   );
	d=max(d,bgauss(0.002,length(uv-vec2(-0.33,0.2)),0.001)   );
	d=max(d,bgauss(0.001,length(uv-vec2(0.2,-0.2)),0.0)   );
	d=max(d,bgauss(0.002*(0.1+0.3*IMG_NORM_PIXEL(iChannel0,mod(uv+TIME*0.1,1.0)).x),length(uv-vec2(0.444,0.2)),0.0)   );
	d=max(d,bgauss(0.001*(0.1+0.3*IMG_NORM_PIXEL(iChannel0,mod(uv+TIME*0.1,1.0)).y),length(uv-vec2(-0.4,-0.0)),0.0)   );
	
	vec3 col=vec3(d);	
	gl_FragColor = vec4(col,1.0);// vec4(uv,0.5+0.5*sin(TIME),1.0);
}
/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#3568.0"
}
*/


//some ugly clouds
//noise function lifted from that eye
//added modification so I have 3d noise
//and now with some ugly god rays

// CBS: Looks awesome! :-)

#ifdef GL_ES
precision mediump float;
#endif


mat2 m = mat2( 0.90,  0.110, -0.70,  1.00 );

float hash( float n )
{
    return fract(sin(n)*758.5453);
}

float noise( in vec3 x )
{
    vec3 p = floor(x);
    vec3 f = fract(x);
    //f = f*f*(3.0-2.0*f);
    float n = p.x + p.y*57.0 + p.z*800.0;
    float res = mix(mix(mix( hash(n+  0.0), hash(n+  1.0),f.x), mix( hash(n+ 57.0), hash(n+ 58.0),f.x),f.y),
		    mix(mix( hash(n+800.0), hash(n+801.0),f.x), mix( hash(n+857.0), hash(n+858.0),f.x),f.y),f.z);
    return res;
}

float fbm( vec3 p )
{
    float f = 0.0;
    f += 0.50000*noise( p ); p = p*2.02;
    f += 0.25000*noise( p ); p = p*2.03;
    f += 0.12500*noise( p ); p = p*2.01;
    f += 0.06250*noise( p ); p = p*2.04;
    f += 0.03125*noise( p );
    return f/0.984375;
}

float cloud(vec3 p)
{
	float a =0.0;
	a+=fbm(p*3.0)*2.2-0.9;
	if (a<0.0) a=0.0;
	//a=a*a;
	return a;
}

vec3 f2(vec3 c)
{
	c+=hash(TIME+gl_FragCoord.x+gl_FragCoord.y*9.9)*0.01;
	
	
	c*=0.7-length(gl_FragCoord.xy / RENDERSIZE.xy -0.5)*0.7;
	float w=length(c);
	c=mix(c*vec3(1.0,1.2,1.6),vec3(w,w,w)*vec3(1.4,1.2,1.0),w*1.1-0.2);
	return c;
}

void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy ) ;
	position.y+=0.2;

	vec2 coord= vec2((position.x-0.5)/position.y,1.0/(position.y+0.2))+mouse*vec2(-0.25,0.25);
	
	
	
	coord+=fbm(vec3(coord*18.0,TIME*0.001))*0.07;
	coord+=TIME*0.01;
	
	
	float q = cloud(vec3(coord*1.0,TIME*0.0222));
	float qx= cloud(vec3(coord+vec2(0.156,0.0),TIME*0.0222));
	float qy= cloud(vec3(coord+vec2(0.0,0.156),TIME*0.0222));
	q+=qx+qy; q*=0.33333;
	qx=q-qx;
	qy=q-qy;
	
	float s =(-qx*2.0-qy*0.3); if (s<-0.05) s=-0.05;
	vec3 d=s*vec3(0.9,0.6,0.3);
	//d=max(vec3(0.0),d);
	//d+=0.1;
	d*=0.2;
	d=mix(vec3(1.0,1.0,1.0)*0.1+d*2.0,vec3(1.0,1.0,1.0)*0.9,1.0-pow(q,0.03)*1.1);
	d*=8.0;
	
	float gr=0.0;
	for(int i=0; i<100; i++)
	{
		vec2 p = position;
		p+=vec2(-float(i-50)*0.003,float(i-50)*0.009);
		
		
		vec2 c = vec2((p.x-0.5)/p.y,1.0/(p.y+0.2))+mouse*vec2(-0.25,0.25);
		//c+=fbm(vec3(coord*18.0,TIME*0.001))*0.07;
		c+=TIME*0.01;
		float gg=cloud(vec3(c,TIME*0.0222));
		gg=1.0-pow(gg,0.2);
		if(i<50 )
		{
			gg*=1.0-clamp(0.0,q*7.0,1.0);
			//continue;
		}
		
		gr+=gg;
	}
	
	
	vec3 col =mix(vec3(01.6/(position.y*2.5+0.2),0.9/(position.y*9.5+0.1),0.2)*0.5,d,clamp(0.0,q*1.1,1.0));
	col +=gr*0.013*vec3(0.9,0.6,0.3);
	gl_FragColor = vec4( f2(col), 1.0 );

}
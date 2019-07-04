/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
  
  		{
			"NAME": "ZOOM",
			"TYPE": "float",
			"DEFAULT": 2.5,
			"MIN": 0.5,
			"MAX": 15.0
			
		},
		{
			"NAME": "SCROLL_X",
			"TYPE": "float",
			"DEFAULT": 0,
			"MIN": -2,
			"MAX": 2
			
		},
		{
			"NAME": "SCROLL_Y",
			"TYPE": "float",
			"DEFAULT": 0,
			"MIN": -2,
			"MAX": 2
			
		},
		{
			"NAME": "VORONOI_SPEED",
			"TYPE": "float",
			"DEFAULT": 2.2831,
			"MIN": 0.1,
			"MAX": 20.0
			
		},
		
		{
			"NAME": "SATURATION",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 2.0
			
		},
		{
			"NAME": "BRIGHTNESS",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 2.0
			
		},
		{
			"NAME": "COLOR_CHANGE",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -0.3,
			"MAX": 0.3
			
		},
		
	{
		"NAME": "COS_DEFORM",
		"TYPE": "bool",
		"DEFAULT": false,
	},
	{
			"NAME": "COS_DEFORM_SPEED",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 2.0
			
		},
	{
			"NAME": "COS_X_DEFORM_PER",
			"TYPE": "float",
			"DEFAULT": 6.0,
			"MIN": 0.0,
			"MAX": 8.0
			
		},
		{
			"NAME": "COS_Y_DEFORM_PER",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 8.0
			
		},
		
		{
			"NAME": "COS_X_DEFORM_AMP",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": -1.0,
			"MAX": 1.0
			
		},
		{
			"NAME": "COS_Y_DEFORM_AMP",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": -1.0,
			"MAX": 1.0
			
		}
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#47219.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

//example from https://www.shadertoy.com/view/MdSGRc
float hash1( float n ) { return fract(sin(n)*43758.5453); }
vec2  hash2( vec2  p ) { p = vec2( dot(p,vec2(127.1,311.7)), dot(p,vec2(269.5,183.3)) ); return fract(sin(p)*43758.5453); }



vec4 voronoi( in vec2 x, float mode )
{
    vec2 n = floor( x );
    vec2 f = fract( x );

	vec3 m = vec3( 8.0 );
	float m2 = 8.0;
    for( int j=-2; j<=2; j++ )
    for( int i=-2; i<=2; i++ )
    {
        vec2 g = vec2( float(i),float(j) );
        vec2 o = hash2( n + g );

		// animate
        o = 0.5 + 0.5*sin( TIME * VORONOI_SPEED*o );

		vec2 r = g - f + o;

        // euclidean		
		vec2 d0 = vec2( sqrt(dot(r,r)), 1.0 );
        // triangular		
		vec2 d2 = vec2( max(abs(r.x)*0.866025+r.y*0.5,-r.y), 
				        step(0.0,0.5*abs(r.x)+0.866025*r.y)*(1.0+step(0.0,r.x)) );

		vec2 d; 
		d=mix( d2, d0, fract(mode) );
		
        if( d.x<m.x )
        {
			m2 = m.x;
            m.x = d.x;
            m.y = hash1( dot(n+g,vec2(7.0,113.0) ) );
			m.z = d.y;
        }
		else if( d.x<m2 )
		{
			m2 = d.x;
		}

    }
    return vec4( m, m2-m.x );
}

void main( void ) {

	
float mode = mod(TIME/7.0,3.0);
	mode = floor(mode) + smoothstep( 0.8, 1.0, fract(mode) );
	
    vec2 p = (gl_FragCoord.xy * 2.0 - RENDERSIZE ) / min(RENDERSIZE.x , RENDERSIZE.y);//gl_FragCoord.xy/RENDERSIZE.xx;
    
    p.x += SCROLL_X;
    p.y += SCROLL_Y;
     
    if(COS_DEFORM){
    	p += cos(p.x * COS_X_DEFORM_PER + (TIME*COS_DEFORM_SPEED)) * COS_X_DEFORM_AMP;
		p -=cos(p.y*COS_Y_DEFORM_PER +(TIME*COS_DEFORM_SPEED))*COS_Y_DEFORM_AMP;
    }
vec4 c = voronoi( ZOOM*p, 2. );
	
	c.y -=COLOR_CHANGE;
	
    vec3 col = BRIGHTNESS + SATURATION *sin( c.y*21.5 + vec3(11.0,1.0,1.9) );
    col *= sqrt( clamp( 1.0 - c.x, 0.0, 1.0 ) );
	col *= clamp( 0.5 + (1.0-c.z/2.0)*0.5, 0.0, 1.0 );
	col *= 0.4 + 0.6*sqrt(clamp( 4.0*c.w, 0.0, 1.0 ));
	
	
    gl_FragColor = vec4( col, 1.0 );
}
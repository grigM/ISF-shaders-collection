/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/3tXXzM by TekF.  Quick and dirty heightfield renderer so I have a base I can use for testing terrain generation stuff. Sharing because it might be useful to others.",
    "IMPORTED": {
        "iChannel0": {
            "NAME": "iChannel0",
            "PATH": "52d2a8f514c4fd2d9866587f4d7b2a5bfa1a11a0e772077d7682deb8b3b517e5.jpg"
        }
    },
    "INPUTS": [
    ],
    "PASSES": [
        {
            "FLOAT": true,
            "PERSISTENT": true,
            "TARGET": "BufferA"
        },
        {
        }
    ]
}

*/


const float heightScale = .1;
const int numSlices = 200;
#define WRAP 0
#define PERSPECTIVE 0
const float cameraOrbitSpeed = .1; // radians per second

// cheap and nasty lighting - can get better results by baking lighting in the height field
const vec3 lightDirection = vec3(1,1,1);
const vec3 lightTint = vec3(1);
const vec3 shadeTint = vec3(.1);

void main() {
	if (PASSINDEX == 0)	{


	    gl_FragColor = IMG_NORM_PIXEL(iChannel0,mod(gl_FragCoord.xy/RENDERSIZE.x,1.0)).xyzy;
	    gl_FragColor.rgb = mix( gl_FragColor.rgb, vec3(1), .2 );
	}
	else if (PASSINDEX == 1)	{


	    float cameraOrbitTime = TIME*cameraOrbitSpeed;
	    vec3 cameraPosition = vec3(cos(cameraOrbitTime),0,sin(cameraOrbitTime))*2. + vec3(0,.8,0);
	    vec3 cameraTarget = vec3(0);
	    
	    vec3 cameraK = normalize(cameraTarget-cameraPosition);
	    vec3 cameraI = normalize(cross(vec3(0,1,0),cameraK));
	    vec3 cameraJ = cross(cameraK,cameraI);
	    
	    vec2 screenPosition = (gl_FragCoord.xy-RENDERSIZE.xy*.5)/RENDERSIZE.x;
	    #if (PERSPECTIVE)
	    	// perspective
	    	vec3 ray = vec3(screenPosition,1);
	    #else
	    	// orthographic
	    	vec3 ray = vec3(0,0,1);
	    	cameraPosition += (cameraI*screenPosition.x + cameraJ*screenPosition.y) * 1.5;
	    #endif
	    
	    ray = ray.x*cameraI + ray.y*cameraJ + ray.z*cameraK;
	    ray = normalize(ray);
	
	    gl_FragColor = vec4(.5);
	    
	    for ( int i=0; i < numSlices; i++ )
	    {
	        float f = numSlices > 1 ? float(i)/float(numSlices-1) : 0.5;
	        f = 1. - f; // from top to bottom!
	        
	        // intersect ray with horizontal plane at top of height field
	        vec3 position = cameraPosition + ray*((f-.5)*heightScale-cameraPosition.y)/ray.y;
	
	        vec2 uv = position.xz;
	
	        uv = uv*vec2(RENDERSIZE.y/RENDERSIZE.x,1)+.5;
	
			#if WRAP
	        	vec4 tap = textureGrad(BufferA,fract(uv),dFdx(uv),dFdy(uv)); // tile with corrected filtering
	        #else
	        	vec4 tap = IMG_NORM_PIXEL(BufferA,mod(uv,1.0));
	        	if ( min(uv.x,uv.y) < 0. || max(uv.x,uv.y) > 1. ) tap.w = -1.;
	        #endif
	        
	        if ( tap.w > f )
	        {
	        	gl_FragColor = tap;
			    gl_FragColor.rgb = pow(gl_FragColor.rgb,vec3(2.2));
	            
	            // estimate dot product with surface normal
	            float sampleDistance = 1./RENDERSIZE.x;
	            vec3 lightDir = normalize(lightDirection);
	            
	            vec2 sampleOffset = vec2(0,sampleDistance);
	            vec2 positionX = position.xz + sampleOffset.yx;
	            vec2 positionZ = position.xz + sampleOffset.xy;
	            vec2 uvX = positionX;
	            uvX = uvX*vec2(RENDERSIZE.y/RENDERSIZE.x,1)+.5;  // todo: put this in a function
	            float heightX = textureGrad(BufferA,fract(uvX),dFdx(uv),dFdy(uv)).w;
	            vec2 uvZ = positionZ;
	            uvZ = uvZ*vec2(RENDERSIZE.y/RENDERSIZE.x,1)+.5;
	            float heightZ = textureGrad(BufferA,fract(uvZ),dFdx(uv),dFdy(uv)).w;
	            
	            float height = tap.w * heightScale;
	            heightX *= heightScale;
	            heightZ *= heightScale;
	            
	            // construct a 3D normal, I think ^ that 2D trick doesn't work
	            vec3 n = normalize(
	                	cross( vec3(positionZ,heightZ).xzy - vec3(position.xz,height).xzy,
	                		   vec3(positionX,heightX).xzy - vec3(position.xz,height).xzy )
	                );
	            
	            float light = max(0.,dot(n,lightDir));
	                          
	            gl_FragColor.rgb *= mix( shadeTint, lightTint, light );
	            
	            break;
	        }
	    }
	    
	    gl_FragColor.rgb = pow(gl_FragColor.rgb,vec3(1./2.2));
	}

}

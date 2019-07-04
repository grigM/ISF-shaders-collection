/*{
    "CREDIT": "mm team",
    "CATEGORIES": [
        "Image Control"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image",
        }
    ]
}*/

in vec2 mm_SurfaceCoord;

uniform bool mmTexturedLine;

uniform vec4 modulationColor;
uniform float ignoreAlpha;

uniform mat4 textureMatrix;
uniform float mmLineWidth;
uniform float mmLineLength;
uniform float mmBlur;
uniform bool mmRounded;
uniform float mmStartStroke;
uniform float mmEndStroke;

out vec4 out_Color;

void main()
{
    vec2 uv=mm_SurfaceCoord;

    float stroke=mmStartStroke*(1.-uv.x)+mmEndStroke*uv.x;

    uv.y+=0.5;
    float vDist=abs(1.-uv.y*2.);
    uv.x*=mmLineLength/mmLineWidth;
    float alpha=1.;
    // Create rounded borders
    if (mmRounded) {
	    if (uv.x<0.5) {
	        float hDist=2.*(0.5-uv.x);
	        alpha*=1.-sqrt(vDist*vDist+hDist*hDist);
	    }
	    else if (uv.x>(mmLineLength/mmLineWidth)-0.5) {
	        float hDist=2.*(0.5-((mmLineLength/mmLineWidth)-uv.x));
	        alpha=min(alpha,1.-sqrt(vDist*vDist+hDist*hDist));
	    }
	}
	// If this pixel is not in rounded border, process vertical blur
    if (alpha==1.) {
	    alpha-=vDist;
	}
    if (alpha<mmBlur) 
    	alpha=alpha*(1./mmBlur);
    else 
    	alpha=1.;

    // Process start/end stroke
    alpha*=stroke;

    vec4 texColor;
    if (mmTexturedLine) {
        texColor = MM_SHADER_THIS_NORM_PIXEL();
        texColor.a += ignoreAlpha;
	} else {
		texColor = vec4(1.,1.,1.,1.);
	}

    texColor.a*=alpha;
    out_Color = texColor * modulationColor;
}

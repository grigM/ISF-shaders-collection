/*{
	"CREDIT": "mm team",
	"CATEGORIES": [
		"Image Control"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image",
		},
        {
            "NAME": "mm_surf_steps",
            "LABEL": "Steps",
            "TYPE": "int",
            "MIN": 0.0,
            "MAX": 255.0,
            "DEFAULT": 5
        },
        {
            "NAME": "mm_surf_animated",
            "LABEL": "Animated",
            "TYPE": "bool",
            "DEFAULT": "True"
        }
	]
}*/

#if defined(RENDER_WIREFRAME)

    uniform vec4 wireframeColor;
	out vec4 out_Color;
	void main() {
        out_Color = wireframeColor;
    }

#else

	#include "MadCommon.glsl"

	uniform float opacity;
	uniform vec4 ambient;

	uniform bool hasShadowMap;

	uniform bool solidEnabled;
	uniform bool texturedEnabled;
	uniform bool lightingEnabled;
	uniform vec4 solidColor;
	uniform vec4 textureColor;

	uniform float ignoreAlpha;

	uniform vec3 lightColors;
	uniform bool lightAttenuated;
	uniform bool lightSpot;
	uniform float lightSpotExponent;
	uniform float lightSpotCutoff;

	// To be able to use multi lighting we will need to have a uniform per shadow map
	uniform sampler2D lightShadowMaps;
	uniform float lightShadowMapsSize;
	uniform int lightShadowMapsSamples;
	uniform float lightShadowMapsOffset;

	in vec4 lightPosition;
	in vec3 normal;
	in vec3 lightDir;
	in vec3 lightSpotDir;
	in vec3 eyeVec;
	in float att;

	out vec4 out_Color;

	float offset_lookup(sampler2D map, vec4 loc, vec2 offset)
	{
	    float distanceFromLight = texture(map,loc.xy + offset).x;
	    return (distanceFromLight < loc.z ? 0.0 : 1.0);
	}

	float antialiasedShadowMap(sampler2D map, float texmapscale, vec4 loc, int samples)
	{
	    float sum;
	    float x, y;

	    float minMax =  texmapscale * (float(samples) - 1.) / 2.;

	    sum = 0.0;
	    for (y = -minMax; y <= minMax; y += texmapscale)
	        for (x = -minMax; x <= minMax; x += texmapscale)
	            sum += offset_lookup(map, loc, vec2(x, y));

	    return sum / float(samples * samples);
	}

	highp float mm_surf_rand(vec2 co)
	{
	    highp float a = 12.9898;
	    highp float b = 78.233;
	    highp float c = 43758.5453;
	    highp float dt= dot(co.xy ,vec2(a,b));
	    highp float sn= mod(dt,3.14);
	    return fract(sin(sn) * c);
	}

	vec3 dither(vec4 inColor, vec2 uv)
	{
	    float luma = 0.2125 * inColor.r + 0.7154 * inColor.g + 0.0721 * inColor.b;

	    float quantizedLuma = int(luma * mm_surf_steps) / float(mm_surf_steps); 

	    // Points
	    vec2 randIn = uv;
	    if (mm_surf_animated) randIn.x += fract(TIME);
	    return mm_surf_rand(randIn) < (quantizedLuma) ? vec3(1) : vec3(0);
	}

	void main()
	{
	    vec3 N = normalize(normal);
	    vec3 E = normalize(eyeVec);

	    vec4 final_color = vec4(0,0,0,0);
	    vec4 object_color = vec4(0,0,0,0);

	    // Solid mode enabled
	    if (solidEnabled)
	    {
			object_color += solidColor;
	    }

	     // Texture mode enabled
		if (texturedEnabled)
		{
	    	vec4 tex_color = textureColor * MM_SHADER_THIS_NORM_PIXEL();
	    	tex_color.a += ignoreAlpha;
	    	tex_color.rgb = dither(tex_color,isf_FragNormCoord);
	        object_color = tex_color.a * vec4(tex_color.rgb,1) + (1. - tex_color.a) * object_color;
	    }

		// Lighting mode enabled
		if (lightingEnabled)
		{
	        final_color += ambient * object_color;

			vec3 L = normalize(lightDir);

			float lambertTerm = dot(N,L);

			if(lambertTerm > 0.0)
			{
				float attenuation = 1.;
				if (lightAttenuated)
					attenuation = att;

				if (lightSpot)
				{
					float clampedCosine = max(0.0, dot(-L, lightSpotDir));
					float maxCosine = cos(radians(lightSpotCutoff));
					if (clampedCosine < maxCosine) // outside of spotlight cone
		                attenuation = 0.0;
	  				else
						attenuation = attenuation * pow(1.-((1.-clampedCosine) / (1.-maxCosine)), lightSpotExponent);

					if (hasShadowMap) {
	                    if (attenuation > 0.0 && lightPosition.w > 0.0)
	                    {
	                        vec4 shadowCoordinateWdivide = lightPosition / lightPosition.w;

	                        // Used to lower moire pattern and self-shadowing
	                        shadowCoordinateWdivide.z -= lightShadowMapsOffset;

	                        attenuation = attenuation * antialiasedShadowMap(lightShadowMaps, 1./lightShadowMapsSize, shadowCoordinateWdivide, lightShadowMapsSamples);
	                    }
					}
				}

				vec4 light_color = vec4(lightColors,1.) * lambertTerm * object_color;
				final_color += light_color * attenuation;
			}
		}
		else
			final_color += object_color;

	    out_Color.rgb = final_color.rgb;

		out_Color = vec4(out_Color.rgb, opacity * final_color.a);
	}

#endif

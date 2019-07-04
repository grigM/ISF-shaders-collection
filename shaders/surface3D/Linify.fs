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
            "NAME": "mm_surf_mode",
            "LABEL": "Mode",
            "TYPE": "long",
            "VALUES": ["Density","Width","Density & Width"],
            "DEFAULT": "Density & Width"
        },
        {
            "NAME": "mm_surf_steps",
            "LABEL": "Steps",
            "TYPE": "int",
            "MIN": 2,
            "MAX": 30,
            "DEFAULT": 5
        },
        {
            "NAME": "mm_surf_density",
            "LABEL": "Density",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 100.0,
            "DEFAULT": 50
        },
        {
            "NAME": "mm_surf_rotation",
            "LABEL": "Rot. Base",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 360.0,
            "DEFAULT": 90.0
        },
        {
            "NAME": "mm_surf_rotation_inc",
            "LABEL": "Rot. Inc",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 360.0,
            "DEFAULT": 360.0
        },
        {
            "NAME": "mm_surf_step_rotation",
            "LABEL": "Step Rot",
            "TYPE": "bool",
            "DEFAULT": "True",
            "FLAGS": "button"
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

	vec3 linify(vec4 inColor, vec2 uv)
	{
	    float luma = 0.2125 * inColor.r + 0.7154 * inColor.g + 0.0721 * inColor.b;

	    float quantizedLuma = int(luma * mm_surf_steps) / float(mm_surf_steps); 

	    float rotation;
	    if (mm_surf_step_rotation)
	        rotation = (mm_surf_rotation+ mm_surf_rotation_inc * quantizedLuma) * 3.141592654 / 180 ;
	    else
	        rotation = (mm_surf_rotation + mm_surf_rotation_inc * luma) * 3.141592654 / 180;

	    vec2 rotatedUv = vec2(uv.x * cos(rotation), uv.y * sin(rotation));

	    if (mm_surf_mode == 0)
	        // Change density
	        return mod(rotatedUv.x - rotatedUv.y,1/(mm_surf_density*(0.3+2*quantizedLuma))) < (0.5/(mm_surf_density*(1+2*quantizedLuma))) ? vec3(1) : vec3(0);
	    else if (mm_surf_mode == 1)
	        // Change width
	        return mod(rotatedUv.x - rotatedUv.y,1/mm_surf_density) < (quantizedLuma*0.5/mm_surf_density) ? vec3(1) : vec3(0);
	    else
	        // Change width
	        return mod(rotatedUv.x - rotatedUv.y,1/(mm_surf_density*(0.3+2*quantizedLuma))) < (quantizedLuma/(mm_surf_density*(0.3+2*quantizedLuma))) ? vec3(1) : vec3(0);
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
	    	vec4 tex_color = textureColor * clamp(MM_SHADER_THIS_NORM_PIXEL(),0,1);
	    	tex_color.a += ignoreAlpha;
		    tex_color.rgb = linify(tex_color, isf_FragNormCoord);
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

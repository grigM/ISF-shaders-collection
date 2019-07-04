in vec2 in_Vertex;
in vec2 in_TexCoord0;
in vec2 in_TexCoord1;

uniform mat4 modelViewProjectionMatrix;
uniform mat4 textureMatrix;

out vec2 isf_FragNormCoord;
out vec2 mm_SurfaceCoord;

#include "MadNoise.glsl"

void main()
{
    isf_FragNormCoord = (textureMatrix*vec4(in_TexCoord0,0,1)).xy;
    mm_SurfaceCoord = in_TexCoord1;

    vec2 pos = in_Vertex.xy;
    float noisex = noise(vec3(pos + 0.123,mm_surf_noise_time));
    float noisey = noise(vec3(pos + 0.345,mm_surf_noise_time));
    gl_Position = modelViewProjectionMatrix * vec4(pos + vec2(noisex,noisey)*mm_surf_power,0,1);

    #ifdef IS_MATERIAL
        materialVsFunc(isf_FragNormCoord);
    #endif
}


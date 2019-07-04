in vec2 in_Vertex;
in vec2 in_TexCoord0;
in vec2 in_TexCoord1;

uniform mat4 modelViewProjectionMatrix;
uniform mat4 textureMatrix;

out vec2 isf_FragNormCoord;
out vec2 mm_SurfaceCoord;

void main()
{
    isf_FragNormCoord = (textureMatrix*vec4(in_TexCoord0,0,1)).xy;
	mm_SurfaceCoord = in_TexCoord1;
    gl_Position = modelViewProjectionMatrix * vec4(in_Vertex.xy,0,1);
    #ifdef IS_MATERIAL
        materialVsFunc(isf_FragNormCoord);
    #endif
}


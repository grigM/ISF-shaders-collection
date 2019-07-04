in vec2 in_Vertex;
in vec2 in_TexCoord0;
in vec2 in_TexCoord1;

uniform mat4 modelViewProjectionMatrix;
uniform mat4 textureMatrix;

out vec2 isf_FragNormCoord;
#ifdef SOFT_EDGE_1_ACTIVE
	out vec2 mm_SurfaceCoord;
#endif

void main()
{
    isf_FragNormCoord = (textureMatrix*vec4(in_TexCoord0.xy,0,1)).xy;
	#ifdef SOFT_EDGE_1_ACTIVE
		mm_SurfaceCoord = in_TexCoord1;
    #endif
    vec2 scale = vec2(mm_surf_extend);
    mat4 S =mat4(mat2(scale[0],0,0,scale[1]));
    gl_Position = modelViewProjectionMatrix * S *vec4(in_Vertex.xy + vec2(mm_surf_offset_x,mm_surf_offset_y),0,1);
    #ifdef IS_MATERIAL
        materialVsFunc(isf_FragNormCoord);
    #endif
}

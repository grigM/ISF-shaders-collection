#if defined(RENDER_WIREFRAME)

	in vec4 in_Vertex;
	uniform mat4 modelViewProjectionMatrix;
	void main() {
		vec4 finalPos = modelViewProjectionMatrix * in_Vertex;
		gl_Position = finalPos;

		float noiseIntensity = mm_surf_noise_power * mm_surf_noise_power;
		gl_Position.x += noiseIntensity * sin(2*3.141592654*fract(abs(200*mm_surf_anim_position*(1+fract(1+sin(dot(in_Vertex.xy, vec2(12.9898,78.233))) * 43758.5453)))));
		gl_Position.y += noiseIntensity * sin(2*3.141592654*fract(abs(200*mm_surf_anim_position*(1+fract(1+sin(dot(in_Vertex.xy, vec2(78.233,12.9898))) * 43758.5453)))));
	}

#else

	in vec4 in_Vertex;
	in vec3 in_Normal;
	in vec2 in_TexCoord0;

	uniform vec3 lightPositions;
	uniform vec3 lightAttenuations;
	uniform vec3 lightSpotDirections;

	uniform mat4 modelViewMatrix;
	uniform mat4 modelViewProjectionMatrix;
	uniform mat3 normalMatrix;
	uniform mat4 textureMatrix;

	uniform mat4 lightMatrices;

	out vec4 lightPosition;
	out vec2 isf_FragNormCoord;
	out vec3 normal;
	out vec3 lightDir;
	out vec3 lightSpotDir;
	out vec3 eyeVec;
	out float att;

	void main()
	{	
		normal = normalMatrix * in_Normal;
		vec3 vVertex = vec3(modelViewMatrix * in_Vertex);
		eyeVec = -vVertex;

		vec3 lightPos = vec3(modelViewMatrix * vec4(lightPositions,1));
		lightSpotDir = normalize(normalMatrix * lightSpotDirections);
		lightDir = lightPos - vVertex;
		float d = length(lightDir);
		att = 1.0 / (lightAttenuations.x + 
	                   (lightAttenuations.y*d) + 
	                   (lightAttenuations.z*d*d) );

		lightPosition = lightMatrices * in_Vertex;

	    isf_FragNormCoord = (textureMatrix * vec4(in_TexCoord0.xy,0,1)).xy;

		vec4 finalPos = modelViewProjectionMatrix * in_Vertex;
		gl_Position = finalPos;

		float noiseIntensity = mm_surf_noise_power * mm_surf_noise_power;
		gl_Position.x += noiseIntensity * sin(2*3.141592654*fract(abs(200*mm_surf_anim_position*(1+fract(1+sin(dot(in_Vertex.xy, vec2(12.9898,78.233))) * 43758.5453)))));
		gl_Position.y += noiseIntensity * sin(2*3.141592654*fract(abs(200*mm_surf_anim_position*(1+fract(1+sin(dot(in_Vertex.xy, vec2(78.233,12.9898))) * 43758.5453)))));

	    #ifdef IS_MATERIAL
	        materialVsFunc(isf_FragNormCoord);
	    #endif
	}

#endif
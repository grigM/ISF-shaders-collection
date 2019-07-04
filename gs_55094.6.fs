/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#55094.6"
}
*/


// https://www.shadertoy.com/view/4dBGRw   666

#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


vec4 DrawExplosion(vec2 uv, vec4 explosionColour, float frame, float size, float seed)
{
    vec4 renderedColour = vec4(0.0);
    
    vec2 point = vec2(1.0, 0.0);
    
    mat2 rr = mat2(0.54030230586, -0.8414709848, 0.8414709848, 0.54030230586);
    
    float particleSize  = (size * 100.0);
    
    for (int particle = 1; particle < 16; particle += 1)
    {
        float particleDist = cos(float(particle) * sin(float(particle) * seed) * seed);
        
        vec2 particpos = (point * frame) * particleDist;
        
        float particleDistance = dot(particpos - uv, particpos - uv);
        
        if (particleDistance < particleSize)
        {
            float fade = (float(particle) / 16.0) * frame;
            
            renderedColour = mix((explosionColour / fade), renderedColour, smoothstep(0.0, size * 2.0, particleDistance));
        }
        
        point = point * rr;
    }
    
    renderedColour *= smoothstep(0.0, 1.0, (1.0 - frame) / 1.0);
    
    return renderedColour;
}

void main(void)
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	
	uv = 1.0 - (2.0 * uv);
	
	if (RENDERSIZE.x > RENDERSIZE.y)
		uv.x *= (RENDERSIZE.x / RENDERSIZE.y);
	else
		uv.y *= (RENDERSIZE.y / RENDERSIZE.x);	
	
	gl_FragColor = DrawExplosion(uv, vec4(0.5, 0.3, 0.8, 1.0), mod(TIME, 1.0), 0.005, floor(TIME) + 1.0);
}
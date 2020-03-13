/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#50097.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


    void main( void ) {
        vec2 center_xy = (gl_FragCoord.xy * 2.0 - RENDERSIZE.xy) / min(RENDERSIZE.x, RENDERSIZE.y);
        for(float i = 0.0; i < 5.0 ; i++)
        {
		float v = i * 5.0;
		float t = TIME / 2.0;
		vec2 xy_1      = vec2(center_xy.x + cos(t + v) * cos(t), center_xy.y + sin(t + v) * sin(t));
		vec2 xy_2      = vec2(xy_1.x      + sin(t + v) * cos(t), xy_1.y      + cos(t + v) * sin(t));
		vec2 xy_3      = vec2(xy_2.x      + cos(t + v) * cos(t), xy_2.y      + cos(t + v) * sin(t));
		vec2 xy_4      = vec2(xy_1.x      + sin(t + v) * cos(t), xy_1.y      + sin(t + v) * sin(t));
		vec2 xy_5      = vec2(xy_2.x      + cos(t + v) * sin(t), xy_2.y      + sin(t + v) * cos(t));
		vec2 xy_6      = vec2(xy_3.x      + sin(t + v) * sin(t), xy_3.y      + cos(t + v) * cos(t));
                vec2 xy_7      = vec2(xy_3.x      + cos(t + v) * sin(t), xy_3.y      + cos(t + v) * cos(t));
		float red      = 0.01 / abs(length(xy_1));
		float green    = 0.01 / abs(length(xy_2));
		float blue     = 0.01 / abs(length(xy_5));
		float yellow   = 0.01 / abs(length(xy_3));
		float purple   = 0.01 / abs(length(xy_4));
		float orange   = 0.01 / abs(length(xy_6));
		float sky      = 0.01 / abs(length(xy_7));
		gl_FragColor  += vec4(0.0, 0.0, blue, 1.0);
		gl_FragColor  += vec4(red, 0.0, 0.0, 1.0);
		gl_FragColor  += vec4(0.0, green, 0.0, 1.0);
		gl_FragColor  += vec4(yellow, yellow, 0.0, 1.0);
		gl_FragColor  += vec4(purple, 0.0, purple, 1.0);
                gl_FragColor  += vec4(orange, orange / 2.0, 0.0, 1.0);
		gl_FragColor  += vec4(0.0, sky / 2.0, sky, 1.0);
	}
    }
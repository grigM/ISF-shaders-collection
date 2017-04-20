/*
{
  "CATEGORIES" : [
    "Automatically Converted"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#35124.0"
}
*/




        #define MAX_ITER 13

        void main( void )
        {
            vec2 v_texCoord = gl_FragCoord.xy / RENDERSIZE;

            vec2 p =  v_texCoord * 8.0 - vec2(20.0);
            vec2 i = p;
            float c = 1.0;
            float inten = .03;

            for (int n = 0; n < MAX_ITER; n++)
            {
                float t = TIME * (1.0 - (3.0 / float(n+1)));

                i = p + vec2(cos(t - i.x) + sin(t + i.y),
                sin(t - i.y) + cos(t + i.x));
		    
                c += 1.0/length(vec2(p.x / (sin(i.x+t)/inten),
                p.y / (cos(i.y+t)/inten)));
            }

            c /= float(MAX_ITER);
            c = 1.5 - sqrt(c);

            vec4 texColor = vec4(0.02, 0.15, 0.02, 1.);

            texColor.rgb *= (1.0 / (1.0 - (c + 0.05)));

            gl_FragColor = texColor;
        }
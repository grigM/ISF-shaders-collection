/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#41674.0"
}
*/


		
		#ifdef GL_ES
		precision mediump float;
		#endif
		#define PI 3.141592653589793238462643383279
		
		float dist2(vec2 p1, vec2 p2)
		{
			float factor = 1.0;
			float f1 = 20.0 / factor;
			float f2 = f1 * f1;
			vec2 d = p1 - p2;
			return sqrt(dot(d,d)+f2) - f1;
		}

		float dist(vec3 p1, vec3 p2)
		{
			return distance(p1,p2);
		}

		vec4 col(float tick, float v) {
			float r = 0.166 * PI + tick * 0.05 + 1.234*TIME;
			float g = 0.5*PI + tick * 0.05 + 5.3 * TIME;
			float b = 0.5*PI + tick * 0.1 + 3.45 * TIME;
			return vec4(cos(r+v)+1.0,cos(g+v)+1.0,cos(b+v)+1.0,1.0);
		}

		void main( void ) {

			vec2 pos = (gl_FragCoord.xy) / RENDERSIZE.xy;
			float metafactor = 0.2*TIME;
			float factor = metafactor * 1000.0 / 2.0;
			
			float tick = (TIME * 18.0);

			float circle1 = tick * 0.145 / 6.0;
			float circle2 = -tick * 0.1 / 6.0;
			float circle3 = tick * .8 / 6.0;
			float circle4 = -tick * .2 / 6.0;
			float circle5 = tick * .34 / 6.0;
			float circle6 = -tick * .15 / 6.0;
			float circle7 = tick * .35 / 6.0;
			float circle8 = -tick * .05 / 6.0;

			float sum = 0.;
			float roll = tick * 8.0;
	
				vec3 p = vec3(pos.x*2.0-1.0,pos.y*2.0-1.0, 0);
				vec3 p1 = vec3( sin(circle3), sin(circle4), 0);
				vec3 p2 = vec3( sin(circle1), cos(circle2), 0);
				vec3 p3 = vec3(sin(circle5), sin(circle6), 0);
				vec3 p4 = vec3( cos(circle7), sin(circle8), 0);
				sum += cos(dist(p,p1)) + sin(dist(p,p3)) - sin(dist(p,p4));
			        
			
			gl_FragColor  = col(tick, sum*30.0+ TIME)/40.;
			gl_FragColor += col(tick*.5, sum*10.0+ TIME)/20.;
			
			gl_FragColor *= mod(gl_FragCoord.y,3.0)+mod(gl_FragCoord.x,3.0);
		}
/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#38834.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable

float t=TIME;
 

                    ////                     ////         
                    ////                     ////
                        ////             ////         
                        void             ////
		    main(){vec2 d=gl_FragCoord.xy;
                    vec4 o=vec4(0),l;o=o+1.+fract(
                .3*t)-o;    d*=.03;int y;    for (int
                i=0;i<7;    i++)l=(1.>mod    (((y=int
            (mod(t-d.y/o,14.)))>3? 35552534e8:56869384.)/
            exp2(float(y*7+int(abs(mod(d.x/o-7.*cos(t),14.
            )-7.    )))),2.))?o*=.5:vec4(o);o.z*=    2.;;
	    ;;;;    gl_FragColor=l;}/////////////    ////
            ////    ////                     ////    ////
            ////    ////                     ////    ////
                        ////////    ////////   
                        ////////    ////////   

// hither ist ein derpy workaround
// gaze upon it if indeed you dare
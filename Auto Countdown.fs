/*
{
  "CATEGORIES" : [
    "Generator"
  ],
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "colorInput",
      "TYPE" : "color",
      "DEFAULT" : [
        1.0,
        0.75,
        0,
        1
      ]
    },
    {
      "LABELS" : [
        "10",
        "20",
        "99"
      ],
      "NAME" : "counterMax",
      "TYPE" : "long",
      "DEFAULT" : 2,
      "VALUES" : [
        0,
        1,
        2
      ]
    },
    {
      "NAME" : "strobeOnZero",
      "TYPE" : "bool"
    },
    {
      "NAME" : "fontSize",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "MIN" : 0
    }
  ],
  "PASSES" : [
    {
      "WIDTH" : "1",
      "HEIGHT" : "1",
      "TARGET" : "currentTime",
      "PERSISTENT" : true
    },
    {

    }
  ],
  "CREDIT" : ""
}
*/



float segment(vec2 uv, bool On) {
	return (On) ?  (1.0 - smoothstep(0.05,0.15,abs(uv.x))) *
			       (1.-smoothstep(0.35,0.55,abs(uv.y)+abs(uv.x)))
		        : 0.;
}

float digit(vec2 uv,int num) {
	float seg= 0.;
    seg += segment(uv.yx+vec2(-1., 0.),num!=-1 && num!=1 && num!=4                    );
	seg += segment(uv.xy+vec2(-.5,-.5),num!=-1 && num!=1 && num!=2 && num!=3 && num!=7);
	seg += segment(uv.xy+vec2( .5,-.5),num!=-1 && num!=5 && num!=6                    );
   	seg += segment(uv.yx+vec2( 0., 0.),num!=-1 && num!=0 && num!=1 && num!=7          );
	seg += segment(uv.xy+vec2(-.5, .5),num==0 || num==2 || num==6 || num==8           );
	seg += segment(uv.xy+vec2( .5, .5),num!=-1 && num!=2                              );
    seg += segment(uv.yx+vec2( 1., 0.),num!=-1 && num!=1 && num!=4 && num!=7          );	
	return seg;
}

float showNum(vec2 uv,int nr, bool zeroTrim) { // nr: 2 digits + sgn . zeroTrim: trim leading "0"
	if (abs(uv.x)>2.*1.5 || abs(uv.y)>1.2) return 0.;

	if (nr<0) {
		nr = -nr;
		if (uv.x>1.5) {
			uv.x -= 2.;
			return segment(uv.yx,true); // minus sign.
		}
	}
	
	if (uv.x>0.) {
		nr /= 10; if (nr==0 && zeroTrim) nr = -1;
		uv -= vec2(.75,0.);
	} else {
		uv += vec2(.75,0.); 
		nr = int(mod(float(nr),10.));
	}

	return digit(uv,nr);
}



void main()	{
	vec4		returnMe = vec4(0.0);

	float		maxTime = 10.99;
		
	if (counterMax == 1)
		maxTime = 20.99;
	else if (counterMax == 2)
		maxTime = 99.99;
	
	int		displayTime = int(maxTime - TIME);
	displayTime = (displayTime > 0) ? displayTime : 0;
	vec2	loc = isf_FragNormCoord;
	loc.x = 1.0 - loc.x;
	loc = (loc * 3.0 - 1.5) / fontSize;
	float	seg = showNum(loc,displayTime,true);
	returnMe = colorInput * seg;
	
	if ((strobeOnZero == true)&&(displayTime == 0))	{
		returnMe.rgb = (mod(float(FRAMEINDEX),2.0) < 1.0) ? returnMe.rgb : 1.0 - returnMe.rgb;	
	}
	
	gl_FragColor = returnMe;
}

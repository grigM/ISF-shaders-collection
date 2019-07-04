/*
{
  "CATEGORIES" : [
    "Generator"
  ],
  "ISFVSN" : "2",
  "INPUTS" : [
    {
      "NAME" : "resetTimer",
      "TYPE" : "event"
    },
    {
      "NAME" : "decTimer",
      "TYPE" : "event"
    },
    {
      "NAME" : "incTimer",
      "TYPE" : "event"
    },
    {
      "NAME" : "colorInput",
      "TYPE" : "color",
      "DEFAULT" : [
        0.003076895238153715,
        0.9597020149230957,
        0.07250135896775131,
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
      "DEFAULT" : 0,
      "VALUES" : [
        0,
        1,
        2
      ]
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


float	pixelDecrement = 1.0 / 255.0;



void main()	{
	vec4		returnMe = vec4(0.0);
	
	//	Time can be set as either time until midnight or a manual countdown timer
	if (PASSINDEX == 0)	{
		float		maxTime = 10.0 * pixelDecrement;
		
		if (counterMax == 1)
			maxTime = 20.0 * pixelDecrement;
		else if (counterMax == 2)
			maxTime = 99.0 * pixelDecrement;

		//	If we're in countdown timer mode, store that value here
		if ((resetTimer == true)||(FRAMEINDEX==0))
			returnMe = vec4(maxTime);
		else
			returnMe = IMG_PIXEL(currentTime,gl_FragCoord.xy);
		
		if (decTimer == true)	{
			if (returnMe.a >= pixelDecrement)
				returnMe = returnMe - vec4(pixelDecrement);
			else
				returnMe = vec4(0.0);
		}
		if (incTimer == true)	{
			if (returnMe.a <= maxTime - pixelDecrement)
				returnMe = returnMe + vec4(pixelDecrement);
			else
				returnMe = vec4(maxTime);
		}
		if (returnMe.a > maxTime)
			returnMe = vec4(maxTime);
	}
	else	{
		int		displayTime = 10;
		vec4	tmpVal = IMG_PIXEL(currentTime,vec2(0.5,0.5));
		displayTime = int(tmpVal.a * 255.0);
		vec2	loc = isf_FragNormCoord;
		loc.x = 1.0 - loc.x;
		loc = (loc * 3.0 - 1.5) / fontSize;;
		float	seg = showNum(loc,displayTime,true);
		returnMe = colorInput * seg;
	}
	
	gl_FragColor = returnMe;
}

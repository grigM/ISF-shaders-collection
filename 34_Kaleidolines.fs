/*
{
  "CATEGORIES" : [
    "Generator"
  ],
  "DESCRIPTION" : "",
  "INPUTS" : [
    {
      "NAME" : "invert",
      "TYPE" : "bool",
      "DEFAULT" : true
    },
    {
      "NAME" : "zoom",
      "TYPE" : "float",
      "MAX" : 20,
      "DEFAULT" : 3.3019793033599854,
      "MIN" : 0.25
    },
    {
      "NAME" : "rotateCanvas",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.45078930258750916,
      "MIN" : 0
    },
    {
      "NAME" : "rotateLines",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.2777867317199707,
      "MIN" : 0
    },
    {
      "NAME" : "lineThickness",
      "TYPE" : "float",
      "MAX" : 20,
      "DEFAULT" : 5.9209065437316895,
      "MIN" : 0.10000000000000001
    },
    {
      "NAME" : "lineLength",
      "TYPE" : "float",
      "MAX" : 10,
      "DEFAULT" : 2.6872844696044922,
      "MIN" : 0.050000000000000003
    },
    {
      "NAME" : "lines1",
      "TYPE" : "float",
      "MAX" : 10,
      "DEFAULT" : 6.8404679298400879,
      "MIN" : 1
    },
    {
      "NAME" : "lines2",
      "TYPE" : "float",
      "MAX" : 10,
      "DEFAULT" : 9.7506446838378906,
      "MIN" : 1
    },
    {
      "NAME" : "offset",
      "TYPE" : "float",
      "MAX" : 2,
      "DEFAULT" : 0.54888850450515747,
      "MIN" : -2
    },
    {
      "NAME" : "motion",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.82939350605010986,
      "MIN" : 0
    },
    {
      "NAME" : "pos",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "DEFAULT" : [
        0.5,
        0.5
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "ISFVSN" : "2",
  "CREDIT" : ""
}
*/


#define TAU 6.28318530718

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

vec2 tile(vec2 _st, float _zoom){
  _st *= _zoom;
  return fract(_st);
}

float segment(vec2 p, vec2 a, vec2 b) {
    vec2 ab = b - a;
    vec2 ap = p - a;
    float k = clamp(dot(ap, ab)/dot(ab, ab), 0.0, lineLength);
    return smoothstep(0.0, 0.003 + lineThickness/RENDERSIZE.y, length(ap - k*ab) - (0.001 * lineThickness * 5. ));
}

float shape(vec2 p, float angle) {
    float d = 100.0;
    vec2 a = vec2(1.0, 0.0), b;
    vec2 rot = vec2(cos(angle), sin(angle));
    
    for (int i = 0; i < 10; ++i) {
    	        a = a + offset;
    	if (i >= int(lines1)) break;
        b = a;
        for (int j = 0; j < 10; ++j) {
        	if (j >= int(lines2)) break;
        	b = vec2(b.x*rot.x - b.y*rot.y, b.x*rot.y + b.y*rot.x);
        	p = rotate2d(rotateLines* -TAU) * p;
        	d = min(d, segment(p,  a, b));
        }
        a = vec2(a.x*rot.x - a.y*rot.y, a.x*rot.y + a.y*rot.x);

    }
    return d;
}

void main() {

    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv -= vec2(pos);
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
	uv = rotate2d(rotateCanvas *-TAU) * uv;
	uv *= zoom;
        
    float col = shape(abs(uv), cos((motion * TAU)));
    
    if  (invert) col = col *-1.0 + 1.0;
    
    gl_FragColor = vec4(vec3(col), 1.0);
}
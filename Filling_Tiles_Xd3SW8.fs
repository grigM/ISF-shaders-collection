/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/Xd3SW8 by Hanley.  Playing with moving square fills. ",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    },
    
    {
      "NAME" : "colorize",
      "TYPE" : "bool",
      "DEFAULT": false
    },
  ]
}
*/


#define PI 3.14159
#define TWO_PI 6.283185

float polygonDistanceField(in vec2 pixelPos, in int N) {
    // N = number of corners
    float a = atan(pixelPos.y, pixelPos.x) + PI/2.; // angle
    float r = TWO_PI/float(N); // ~?
    // shapping function that modulates the distances
    float distanceField = cos(floor(0.5 + a/r) * r - a) * length(pixelPos);
    return distanceField;
}

float minAngularDifference(in float angleA, in float angleB) {
    // Ensure input angles are -Ï€ to Ï€
    angleA = mod(angleA, TWO_PI);
    if (angleA>PI) angleA -= TWO_PI;
    if (angleA<PI) angleA += TWO_PI;
    angleB = mod(angleB, TWO_PI);
    if (angleB>PI) angleB -= TWO_PI;
    if (angleB<PI) angleB += TWO_PI;

    // Calculate angular difference
    float angularDiff = abs(angleA - angleB);
    angularDiff = min(angularDiff, TWO_PI - angularDiff);
    return angularDiff;
}

float map(in float value, in float istart, in float istop, in float ostart, in float ostop) {
    return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
}
float mapAndCap(in float value, in float istart, in float istop, in float ostart, in float ostop) {
    float v = map(value, istart, istop, ostart, ostop);
    v = max( min(ostart,ostop), v);
    v = min( max(ostart,ostop), v);
    return v;
}


// Matrix Transforms
mat2 rotate2d(float angle);
mat2 scale(vec2 scale);

void main() {



    float u_time = TIME;
  	vec2 u_mouse = iMouse.xy;
  	vec2 u_resolution = RENDERSIZE.xy;
    
    vec3 color = vec3(0.2);
    float t = u_time;
    vec2 mouse_n = u_mouse.xy / u_resolution;
    vec2 st = gl_FragCoord.xy / u_resolution.xy;
    st.x *= u_resolution.x / u_resolution.y; // quick aspect ratio fix
    // manip st grid - into 3x3 tiles
    float divisions = 4.;
    vec2 mst = st;
    mst *= divisions;
    // give each cell an index number according to position (left-right, down-up)
    float index = 0.;
    float cellx = floor(mst.x);
    float celly = floor(mst.y);
    index += floor(mst.x);
    index += floor(mst.y)*divisions;
    // tile mst
    mst = mod(mst, 1.);
    
    ////
    // draw square tile
    
    // t = 1.6;
    float tt = t-(sin(cellx*.3)+cos(celly*.3))*.5; //t * .3;
    float squareProgress = mod(tt*.3, 1.); //0.22; // mouse_n.x; //0.2; //mod(t*.3, 1.);
    float squareEntryProgress = mapAndCap(squareProgress, 0., 0.6, 0., 1.); //mod(t*.7, 1.); //mouse_n.x;
    float squareExitProgress = mapAndCap(squareProgress, 0.9, .999, 0., 1.);
        squareExitProgress = pow(squareExitProgress, 3.);
    float borderProgress = mapAndCap(squareEntryProgress,0.,0.55,0.,1.);
        borderProgress = pow(borderProgress, 1.5);
    float fillProgress = mapAndCap(squareEntryProgress,0.4, 0.9, 0., 1.);
        fillProgress = pow(fillProgress, 4.);
    // MATRIX MANIP
    mst = mst*2.-1.; // centre origin point
    // rotate
    // mst = rotate2d(floor(mod(index,2.))*PI*.5 + PI*.25)*mst;
    mst = rotate2d(cellx*PI*.5 + celly*PI*.5 + PI*.25)*mst;
    float d = polygonDistanceField(mst, 4);
    float r = map(squareExitProgress, 0., 1., 0.7, 0.); // 0.5;
    float innerCut = map(fillProgress, 0., 1., 0.9, 0.0001); //0.9; //mouse_n.x;
    float buf = 1.01;
    float shape = smoothstep(r*buf, r, d) - smoothstep(r*innerCut, r*innerCut/buf, d);
    // add smoother shape glow
    buf = 1.5;
    float shape2 = smoothstep(r*buf, r, d) - smoothstep(r*innerCut, r*innerCut/buf, d);
    // shape += shape2*.5;
    // angular mask on square tile
    float sta = atan(mst.y, mst.x); // st-angle - technically its msta here
    float targetAngle = map(borderProgress, 0., 1., 0., PI)+PI*.251;
    float adiff = minAngularDifference(sta, targetAngle);
    float arange = map(borderProgress, 0., 1., 0., PI);
    float amask = 1. - smoothstep(arange, arange, adiff);
    shape *= amask;
    // color
    // color = vec3(shape) * vec3(0.8, 0.6, 0.8)*2.;
    if(colorize){
    	color = vec3(shape) * (vec3(1.-st.x, st.y, st.y)+vec3(.2));
    }else{
    	color = vec3(shape);
    }
    //color += vec3(mst.y, 0., mst.x);
    
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	gl_FragColor = vec4(color,1.0);
}


// rotate matrix
mat2 rotate2d(float angle) {
    return mat2(cos(angle), -sin(angle),
                sin(angle),  cos(angle) );
}

// scale matrix
mat2 scale(vec2 scale) {
    return mat2(scale.x, 0,
                0, scale.y);
}

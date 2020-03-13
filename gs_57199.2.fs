/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#57199.2"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


float random (in vec2 _st) {
    return fract(sin(dot(_st.xy,
                         vec2(12.9898,78.233)))*
        43758.5453123);
}

float offset(in float _value) {
    return sin(TIME)*_value;
}

vec2 truchetPattern(in vec2 _st, in float _index){
    _index = fract(((_index-0.5)*2.0));
    if (_index + offset(0.25) > 0.75) {
        _st = vec2(1.0) - _st;
    } else if (_index + offset(0.25) > 0.5) {
        _st = vec2(1.0-_st.x,_st.y);
    } else if (_index + offset(0.25) > 0.25) {
        _st = 1.0-vec2(1.0-_st.x,_st.y);
    }
    return _st;
}

void main( void ) {

    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
    st *= 20.0;
    // st = (st-vec2(5.0))*(abs(sin(TIME*0.2))*5.);
    // st.x += TIME*3.0;

    vec2 ipos = floor(st);  // integer
    vec2 fpos = fract(st);  // fraction

    vec2 tile = truchetPattern(fpos, random( ipos ));

    float color = 0.0;

    // Maze
    color = smoothstep(tile.x-0.1,tile.x,tile.y)-
            smoothstep(tile.x,tile.x+0.1,tile.y);

    // Circles
	/*
    color = (step(length(tile),0.6) -
             step(length(tile),0.4) ) +
            (step(length(tile-vec2(1.)),0.6) -
             step(length(tile-vec2(1.)),0.4) );
	*/


    // Truchet (2 triangles)
    // color = step(tile.x,tile.y);

    gl_FragColor = vec4(vec3(color),1.0);

}
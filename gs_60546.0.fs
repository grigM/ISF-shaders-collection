/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
            "NAME": "enable_stars",
            "TYPE": "bool",
           "DEFAULT": 0,
            
          },
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#60546.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


#define PI 3.141592653589793238462
float tri(float x) {
    return (asin(sin(x))/(PI/2.));
}
float postri(float x) {
    return ((asin(sin(x))/(PI/2.))/2.)+1.;
}
float puls(float x) {
    return (floor(sin(x))+0.5)*2.;
}
float saw(float x) {
    return (fract((x/2.)/PI)-0.5)*2.;
}
float noise(float x) {
    return (fract(sin((x*2.) *(12.9898+78.233)) * 43758.5453)-0.5)*2.;
}
float distanceToSegment( in vec2 p, in vec2 a, in vec2 b, float size )
{
    //iq's function
	vec2 pa = p - a;
	vec2 ba = b - a;
	float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
	
	return 1.-clamp((length( pa - ba*h )-size)*500.,0.,1.);
}
vec3 hsl2rgb( in vec3 c )
{
    vec3 rgb = clamp( abs(mod(c.x*6.0+vec3(0.0,4.0,2.0),6.0)-3.0)-1.0, 0.0, 1.0 );

    return c.z + c.y * (rgb-0.5)*(1.0-abs(2.0*c.z-1.0));
}

void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );
	vec2 uv = vec2(((position.x-0.5)*(RENDERSIZE.x/RENDERSIZE.y))+0.5,position.y);
	
	vec2 enlarge = vec2(1.9,1.0);
	
	vec2 array[3]; //Create array
	float i = 0.;
	array[0] = (vec2(tri(((TIME*4.2)+1.)+i  )*enlarge.x,tri(((TIME)*1.3)+i  )*enlarge.y )/2.)+1.; //We start at zero
	array[1] = (vec2(tri(((TIME*1.53)+1.)+i  )*enlarge.x,tri(((TIME+0.5)*2.8)+i  )*enlarge.y )/2.)+1.;
	array[2] = (vec2(tri(((TIME+0.1)*1.5)+i  )*enlarge.x,tri(((TIME+2.)*1.31)+i)*enlarge.y )/2.)+1.;
	
	float draw = 0.;
	float draw2 = 0.;
	float width_thick = 0.002;
	vec3 final_drawing = vec3(0.,0.,0.);

	draw = distanceToSegment(uv,array[0]-0.5,array[1]-0.5,width_thick);
	draw += distanceToSegment(uv,array[1]-0.5,array[2]-0.5,width_thick);
	draw = clamp(draw,0.,1.);
	final_drawing = hsl2rgb(vec3(0.,1.,draw/1.6));
	float mod_time = mod(TIME,2.0);
	for (float i = 0.; i < 1.8; i+=0.07) {
		if (i < mod_time) {
		array[0] = (vec2(tri(((TIME*4.2)+1.)+i  )*enlarge.x,tri(((TIME)*1.3)+i  )*enlarge.y )/2.)+1.; //We start at zero
		array[1] = (vec2(tri(((TIME*1.53)+1.)+i  )*enlarge.x,tri(((TIME+0.5)*2.8)+i  )*enlarge.y )/2.)+1.;
		array[2] = (vec2(tri(((TIME+0.1)*1.5)+i  )*enlarge.x,tri(((TIME+2.)*1.31)+i)*enlarge.y )/2.)+1.;
		draw2 = distanceToSegment(uv,array[0]-0.5,array[1]-0.5,width_thick);
		draw2 += distanceToSegment(uv,array[1]-0.5,array[2]-0.5,width_thick);
		draw2 = clamp(draw2,0.,1.);
		draw = clamp(draw+draw2,0.,1.);
		final_drawing += mix(vec3(0.),clamp(hsl2rgb(vec3(i*3.,1.,draw2/1.6)),0.,1.),clamp(1.-(final_drawing*3.),0.,1.));
		}
	}
	
	float stars = 0.0; //Weird glitch clamp when setting min to 0. Also blinking for some rare reason.
	if(enable_stars){
		stars = clamp(pow(noise((uv.x+2.)*(uv.y+2.)),200.),0.001,1.);
	}
	gl_FragColor = vec4(clamp(final_drawing,0.,1.)+stars, 1.0 );
}
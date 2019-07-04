/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
	{
      "NAME" : "speed",
      "TYPE" : "float",
      "DEFAULT" : 1.0,
      "MIN" : 0.0,
      "MAX" : 8.0
    },
     {
      "NAME" : "rays_count",
      "TYPE" : "float",
      "DEFAULT" : 1.0,
      "MIN" : 0.0,
      "MAX" : 80.0
    }
    ,
     {
      "NAME" : "scale",
      "TYPE" : "float",
      "DEFAULT" : 0.7,
      "MIN" : 0.05,
      "MAX" : 3.0
    }
    ,
     {
      "NAME" : "cam_y_speed",
      "TYPE" : "float",
      "DEFAULT" : 0.05,
      "MIN" : 0.0,
      "MAX" : 1.2
    },
     {
      "NAME" : "ball_res",
      "TYPE" : "float",
      "DEFAULT" : 120,
      "MIN" : 40.0,
      "MAX" : 300.0
    }
    
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#25363.1"
}
*/


#ifdef GL_ES
precision mediump float;
#endif


uniform sampler2D tex0;
uniform sampler2D tex1;
/// mr Iq shader toool ;
vec4 s(vec2 px,float z)
{
    float l=3.1415;
    float k=(TIME*speed)*sign(z);
    float x = px.x*640.0*.0060*z;
    float y = px.y*640.0*.0060*z;
    float c=sqrt(x*x+y*y);
    if(c>1.0)
    {
        return vec4(0.0);
    }
    else
    {
        float u=-.4*sign(z)+sin(k*cam_y_speed);
        float v=sqrt(1.0-x*x-y*y);
        float q=y*sin(u)-v*cos(u);
        y=y*cos(u)+v*sin(u);
        v=acos(y);
        u=acos(x/sin(v))/(2.0*l)*float(int(ball_res))*sign(q)-k;
        v=v*float(int(ball_res/2.0))/l;
        q=cos(floor(v/l));
        c=pow(abs(cos(u)*sin(v)),.2)*.1/(q+sin(float(int((u+l/2.0)/l))+k*.6+cos(q*25.0)))*pow(1.0-c,.9);

        vec4 res;
        if(c<0.0)
           res = vec4(-c/2.0*abs(cos(k*.1)),0.0,-c*2.0*abs(sin(k*.04)),1.0);
        else
           res = vec4(c,c*2.0,c*2.0,1.0);
        return res;
    }
}

void main(void)
{
    //vec2 p = -1.0 + 2.0 * gl_FragCoord.xy / RENDERSIZE.xy;
    
    vec2 p = 2.0*( gl_FragCoord.xy / RENDERSIZE.xy )-1.0;
	p.x *= RENDERSIZE.x/RENDERSIZE.y; 



    vec4 c = vec4(0.0);
    for(int i=int(rays_count);i>0;i--) // change to 80 cant save with 80 .. fast GPU need
        c+=s(p,scale-float(i)/80.0)*(.008-float(i)*.00005);
    vec4 d=s(p,scale);
    gl_FragColor = (d.a==0.0?s(p,-.2)*.02:d)+sqrt(c);
}
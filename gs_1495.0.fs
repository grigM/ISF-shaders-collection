/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#1495.0",
  "INPUTS" : [
	  	{
      "NAME" : "max_dof",
      "TYPE" : "float",
      "MAX" : 100000.0,
      "DEFAULT" : 50000.0,
      "MIN" : 1.0
    },
    {
      "NAME" : "camSpeed",
      "TYPE" : "float",
      "MAX" : 15,
      "DEFAULT" : 1,
      "MIN" : 0
    },
    {
      "NAME" : "cam_target_y",
      "TYPE" : "float",
      "MAX" : 4,
      "DEFAULT" : 1.0,
      "MIN" : -4
    },
    
    {
      "NAME" : "floatWAVESpeed",
      "TYPE" : "float",
      "MAX" : 10,
      "DEFAULT" : 0.80000000000000004,
      "MIN" : 0
    },
    {
      "NAME" : "floatWAVEAmpl",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.50000000000000001,
      "MIN" : 0
    },
    {
      "NAME" : "resX",
      "TYPE" : "float",
      "MAX" : 50,
      "DEFAULT" : 20,
      "MIN" : 0
    },
    {
      "NAME" : "resY",
      "TYPE" : "float",
      "MAX" : 40,
      "DEFAULT" : 20,
      "MIN" : 0
    },
    {
      "NAME" : "resStepX",
      "TYPE" : "float",
      "MAX" : 80,
      "DEFAULT" : 20,
      "MIN" : 0
    },
    {
      "NAME" : "resStepY",
      "TYPE" : "float",
      "MAX" : 80,
      "DEFAULT" : 20,
      "MIN" : 0
    },
    {
      "NAME" : "resPosX",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "MIN" : -1
    },
    {
      "NAME" : "resPosY",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "MIN" : -1
    },
    {
      "NAME" : "displayMoon",
      "TYPE" : "bool",
      "DEFAULT" : 1,
    },
    {
      "NAME" : "moonSize",
      "TYPE" : "float",
      "MAX" : 800,
      "DEFAULT" : 400,
      "MIN" : 0
    },
    {
      "NAME" : "moonPosX",
      "TYPE" : "float",
      "MAX" : 50,
      "DEFAULT" : 10,
      "MIN" : -50
    },
    {
      "NAME" : "moonPosY",
      "TYPE" : "float",
      "MAX" : 50,
      "DEFAULT" : 0,
      "MIN" : -50
    },
    {
      "NAME" : "moonPosZ",
      "TYPE" : "float",
      "MAX" : 50,
      "DEFAULT" : 50,
      "MIN" : -50
    }
  ],
  "ISFVSN" : "2"
}
*/


// "moon river" by @eddbiddulph

#ifdef GL_ES
precision mediump float;
#endif


vec3 cam_pos;
mat3 cam_rot;


vec3 transform(vec3 p)
{
   return cam_rot * (p - cam_pos);
}

vec2 project(vec3 p)
{
   return p.xy / p.z;
}

float gauss(float x, float a)
{
   return sqrt(a / 3.14159265) * exp(-a * x * x);
}

float blob(vec3 o, vec2 p)
{
	return gauss(distance(project(o), p), max_dof / abs(o.z - 6.01)) * 0.01;
}

void main()
{
   vec2 p = (gl_FragCoord.xy / RENDERSIZE.xy - 0.5) * vec2(RENDERSIZE.x / RENDERSIZE.y, 1.0) * 1.0;
	
   cam_pos = vec3(cos((TIME*camSpeed) * 0.1) * 5.0, cam_target_y,  -7.0 + 1.0 + sin((TIME*camSpeed) * 0.07));

   vec3 cam_target = vec3(0.0, 1.0, 3.0);

   cam_rot[2] = normalize(cam_target - cam_pos);

   cam_rot[0] = cross(cam_rot[2], vec3(0.0, 1.0, 0.0));
   cam_rot[1] = cross(cam_rot[2], cam_rot[0]);

   gl_FragColor.rgb = vec3(0.0);

   for(int i = 0; i < int(resX); ++i)
   {
      for(int j = 0; j < int(resY); ++j)
      {
         float u = (float(i) / resStepX - resPosX) * 10.0,
               v = (float(j) / resStepY - resPosY) * 10.0;

         float h = 1.0 + cos(u - (TIME)) * sin(v + (TIME) * floatWAVESpeed) * floatWAVEAmpl;

         vec3 g = transform(vec3(u + cos(v) * 0.5, h, v + sin(u) * 0.5));

         if(g.z > 0.0)
         {
         	//if(enable_dof){
            gl_FragColor.rgb += blob(g, p.xy) * (2.0 + sin(v * 30.0 + (TIME))) * (2.0 + cos(u * 30.0)) * 0.25;
         	//}
         }
      }
   }
   if(displayMoon){
   gl_FragColor.rgb += blob(transform(vec3(moonPosX, moonPosY, moonPosZ)), p.xy) * moonSize;
   //gl_FragColor.rgb += blob(transform(vec3(0.0, 5.0, 50.0)), p.xy) * 50.2;
   }

   gl_FragColor.a = 1.0;
}
/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "terrain",
    "raymarch",
    "triangles",
    "heightmap",
    "mesh",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/ltfXWr by Flyguy.  Testing ray-marched heightmap terrain made up of vertical triangular prisms to mimic the look of low-poly, flat-shaded mesh terrain. \nIncrease\/decrease QUAD_SIZE to make the terrain less\/more detailed.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/


#define MIN_DIST 0.001
#define MAX_STEPS 96
#define STEP_MULT 0.5
#define NORMAL_OFFS 0.01
#define QUAD_SIZE 0.5

float pi = atan(1.0)*4.0;
float tau = atan(1.0)*8.0;

//Returns the height at a given position.
float Height(vec2 p)
{
    p *= QUAD_SIZE;
    p *= 8.0;
    
    float h = 0.0;
    
	h += 0.5 * sin(p.x * 0.10 + 0.0);
    h += 0.2 * sin(p.x * 0.60 + 0.5 - TIME);
    h += 1.0 * sin(p.x * 0.02 + 1.2 + TIME);
    h += 0.6 * sin(p.y * 0.30 + 0.8);
    h += 0.4 * sin(p.y * 0.10 + 1.2 + TIME);
    h += 1.0 * sin(p.y * 0.05 + 1.2 + TIME);
    
    h *= smoothstep(8.0,16.0,length(p));
    
	return h * 0.5;
}

//Returns a rotation matrix for the given angles around the X,Y,Z axes.
mat3 Rotate(vec3 angles)
{
    vec3 c = cos(angles);
    vec3 s = sin(angles);
    
    mat3 rotX = mat3( 1.0, 0.0, 0.0, 0.0,c.x,s.x, 0.0,-s.x, c.x);
    mat3 rotY = mat3( c.y, 0.0,-s.y, 0.0,1.0,0.0, s.y, 0.0, c.y);
    mat3 rotZ = mat3( c.z, s.z, 0.0,-s.z,c.z,0.0, 0.0, 0.0, 1.0);

    return rotX*rotY*rotZ;
}

//==== Distance field operators/functions by iq. ====
float opU( float d1, float d2 )
{
    return min(d1,d2);
}

float opS( float d1, float d2 )
{
    return max(-d1,d2);
}

float sdSphere( vec3 p, float s )
{
  return length(p)-s;
}

//Modified to create a plane from 3 points.
float sdPlane( vec3 p, vec3 p0, vec3 p1, vec3 p2 )
{
  return dot(p - p0,normalize(cross(p0 - p1, p0 - p2)));
}
//===================================================

/*
Distance to a vertical quad comprised of two triangles.

1-----2
| 0  /|
|  /  |
|/  1 |
0-----3
*/
float sdVQuad( vec3 p, float h0, float h1, float h2, float h3)
{
    float s = QUAD_SIZE;
       
    float diag = sdPlane(p, vec3(0,0,0),vec3(s,s,0),vec3(0,0,s));
    
    float tri0 = sdPlane(p, vec3(0,0,-h0),vec3(0,s,-h1),vec3(s,s,-h2)); //Triangle 0 (0,1,2)
    tri0 = opS(-diag, tri0);
    
    float tri1 = sdPlane(p, vec3(0,0,-h0),vec3(s,s,-h2),vec3(s,0,-h3)); //Triangle 1 (0,2,3)
    tri1 = opS(diag, tri1);
    
    float d = min(tri0,tri1);
    
    return d;
}

float Scene(vec3 p)
{
    float d = 1000.0;
    
    vec3 pm = vec3(mod(p.xy,vec2(QUAD_SIZE)),p.z);
    
    vec2 uv = floor(p.xy/QUAD_SIZE);
    
    d = sdVQuad(pm - vec3(0,0,1.0), Height(uv+vec2(0,0)),Height(uv+vec2(0,1)),Height(uv+vec2(1,1)),Height(uv+vec2(1,0)));
    
    d = opU(d, -sdSphere(p,12.0));
    
	return d;
}

vec3 MarchRay(vec3 origin,vec3 dir)
{
    vec3 marchPos = origin;
    for(int i = 0;i < MAX_STEPS;i++)
    {
        float sceneDist = Scene(marchPos);
        
        marchPos += dir * sceneDist * STEP_MULT;
        
        if(sceneDist < MIN_DIST)
        {
            break;
        }
    }
    
    return marchPos;
}

vec3 Normal(vec3 p)
{
    vec3 off = vec3(NORMAL_OFFS,0,0);
    return normalize
    ( 
        vec3
        (
            Scene(p+off.xyz) - Scene(p-off.xyz),
            Scene(p+off.zxy) - Scene(p-off.zxy),
            Scene(p+off.yzx) - Scene(p-off.yzx)
        )
    );
}

vec3 Shade(vec3 position, vec3 normal, vec3 direction, vec3 camera)
{
    vec3 color = vec3(1.0);
    
    vec2 gridRep = mod(position.xy, vec2(QUAD_SIZE)) / float(QUAD_SIZE) - 0.5;
    
    float grid = 0.5 - max(abs(gridRep.x), abs(gridRep.y));
    grid = min(grid,abs(dot(gridRep.xy, normalize(vec2(-1, 1)))));
        
    float lineSize = 0.8 * length(fwidth(position)) / float(QUAD_SIZE);
    
    color *= smoothstep(lineSize,0.0,grid);
    color = color * 0.75 + 0.25;
    
    float ambient = 0.1;
    float diffuse = 0.5 * -dot(normal,direction);
    float specular = 1.1 * max(0.0, -dot(direction, reflect(direction,normal)));
    
    color *= vec3(ambient + diffuse + pow(specular,5.0));

    color *= smoothstep(12.0,6.0,length(position));
    
    return color;
}

void main()
{
    vec2 res = RENDERSIZE.xy / RENDERSIZE.y;
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.y;
    
    vec3 angles = vec3((iMouse.xy/RENDERSIZE.xy)*pi,0);
    
    angles.xy *= vec2(2.0,1.0);
    angles.y = clamp(angles.y,0.0,tau/4.0);
    
    mat3 rotate = Rotate(angles.yzx);
    
    vec3 orig = vec3(0,0,-2) * rotate;
    vec3 dir = normalize(vec3(uv - res/2.0,0.5)) * rotate;
    
    vec3 hit = MarchRay(orig,dir);
    vec3 norm = Normal(hit);
    
    vec3 color = Shade(hit,norm,dir,orig);
    
	gl_FragColor = vec4(color,1.0);
}
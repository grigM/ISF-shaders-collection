/*
{
  "IMPORTED" : [

  ],
  "CATEGORIES" : [
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MdB3zK by arthursw.  A simple function to draw a segment defined by two points, with square or rounded end.",
  "INPUTS" : [
    {
      "NAME" : "iMouse",
      "TYPE" : "point2D"
    }
  ]
}
*/



vec2 rotate(vec2 v, float alpha)
{
	float vx = v.x*cos(alpha)-v.y*sin(alpha);
	float vy = v.x*sin(alpha)+v.y*cos(alpha);
	v.x = vx;
	v.y = vy;
	return v;
}

// lineVecor must be normalized
float distancePointLine(vec2 linePoint, vec2 lineVector, vec2 point)
{
	vec2 linePointToPoint = point-linePoint;
	float projectionDistance = dot(lineVector,linePointToPoint);
	return length(lineVector*projectionDistance-linePointToPoint);
}

float distToLineSquare(vec2 p1, vec2 p2, vec2 p, float thickness)
{
	p -= p1;
	vec2 lineVector = p2-p1;
		
	float angle = -atan(lineVector.y,lineVector.x);
	p = rotate(p,angle);
	
	float dx = 0.0;
	if(p.x<0.0)
		dx = abs(p.x);
	else if(p.x>length(lineVector))
		dx = abs(p.x) - length(lineVector);
		
	return thickness/(dx+abs(p.y));
}

float distToLineRound(vec2 p1, vec2 p2, vec2 p, float thickness)
{
	float d = length(p-p2);
	p -= p1;
	vec2 lineVector = p2-p1;
		
	float angle = -atan(lineVector.y,lineVector.x);
	p = rotate(p,angle);

	if(p.x<0.0)
		d = length(p);
	else if(p.x<length(lineVector))
		d = abs(p.y);
		
	return thickness/d;
}

float squareDrop(vec2 p1, vec2 p2, vec2 p, float thickness)
{
	float d = 1.0; //length(p-p2);
	p -= p1;
	vec2 lineVector = p2-p1;
		
	float angle = -atan(lineVector.y,lineVector.x);
	p = rotate(p,angle);

	float llv = length(lineVector);
	if(p.x<0.0)
		d = 0.1*length(p);
	else if(p.x<llv)
		d = (llv/(llv-p.x)*0.1)*abs(p.y);
		
	return thickness/d;
}

float expDrop(vec2 p1, vec2 p2, vec2 p, float thickness)
{
	float d = 1.0; //length(p-p2);
	p -= p1;
	vec2 lineVector = p2-p1;
		
	float angle = -atan(lineVector.y,lineVector.x);
	p = rotate(p,angle);

	float llv = length(lineVector);
	if(p.x<0.0)
		d = 0.06*length(p);
	else if(p.x<llv)
		d = exp(10.0*(p.x-0.05))*(llv/(llv-p.x)*0.1)*abs(p.y);
		
	return thickness/d;
}

void main() {



	vec2 p = gl_FragCoord.xy / RENDERSIZE.xx;
	vec2 m = iMouse.xy / RENDERSIZE.xx;
	p -= vec2(0.5,0.5*RENDERSIZE.y/RENDERSIZE.x);
	m -= vec2(0.5,0.5*RENDERSIZE.y/RENDERSIZE.x);
	
	vec2 o1 = vec2(0.15,0.15);
	vec2 o2 = vec2(0.15,0.1);
	vec2 o3 = vec2(0.4,0.0);
	vec2 o4 = vec2(0.25,0.0);
	
	float angle = 1.0*TIME;
	o1 = rotate(o1,angle);
	
	
	angle = 2.0*TIME;
	o2 = rotate(o2,angle);
	float thickness = 0.01;
	float dist = 0.0;
	dist += distToLineSquare(o1,o2,p,thickness);
	dist += distToLineSquare(o1,-o2,p,thickness);
	o1.y *= -1.0;
	o2.y *= -1.0;
	dist += distToLineRound(o1,o2,p,thickness);
	dist += distToLineRound(o1,-o2,p,thickness);
	dist += squareDrop(o3,o4,p,thickness*0.5);
	dist += expDrop(-o3,-o4*0.5,p,thickness*0.35);
	gl_FragColor = vec4(dist*vec3(0.36,0.32,0.45),1.0);
}

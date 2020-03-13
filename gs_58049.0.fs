/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [
    {
      "NAME" : "mouse",
      "TYPE" : "point2D",
      "MAX" : [
        1,
        1
      ],
      "MIN" : [
        0,
        0
      ]
    }
  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#58049.0"
}
*/


#ifdef GL_ES
precision highp float;
#endif


void main( void ) {

	vec2 position = ( gl_FragCoord.xy / RENDERSIZE.xy );

	float color = 1.0;	
	float vx, vy, vz, vxr;
	float dx, llx, dy, lly, dz, llz;
	int px, py, pz, ccc, P;
	float k, k2;
	ivec4 di;
	vec4 X;
	vec4 d=vec4(TIME,mouse.y+3.0,.0,.0);
	vx=(position.x-1.0)+0.0001;

	vy=(position.y -1.0)+0.0001;
	vz=0.5+.0001;
	vxr=(vx*cos(mouse.x*2.5)+vz*sin(mouse.x*2.5));
	 vz=(-vx*sin(mouse.x*2.5)+vz*cos(mouse.x*2.5));
	vx=vxr;
	X=fract(d);
	dx = 1000.0/vx; dy = 1000.0/vy; dz = 1000.0/vz;
	px=1; llx=dx*(1.0-X[0]);
	py=16; lly=dy*(1.0-X[1]);
	pz=256; llz=dz*(1.0-X[2]);
	if (dx<.0) {px=-1; dx=-dx; llx=dx*X[0];}
	if (dy<.0) {py=-16; dy=-dy; lly=dy*X[1];}
	if (dz<.0) {pz=-256; dz=-dz; llz=dz*X[2];}
	ccc=0;
	di=ivec4(d[0],d[1],d[2],d[3]);
	P=di[2]*256+di[1]*16+di[0];
	color=1.0;
	for (int i=0; i<40; i++)
	{
		if ((llx<=lly) && (llx<=llz))
		{
			P+=px; llx+=dx; k=0.75;
		}
		else
		{
			if (lly<=llz)
			{
				P+=py; lly+=dy; k=0.9;
			}
			else
			{
				P+=pz; llz+=dz; k=1.0;
			}
		}
		if ((fract(float(P)/29.0)<.01)&&(color==1.0)) {
			color=float(i)/40.0;
			k2=k;
		}
	}
	gl_FragColor = vec4(k2*(1.0-color),k2*(1.0-color),k2*k2*(1.0-color),1.0 );

}
/*
{
    "CATEGORIES": [
        "Automatically Converted",
        "Shadertoy"
    ],
    "DESCRIPTION": "Automatically converted from https://www.shadertoy.com/view/ttBSWw by 1GR3.  Deltoid Zebra",
    "IMPORTED": {
    },
    "INPUTS": [
    ]
}

*/


#define PI 3.1415926536
#define TWO_PI 6.2831853072
#define time TIME

// --- Config ---
#define totalT 5.0
#define spread 0.5
// Triangle
#define triRadius 0.45

mat2 rotMat2 (in float a);
float vfbmWarp(in vec2 p);

vec3 pattern (in vec2 uv) {
  vec3 color = vec3(0);

  float modT = mod(time, totalT);
  float cosT = TWO_PI / totalT * modT;

  // --- Space warp ---
  vec2 qW = uv;
  qW += vec2(0.3, -0.1);
  qW += 0.050 * cos(3.0 * qW.yx + cosT);
  qW += 0.025 * cos(5.0 * qW.yx + cosT);
  qW *= 1.075 * vfbmWarp(vec2(0.05) * qW);

  const float edge = 0.2;
  const float thickness = 0.5;
  vec2 axis = vec2(0, 1);

  // --- Rotate axis over time ---
  axis *= rotMat2(PI * 0.0625 * sin(TWO_PI * (modT / totalT + 0.5 * dot(qW, vec2(1)))));
  axis *= rotMat2(PI * 0.25 * sin(TWO_PI * (modT / totalT - 0.5 * length(qW))));

  // --- Render lines ---
  float n = smoothstep(thickness, thickness + edge, sin(TWO_PI * 24.0 * dot(qW, axis)));
  color.r = n;
  n = smoothstep(thickness, thickness + edge, sin(TWO_PI * 24.0 * dot(qW, axis) + 0.5 * spread));
  color.g = n;
  n = smoothstep(thickness, thickness + edge, sin(TWO_PI * 24.0 * dot(qW, axis) + spread));
  color.b = n;

  // --- Triangle crop ---
  vec2 q = uv;
  const float cropEdge = 0.01;
  const vec2 point1 = vec2(0, triRadius * 1.414214);
  const vec2 point2 = vec2(0.5 * triRadius, 0);
  // Point 3 is created via horizontal mirror
  q.x = abs(q.x);

  // Height adjustment (Number is tan(30º))
  q.y += 0.5 * triRadius * 0.5773502692;
  q.y += triRadius * 0.333333; // Manual adjustment

  float m = 1.0; // Start w/ everything included
  m *= smoothstep(point2.y - cropEdge, point2.y, q.y); // Bottom edge
  vec2 midPoint = 0.5 * (point1 + point2);
  float d = length(midPoint);

  // Find Perpendicular vector pointing outward
  vec2 midPointPerpendicular = vec2(0, 1) * rotMat2(-0.333333 * PI);

  m *= smoothstep(d, d - cropEdge, dot(q, midPointPerpendicular));
  color *= m;

  return color;
}

// --- Utility functions & noise ---
mat2 rotMat2 (in float a ) {
  float c = cos(a);
  float s = sin(a);
  return mat2(c, -s, s, c);
}


// source: https://www.shadertoy.com/view/4d3fWf
float noise( in vec2 x ) {
  return sin(1.52*x.x)*sin(1.48*x.y);
}

float vfbm4 (vec2 p) {
  float f = 0.0;
  float a = PI * 0.173;
  mat2 m = rotMat2(a);

  f += 0.500000 * noise( p ); p *= m * 2.02;
  f += 0.250000 * noise( p ); p *= m * 2.03;
  f += 0.125000 * noise( p ); p *= m * 2.01;
  f += 0.062500 * noise( p ); p *= m * 2.025;

  return f * 0.9875;
}

float vfbm6 (vec2 p) {
  float f = 0.0;
  float a = 1.123;
  mat2 m = rotMat2(a);

  f += 0.500000 * (0.5 + 0.5 * noise( p )); p *= m * 2.02;
  f += 0.250000 * (0.5 + 0.5 * noise( p )); p *= m * 2.03;
  f += 0.125000 * (0.5 + 0.5 * noise( p )); p *= m * 2.01;
  f += 0.062500 * (0.5 + 0.5 * noise( p )); p *= m * 2.025;
  f += 0.031250 * (0.5 + 0.5 * noise( p )); p *= m * 2.011;
  f += 0.015625 * (0.5 + 0.5 * noise( p )); p *= m * 2.0232;

  return f * 0.9875;
}

float vfbmWarp (vec2 p, out vec2 q, out vec2 s, vec2 r) {
  const float scale = 4.0;
  const float angle = 0.01 * PI;
  float si = sin(angle);
  float c = cos(angle);
  mat2 rot = mat2(c, si, -si, c);

  q = vec2(
        vfbm4(p + vec2(0.0, 0.0)),
        vfbm4(p + vec2(3.2, 34.5)));
  q *= rot;

  s = vec2(
        vfbm4(p + scale * q + vec2(23.9, 234.0)),
        vfbm4(p + scale * q + vec2(7.0, -232.0)));
  s *= rot;

  return vfbm6(p + scale * s);
}

// Overload for 1 argument
float vfbmWarp (vec2 p) {
  vec2 q = vec2(0);
  vec2 s = vec2(0);
  vec2 r = vec2(0);

  return vfbmWarp(p, q, s, r);
}

// --- Render ---
void main() {



    // Normalize to [-1, -1] -> [1, 1]
    vec2 uv = (2.*gl_FragCoord.xy - RENDERSIZE.xy) / RENDERSIZE.y;
    // Output to screen
    gl_FragColor = vec4(pattern(uv),1.0);
}

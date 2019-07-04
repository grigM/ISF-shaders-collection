/*
{
  "CATEGORIES" : [
    "Automatically Converted",
    "GLSLSandbox"
  ],
  "INPUTS" : [

  ],
  "DESCRIPTION" : "Automatically converted from http:\/\/glslsandbox.com\/e#42871.0"
}
*/


#ifdef GL_ES
precision mediump float;
#endif

#extension GL_OES_standard_derivatives : enable


#define iTime TIME
#define iResolution RENDERSIZE

// Initial pass (this is where the beef is!)

// Dedicated to the public domain under CC0 1.0 Universal
//  https://creativecommons.org/publicdomain/zero/1.0/legalcode

#define saturate(x) clamp((x), 0.0, 1.0)

// Noise by iq
float hash( float n ) { return fract(sin(n)*753.5453123); }
float noise( in vec3 x )
{
    vec3 p = floor(x);
    vec3 f = fract(x);
    f = f*f*(3.0-2.0*f);

    float n = p.x + p.y*157.0 + 113.0*p.z;
    return mix(mix(mix( hash(n+  0.0), hash(n+  1.0),f.x),
                   mix( hash(n+157.0), hash(n+158.0),f.x),f.y),
               mix(mix( hash(n+113.0), hash(n+114.0),f.x),
                   mix( hash(n+270.0), hash(n+271.0),f.x),f.y),f.z);
}

float fbm(vec3 x)
{
    float ret = noise(x);
    ret += noise(x * 2.0) / 2.0;
    ret += noise(x * 4.0) / 4.0;
    ret += noise(x * 8.0) / 8.0;
    ret += noise(x * 16.0) / 16.0;
    return ret;
}

float fbm2(vec3 x)
{
    float ret = noise(x);
    ret += noise(x * 2.0) / 2.0;
    ret += noise(x * 4.0) / 4.0;
    return ret;
}

// pos is assumed to be normalized
vec4 glitch(vec2 pos)
{
    float glitchAmount = saturate(pow(abs(sin(iTime * 0.2)), 8.0)) * 0.8;
    float t = iTime * 0.4;

    vec2 cellIndex1 = floor(pos / vec2(0.07, 0.04));
    float offsetX = step(0.8, fbm(vec3(cellIndex1, t)));
    float offsetXSign = step(0.8, fbm(vec3(cellIndex1, t * 1.23) + vec3(30.0))) * 2.0 - 1.0;
    float offsetY = step(0.8, fbm(vec3(cellIndex1, t * 0.995) + vec3(20.0)));
    float offsetYSign = step(0.8, fbm(vec3(cellIndex1, t * 1.23) - vec3(30.0))) * 2.0 - 1.0;
    float offsetScale = step(0.8, fbm(vec3(cellIndex1, t * 2.44) - vec3(129.0))) * 0.08;
    float offsetAmount = step(0.0, glitchAmount - saturate(fbm(vec3(cellIndex1, t * 3.84) - vec3(83.0))) * 0.24);
    vec2 newPos = pos + vec2(offsetX, offsetY) * vec2(offsetXSign, offsetYSign) * offsetScale * offsetAmount;

    vec2 cellIndex2 = floor(pos / vec2(0.014 , 0.01));
    float addAmount = step(0.8, fbm(vec3(cellIndex2, t))) * 0.01 * glitchAmount;

    float darkAmount = step(0.8, fbm(vec3(cellIndex1, t) - vec3(30.0))) * glitchAmount;

    return vec4(newPos, addAmount, darkAmount);
}

float movieMagicFlashyFade(float f)
{
    if (f > 1.0)
    {
        return 1.0;
    }
    else if (f > .8)
    {
        return (f - .8) / .2;
    }
    else if (f > .7)
    {
        return step(0.5, fract((f - .7) * 30.0));
    }
    else
    {
        return 0.0;
    }
}

// pos is assumed to be normalized
vec3 rings(vec2 pos, vec3 color1, vec3 color2, float numRings, float minCells, float maxCells, float fade, float seedOffset, float t)
{
    if (fade <= 0.0)
        return vec3(0.0);

    pos = pos * 2.0 - 1.0;

    float an = atan(pos.y, pos.x);
    float dis = length(pos);

    float minDis = 0.1;
    float maxDis = 1.0;
    if (dis < minDis || dis >= maxDis)
        return vec3(0.0);

    float scaledDis = (dis - minDis) / (maxDis - minDis) * numRings;
    float ringIndex = floor(scaledDis);
    float ringDis = fract(scaledDis);

    if (ringIndex >= numRings)
        return vec3(0.0);

    float ringDisMargin = 0.2;
    if (ringDis < ringDisMargin || ringDis >= 1.0 - ringDisMargin)
        return vec3(0.0);

    float numCells = minCells + floor(saturate(fbm2(vec3(ringIndex * 29.0, seedOffset, 0.0))) * (maxCells - minCells));
    float scaledAn = an / (2.0 * 3.14159265) * numCells;
    float cellIndex = floor(scaledAn);
    float cellAn = fract(scaledAn);

    // These conditionals have been split up, as writing them on one line
    //  seemed to fail on some systems
    float cellAnMargin = 0.1;
    if (cellAn < cellAnMargin)
        return vec3(0.0);
    if (cellAn >= 1.0 - cellAnMargin)
        return vec3(0.0);

    float cellBrightness = movieMagicFlashyFade(saturate(sin(fbm2(vec3(ringIndex * 34.0 + cellIndex * 50.0, seedOffset + 2.0, t * 0.03)) * 6.0) * 0.5 + 0.5));
    float cellFade = movieMagicFlashyFade(0.7 + 0.3 * saturate(fade * 2.0 - 1.0 + (1.0 - ringIndex / numRings)));

    vec3 cellColor = mix(color1, color2, saturate(sin(fbm2(vec3(ringIndex * -34.0 + cellIndex * -50.0, seedOffset - 8.0, -t * 0.02)) * 6.0) * 0.5 + 0.5));

    return cellColor * cellBrightness * cellFade;
}

// pos is assumed to be normalized
vec3 code(vec2 pos, float res, float glyphRes, float fade, float seedOffset, float t)
{
    if (fade <= 0.0)
        return vec3(0.0);

    if (pos.x < 0.0 || pos.x >= 1.0 || pos.y < 0.0 || pos.y >= 1.0)
        return vec3(0.0);

    vec2 scaledPos = pos * res;
    vec2 charPos = floor(scaledPos);
    vec2 pixelPos = fract(scaledPos);

    float charRow = charPos.y - floor(t);
    float charRowProgress = fract(t);

    float minRowStart = 0.0;
    float maxRowStart = floor(res / 2.0);
    float rowStart = minRowStart + floor(saturate(fbm2(vec3(charRow * 0.3, seedOffset, 0.0))) * (maxRowStart - minRowStart));
    if (charPos.x < rowStart)
        return vec3(0.0);

    float minRowEnd = minRowStart + 1.0;
    float maxRowEnd = res;
    float rowEnd = minRowEnd + floor(saturate(fbm2(vec3(charRow * 0.3, 46.0 + seedOffset, 0.0))) * (maxRowEnd - minRowEnd));
    if (charPos.y < 1.0)
        rowEnd *= charRowProgress;
    if (charPos.x >= rowEnd)
        return vec3(0.0);

    // These conditionals have been split up, as writing them on one line
    //  seemed to fail on some systems
    float pixelMargin = 0.2;
    if (pixelPos.x < pixelMargin)
        return vec3(0.0);
    if (pixelPos.x >= 1.0 - pixelMargin)
        return vec3(0.0);
    if (pixelPos.y < pixelMargin)
        return vec3(0.0);
    if (pixelPos.y >= 1.0 - pixelMargin)
        return vec3(0.0);

    float numGlyphs = 64.0;
    float glyph = floor(saturate(fbm2(vec3(charPos.x, charRow, seedOffset) * 20.0)) * numGlyphs);
    float pixelBrightness = saturate(pow(fbm2(vec3(floor(pixelPos * glyphRes), glyph) * 20.0), 80.0));
    float pixelFade = movieMagicFlashyFade(saturate(fbm2(vec3(charPos.x, charRow, seedOffset - 80.0) * 46.0)) + (fade * 2.0 - 1.0) + 0.7);

    return vec3(1.0) * pixelBrightness * pixelFade;
}

// pos is assumed to be normalized
vec3 waveform(vec2 pos, vec3 color1, vec3 color2, float res, float scale, float fade, float seedOffset, float t)
{
    if (fade <= 0.0)
        return vec3(0.0);

    if (pos.x < 0.0 || pos.x >= 1.0 || pos.y < 0.0 || pos.y >= 1.0)
        return vec3(0.0);

    float scaledPos = pos.x * res;
    float waveIndex = floor(scaledPos);
    float wavePos = fract(scaledPos);

    float waveCol = waveIndex + floor(t);

    float waveMargin = 0.2;
    if (wavePos < waveMargin || wavePos >= 1.0 - waveMargin)
        return vec3(0.0);

    float minWaveHeight = 0.1;
    float maxWaveHeight = 1.0;

    float waveHeight = minWaveHeight + saturate(fbm2(vec3(waveCol * scale, 46.0 + seedOffset, 0.0)) - 0.4) * (maxWaveHeight - minWaveHeight);
    float verticalPos = abs(pos.y * 2.0 - 1.0);
    if (verticalPos >= waveHeight)
        return vec3(0.0);

    float waveBrightness = movieMagicFlashyFade(saturate(sin(fbm2(vec3(waveCol * 34.0, seedOffset + 2.0, t * 0.03)) * 6.0) * 0.5 + 1.0)) * 0.98 + 0.02;
    float waveFade = movieMagicFlashyFade(0.7 + 0.3 * saturate(fade * 2.0 - 1.0 + (1.0 - waveIndex / res)));

    vec3 waveColor = mix(color1, color2, saturate(sin(fbm2(vec3(waveCol * -34.0, seedOffset - 8.0, -t * 0.02)) * 6.0) * 0.5 + 0.5));

    return waveColor * waveBrightness * waveFade;
}

void mainImage( out vec4 fragColor)
{
    vec2 pixelPos = gl_FragCoord.xy;
    vec2 res = iResolution.xy;
    
    float fade = min(iTime * 0.35, 1.0);
    
    vec4 glitchedPixelPosAddAmountDarkAmount = glitch(pixelPos / res) * vec4(res, 1.0, 1.0);
    pixelPos = glitchedPixelPosAddAmountDarkAmount.xy;
    float addAmount = glitchedPixelPosAddAmountDarkAmount.z;
    float darkAmount = glitchedPixelPosAddAmountDarkAmount.w;

    vec3 color = vec3(0.01);

    float elementSize = res.y * 0.4;

    color += rings(
        (pixelPos - (res / 2.0 - vec2(elementSize / 2.0) + vec2(-1.0,  1.0) * elementSize * vec2(1.2, 0.6))) / elementSize, vec3(0.2, 0.7, 1.0), vec3(1.0, 0.6, 0.2), 14.0, 4.0, 26.0, fade, 0.0, iTime) * 3.0;
    color += code(
        (pixelPos - (res / 2.0 - vec2(elementSize / 2.0) + vec2( 0.0,  1.0) * elementSize * vec2(1.2, 0.6))) / elementSize, 40.0, 2.0, fade, 0.0, iTime * 0.8) * 3.0;
    color += waveform(
        (pixelPos - (res / 2.0 - vec2(elementSize / 2.0) + vec2( 1.0,  1.0) * elementSize * vec2(1.2, 0.6))) / elementSize, vec3(0.2, 0.7, 1.0), vec3(1.0, 0.05, 0.05), 24.0, 0.6, fade, -12.0, iTime * 0.9) * 3.0;

    color += code(
        (pixelPos - (res / 2.0 - vec2(elementSize / 2.0) + vec2(-1.0, -1.0) * elementSize * vec2(1.2, 0.6))) / elementSize, 16.0, 7.0, fade, 80.0, iTime * 0.3) * 3.0;
    color += waveform(
        (pixelPos - (res / 2.0 - vec2(elementSize / 2.0) + vec2( 0.0, -1.0) * elementSize * vec2(1.2, 0.6))) / elementSize, vec3(0.2, 0.7, 1.0), vec3(1.0, 0.6, 0.2), 20.0, 0.2, fade, -80.0, iTime * 1.2) * 3.0;
    color += rings(
        (pixelPos - (res / 2.0 - vec2(elementSize / 2.0) + vec2( 1.0, -1.0) * elementSize * vec2(1.2, 0.6))) / elementSize, vec3(0.1, 1.0, 0.3), vec3(1.0, 0.12, 0.02), 8.0, 4.0, 16.0, fade, 10.0, iTime * 0.3) * 3.0;
    
    color *= 2.0;

    color *= 1.0 - darkAmount;

	color += addAmount;
    
	fragColor = vec4(color, 1.0);
}

void main() {
	mainImage(gl_FragColor);
}
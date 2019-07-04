/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "f735bee5b64ef98879dc618b016ecf7939a5756040c2cde21ccb15e69a6e1cfb.png"
    }
  ],
  "CATEGORIES" : [
    "raymarching",
    "noise",
    "volume",
    "clouds",
    "camera",
    "sky",
    "Automatically Converted",
    "Shadertoy"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/XtlfDn by Shane.  Moving a camera through a cloud scene. It's rendered in a surreal, stylized fashion, and utilizes a relatively cheap construction process.",
  "INPUTS" : [
	{
            "NAME": "flight_speed",
            "TYPE": "float",
            "DEFAULT": 2.0,
            "MIN": 0.0,
            "MAX": 8.0
    },
    {
            "NAME": "cloud_speed",
            "TYPE": "float",
            "DEFAULT": 1.0,
            "MIN": 0.0,
            "MAX": 5.0
    },
    {
            "NAME": "cloud_threshold",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.35,
            "MAX": 0.55
    },
    
    {
            "NAME": "FAR",
            "TYPE": "float",
            "DEFAULT": 60,
            "MIN": 0,
            "MAX": 100
    },
    {
            "NAME": "FOV",
            "TYPE": "float",
            "DEFAULT": 1.14,
            "MIN": 0.7,
            "MAX": 1.8
    },
    {
            "NAME": "lkAtx",
            "TYPE": "float",
            "DEFAULT": 0,
            "MIN": -0.15,
            "MAX": 0.15
    },
    {
            "NAME": "lkAtY",
            "TYPE": "float",
            "DEFAULT": 0,
            "MIN": -0.15,
            "MAX": 0.15
    },
    
    {
            "NAME": "sunFade1",
            "TYPE": "float",
            "DEFAULT": 0.25,
            "MIN": 0.0,
            "MAX": 1.0
    },
    {
            "NAME": "sunFade2",
            "TYPE": "float",
            "DEFAULT": 0.35,
            "MIN": 0.0,
            "MAX": 1.0
    },
    {
            "NAME": "sunFade3",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 5.0
    },
    
    {
			"NAME": "ARRANGEMENT",
			"TYPE": "long",
			"VALUES": [
				0,
				1,
				2,
				3

			],
			"LABELS": [
				"Layered",
				"path",
				"fluffy layered",
				"just cloud"
		
			],
			"DEFAULT": 0
		}
  ]
}
*/





// A between cloud layers look, between the individual clouds, a more conventional fluffy cloud look,
// or right through them. I prefer the latter arrangement, but it doesn't give a clear enough view.


// Standard 2D rotation formula - See Nimitz's comment.
mat2 r2(in float a){ float c = cos(a), s = sin(a); return mat2(c, s, -s, c); }

// Smooth maximum, based on IQ's smooth minimum.
float smax(float a, float b, float s){
    
    float h = clamp(.5 + .5*(a - b)/s, 0., 1.);
    return mix(b, a, h) + h*(1. - h)*s;
}

// Hash function. This particular one probably doesn't disperse things quite 
// as nicely as some of the others around, but it's compact, and seems to work.
//
vec3 hash33(vec3 p){ 
    float n = sin(dot(p, vec3(7, 157, 113)));    
    return fract(vec3(2097152, 262144, 32768)*n); 
}

// IQ's texture lookup noise... in obfuscated form. There's less writing, so
// that makes it faster. That's how optimization works, right? :) Seriously,
// though, refer to IQ's original for the proper function.
// 
// By the way, you could replace this with the non-textured version, and the
// shader should run at almost the same efficiency.
float n3D( in vec3 p ){
    
    //return texture(iChannel1, p/24., 0.25).x;
    
    vec3 i = floor(p); p -= i; p *= p*(3. - 2.*p);
	p.xy = IMG_NORM_PIXEL(iChannel0,mod((p.xy + i.xy + vec2(37, 17)*i.z + .5)/256.,1.0),-100.).yx;
	return mix(p.x, p.y, p.z);
}

/*
// Textureless 3D Value Noise:
//
// This is a rewrite of IQ's original. It's self contained, which makes it much
// easier to copy and paste. I've also tried my best to minimize the amount of 
// operations to lessen the work the GPU has to do, but I think there's room for
// improvement. I have no idea whether it's faster or not. It could be slower,
// for all I know, but it doesn't really matter, because in its current state, 
// it's still no match for IQ's texture-based, smooth 3D value noise.
//
// By the way, a few people have managed to reduce the original down to this state, 
// but I haven't come across any who have taken it further. If you know of any, I'd
// love to hear about it.
//
// I've tried to come up with some clever way to improve the randomization line
// (h = mix(fract...), but so far, nothing's come to mind.
float n3D(vec3 p){
    
    // Just some random figures, analogous to stride. You can change this, if you want.
	const vec3 s = vec3(7, 157, 113);
	
	vec3 ip = floor(p); // Unique unit cell ID.
    
    // Setting up the stride vector for randomization and interpolation, kind of. 
    // All kinds of shortcuts are taken here. Refer to IQ's original formula.
    vec4 h = vec4(0., s.yz, s.y + s.z) + dot(ip, s);
    
	p -= ip; // Cell's fractional component.
	
    // A bit of cubic smoothing, to give the noise that rounded look.
    p = p*p*(3. - 2.*p);
    
    // Smoother version of the above. Weirdly, the extra calculations can sometimes
    // create a surface that's easier to hone in on, and can actually speed things up.
    // Having said that, I'm sticking with the simpler version above.
	//p = p*p*p*(p*(p * 6. - 15.) + 10.);
    
    // Even smoother, but this would have to be slower, surely?
	//vec3 p3 = p*p*p; p = ( 7. + ( p3 - 7. ) * p ) * p3;	
	
    // Cosinusoidal smoothing. OK, but I prefer other methods.
    //p = .5 - .5*cos(p*3.14159);
    
    // Standard 3D noise stuff. Retrieving 8 random scalar values for each cube corner,
    // then interpolating along X. There are countless ways to randomize, but this is
    // the way most are familar with: fract(sin(x)*largeNumber).
    h = mix(fract(sin(h)*43758.5453), fract(sin(h + s.x)*43758.5453), p.x);
	
    // Interpolating along Y.
    h.xy = mix(h.xz, h.yw, p.y);
    
    // Interpolating along Z, and returning the 3D noise value.
    return mix(h.x, h.y, p.z); // Range: [0, 1].
	
}
*/




// The path is a 2D sinusoid that varies over time, depending upon the frequencies, and amplitudes.
vec2 path(in float z){ 

    //return vec2(0); // Straight path.
    return vec2(sin(z*.075)*8., cos(z*.1)*.75*2.); // Windy path.
    
}

// Distance function. Just some layered noise, and depending on the arrangement, some shapes
// smoothy carved out.
float map(vec3 p) {
    
    // Time factor.
    vec3 t = vec3(1.*cloud_speed, .5, .25)*(TIME*(cloud_speed/3.));

    
    // Two base layers of low fregency noise to shape the clouds. It's been contracted in the Y
    // direction, since a lot clouds seem to look that way.
    float mainLayer = n3D(p*vec3(.4, 1, .4))*.66 + n3D(p*vec3(.4, 1, .4)*2.*.8)*.34 - .0;    
    
    // Three layers of higher frequency noise to add detail.
    float detailLayer = n3D(p*3. + t)*.57 +  n3D(p*6.015 + t*2.)*.28 +  n3D(p*12.01 + t*4.)*.15 - .0;
    // Two layers, if you're computer can't handle three.
	//float detailLayer = n3D(p*3. + t)*.8 +  n3D(p*12. + t*4.)*.2;

    // Higher weighting is given to the base layers than the detailed ones.
    float clouds = mainLayer*.84 + detailLayer*.16;
    
    
    if (ARRANGEMENT != 3){
    // Mapping the hole or plane around the path.
    p.xy -= path(p.z);
    }
    
    
    // Between cloud layers.
    if (ARRANGEMENT == 0){ // Layered.
    //return smax(tn, -abs(p.y) + 1.1 + (clouds - .5), .5) + (clouds - .5);
    	return smax(clouds, -length(p.xy*vec2(1./32., 1.)) + 1.1 + (clouds - .5), .5) + (clouds - .5);
    }else if (ARRANGEMENT == 1){ // Path - Tunnel in disguise.
    // Mapping the hole around the path.
    	return smax((clouds - .25)*2., -smax(abs(p.x) - .5, abs(p.y) - .5, 1.), 2.);
    }else if (ARRANGEMENT == 2){ // Path - Tunnel in disguise.
    // Between layers, but with fluffier clouds.
    	return smax(clouds - .075, -length(p.xy*vec2(1./32., 1.)) + 1.1 + (clouds - .5), .5) + (clouds - .5)*.35;
    }else { // The clouds only.
    	return (clouds - .25)*2.; 
    //return tn; // Fluffier, but blurrier.
    }
   
   


}


// Less accurate 4 tap (3 extra taps, in this case) normal calculation. Good enough for this example.
vec3 fNorm(in vec3 p, float d){
    
    // Note the large sampling distance.
    vec2 e = vec2(.075, 0); 

    // Return the normal.
    return normalize(vec3(d - map(p - e.xyy), d - map(p - e.yxy), d - map(p - e.yyx)));
}

void main() {

    
    // Screen coordinates.
	vec2 uv = (gl_FragCoord.xy - RENDERSIZE.xy*.5)/RENDERSIZE.y;
    // Ray origin. Moving along the Z-axis.
    vec3 ro = vec3(0, 0, (TIME*flight_speed));
	vec3 lk = ro + vec3(lkAtx, lkAtY, .25);  // "Look At" position.
 	
    	// Using the Z-value to perturb the XY-plane.
	// Sending the camera, "look at," and light vector down the path. The "path" function is 
	// synchronized with the distance function.
    ro.xy += path(ro.z);
	lk.xy += path(lk.z);
    
    // Using the above to produce the unit ray-direction vector.
    
    vec3 forward = normalize(lk-ro);
    vec3 right = normalize(vec3(forward.z, 0., -forward.x )); 
    vec3 up = cross(forward, right);
    // rd - Ray direction.
    vec3 rd = normalize(forward + FOV*uv.x*right + FOV*uv.y*up);
    //rd = normalize(vec3(rd.xy, rd.z - length(rd.xy)*.15));
    
    // Camera swivel - based on path position.
    vec2 sw = path(lk.z);
    rd.xy *= r2(-sw.x/24.);
    rd.yz *= r2(-sw.y/16.);
    
    // The ray is effectively marching through discontinuous slices of noise, so at certain
    // angles, you can see the separation. A bit of randomization can mask that, to a degree.
    // At the end of the day, it's not a perfect process. Anyway, the hash below is used to
    // at jitter to the jump off point (ray origin).
    //    
    // It's also used for some color based jittering inside the loop.
    vec3 rnd = hash33(rd.yzx + fract(TIME));
    // Local density, total density, and weighting factor.
    float lDen = 0., td = 0., w = 0.;
    // Closest surface distance, a second sample distance variable, and total ray distance 
    // travelled. Note the comparitively large jitter offset. Unfortunately, due to cost 
    // cutting (64 largish steps, it was  necessary to get rid of banding.
    float d = 1., d2 = 0., t = dot(rnd, vec3(.333));
    // Distance threshold. Higher numbers give thicker clouds, but fill up the screen too much.    
    //float h = cloud_distance_threshold;
    // Initializing the scene color to black, and declaring the surface position vector.
    vec3 col = vec3(0), sp;
    
    // Directional light. Don't quote me on it, but I think directional derivative lighting
    // only works with unidirectional light... Thankfully, the light source is the cun which 
    // tends to be unidirectional anyway.
    vec3 ld = normalize(vec3(-.2, .3, .8));
    
    
    // Using the light position to produce a blueish sky and sun. Pretty standard.
    vec3 sky = mix(vec3(1, 1, .9), vec3(.19, .35, .56), rd.y*0.5 + 0.5);
    //sky = mix(sky, mix(vec3(1, .8, .7), vec3(.31, .52, .73), rd.y*0.5 + 0.5), .5);
    
    
    // Sun position in the sky - Note that the sun has been cheated down a little lower for 
    // aesthetic purposes. All this is fake anyway.
    vec3 fakeLd = normalize(vec3(-.2, .3, .8*1.5));
    float sun = clamp(dot(fakeLd, rd), 0.0, 1.0);
    
    
    
    // Combining the clouds, sky and sun to produce the final color.
    sky += vec3(1, .3, .05)*pow(sun, 5.)*sunFade1; 
    sky += vec3(1, .4, .05)*pow(sun, 8.)*sunFade2; 
    sky += vec3(1, .9, .7)*pow(sun, 128.)*sunFade3; 
    // Ramping up the sky contrast a bit.
    sky *= sqrt(sky); 
    
    // I thought I'd mix in a tiny bit of sky color with the clouds here... It seemed like a
    // good idea at the time. :)
    vec3 cloudCol = mix(sky, vec3(1, .9, .8), .66);
    
    // Raymarching loop.
    for (int i=0; i<64; i++) {
        sp = ro + rd*t; // Current ray position.
        d = map(sp); // Closest distance to the surface... particle.
        
        // Loop break conditions - If the ray hits the surface, the accumulated density maxes out,
        // or if the total ray distance goes beyong the maximum, break.
        if(d<.001*(1. + t*.125) || td>1. || t>FAR) break;
        // If we get within a certain distance, "h," of the surface, accumulate some surface values.
        //
        // Values further away have less influence on the total. When you accumulate layers, you'll
        // usually need some kind of weighting algorithm based on some identifying factor - in this
        // case, it's distance. This is one of many ways to do it. In fact, you'll see variations on 
        // the following lines all over the place.
        //
        // On a side note, you could wrap the next few lines in an "if" statement to save a
        // few extra "map" calls, etc. However, some cards hate branching, nesting, etc, so it
        // could be a case of diminishing returns... Not sure what the right call is, so I'll 
        // leave it to the experts. :)
        w = d<cloud_threshold? (1. - td)*(cloud_threshold - d) : 0.;   
        // Use the weighting factor to accumulate density. How you do this is up to you. 
        //td += w*w*8. + 1./64.; // More transparent looking... kind of.
        td += w + 1./64.; // Looks cleaner, but a little washed out.
        
       
        // Lighting calculations.
        // Standard diffuse calculation, using a more expensive four tap tetrahedral normal.
        // However, this will work with point light and enables are normal-based lighting.
        //float diff = max(dot(ld, fNorm(sp, d)), 0.)*2.;
        
        // Directional derivative-based diffuse calculation. Uses only two function taps,
        // but only works with unidirectional light.
        d2 = map(sp + ld*.1);
        // Possibly quicker than the line above, but I feel it overcomplicates things... Maybe. 
        //d2 = d<h? map(sp + ld*.1) : d;
        float diff = max(d2 - d, 0.)*20.; 
        //float diff = max(d2*d2 - d*d, 0.)*20.; // Slightly softer diffuse effect.
        
        // Accumulating the color. You can do this any way you like.
        //
        // Note that "1. - d2" is a very lame one-tap shadow value - Basically, you're traversing
        // toward the light once. It's artificially darkened more by multiplying by "d," etc, which
        // was made up on the spot. It's not very accurate, but it's better than no shadowing at all.
        // Also note, that diffuse light gives a shadowy feel, but is not shadowing.
        col += w*max(d*d*(1. - d2)*3. - .05, 0.)*(diff*cloudCol*2. + vec3(.95, 1, 1.05))*2.5; // Darker, brooding.
        // Other variations - Tweak them to suit your needs.
        //col += w*d*(sqrt(diff)*vec3(1, .85, .7)*2. + 2.); // Whiter, softer, fluffier.
        //col += w*d*((1. - exp(-diff*8.)*1.25)*vec3(1, .85, .7)*2. + 2.);
        
       
        // Optional extra: Color-based jittering. Roughens up the clouds that hit the camera lens.
        col *= .98 + fract(rnd*289. + t*41.13)*.04;
        // Enforce minimum stepsize. This is probably the most important part of the procedure.
        // It reminds me a little of of the soft shadows routine.
        t += max(d*.5, .05); //
        //t += 0.25; // t += d*.5;// These also work - It depends what you're trying to achieve.
    }
    
    // Clamp above zero... It might not need it, but just in case.
    col = max(col, 0.);
    
    
    // Postprocessing the cloud color just to more evenly match the background. Made up.
    col *= mix(vec3(1), sky, .25);
    
    // Fogging out the volumetric substance. The fog blend is heavier than usual. It was a style
    // choice - Not sure if was the right one though. :)
    col = mix(col, sky, smoothstep(0., .85, t/FAR));
    col = mix(col, sky*sky*2., 1. - 1./(1.+ t*t*.001));//
 	//col += vec3(1, .4, .2)*pow(sun, 16.)*.25; 	
    
    // More postprocessing. Adding some very subtle fake warm highlights.
    vec3 fCol = mix(pow(vec3(1.3, 1, 1)*col, vec3(1, 2, 10)), sky, .5);
    col = mix(fCol, col, dot(cos(rd*6. +sin(rd.yzx*6.)), vec3(.333))*.1 + .9);
    
    
    // If it were up to me, I'd be ramping up the contrast, but I figured it might be a little to
    // broody for some. :)
    //col *= sqrt(col)*1.5;
 
    
    // Subtle vignette.
    uv = gl_FragCoord.xy/RENDERSIZE.xy;
    //col *= pow(16.*uv.x*uv.y*(1. - uv.x)*(1. - uv.y) , .25)*.35 + .65;
    // Colored varation.
    col = mix(pow(min(vec3(1.5, 1, 1).zyx*col, 1.), vec3(1, 3, 16)), col, 
              pow(16.*uv.x*uv.y*(1. - uv.x)*(1. - uv.y) , .125)*.5 + .5);
 
    // Done.
    gl_FragColor = vec4(sqrt(min(col, 1.)), 1.0);
    
}

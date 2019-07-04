/*
{
  "IMPORTED" : [
    {
      "NAME" : "iChannel0",
      "PATH" : "tex16.png"
    }
  ],
  "CATEGORIES" : [
    "noise",
    "cloud",
    "volumetric",
    "sinusoidal",
    "flythrough",
    "Automatically Converted"
  ],
  "DESCRIPTION" : "Automatically converted from https:\/\/www.shadertoy.com\/view\/MscXRH by Shane.  A daytime version of my \"Cheap Cloud Flythrough\" example.",
  "INPUTS" : [
	{
            "NAME": "flight_speed",
            "TYPE": "float",
            "DEFAULT": 4.0,
            "MIN": 0.0,
            "MAX": 10.0
    },
    {
            "NAME": "cloud_speed",
            "TYPE": "float",
            "DEFAULT": 1.0,
            "MIN": 0.0,
            "MAX": 5.0
    },
    {
            "NAME": "cloud_distance_threshold",
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
            "NAME": "lkAtx",
            "TYPE": "float",
            "DEFAULT": 0,
            "MIN": -5.15,
            "MAX": 5.15
    },
    {
            "NAME": "lkAtY",
            "TYPE": "float",
            "DEFAULT": 0,
            "MIN": -5.15,
            "MAX": 5.15
    },
    
    {
            "NAME": "sunPosX",
            "TYPE": "float",
            "DEFAULT": 0,
            "MIN": -15.15,
            "MAX": 15.15
    },
    {
            "NAME": "sunPosY",
            "TYPE": "float",
            "DEFAULT": 0,
            "MIN": -15.15,
            "MAX": 15.15
    },
    {
            "NAME": "sunPosZ",
            "TYPE": "float",
            "DEFAULT": 6,
            "MIN": -30.15,
            "MAX": 30.15
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
    }
  ]
}
*/


/*

	Combustible Clouds
	------------------
	
	This is just a daytime version of my cheap cloud flythrough example. I'm not sure why
	the clouds exist in a combustible atmosphere, or if that's even possible... but it's 
	just a cheap hack, so isn't meant to be taken seriously. :)
	
	Based on:
	
	Cloudy Spikeball - Duke
    https://www.shadertoy.com/view/MljXDw
    // Port from a demo by Las - Worth watching.
    // http://www.pouet.net/topic.php?which=7920&page=29&x=14&y=9

*/

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

// Basic low quality noise consisting of three layers of rotated, mutated 
// trigonometric functions. Needs work, but sufficient for this example.
float trigNoise3D(in vec3 p){

    
    float res = 0., sum = 0.;
    
    // IQ's cheap, texture-lookup noise function. Very efficient, but still 
    // a little too processor intensive for multiple layer usage in a largish 
    // "for loop" setup. Therefore, just one layer is being used here.
    float n = n3D(p*8. + TIME*cloud_speed);


    // Two sinusoidal layers. I'm pretty sure you could get rid of one of 
    // the swizzles (I have a feeling the GPU doesn't like them as much), 
    // which I'll try to do later.
    
    vec3 t = sin(p.yzx*3.14159265 + cos(p.zxy*3.14159265+1.57/2.))*0.5 + 0.5;
    p = p*1.5 + (t - 1.5); //  + TIME*0.1
    res += (dot(t, vec3(0.333)));

    t = sin(p.yzx*3.14159265 + cos(p.zxy*3.14159265+1.57/2.))*0.5 + 0.5;
    res += (dot(t, vec3(0.333)))*0.7071;    
	 
	return ((res/1.7071))*0.85 + n*0.15;
}

// Distance function.
float map(vec3 p) {

    return trigNoise3D(p*0.5);
    
    // Three layers of noise, for comparison.
    //p += TIME;
    //return n3D(p*.75)*0.57 + n3D(p*1.875)*0.28 + n3D(p*4.6875)*0.15;
}


void main()
{  

    // Unit direction ray vector: Note the absence of a divide term. I came across
    // this via a comment Shadertoy user "coyote" made. I'm pretty easy to please,
    // but I thought it was pretty cool.
    vec3 rd = normalize(vec3(gl_FragCoord.xy - RENDERSIZE.xy*.5, RENDERSIZE.y*.75)); 

    // Ray origin. Moving along the Z-axis.
    vec3 ro = vec3(lkAtx, lkAtY , TIME*flight_speed);

    // Cheap camera rotation.
    //
    // 2D rotation matrix. Note the absence of a cos variable. It's there, but in disguise.
    // This one came courtesy of Shadertoy user, "Fabrice Neyret."
    vec2 a = sin(vec2(1.5707963, 0) + TIME*0.1875); 
    mat2 rM = mat2(a, -a.y, a.x);
    
    rd.xy = rd.xy*rM; // Apparently, "rd.xy *= rM" doesn't work on some setups. Crazy.
    a = sin(vec2(1.5707963, 0) + cos(TIME*0.1875*.7)*.7);
    rM = mat2(a, -a.y, a.x); 
    rd.xz = rd.xz*rM;

    // Placing a light in front of the viewer and up a little. You could just make the 
    // light directional and be done with it, but giving it some point-like qualities 
    // makes it a little more interesting. You could also rotate it in sync with the 
    // camera, like a light beam from a flying vehicle.
    vec3 lp = vec3( sunPosX, sunPosY, 6.0);
    //lp.xz = lp.xz*rM;
    lp += ro;
    
    

    // The ray is effectively marching through discontinuous slices of noise, so at certain
    // angles, you can see the separation. A bit of randomization can mask that, to a degree.
    // At the end of the day, it's not a perfect process. Note, the ray is deliberately left 
    // unnormalized... if that's a word.
    //
    // Randomizing the direction.
    rd = (rd + (hash33(rd.zyx)*0.004-0.002)); 
    // Randomizing the length also. 
    rd *= (1. + fract(sin(dot(vec3(7, 157, 113), rd.zyx))*43758.5453)*0.04-0.02);  
    
    //rd = rd*.5 + normalize(rd)*.5;    
    
    // Some more randomization, to be used for color based jittering inside the loop.
    vec3 rnd = hash33(rd+311.);

    // Local density, total density, and weighting factor.
    float ld=0., td=0., w=0.;

    // Closest surface distance, and total ray distance travelled.
    float d=1., t=0.;

    // Distance threshold. Higher numbers give thicker clouds, but fill up the screen too much.    
    //const float h = .5;


    // Initializing the scene color to black, and declaring the surface position vector.
    vec3 col = vec3(0), sp;



    // Particle surface normal.
    //
    // Here's my hacky reasoning. I'd imagine you're going to hit the particle front on, so the normal
    // would just be the opposite of the unit direction ray. However particles are particles, so there'd
    // be some randomness attached... Yeah, I'm not buying it either. :)
    vec3 sn = normalize(hash33(rd.yxz)*.03-rd);

    // Raymarching loop.
    for (int i=0; i<int(FAR); i++) {

        // Loop break conditions. Seems to work, but let me
        // know if I've overlooked something.
        if((td>1.) || d<0.001*t || t>80.)break;


        sp = ro + rd*t; // Current ray position.
        d = map(sp); // Closest distance to the surface... particle.

        // If we get within a certain distance, "h," of the surface, accumulate some surface values.
        // The "step" function is a branchless way to do an if statement, in case you're wondering.
        //
        // Values further away have less influence on the total. When you accumulate layers, you'll
        // usually need some kind of weighting algorithm based on some identifying factor - in this
        // case, it's distance. This is one of many ways to do it. In fact, you'll see variations on 
        // the following lines all over the place.
        //
        ld = (cloud_distance_threshold - d) * step(d, cloud_distance_threshold); 
        w = (1. - td) * ld;   

        // Use the weighting factor to accumulate density. How you do this is up to you. 
        td += w*w*8. + 1./60.; //w*w*5. + 1./50.;
        //td += w*.4 + 1./45.; // Looks cleaner, but a little washed out.


        // Point light calculations.
        vec3 ld = lp-sp; // Direction vector from the surface to the light position.
        float lDist = max(length(ld), 0.001); // Distance from the surface to the light.
        ld/=lDist; // Normalizing the directional light vector.

        // Using the light distance to perform some falloff.
        float atten = 1./(1. + lDist*0.1 + lDist*lDist*0.03);

        // Ok, these don't entirely correlate with tracing through transparent particles,
        // but they add a little anglular based highlighting in order to fake proper lighting...
        // if that makes any sense. I wouldn't be surprised if the specular term isn't needed,
        // or could be taken outside the loop.
        float diff = max(dot( sn, ld ), 0.);
        float spec = pow(max( dot( reflect(-ld, sn), -rd ), 0. ), 4.);


        // Accumulating the color. Note that I'm only adding a scalar value, in this case,
        // but you can add color combinations.
        col += w*(1.+diff*.5+spec*.5)*atten;
        // Optional extra: Color-based jittering. Roughens up the grey clouds that hit the camera lens.
        col += (fract(rnd*289. + t*41.)-.5)*0.02;;

        // Try this instead, to see what it looks like without the fake contrasting. Obviously,
        // much faster.
        //col += w*atten*1.25;


        // Enforce minimum stepsize. This is probably the most important part of the procedure.
        // It reminds me a little of of the soft shadows routine.
        t +=  max(d * 0.5, 0.02); //
        // t += 0.2; // t += d*0.5;// These also work, but don't seem as efficient.

    }
    
    col = max(col, 0.);

    
    // Adding a bit of a firey tinge to the cloud value.
    col = mix(vec3(min(col.x*1.3, 1.),  pow(col.x, 2.), pow(col.x, 10.)), col, dot(cos(rd*6. +sin(rd.yzx*6.)), vec3(.333))*.2+.8);
 
    // Using the light position to produce a blueish sky and sun. Pretty standard.
    vec3 sky = vec3(.6, .8, 1.)*min((1.5+rd.y*.5)/2., 1.); 	
    sky = mix(vec3(1, 1, .9), vec3(.31, .42, .53), rd.y*0.5 + 0.5);
    
    float sun = clamp(dot(normalize(lp-ro), rd), 0.0, 1.0);
   
    // Combining the clouds, sky and sun to produce the final color.
    col += vec3(1.0,0.5,0.1)*pow(sun, 5.0)*sunFade1; 
    col = mix(col, sky, smoothstep(0., 25., t));
 	col += vec3(1.0,0.5,0.1)*pow(sun, 16.0)*sunFade2; 	
 
    // Done.
    gl_FragColor = vec4(min(col, 1.), 1.0);
    
}
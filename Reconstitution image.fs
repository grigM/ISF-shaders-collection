/*{
	"CREDIT": "by You",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"filter"
	],
	"INPUTS": [
		{
			"NAME": "inputImage",
			"TYPE": "image"
		},
		{
			"NAME": "vitesse_chute",
			"TYPE": "float",
			"MIN": 0,
			"MAX": 20,
			"DEFAULT": 8
		},
		{
			"NAME": "vitesse_temps",
			"TYPE": "float",
			"MIN": 0,
			"MAX": 1,
			"DEFAULT": 0.2
		},
		{
			"NAME": "decoupage",
			"TYPE": "float",
			"MIN": 2,
			"MAX": 12,
			"DEFAULT": 12
		}
	]
}*/

// vitesse_temps permet d'accéler ou de ralentir l'animation
// vitesse_chute est la vitesse des cases, elle est constante
// decoupage donne le nombre de cases horizontalement et verticalement

// Une case est un ensemble de pixels ayant le même comportement : départ au même moment et même vitesse.
// Il y a decoupage*decoupage cases.
// On repère une case par un pixel quelconque qu'elle contient.
// Il suffit ensuite d'utiliser la fonction bornesup pour déterminer son ordonnée la plus haute et donc connaître sa position exacte.


vec4 noir = vec4(0., 0., 0., 1.);

float n = floor(decoupage) ;
float pas = 1./n ;
float t = fract(TIME * vitesse_temps) ;

float random (vec2 st) {
	return fract(sin(dot(st.xy,
	vec2(12.9898,78.233)))*43758.5453123);
}

float bornesup (vec2 p){			// Détermine l'ordonnée de la borne haute de la case contenant le pixel d'entrée
	return ((floor(p.y* n + 1.))/n);
}

float starttime (vec2 p){			// Détermine le temps de départ de la case contenant le pixel d'entrée
	float xinf = floor(p.x * n)/n ;
	float yinf = floor(p.y * n)/n ;
	float t0 = (3. * yinf + random(vec2(xinf,yinf))) + 0.5 ;
	return t0 / 4. ;
}

vec2 positions_max (vec2 case){		// Détermine les ordonnées inférieure et supérieure de la case d'entrée en fonction du temps
	float t0 = starttime(case) ;
	float sup = bornesup(case) ;
	float inf = sup - pas ;
	float pos_sup = sup ;					// Après l'arrivée
	float pos_inf  = inf ;
	if (t < t0){							// Avant l'arrivée
		float translation =  (t0 - t) * vitesse_chute ;
		pos_sup = sup + translation ;
		pos_inf = inf + translation ;
	}
	return vec2(pos_inf, pos_sup)  ;
}

vec2 courant (vec2 p){				// Détermine la case devant être affichée sur le pixel en entrée
	vec2 increment = vec2(0.,0.) ;
	vec2 case = p - increment ;				// Increment nul donc il s'agit de la case contenant le pixel d'entrée
	vec2 position = positions_max(case) ;
	
	if (p.y < position.x || p.y > position.y){		// Si la case ne recouvre pas le pixel d'entrée, on considère la case suivante (celle du dessous)
		increment.y = increment.y + pas ;
		case = p - increment ;
		position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		if (p.y < position.x || p.y > position.y){
			increment.y = increment.y + pas ;
			case = p - increment ;
			position = positions_max(case) ;
		}
		}
		}
		}
		}
		}
		}
		}
		}
		}
		}
		}
		}
		}
		}
		}
	}
	
	return case ;							// La fonction retourne la case recouvrant le pixel d'entrée
}

void main() {
	vec2 p = vec2(isf_FragNormCoord.xy) ;	// Coordonnées normalisées du pixel traité
	vec4 color = noir ;						// Initialisation de la couleur de ce pixel à afficher
	
	float fin = starttime(p);				// Fin de la chute de la case
	vec2 c = courant(p) ;					// Case courante recouvrant le pixel
	
	if (t > fin){							// Situation finale
		color = IMG_NORM_PIXEL(inputImage, p) ;		// On affiche l'image originale
	}
	else{
		float t0 = starttime(c) ;			// Temps d'arrivée de la case recouvrant le pixel
		// Coordonnée du pixel de l'image à afficher : coordonnées initiales décalées de la distance parcourue
		vec2 translate = p - vec2(0., (t0 - t) * vitesse_chute) ;
		if (translate.y <= 1. && translate.y > 0.){				// Si le pixel est dans l'image
			color = IMG_NORM_PIXEL(inputImage, translate) ;		// On affiche le pixel de l'image que l'on vient de déterminer
		}
	}
	
	gl_FragColor = color ;
}

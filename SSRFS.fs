#version 460 core

layout (location = 0) out vec4 refColor;

uniform sampler2D gNormal;
uniform sampler2D colorBuffer;
uniform sampler2D depthMap;

uniform float SCR_WIDTH;
uniform float SCR_HEIGHT;
uniform mat4 invProjection;
uniform mat4 projection;


float ReturnZInView(vec3 ts){
	vec4 r = invProjection * vec4(ts*2 - vec3(1,1,1),1);
	r /= r.w;
	return r.z;
}

void main(){
	vec3 skyColor = vec3(0.6,0.8,1.0f);
	float maxRayDistance = 100.0f;

	vec2 texCoord = vec2(gl_FragCoord.x / SCR_WIDTH,  gl_FragCoord.y / SCR_HEIGHT);		// 0< <1
	vec3 normalView = texture(gNormal, texCoord).rgb;	
	float depth = texture(depthMap, texCoord).r;
	float depthView = depth * 2 - 1;		// -1< <1				
	vec4 positionView = invProjection * vec4((texCoord * 2) - vec2(1,1), depthView, 1); 
	positionView /= positionView.w;
	vec3 reflectionView = normalize(reflect(positionView.xyz, normalView));

	if(reflectionView.z > 0){	//reflection ray going towards the camera
		refColor = vec4(0,0,0,1);
		return;
	}

	vec3 rayEndPositionView = positionView.xyz + reflectionView * maxRayDistance;

	vec3 pixelPositionTexture = vec3(texCoord, depth);
	
	vec4 rayEndPositionTexture = projection * vec4(rayEndPositionView,1);
	rayEndPositionTexture /= rayEndPositionTexture.w;
	rayEndPositionTexture.xyz = (rayEndPositionTexture.xyz + vec3(1,1,1)) / 2.0f;

	vec3 rayDirectionTexture = rayEndPositionTexture.xyz - pixelPositionTexture;	//dp

	ivec2 screenSpaceStartPosition = ivec2(pixelPositionTexture.x * SCR_WIDTH, pixelPositionTexture.y * SCR_HEIGHT); 
	ivec2 screenSpaceEndPosition = ivec2(rayEndPositionTexture.x * SCR_WIDTH, rayEndPositionTexture.y * SCR_HEIGHT); 

	ivec2 screenSpaceDistance = screenSpaceEndPosition - screenSpaceStartPosition;

	int screenSpaceMaxDistance = max(abs(screenSpaceDistance.x), abs(screenSpaceDistance.y)) / 2;
	vec3 dirTexture = rayDirectionTexture / max(screenSpaceMaxDistance, 0.001f);

	vec3 rayStart = pixelPositionTexture;
	float sampleDepth;
	vec3 reflectionColor = vec3(0.0f,0,0.0f);
	bool hit = false;
	for(int i = 0; i < screenSpaceMaxDistance; i++){
		rayStart += dirTexture;
		if(rayStart.x > 1 || rayStart.y > 1 || rayStart.x < 0 || rayStart.y < 0){
			break;
		}
		sampleDepth = texture(depthMap, rayStart.xy).r;

		float depthDif = rayStart.z - sampleDepth;
		if(depthDif >= 0 && depthDif < 0.00001){ //we have a hit
			//reflectionColor = vec3(0.6f,0,0);
			hit = true;
			reflectionColor = texture(colorBuffer, rayStart.xy).rgb;
			break;
		}
	}
	refColor = vec4(reflectionColor,1);
	//refColor = hit ? vec4(reflectionColor,1) : vec4(skyColor,1);
}

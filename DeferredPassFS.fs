#version 460 core

layout (location = 0) out vec4 colorBuffer;
//layout (location = 1) out vec4 FragColor;

in vec2 texCoord;

//uniform sampler2D gPosition;
uniform sampler2D gNormal;
uniform sampler2D gAlbedo;
uniform sampler2D gSpecular;
uniform sampler2D depthMap;

uniform vec3 lightPosition;
uniform float SCR_WIDTH;
uniform float SCR_HEIGHT;
uniform mat4 invProj;

vec3 computePositionInViewSpace(float z){
	vec2 posCanonical  = vec2(gl_FragCoord.x / SCR_WIDTH, gl_FragCoord.y / SCR_HEIGHT) * 2 - 1; //pos in Canonical view volume
	vec4 posView = invProj * vec4(posCanonical, z , 1);
	posView /= posView.w;
	return posView.xyz;
}

void main(){
	vec3 I = vec3(1,1,1);
	vec3 Kd = texture(gAlbedo, texCoord).rgb;
	vec3 ka = 0.1f*Kd;
	vec4 spec = texture(gSpecular, texCoord);
	vec3 Ks = spec.rgb;
	float specularExponent = spec.a;
	float depth = texture(depthMap, texCoord).r * 2 - 1;

	vec3 norm = texture(gNormal, texCoord).rgb;
	//vec3 posForColoring = texture(gPosition, texCoord).rgb;

	vec3 posForColoring = computePositionInViewSpace(depth);

	vec3 lightDir = normalize(lightPosition - posForColoring);
	vec3 view = normalize(-posForColoring);
	vec3 halfVec = normalize(lightDir+view);

	vec3 diffuse = max(0, dot(norm,lightDir)) * Kd;
	vec3 specular = pow(max(0,dot(halfVec, norm)), 1000) * Ks;
	vec3 blinn = I * (diffuse + specular) + ka; 

	//FragColor = vec4(blinn, 1.0f);
	colorBuffer = vec4(blinn, 1.0f);
}
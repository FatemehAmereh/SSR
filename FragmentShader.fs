#version 460 core

out vec4 FragColor;

in vec3 normal;
in vec3 posForColoring;
in vec2 texCoord;

uniform vec3 ka;
uniform vec3 kd;
uniform vec3 ks;
uniform float specularExponent;

uniform bool usetexd;
uniform sampler2D diffuseTexture;
uniform bool usetexs;
uniform sampler2D specularTexture;

uniform vec3 lightPosition;

uniform bool useCubeMap;
uniform samplerCube cubemap;

uniform mat4 viewMatrix;

void main(){
	vec3 I = vec3(1,1,1);
	vec3 Kd = usetexd ? texture(diffuseTexture, texCoord).rgb : kd;
	vec3 Ks = usetexs ? texture(specularTexture, texCoord).rgb : ks;

	vec3 norm = normalize(normal);
	vec3 lightDir = normalize(lightPosition - posForColoring);
	vec3 view = normalize(-posForColoring);
	vec3 halfVec = normalize(lightDir+view);

	vec3 diffuse = max(0, dot(norm,lightDir)) * Kd;
	vec3 specular = pow(max(0,dot(halfVec, norm)), specularExponent) * Ks;
	vec3 blinn = I * (diffuse + specular) + ka;

	FragColor = vec4(blinn, 1.0f);
}
#version 460 core

out vec4 FragColor;

in vec3 normal;
in vec3 posForColoring;
in vec2 texCoord;

uniform vec3 ka;
uniform vec3 kd;
uniform vec3 ks;
uniform float specularExponent;

uniform bool usetexa;
uniform sampler2D ambientTexture;
uniform bool usetexd;
uniform sampler2D diffuseTexture;
uniform bool usetexs;
uniform sampler2D specularTexture;
uniform bool usetexsc;
uniform sampler2D specularExponentTexture;
uniform bool usetexalpha;
uniform sampler2D alphaTexture;

uniform vec3 lightPosition;

uniform bool useCubeMap;
uniform samplerCube cubemap;

uniform mat4 viewMatrix;

void main(){
	vec3 I = vec3(1,1,1);
	vec3 Kd = usetexd ? texture(diffuseTexture, texCoord).rgb : kd;
	vec3 Ks = usetexs ? texture(specularTexture, texCoord).rgb : ks;
	vec3 Ka = usetexa ? texture(ambientTexture, texCoord).rgb : ka;
	vec3 ambient = vec3(0.1f,0.1f,0.1f);
	float shininess = usetexsc ? texture(specularExponentTexture, texCoord).r : specularExponent;

	vec3 norm = normalize(normal);
	vec3 lightDir = normalize(lightPosition - posForColoring);
	vec3 view = normalize(-posForColoring);
	vec3 halfVec = normalize(lightDir+view);

	vec3 diffuse = max(0,dot(norm,lightDir)) * Kd;
	vec3 specular = pow(max(0,dot(halfVec, norm)),shininess) * Ks;
	vec3 blinn = I*(diffuse + specular) + ambient*Ka;


	//computing reflection in view space
	vec3 vi = posForColoring;
	vec3 r = inverse(mat3(viewMatrix)) * reflect(vi, norm);
	vec3 reflection = useCubeMap ? texture(cubemap, r).rgb : vec3(0,0,0);

	/*ANOTHER WAY
	Computing reflections in world space
	vec3 vi = pos_CubeMap - vec3(inverse(viewMatrix) * vec4(0,0,0,1));
	vec3 r = reflect(vi, norm_CubeMap);
	vec3 reflection = useCubeMap ? texture(cubemap, r).rgb : vec3(0,0,0);*/

	FragColor = vec4(blinn + reflection * Ks, 1.0f);
}
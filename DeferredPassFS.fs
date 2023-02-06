#version 460 core

out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D gPosition;
uniform sampler2D gNormal;
uniform sampler2D gAlbedo;
uniform sampler2D gSpecular;

uniform vec3 lightPosition;

void main(){
	vec3 I = vec3(1,1,1);
	vec3 Kd = texture(gAlbedo, texCoord).rgb;
	vec4 spec = texture(gSpecular, texCoord);
	vec3 Ks = spec.rgb;
	float specularExponent = spec.a;

	vec3 norm = texture(gNormal, texCoord).rgb;
	vec3 posForColoring = texture(gPosition, texCoord).rgb;

	vec3 lightDir = normalize(lightPosition - posForColoring);
	vec3 view = normalize(-posForColoring);
	vec3 halfVec = normalize(lightDir+view);

	vec3 diffuse = max(0, dot(norm,lightDir)) * Kd;
	vec3 specular = pow(max(0,dot(halfVec, norm)), 1000) * Ks;
	vec3 blinn = I * (diffuse + specular); //+ ka;

	FragColor = vec4(blinn, 1.0f);
}
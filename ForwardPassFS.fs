#version 460 core

layout (location = 0) out vec3 gPosition;
layout (location = 1) out vec3 gNormal;
layout (location = 2) out vec3 gAlbedo;
layout (location = 3) out vec4 gSpecular;

in vec3 normal;
in vec3 posForColoring;
in vec2 texCoord;

//uniform vec3 ka;
uniform vec3 kd;
uniform vec3 ks;
uniform float specularExponent;

uniform bool usetexd;
uniform sampler2D diffuseTexture;
uniform bool usetexs;
uniform sampler2D specularTexture;

uniform vec3 lightPosition;

void main(){
	gPosition = posForColoring;
	gNormal = normalize(normal);
	gAlbedo = usetexd ? texture(diffuseTexture, texCoord).rgb : kd;
	gSpecular.rgb = usetexs ? texture(specularTexture, texCoord).rgb : ks;
	gSpecular.a = specularExponent;

}
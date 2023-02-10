#version 460 core

out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D colorTexture;
uniform sampler2D refTexture;

void main(){
	FragColor = vec4(texture(colorTexture, texCoord).rgb, 1);
}

#version 460 core

out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D colorTexture;

void main(){
	FragColor = vec4(texture(colorTexture, texCoord).rgb, 1);
}

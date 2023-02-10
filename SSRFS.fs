#version 460 core

layout (location = 0) out vec4 colorBufferOut;

uniform sampler2D gNormal;
uniform sampler2D colorBuffer;
uniform sampler2D depthMap;

uniform float SCR_WIDTH;
uniform float SCR_HEIGHT;
uniform mat4 invProj;

void main(){
	colorBufferOut = vec4(1,1,1,1);
}

#ifndef QUAD_H
#define QUAD_H

#include <glad/glad.h> 
#include "Shader.h"

class Quad {
private:
	GLuint VAO, VBO, EBO;
public:
	Quad(){
		float vertices[] = {
			-1,  1, 0,  0, 1,
			 1,  1, 0,  1, 1,
			-1, -1, 0,  0, 0,
			 1, -1, 0,  1, 0
		};
		float indices[]{
			0, 1, 2,
			1, 2, 3
		};

		glGenVertexArrays(1, &VAO);
		glGenBuffers(1, &VBO);
		glGenBuffers(1, &EBO);
		glBindVertexArray(VAO);
		glBindBuffer(GL_ARRAY_BUFFER, VBO);
		glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);
		glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 2 * sizeof(float), (void*)(3 * sizeof(float)));
		glEnableVertexAttribArray(0);
	};

	GLuint GetVAO() {
		return VAO;
	}
};
#endif

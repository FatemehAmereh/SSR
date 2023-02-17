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
			-1.0f,  1.0f, -1.0f,  0.0f, 1.0f,
			 1.0f,  1.0f, -1.0f,  1.0f, 1.0f,
			-1.0f, -1.0f, -1.0f,  0.0f, 0.0f,
			 1.0f, -1.0f, -1.0f,  1.0f, 0.0f
		};
		unsigned int indices[]{
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
		glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)(3 * sizeof(float)));
		glEnableVertexAttribArray(1);
	};

	~Quad() {
		glDeleteVertexArrays(1, &VAO);
		glDeleteBuffers(1, &VBO);
		glDeleteBuffers(1, &EBO);
	}

	GLuint GetVAO() {
		return VAO;
	}
};
#endif

#ifndef CAMERA_H
#define CAMERA_H

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

enum Movement { FORWARD, BACKWARD, RIGHT, LEFT };

class Camera {
private:
	glm::vec3 cameraPos;
	glm::vec3 cameraFront;
	glm::vec3 cameraUp;
	glm::vec3 cameraRight;// = glm::normalize(glm::cross(cameraFront, cameraUp));

	float yaw;
	float pitch;
	float mouseSensitivity;
	float movement_speed;
	bool firstMovementofMouse = true;

	double xPos = 0, yPos = 0;
	glm::vec3 worldUpVector = glm::vec3(0, 1, 0);
public:
	Camera(glm::vec3 cp = glm::vec3(0, 0, 0), glm::vec3 cu = glm::vec3(0.0f, 1.0f, 0.0f), glm::vec3 cf = glm::vec3(0.0f, 0.0f, -1.0f),
		glm::vec3 cr = glm::vec3(1.0f, 0.0f, 0.0f), float yaw = -90.0f, float pitch = 0.0f, float mouseSensitivity = 0.1f, float movement_speed = 10.0f) : cameraPos(cp),
		cameraUp(cu), cameraFront(cf), cameraRight(cr), yaw(yaw), pitch(pitch), mouseSensitivity(mouseSensitivity), movement_speed(movement_speed) {};

	void move(Movement m, float deltaTime) {
		switch (m) {
		case FORWARD:
			cameraPos += cameraFront * movement_speed * deltaTime;
			break;
		case BACKWARD:
			cameraPos -= cameraFront * movement_speed * deltaTime;
			break;
		case RIGHT:
			cameraPos += glm::normalize(glm::cross(cameraFront, cameraUp)) * movement_speed * deltaTime;
			break;
		case LEFT:
			cameraPos -= glm::normalize(glm::cross(cameraFront, cameraUp)) * movement_speed * deltaTime;
			break;
		}
	}

	void rotate(float xpos, float ypos) {
		if (firstMovementofMouse) {
			xPos = xpos;
			yPos = ypos;
			firstMovementofMouse = false;
		}
		float xoffset;
		float yoffset;
		xoffset = xpos - xPos;
		yoffset = ypos - yPos;
		xPos = xpos;
		yPos = ypos;
		xoffset *= mouseSensitivity;
		yoffset *= mouseSensitivity;

		yaw += xoffset;
		pitch += yoffset;

		if (pitch > 89.0f)
			pitch = 89.0f;
		if (pitch < -89.0f)
			pitch = -89.0f;

		glm::vec3 front;
		front.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch));
		front.y = -sin(glm::radians(pitch));
		front.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch));
		cameraFront = glm::normalize(front);
		cameraRight = glm::normalize(glm::cross(cameraFront, worldUpVector));
		cameraUp = glm::normalize(-glm::cross(cameraFront, cameraRight));
	}

	glm::mat4 GetViewMatrix() {
		return glm::lookAt(cameraPos, cameraPos + cameraFront, cameraUp);
	}

	glm::vec3 getCameraPosition() const {
		return cameraPos;
	}
};

#endif

#include <glad/glad.h> 
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <iostream>
#include <vector>
#include <memory>

#include "Shader.h"
#include "Model.h"
#include "cyTriMesh.h"
#include "Camera.h"

void framebuffer_size_callback(GLFWwindow* window, int width, int height);
void processInput(GLFWwindow* window);
void mouseMovementCallback(GLFWwindow* window, double xpos, double ypos);

#pragma region parameters
const unsigned int SCR_WIDTH = 800;
const unsigned int SCR_HEIGHT = 800;

Camera camera(glm::vec3(0, 0, 30.0f));
float cameraSpeed = 2.0f;
float deltaTime = 0.0f;
float lastFrame = 0.0f;

std::vector<Model*> models;
#pragma endregion
glm::vec3 lightPosition = glm::vec3(0, 10, 10);

int main() {

#pragma region OpenGL Initialization
    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 4);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 6);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
    glfwWindowHint(GLFW_OPENGL_DEBUG_CONTEXT, false);

    GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "SSR", NULL, NULL);
    if (window == NULL)
    {
        std::cout << "Failed to create GLFW window" << std::endl;
        glfwTerminate();
        return -1;
    }
    glfwMakeContextCurrent(window);
    glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
    glfwSetCursorPosCallback(window, mouseMovementCallback);
    glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);


    if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
    {
        std::cout << "Failed to initialize GLAD" << std::endl;
        return -1;
    }

    glEnable(GL_DEPTH_TEST);
#pragma endregion
    glm::mat4 projection = glm::mat4(1.0f);
    projection = glm::perspective(glm::radians(45.0f), (float)(SCR_WIDTH / SCR_HEIGHT), 0.1f, 100.0f);

    cyTriMesh Objectctm;
    if (!Objectctm.LoadFromFileObj("Models/bunny.obj")) {
        return -1;
    }

    cyTriMesh Cubectm;
    if (!Cubectm.LoadFromFileObj("Models/cube.obj")) {
        return -1;
    }

    Shader generalShader("VertexShader.vs", "FragmentShader.fs");
    models.push_back(new Model(Objectctm, generalShader, glm::vec3(0.0f, 0.0f, 0.0f), glm::vec3(0.5f, 0.5f, 0.5f), projection, true, false));
    models.push_back(new Model(Cubectm, generalShader, glm::vec3(0.0f, -9.0f, 0.0f), glm::vec3(0.6f, 1.0f, 0.6f), projection, true, false));

    glm::mat4 view = glm::mat4(1.0);

    while (!glfwWindowShouldClose(window))
    {
        processInput(window);
        float currentFrame = static_cast<float>(glfwGetTime());
        deltaTime = currentFrame - lastFrame;
        lastFrame = currentFrame;

        glClearColor(0, 0, 0, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        view = camera.GetViewMatrix();
        for (Model* m : models)
        {
            m->setLightPosition(view * glm::vec4(lightPosition, 1));
            m->Draw(view);
        }

        glfwSwapBuffers(window);
        glfwPollEvents();
    }

    glfwTerminate();
    return 0;
}

void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
    glViewport(0, 0, width, height);
}

void processInput(GLFWwindow* window)
{
    if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS) {
        glfwSetWindowShouldClose(window, true);
    }
    if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS) {
        camera.move(FORWARD, deltaTime * cameraSpeed);
    }
    if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS) {
        camera.move(BACKWARD, deltaTime * cameraSpeed);
    }
    if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS) {
        camera.move(LEFT, deltaTime * cameraSpeed);
    }
    if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS) {
        camera.move(RIGHT, deltaTime * cameraSpeed);
    }
}

void mouseMovementCallback(GLFWwindow* window, double xpos, double ypos) {
    camera.rotate(xpos, ypos);
}

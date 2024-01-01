#version 330 core
layout(location = 0) in vec3 position;

uniform mat4 mvp;

void main(void)
{
	gl_Position =  vec4(position, 1.0) * mvp; // OpenTK row-based matrix order
}
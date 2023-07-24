#version 330 core
layout(location = 0) in vec2 position;
layout(location = 1) in vec2 textureCoords;

out vec2 pass_texture;

uniform mat4 modelProjection;

void main(void)
{
	pass_texture = textureCoords;
	gl_Position = vec4(position.xy, 0.0, 1.0) * modelProjection; // openTK is row-based so you multiply inverse
}
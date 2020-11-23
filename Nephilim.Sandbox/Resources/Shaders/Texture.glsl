#type vertex

#version 430 core

layout(location = 0) in vec3 position;
layout(location = 1) in vec2 textureCoords;
layout(location = 2) in vec4 color;
layout(location = 3) in float textureID;

out vec2 pass_textureCoordinates;
out float pass_textureID;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

void main(void) {
	gl_Position = projectionMatrix * viewMatrix * vec4(position, 1.0);
	pass_textureCoordinates = textureCoords;
	pass_textureID = textureID;
}

#type fragment

#version 430 core

in vec2 pass_textureCoordinates;
in float pass_textureID;

out vec4 out_Color;

uniform sampler2D textures[32];

void main(void){

	vec4 color = vec4(1,0,0,1);//texture(textures[int(pass_textureID)], pass_textureCoordinates);

	//if(color.a < 0.5) 
	//	discard;

	out_Color = color;
}
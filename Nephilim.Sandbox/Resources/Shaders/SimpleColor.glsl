#type vertex

#version 430 core

layout(location = 0) in vec3 position;
layout(location = 1) in vec2 textureCoords;

out vec2 pass_textureCoordinates;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform vec4 textureOffset;
uniform float useTextureOffset;

void main(void) {
	gl_Position = projectionMatrix * viewMatrix * transformationMatrix * vec4(position, 1.0);
	pass_textureCoordinates = useTextureOffset > 0.5 ?  vec2(
	(textureCoords.x*textureOffset.z) + (textureOffset.x*textureOffset.z), 
	(textureCoords.y*textureOffset.w) + (textureOffset.y*textureOffset.w)) : textureCoords;
	
}

#type fragment

#version 430 core

in vec2 pass_textureCoordinates;

out vec4 out_Color;

uniform sampler2D modelTexture;
uniform float useColorOnly;
uniform vec4 color;

void main(void){

	vec4 color = useColorOnly < 0.5 ? texture(modelTexture, pass_textureCoordinates) : color;

	if(color.a < 0.5) 
		discard;

	out_Color = color;

}
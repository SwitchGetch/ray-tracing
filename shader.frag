#version 330 core
out vec4 FragColor; 

uniform vec2 u_resolution;
uniform float u_totalTime;

bool sphereIntersect(in vec3 ro, in vec3 rd, in vec3 ce, float ra)
{
    vec3 v = ro - ce;
    float a = dot(rd, rd);
    float b = 2 * dot(rd, v);
    float c = dot(v, v) - ra * ra;
    float D = b * b - 4 * a * c;
    
    if (D > 0)
    {
	return (-b + sqrt(D)) / (2 * a) > 0;
    }
    else if (D == 0)
    {
	return -b / (2 * a) > 0;
    }
    else
    {
	return false;
    }
}

void main()
{
    vec2 uv = gl_FragCoord.xy / u_resolution - 0.5;
    
    vec3 rayOrigin = vec3(0.0);
    vec3 rayDirection = normalize(vec3(uv, 1.0));
    vec3 sphereCenter = vec3(3.0 * sin(u_totalTime), 0.0, 10.0);
    float sphereRadius = 1.0;

    vec4 color = vec4(0.0, 0.0, 0.0, 1.0);

    if (sphereIntersect(rayOrigin, rayDirection, sphereCenter, sphereRadius))
    {
	color = vec4(1.0, 0.0, 0.0, 1.0);
    }

    FragColor = color;
} 
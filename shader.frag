#version 330 core
out vec4 FragColor; 

uniform vec2 u_resolution;
uniform float u_time;
uniform vec3 u_cameraPosition;
uniform vec2 u_cameraAngle;

mat3 rotateX(float angle)
{
    float s = sin(angle);
    float c = cos(angle);

    return mat3(1.0, 0.0, 0.0, 0.0, c, s, 0.0, -s, c);
}

mat3 rotateY(float angle)
{
    float s = sin(angle);
    float c = cos(angle);

    return mat3(c, 0.0, -s, 0.0, 1.0, 0.0, s, 0.0, c);
}

mat3 rotateZ(float angle)
{
    float s = sin(angle);
    float c = cos(angle);

    return mat3(c, s, 0.0, -s, c, 0.0, 0.0, 0.0, 1.0);
}

float sphereIntersect(in vec3 ro, in vec3 rd, in vec3 ce, float ra)
{
    vec3 v = ro - ce;
    float a = dot(rd, rd);
    float b = 2 * dot(rd, v);
    float c = dot(v, v) - ra * ra;
    float D = b * b - 4 * a * c;
    
    if (D > 0)
    {
	    D = sqrt(D);
        a *= 2;

        float t1 = (-b - D) / a;
        float t2 = (-b + D) / a;

        if (t1 > 0) return t1;
        else if (t2 > 0) return t2;
    }
    else if (D == 0)
    {
	    float t = -b / (2 * a);

        if (t > 0) return t;
    }

    return 0.0;
}

float planeIntersect(in vec3 ro, in vec3 rd, in vec3 po, in vec3 pn)
{
    float d = dot(rd, pn);

    if (d == 0) return 0.0;

    float t = dot(pn, po - ro) / d;

    if (t > 0) return t;

    return 0.0;
}

vec4 castRay(in vec3 ro, in vec3 rd)
{
    vec3 ce = vec3(0.0, 0.0, 5.0);
    float ra = 1.0;
    vec3 po = vec3(0.0, -1.0, 0.0);
    vec3 pn = vec3(0.0, 1.0, 0.0);

    float minDistance = 0.0;
    float tempDistance = 0.0;

    vec3 cross = vec3(0.0);
    vec3 n = vec3(0.0);
    vec4 objectColor = vec4(0.0, 0.0, 0.0, 1.0);

    tempDistance = sphereIntersect(ro, rd, ce, ra);
    if (tempDistance > 0.0 && (tempDistance < minDistance || minDistance == 0.0))
    {
        minDistance = tempDistance;
        cross = ro + minDistance * rd;
        n = normalize(cross - ce);
        objectColor = vec4(1.0, 0.0, 0.0, 1.0);
    }

    tempDistance = planeIntersect(ro, rd, po, pn);
    if (tempDistance > 0.0 && (tempDistance < minDistance || minDistance == 0.0))
    {
        minDistance = tempDistance;
        cross = ro + minDistance * rd;
        n = pn;
        objectColor = vec4(0.0, 0.0, 1.0, 1.0);
    }

    if (minDistance <= 0.0) return objectColor;

    vec3 light = normalize(vec3(-1.0, -1.0, 1.0)) * rotateY(u_time);
    vec3 v = normalize(ro - cross);
    vec3 r = reflect(light, n);

    float ambient = 0.5;
    float diffuse = 0.5 * max(0.0, dot(n, -light));
    float specular = 0.5 * pow(max(0.0, dot(r, v)), 25.0);

    return (ambient + diffuse + specular) * objectColor;
}

void main()
{
    vec2 uv = gl_FragCoord.xy / u_resolution - 0.5;
    
    vec3 rayOrigin = u_cameraPosition;
    vec3 rayDirection = normalize(vec3(uv, 1.0)) * rotateX(u_cameraAngle.x) * rotateY(u_cameraAngle.y);

    FragColor = castRay(rayOrigin, rayDirection);
} 
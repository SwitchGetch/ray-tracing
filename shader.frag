#version 330 core
out vec4 FragColor; 

uniform vec2 u_resolution;
uniform float u_time;

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

bool sphereIntersect(in vec3 ro, in vec3 rd, in vec3 ce, float ra, out vec3 cross)
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

        if (t1 > 0)
        {
            cross = ro + t1 * rd;
            return true;
        }
        else if (t2 > 0)
        {
            cross = ro + t2 * rd;
            return true;
        }
        else
        {
            cross = vec3(0.0);
            return false;
        }
    }
    else if (D == 0)
    {
	    float t = -b / (2 * a);

        if (t > 0)
        {
            cross = ro + t * rd;
            return true;
        }
        else
        {
            cross = vec3(0.0);
            return false;
        }
    }
    else
    {
	    cross = vec3(0.0);
        return false;
    }
}

vec4 castRay(in vec3 ro, in vec3 rd)
{
    vec3 ce = vec3(0.0, 0.0, 5.0);
    float ra = 1.0;
    vec3 cross = vec3(0.0);

    if (!sphereIntersect(ro, rd, ce, ra, cross)) return vec4(vec3(0.0), 1.0);

    vec4 sphereColor = vec4(1.0, 0.0, 0.0, 1.0);

    vec3 light = normalize(vec3(0.0, 0.0, 1.0)) * rotateY(u_time);
    vec3 n = normalize(cross - ce);

    float diffuse = max(0.0, dot(n, -light));

    return diffuse * sphereColor;
}

void main()
{
    vec2 uv = gl_FragCoord.xy / u_resolution - 0.5;
    
    vec3 rayOrigin = vec3(0.0);
    vec3 rayDirection = normalize(vec3(uv, 1.0));

    FragColor = castRay(rayOrigin, rayDirection);
} 

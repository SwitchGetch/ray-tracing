#version 330 core
out vec4 FragColor; 

uniform vec2 u_resolution;
uniform float u_time;
uniform vec3 u_cameraPosition;
uniform vec2 u_cameraAngle;

struct Ray
{
    vec3 origin;
    vec3 direction;
};

struct Sphere
{
    float radius;
    vec3 center;
    vec4 color;
};

struct Plane
{
    vec3 origin;
    vec3 normal;
    vec4 color;
};

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

float sphereIntersect(Ray r, Sphere s)
{
    vec3 v = r.origin - s.center;
    float a = dot(r.direction, r.direction);
    float b = 2 * dot(r.direction, v);
    float c = dot(v, v) - s.radius * s.radius;
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

float planeIntersect(Ray r, Plane p)
{
    float d = dot(r.direction, p.normal);

    if (d == 0) return 0.0;

    float t = dot(p.normal, p.origin - r.origin) / d;

    if (t > 0) return t;

    return 0.0;
}

vec4 castRay(Ray r)
{
    Sphere s = Sphere(1.0, vec3(0.0, 0.0, 5.0), vec4(1.0, 0.0, 0.0, 1.0));
    Plane p = Plane(vec3(0.0, -1.0, 0.0), vec3(0.0, 1.0, 0.0), vec4(0.0, 0.0, 1.0, 1.0));

    float minDistance = 0.0;
    float tempDistance = 0.0;

    vec3 intersect = vec3(0.0);
    vec3 n = vec3(0.0);
    vec4 objectColor = vec4(0.0, 0.0, 0.0, 1.0);

    tempDistance = sphereIntersect(r, s);
    if (tempDistance > 0.0 && (tempDistance < minDistance || minDistance == 0.0))
    {
        minDistance = tempDistance;
        intersect = r.origin + minDistance * r.direction;
        n = normalize(intersect - s.center);
        objectColor = s.color;
    }

    tempDistance = planeIntersect(r, p);
    if (tempDistance > 0.0 && (tempDistance < minDistance || minDistance == 0.0))
    {
        minDistance = tempDistance;
        intersect = r.origin + minDistance * r.direction;
        n = p.normal;
        objectColor = p.color;
    }

    if (minDistance <= 0.0) return objectColor;

    vec3 light = normalize(vec3(-1.0, -1.0, 1.0)) * rotateY(u_time);
    vec3 onCamera = normalize(r.origin - intersect);
    vec3 reflected = reflect(light, n);

    float ambient = 0.5;
    float diffuse = 0.5 * max(0.0, dot(n, -light));
    float specular = 0.5 * pow(max(0.0, dot(reflected, onCamera)), 25.0);

    return (ambient + diffuse + specular) * objectColor;
}

void main()
{
    vec2 uv = (gl_FragCoord.xy - 0.5 * u_resolution) / min(u_resolution.x, u_resolution.y);
    
    vec3 rayOrigin = u_cameraPosition;
    vec3 rayDirection = normalize(vec3(uv, 1.0)) * rotateX(u_cameraAngle.x) * rotateY(u_cameraAngle.y);

    FragColor = castRay(Ray(rayOrigin, rayDirection));
} 
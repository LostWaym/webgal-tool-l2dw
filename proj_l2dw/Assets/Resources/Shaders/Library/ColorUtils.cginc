#ifndef COLOR_UTILS_INCLUDED
#define COLOR_UTILS_INCLUDED

float3 HsvToRgb(float h, float s, float v)
{
    // h = pow(h, 2.2);
    // s = pow(s, 1.0 / 2.2);
    // v = pow(v, 2.2);
    float3 rgb = clamp(
        abs(fmod(h * 6.0 + float3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0,
        0.0,
        1.0
    );
    // rgb = rgb*rgb*(3.0-2.0*rgb);

    return v * lerp( float3(1.0, 1.0, 1.0), rgb, s);
}

#endif
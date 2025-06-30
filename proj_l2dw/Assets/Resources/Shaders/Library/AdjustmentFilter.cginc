#ifndef ADJUSTMENT_FILTER_INCLUDED
#define ADJUSTMENT_FILTER_INCLUDED

#include "./WebgalContainerInput.cginc"

float _Brightness;
float _Contrast;
float _Saturation;
float _Gamma;
float _ColorRed;
float _ColorGreen;
float _ColorBlue;

fixed4 ApplyAdjustmentFilter(float2 rawUv)
{
    fixed4 tex = tex2D(_MainTex, rawUv);
    // 第一次矫正
    tex.rgb = pow(tex.rgb, 1.0 / 2.0);
    
    float3 rgb = pow(tex.rgb, 1.0 / _Gamma);
    float s = dot(float3(0.2125, 0.7154, 0.0721), rgb);
    rgb = lerp(float3(s, s, s), rgb, _Saturation);
    float gray = 0.5;
    rgb = lerp(float3(gray, gray, gray), rgb, _Contrast);
    rgb.r *= _ColorRed / 255.0;
    rgb.g *= _ColorGreen / 255.0;
    rgb.b *= _ColorBlue / 255.0;
    tex.rgb = rgb * _Brightness;

    tex.rgb = clamp(tex.rgb, 0.0, 1.0);

    return tex;
}

#endif 
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
    
    float3 rgb = pow(tex.rgb, 1.0 / _Gamma);
    float s = dot(float3(0.2125, 0.7154, 0.0721), rgb);
    rgb = lerp(float3(s, s, s), rgb, _Saturation);
    float gray = pow(0.5, 2.2);
    rgb = lerp(float3(gray, gray, gray), rgb, _Contrast);
    rgb.r *= pow(_ColorRed / 255.0, 2.2);
    rgb.g *= pow(_ColorGreen / 255.0, 2.2);
    rgb.b *= pow(_ColorBlue / 255.0, 2.2);
    tex.rgb = rgb * pow(_Brightness, 2.2);

    return fixed4(tex);
}

#endif 
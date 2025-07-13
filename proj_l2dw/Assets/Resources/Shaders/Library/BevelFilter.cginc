#ifndef BEVEL_FILTER_INCLUDED
#define BEVEL_FILTER_INCLUDED

#include "WebgalContainerInput.cginc"
#include "AdjustmentFilter.cginc"

#pragma multi_compile _ _BEVEL_FILTER_LEGACY
#pragma multi_compile _ _BEVEL_FILTER_SOFTNESS

float _Bevel;
float _BevelThickness;
float _BevelRotation;
float _BevelSoftness;
float _BevelRed;
float _BevelGreen;
float _BevelBlue;

fixed4 ApplyBevelFilter(float2 rawUv)
{
    fixed3 bevelColor = fixed3(_BevelRed, _BevelGreen, _BevelBlue) / 255.0;
    float bevelRadians = radians(_BevelRotation * -1.0);
    float2 sampleVector = float2(cos(bevelRadians), sin(bevelRadians));
    sampleVector *= -1.0;
    sampleVector *= _BevelThickness;
    sampleVector *= float2(_SampleScaleX, _SampleScaleY);

    fixed4 color = ApplyAdjustmentFilter(rawUv);

#if (_BEVEL_FILTER_SOFTNESS && !_BEVEL_FILTER_LEGACY)
    const int kernelSize = 7;
    float weightSum = 0.0;
    float sigma = 4;
    float bevelAlpha = 0.0;

    // UNITY_UNROLL
    UNITY_LOOP
    for (int i = 0; i < kernelSize; i++)
    {
        // float weight = exp(-(i*i) / (2 * sigma * sigma));
        float weight = (((float)i + 1.0) / (float)kernelSize);
        float2 sampleOffset = sampleVector * (((float)i + 1.0) / (float)kernelSize);
        sampleOffset = lerp(sampleVector, sampleOffset, _BevelSoftness);
        fixed4 tex = tex2D(_MainTex, rawUv + sampleOffset);
        bevelAlpha += clamp((color.a - tex.a) * clamp(_Bevel, 0.0, 1.0), 0.0, 1.0) * weight;
        weightSum += weight;
    }
    // bevelAlpha /= (float)kernelSize;
    bevelAlpha /= weightSum;
#else
    fixed4 tex = tex2D(_MainTex, rawUv + sampleVector);
    float bevelAlpha = clamp((color.a - tex.a) * clamp(_Bevel, 0.0, 1.0), 0.0, 1.0);
#endif
    
#if _BEVEL_FILTER_LEGACY
    color.rgb = lerp(color.rgb, bevelColor, bevelAlpha);
#else
    color.rgb = lerp(
        color.rgb,
        1.0 - (1.0 - color.rgb) * (1.0 - bevelColor),
        bevelAlpha
    );
#endif
    color.rgb = clamp(color.rgb, 0.0, 1.0);
    
    return color;
}

#endif
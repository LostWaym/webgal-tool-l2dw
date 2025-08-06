#ifndef BLOOM_FILTER_INCLUDED
#define BLOOM_FILTER_INCLUDED

#include "UnityCG.cginc"
#include "BevelFilter.cginc"

#pragma multi_compile _ _BLOOM_FILTER_BLUR

float _Bloom;
float _BloomBrightness;
float _BloomBlur;
float _BloomThreshold;

float3 ExtractBrightness(float3 color)
{
    float _max = max(max(color.r, color.g), color.b);
    float _min = min(min(color.r, color.g), color.b);
    float brightness = (_max + _min) * 0.5;

    float3 result = lerp(
        float3(0.0, 0.0, 0.0),
        color,
        step(_BloomThreshold, brightness)
    );
    return result;
}

fixed4 BloomBlur(float2 rawUv)
{
    fixed4 colInner = fixed4(0, 0, 0, 0);

    float weightSumInner = 0.0;
                
    const int kernelSize = 3; // 可以改大或用参数控制
    float sigma = 4; // 控制模糊强度
    float2 blurUv = float2(_SampleScaleX, _SampleScaleY) * _BloomBlur * 2.5;
    
    // 高斯权重计算
    // UNITY_UNROLL
    UNITY_LOOP
    for (int x = -kernelSize; x <= kernelSize; x++)
    {
        UNITY_LOOP
        for (int y = -kernelSize; y <= kernelSize; y++)
        {
            float2 offset = float2(x, y) * blurUv / kernelSize;
            float weight = exp(-(x*x + y*y) / (2 * sigma * sigma));
            // float weight = 1.0;
            fixed4 sampleColor = ApplyBevelFilter(rawUv + offset);
            sampleColor.rgb = ExtractBrightness(sampleColor.rgb);

            colInner += sampleColor * weight;
            weightSumInner += weight;
        }
    }

    colInner /= weightSumInner; // 归一化颜色值

    fixed4 col = colInner;
    
    return col;
}

fixed4 ApplyBloomFilter(float2 rawUv)
{
    fixed4 color = ApplyBevelFilter(rawUv);
#if _BLOOM_FILTER_BLUR
    fixed4 bloomColor = BloomBlur(rawUv);
    // bloomColor.rgb += (1.0 - color.a) * (1.0 - bloomColor.rgb) * 0.2;
    // bloomColor.rgb += (1.0 - color.a) * bloomColor.rgb;

    float oldBloomAlpha = bloomColor.a;
    bloomColor.rgb += (1.0 - color.a) * bloomColor.rgb / oldBloomAlpha;
    bloomColor.a = lerp(bloomColor.a * bloomColor.a, bloomColor.a, color.a);
    // bloomColor.a = lerp(color.a, bloomColor.a, _Bloom);
    bloomColor.a = bloomColor.a * _Bloom;
    // bloomColor.a = lerp(
    //     dot(bloomColor.rgb, bloomColor.rgb) / 3.0,
    //     bloomColor.a,
    //     color.a
    // );
#else
    fixed4 bloomColor = fixed4(
        ExtractBrightness(color.rgb),
        0.0
    );
#endif

    color.rgb *= _BloomBrightness;
    bloomColor.rgb *= _Bloom;
    
    fixed4 result = color + bloomColor;
    result = saturate(result);
    // const float baseAlpha = 0.0001;
    // result.rgb = lerp(result.rgb * result.a / baseAlpha, result.rgb, color.a);
    // result.a = lerp(baseAlpha, result.a, color.a);
    
    return result;
    // return bloomColor;
}


#endif
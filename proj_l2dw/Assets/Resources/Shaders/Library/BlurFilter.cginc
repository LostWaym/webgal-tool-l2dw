#ifndef BLUR_FILTER_INCLUDED
#define BLUR_FILTER_INCLUDED

#include "UnityCG.cginc"
#include "WebgalContainerInput.cginc"
#include "BloomFilter.cginc"

#pragma multi_compile _ _BLUR_FILTER

float _Blur;

fixed4 ApplyBlurFilter(float2 rawUv)
{
#ifndef _BLUR_FILTER
    // return tex2D(_MainTex, rawUv);
    return ApplyBloomFilter(rawUv);
#else
    fixed4 col = fixed4(0, 0, 0, 0);

    float weightSum = 0.0;
                
    const int kernelSize = 3; // 可以改大或用参数控制
    float sigma = 4; // 控制模糊强度
    float2 blurUv = float2(_SampleScaleX, _SampleScaleY) * _Blur;
    
    // 高斯权重计算
    UNITY_UNROLL
    for (int x = -kernelSize; x <= kernelSize; x++)
    {
        for (int y = -kernelSize; y <= kernelSize; y++)
        {
            float2 offset = float2(x, y) * blurUv / kernelSize;
            float weight = exp(-(x*x + y*y) / (2 * sigma * sigma));
            fixed4 sampleColor = ApplyBloomFilter(rawUv + offset);
            col += sampleColor * weight;
            weightSum += weight;
        }
    }

    col /= weightSum; // 归一化颜色值
    
    return col;
#endif
}

#endif
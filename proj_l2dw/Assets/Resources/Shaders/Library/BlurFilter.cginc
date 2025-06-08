#ifndef BLUR_FILTER_INCLUDED
#define BLUR_FILTER_INCLUDED

#include "UnityCG.cginc"
#include "./WebgalContainerInput.cginc"

#pragma multi_compile _ _BLUR_FILTER

float _Blur;
float _BlurSampleScaleX;
float _BlurSampleScaleY;

fixed4 ApplyBlurFilter(float2 rawUv)
{
#ifndef _BLUR_FILTER
    return tex2D(_MainTex, rawUv);
#else
    fixed4 col = fixed4(0, 0, 0, 0);

    float weightSum = 0.0;
                
    const int kernelSize = 5; // 可以改大或用参数控制
    float sigma = 4; // 控制模糊强度
    float2 blurUv = float2(_Blur * _BlurSampleScaleX, _Blur * _BlurSampleScaleY);
    
    // 高斯权重计算
    UNITY_UNROLL
    for (int x = -kernelSize; x <= kernelSize; x++)
    {
        for (int y = -kernelSize; y <= kernelSize; y++)
        {
            float2 offset = float2(x, y) * blurUv / kernelSize;
            float weight = exp(-(x*x + y*y) / (2 * sigma * sigma));
            col += tex2D(_MainTex, rawUv + offset) * weight;
            weightSum += weight;
        }
    }

    return col / weightSum; // 归一化颜色值
#endif
}

#endif
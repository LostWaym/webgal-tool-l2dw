#ifndef BLUR_FILTER_INCLUDED
#define BLUR_FILTER_INCLUDED

#include "UnityCG.cginc"
#include "WebgalContainerInput.cginc"
#include "BevelFilter.cginc"

#pragma multi_compile _ _BLUR_FILTER

float _Blur;

fixed4 ApplyBlurFilter(float2 rawUv)
{
#ifndef _BLUR_FILTER
    // return tex2D(_MainTex, rawUv);
    return ApplyBevelFilter(rawUv);
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
            fixed4 sampleColor = ApplyBevelFilter(rawUv + offset);
            sampleColor.rgb = pow(sampleColor.rgb, 1.0 / 2.2);
            col += sampleColor * weight;
            weightSum += weight;
        }
    }

    col /= weightSum; // 归一化颜色值
    col.rgb = pow(col.rgb, 2.2);
    
    return col;
#endif
}

#endif
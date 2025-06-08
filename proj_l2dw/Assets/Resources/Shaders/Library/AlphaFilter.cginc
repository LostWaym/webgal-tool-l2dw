#ifndef ALPHA_FILTER_INCLUDED
#define ALPHA_FILTER_INCLUDED

float _Alpha;

fixed4 ApplyAlphaFilter(fixed4 inputColor)
{
    _Alpha = clamp(_Alpha, 0.0f, 1.0f);
    return fixed4(inputColor.rgb, inputColor.a * _Alpha);
}

#endif

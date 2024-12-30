Shader "Hidden/Shader/EdgeDetect"
{
    HLSLINCLUDE
    #pragma target 4.5
    #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/NormalBuffer.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord   : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
    };
    Varyings Vert(Attributes input)
    {
        Varyings output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
        return output;
    }

    // List of properties to control your post process effect
    float _IntensityColor;
    float _ThicknessColor;
    float _RangeMinColor;
    float _RangeMaxColor;
    float _IntensityDepth;
    float _ThicknessDepth;
    float _RangeMinDepth;
    float _RangeMaxDepth;
    float _IntensityNormal;
    float _ThicknessNormal;
    float _RangeMinNormal;
    float _RangeMaxNormal;
    float4 _BGColor;
    float4 _FGColor;
    TEXTURE2D_X(_InputTexture);

    float bw(float4 c)
    {
        return dot(c.rgb, float3(0.3, 0.59, 0.11));
    }

    float sobelColor(Texture2DArray<float4> tex, float2 uv)
    {
        float2 delta = float2(_ThicknessColor, _ThicknessColor);
        float hr = 0;
        float vt = 0;
        float c;
        c = bw(LOAD_TEXTURE2D_X(tex, (uv + float2(-1.0, -1.0) * delta)));
        hr += c *  1.0;
        vt += c *  1.0;
        c = bw(LOAD_TEXTURE2D_X(tex, (uv + float2( 0.0, -1.0) * delta)));
        vt += c *  2.0;
        c = bw(LOAD_TEXTURE2D_X(tex, (uv + float2( 1.0, -1.0) * delta)));
        hr += c * -1.0;
        vt += c *  1.0;
        c = bw(LOAD_TEXTURE2D_X(tex, (uv + float2(-1.0,  0.0) * delta)));
        hr += c *  2.0;
        c = bw(LOAD_TEXTURE2D_X(tex, (uv + float2( 1.0,  0.0) * delta)));
        hr += c * -2.0;
        c = bw(LOAD_TEXTURE2D_X(tex, (uv + float2(-1.0,  1.0) * delta)));
        hr += c *  1.0;
        vt += c * -1.0;
        c = bw(LOAD_TEXTURE2D_X(tex, (uv + float2( 0.0,  1.0) * delta)));
        vt += c * -2.0;
        c = bw(LOAD_TEXTURE2D_X(tex, (uv + float2( 1.0,  1.0) * delta)));
        hr += c * -1.0;
        vt += c * -1.0;
        float s = sqrt(hr * hr + vt * vt);
        s *= _IntensityColor;
        s = s < _RangeMinColor ? 0 : s;
        s = s > _RangeMaxColor ? 1 : s;
        return s;
    }

    float sobelDepth(float2 uv)
    {
        float2 delta = float2(_ThicknessDepth, _ThicknessDepth);
        float hr = 0;
        float vt = 0;
        float c;
        c = LoadCameraDepth(uv + float2(-1.0, -1.0) * delta);
        hr += c *  1.0;
        vt += c *  1.0;
        c = LoadCameraDepth(uv + float2( 0.0, -1.0) * delta);
        vt += c *  2.0;
        c = LoadCameraDepth(uv + float2( 1.0, -1.0) * delta);
        hr += c * -1.0;
        vt += c *  1.0;
        c = LoadCameraDepth(uv + float2(-1.0,  0.0) * delta);
        hr += c *  2.0;
        c = LoadCameraDepth(uv + float2( 1.0,  0.0) * delta);
        hr += c * -2.0;
        c = LoadCameraDepth(uv + float2(-1.0,  1.0) * delta);
        hr += c *  1.0;
        vt += c * -1.0;
        c = LoadCameraDepth(uv + float2( 0.0,  1.0) * delta);
        vt += c * -2.0;
        c = LoadCameraDepth(uv + float2( 1.0,  1.0) * delta);
        hr += c * -1.0;
        vt += c * -1.0;
        float s = sqrt(hr * hr + vt * vt);
        s *= _IntensityDepth * 100;
        s = s < _RangeMinDepth ? 0 : s;
        s = s > _RangeMaxDepth ? 1 : s;
        return s;
    }

    float3 getNormalFromGBuffer(float2 uv)
    {
        NormalData normalData;
        DecodeFromNormalBuffer(uv, normalData);
        return normalData.normalWS;
    }

    float sobelNormal(float2 uv)
    {
        float2 delta = float2(_ThicknessNormal, _ThicknessNormal);
        float3 o = getNormalFromGBuffer(uv);
        float hr = 0;
        float vt = 0;
        float3 c;
        c = getNormalFromGBuffer(uv + float2(-1.0, -1.0) * delta);
        hr += distance(o, c) * 1.0;
        vt += distance(o, c) * 1.0;
        c = getNormalFromGBuffer(uv + float2( 0.0, -1.0) * delta);
        vt += distance(o, c) * 2.0;
        c = getNormalFromGBuffer(uv + float2( 1.0, -1.0) * delta);
        hr += distance(o, c) * -1.0;
        vt += distance(o, c) *  1.0;
        c = getNormalFromGBuffer(uv + float2(-1.0,  0.0) * delta);
        hr += distance(o, c) * 2.0;
        c = getNormalFromGBuffer(uv + float2( 1.0,  0.0) * delta);
        hr += distance(o, c) *-2.0;
        c = getNormalFromGBuffer(uv + float2(-1.0,  1.0) * delta);
        hr += distance(o, c) * 1.0;
        vt += distance(o, c) *-1.0;
        c = getNormalFromGBuffer(uv + float2( 0.0,  1.0) * delta);
        vt += distance(o, c) *-2.0;
        c = getNormalFromGBuffer(uv + float2( 1.0,  1.0) * delta);
        hr += distance(o, c) *-1.0;
        vt += distance(o, c) *-1.0;
        float s = sqrt(hr * hr + vt * vt);
        s *= _IntensityNormal;
        s = s < _RangeMinNormal ? 0 : s;
        s = s > _RangeMaxNormal ? 1 : s;
        return s;
    }

    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        float s = 0;
        s += sobelColor(_InputTexture, input.texcoord*_ScreenSize.xy);
        s += sobelDepth(input.positionCS.xy);
        s += sobelNormal(input.texcoord*_ScreenSize.xy);
        float4 org = LOAD_TEXTURE2D_X(_InputTexture, input.texcoord*_ScreenSize.xy);
        float3 col = lerp(lerp(_BGColor, org, 1-_BGColor.a), _FGColor, clamp(s, 0, 1));
        return float4(col.rgb, 1);
    }
    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "EdgeDetect"
            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off
            HLSLPROGRAM
                #pragma fragment CustomPostProcess
                #pragma vertex Vert
            ENDHLSL
        }
    }
    Fallback Off
}
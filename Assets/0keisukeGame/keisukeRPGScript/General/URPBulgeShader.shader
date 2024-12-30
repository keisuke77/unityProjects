Shader "Custom/URPBulgeShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _BulgeAmount("Bulge Amount", Range(0, 2)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Name "MainPass"
            HLSLPROGRAM
            #pragma target 2.0
            #pragma exclude_renderers gles
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma renderqueue opaque

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float _BulgeAmount;

            Varyings vert(Attributes input)
            {
                Varyings output;
                float3 bulgedPosition = input.positionOS.xyz + input.normalOS * _BulgeAmount;
                float4 positionWS = mul(unity_ObjectToWorld, float4(bulgedPosition, 1.0));
                output.positionCS = TransformObjectToHClip(positionWS.xyz); // 変換関数の変更
                output.uv = input.uv;
                UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(output, output.positionCS)
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 col = tex2D(_MainTex, input.uv);
                return col;
            }
            ENDHLSL
        }
    }
}

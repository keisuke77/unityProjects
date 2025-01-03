Shader "Custom/FireShader"
{
    Properties
    {
        _NoiseTex ("Noise Texture", 2D) = "white" { }
        _DistortTex ("Distortion Texture", 2D) = "white" { }
        _Color ("Main Flame Color", Color) = (1, 1, 1, 1)
        _Color2 ("Flame Rim Color", Color) = (1, 1, 1, 1)
        _GradientPow ("Gradient Power", Float) = 1.0
        _GradientThickness ("Gradient Thickness", Float) = 1.0
        _EdgePow ("Edge Power", Range(0.0, 10)) = 1.0
        _Hight ("Flame Hight", Range(-0.4, 1.0)) = 0.0
        _Edge ("Rim Strength", Range(-1.0, 1.0)) = 0.0
        _Shininess ("Rim Shininess", Float) = 1.0
        _Distort ("Distortion", Float) = 1.0
        _SpeedX ("Speed X", Float) = 1.0
        _SpeedY ("Speed Y", Float) = 1.0
        [Enum(OFF, 0, ON, 1)] _Hoge ("Toggle Billboard", int) = 0
        [Enum(OFF, 0, FRONT, 1, BACK, 2)] _CullMode ("Cull Mode", int) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull[_CullMode]

        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };

            struct v2f
            {
                float2 uv: TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex: SV_POSITION;
                float2 uvN: TEXCOORD2;
                float2 uvD: TEXCOORD3;
            };

            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _NoiseTex; uniform float4 _NoiseTex_ST;
            uniform sampler2D _DistortTex; uniform float4 _DistortTex_ST;
            uniform float _GradientPow;
            uniform float _GradientThickness;
            uniform float _EdgePow;
            uniform fixed4 _Color;
            uniform float _Hight;
            uniform fixed4 _Color2;
            uniform float _Edge;
            uniform float _Shininess;
            uniform float _Distort;
            uniform float _SpeedX;
            uniform float _SpeedY;
            uniform int _Hoge;
            
            v2f vert(appdata v)
            {
                v2f o;
                float4 pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1)) + float4(v.vertex.x, v.vertex.y, v.vertex.z, 0));
                o.vertex = lerp(UnityObjectToClipPos(v.vertex), pos, _Hoge);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvN = TRANSFORM_TEX(v.uv, _NoiseTex);
                o.uvD = TRANSFORM_TEX(v.uv, _DistortTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                float4 gradient = lerp(fixed4(1, 1, 1, 1) * 2, fixed4(0, 0, 0, 0), i.uv.y + _Hight);

                float2 uvD = float2(i.uvD.x, i.uvD.y) * _Distort;
                fixed4 distort = tex2D(_DistortTex, uvD);

                float2 uvN = float2(i.uvN.x * _SpeedX - distort.r, i.uvN.y - distort.g - _Time.x * _SpeedY);
                fixed4 noise = tex2D(_NoiseTex, uvN);
                
                float a = saturate(pow(gradient.x, _GradientPow) * _GradientThickness);
                gradient = float4(a, a, a, a);
                noise += gradient;
                noise.a = (noise.r + noise.g + noise.b) / 3.0;
                float b = max(0, _EdgePow * 20);
                float edgePow = saturate(pow(noise.a, b));
                float edgePow2 = saturate(pow(noise.a + lerp(_Edge, 0, i.uv.y), b)) - edgePow;

                fixed4 col = edgePow * _Color + edgePow2 * _Color2 * _Shininess;

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
            
        }
    }
    Fallback "Diffuse"
}

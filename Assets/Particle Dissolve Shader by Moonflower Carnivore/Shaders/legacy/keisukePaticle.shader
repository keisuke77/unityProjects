Shader "Custom/Particle Glow Edge Only"
{
    Properties
    {
        _MainTex ("Particle Texture", 2D) = "white" {}
        _TransparentColor ("Transparent Color", Color) = (0,0,0,1)
        _Tolerance ("Color Tolerance", Range(0,1)) = 0.1
        _SingleColor ("Single Color", Color) = (1,1,1,1) // デフォルトで白
        _GlowColor ("Glow Color", Color) = (1,1,1,1) // 発光の色
        _GlowIntensity ("Glow Intensity", Range(0,1)) = 0.5 // 発光の強度
        _GlowThickness ("Glow Thickness", Range(0,1)) = 0.1 // 発光の厚さ
    }
    
    SubShader
    {
        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Opaque" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off 
        Lighting Off 
        ZWrite Off

        // パス1: パーティクル本体の描画
        Pass
        {
            Name "PARTICLE"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _TransparentColor;
            float _Tolerance;
            float4 _SingleColor;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.texcoord = v.texcoord;
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float4 tex = tex2D(_MainTex, i.texcoord);

                // カラー範囲をチェック
                float colorDistance = distance(tex.rgb, _TransparentColor.rgb);
                float alpha = colorDistance < _Tolerance ? 0.0 : 1.0;

                // 透明でない部分をシングルカラーとColor over Lifetimeの乗算に変更
                float3 finalColor = alpha > 0.0 ? (_SingleColor.rgb * i.color.rgb) : tex.rgb;

                return float4(finalColor, alpha);
            }
            ENDCG
        }

        // パス2: 発光エッジの描画
        Pass
        {
            Name "GLOW"
            Blend SrcAlpha One // 発光部分のブレンド
            ZWrite Off // 深度書き込みをオフ
            Cull Off // 両面を描画する

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment glowFrag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _GlowColor;
            float _GlowIntensity;
            float _GlowThickness;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            float4 glowFrag (v2f i) : SV_Target
            {
                // テクスチャをサンプリング
                float4 tex = tex2D(_MainTex, i.texcoord);
                
                // 発光のエッジを計算
                float2 offset = _GlowThickness * (0.5 - abs(0.5 - i.texcoord)); // エッジを広げるためのオフセット
                float4 glow = tex2D(_MainTex, i.texcoord + offset) * _GlowColor * _GlowIntensity;

                return glow;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

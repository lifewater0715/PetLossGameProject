Shader "Sprites/Alpha Glow"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowSize ("Glow Size", Range(0, 30)) = 2
        _GlowPower ("Glow Power", Range(0, 5)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _Color;
            fixed4 _GlowColor;
            float _GlowSize;
            float _GlowPower;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 main = tex2D(_MainTex, i.uv) * i.color;

                float2 offset = _MainTex_TexelSize.xy * _GlowSize;

                float alpha = 0;
                alpha += tex2D(_MainTex, i.uv + float2(offset.x, 0)).a;
                alpha += tex2D(_MainTex, i.uv + float2(-offset.x, 0)).a;
                alpha += tex2D(_MainTex, i.uv + float2(0, offset.y)).a;
                alpha += tex2D(_MainTex, i.uv + float2(0, -offset.y)).a;
                alpha += tex2D(_MainTex, i.uv + float2(offset.x, offset.y)).a;
                alpha += tex2D(_MainTex, i.uv + float2(-offset.x, offset.y)).a;
                alpha += tex2D(_MainTex, i.uv + float2(offset.x, -offset.y)).a;
                alpha += tex2D(_MainTex, i.uv + float2(-offset.x, -offset.y)).a;

                alpha = saturate(alpha * _GlowPower);

                if (main.a > 0.01)
                    return main;

                fixed4 glow = _GlowColor;
                glow.a *= alpha;

                return glow;
            }

            ENDCG
        }
    }
}
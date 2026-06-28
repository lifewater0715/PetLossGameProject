Shader "UI/SpotlightOverlay"
{
    Properties
    {
        _Color ("Overlay Color", Color) = (0, 0, 0, 0.75)
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Radius", Float) = 0.15
        _Feather ("Feather", Float) = 0.05
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _Color;
            float4 _Center;
            float _Radius;
            float _Feather;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = distance(i.uv, _Center.xy);

                float mask = smoothstep(_Radius, _Radius + _Feather, dist);

                fixed4 col = _Color;
                col.a *= mask;

                return col;
            }
            ENDCG
        }
    }
}
Shader "Custom/UIBackgroundBlur"
{
    Properties
    {
        [PerRendererData] _MainTex ("Mask Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 0.85)
        _BlurSize ("Blur Size", Range(0, 8)) = 2
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "IgnoreProjector" = "True"
        }

        Pass
        {
            Name "BackgroundBlur"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 screenPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _Color;
                float _BlurSize;
            CBUFFER_END

            Varyings Vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionHCS = positionInputs.positionCS;
                output.screenPos = ComputeScreenPos(positionInputs.positionCS);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }

            half3 SampleBlurredScene(float2 screenUV, float2 blurOffset)
            {
                half3 color = SampleSceneColor(screenUV) * 0.22702703;
                color += SampleSceneColor(screenUV + float2( blurOffset.x, 0.0)) * 0.19459459;
                color += SampleSceneColor(screenUV + float2(-blurOffset.x, 0.0)) * 0.19459459;
                color += SampleSceneColor(screenUV + float2(0.0,  blurOffset.y)) * 0.19459459;
                color += SampleSceneColor(screenUV + float2(0.0, -blurOffset.y)) * 0.19459459;
                color += SampleSceneColor(screenUV + float2( blurOffset.x,  blurOffset.y)) * 0.06081081;
                color += SampleSceneColor(screenUV + float2(-blurOffset.x,  blurOffset.y)) * 0.06081081;
                color += SampleSceneColor(screenUV + float2( blurOffset.x, -blurOffset.y)) * 0.06081081;
                color += SampleSceneColor(screenUV + float2(-blurOffset.x, -blurOffset.y)) * 0.06081081;
                return color;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                float2 blurOffset = (_BlurSize / _ScreenParams.xy);

                half4 mask = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                half maskAlpha = mask.a * _Color.a;
                half3 blurredScene = SampleBlurredScene(screenUV, blurOffset) * _Color.rgb;

                return half4(blurredScene, maskAlpha);
            }
            ENDHLSL
        }
    }
}

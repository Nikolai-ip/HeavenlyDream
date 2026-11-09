
Shader "Custom/URP/SpriteGaussianBlur"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _BlurSize ("Blur Size (Pixels)", Range(0, 10)) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _BlurSize;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                output.color = input.color * _Color;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 texel = _MainTex_TexelSize.xy * _BlurSize;

                // Gaussian kernel weights: 1, 4, 6, 4, 1
                const float weights[5] =
                {
                    1.0, 4.0, 6.0, 4.0, 1.0
                };

                float3 blurredRGB = 0;
                float blurredAlpha = 0;
                float totalWeight = 0;

                // 5x5 Gaussian kernel
                [unroll]
                for (int y = -2; y <= 2; y++)
                {
                    [unroll]
                    for (int x = -2; x <= 2; x++)
                    {
                        float weight = weights[x + 2] * weights[y + 2];

                        float2 offset = float2(x, y) * texel;

                        half4 sampleColor = SAMPLE_TEXTURE2D(
                            _MainTex,
                            sampler_MainTex,
                            input.uv + offset
                        );

                        // Premultiplied alpha prevents transparent
                        // pixels from creating colored fringes.
                        blurredRGB += sampleColor.rgb * sampleColor.a * weight;
                        blurredAlpha += sampleColor.a * weight;

                        totalWeight += weight;
                    }
                }

                blurredRGB /= max(blurredAlpha, 0.00001);
                blurredAlpha /= totalWeight;

                half4 result;

                result.rgb = blurredRGB * input.color.rgb;
                result.a = blurredAlpha * input.color.a;

                return result;
            }

            ENDHLSL
        }
    }
}

Shader "Custom/Dissolve"
{
    Properties {
        _MainTex("Main Texture", 2D) = "white" {}
        _NoiseMap("Fill Map", 2D) = "white" {}
        _LightTexture("Light Texture", 2D) = "white" {}
 
        _Fill("Fill amount", Range(-1, 1)) = 1
        _LigthSize("Light Size", Range(0.0, 1)) = 0.
        _LightInt("Light Intensity", Range(0.0, 5)) = 0
        _GlowDist("Glow Distance", float) = 0

        _LightColor("LightColor Color", Color) = (1,1,1,1) 
        _GlowColor("Glow Color", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader {
        Tags { "RenderType"="Trasparent" }

        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
        }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag Alpha

            #include "UnityCG.cginc"

            sampler2D _NoiseMap, _MainTex;
            float4 _GlowColor, _MainTex_ST;
            float _GlowDist, _Fill, _LightInt;

            struct vertexData
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct fragInput
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fragInput vert(vertexData i)
            {
                fragInput o;
                o.vertex = UnityObjectToClipPos(i.position);
                o.uv = TRANSFORM_TEX(i.uv, _MainTex);
                return o;
            }

            fixed4 frag(fragInput IN) : SV_Target
            {
                float4 o = float4(0, 0, 0, 0);
                float compare = _Fill + _GlowDist - tex2D(_NoiseMap, IN.uv).r;

                clip(compare);
                clip(tex2D(_MainTex, IN.uv).a -0.1f);
                o = _GlowColor;
                o.a = tex2D(_NoiseMap, IN.uv).r / _LightInt;

                return o;
            }

            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma fragment frag
            #pragma vertex vert

            #include "UnityCG.cginc"

            sampler2D _MainTex, _NoiseMap, _LightTexture;
            float _Fill, _LigthSize, _LightInt;
            float4 _LightColor, _MainTex_ST;
 
            struct vertexData
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
            };
            struct Input {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
 
            Input vert(vertexData i)
            {
                Input o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = TRANSFORM_TEX(i.uv, _MainTex);
                return o;
            }

            fixed4 frag (Input IN) : SV_Target
            {
                float4 o = tex2D(_MainTex, IN.uv);
                float4 e = float4(0, 0, 0, 0);
                float compare = _Fill - tex2D(_NoiseMap, IN.uv).r;

                clip(compare);

                if(_Fill < 1 && compare < _LigthSize)
                {
                    e = tex2D(_LightTexture, compare/_LigthSize) * _LightInt * _LightColor;
                }
                o.rgb += (_LightColor * (1 - _Fill)) / 2;
                o.rgb += e.rgb;

                return o;
            }
            ENDCG
        }
    }
}

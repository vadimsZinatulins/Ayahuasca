Shader "Custom/OverrideStencil"
{
    Properties
    {
        _StencilRef ("Stencil Reference Value", Range(0, 255)) = 0
    }
    SubShader
    {
        Blend Zero One
        ZWrite Off
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1" }

        Stencil {
            Ref [_StencilRef]
            Comp Always
            Pass Replace
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return 0;
            }
            ENDCG
        }
    }
}

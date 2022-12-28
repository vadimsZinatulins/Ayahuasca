Shader "Custom/StencilGeom"
{
    Properties
    {
        _RenderTextureCamOne ("Render Texture Camera One", 2D) = "white" {}
        _RenderTextureCamTwo ("Render Texture Camera Two", 2D) = "white" {}
        _CutDirectionX ("Cut Direction X", Float) = 0
        _CutDirectionY ("Cut Direction Y", Float) = 0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalRenderPipeline"
            "Queue" = "Geometry-1"
        }

        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _RenderTextureCamOne;
            sampler2D _RenderTextureCamTwo;

            float _CutDirectionX;
            float _CutDirectionY;
            float4 _RenderTextureCamOne_ST;

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _RenderTextureCamOne);

                return o;
            }

            float magnitude(float2 vec) 
            {
                return sqrt(vec.x * vec.x + vec.y * vec.y);
            }

            float2 normalize(float2 vec)
            {
                return vec / length(vec);
            }

            float distance(float2 a, float2 b)
            {
                return length(b * dot(a, b) / dot(b, b));
            }

            fixed4 frag(v2f i) : SV_TARGET 
            {
                float2 fragCoord = (i.vertex.xy/_ScreenParams.xy) - 0.5;

                float2 fragDirection = normalize(fragCoord);
                float2 direction = normalize(float2(_CutDirectionX, _CutDirectionY));
                
                if(distance(fragCoord, direction) < 0.005)
                {
                    return float4(0, 0, 0, 1);
                } 

                if(dot(direction, fragDirection) < 0) 
                {
                    return tex2D(_RenderTextureCamTwo, i.uv);
                } 

                return tex2D(_RenderTextureCamOne, i.uv);
            }

            ENDCG 
        }
    }
}

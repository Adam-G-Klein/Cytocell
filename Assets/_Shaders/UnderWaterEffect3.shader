
Shader "Beset/UnderWaterEffect3"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Size("Size", float) = 8 
        _Speed("Speed", float) = 1
        _Color("MainColor", Color) = (1,1,1,1)
        _BackColor("BackColor", Color) = (1,1,1,1)
        _initTime("initTime", float) = 5 
        [PerRendererData] _colorDimmer("colorDimmer", float) = 1
        _noise1("noise1", float) = 1
        _noise2("noise2", float) = 0.2500
        _noise3("noise3", float) = 0.0625
        // total: 1.3125
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Opaque" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off //don't render to depth buffer
        Blend SrcAlpha OneMinusSrcAlpha //traditional transparency

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag addshadow keepalpha
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };
            
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_INITIALIZE_OUTPUT(v2f,OUT);
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            sampler2D _MainTex;
            float _Size;
            float _Speed;
            float4 _Color;
            float _initTime;
            float4 _BackColor;
            float _colorDimmer;
            float _noise1;
            float _noise2;
            float _noise3;
            
            float2 hash22(float2 p)
            {
                p = float2 (
                    dot(p, float2(127.1, 311.7)),
                    dot(p, float2(269.5, 183.3))
                );
                return -1.0 + 2.0 * frac(sin(p) * 43758.5453123) ;
            }

            float wnoise(float2 p, float time)
            {
                float2 pi = floor(p);
                float2 pf = p - pi;
                float minDistance = 5.0 ;
                
                for(int i = -1; i <= 1; i++)
                {
                    for(int j = -1; j <=1; j++)
                    {
                        float2 offset = float2(i, j);
                        float2 rand = hash22(pi + offset);
                        float2 move = 0.5 + 0.5 * sin(rand * time);
                        move = pi + offset + move - p;
                        float distance = dot(move, move) * 1.0;
                        if(distance < minDistance)
                        {
                            minDistance = distance;
                        }
                    }
                }
                return minDistance;
            }

            float noise_sum(float2 p)
            {
                float f = 0.0;
                float time = _Time + _initTime;
                float spd = _Speed * time;
                p = p / 2.0;
                f += _noise1 * wnoise(p, spd); p = 2.0 * p;
                //f += 0.5000 * wnoise(p, _Time); 
                p = 2.0 * p;
                f += _noise2 * wnoise(p, spd); p = 2.0 * p;
                //f += 0.1250 * wnoise(p, _Time); 
                p = 2.0 * p;
                f += _noise3 * wnoise(p, spd); p = 2.0 * p;
                /* good noise vals:
                0.5
                6.1
                0.0625
                total: 6.6625
                0.075
                0.9155
                0.009
                */

            return f;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                
                float2 uv = IN.texcoord * _Size;
                float val = noise_sum(uv);
                float invVal = saturate(1.0 - val);
                float4 c = _Color * float4(val, val, val, 1.0) + _BackColor * float4(invVal, invVal, invVal, 1.0);
                float4 dimmed = float4(c.rgb * _colorDimmer, 1);
                return dimmed;
            }
        ENDCG
        }
    }
}

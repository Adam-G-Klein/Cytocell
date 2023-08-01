Shader "Beset/SwipeMarker"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _radius ("test radius", float) = 0.5
        _centerLoc ("Nucleus Location",Vector ) = (0.5,0.5,0,0)

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
            
            float _radius; 
            Vector _centerLoc;
            float _epicenterDist;

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

            fixed4 frag(v2f IN) : SV_Target
            {

                fixed4 c = tex2D(_MainTex, IN.texcoord);

                _epicenterDist = distance(IN.texcoord, _centerLoc.xy);
                c.a = IN.texcoord.x > IN.texcoord.y && _epicenterDist < _radius ? 1 : 0;
                return c;
            }
        ENDCG
        }
    }
}

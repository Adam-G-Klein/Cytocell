Shader "Beset/Arrow"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "yellow" {}
        _mainColor ("Main Color", Color) = (1,1,1,1)
        //x,y,unused,unused
        _circle1 ("Circle1", Vector) = (1,1,0.5,0)
        //x,y,unused,unused
        _circle2 ("Circle2", Vector) = (1,1,0.5,0)
        _sideCirclesRadius("Side Circles Radius", Float) = 0.5
        _baseCircle("Base Circle", Vector) = (0.5,-1,1.25,0)
        _pulseSpeed("Pulse Speed", Float) = 1
        _bandWidth("Band Width", Float) = 0.1
        _visible("Visible", Int) = 1
        //Good values:
        //circle1: (-1,1,1.5,0)
        //circle2: (2,1,1.5,0)
        //tween one z value to 1.65 to make the arrow squat
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
            float4 _circle1;
            float4 _circle2;
            float4 _baseCircle;
            float _pulseSpeed;
            float _bandWidth;
            fixed4 _mainColor;
            int _visible;
            float _sideCirclesRadius;


            float distFromCircle(float2 pos, float4 circle)
            {
                return distance(pos, circle.xy);
            }

            float distFromSideCircles(float2 pos)
            {
                float dist1 = distFromCircle(pos, _circle1);
                float dist2 = distFromCircle(pos, _circle2);
                return min(dist1, dist2);
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord);
                //gradient with sin time
                float sideDist = distFromSideCircles(IN.texcoord);
                // distance from either circle, only need to tween one z value this way
                if (sideDist < _sideCirclesRadius)
                {
                    c.a = 0;
                } else {
                    c.a = 1;
                }

                // distance from base circle
                if (distFromCircle(IN.texcoord, _baseCircle) < _baseCircle.z)
                {
                    c.a = 0;
                }
                // pulse the arrow with the gradient waves
                c.a *= sin((IN.texcoord.y + (frac(_Time) * _pulseSpeed)) / (_bandWidth * 3.14159)) + 1;
                c *= _mainColor;
                c.a = _visible == 1 ? c.a : 0;
                return c;
            }
        ENDCG
        }
    }
}

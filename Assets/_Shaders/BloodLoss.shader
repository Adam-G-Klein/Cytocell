Shader "uglyVamp/bloodLoss"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_AspectRatio("Aspect Ratio" , Vector) = (640, 480,0,0)
		_LossColor("BloodLossColor", Color) = (0,0,0,1)
		_LossAmnt("LossAmount", float) = 0
		_maxAlpha("maxAlpha", float) = 0

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #define M_PI 3.141592653589

            uniform float _NoiseFrequency, _NoiseScale, _NoiseSpeed, _PixelOffset;
            sampler2D _CameraDepthTexture;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 scrPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.scrPos = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }
			fixed4 _LossColor;
			float _LossAmnt;
			float _maxAlpha;

            sampler2D _MainTex;
            float _DepthDistance, _DepthStart;

            fixed4 frag (v2f i) : COLOR
            {
/*                float depthValue = Linear01Depth(
                    tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.scrPos)).r)
                    * _ProjectionParams.z;
                depthValue = saturate((depthValue - _DepthStart)/_DepthDistance);*/

				/*
                float3 spos = float3(i.scrPos.x, i.scrPos.y, 0) * _NoiseFrequency;
                spos.z += _Time.x * _NoiseSpeed;
                float noise = _NoiseScale * ((snoise(spos) + 1) / 2);
                float4 noiseToDirection = float4(cos(noise*M_PI*2), sin(noise*M_PI*2),0,0);
				*/
				/*
                fixed4 lossColor = _LossColor * depthValue;
                fixed4 col = tex2Dproj(_MainTex, i.scrPos);
                return lerp(col,lossColor,depthValue);
				*/




				float distFromCenter = pow((pow((i.uv.x - 0.5f), 2) + pow((i.uv.y - 0.5f), 2)), 0.5f);
                fixed4 lossColor = _LossColor;
                fixed4 col = tex2Dproj(_MainTex, i.scrPos);
				float lerpVal = clamp(_LossAmnt * distFromCenter, 0, _maxAlpha);
                fixed4 lerped = lerp(col,lossColor, lerpVal);
				return lerped;

            }
            ENDCG
        }
    }
}


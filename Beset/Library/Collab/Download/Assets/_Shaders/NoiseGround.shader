Shader "PeerPlay/NoiseGround"
{
    Properties
    {
        _Tess ("Tessellation", Range(1,8)) = 4
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _NoiseScale ("Noise Scale" ,float) = 1
        _NoiseFrequency ("Noise Frequency" , float) = 1
        _NoiseOffset("Noise Offset", Vector) = (0,0,0,0)
        _whiteScale("white scale", float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows tessellate:tess vertex:vert

        #pragma target 4.6

        #include "noiseSimplex.cginc"

        struct appdata{
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 tangent : TANGENT;
            float2 texcoord : TEXCOORD0;
        };

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        float _Tess;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _whiteScale;

        float _NoiseScale, _NoiseFrequency;
        float4 _NoiseOffset;
        
        float4 tess(){
            return _Tess;
        }

        void vert(inout appdata v){
            float3 v0 = v.vertex.xyz;
            float3 bitangent = cross(v.normal, v.tangent.xyz);
            float3 v1 = v0 + (v.tangent.xyz * 0.01); 
            float3 v2 = v0 + (bitangent * 0.01); 

            float ns0 = _NoiseScale * snoise(
                                float3(v0.x + _NoiseOffset.x, 
                                       v0.y + _NoiseOffset.y,
                                       v0.z + _NoiseOffset.z) 
                                    * _NoiseFrequency);
            v0.xyz += (ns0 + 1)/2 * v.normal;
            float ns1 = _NoiseScale * snoise(
                                float3(v1.x + _NoiseOffset.x, 
                                       v1.y + _NoiseOffset.y,
                                       v1.z + _NoiseOffset.z) 
                                    * _NoiseFrequency);
            v1.xyz += (ns1 + 1)/2 * v.normal;
            float ns2 = _NoiseScale * snoise(
                                float3(v2.x + _NoiseOffset.x, 
                                       v2.y + _NoiseOffset.y,
                                       v2.z + _NoiseOffset.z) 
                                    * _NoiseFrequency);
            v2.xyz += (ns2 + 1)/2 * v.normal;

            float3 vn = cross(v2-v0, v1-v0);

            v.normal = normalize(-vn);
            v.vertex.xyz = v0;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            float r = (c.r + c.g + c.b)/3;
            float rShift = abs(r - .5) * _whiteScale;
            r = r <= .5 ? r - rShift : r + rShift;

            float g = (c.r + c.g + c.b)/3;
            float gShift = abs(g - .5) * _whiteScale;
            g = g <= .5 ? g - gShift : g + gShift;

            float b = (c.r + c.g + c.b)/3;
            float bShift = abs(b - .5) * _whiteScale;
            b = b <= .5 ? b - bShift : b + bShift;

            c = fixed4(r,g,b,
                        1);

            o.Albedo = c.rgb;
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}

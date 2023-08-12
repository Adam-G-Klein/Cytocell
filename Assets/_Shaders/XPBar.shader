  Shader "Beset/XPBar"
  {
      Properties
      {
         [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
          [Header(GrowingXP)]_growingXPColor ("Growing XP Color", Color) = (0.2,1,0.2,1)
          [Header(Bar Size)]_barSize ("Bar Size", float) = 0.2
          _Steps ("Steps", Float) = 1
          _growingXPpercent ("Growing XP Percent", Float) = 1
          _MinAlpha ("Min Alpha", float) = 0.1
          _publicAlpha ("Min Alpha", float) = 0
          _xpGradient ("XP Gradient", float) = 1
          _xpSolid ("XP Solid", float) = 1
          _barXScale("Bar X Scale", float) = 1.1
          
      
          [Header(FutureXP)]_realXPColor("real XP Color", Color) = (1,1,0,1)
          _realXPpercent ("real XP Percent", Float) = 1
      
      
          //for debugging
          [Header(DebugColor)]_weirdColor ("Weird Color", Color) = (1,0,0,1)
          [Header(emptyBarColor)]_emptyBarColor ("Empty Bar Color", Color) = (1,0,0,1)

          [Header(BorderColor)]_BorderColor ("Border color", Color) = (0.1,0.1,0.1,1)
          _BorderWidth ("Border width", Float) = 1
          [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
      
      
          _ImageSize ("Image Size", Vector) = (100, 100, 0, 0)
          _ImageCenter ("Image Size", Vector) = (0.5, 0.5, 0, 0)
          _ColorMask ("Color Mask", Float) = 15
      }
  
      SubShader
      {
          Tags
          { 
              "Queue"="Transparent"
              "IgnoreProjector"="True"
              "RenderType"="Transparent"
              "PreviewType"="Plane"
              "CanUseSpriteAtlas"="True"
          }
  
          Cull Off
          Lighting Off
          ZWrite Off
          Blend SrcAlpha OneMinusSrcAlpha
          ZTest [unity_GUIZTestMode]
          ColorMask [_ColorMask]
  
          Pass
          {
          CGPROGRAM
              #pragma vertex vert
              #pragma fragment frag
              #pragma target 2.0

              #include "UnityCG.cginc"
              #include "UnityUI.cginc"

              #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
              #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
              
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
              
              fixed4 _growingXPColor;
              half _Steps;
              half _growingXPpercent;
              half _realXPpercent;
              
              fixed4 _realXPColor;
              half _futureXPpercent;
              
              fixed4 _weirdColor;
              fixed4 _emptyBarColor;

              fixed4 _BorderColor;
              half _BorderWidth;
              half _imageMid;
              half _imageRL;
  
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
              float4 _ImageSize;
              float4 _ImageCenter;
              float _xpGradient;
              float _xpSolid;
              float _MinAlpha;
              float _barXScale;
              float _publicAlpha;
  
              fixed4 frag(v2f IN) : SV_Target
              {
                  fixed4 c = tex2D(_MainTex, IN.texcoord);
                  fixed4 coordsum = IN.texcoord.y+IN.texcoord.x;
                  
                  //if the pixel is further right than growing xp
                  //and future xp, color it like the border
                  //If I want to change the color of the empty bar, 
                  //change the color here
                  /*if ( IN.texcoord.x > _growingXPpercent + _futureXPpercent )
                  {
                     c*= _weirdColor;
                  }*/
                  //if the pixel is less than the border width, and it wasn't
                  //within the areas for growing or future xp, color it like a border
                  //the left border
                  /*
                  if( (IN.texcoord.x * _ImageSize.x ) < _BorderWidth )
                    c *= _BorderColor;
                  //the right border
                  else if ( (IN.texcoord.y * _ImageSize.y) < _BorderWidth)
                    c *= _BorderColor;
                  //the upper border
                  else if ((IN.texcoord.y * _ImageSize.y) > _ImageSize.y - _BorderWidth )
                    c *= _BorderColor;
                  //the lower border
                  else if ((IN.texcoord.x * _ImageSize.x) > _ImageSize.x - _BorderWidth )
                    c *= _BorderColor;
                    */
                      //from all the other cases, we know this pixel is inside the bar
                  /*if ( IN.texcoord.x < _realXPpercent && IN.texcoord.x > _growingXPpercent)
                      c *= ;
                  else if (IN.texcoord.x < _growingXPpercent)
                  {
                          c *= _growingXPColor;

                  }
                  //here, pixel is not in future or growing regions
                  else
                  {
                    c *= _emptyBarColor;


                  }*/
                  fixed4 scaledLoc = fixed4(IN.texcoord.x ,IN.texcoord.y,0,0);
                  float centerDist = distance(_ImageCenter,scaledLoc);
                  if(centerDist > _xpSolid){
                    c*= _BorderColor;

                    c.a = 1 / (centerDist - _xpSolid)*_xpGradient;

                    c.a = c.a < _MinAlpha ? 0 : c.a;
                    c.a = c.a > 1 ? 1 : c.a;
                  }else if(IN.texcoord.x < _realXPpercent && IN.texcoord.x > _growingXPpercent){
                    c *= _realXPColor;
                  }else if(IN.texcoord.x < _growingXPpercent){
                    c *= _growingXPColor;

                  }else
                  {
                    c *= _emptyBarColor;
                  }
                  c.a *= _publicAlpha;


                  return c;
              }
          ENDCG
          }
      }
  }
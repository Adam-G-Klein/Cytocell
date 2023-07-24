Shader "Beset/JellyAlpha"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _cellSolid ("Cell Solidness", float) = 0.5
        _minAlpha ("Min Alpha", float) = 0.1
        _membraneThickness ("Membrane Thickness", float)  = 0.2
        _membraneGradient ("Membrane Gradient", float) = 0.5
        _organNucDist ("Organ NucDist ", float)  = 10
        _organGradient ("Organ Gradient", float) = 0.5
        [PerRendererData] _nucleusLocation ("Nucleus Location",Vector ) = (0.5,0.5,0,0)
        _nucleusRadius ("Nucleus Radius", float) = 0.2
        _nucleusGradient ("Nucleus Gradient", float) = 0.5
        _numNuclei ("Number of Nuclei", int) = 5
//        _nucleiPos("Nuclei Locations", Vector[]) = {}

        _testAdd ("test add", float) = 0.5
        _jellyBottom ("jelly Bottom", float) = 0.5
        _epicenterSize ("epicenter Size", float) = 0.1
        _bottomMembraneGradient("bottom membrane gradient", float) = 40
       // _nucleiEpicenterLocation ("Nucleus Epicenter Location", Vector) = (0.5,0.5,0,0)
        _nucleiEpicenterDistance("Nucleus Epicenter Distance", float) = 0.3

        _membraneColor ("Membrane Color", Color) = (1,0,0,1)
        _nucleusColor ("Nucleus Color", Color) = (1,0,0,1)
        _cytoplasmColor ("Cytoplasm Color", Color) = (1,0,0,0.5)
        _organColor ("Organ Color", Color) = (1,0,0,0.5)


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
            
            fixed4 _membraneColor;
            fixed4 _nucleusColor;
            fixed4 _cytoplasmColor;
            fixed4 _organColor;
            float _cellSolid;
            float _minAlpha;
            Vector _nucleusLocation;
            float _membraneThickness;
            float _membraneGradient;
            float _bottomMembraneGradient;
            float _nucleusGradient;
            float _nucleusRadius;
            Vector _nucleiEpicenterLocation;
            float _nucleiEpicenterDistance;
            int _numNuclei;
            float _minNucleiAngle;
            float _maxNucleiAngle;
            Vector _nucleiPos[20];
            float _nucleiAngles[20];
            float PI = 3.1415926535897932384626433832795028841971693993751;
            float _organNucDist;
            float _organGradient;
            float _epicenterDist;
            float _epicenterSize;
            float _testAdd;
            float _jellyBottom;

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
                float _centerAng;
                float _nucleusDist;
                float _nucleusAng;
                float _angDist;
                //the layer of the cell the color and alpha currently represents
                //the highest layer is nucleus, followed by membrane, and then cytoplasm
                int _currLayer = 0;
                float _nucLayerDist = 1.1;
                float _angleLayerDist = 360;
                //for membrane:
                //the lowest distance is what the alpha needs to be based on
                //if we'd end up dividing by 0
                float xDiff = IN.texcoord.x - _nucleiEpicenterLocation.x;
                float yDiff = IN.texcoord.y - _nucleiEpicenterLocation.y;

                c = _membraneColor;
                if(xDiff <= 0){
                    _nucleusAng = atan(yDiff/xDiff);
                    _nucleusAng += _testAdd;
                }else{
                    _nucleusAng = atan(yDiff/xDiff);
                }

                _epicenterDist = distance(IN.texcoord, _nucleiEpicenterLocation.xy);
                //organ and nucleus coloring
                if(yDiff >= _jellyBottom){
                    //only do organ or nucleus coloring if above epicenter
                    for(int i = 0; i < _numNuclei; i++){
                        
                        _angDist = abs(_nucleusAng - _nucleiAngles[i]);
                        
                        _nucleusDist = distance(IN.texcoord, _nucleiPos[i].xyz);
                        if(_nucleusDist < _nucleusRadius && _currLayer <= 3 && _nucleusDist < _nucLayerDist){
                            //always doing nucleus first
                            c = _nucleusColor;
                            c.a = 1 / (_nucleusGradient * _nucleusDist);
                            _nucLayerDist = _nucleusDist;
                            _currLayer = 3;
                        }else if(_epicenterDist > _membraneThickness && _currLayer <= 2){
                            //override all organ colors if a part of membrane
                            c = _membraneColor;
                            c.a = (_membraneGradient * (_epicenterDist - _membraneThickness));

                        }
                        else if(_angDist >= _organNucDist && _angDist <= _angleLayerDist && _currLayer <= 1){
                            c = _organColor;
                            c.a = (_organGradient * _angDist);
                            _angleLayerDist = _angDist;
                            _currLayer = 1;
                        }
                        
                    }
                }
                else{
                    c.a = 0;
                }
                
                return c;
            }
        ENDCG
        }
    }
}

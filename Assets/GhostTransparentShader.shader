Shader "GhostTransparentShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,0.5)
        _PulseSpeed ("Pulse Speed", Float) = 0.1
        _Delay ("Reappear Delay", Float) = 5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _Color;
            float _PulseSpeed;
            float _StartTime;
            float _Delay;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float elapsed = _Time.y - _StartTime;
                if (elapsed < _Delay) return float4(0, 0, 0, 0);
                float pulse = abs(sin((_Time.y - _StartTime - _Delay) * _PulseSpeed));
                return fixed4(_Color.rgb, _Color.a * pulse);
            }
            ENDCG
        }
    }
}
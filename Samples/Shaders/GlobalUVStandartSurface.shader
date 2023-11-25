Shader "Custom/GlobalUVStandardSurface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
//        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
//        ZWrite Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 worldNormal;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = float2(0,0);
            float absY=abs(IN.worldNormal.y);
            if (absY > 0.5)
            {
                uv = float2(IN.worldPos.x, IN.worldPos.z);
            }else
            {
                float absX=abs(IN.worldNormal.x);
                if (absX > 0.5)
                {
                    uv = float2(IN.worldPos.y, IN.worldPos.z);
                }else
                {
                    uv = float2(IN.worldPos.x, IN.worldPos.y);
                }
            }
            
            fixed4 c = tex2D(_MainTex, uv);
            o.Albedo = c.rgb * _Color.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = _Color.a;
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}

Shader "PostProcess/HeightFog"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _FogColor ("FogColor", Color) = (1,1,1,1)
        _FogDensity ("FogDensity", Range(0,1)) = 0.2
        _FogHeight ("FogHeight", float) = 5.0
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;

            float4x4 _Matrix_Inv_VP;

            float4 _FogColor;
            float _FogDensity;
            float _FogHeight;

            // https://github.com/hiryma/UnitySamples/blob/master/Fog/Assets/Shaders/Fog.cginc
            float calcFogHeightUniform(float3 objectPosition, float3 cameraPosition, float fogDensity, float fogEndHeight)
            {
	            float3 camToObj = cameraPosition - objectPosition;
	            float t;
	            if (objectPosition.y < fogEndHeight) // 物が霧の中にある
	            {
		            if (cameraPosition.y > fogEndHeight) // カメラは霧の外にある
		            {
			            t = (fogEndHeight - objectPosition.y) / camToObj.y;
		            }
		            else // カメラも霧の中にある
		            {
			            t = 1.0;
		            }
	            }
	            else // 物が霧の外にいる
	            {
		            if (cameraPosition.y < fogEndHeight) // カメラは霧の中にいる
		            {
			            t = (cameraPosition.y - fogEndHeight) / camToObj.y;
		            }
		            else // カメラも霧の外にいる
		            {
			            t = 0.0;
		            }
	            }
	            float distance = length(camToObj) * t;
	            float fog = exp(-distance * fogDensity);
	            return fog;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 mainColor = tex2D(_MainTex, i.uv);
                // depth texutre から world position を復元する
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                float4 vpos = float4(i.uv * 2.0 - 1.0, depth, 1) * _ProjectionParams.z;
                vpos.y *= -1.0;
                float4 wpos = mul(_Matrix_Inv_VP, vpos);
                wpos /= wpos.w;

                float fog = calcFogHeightUniform(wpos.xyz, _WorldSpaceCameraPos, _FogDensity, _FogHeight);
                return lerp(_FogColor, mainColor, fog);
            }
            ENDCG
        }
    }
}

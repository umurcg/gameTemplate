Shader "ExactColorShader/Toon"
{
    Properties
    {
        _ColorLight ("Color Light", Color) = (1,1,1,1)
        _ColorDark ("Color Dark", Color) = (1,1,1,1)

        [Space]
        _ShadowLight("Shadow Light Color", Color) = (1,1,1,1)
        _ShadowDark("Shadow Dark Color", Color) = (1,1,1,1)

        [Space]
        _RampThreshold ("Ramp Threshold", Range(0,1)) = 0.75
        _RampSmooth ("Ramp Smoothness", Range(0, 2)) = 1

        [Space][Space][Space]
        [HDR]
        _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,0)
        _SpecularAmount("Specular Amount", Range(0, 1)) = 0.1
        _SpecularHardness("Specular Hardness", Range(0, 1)) = 1
        _SpecularVisibleInShadow("Specular Visible In Shadow", Range(0, 1)) = 1

        [Space][Space][Space]
        [HDR]
        _RimColor("Rim Color", Color) = (1,1,1,0)
        _RimAmount("Rim Amount", Range(0, 1)) = 250
        _RimHardness("Rim Hardness", Range(0, 1)) = 1
        _RimInShadow("Rim In Shadow", Range(0, 1)) = 1

        
        [Space][Space][Space]
        [MaterialToggle] _UseUnityFog ("Unity Fog", Float) = 0
    }
    SubShader
    {
        Tags
        { 
            "RenderType" = "Opaque"
        }
        LOD 100

        Pass
        {
            Tags
            {
                "LightMode" = "ForwardBase"
                "PassFlags" = "OnlyDirectional"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD1;
                SHADOW_COORDS(2)
                UNITY_FOG_COORDS(3)
            };

            half4 _ColorLight;
            half4 _ColorDark;
            half _RampSmooth;
            half _RampThreshold;
            half4 _SpecularColor;
            half _SpecularAmount;
            half _SpecularHardness;
            half _SpecularVisibleInShadow;
            half4 _RimColor;
            half _RimAmount;
            half _RimHardness;
            half _RimInShadow;
            half4 _ShadowLight;
            half4 _ShadowDark;
            half _UseUnityFog;

            float remap(float inputValue, float rangeInputMin, float rangeInputMax, float rangeOutputMin, float rangeOutputMax)
            {
                return ((inputValue - rangeInputMin) / (rangeInputMax - rangeInputMin)) * (rangeOutputMax - rangeOutputMin) + rangeOutputMin;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                TRANSFER_SHADOW(o);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half3 viewDir = normalize(i.viewDir);
                half3 normal = normalize(i.worldNormal);
                half NdotL = dot(_WorldSpaceLightPos0, normal);
                half threshold01 = remap(_RampThreshold, 0, 1, -1, 1);
                half NdotLRamp = saturate(smoothstep(threshold01 - _RampSmooth * 0.5, threshold01 + _RampSmooth * 0.5, NdotL));
                half4 baseColor = lerp(_ColorDark, _ColorLight, NdotLRamp);

                //shadow
                half shadow = SHADOW_ATTENUATION(i);
                half shadowAmount = saturate(shadow + 1 - (_ShadowLight.a * _ShadowDark.a));
                half4 shadowColorResult = lerp(_ShadowDark, _ShadowLight, NdotLRamp);
                half4 baseColorPlusShadow = lerp(shadowColorResult, baseColor, shadowAmount);

                half4 result = baseColorPlusShadow;

                //specular
                if (_SpecularColor.a > 0 && _SpecularAmount > 0)
                {
                    half3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                    half NdotH = dot(normal, halfVector);
                    half specularHardness = smoothstep((1 - _SpecularAmount) - 0, (1 - _SpecularAmount) + ((1 - _SpecularHardness) * _SpecularAmount), NdotH);

                    // color and add specular
                    result = lerp(result, _SpecularColor, specularHardness * _SpecularColor.a * saturate(shadowAmount + _SpecularVisibleInShadow));
                }

                //rim light
                if (_RimColor.a > 0 && _RimAmount > 0)
                {
                    half4 rimDot = 1 - dot(viewDir, normal);
                    half4 rimHardness = smoothstep((1 - _RimAmount) - (1 - _RimHardness) * 0.5, (1 - _RimAmount) + (1 - _RimHardness) * 0.5, rimDot);
                    half4 rimWithShadow = rimHardness * saturate(shadowAmount + _RimInShadow) /* * shadowAmount */;
                    result = lerp(result, _RimColor, rimWithShadow * _RimColor.a);
                }

                if (_UseUnityFog)
                {
                    UNITY_APPLY_FOG(i.fogCoord, result);
                }
                return result;
            }
            ENDCG
        }

        // shadow caster rendering pass
        Pass
        {
            Tags {"LightMode"="ShadowCaster"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f 
            { 
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}

Shader "JGH/FogWallShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Spd("Spd",Float) = 1
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows

            #pragma target 3.0

            sampler2D _MainTex;

            struct Input
            {
                float2 uv_MainTex;
            };

            fixed4 _Color;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_INSTANCING_BUFFER_END(Props)

                float _Spd;

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                fixed4 c = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y + (_Time.y * _Spd))) * _Color;
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
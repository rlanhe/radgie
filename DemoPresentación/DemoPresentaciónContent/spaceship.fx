/*

% Specular Multiple Colored Lights

*/

#include "Macros.fxh"

// Light data
struct Light 
{
    float4 color : LightColor;
    float3 position : LightPosition;
    float falloff : LightFallOff;
    float range : LightRange;
};
struct VertexShaderOutput
{
     float4 Position : POSITION;
     float2 TexCoords : TEXCOORD0;
     float3 WorldNormal : TEXCOORD1;
     float3 WorldPosition : TEXCOORD2;
	 float3 DiffuseDir0 : TEXCOORD3;
	 float3 DiffuseDir1 : TEXCOORD4;
	 float3 DiffuseDir2 : TEXCOORD5;
};
struct PixelShaderInput
{
     float2 TexCoords : TEXCOORD0;
     float3 WorldNormal : TEXCOORD1;
     float3 WorldPosition : TEXCOORD2;
	 float3 DiffuseDir0 : TEXCOORD3;
	 float3 DiffuseDir1 : TEXCOORD4;
	 float3 DiffuseDir2 : TEXCOORD5;
};


shared float4x4 view : View;
shared float4x4 projection : Projection;
shared float3 cameraPosition : EyePosition;
shared float4 ambientLightColor : AmbientColor;
shared float numLights : NumberOfLights = 1;

sampler diffuseSampler;
sampler specularSampler;

//texture parameters can be used to set states in the 
//effect state pass code
texture2D diffuseTexture : Texture0;
texture2D specularTexture : SpecularTexture;
//float4 modelDiffuseColor : DiffuseColor;

//the world paramter is not shared because it will
//change on every Draw() call
float4x4 world : World;

// Material data
float4 materialColor : EmissiveColor = float4(1.0f,1.0f,1.0f,1.0f);
float specularPower : SpecularPower = 45;
float specularIntensity : SpecularIntensity = 1.0;
bool diffuseTexEnabled : DiffuseTextureEnabled = false;
bool specularTexEnabled : SpecularTextureEnabled = false;
bool lightsEnabled : LightsEnabled = false;
float textureUReps = 1.0;
float textureVReps = 1.0;
bool diffuseStaticsLightsEnabled : DiffuseLigthsEnabled = false;
float4 diffuseColor0 : DiffuseColor0 = float4(1.0f,1.0f,1.0f,1.0f);
float3 diffuseDir0 : DiffuseDir0 = float3(0.0, 0.0, -1.0);
float diffuseIntensity0 : DiffuseIntensity0 = 0.5;
float4 diffuseColor1 : DiffuseColor1 = float4(1.0f,1.0f,1.0f,1.0f);
float3 diffuseDir1 : DiffuseDir1 = float3(-1.0, 0.0, 0.0);
float diffuseIntensity1 : DiffuseIntensity1 = 0.5;
float4 diffuseColor2 : DiffuseColor2 = float4(1.0f,1.0f,1.0f,1.0f);
float3 diffuseDir2 : DiffuseDir2 = float3(0.0, 0.0, 1.0);
float diffuseIntensity2 : DiffuseIntensity2 = 0.5;
float alpha : Alpha = 0.5f;

float time : Time;

VertexShaderOutput BasicVS(
     float3 position : POSITION,
     float3 normal : NORMAL,
     float2 texCoord : TEXCOORD0 )
{
     VertexShaderOutput output = (VertexShaderOutput)0;

     //generate the world-view-projection matrix
     float4x4 wvp = mul(mul(world, view), projection);
     
     //transform the input position to the output
     output.Position = mul(float4(position, 1.0), wvp);

	 output.WorldNormal =  mul(normal, world);
	 output.WorldNormal = normalize(output.WorldNormal);
     float4 worldPosition =  mul(float4(position, 1.0), world);
     output.WorldPosition = worldPosition / worldPosition.w;
     
     //copy the tex coords to the interpolator
     output.TexCoords.x = texCoord.x * textureUReps;
     output.TexCoords.y = texCoord.y * textureVReps;

	 if(diffuseStaticsLightsEnabled)
	 {
		 output.DiffuseDir0 = normalize(diffuseDir0);
		 output.DiffuseDir1 = normalize(diffuseDir1);
		 output.DiffuseDir2 = normalize(diffuseDir2);
	 }

     //return the output structure
     return output;
}

float4 CalculateSingleLight(Light light, float3 worldPosition, float3 worldNormal, 
                            float4 diffuseColor, float4 specularColor )
{
     float3 lightVector = light.position - worldPosition;
     float lightDist = length(lightVector);
     float3 directionToLight = normalize(lightVector);
     
     //calculate the intensity of the light with exponential falloff
     float baseIntensity = pow(saturate((light.range - lightDist) / light.range),
                                 light.falloff);
     
     
     float diffuseIntensity = saturate( dot(directionToLight, worldNormal));
     float4 diffuse = diffuseIntensity * light.color * diffuseColor;

     //calculate Phong components per-pixel
     float3 reflectionVector = normalize(reflect(-directionToLight, worldNormal));
     float3 directionToCamera = normalize(cameraPosition - worldPosition);
     
     //calculate specular component
     float4 specular = saturate(light.color * specularColor * specularIntensity * 
                       pow(saturate(dot(reflectionVector, directionToCamera)), 
                           specularPower));
                           
     return  baseIntensity * (diffuse + specular);
}

float4 CalculateStaticDiffuseLight(float4 color, float3 dir, float3 worldNormal, float intensity)
{
    return saturate( dot(dir, worldNormal)) * intensity * color;
}

shared Light lights[8] : Lights;

float4 MultipleLightPS(PixelShaderInput input) : COLOR
{
    float4 diffuseColor = materialColor;
    float4 specularColor = materialColor;
     
	if(diffuseStaticsLightsEnabled)
	{
		diffuseColor += CalculateStaticDiffuseLight(diffuseColor0, input.DiffuseDir0, input.WorldNormal, diffuseIntensity0);
		diffuseColor += CalculateStaticDiffuseLight(diffuseColor1, input.DiffuseDir1, input.WorldNormal, diffuseIntensity1);
		diffuseColor += CalculateStaticDiffuseLight(diffuseColor2, input.DiffuseDir2, input.WorldNormal, diffuseIntensity2);

		if(diffuseTexEnabled)
		{
			diffuseColor *= tex2D(diffuseSampler, input.TexCoords);
		}
	}
	else
	{
		if(diffuseTexEnabled)
		{
			diffuseColor = tex2D(diffuseSampler, input.TexCoords);
		}
	}
	 
    if(specularTexEnabled)
    {
        specularColor *= tex2D(specularSampler, input.TexCoords);
    }
     
    float4 color = ambientLightColor * diffuseColor;
     
    // Each light is added into a final pixel color
	if(lightsEnabled)
	{
		for(int i=0; i< numLights; i++)
		{
			color += CalculateSingleLight(lights[i], 
					  input.WorldPosition, input.WorldNormal,
					  diffuseColor, specularColor );
		}
	}
    color.a = alpha;
    return color;
}

// Glow size
float inflate = 0.01f;
// Glow color
float4 glowColor = float4(0.0f,0.0f,1.0f,1.0f);
// Glow edge
float glowStep = 1.7f;

struct GlowVertexShaderOutput
{
     float4 Position : POSITION;
     float3 WorldNormal : TEXCOORD0;
     float3 WorldView : TEXCOORD1;
};

struct GlowPixelShaderInput
{
	 float4 Position : POSITION;
     float3 WorldNormal : TEXCOORD0;
     float3 WorldView : TEXCOORD1;
};

GlowVertexShaderOutput GlowVS(
     float3 position : POSITION,
     float3 normal : NORMAL)
{
     GlowVertexShaderOutput output;

     //generate the world-view-projection matrix
     float4x4 wvp = mul(mul(world, view), projection);

	 output.WorldNormal =  mul(normal, world);
	 output.WorldNormal = normalize(output.WorldNormal);
     float4 Po = float4(position.xyz,1);
     Po += (inflate*normalize(float4(normal.xyz,0))); // the balloon effect
     float4 Pw = mul(Po,world);
     output.WorldView = normalize(view[3].xyz - Pw.xyz);
     output.Position = mul(Po,wvp);

	 return output;
}

float4 GlowPS(GlowPixelShaderInput input) : COLOR
{
    float edge = 1.0 - dot(input.WorldNormal,input.WorldView);
    edge = pow(edge,glowStep);
    float3 result = edge * glowColor.rgb;
    return float4(result,edge);
}

technique Default
{

    pass P0
    {
        AlphaBlendEnable = FALSE;
        LIGHTING_PASS
    }
}

technique Invincibility
{
	pass P0
	{
		AlphaBlendEnable = FALSE;
		LIGHTING_PASS
	}

	pass P1
	{
		ZEnable = false;  
        ZWriteEnable = false;  
        AlphaBlendEnable = true;  
        SrcBlend = One;  
        DestBlend = One;  
		
		VertexShader = compile vs_3_0 GlowVS();
		PixelShader = compile ps_3_0 GlowPS();
	}
}

technique HitRecieved
{
	pass P0
	{
		AlphaBlendEnable = TRUE;
		LIGHTING_PASS
	}
}
/*

% Asteroid Effect

*/

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
     float3 WorldPosition : TEXCOORD2;
	 float3 DiffuseDir0 : TEXCOORD3;
	 float3 DiffuseDir1 : TEXCOORD4;
	 float3 DiffuseDir2 : TEXCOORD5;
};
struct PixelShaderInput
{
     float2 TexCoords : TEXCOORD0;
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
sampler bumpSampler;

//texture parameters can be used to set states in the 
//effect state pass code
texture2D diffuseTexture : Texture0;
texture2D specularTexture : SpecularTexture;
texture2D bumpTexture : Texture1;

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
float diffuseIntensity1 : DiffuseIntensity1 = 1.0;
float4 diffuseColor2 : DiffuseColor2 = float4(1.0f,1.0f,1.0f,1.0f);
float3 diffuseDir2 : DiffuseDir2 = float3(0.0, 0.0, 1.0);
float diffuseIntensity2 : DiffuseIntensity2 = 0.2;


VertexShaderOutput BasicVS(
     float3 position : POSITION,
     float3 normal : NORMAL,
     float2 texCoord : TEXCOORD0,
	 float3 tangent : TANGENT )
{
     VertexShaderOutput output = (VertexShaderOutput)0;

     //generate the world-view-projection matrix
     float4x4 wvp = mul(mul(world, view), projection);
     
     //transform the input position to the output
     output.Position = mul(float4(position, 1.0), wvp);
	 
	 // calculate tangent space
	 float3x3 worldToTangentSpace;
	 worldToTangentSpace[0] = mul(tangent,world);
	 worldToTangentSpace[1] = mul(cross(tangent,normal),world);
	 worldToTangentSpace[2] = mul(normal,world);
	      
     //copy the tex coords to the interpolator
     output.TexCoords.x = texCoord.x * textureUReps;
     output.TexCoords.y = texCoord.y * textureVReps;

	 if(diffuseStaticsLightsEnabled)
	 {
		 output.DiffuseDir0 = normalize(mul(worldToTangentSpace,diffuseDir0));
		 output.DiffuseDir1 = normalize(mul(worldToTangentSpace,diffuseDir1));
		 output.DiffuseDir2 = normalize(mul(worldToTangentSpace,diffuseDir2));
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

	float3 Normal = (2 * (tex2D(bumpSampler,input.TexCoords))) - 1.0;
     
	if(diffuseStaticsLightsEnabled)
	{
		diffuseColor += CalculateStaticDiffuseLight(diffuseColor0, input.DiffuseDir0, Normal, diffuseIntensity0);
		diffuseColor += CalculateStaticDiffuseLight(diffuseColor1, input.DiffuseDir1, Normal, diffuseIntensity1);
		diffuseColor += CalculateStaticDiffuseLight(diffuseColor2, input.DiffuseDir2, Normal, diffuseIntensity2);

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
     
    // Each light is summed into a final pixel color
	if(lightsEnabled)
	{
		for(int i=0; i< numLights; i++)
		{
			color += CalculateSingleLight(lights[i], 
					  input.WorldPosition, Normal,
					  diffuseColor, specularColor );
		}
	}
    color.a = 1.0;
    return color;
}


technique MultipleLights
{

    pass P0
    {        
        //set sampler states
        MagFilter[0] = LINEAR;
        MinFilter[0] = LINEAR;
        MipFilter[0] = LINEAR;
        AddressU[0] = WRAP;
        AddressV[0] = WRAP;
        MagFilter[1] = LINEAR;
        MinFilter[1] = LINEAR;
        MipFilter[1] = LINEAR;
        AddressU[1] = WRAP;
        AddressV[1] = WRAP;
		MagFilter[2] = LINEAR;
        MinFilter[2] = LINEAR;
        MipFilter[2] = LINEAR;
        AddressU[2] = WRAP;
        AddressV[2] = WRAP;
        
        Texture[0] = <diffuseTexture>;
        Texture[1] = <specularTexture>;
		Texture[2] = <bumpTexture>;
        
        // set render states
        AlphaBlendEnable = FALSE;

        //set pixel shader states
        VertexShader = compile vs_3_0 BasicVS();     
        PixelShader = compile ps_3_0 MultipleLightPS();
    }
}
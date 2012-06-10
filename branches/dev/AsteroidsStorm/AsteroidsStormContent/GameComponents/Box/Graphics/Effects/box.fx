/*

% Basic shadder.
% Default shadder for Radgie graphic objects.

*/

float4x4 World : World;
float4x4 WorldViewProj : WorldViewProj;
float Time : Time;
float K = 0.2f;
float Multiplier = 2;
float3 TargetPosition = float3(0.0f, 0.0f, 0.0f);

texture Texture : Texture0;
sampler TextureSampler = sampler_state
{
	Texture = <Texture>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

struct a2v
{
	float3 pos : POSITION;
	float2 tex : TEXCOORD;
};

struct v2p
{
	float4 pos : POSITION;
	float2 tex : TEXCOORD;
	float values : COLOR0;
	float3 worldPos : COLOR1;
};

void mainVS(in a2v input, out v2p output)
{
	output.pos = mul(float4(input.pos.xyz, 1.0), WorldViewProj);
	
	float desplz = K * Time;
	output.tex.x = input.tex.x;
	output.tex.y = input.tex.y-desplz;
	
	float value = sin(Time*Multiplier);
	if(value < 0)
	{
		value = -value;
	}
	output.values.x = smoothstep(0,1,value + 0.7f);
	output.worldPos = mul(float4(input.pos.xyz, 1.0), World);
}

float4 mainPS(in v2p input) : COLOR
{
	float4 color = tex2D(TextureSampler, input.tex) * input.values.x;
	color.a = color.a * (1-smoothstep(2,4,distance(input.worldPos, TargetPosition)));
	return color;
}

technique technique0 {
	pass p0 {
		VertexShader = compile vs_3_0 mainVS();
		PixelShader = compile ps_3_0 mainPS();
	}
}

sampler TextureSampler;

struct PixelInput
{
    float2 TextCoord : TEXCOORD0;
};

float4 PixelShaderFunction(PixelInput input) : COLOR
{
	float4 color = tex2D(TextureSampler, input.TextCoord);
	color.b *= 1.25;
	color.rg *= 0.25;
	return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

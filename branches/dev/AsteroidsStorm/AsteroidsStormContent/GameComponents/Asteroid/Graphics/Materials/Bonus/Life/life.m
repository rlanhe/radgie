<?xml version="1.0" encoding="utf-8" ?>
<Material>
  <RenderState>
    <BlendState>
      <Relative>../../../../../../Radgie/Graphics/RenderStates/BlendStates/opaque</Relative>
    </BlendState>
    <RasterizerState>
      <Relative>../../../../../../Radgie/Graphics/RenderStates/RasterizerStates/cullCounterClock</Relative>
    </RasterizerState>
    <SamplerState>
      <Relative>../../../../../../Radgie/Graphics/RenderStates/SamplerStates/linearWrap</Relative>
    </SamplerState>
    <DepthStencilState>
      <Relative>../../../../../../Radgie/Graphics/RenderStates/DepthStencilStates/default</Relative>
    </DepthStencilState>
    <BlendFactor>
      <Color>
        <R>255</R>
        <G>255</G>
        <B>255</B>
        <A>255</A>
      </Color>
    </BlendFactor>
    <MultiSampleMask>-1</MultiSampleMask>
    <ReferenceStencil>0</ReferenceStencil>
  </RenderState>
  <Effect>
    <Relative>../../../../../../Radgie/Graphics/Effects/defaultLighting</Relative>
  </Effect>
  <EffectParameters>
    <EffectParameter semantic="EmissiveColor">
      <Color>
        <R>255</R>
        <G>255</G>
        <B>255</B>
        <A>255</A>
      </Color>
    </EffectParameter>
    <EffectParameter semantic="Texture0">
      <Texture>
        <Relative>../../../Textures/Bonus/Life/life</Relative>
      </Texture>
    </EffectParameter>
    <EffectParameter semantic="DiffuseTextureEnabled">true</EffectParameter>
    <EffectParameter semantic="LightsEnabled">false</EffectParameter>
    <EffectParameter semantic="DiffuseLigthsEnabled">true</EffectParameter>
  </EffectParameters>
</Material>

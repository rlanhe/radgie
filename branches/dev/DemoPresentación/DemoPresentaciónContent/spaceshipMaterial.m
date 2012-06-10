<?xml version="1.0" encoding="utf-8" ?>
<Material>
  <RenderState>
    <BlendState>
      <Relative>opaque</Relative>
    </BlendState>
    <RasterizerState>
      <Relative>cullCounterClock</Relative>
    </RasterizerState>
    <SamplerState>
      <Relative>linearWrap</Relative>
    </SamplerState>
    <DepthStencilState>
      <Relative>default</Relative>
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
    <Relative>spaceship</Relative>
  </Effect>
  <EffectParameters>
    <!-- Lighting section -->
    <EffectParameter semantic="EmissiveColor">
      <Color>
        <R>20</R>
        <G>20</G>
        <B>20</B>
        <A>20</A>
      </Color>
    </EffectParameter>
    <EffectParameter semantic="DiffuseTextureEnabled">true</EffectParameter>
    <EffectParameter semantic="LightsEnabled">false</EffectParameter>
    <EffectParameter semantic="DiffuseLigthsEnabled">true</EffectParameter>
    <EffectParameter semantic="DiffuseColor0">
      <Color>
        <R>255</R>
        <G>255</G>
        <B>255</B>
        <A>255</A>
      </Color>
    </EffectParameter>
    <EffectParameter semantic="DiffuseIntensity0">3</EffectParameter>
    <EffectParameter semantic="DiffuseColor1">
      <Color>
        <R>255</R>
        <G>255</G>
        <B>255</B>
        <A>255</A>
      </Color>
    </EffectParameter>
    <EffectParameter semantic="DiffuseIntensity1">3</EffectParameter>
    <EffectParameter semantic="DiffuseColor2">
      <Color>
        <R>255</R>
        <G>255</G>
        <B>255</B>
        <A>255</A>
      </Color>
    </EffectParameter>
    <EffectParameter semantic="DiffuseIntensity2">3</EffectParameter>
    <EffectParameter semantic="SpecularPower">45</EffectParameter>
    <!-- Glow section -->
    <EffectParameter name="inflate">0.05</EffectParameter>
    <EffectParameter name="glowColor">
      <Color>
        <R>0</R>
        <G>255</G>
        <B>0</B>
        <A>255</A>
      </Color>
    </EffectParameter>
    <EffectParameter name="glowStep">1.7</EffectParameter>
  </EffectParameters>
  <DrawOrder>900</DrawOrder>
</Material>

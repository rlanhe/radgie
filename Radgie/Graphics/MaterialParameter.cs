using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics
{
    /// <summary>
    /// Semantics de los parametros de un material de un Effect en Radgie.
    /// </summary>
    public enum Semantic
    {
    	Alpha,
        AmbientColor,
        DiffuseColor,
        DiffuseColor0,
        DiffuseIntensity0,
        DiffuseDir0,
        DiffuseColor1,
        DiffuseIntensity1,
        DiffuseDir1,
        DiffuseColor2,
        DiffuseIntensity2,
        DiffuseDir2,
        DiffuseLigthsEnabled,
        DiffuseTexture,
        DiffuseTextureEnabled,
        EmissiveColor,
        EyePosition,
        FogColor,
        FogVector,
        LightColor,
        LightFallOff,
        LightPosition,
        LightRange,
        LightsEnabled,
        Lights,
        NoSemantic,
        NumberOfLights,
        Projection,
        SpecularColor,
        SpecularPower,
        SpecularTexture,
        SpecularTextureEnabled,
        SpecularIntensity,
        Texture0,
        Texture1,
        Texture2,
        Texture3,
        Texture4,
        Texture5,
        Texture6,
        Time,
        View,
        World,
        WorldViewProj
    }

    /// <summary>
    /// Indica la frecuencia de actualizacion de un objeto.
    /// </summary>
    public enum UpdateFreq
    {
        PerDraw,
        PerFrame,
        ByDemand
    }

    /// <summary>
    /// Delegado para actualizar los valores de MaterialParameters.
    /// </summary>
    /// <param name="renderer"></param>
    /// <param name="materialParameter"></param>
    public delegate void UpdateMaterialParameterCallback(IRenderer renderer, MaterialParameter materialParameter);

    /// <summary>
    /// Parametro de un material.
    /// Encapsula objetos EffectParameter de XNA que amplian su funcionalidad.
    /// </summary>
    public class MaterialParameter
    {
        #region Properties
        /// <summary>
        /// Nombre del parametro.
        /// </summary>
        public string Name
        {
            get
            {
                return mName;
            }
        }
        private string mName;

        /// <summary>
        /// Semantic del parametro.
        /// </summary>
        public Semantic Semantic
        {
            get
            {
                return mSemantic;
            }
        }
        private Semantic mSemantic;

        /// <summary>
        /// Objeto equivalente XNA.
        /// </summary>
        public EffectParameter EffectParameter
        {
            get
            {
                return mEffectParameter;
            }
        }
        private EffectParameter mEffectParameter;

        /// <summary>
        /// Frecuencia de actualizacion.
        /// </summary>
        public UpdateFreq UpdateFreq
        {
            get
            {
                return mUpdateFreq;
            }
            set
            {
                mUpdateFreq = value;
            }
        }
        private UpdateFreq mUpdateFreq;

        /// <summary>
        /// Callback llamado cuando se necesita actualizar el valor del parametro.
        /// </summary>
        public UpdateMaterialParameterCallback UpdateCallback
        {
            get
            {
                return mUpdateCallback;
            }
            set
            {
                mUpdateCallback = value;
            }
        }
        private UpdateMaterialParameterCallback mUpdateCallback;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo parametro del material.
        /// </summary>
        /// <param name="effectParameter">Objeto XNA que encapsula.</param>
        public MaterialParameter(EffectParameter effectParameter): this(effectParameter, MaterialParameter.GetSemantic(effectParameter))
        {
        }

        /// <summary>
        /// Crea un nuevo parametro del material.
        /// </summary>
        /// <param name="effectParameter">Parametro</param>
        /// <param name="semantic">Semantic</param>
        public MaterialParameter(EffectParameter effectParameter, Semantic semantic)
        {
            mEffectParameter = effectParameter;
            mName = effectParameter.Name;
            mSemantic = semantic;

            // TODO: Completar y sacar fuera para compartir funcionalidad con Material.LoadParameterValues
            if (mSemantic == Semantic.NoSemantic)
            {
                // Aunque el parametro no tiene semantic, trata de asignarle una a partir del nombre. Por compatibilidad con los shadders basicos de XNA.
                switch (mName)
                {
                    case "Texture":
                        mSemantic = Semantic.Texture0;
                        break;
                }
            }

            // En funcion del semantic configura el parametro
            switch (mSemantic)
            {
            	case Semantic.Alpha:
            		mUpdateFreq = UpdateFreq.ByDemand;
                    break;
                case Semantic.AmbientColor:
                    mUpdateCallback = MaterialParameter.UpdateAmbientColor;
                    mUpdateFreq = UpdateFreq.PerFrame;
                    break;
                case Semantic.DiffuseColor:
                case Semantic.DiffuseColor0:
                case Semantic.DiffuseIntensity0:
                case Semantic.DiffuseDir0:
                case Semantic.DiffuseColor1:
                case Semantic.DiffuseIntensity1:
                case Semantic.DiffuseDir1:
                case Semantic.DiffuseColor2:
                case Semantic.DiffuseIntensity2:
                case Semantic.DiffuseDir2:
                case Semantic.DiffuseLigthsEnabled:
                case Semantic.DiffuseTexture:
                case Semantic.DiffuseTextureEnabled:
                case Semantic.EmissiveColor:
                    mUpdateFreq = UpdateFreq.ByDemand;
                    break;
                case Semantic.EyePosition:
                    mUpdateCallback = MaterialParameter.UpdateEyePosition;
                    mUpdateFreq = UpdateFreq.PerFrame;
                    break;
                case Semantic.FogColor:
                case Semantic.FogVector:
                case Semantic.LightColor:
                case Semantic.LightFallOff:
                case Semantic.LightPosition:
                case Semantic.LightRange:
                case Semantic.LightsEnabled:
                    mUpdateFreq = UpdateFreq.ByDemand;
                    break;
                case Semantic.Lights:
                    mUpdateCallback = MaterialParameter.UpdateLights;
                    mUpdateFreq = UpdateFreq.PerFrame;
                    break;
                case Semantic.NoSemantic:
                    mUpdateFreq = UpdateFreq.ByDemand;
                    break;
                case Semantic.NumberOfLights:
                    mUpdateFreq = UpdateFreq.ByDemand;
                    break;
                case Semantic.Projection:
                    mUpdateCallback = MaterialParameter.UpdateProjection;
                    mUpdateFreq = UpdateFreq.PerFrame;
                    break;
                case Semantic.SpecularColor:
                case Semantic.SpecularPower:
                case Semantic.SpecularTexture:
                case Semantic.SpecularTextureEnabled:
                case Semantic.SpecularIntensity:
                case Semantic.Texture0:
                case Semantic.Texture1:
                case Semantic.Texture2:
                case Semantic.Texture3:
                case Semantic.Texture4:
                case Semantic.Texture5:
                case Semantic.Texture6:
                    mUpdateFreq = UpdateFreq.ByDemand;
                    break;
                case Semantic.Time:
                    mUpdateCallback = MaterialParameter.UpdateTime;
                    mUpdateFreq = UpdateFreq.PerFrame;
                    break;
                case Semantic.View:
                    mUpdateCallback = MaterialParameter.UpdateView;
                    mUpdateFreq = UpdateFreq.PerFrame;
                    break;
                case Semantic.World:
                    mUpdateCallback = MaterialParameter.UpdateWorld;
                    mUpdateFreq = UpdateFreq.PerFrame;
                    break;
                case Semantic.WorldViewProj:
                    mUpdateCallback = MaterialParameter.UpdateWorldViewProjection;
                    mUpdateFreq = UpdateFreq.PerFrame;
                    break;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtiene el semantic asociado a un parametro a partir del nombre de esta.
        /// </summary>
        /// <param name="effectParameter">Parametro</param>
        /// <returns>Semantic del parametro</returns>
        public static Semantic GetSemantic(EffectParameter effectParameter)
        {
            string semantic = effectParameter.Semantic;
            Semantic effectSemantic = effectSemantic = Semantic.NoSemantic;
            try
            {
                effectSemantic = (Semantic)Enum.Parse(typeof(Semantic), semantic, false);
            }
            catch
            {
            }

            return effectSemantic;
        }

        /// <summary>
        /// Actualiza el valor del parametro.
        /// </summary>
        /// <param name="renderer">Renderer</param>
        public void UpdateValue(IRenderer renderer)
        {
            if (mUpdateCallback != null)
            {
                if((mUpdateFreq == UpdateFreq.PerDraw) || (mUpdateFreq == UpdateFreq.PerFrame))//TODO: si es perFrame, asegurar que solo se llama una vez
                {
                    mUpdateCallback(renderer, this);
                }
            }
        }

        #region Setter methods

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Boolean value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Boolean[] value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Int32 value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Int32[] value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Matrix value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Matrix[] value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Quaternion value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Quaternion[] value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Single value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Single[] value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(String value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Texture value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Vector2 value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Vector2[] value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Vector3 value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Vector3[] value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Vector4 value)
        {
            mEffectParameter.SetValue(value);
        }

        /// <summary>
        /// Set MaterialParameter value.
        /// </summary>
        /// <param name="value">New value for MaterialParameter.</param>
        public void SetValue(Vector4[] value)
        {
            mEffectParameter.SetValue(value);
        }

        #endregion
        
        #region Effect Parameter Update Callbacks
        /// <summary>
        /// Updates material parameter with World matrix.
        /// This method is called atomatically during Material.Bind method call.
        /// </summary>
        /// <param name="renderer">Renderer</param>
        /// <param name="materialParameter">material parameter</param>
        public static void UpdateWorld(IRenderer renderer, MaterialParameter materialParameter)
        {
            materialParameter.SetValue(renderer.World);
        }

        /// <summary>
        /// Updates material parameter with View matrix.
        /// This method is called atomatically during Material.Bind method call.
        /// </summary>
        /// <param name="renderer">Renderer</param>
        /// <param name="materialParameter">material parameter</param>
        public static void UpdateView(IRenderer renderer, MaterialParameter materialParameter)
        {
            materialParameter.SetValue(renderer.View);
        }

        /// <summary>
        /// Updates material parameter with Projection matrix.
        /// This method is called atomatically during Material.Bind method call.
        /// </summary>
        /// <param name="renderer">Renderer</param>
        /// <param name="materialParameter">material parameter</param>
        public static void UpdateProjection(IRenderer renderer, MaterialParameter materialParameter)
        {
            materialParameter.SetValue(renderer.Projection);
        }

        /// <summary>
        /// Updates material parameter with WorldViewProjection matrix.
        /// This method is called atomatically during Material.Bind method call.
        /// </summary>
        /// <param name="renderer">Renderer</param>
        /// <param name="materialParameter">material parameter</param>
        public static void UpdateWorldViewProjection(IRenderer renderer, MaterialParameter materialParameter)
        {
            materialParameter.SetValue(renderer.WorldViewProjection);
        }

        /// <summary>
        /// Actualiza los valores de las luces en el material.
        /// </summary>
        /// <param name="renderer">Renderer.</param>
        /// <param name="materialParameter">Parametro del material.</param>
        public static void UpdateLights(IRenderer renderer, MaterialParameter materialParameter)
        {
            MaterialParameter enabled = renderer.Material.TryParameter(Semantic.LightsEnabled);
            if((enabled != null) && (enabled.mEffectParameter.GetValueBoolean()))
            {
                List<ILight> lights = renderer.Lights;
                int max = renderer.MaxLightsNumber;
                for(int i=0; i<max; i++)
                {
                    ILight light = lights[i];
                    EffectParameter effectLight = materialParameter.EffectParameter.Elements[i];
                    EffectParameterCollection parameters = effectLight.StructureMembers;
                    foreach (EffectParameter p in parameters)
                    {
                        switch ((Semantic)Enum.Parse(typeof(Semantic), p.Semantic, false))
                        {
                            case Semantic.LightColor:
                                p.SetValue(light.Color.ToVector4());
                                break;
                            case Semantic.LightFallOff:
                                p.SetValue(light.FallOff);
                                break;
                            case Semantic.LightPosition:
                                p.SetValue(light.Position);
                                break;
                            case Semantic.LightRange:
                                p.SetValue(light.Radius);
                                break;
                        }
                    }
                }
                renderer.Material[Semantic.NumberOfLights].SetValue(max);
            }
        }

        /// <summary>
        /// Actualia la posicion de la camara.
        /// </summary>
        /// <param name="renderer">Renderer.</param>
        /// <param name="materialParameter">Parametro del material.</param>
        public static void UpdateEyePosition(IRenderer renderer, MaterialParameter materialParameter)
        {
            materialParameter.SetValue(renderer.CurrentCamera.World.Translation);
        }

        /// <summary>
        /// Actualiza el color de la luz de ambiente.
        /// </summary>
        /// <param name="renderer">Renderer.</param>
        /// <param name="materialParameter">Parametro del material.</param>
        public static void UpdateAmbientColor(IRenderer renderer, MaterialParameter materialParameter)
        {
            materialParameter.SetValue(renderer.AmbientLightColor.ToVector4());
        }

        public static void UpdateTime(IRenderer renderer, MaterialParameter materialParameter)
        {
            materialParameter.SetValue((float)renderer.GraphicSystem.LastTimeUpdated.TotalGameTime.TotalSeconds);
        }
        #endregion
        #endregion
    }
}

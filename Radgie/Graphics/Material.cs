using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;
using Radgie.Core;

namespace Radgie.Graphics
{
    /// <summary>
    /// Material de los objetos graficos.
    /// Indica la configuracion a usar para dibujar un objeto. Esta configuracion va desde los render states que debe usar el dispositivo grafico, 
    /// el shader a usar y sus parametros. Se encarga tambien de actualizar en lugar del usuario los valores de los parametros comunes a todos los shaders.
    /// </summary>
    public class Material: IComparable<Material>, IIdentifiable
    {
        #region Properties

        #region IIdentifiable Members
        /// <summary>
        /// Ver <see cref="Radgie.Core.IIdentifiable.Id"/>
        /// </summary>
        public string Id
        {
            get
            {
                return mId;
            }
        }
        private string mId;
        #endregion

        /// <summary>
        /// RenderState que hay que aplicar al usar el material.
        /// </summary>
        public RenderState RenderState
        {
            get
            {
                return mRenderState;
            }
            set
            {
                mRenderState = value;
            }
        }
        protected RenderState mRenderState;

        /// <summary>
        /// Shader a usar al dibujar objetos empleando este material.
        /// </summary>
        public Effect Effect
        {
            get
            {
                return mEffect;
            }
            set
            {
                mEffect = value;
                ConfigureParameters();
            }
        }
        private Effect mEffect;

        /// <summary>
        /// Coleccion de parametros del shader segun su identificador.
        /// </summary>
        protected Dictionary<string, MaterialParameter> mEffectParameters;
        /// <summary>
        /// Coleccion de parametros del shader segun su semantic.
        /// </summary>
        protected Dictionary<Semantic, MaterialParameter> mEffectParametersBySemantic;

        /// <summary>
        /// Indica como se deben ordenar los objetos a la hora de dibujarse segun su material. Los objetos que tienen transparencias deben dibujarse
        /// en ultimo lugar para evitar problemas.
        /// </summary>
        public int DrawOrder
        {
            get
            {
                return mDrawOrder;
            }
            set
            {
                mDrawOrder = value;
            }
        }
        private int mDrawOrder = 0;

        /// <summary>
        /// Accede a un parametro del shader a partir de su nombre.
        /// </summary>
        /// <param name="param">Nombre del parametro.</param>
        /// <returns>Parametro del material.</returns>
        public MaterialParameter this[string param]
        {
            get
            {
                return mEffectParameters[param];
            }
        }

        /// <summary>
        /// Accede a un parametro del shader a partir de su semantic.
        /// </summary>
        /// <param name="param">Semantic del parametro.</param>
        /// <returns>Parametro del material.</returns>
        public MaterialParameter this[Semantic param]
        {
            get
            {
                return mEffectParametersBySemantic[param];
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo material.
        /// </summary>
        /// <param name="id">Identificador del material.</param>
        public Material(string id)
        {
            mId = id;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Trata de obtener un parametro del material.
        /// </summary>
        /// <param name="param">Nombre del parametro.</param>
        /// <returns>El parametro buscado o null en caso de no encontrarlo.</returns>
        public MaterialParameter TryParameter(string param)
        {
            MaterialParameter eParameter;
            mEffectParameters.TryGetValue(param, out eParameter);
            return eParameter;
        }

        /// <summary>
        /// Trata de obtener un parametro del material.
        /// </summary>
        /// <param name="param">Semantic del parametro.</param>
        /// <returns>El parametro buscado o null en caso de no encontrarlo.</returns>
        public MaterialParameter TryParameter(Semantic param)
        {
            MaterialParameter eParameter;
            mEffectParametersBySemantic.TryGetValue(param, out eParameter);
            return eParameter;
        }
        
        /// <summary>
        /// Carga los parametros a partir del shader asignado al material.
        /// </summary>
        /// <param name="effect">Shader del que tomar los parametros.</param>
        public void LoadParametersFromEffect(Effect effect)
        {
            lock (effect.GraphicsDevice)
            {
            	// Carga los valores de los parametros para que, posteriormente pueda copiarlos.
                effect.CurrentTechnique.Passes[0].Apply();
            }

            foreach(EffectParameter p in effect.Parameters)
            {
                
                if ((p.Semantic != null) && (p.Semantic != " ") && (p.Semantic != ""))
                {
                    MaterialParameter output = this[p.Semantic];
                    CopyParameterValue(p, output);
                }
                else
                {
                    // Para ciertos parametros, se trata de cargar su valor aunque no tengan semantic asociado, a partir de su nombre comun.
                    string name = p.Name;
                    MaterialParameter output = null;
                    switch (name)
                    {
                        case "Texture":
                            output = TryParameter(Semantic.Texture0);
                            break;
                        /*
                        case "DirLight0Direction":
                            output = TryParameter(Semantic.DiffuseDir0);
                            break;
                        case "DirLight0DiffuseColor":
                            output = TryParameter(Semantic.DiffuseColor0);
                            break;
                        case "DirLight0SpecularColor":
                            output = TryParameter(Semantic.SpecularColor);
                            break;
                        case "DirLight1Direction":
                            output = TryParameter(Semantic.DiffuseDir1);
                            break;
                        case "DirLight1DiffuseColor":
                            output = TryParameter(Semantic.DiffuseColor1);
                            break;
                        case "DirLight1SpecularColor":
                            output = TryParameter(Semantic.SpecularColor);
                            break;
                        case "DirLight2Direction":
                            output = TryParameter(Semantic.DiffuseDir2);
                            break;
                        case "DirLight2DiffuseColor":
                            output = TryParameter(Semantic.DiffuseColor2);
                            break;
                        case "DirLight2SpecularColor":
                            output = TryParameter(Semantic.SpecularColor);
                            break;
                         */
                        default:
                            Semantic semanticValue;
                            try
                            {
                                semanticValue = (Semantic)Enum.Parse(typeof(Semantic), name, false);
                            }
                            catch
                            {
                            }
                            break;
                    }
                    if (output != null)
                    {
                        CopyParameterValue(p, output);
                    }
                }
            }
        }

        /// <summary>
        /// Copia el valor de un parametro en otro en funcion del tipo de ambos.
        /// </summary>
        /// <param name="input">Parametro de entrada.</param>
        /// <param name="output">Parametro de salida.</param>
        private void CopyParameterValue(EffectParameter input, MaterialParameter output)
        {
            int count = input.Elements.Count;
            bool array = count > 0;
            switch (output.EffectParameter.ParameterClass)
            {
                case EffectParameterClass.Matrix:
                    if (array)
                    {
                        output.SetValue(input.GetValueMatrixArray(count));
                    }
                    else
                    {
                        output.SetValue(input.GetValueMatrix());
                    }
                    break;
                case EffectParameterClass.Scalar:
                    if (input.ParameterType == EffectParameterType.Single)
                    {
                        if (array)
                        {
                            output.SetValue(input.GetValueSingleArray(count));
                        }
                        else
                        {
                            output.SetValue(input.GetValueSingle());
                        }
                    }
                    break;
                case EffectParameterClass.Vector:
                    if (input.ParameterType == EffectParameterType.Single)
                    {
                        switch (input.ColumnCount)
                        {
                            case 2:
                                if (array)
                                {
                                    output.SetValue(input.GetValueVector2Array(count));
                                }
                                else
                                {
                                    Vector2 inputValue = input.GetValueVector2();
                                    if (output.EffectParameter.ColumnCount == 2)
                                    {
                                        output.SetValue(inputValue);
                                    }
                                    else if (output.EffectParameter.ColumnCount == 3)
                                    {
                                        output.SetValue(new Vector3(inputValue.X, inputValue.Y, 1.0f));
                                    }
                                    else if (output.EffectParameter.ColumnCount == 4)
                                    {
                                        output.SetValue(new Vector4(inputValue.X, inputValue.Y, 1.0f, 1.0f));
                                    }
                                }
                                break;
                            case 3:
                                if (array)
                                {
                                    output.SetValue(input.GetValueVector3Array(count));
                                }
                                else
                                {
                                    Vector3 inputValue = input.GetValueVector3();
                                    if (output.EffectParameter.ColumnCount == 2)
                                    {
                                        output.SetValue(new Vector2(inputValue.X, inputValue.Y));
                                    }
                                    else if (output.EffectParameter.ColumnCount == 3)
                                    {
                                        output.SetValue(inputValue);
                                    }
                                    else if (output.EffectParameter.ColumnCount == 4)
                                    {
                                        output.SetValue(new Vector4(inputValue.X, inputValue.Y, inputValue.Z, 1.0f));
                                    }
                                }
                                break;
                            case 4:
                                if (array)
                                {
                                    output.SetValue(input.GetValueVector4Array(count));
                                }
                                else
                                {
                                    Vector4 inputValue = input.GetValueVector4();
                                    if (output.EffectParameter.ColumnCount == 2)
                                    {
                                        output.SetValue(new Vector2(inputValue.X, inputValue.Y));
                                    }
                                    else if (output.EffectParameter.ColumnCount == 3)
                                    {
                                        output.SetValue(new Vector3(inputValue.X, inputValue.Y, inputValue.Z));
                                    }
                                    else if (output.EffectParameter.ColumnCount == 4)
                                    {
                                        output.SetValue(inputValue);
                                    }
                                }
                                break;
                            // TODO: cualquier otro caso exception
                        }
                    }
                    else
                    {
                        // TODO: exception
                    }
                    break;
                case EffectParameterClass.Struct:
                    // TODO: exception
                    break;
                case EffectParameterClass.Object:
                    if (array)
                    {
                        // TODO: exception
                    }
                    switch (input.ParameterType)
                    {
                        case EffectParameterType.String:
                            output.SetValue(input.GetValueString());
                            break;
                        case EffectParameterType.Texture2D:
                            output.SetValue(input.GetValueTexture2D());
                            break;
                        case EffectParameterType.Texture3D:
                            output.SetValue(input.GetValueTexture3D());
                            break;
                        case EffectParameterType.TextureCube:
                            output.SetValue(input.GetValueTextureCube());
                            break;
                    }
                    break;
            }
        }
        
        /// <summary>
        /// Inicializa la lista de parametros del shader.
        /// </summary>
        protected void ConfigureParameters()
        {
            mEffectParameters = new Dictionary<string, MaterialParameter>();

            foreach (EffectParameter param in mEffect.Parameters)
            {
                mEffectParameters.Add(param.Name, new MaterialParameter(param));
            }

            mEffectParametersBySemantic = new Dictionary<Semantic, MaterialParameter>();

            foreach(KeyValuePair<string, MaterialParameter> tuple in mEffectParameters)
            {
                Semantic semantic = tuple.Value.Semantic;
                if(semantic != Semantic.NoSemantic)
                {
                    mEffectParametersBySemantic.Add(semantic, tuple.Value);
                }
            }
        }

        /// <summary>
        /// Actualiza el valor de los parametros.
        /// </summary>
        /// <param name="renderer">Renderer.</param>
        protected void LoadParameterValues(IRenderer renderer)
        {
            foreach (KeyValuePair<string, MaterialParameter> tuple in mEffectParameters)
            {
                tuple.Value.UpdateValue(renderer);
            }
        }

        /// <summary>
        /// Carga los valores de los parametros en el render.
        /// </summary>
        /// <param name="renderer">Renderer.</param>
        public void Bind(IRenderer renderer)
        {
            LoadParameterValues(renderer);
        }

        /// <summary>
        /// Crea una copia identica de si mismo.
        /// </summary>
        /// <returns>Nueva copia del material.</returns>
        public Material Clone()
        {
            Material clone = new Material(Id);

            // Bloquea el dispositivo grafico para evitar conflictos con otros hilos de ejecucion.
            lock (((IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem))).Device)
            {
                clone.Effect = Effect.Clone();
            }

            clone.RenderState = RenderState;
            clone.DrawOrder = DrawOrder;

            foreach (KeyValuePair<string, MaterialParameter> tuple in mEffectParameters)
            {
                MaterialParameter p = clone.mEffectParameters[tuple.Key];
                CopyParameterValue(clone.mEffectParameters[tuple.Key].EffectParameter, p);
                p.UpdateCallback = tuple.Value.UpdateCallback;
                p.UpdateFreq = tuple.Value.UpdateFreq;
            }

            foreach (KeyValuePair<Semantic, MaterialParameter> tuple in mEffectParametersBySemantic)
            {
                MaterialParameter p = clone.mEffectParametersBySemantic[tuple.Key];
                CopyParameterValue(clone.mEffectParametersBySemantic[tuple.Key].EffectParameter, p);
                p.UpdateCallback = tuple.Value.UpdateCallback;
                p.UpdateFreq = tuple.Value.UpdateFreq;
            }
            return clone;
        }

        /// <summary>
        /// Metodo para ordenar objetos segun el orden en que tengan que ser dibujados.
        /// </summary>
        /// <param name="other">Otro elemento para compararse.</param>
        /// <returns>-1 si es menor, 0 si es igual y 1 si es mayor.</returns>
        public int CompareTo(Material other)
        {
            return DrawOrder.CompareTo(other.DrawOrder);
        }
        #endregion
    }
}

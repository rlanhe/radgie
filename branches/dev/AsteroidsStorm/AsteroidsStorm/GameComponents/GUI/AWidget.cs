using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsStorm.GameComponents.GUI
{
    public delegate void WidgetActionDelegate(IWidget widget);

    /// <summary>
    /// Abstraccion de un componente de una interfaz grafica.
    /// Los componenetes de la interfaz grafica deben derivar de esta clase que provee su funcionalidad general.
    /// </summary>
    abstract class AWidget: IWidget, IIdentifiable
    {
        #region Properties

        #region IIdentifiable Properties
        /// <summary>
        /// Identificador del widget.
        /// </summary>
        public string Id
        {
            get
            {
                return mId;
            }
        }
        protected string mId;
        #endregion

        #region IWidget Properties
        /// <summary>
        /// Widget padre.
        /// Si es null, se trata del root de la jerarquia de componentes.
        /// </summary>
        public IContainer Parent
        {
            get
            {
                return mParent;
            }
            set
            {
                if (mParent != value)
                {
                    Microsoft.Xna.Framework.Vector2 pos = new Microsoft.Xna.Framework.Vector2(X, Y);
                    mParent = value;
                    X = (int)pos.X;
                    Y = (int)pos.Y;
                }
            }
        }
        protected IContainer mParent;

        /// <summary>
        /// Posicion en el eje X del widget respecto a la esquina superior izquierda de la pantalla.
        /// </summary>
        public virtual int X
        {
            get
            {
                int x = (int)GameComponent.Transformation.Translation2D.X;
                return x + (Parent != null ? (int)(Parent.Width*0.5f) : 0);
            }
            set
            {
                Microsoft.Xna.Framework.Vector2 pos = GameComponent.Transformation.Translation2D;
                pos.X = value - (Parent != null ? (int)(Parent.Width*0.5f) : 0);
                GameComponent.Transformation.Translation2D = pos;
            }
        }

        /// <summary>
        /// Posicion en el eje Y del widget respecto a la esquina superior izquierda de la pantalla.
        /// </summary>
        public virtual int Y
        {
            get
            {
                int y = (int)GameComponent.Transformation.Translation2D.Y;
                if (Parent != null)
                {
                    y = -(y - Parent.Height) + (int)(Parent.Height * 0.5f);
                }
                return y;
            }
            set
            {
                Microsoft.Xna.Framework.Vector2 pos = GameComponent.Transformation.Translation2D;
                pos.Y = Parent == null ? value : -(value - Parent.Height) - (int)(Parent.Height * 0.5f);
                GameComponent.Transformation.Translation2D = pos;
            }
        }

        /// <summary>
        /// Ancho del widget en pixeles.
        /// </summary>
        public virtual int Width
        {
            get
            {
                return mWidth;
            }
            set
            {
                mWidth = value;
            }
        }
        protected int mWidth;

        /// <summary>
        /// Altura del widget en pixeles.
        /// </summary>
        public virtual int Height
        {
            get
            {
                return mHeight;
            }
            set
            {
                mHeight = value;
            }
        }
        protected int mHeight;
        
        /// <summary>
        /// GameComponent que encapsula el widget.
        /// </summary>
        public Radgie.Core.IGameComponent GameComponent
        {
            get
            {
                return mGameComponent;
            }
        }
        protected Radgie.Core.IGameComponent mGameComponent;

        /// <summary>
        /// Orden de tabulacion.
        /// Indica el orden natural en que se debe iterar por los widget de gui de usuario al desplazarse por ellos de manera secuencial. Un valor mas bajo indica que va primero.
        /// </summary>
        public byte TabOrder { get; set; }

        /// <summary>
        /// Indica el orden en que debe dibujarse el widget respecto al resto de widgets que componen la interfaz grafica.
        /// Permite superponer unos componentes a otros dibujandolos en el orden deseado.
        /// </summary>
        public virtual int DrawOrder { get; set; }

        /// <summary>
        /// Indica si el widget es visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                return mGameComponent.Visible;
            }
            set
            {
                mGameComponent.Visible = value;
            }
        }
        
        /// <summary>
        /// Accion que se ejecuta al convertirse en el widget activo.
        /// </summary>
        public WidgetActionDelegate OnActivateDelegate
        {
            get
            {
                return mOnActivateDelegate;
            }
            set
            {
                SetDelegate(ref mOnActivateDelegate, value);
            }
        }
        protected WidgetActionDelegate mOnActivateDelegate;

        /// <summary>
        /// Accion que se ejecuta al dejar de ser el widget activo.
        /// </summary>
        public WidgetActionDelegate OnDeactivateDelegate
        {
            get
            {
                return mOnDeactivateDelegate;
            }
            set
            {
                SetDelegate(ref mOnDeactivateDelegate, value);
            }
        }
        protected WidgetActionDelegate mOnDeactivateDelegate;

        /// <summary>
        /// Accion que se ejecuta al seleccionar el widget.
        /// </summary>
        public WidgetActionDelegate FireDelegate
        {
            get
            {
                return mFireDelegate;
            }
            set
            {
                SetDelegate(ref mFireDelegate, value);
            }
        }
        protected WidgetActionDelegate mFireDelegate;
        #endregion

        /// <summary>
        /// Accion por defecto vacia.
        /// </summary>
        public static WidgetActionDelegate EmptyAction = delegate(IWidget widget) { };

        #endregion

        #region Constructors

        /// <summary>
        /// Crea un nuevo widget.
        /// </summary>
        /// <param name="id">Identificador del widget.</param>
        /// <param name="width">Anchura del widget en pixeles.</param>
        /// <param name="height">Altura del widget en pixeles.</param>
        public AWidget(string id, int width, int height)
        {
            mId = id;

            Width = width;
            Height = height;

            CreateGameComponent();
            
            X = 0;
            Y = 0;

            Visible = true;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Crea el GameComponent que representa al widget en la escena.
        /// </summary>
        protected abstract void CreateGameComponent();

        /// <summary>
        /// Establece el delegado para una de las acciones soportadas por el widget.
        /// </summary>
        /// <param name="dest">Accion del widget.</param>
        /// <param name="value">Accion que se desencadenara al ocurrir una accion del widget.</param>
        private void SetDelegate(ref WidgetActionDelegate dest, WidgetActionDelegate value)
        {
            if ((value != null) && (value != EmptyAction))
            {
                dest = value;
            }
        }

        #region IComparable Methods
        /// <summary>
        /// Ordena los widget en funcion del valor de TabOrder.
        /// </summary>
        /// <param name="other">Otro widget con el que se compara.</param>
        /// <returns>-1, 0 o 1 en funcion de si es menor, igual o mayor.</returns>
        public int CompareTo(IWidget other)
        {
            return TabOrder.CompareTo(other.TabOrder);
        }
        #endregion

        #region IWidget Methods
        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IWidget.OnActive"/>
        /// </summary>
        public void OnActivate()
        {
            ExecuteDelegate(mOnActivateDelegate);
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IWidget.OnDeactive"/>
        /// </summary>
        public void OnDeactive()
        {
            ExecuteDelegate(mOnDeactivateDelegate);
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IWidget.FireAction"/>
        /// </summary>
        public virtual void FireAction()
        {
            ExecuteDelegate(mFireDelegate);
        }

        /// <summary>
        /// Ejecuta las acciones asociadas a los eventos del widget.
        /// </summary>
        /// <param name="delegateMethod">Delegado.</param>
        private void ExecuteDelegate(WidgetActionDelegate delegateMethod)
        {
            if ((delegateMethod != null) && (delegateMethod != EmptyAction))
            {
                delegateMethod(this);
            }
        }

        #endregion
        #endregion
    }
}

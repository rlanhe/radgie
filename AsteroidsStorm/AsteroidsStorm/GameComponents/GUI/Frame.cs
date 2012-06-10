using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsStorm.GameComponents.GUI
{
    /// <summary>
    /// Widget contenedor de otros widgets.
    /// </summary>
    class Frame : AWidget, IContainer
    {
        /// <summary>
        /// Widget seleccionado dentro de los widgets del frame.
        /// </summary>
        private IWidget mSelectedWidget;
        /// <summary>
        /// Widgets hijos del frame.
        /// </summary>
        private List<IWidget> mChilds;
        /// <summary>
        /// Lista auxilitar para componer la secuencia en que se deben recorrer los widgets.
        /// </summary>
        private List<IWidget> mTmpList;

        /// <summary>
        /// Accion que se ejecuta al seleccionar un widget.
        /// </summary>
        /// <param name="widget">Widget seleccionado.</param>
        private static void FireAction(IWidget widget)
        {
            widget.FireAction();
        }

        /// <summary>
        /// Crea un nuevo marco contenedor de widgets.
        /// </summary>
        /// <param name="id">Identificador del marco.</param>
        /// <param name="width">Anchura en pixeles del marco.</param>
        /// <param name="height">Altura en pixeles del marco.</param>
        public Frame(string id, int width, int height)
            : base(id, width, height)
        {
            mTmpList = new List<IWidget>();
            mChilds = new List<IWidget>();
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.AWidget.CreateGameComponent"/>
        /// </summary>
        protected override void CreateGameComponent()
        {
            mGameComponent = new GameComponent(Id);
        }

        /// <summary>
        /// Lista de widgets hijos.
        /// </summary>
        public IEnumerator<IWidget> Childs
        {
            get
            {
                return mChilds.GetEnumerator();
            }
        }

        #region IContainer Methods
        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IContainer.AddWidget"/>
        /// </summary>
        public bool AddWidget(IWidget widget)
        {
            if (!mChilds.Contains(widget))
            {
                if (widget.Parent != null)
                {
                    Parent.RemoveWidget(widget);
                }

                mChilds.Add(widget);
                widget.Parent = this;
                mGameComponent.AddGameComponent(widget.GameComponent);
                mChilds.Sort();
                GameComponent.Update(null);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IContainer.RemoveWidget"/>
        /// </summary>
        public bool RemoveWidget(IWidget widget)
        {
            if (widget.Parent == this)
            {
                if (mChilds.Contains(widget))
                {
                    if ((widget == mSelectedWidget) && (widget == NextWidget()))
                    {
                        mSelectedWidget = null;
                    }

                    mChilds.Remove(widget);
                    widget.Parent = null;
                    GameComponent.RemoveGameComponent(widget.GameComponent);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Crea una lista de widgets en el orden en que debe recorrerse.
        /// </summary>
        /// <param name="list">Lista donde dejara el resultado.</param>
        private void ComposeWidgetList(List<IWidget> list)
        {
            foreach (IWidget widget in mChilds)
            {
                if (widget is Frame)
                {
                    ((Frame)widget).ComposeWidgetList(list);
                }
                else if (widget.TabOrder > 0)
                {
                    list.Add(widget);
                }
            }
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IContainer.NextWidget"/>
        /// </summary>
        public IWidget NextWidget()
        {
            IWidget lastWidget = mSelectedWidget;
            mTmpList.Clear();
            this.ComposeWidgetList(mTmpList);

            if (mSelectedWidget == null)
            {
                if (mTmpList.Count > 0)
                {
                    mSelectedWidget = mTmpList[0];
                }
            }
            else
            {
                int currentIndex = mTmpList.IndexOf(lastWidget);
                currentIndex++;

                if (currentIndex <= mTmpList.Count - 1)
                {
                    mSelectedWidget = mTmpList[currentIndex];
                }
                else
                {
                    mSelectedWidget = mTmpList[0];
                }
            }

            if (lastWidget != null)
            {
                lastWidget.OnDeactive();
            }

            if (mSelectedWidget != null)
            {
                mSelectedWidget.OnActivate();
            }

            return mSelectedWidget;
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IContainer.CurrentWidget"/>
        /// </summary>
        public IWidget CurrentWidget()
        {
            return mSelectedWidget;
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IWidget.FireAction"/>
        /// </summary>
        public override void FireAction()
        {
            if (mSelectedWidget != null)
            {
                mSelectedWidget.FireAction();
            }
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IContainer.PreviousWidget"/>
        /// </summary>
        public IWidget PreviousWidget()
        {
            IWidget lastWidget = mSelectedWidget;
            mTmpList.Clear();
            this.ComposeWidgetList(mTmpList);

            if (mSelectedWidget == null)
            {
                if (mTmpList.Count > 0)
                {
                    mSelectedWidget = mTmpList[0];
                }
            }
            else
            {
                int currentIndex = mTmpList.IndexOf(lastWidget);
                currentIndex--;

                if (currentIndex >= 0)
                {
                    mSelectedWidget = mTmpList[currentIndex];
                }
                else
                {
                    mSelectedWidget = mTmpList[mTmpList.Count - 1];
                }
            }

            if (lastWidget != null)
            {
                lastWidget.OnDeactive();
            }

            if (mSelectedWidget != null)
            {
                mSelectedWidget.OnActivate();
            }

            return mSelectedWidget;
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IContainer.Reset"/>
        /// </summary>
        public void Reset()
        {
            mSelectedWidget = null;
        }
        #endregion
    }
}

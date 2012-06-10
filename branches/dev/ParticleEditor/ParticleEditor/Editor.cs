using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Radgie.Graphics;
using Microsoft.Xna.Framework;

namespace ParticleEditor
{
    public partial class Editor : Form
    {
        public delegate void ParticleSystemAction(ParticleSystemSettings settings);

        private ParticleSystemAction mAction;

        public int ParticlesPerSecond
        {
            get
            {
                return (int)ParticlesPerSecond_Value.Value;
            }
            set
            {
                ParticlesPerSecond_Value.Value = (decimal)value;
            }
        }

        public ParticleSystemSettings Settings
        {
            get
            {
                return SettingsFromValues();
            }
            set
            {
                UpdateValues(value);
            }
        }

        public Vector3 EmitterVelocity
        {
            get
            {
                return new Vector3(float.Parse(EmitterVelocity_X_Value.Text), float.Parse(EmitterVelocity_Y_Value.Text),float.Parse(EmitterVelocity_Z_Value.Text));
            }
            set
            {
                EmitterVelocity_X_Value.Text = value.X.ToString();
                EmitterVelocity_Y_Value.Text = value.Y.ToString();
                EmitterVelocity_Z_Value.Text = value.Z.ToString();
            }
        }
        
        public Editor(ParticleSystemAction action)
        {
            InitializeComponent();
            RemoveCloseButton(this);

            mAction = action;
        }

        private ParticleSystemSettings SettingsFromValues()
        {
            ParticleSystemSettings settings = new ParticleSystemSettings();

            settings.MaxParticles = (int)MaxParticles_Value.Value;

            settings.Duration = TimeSpan.FromSeconds((float)Duration_Value.Value);

            settings.DurationRandomness = (float)DurationRandomness_Value.Value;

            settings.EmitterVelocitySensitivity = (float)EmitterVelocitySensivity_Value.Value;

            settings.MinHorizontalVelocity = (float)MinHorizontalVelocity_Value.Value;
            settings.MaxHorizontalVelocity = (float)MaxHorizontalVelocity_Value.Value;

            settings.MinVerticalVelocity = (float)MinVerticalVelocity_Value.Value;
            settings.MaxVerticalVelocity = (float)MaxVerticalVelocity_Velocity.Value;

            settings.Gravity = new Microsoft.Xna.Framework.Vector3(float.Parse(Gravity_X_Value.Text), float.Parse(Gravity_Y_Value.Text), float.Parse(Gravity_Z_Value.Text));

            settings.EndVelocity = (float)EndVelocity_Value.Value;
            
            settings.MinColor = new Microsoft.Xna.Framework.Color(int.Parse(MinColor_R_Value.Text), int.Parse(MinColor_G_Value.Text), int.Parse(MinColor_B_Value.Text));

            settings.MaxColor = new Microsoft.Xna.Framework.Color(int.Parse(MaxColor_R_Value.Text), int.Parse(MaxColor_G_Value.Text), int.Parse(MaxColor_B_Value.Text));

            settings.MinRotateSpeed = (float)MinRotateSpeed_Value.Value;
            settings.MaxRotateSpeed = (float)MaxRotateSpeed_Value.Value;

            settings.MinStartSize = (float)MinStartSize_Value.Value;
            settings.MaxStartSize = (float)MaxStartSize_Value.Value;

            settings.MinEndSize = (float)MinEndSize_Value.Value;
            settings.MaxEndSize = (float)MaxEndSize_Value.Value;

            settings.Material = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<Material>(Material_Value.Text);

            return settings;
        }

        private void UpdateValues(ParticleSystemSettings settings)
        {
            MaxParticles_Value.Value = settings.MaxParticles;

            Duration_Value.Value = (decimal)settings.Duration.TotalSeconds;

            DurationRandomness_Value.Value = (decimal)settings.DurationRandomness;

            EmitterVelocitySensivity_Value.Value = (decimal)settings.EmitterVelocitySensitivity;

            MinHorizontalVelocity_Value.Value = (decimal)settings.MinHorizontalVelocity;
            MaxHorizontalVelocity_Value.Value = (decimal)settings.MaxHorizontalVelocity;

            MinVerticalVelocity_Value.Value = (decimal)settings.MinVerticalVelocity;
            MaxVerticalVelocity_Velocity.Value = (decimal)settings.MaxVerticalVelocity;

            Gravity_X_Value.Text = settings.Gravity.X.ToString();
            Gravity_Y_Value.Text = settings.Gravity.Y.ToString();
            Gravity_Z_Value.Text = settings.Gravity.Z.ToString();

            EndVelocity_Value.Value = (decimal)settings.EndVelocity;

            MinColor_R_Value.Text = settings.MinColor.R.ToString();
            MinColor_G_Value.Text = settings.MinColor.G.ToString();
            MinColor_B_Value.Text = settings.MinColor.B.ToString();

            MaxColor_R_Value.Text = settings.MaxColor.R.ToString();
            MaxColor_G_Value.Text = settings.MaxColor.G.ToString();
            MaxColor_B_Value.Text = settings.MaxColor.B.ToString();

            MinRotateSpeed_Value.Value = (decimal)settings.MinRotateSpeed;
            MaxRotateSpeed_Value.Value = (decimal)settings.MaxRotateSpeed;

            MinStartSize_Value.Value = (decimal)settings.MinStartSize;
            MaxStartSize_Value.Value = (decimal)settings.MaxStartSize;

            MinEndSize_Value.Value = (decimal)settings.MinEndSize;
            MaxEndSize_Value.Value = (decimal)settings.MaxEndSize;

            Material_Value.Text = settings.Material.Id;
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, int bRevert);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int GetMenuItemCount(IntPtr hMenu);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int DrawMenuBar(IntPtr hWnd);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int
        wFlags);
        private const int MF_BYPOSITION = 0x400;
        private const int MF_REMOVE = 0x1000;
        private void RemoveCloseButton(Form frmForm)
        {
            IntPtr hMenu;
            int n;
            hMenu = GetSystemMenu(frmForm.Handle, 0);
            if (!(hMenu == IntPtr.Zero))
            {
                n = GetMenuItemCount(hMenu);
                if (n > 0)
                {
                    RemoveMenu(hMenu, n - 1, MF_BYPOSITION);
                    RemoveMenu(hMenu, n - 2, MF_BYPOSITION);
                }
                DrawMenuBar(frmForm.Handle);
            }
        }

        private void Apply_Button_Click(object sender, EventArgs e)
        {
            mAction(Settings);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Radgie.Input.Action;
using Radgie.Input.Device.Mouse;
using Radgie.Input.Adapters;
using Microsoft.Xna.Framework;
using Radgie.Input.Device.Gamepad;
using Radgie.Input.Device.Keyboard;

namespace RadgieDevelopmentTestProject.Demos.Input
{
    public class CarController
    {
        private PlayerIndex mPIndex;

        public AnalogicalAction Steer;
        public AnalogicalAction Accelerator;
        public AnalogicalAction Brake;
        public DigitalAction HandBrake;
        public PositionAction Gun;
        

        public CarController(PlayerIndex pIndex)
        {
            mPIndex = pIndex;

            Steer = new AnalogicalAction(0.0f, 0.0f);
            Steer.AddBinding(new AnalogicalDirectionControl2AnalogicalAdapter(Gamepad.Get(mPIndex).LeftThumb, AnalogicalDirectionControl2AnalogicalAdapter.Component.X));
            Steer.AddBinding(new Digital2AnalogicalAdapter(Keyboard.Get(mPIndex).LeftKey, false, -1.0f, 0.0f, 5.0f));
            Steer.AddBinding(new Digital2AnalogicalAdapter(Keyboard.Get(mPIndex).RightKey, true, 0.0f, 1.0f, 5.0f));

            Accelerator = new AnalogicalAction(0.5f, 0.0f);
            Accelerator.AddBinding(Gamepad.Get(mPIndex).RightTrigger);

            Brake = new AnalogicalAction(0.5f, 0.0f);
            Brake.AddBinding(Gamepad.Get(mPIndex).LeftTrigger);

            HandBrake = new DigitalAction();
            HandBrake.AddBinding(Gamepad.Get(mPIndex).AButton);
            HandBrake.AddBinding(Mouse.Get(mPIndex).MiddleButton);

            Gun = new PositionAction();
            Gun.AddBinding(Mouse.Get(mPIndex).Position);
        }

        public override string ToString()
        {
            string res = "";
            res += "Steer: " + Steer.Value + " - " + Steer.PreviousValue + "\n";
            res += "Accelerator: " + Accelerator.Value + " - " + Accelerator.PreviousValue + "\n";
            res += "Brake: " + Brake.Value + " - " + Brake.PreviousValue + "\n";
            res += "HandBrake: " + HandBrake.Pressed + " - " + HandBrake.PreviousValue + "\n";
            res += "Gun: " + Gun.X + "," + Gun.Y + " - " + Gun.PreviousX + "," + Gun.PreviousY + "\n";
            return base.ToString() + "\r" + res;
        }
    }
}

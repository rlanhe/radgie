using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Action;

namespace RadgieDevelopmentTestProject.Demos
{
    public interface IDemoController
    {
        DigitalAction NextDemo { get; }
        DigitalAction PreviousDemo { get; }
        DigitalAction ExitDemo { get; }
    }
}

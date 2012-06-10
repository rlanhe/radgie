using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework.Input;

namespace Radgie.Input.Device.Keyboard
{
    /// <summary>
    /// Interfaz del dispositivo Keyboard
    /// </summary>
    public interface IKeyboard: IDevice
    {
        #region Properties
        #region Keys
        IDigitalControl AKey { get; }
        IDigitalControl AddKey { get; }
        IDigitalControl AppsKey { get; }
        IDigitalControl BKey { get; }
        IDigitalControl BackKey { get; }
        IDigitalControl BrowserBackKey { get; }
        IDigitalControl BrowserFavoritesKey { get; }
        IDigitalControl BrowserForwardKey { get; }
        IDigitalControl BrowserHomeKey { get; }
        IDigitalControl BrowserRefreshKey { get; }
        IDigitalControl BrowserSearchKey { get; }
        IDigitalControl BrowserStopKey { get; }
        IDigitalControl CKey { get; }
        IDigitalControl CapsLockKey { get; }
        IDigitalControl ChatPadGreenKey { get; }
        IDigitalControl ChatPadOrangeKey { get; }
        IDigitalControl CrselKey { get; }
        IDigitalControl DKey { get; }
        IDigitalControl D0Key { get; }
        IDigitalControl D1Key { get; }
        IDigitalControl D2Key { get; }
        IDigitalControl D3Key { get; }
        IDigitalControl D4Key { get; }
        IDigitalControl D5Key { get; }
        IDigitalControl D6Key { get; }
        IDigitalControl D7Key { get; }
        IDigitalControl D8Key { get; }
        IDigitalControl D9Key { get; }
        IDigitalControl DecimalKey { get; }
        IDigitalControl DeleteKey { get; }
        IDigitalControl DivideKey { get; }
        IDigitalControl DownKey { get; }
        IDigitalControl EKey { get; }
        IDigitalControl EndKey { get; }
        IDigitalControl EnterKey { get; }
        IDigitalControl EraseEofKey { get; }
        IDigitalControl EscapeKey { get; }
        IDigitalControl ExecuteKey { get; }
        IDigitalControl ExselKey { get; }
        IDigitalControl FKey { get; }
        IDigitalControl F1Key { get; }
        IDigitalControl F10Key { get; }
        IDigitalControl F11Key { get; }
        IDigitalControl F12Key { get; }
        IDigitalControl F13Key { get; }
        IDigitalControl F14Key { get; }
        IDigitalControl F15Key { get; }
        IDigitalControl F16Key { get; }
        IDigitalControl F17Key { get; }
        IDigitalControl F18Key { get; }
        IDigitalControl F19Key { get; }
        IDigitalControl F2Key { get; }
        IDigitalControl F20Key { get; }
        IDigitalControl F21Key { get; }
        IDigitalControl F22Key { get; }
        IDigitalControl F23Key { get; }
        IDigitalControl F24Key { get; }
        IDigitalControl F3Key { get; }
        IDigitalControl F4Key { get; }
        IDigitalControl F5Key { get; }
        IDigitalControl F6Key { get; }
        IDigitalControl F7Key { get; }
        IDigitalControl F8Key { get; }
        IDigitalControl F9Key { get; }
        IDigitalControl GKey { get; }
        IDigitalControl HKey { get; }
        IDigitalControl HelpKey { get; }
        IDigitalControl HomeKey { get; }
        IDigitalControl IKey { get; }
        IDigitalControl ImeConvertKey { get; }
        IDigitalControl ImeNoConvertKey { get; }
        IDigitalControl InsertKey { get; }
        IDigitalControl JKey { get; }
        IDigitalControl KKey { get; }
        IDigitalControl KanaKey { get; }
        IDigitalControl KanjiKey { get; }
        IDigitalControl LKey { get; }
        IDigitalControl LaunchApplication1Key { get; }
        IDigitalControl LaunchApplication2Key { get; }
        IDigitalControl LaunchMailKey { get; }
        IDigitalControl LeftKey { get; }
        IDigitalControl LeftAltKey { get; }
        IDigitalControl LeftControlKey { get; }
        IDigitalControl LeftShiftKey { get; }
        IDigitalControl LeftWindowsKey { get; }
        IDigitalControl MKey { get; }
        IDigitalControl MediaNextTrackKey { get; }
        IDigitalControl MediaPlayPauseKey { get; }
        IDigitalControl MediaPreviousTrackKey { get; }
        IDigitalControl MediaStopKey { get; }
        IDigitalControl MultiplyKey { get; }
        IDigitalControl NKey { get; }
        IDigitalControl NoneKey { get; }
        IDigitalControl NumLockKey { get; }
        IDigitalControl NumPad0Key { get; }
        IDigitalControl NumPad1Key { get; }
        IDigitalControl NumPad2Key { get; }
        IDigitalControl NumPad3Key { get; }
        IDigitalControl NumPad4Key { get; }
        IDigitalControl NumPad5Key { get; }
        IDigitalControl NumPad6Key { get; }
        IDigitalControl NumPad7Key { get; }
        IDigitalControl NumPad8Key { get; }
        IDigitalControl NumPad9Key { get; }
        IDigitalControl OKey { get; }
        IDigitalControl Oem8Key { get; }
        IDigitalControl OemAutoKey { get; }
        IDigitalControl OemBackslashKey { get; }
        IDigitalControl OemClearKey { get; }
        IDigitalControl OemCloseBracketsKey { get; }
        IDigitalControl OemCommaKey { get; }
        IDigitalControl OemCopyKey { get; }
        IDigitalControl OemEnlWKey { get; }
        IDigitalControl OemMinusKey { get; }
        IDigitalControl OemOpenBracketsKey { get; }
        IDigitalControl OemPeriodKey { get; }
        IDigitalControl OemPipeKey { get; }
        IDigitalControl OemPlusKey { get; }
        IDigitalControl OemQuestionKey { get; }
        IDigitalControl OemQuotesKey { get; }
        IDigitalControl OemSemicolonKey { get; }
        IDigitalControl OemTildeKey { get; }
        IDigitalControl PKey { get; }
        IDigitalControl Pa1Key { get; }
        IDigitalControl PageDownKey { get; }
        IDigitalControl PageUpKey { get; }
        IDigitalControl PauseKey { get; }
        IDigitalControl PlayKey { get; }
        IDigitalControl PrintKey { get; }
        IDigitalControl PrintScreenKey { get; }
        IDigitalControl ProcessKeyKey { get; }
        IDigitalControl QKey { get; }
        IDigitalControl RKey { get; }
        IDigitalControl RightKey { get; }
        IDigitalControl RightAltKey { get; }
        IDigitalControl RightControlKey { get; }
        IDigitalControl RightShiftKey { get; }
        IDigitalControl RightWindowsKey { get; }
        IDigitalControl SKey { get; }
        IDigitalControl ScrollKey { get; }
        IDigitalControl SelectKey { get; }
        IDigitalControl SelectMediaKey { get; }
        IDigitalControl SeparatorKey { get; }
        IDigitalControl SleepKey { get; }
        IDigitalControl SpaceKey { get; }
        IDigitalControl SubtractKey { get; }
        IDigitalControl TKey { get; }
        IDigitalControl TabKey { get; }
        IDigitalControl UKey { get; }
        IDigitalControl UpKey { get; }
        IDigitalControl VKey { get; }
        IDigitalControl VolumeDownKey { get; }
        IDigitalControl VolumeMuteKey { get; }
        IDigitalControl VolumeUpKey { get; }
        IDigitalControl WKey { get; }
        IDigitalControl XKey { get; }
        IDigitalControl YKey { get; }
        IDigitalControl ZKey { get; }
        IDigitalControl ZoomKey { get; }
        #endregion

        /// <summary>
        /// Estado del control en el frame actual.
        /// </summary>
        KeyboardState State { get; }
        /// <summary>
        /// Estado del control en el frame anterior.
        /// </summary>
        KeyboardState PreviousState { get; }
        #endregion
    }
}

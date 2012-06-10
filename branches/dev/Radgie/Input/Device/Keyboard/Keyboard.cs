
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Input.Control;
using Microsoft.Xna.Framework.Input;
using System.Xml;

namespace Radgie.Input.Device.Keyboard
{
    /// <summary>
    /// Dispositivo Keyboard.
    /// Aunque para Radgie existe un teclado por jugador, solo existe un teclado que es compartido por todos los jugadores.
    /// </summary>
    public class Keyboard : ADevice<IKeyboard>, IKeyboard
    {
        #region Properties
        #region IKeyboard members
        #region Keys

        public IDigitalControl AKey
        {
            get
            {
                return mAKey;
            }
        }
        protected KeyboardKey mAKey;

        public IDigitalControl AddKey
        {
            get
            {
                return mAddKey;
            }
        }
        protected KeyboardKey mAddKey;

        public IDigitalControl AppsKey
        {
            get
            {
                return mAppsKey;
            }
        }
        protected KeyboardKey mAppsKey;

        public IDigitalControl AttnKey
        {
            get
            {
                return mAttnKey;
            }
        }
        protected KeyboardKey mAttnKey;

        public IDigitalControl BKey
        {
            get
            {
                return mBKey;
            }
        }
        protected KeyboardKey mBKey;

        public IDigitalControl BackKey
        {
            get
            {
                return mBackKey;
            }
        }
        protected KeyboardKey mBackKey;

        public IDigitalControl BrowserBackKey
        {
            get
            {
                return mBrowserBackKey;
            }
        }
        protected KeyboardKey mBrowserBackKey;

        public IDigitalControl BrowserFavoritesKey
        {
            get
            {
                return mBrowserFavoritesKey;
            }
        }
        protected KeyboardKey mBrowserFavoritesKey;

        public IDigitalControl BrowserForwardKey
        {
            get
            {
                return mBrowserForwardKey;
            }
        }
        protected KeyboardKey mBrowserForwardKey;

        public IDigitalControl BrowserHomeKey
        {
            get
            {
                return mBrowserHomeKey;
            }
        }
        protected KeyboardKey mBrowserHomeKey;

        public IDigitalControl BrowserRefreshKey
        {
            get
            {
                return mBrowserRefreshKey;
            }
        }
        protected KeyboardKey mBrowserRefreshKey;

        public IDigitalControl BrowserSearchKey
        {
            get
            {
                return mBrowserSearchKey;
            }
        }
        protected KeyboardKey mBrowserSearchKey;

        public IDigitalControl BrowserStopKey
        {
            get
            {
                return mBrowserStopKey;
            }
        }
        protected KeyboardKey mBrowserStopKey;

        public IDigitalControl CKey
        {
            get
            {
                return mCKey;
            }
        }
        protected KeyboardKey mCKey;

        public IDigitalControl CapsLockKey
        {
            get
            {
                return mCapsLockKey;
            }
        }
        protected KeyboardKey mCapsLockKey;

        public IDigitalControl ChatPadGreenKey
        {
            get
            {
                return mChatPadGreenKey;
            }
        }
        protected KeyboardKey mChatPadGreenKey;

        public IDigitalControl ChatPadOrangeKey
        {
            get
            {
                return mChatPadOrangeKey;
            }
        }
        protected KeyboardKey mChatPadOrangeKey;

        public IDigitalControl CrselKey
        {
            get
            {
                return mCrselKey;
            }
        }
        protected KeyboardKey mCrselKey;

        public IDigitalControl DKey
        {
            get
            {
                return mDKey;
            }
        }
        protected KeyboardKey mDKey;

        public IDigitalControl D0Key
        {
            get
            {
                return mD0Key;
            }
        }
        protected KeyboardKey mD0Key;

        public IDigitalControl D1Key
        {
            get
            {
                return mD1Key;
            }
        }
        protected KeyboardKey mD1Key;

        public IDigitalControl D2Key
        {
            get
            {
                return mD2Key;
            }
        }
        protected KeyboardKey mD2Key;

        public IDigitalControl D3Key
        {
            get
            {
                return mD3Key;
            }
        }
        protected KeyboardKey mD3Key;

        public IDigitalControl D4Key
        {
            get
            {
                return mD4Key;
            }
        }
        protected KeyboardKey mD4Key;

        public IDigitalControl D5Key
        {
            get
            {
                return mD5Key;
            }
        }
        protected KeyboardKey mD5Key;

        public IDigitalControl D6Key
        {
            get
            {
                return mD6Key;
            }
        }
        protected KeyboardKey mD6Key;

        public IDigitalControl D7Key
        {
            get
            {
                return mD7Key;
            }
        }
        protected KeyboardKey mD7Key;

        public IDigitalControl D8Key
        {
            get
            {
                return mD8Key;
            }
        }
        protected KeyboardKey mD8Key;

        public IDigitalControl D9Key
        {
            get
            {
                return mD9Key;
            }
        }
        protected KeyboardKey mD9Key;

        public IDigitalControl DecimalKey
        {
            get
            {
                return mDecimalKey;
            }
        }
        protected KeyboardKey mDecimalKey;

        public IDigitalControl DeleteKey
        {
            get
            {
                return mDeleteKey;
            }
        }
        protected KeyboardKey mDeleteKey;

        public IDigitalControl DivideKey
        {
            get
            {
                return mDivideKey;
            }
        }
        protected KeyboardKey mDivideKey;

        public IDigitalControl DownKey
        {
            get
            {
                return mDownKey;
            }
        }
        protected KeyboardKey mDownKey;

        public IDigitalControl EKey
        {
            get
            {
                return mEKey;
            }
        }
        protected KeyboardKey mEKey;

        public IDigitalControl EndKey
        {
            get
            {
                return mEndKey;
            }
        }
        protected KeyboardKey mEndKey;

        public IDigitalControl EnterKey
        {
            get
            {
                return mEnterKey;
            }
        }
        protected KeyboardKey mEnterKey;

        public IDigitalControl EraseEofKey
        {
            get
            {
                return mEraseEofKey;
            }
        }
        protected KeyboardKey mEraseEofKey;

        public IDigitalControl EscapeKey
        {
            get
            {
                return mEscapeKey;
            }
        }
        protected KeyboardKey mEscapeKey;

        public IDigitalControl ExecuteKey
        {
            get
            {
                return mExecuteKey;
            }
        }
        protected KeyboardKey mExecuteKey;

        public IDigitalControl ExselKey
        {
            get
            {
                return mExselKey;
            }
        }
        protected KeyboardKey mExselKey;

        public IDigitalControl FKey
        {
            get
            {
                return mFKey;
            }
        }
        protected KeyboardKey mFKey;

        public IDigitalControl F1Key
        {
            get
            {
                return mF1Key;
            }
        }
        protected KeyboardKey mF1Key;

        public IDigitalControl F10Key
        {
            get
            {
                return mF10Key;
            }
        }
        protected KeyboardKey mF10Key;

        public IDigitalControl F11Key
        {
            get
            {
                return mF11Key;
            }
        }
        protected KeyboardKey mF11Key;

        public IDigitalControl F12Key
        {
            get
            {
                return mF12Key;
            }
        }
        protected KeyboardKey mF12Key;

        public IDigitalControl F13Key
        {
            get
            {
                return mF13Key;
            }
        }
        protected KeyboardKey mF13Key;

        public IDigitalControl F14Key
        {
            get
            {
                return mF14Key;
            }
        }
        protected KeyboardKey mF14Key;

        public IDigitalControl F15Key
        {
            get
            {
                return mF15Key;
            }
        }
        protected KeyboardKey mF15Key;

        public IDigitalControl F16Key
        {
            get
            {
                return mF16Key;
            }
        }
        protected KeyboardKey mF16Key;

        public IDigitalControl F17Key
        {
            get
            {
                return mF17Key;
            }
        }
        protected KeyboardKey mF17Key;

        public IDigitalControl F18Key
        {
            get
            {
                return mF18Key;
            }
        }
        protected KeyboardKey mF18Key;

        public IDigitalControl F19Key
        {
            get
            {
                return mF19Key;
            }
        }
        protected KeyboardKey mF19Key;

        public IDigitalControl F2Key
        {
            get
            {
                return mF2Key;
            }
        }
        protected KeyboardKey mF2Key;

        public IDigitalControl F20Key
        {
            get
            {
                return mF20Key;
            }
        }
        protected KeyboardKey mF20Key;

        public IDigitalControl F21Key
        {
            get
            {
                return mF21Key;
            }
        }
        protected KeyboardKey mF21Key;

        public IDigitalControl F22Key
        {
            get
            {
                return mF22Key;
            }
        }
        protected KeyboardKey mF22Key;

        public IDigitalControl F23Key
        {
            get
            {
                return mF23Key;
            }
        }
        protected KeyboardKey mF23Key;

        public IDigitalControl F24Key
        {
            get
            {
                return mF24Key;
            }
        }
        protected KeyboardKey mF24Key;

        public IDigitalControl F3Key
        {
            get
            {
                return mF3Key;
            }
        }
        protected KeyboardKey mF3Key;

        public IDigitalControl F4Key
        {
            get
            {
                return mF4Key;
            }
        }
        protected KeyboardKey mF4Key;

        public IDigitalControl F5Key
        {
            get
            {
                return mF5Key;
            }
        }
        protected KeyboardKey mF5Key;

        public IDigitalControl F6Key
        {
            get
            {
                return mF6Key;
            }
        }
        protected KeyboardKey mF6Key;

        public IDigitalControl F7Key
        {
            get
            {
                return mF7Key;
            }
        }
        protected KeyboardKey mF7Key;

        public IDigitalControl F8Key
        {
            get
            {
                return mF8Key;
            }
        }
        protected KeyboardKey mF8Key;

        public IDigitalControl F9Key
        {
            get
            {
                return mF9Key;
            }
        }
        protected KeyboardKey mF9Key;

        public IDigitalControl GKey
        {
            get
            {
                return mGKey;
            }
        }
        protected KeyboardKey mGKey;

        public IDigitalControl HKey
        {
            get
            {
                return mHKey;
            }
        }
        protected KeyboardKey mHKey;

        public IDigitalControl HelpKey
        {
            get
            {
                return mHelpKey;
            }
        }
        protected KeyboardKey mHelpKey;

        public IDigitalControl HomeKey
        {
            get
            {
                return mHomeKey;
            }
        }
        protected KeyboardKey mHomeKey;

        public IDigitalControl IKey
        {
            get
            {
                return mIKey;
            }
        }
        protected KeyboardKey mIKey;

        public IDigitalControl ImeConvertKey
        {
            get
            {
                return mImeConvertKey;
            }
        }
        protected KeyboardKey mImeConvertKey;

        public IDigitalControl ImeNoConvertKey
        {
            get
            {
                return mImeNoConvertKey;
            }
        }
        protected KeyboardKey mImeNoConvertKey;

        public IDigitalControl InsertKey
        {
            get
            {
                return mInsertKey;
            }
        }
        protected KeyboardKey mInsertKey;

        public IDigitalControl JKey
        {
            get
            {
                return mJKey;
            }
        }
        protected KeyboardKey mJKey;

        public IDigitalControl KKey
        {
            get
            {
                return mKKey;
            }
        }
        protected KeyboardKey mKKey;

        public IDigitalControl KanaKey
        {
            get
            {
                return mKanaKey;
            }
        }
        protected KeyboardKey mKanaKey;

        public IDigitalControl KanjiKey
        {
            get
            {
                return mKanjiKey;
            }
        }
        protected KeyboardKey mKanjiKey;

        public IDigitalControl LKey
        {
            get
            {
                return mLKey;
            }
        }
        protected KeyboardKey mLKey;

        public IDigitalControl LaunchApplication1Key
        {
            get
            {
                return mLaunchApplication1Key;
            }
        }
        protected KeyboardKey mLaunchApplication1Key;

        public IDigitalControl LaunchApplication2Key
        {
            get
            {
                return mLaunchApplication2Key;
            }
        }
        protected KeyboardKey mLaunchApplication2Key;

        public IDigitalControl LaunchMailKey
        {
            get
            {
                return mLaunchMailKey;
            }
        }
        protected KeyboardKey mLaunchMailKey;

        public IDigitalControl LeftKey
        {
            get
            {
                return mLeftKey;
            }
        }
        protected KeyboardKey mLeftKey;

        public IDigitalControl LeftAltKey
        {
            get
            {
                return mLeftAltKey;
            }
        }
        protected KeyboardKey mLeftAltKey;

        public IDigitalControl LeftControlKey
        {
            get
            {
                return mLeftControlKey;
            }
        }
        protected KeyboardKey mLeftControlKey;

        public IDigitalControl LeftShiftKey
        {
            get
            {
                return mLeftShiftKey;
            }
        }
        protected KeyboardKey mLeftShiftKey;

        public IDigitalControl LeftWindowsKey
        {
            get
            {
                return mLeftWindowsKey;
            }
        }
        protected KeyboardKey mLeftWindowsKey;

        public IDigitalControl MKey
        {
            get
            {
                return mMKey;
            }
        }
        protected KeyboardKey mMKey;

        public IDigitalControl MediaNextTrackKey
        {
            get
            {
                return mMediaNextTrackKey;
            }
        }
        protected KeyboardKey mMediaNextTrackKey;

        public IDigitalControl MediaPlayPauseKey
        {
            get
            {
                return mMediaPlayPauseKey;
            }
        }
        protected KeyboardKey mMediaPlayPauseKey;

        public IDigitalControl MediaPreviousTrackKey
        {
            get
            {
                return mMediaPreviousTrackKey;
            }
        }
        protected KeyboardKey mMediaPreviousTrackKey;

        public IDigitalControl MediaStopKey
        {
            get
            {
                return mMediaStopKey;
            }
        }
        protected KeyboardKey mMediaStopKey;

        public IDigitalControl MultiplyKey
        {
            get
            {
                return mMultiplyKey;
            }
        }
        protected KeyboardKey mMultiplyKey;

        public IDigitalControl NKey
        {
            get
            {
                return mNKey;
            }
        }
        protected KeyboardKey mNKey;

        public IDigitalControl NoneKey
        {
            get
            {
                return mNoneKey;
            }
        }
        protected KeyboardKey mNoneKey;

        public IDigitalControl NumLockKey
        {
            get
            {
                return mNumLockKey;
            }
        }
        protected KeyboardKey mNumLockKey;

        public IDigitalControl NumPad0Key
        {
            get
            {
                return mNumPad0Key;
            }
        }
        protected KeyboardKey mNumPad0Key;

        public IDigitalControl NumPad1Key
        {
            get
            {
                return mNumPad1Key;
            }
        }
        protected KeyboardKey mNumPad1Key;

        public IDigitalControl NumPad2Key
        {
            get
            {
                return mNumPad2Key;
            }
        }
        protected KeyboardKey mNumPad2Key;

        public IDigitalControl NumPad3Key
        {
            get
            {
                return mNumPad3Key;
            }
        }
        protected KeyboardKey mNumPad3Key;

        public IDigitalControl NumPad4Key
        {
            get
            {
                return mNumPad4Key;
            }
        }
        protected KeyboardKey mNumPad4Key;

        public IDigitalControl NumPad5Key
        {
            get
            {
                return mNumPad5Key;
            }
        }
        protected KeyboardKey mNumPad5Key;

        public IDigitalControl NumPad6Key
        {
            get
            {
                return mNumPad6Key;
            }
        }
        protected KeyboardKey mNumPad6Key;

        public IDigitalControl NumPad7Key
        {
            get
            {
                return mNumPad7Key;
            }
        }
        protected KeyboardKey mNumPad7Key;

        public IDigitalControl NumPad8Key
        {
            get
            {
                return mNumPad8Key;
            }
        }
        protected KeyboardKey mNumPad8Key;

        public IDigitalControl NumPad9Key
        {
            get
            {
                return mNumPad9Key;
            }
        }
        protected KeyboardKey mNumPad9Key;

        public IDigitalControl OKey
        {
            get
            {
                return mOKey;
            }
        }
        protected KeyboardKey mOKey;

        public IDigitalControl Oem8Key
        {
            get
            {
                return mOem8Key;
            }
        }
        protected KeyboardKey mOem8Key;

        public IDigitalControl OemAutoKey
        {
            get
            {
                return mOemAutoKey;
            }
        }
        protected KeyboardKey mOemAutoKey;

        public IDigitalControl OemBackslashKey
        {
            get
            {
                return mOemBackslashKey;
            }
        }
        protected KeyboardKey mOemBackslashKey;

        public IDigitalControl OemClearKey
        {
            get
            {
                return mOemClearKey;
            }
        }
        protected KeyboardKey mOemClearKey;

        public IDigitalControl OemCloseBracketsKey
        {
            get
            {
                return mOemCloseBracketsKey;
            }
        }
        protected KeyboardKey mOemCloseBracketsKey;

        public IDigitalControl OemCommaKey
        {
            get
            {
                return mOemCommaKey;
            }
        }
        protected KeyboardKey mOemCommaKey;

        public IDigitalControl OemCopyKey
        {
            get
            {
                return mOemCopyKey;
            }
        }
        protected KeyboardKey mOemCopyKey;

        public IDigitalControl OemEnlWKey
        {
            get
            {
                return mOemEnlWKey;
            }
        }
        protected KeyboardKey mOemEnlWKey;

        public IDigitalControl OemMinusKey
        {
            get
            {
                return mOemMinusKey;
            }
        }
        protected KeyboardKey mOemMinusKey;

        public IDigitalControl OemOpenBracketsKey
        {
            get
            {
                return mOemOpenBracketsKey;
            }
        }
        protected KeyboardKey mOemOpenBracketsKey;

        public IDigitalControl OemPeriodKey
        {
            get
            {
                return mOemPeriodKey;
            }
        }
        protected KeyboardKey mOemPeriodKey;

        public IDigitalControl OemPipeKey
        {
            get
            {
                return mOemPipeKey;
            }
        }
        protected KeyboardKey mOemPipeKey;

        public IDigitalControl OemPlusKey
        {
            get
            {
                return mOemPlusKey;
            }
        }
        protected KeyboardKey mOemPlusKey;

        public IDigitalControl OemQuestionKey
        {
            get
            {
                return mOemQuestionKey;
            }
        }
        protected KeyboardKey mOemQuestionKey;

        public IDigitalControl OemQuotesKey
        {
            get
            {
                return mOemQuotesKey;
            }
        }
        protected KeyboardKey mOemQuotesKey;

        public IDigitalControl OemSemicolonKey
        {
            get
            {
                return mOemSemicolonKey;
            }
        }
        protected KeyboardKey mOemSemicolonKey;

        public IDigitalControl OemTildeKey
        {
            get
            {
                return mOemTildeKey;
            }
        }
        protected KeyboardKey mOemTildeKey;

        public IDigitalControl PKey
        {
            get
            {
                return mPKey;
            }
        }
        protected KeyboardKey mPKey;

        public IDigitalControl Pa1Key
        {
            get
            {
                return mPa1Key;
            }
        }
        protected KeyboardKey mPa1Key;

        public IDigitalControl PageDownKey
        {
            get
            {
                return mPageDownKey;
            }
        }
        protected KeyboardKey mPageDownKey;

        public IDigitalControl PageUpKey
        {
            get
            {
                return mPageUpKey;
            }
        }
        protected KeyboardKey mPageUpKey;

        public IDigitalControl PauseKey
        {
            get
            {
                return mPauseKey;
            }
        }
        protected KeyboardKey mPauseKey;

        public IDigitalControl PlayKey
        {
            get
            {
                return mPlayKey;
            }
        }
        protected KeyboardKey mPlayKey;

        public IDigitalControl PrintKey
        {
            get
            {
                return mPrintKey;
            }
        }
        protected KeyboardKey mPrintKey;

        public IDigitalControl PrintScreenKey
        {
            get
            {
                return mPrintScreenKey;
            }
        }
        protected KeyboardKey mPrintScreenKey;

        public IDigitalControl ProcessKeyKey
        {
            get
            {
                return mProcessKeyKey;
            }
        }
        protected KeyboardKey mProcessKeyKey;

        public IDigitalControl QKey
        {
            get
            {
                return mQKey;
            }
        }
        protected KeyboardKey mQKey;

        public IDigitalControl RKey
        {
            get
            {
                return mRKey;
            }
        }
        protected KeyboardKey mRKey;

        public IDigitalControl RightKey
        {
            get
            {
                return mRightKey;
            }
        }
        protected KeyboardKey mRightKey;

        public IDigitalControl RightAltKey
        {
            get
            {
                return mRightAltKey;
            }
        }
        protected KeyboardKey mRightAltKey;

        public IDigitalControl RightControlKey
        {
            get
            {
                return mRightControlKey;
            }
        }
        protected KeyboardKey mRightControlKey;

        public IDigitalControl RightShiftKey
        {
            get
            {
                return mRightShiftKey;
            }
        }
        protected KeyboardKey mRightShiftKey;

        public IDigitalControl RightWindowsKey
        {
            get
            {
                return mRightWindowsKey;
            }
        }
        protected KeyboardKey mRightWindowsKey;

        public IDigitalControl SKey
        {
            get
            {
                return mSKey;
            }
        }
        protected KeyboardKey mSKey;

        public IDigitalControl ScrollKey
        {
            get
            {
                return mScrollKey;
            }
        }
        protected KeyboardKey mScrollKey;

        public IDigitalControl SelectKey
        {
            get
            {
                return mSelectKey;
            }
        }
        protected KeyboardKey mSelectKey;

        public IDigitalControl SelectMediaKey
        {
            get
            {
                return mSelectMediaKey;
            }
        }
        protected KeyboardKey mSelectMediaKey;

        public IDigitalControl SeparatorKey
        {
            get
            {
                return mSeparatorKey;
            }
        }
        protected KeyboardKey mSeparatorKey;

        public IDigitalControl SleepKey
        {
            get
            {
                return mSleepKey;
            }
        }
        protected KeyboardKey mSleepKey;

        public IDigitalControl SpaceKey
        {
            get
            {
                return mSpaceKey;
            }
        }
        protected KeyboardKey mSpaceKey;

        public IDigitalControl SubtractKey
        {
            get
            {
                return mSubtractKey;
            }
        }
        protected KeyboardKey mSubtractKey;

        public IDigitalControl TKey
        {
            get
            {
                return mTKey;
            }
        }
        protected KeyboardKey mTKey;

        public IDigitalControl TabKey
        {
            get
            {
                return mTabKey;
            }
        }
        protected KeyboardKey mTabKey;

        public IDigitalControl UKey
        {
            get
            {
                return mUKey;
            }
        }
        protected KeyboardKey mUKey;

        public IDigitalControl UpKey
        {
            get
            {
                return mUpKey;
            }
        }
        protected KeyboardKey mUpKey;

        public IDigitalControl VKey
        {
            get
            {
                return mVKey;
            }
        }
        protected KeyboardKey mVKey;

        public IDigitalControl VolumeDownKey
        {
            get
            {
                return mVolumeDownKey;
            }
        }
        protected KeyboardKey mVolumeDownKey;

        public IDigitalControl VolumeMuteKey
        {
            get
            {
                return mVolumeMuteKey;
            }
        }
        protected KeyboardKey mVolumeMuteKey;

        public IDigitalControl VolumeUpKey
        {
            get
            {
                return mVolumeUpKey;
            }
        }
        protected KeyboardKey mVolumeUpKey;

        public IDigitalControl WKey
        {
            get
            {
                return mWKey;
            }
        }
        protected KeyboardKey mWKey;

        public IDigitalControl XKey
        {
            get
            {
                return mXKey;
            }
        }
        protected KeyboardKey mXKey;

        public IDigitalControl YKey
        {
            get
            {
                return mYKey;
            }
        }
        protected KeyboardKey mYKey;

        public IDigitalControl ZKey
        {
            get
            {
                return mZKey;
            }
        }
        protected KeyboardKey mZKey;

        public IDigitalControl ZoomKey
        {
            get
            {
                return mZoomKey;
            }
        }
        protected KeyboardKey mZoomKey;

        #endregion

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IKeyboard.State"/>
        /// </summary>
        public KeyboardState State
        {
            get
            {
                return mState;
            }
        }
        private KeyboardState mState;

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IKeyboard.PreviousState"/>
        /// </summary>
        public KeyboardState PreviousState
        {
            get
            {
                return mPreviousState;
            }
        }
        private KeyboardState mPreviousState;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa un objeto Keyboard.
        /// </summary>
        /// <param name="index">Id del jugador con el que esta asociado.</param>
        public Keyboard(PlayerIndex index)
            : base(index)
        {
            AddDevice(index, this);

            #region Keys Mapping
            mControls.Add(mAKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.A); }));
            mControls.Add(mAddKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Add); }));
            mControls.Add(mAppsKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Apps); }));
            mControls.Add(mAttnKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Attn); }));
            mControls.Add(mBKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.B); }));
            mControls.Add(mBackKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Back); }));
            mControls.Add(mBrowserBackKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.BrowserBack); }));
            mControls.Add(mBrowserFavoritesKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.BrowserFavorites); }));
            mControls.Add(mBrowserForwardKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.BrowserForward); }));
            mControls.Add(mBrowserHomeKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.BrowserHome); }));
            mControls.Add(mBrowserRefreshKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.BrowserRefresh); }));
            mControls.Add(mBrowserSearchKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.BrowserSearch); }));
            mControls.Add(mBrowserStopKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.BrowserStop); }));
            mControls.Add(mCKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.C); }));
            mControls.Add(mCapsLockKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.CapsLock); }));
            mControls.Add(mChatPadGreenKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.ChatPadGreen); }));
            mControls.Add(mChatPadOrangeKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.ChatPadOrange); }));
            mControls.Add(mCrselKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Crsel); }));
            mControls.Add(mDKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.D); }));
            mControls.Add(mD0Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.D0); }));
            mControls.Add(mD1Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.D1); }));
            mControls.Add(mD2Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.D2); }));
            mControls.Add(mD3Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.D3); }));
            mControls.Add(mD4Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.D4); }));
            mControls.Add(mD5Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.D5); }));
            mControls.Add(mD6Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.D6); }));
            mControls.Add(mD7Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.D7); }));
            mControls.Add(mD8Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.D8); }));
            mControls.Add(mD9Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.D9); }));
            mControls.Add(mDecimalKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Decimal); }));
            mControls.Add(mDeleteKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Delete); }));
            mControls.Add(mDivideKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Divide); }));
            mControls.Add(mDownKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Down); }));
            mControls.Add(mEKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.E); }));
            mControls.Add(mEndKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.End); }));
            mControls.Add(mEnterKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Enter); }));
            mControls.Add(mEraseEofKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.EraseEof); }));
            mControls.Add(mEscapeKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Escape); }));
            mControls.Add(mExecuteKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Execute); }));
            mControls.Add(mExselKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Exsel); }));
            mControls.Add(mFKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F); }));
            mControls.Add(mF1Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F1); }));
            mControls.Add(mF10Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F10); }));
            mControls.Add(mF11Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F11); }));
            mControls.Add(mF12Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F12); }));
            mControls.Add(mF13Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F13); }));
            mControls.Add(mF14Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F14); }));
            mControls.Add(mF15Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F15); }));
            mControls.Add(mF16Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F16); }));
            mControls.Add(mF17Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F17); }));
            mControls.Add(mF18Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F18); }));
            mControls.Add(mF19Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F19); }));
            mControls.Add(mF2Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F2); }));
            mControls.Add(mF20Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F20); }));
            mControls.Add(mF21Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F21); }));
            mControls.Add(mF22Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F22); }));
            mControls.Add(mF23Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F23); }));
            mControls.Add(mF24Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F24); }));
            mControls.Add(mF3Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F3); }));
            mControls.Add(mF4Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F4); }));
            mControls.Add(mF5Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F5); }));
            mControls.Add(mF6Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F6); }));
            mControls.Add(mF7Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F7); }));
            mControls.Add(mF8Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F8); }));
            mControls.Add(mF9Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.F9); }));
            mControls.Add(mGKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.G); }));
            mControls.Add(mHKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.H); }));
            mControls.Add(mHelpKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Help); }));
            mControls.Add(mHomeKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Home); }));
            mControls.Add(mIKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.I); }));
            mControls.Add(mImeConvertKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.ImeConvert); }));
            mControls.Add(mImeNoConvertKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.ImeNoConvert); }));
            mControls.Add(mInsertKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Insert); }));
            mControls.Add(mJKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.J); }));
            mControls.Add(mKKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.K); }));
            mControls.Add(mKanaKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Kana); }));
            mControls.Add(mKanjiKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Kanji); }));
            mControls.Add(mLKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.L); }));
            mControls.Add(mLaunchApplication1Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.LaunchApplication1); }));
            mControls.Add(mLaunchApplication2Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.LaunchApplication2); }));
            mControls.Add(mLaunchMailKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.LaunchMail); }));
            mControls.Add(mLeftKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Left); }));
            mControls.Add(mLeftAltKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.LeftAlt); }));
            mControls.Add(mLeftControlKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.LeftControl); }));
            mControls.Add(mLeftShiftKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.LeftShift); }));
            mControls.Add(mLeftWindowsKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.LeftWindows); }));
            mControls.Add(mMKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.M); }));
            mControls.Add(mMediaNextTrackKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.MediaNextTrack); }));
            mControls.Add(mMediaPlayPauseKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.MediaPlayPause); }));
            mControls.Add(mMediaPreviousTrackKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.MediaPreviousTrack); }));
            mControls.Add(mMediaStopKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.MediaStop); }));
            mControls.Add(mMultiplyKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Multiply); }));
            mControls.Add(mNKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.N); }));
            mControls.Add(mNoneKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.None); }));
            mControls.Add(mNumLockKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.NumLock); }));
            mControls.Add(mNumPad0Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.NumPad0); }));
            mControls.Add(mNumPad1Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.NumPad1); }));
            mControls.Add(mNumPad2Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.NumPad2); }));
            mControls.Add(mNumPad3Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.NumPad3); }));
            mControls.Add(mNumPad4Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.NumPad4); }));
            mControls.Add(mNumPad5Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.NumPad5); }));
            mControls.Add(mNumPad6Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.NumPad6); }));
            mControls.Add(mNumPad7Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.NumPad7); }));
            mControls.Add(mNumPad8Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.NumPad8); }));
            mControls.Add(mNumPad9Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.NumPad9); }));
            mControls.Add(mOKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.O); }));
            mControls.Add(mOem8Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Oem8); }));
            mControls.Add(mOemAutoKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemAuto); }));
            mControls.Add(mOemBackslashKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemBackslash); }));
            mControls.Add(mOemClearKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemClear); }));
            mControls.Add(mOemCloseBracketsKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemCloseBrackets); }));
            mControls.Add(mOemCommaKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemComma); }));
            mControls.Add(mOemCopyKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemCopy); }));
            mControls.Add(mOemEnlWKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemEnlW); }));
            mControls.Add(mOemMinusKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemMinus); }));
            mControls.Add(mOemOpenBracketsKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemOpenBrackets); }));
            mControls.Add(mOemPeriodKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemPeriod); }));
            mControls.Add(mOemPipeKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemPipe); }));
            mControls.Add(mOemPlusKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemPlus); }));
            mControls.Add(mOemQuestionKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemQuestion); }));
            mControls.Add(mOemQuotesKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemQuotes); }));
            mControls.Add(mOemSemicolonKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemSemicolon); }));
            mControls.Add(mOemTildeKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.OemTilde); }));
            mControls.Add(mPKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.P); }));
            mControls.Add(mPa1Key = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Pa1); }));
            mControls.Add(mPageDownKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.PageDown); }));
            mControls.Add(mPageUpKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.PageUp); }));
            mControls.Add(mPauseKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Pause); }));
            mControls.Add(mPlayKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Play); }));
            mControls.Add(mPrintKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Print); }));
            mControls.Add(mPrintScreenKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.PrintScreen); }));
            mControls.Add(mProcessKeyKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.ProcessKey); }));
            mControls.Add(mQKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Q); }));
            mControls.Add(mRKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.R); }));
            mControls.Add(mRightKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Right); }));
            mControls.Add(mRightAltKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.RightAlt); }));
            mControls.Add(mRightControlKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.RightControl); }));
            mControls.Add(mRightShiftKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.RightShift); }));
            mControls.Add(mRightWindowsKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.RightWindows); }));
            mControls.Add(mSKey = new KeyboardKey(this, delegate(KeyboardState state) { 
                return state.IsKeyDown(Keys.S); 
            }));
            mControls.Add(mScrollKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Scroll); }));
            mControls.Add(mSelectKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Select); }));
            mControls.Add(mSelectMediaKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.SelectMedia); }));
            mControls.Add(mSeparatorKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Separator); }));
            mControls.Add(mSleepKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Sleep); }));
            mControls.Add(mSpaceKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Space); }));
            mControls.Add(mSubtractKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Subtract); }));
            mControls.Add(mTKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.T); }));
            mControls.Add(mTabKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Tab); }));
            mControls.Add(mUKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.U); }));
            mControls.Add(mUpKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Up); }));
            mControls.Add(mVKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.V); }));
            mControls.Add(mVolumeDownKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.VolumeDown); }));
            mControls.Add(mVolumeMuteKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.VolumeMute); }));
            mControls.Add(mVolumeUpKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.VolumeUp); }));
            mControls.Add(mWKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.W); }));
            mControls.Add(mXKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.X); }));
            mControls.Add(mYKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.A); }));
            mControls.Add(mZKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Z); }));
            mControls.Add(mZoomKey = new KeyboardKey(this, delegate(KeyboardState state) { return state.IsKeyDown(Keys.Zoom); }));
            #endregion
            
            mState = mPreviousState = Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One);
        }
        #endregion

        #region Methods
        #region ADevice members
        /// <summary>
        /// Ver <see cref="Radgie.Input.ADevice.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
            mPreviousState = mState;
            mState = Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.ADevice.HasChanged"/>
        /// </summary>
        public override bool HasChanged()
        {
            return mState != mPreviousState;
        }
        #endregion

        #endregion
    }
}

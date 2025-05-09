#if UNITY_EDITOR || UNITY_SWITCH || PACKAGE_DOCS_GENERATION
using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Switch.LowLevel;
using UnityEngine.InputSystem.Utilities;

////REVIEW: The Switch controller can be used to point at things; can we somehow help leverage that?

namespace UnityEngine.InputSystem.Switch.LowLevel
{
    /// <summary>
    /// Structure of native input reports for Switch NPad controllers.
    /// </summary>
    /// <seealso href="http://en-americas-support.nintendo.com/app/answers/detail/a_id/22634/~/joy-con-controller-diagram"/>
    [StructLayout(LayoutKind.Explicit, Size = 60)]
    internal struct NPadInputState : IInputStateTypeInfo
    {
        public FourCC format
        {
            get { return new FourCC('N', 'P', 'A', 'D'); }
        }

        [InputControl(name = "dpad")]
        [InputControl(name = "buttonNorth", displayName = "X", shortDisplayName = "X", bit = (uint)Button.North)]
        [InputControl(name = "buttonSouth", displayName = "B", shortDisplayName = "B", bit = (uint)Button.South, usages = new[] { "Back", "Cancel" })]
        [InputControl(name = "buttonWest", displayName = "Y", shortDisplayName = "Y", bit = (uint)Button.West, usage = "SecondaryAction")]
        [InputControl(name = "buttonEast", displayName = "A", shortDisplayName = "A", bit = (uint)Button.East, usages = new[] { "PrimaryAction", "Submit" })]
        [InputControl(name = "leftStickPress", displayName = "Left Stick", bit = (uint)Button.StickL)]
        [InputControl(name = "rightStickPress", displayName = "Right Stick", bit = (uint)Button.StickR)]
        [InputControl(name = "leftShoulder", displayName = "L", shortDisplayName = "L", bit = (uint)Button.L)]
        [InputControl(name = "rightShoulder", displayName = "R", shortDisplayName = "R", bit = (uint)Button.R)]
        [InputControl(name = "leftTrigger", displayName = "ZL", shortDisplayName = "ZL", format = "BIT", bit = (uint)Button.ZL)]
        [InputControl(name = "rightTrigger", displayName = "ZR", shortDisplayName = "ZR", format = "BIT", bit = (uint)Button.ZR)]
        [InputControl(name = "start", displayName = "Plus", bit = (uint)Button.Plus, usage = "Menu")]
        [InputControl(name = "select", displayName = "Minus", bit = (uint)Button.Minus)]
        [InputControl(name = "leftSL", displayName = "SL (Left)", shortDisplayName = "LSL", layout = "Button", bit = (uint)Button.LSL)]
        [InputControl(name = "leftSR", displayName = "SR (Left)", shortDisplayName = "LSR", layout = "Button", bit = (uint)Button.LSR)]
        [InputControl(name = "rightSL", displayName = "SL (Right)", shortDisplayName = "RSL", layout = "Button", bit = (uint)Button.RSL)]
        [InputControl(name = "rightSR", displayName = "SR (Right)", shortDisplayName = "RSR", layout = "Button", bit = (uint)Button.RSR)]
        [FieldOffset(0)]
        public uint buttons;

        [FieldOffset(4)]
        public Vector2 leftStick;

        [FieldOffset(12)]
        public Vector2 rightStick;

        [InputControl(name = "acceleration", noisy = true)]
        [FieldOffset(20)]
        public Vector3 acceleration;

        [InputControl(name = "attitude", noisy = true)]
        [FieldOffset(32)]
        public Quaternion attitude;

        [InputControl(name = "angularVelocity", noisy = true)]
        [FieldOffset(48)]
        public Vector3 angularVelocity;

        public float leftTrigger => ((buttons & (1 << (int)Button.ZL)) != 0) ? 1f : 0f;

        public float rightTrigger => ((buttons & (1 << (int)Button.ZR)) != 0) ? 1f : 0f;

        public enum Button
        {
            // Dpad buttons. Important to be first in the bitfield as we'll
            // point the DpadControl to it.
            // IMPORTANT: Order has to match what is expected by DpadControl.
            Up,
            Down,
            Left,
            Right,

            North,
            South,
            West,
            East,

            StickL,
            StickR,
            L,
            R,

            ZL,
            ZR,
            Plus,
            Minus,

            LSL,
            LSR,
            RSL,
            RSR,

            X = North,
            B = South,
            Y = West,
            A = East,
        }

        public NPadInputState WithButton(Button button, bool value = true)
        {
            var bit = (uint)1 << (int)button;
            if (value)
                buttons |= bit;
            else
                buttons &= ~bit;
            return this;
        }
    }

    /// <summary>
    /// Switch output report sent as command to the backend.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = kSize)]
    internal struct NPadStatusReport : IInputDeviceCommandInfo
    {
        public static FourCC Type => new FourCC('N', 'P', 'D', 'S');

        internal const int kSize = InputDeviceCommand.BaseCommandSize + 24;

        [FieldOffset(0)]
        public InputDeviceCommand baseCommand;

        [FieldOffset(InputDeviceCommand.BaseCommandSize + 0)]
        public NPad.NpadId npadId;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 1)]
        public NPad.Orientation orientation;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 2)]
        public NPad.Attributes attributes;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 3)]
        public char padding0;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 4)]
        public NPad.NpadStyles styleMask;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 8)]
        public int colorLeftMain;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 12)]
        public int colorLeftSub;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 16)]
        public int colorRightMain;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 20)]
        public int colorRightSub;

        public FourCC typeStatic
        {
            get { return Type; }
        }

        public static NPadStatusReport Create()
        {
            return new NPadStatusReport
            {
                baseCommand = new InputDeviceCommand(Type, kSize),
            };
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = kSize)]
    internal struct NPadControllerSupportCommand : IInputDeviceCommandInfo
    {
        public static FourCC Type => new FourCC('N', 'P', 'D', 'U');

        internal const int kSize = InputDeviceCommand.BaseCommandSize + 8;

        [FieldOffset(0)]
        public InputDeviceCommand baseCommand;

        [FieldOffset(InputDeviceCommand.BaseCommandSize + 0)]
        public int command;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 4)]
        public int option;

        public enum Command : int
        {
            kShowUI,
            kSetHorizontalLayout,
            kStartSixAxisSensor,
            kStopSixAxisSensor,
        }

        public FourCC typeStatic
        {
            get { return Type; }
        }

        public static NPadControllerSupportCommand Create(NPadControllerSupportCommand.Command command, int option = 0)
        {
            return new NPadControllerSupportCommand
            {
                baseCommand = new InputDeviceCommand(Type, kSize),
                command = (int)command,
                option = option,
            };
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = kSize)]
    internal struct NpadDeviceIOCTLShowUI : IInputDeviceCommandInfo
    {
        public static FourCC Type => new FourCC("NSUI");
        internal const int kSize = InputDeviceCommand.BaseCommandSize;

        [FieldOffset(0)]
        public InputDeviceCommand baseCommand;

        public FourCC typeStatic
        {
            get { return Type; }
        }

        public static NpadDeviceIOCTLShowUI Create()
        {
            return new NpadDeviceIOCTLShowUI
            {
                baseCommand = new InputDeviceCommand(Type, kSize),
            };
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = kSize)]
    internal struct NpadDeviceIOCTLSetOrientation : IInputDeviceCommandInfo
    {
        public static FourCC Type => new FourCC("NSOR");
        internal const int kSize = InputDeviceCommand.BaseCommandSize + 1;

        [FieldOffset(0)]
        public InputDeviceCommand baseCommand;

        [FieldOffset(InputDeviceCommand.BaseCommandSize + 0)]
        public NPad.Orientation orientation;

        public FourCC typeStatic
        {
            get { return Type; }
        }

        public static NpadDeviceIOCTLSetOrientation Create(NPad.Orientation orientation)
        {
            return new NpadDeviceIOCTLSetOrientation
            {
                baseCommand = new InputDeviceCommand(Type, kSize),
                orientation = orientation,
            };
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = kSize)]
    internal struct NpadDeviceIOCTLStartSixAxisSensor : IInputDeviceCommandInfo
    {
        public static FourCC Type => new FourCC("SXST");
        internal const int kSize = InputDeviceCommand.BaseCommandSize;

        [FieldOffset(0)]
        public InputDeviceCommand baseCommand;

        public FourCC typeStatic
        {
            get { return Type; }
        }

        public static NpadDeviceIOCTLStartSixAxisSensor Create()
        {
            return new NpadDeviceIOCTLStartSixAxisSensor
            {
                baseCommand = new InputDeviceCommand(Type, kSize),
            };
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = kSize)]
    internal struct NpadDeviceIOCTLStopSixAxisSensor : IInputDeviceCommandInfo
    {
        public static FourCC Type => new FourCC("SXSP");
        internal const int kSize = InputDeviceCommand.BaseCommandSize;

        [FieldOffset(0)]
        public InputDeviceCommand baseCommand;

        public FourCC typeStatic
        {
            get { return Type; }
        }

        public static NpadDeviceIOCTLStopSixAxisSensor Create()
        {
            return new NpadDeviceIOCTLStopSixAxisSensor
            {
                baseCommand = new InputDeviceCommand(Type, kSize),
            };
        }
    }
    /// <summary>
    /// Switch output report sent as command to backend.
    /// </summary>
    // IMPORTANT: Struct must match the NpadDeviceIOCTLOutputReport in native
    [StructLayout(LayoutKind.Explicit, Size = kSize)]
    internal struct NPadDeviceIOCTLOutputCommand : IInputDeviceCommandInfo
    {
        public static FourCC Type { get { return new FourCC('N', 'P', 'G', 'O'); } }

        internal const int kSize = InputDeviceCommand.BaseCommandSize + 20;
        public const float DefaultFrequencyLow = 160.0f;
        public const float DefaultFrequencyHigh = 320.0f;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32", Justification = "Need to match native struct data size")]
        public enum NPadRumblePostion : byte
        {
            Left = 0x02,
            Right = 0x04,
            All = 0xFF,
            None = 0x00,
        }

        [FieldOffset(0)] public InputDeviceCommand baseCommand;

        [FieldOffset(InputDeviceCommand.BaseCommandSize + 0)]
        public byte positions;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 4)]
        public float amplitudeLow;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 8)]
        public float frequencyLow;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 12)]
        public float amplitudeHigh;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 16)]
        public float frequencyHigh;

        public FourCC typeStatic
        {
            get { return Type; }
        }

        public static NPadDeviceIOCTLOutputCommand Create(NPadRumblePostion rumblePos = NPadRumblePostion.All)
        {
            return new NPadDeviceIOCTLOutputCommand()
            {
                baseCommand = new InputDeviceCommand(Type, kSize),
                positions = (byte)rumblePos,
                amplitudeLow = 0,
                frequencyLow = DefaultFrequencyLow,
                amplitudeHigh = 0,
                frequencyHigh = DefaultFrequencyHigh
            };
        }
    }
}

namespace UnityEngine.InputSystem.Switch
{
    /// <summary>
    /// An NPad controller for Switch, which can be a Joy-Con.
    /// </summary>
    /// <seealso cref="NPadInputState"/>
    [InputControlLayout(stateType = typeof(NPadInputState), displayName = "Switch Controller (on Switch)")]
    [Scripting.Preserve]
    public class NPad : Gamepad, INPadRumble
    {
        public ButtonControl leftSL { get; private set; }
        public ButtonControl leftSR { get; private set; }
        public ButtonControl rightSL { get; private set; }
        public ButtonControl rightSR { get; private set; }

        public Vector3Control acceleration { get; private set; }
        public QuaternionControl attitude { get; private set; }
        public Vector3Control angularVelocity { get; private set; }

        // Override inherited a/b/x/y mappings to match the JoyCon layout
        public new ButtonControl bButton => buttonSouth;
        public new ButtonControl aButton => buttonEast;
        public new ButtonControl yButton => buttonWest;
        public new ButtonControl xButton => buttonNorth;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32", Justification = "Need to match native struct data size")]
        public enum Orientation : byte
        {
            Vertical,
            Horizontal,
            Default = Vertical,
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32", Justification = "Need to match native struct data size")]
        public enum NpadId : byte
        {
            No1 = 0x00,
            No2 = 0x01,
            No3 = 0x02,
            No4 = 0x03,
            No5 = 0x04,
            No6 = 0x05,
            No7 = 0x06,
            No8 = 0x07,
            Handheld = 0x20,
            Debug = 0xF0,
            Invalid = 0xFF,
        }

        [Flags]
        public enum Attributes
        {
            IsConnected = 0x01,
            IsWired = 0x02,
            IsLeftConnected = 0x04,
            IsLeftWired = 0x08,
            IsRightConnected = 0x10,
            IsRightWired = 0x20,
        }

        //orientation matters for stick axes!!!!

        //there's a touchpad and 90% of games don't support it

        //each person could play with a different style
        [Flags]
        public enum NpadStyles
        {
            FullKey = 1 << 0,//separate;or pro controller;only one accel
            Handheld = 1 << 1,//docked to switch
            JoyDual = 1 << 2,//separate; one accel per joycon
            JoyLeft = 1 << 3,//just one; orientation matters
            JoyRight = 1 << 4,//just one; orientation matters
        }

        public struct JoyConColor
        {
            public Color32 Main;
            public Color32 Sub;
        }

        public Orientation orientation
        {
            get
            {
                RefreshConfigurationIfNeeded();
                return m_Orientation;
            }
        }

        public NpadId npadId
        {
            get
            {
                RefreshConfigurationIfNeeded();
                return m_NpadId;
            }
        }

        public NpadStyles styleMask
        {
            get
            {
                RefreshConfigurationIfNeeded();
                return m_StyleMask;
            }
        }

        public JoyConColor leftControllerColor
        {
            get
            {
                RefreshConfigurationIfNeeded();
                return m_LeftControllerColor;
            }
        }

        public JoyConColor rightControllerColor
        {
            get
            {
                RefreshConfigurationIfNeeded();
                return m_RightControllerColor;
            }
        }

        public bool isConnected
        {
            get
            {
                Debug.Assert(SwitchConsoleSupport.NpadAttributesSupported(), k_StringAttributesNotSupported);
                RefreshConfigurationIfNeeded();
                return (m_Attributes & Attributes.IsConnected) != 0;
            }
        }

        public bool isLeftConnected
        {
            get
            {
                Debug.Assert(SwitchConsoleSupport.NpadAttributesSupported(), k_StringAttributesNotSupported);
                RefreshConfigurationIfNeeded();
                return (m_Attributes & Attributes.IsLeftConnected) != 0;
            }
        }

        public bool isRightConnected
        {
            get
            {
                Debug.Assert(SwitchConsoleSupport.NpadAttributesSupported(),k_StringAttributesNotSupported);
                RefreshConfigurationIfNeeded();
                return (m_Attributes & Attributes.IsRightConnected) != 0;
            }
        }

        public bool isWired
        {
            get
            {
                Debug.Assert(SwitchConsoleSupport.NpadAttributesSupported(), k_StringAttributesNotSupported);
                RefreshConfigurationIfNeeded();
                return (m_Attributes & Attributes.IsWired) != 0;
            }
        }

        public bool isLeftWired
        {
            get
            {
                Debug.Assert(SwitchConsoleSupport.NpadAttributesSupported(), k_StringAttributesNotSupported);
                RefreshConfigurationIfNeeded();
                return (m_Attributes & Attributes.IsLeftWired) != 0;
            }
        }

        public bool isRightWired
        {
            get
            {
                Debug.Assert(SwitchConsoleSupport.NpadAttributesSupported(), k_StringAttributesNotSupported);
                RefreshConfigurationIfNeeded();
                return (m_Attributes & Attributes.IsRightWired) != 0;
            }
        }

        private Orientation m_Orientation;
        private NpadId m_NpadId = NpadId.Invalid;
        private NpadStyles m_StyleMask;
        private JoyConColor m_LeftControllerColor;
        private JoyConColor m_RightControllerColor;
        private Attributes m_Attributes;

        private const string k_StringAttributesNotSupported = "Attributes not supported in this Unity version! Requires 2021.2.0a10, 2021.1.1f1, 2020.3.4f1, 2019.4.24f1 or higher";

        private struct NPadRumbleValues
        {
            public float? amplitudeLow;
            public float? frequencyLow;
            public float? amplitudeHigh;
            public float? frequencyHigh;

            public bool HasValues => amplitudeLow.HasValue && frequencyLow.HasValue && amplitudeHigh.HasValue && frequencyHigh.HasValue;

            public void SetRumbleValues(float lowAmplitude, float lowFrequency, float highAmplitude, float highFrequency)
            {
                amplitudeLow = Mathf.Clamp01(lowAmplitude);
                frequencyLow = lowFrequency;
                amplitudeHigh = Mathf.Clamp01(highAmplitude);
                frequencyHigh = highFrequency;
            }

            public void Reset()
            {
                amplitudeLow = null;
                frequencyLow = null;
                amplitudeHigh = null;
                frequencyHigh = null;
            }

            public void ApplyRumbleValues(ref NPadDeviceIOCTLOutputCommand cmd)
            {
                cmd.amplitudeLow = (float)amplitudeLow;
                cmd.frequencyLow = (float)frequencyLow;
                cmd.amplitudeHigh = (float)amplitudeHigh;
                cmd.frequencyHigh = (float)frequencyHigh;
            }
        }

        private NPadRumbleValues m_leftRumbleValues;
        private NPadRumbleValues m_rightRumbleValues;

        protected override void RefreshConfiguration()
        {
            base.RefreshConfiguration();

            var command = NPadStatusReport.Create();

            if (ExecuteCommand(ref command) > 0)
            {
                m_NpadId = command.npadId;
                m_Orientation = command.orientation;
                m_StyleMask = command.styleMask;
                m_Attributes = command.attributes;
                ReadNNColorIntoJoyConColor(ref m_LeftControllerColor, command.colorLeftMain, command.colorLeftSub);
                ReadNNColorIntoJoyConColor(ref m_RightControllerColor, command.colorRightMain, command.colorRightSub);
            }
        }

        // NOTE: This function should be static
        public long SetOrientationToSingleJoyCon(Orientation orientation)
        {
            var supportCommand = NpadDeviceIOCTLSetOrientation.Create(orientation);

            return ExecuteCommand(ref supportCommand);
        }
        
        public long StartSixAxisSensor()
        {
            var supportCommand = NpadDeviceIOCTLStartSixAxisSensor.Create();

            return ExecuteCommand(ref supportCommand);
        }

        public long StopSixAxisSensor()
        {
            var supportCommand = NpadDeviceIOCTLStopSixAxisSensor.Create();

            return ExecuteCommand(ref supportCommand);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            leftSL = GetChildControl<ButtonControl>("leftSL");
            leftSR = GetChildControl<ButtonControl>("leftSR");
            rightSL = GetChildControl<ButtonControl>("rightSL");
            rightSR = GetChildControl<ButtonControl>("rightSR");

            acceleration = GetChildControl<Vector3Control>("acceleration");
            attitude = GetChildControl<QuaternionControl>("attitude");
            angularVelocity = GetChildControl<Vector3Control>("angularVelocity");
        }

        private static void ReadNNColorIntoJoyConColor(ref JoyConColor controllerColor, int mainColor, int subColor)
        {
            controllerColor.Main = ConvertNNColorToColor32(mainColor);
            controllerColor.Sub = ConvertNNColorToColor32(subColor);
        }

        private static Color32 ConvertNNColorToColor32(int color)
        {
            return new Color32((byte)(color & 0xFF), (byte)((color >> 8) & 0xFF), (byte)((color >> 16) & 0xFF), (byte)((color >> 24) & 0xFF));
        }

        public override void PauseHaptics()
        {
            var cmd = NPadDeviceIOCTLOutputCommand.Create();
            ExecuteCommand(ref cmd);
        }

        public override void ResetHaptics()
        {
            var cmd = NPadDeviceIOCTLOutputCommand.Create();
            ExecuteCommand(ref cmd);
            m_leftRumbleValues.Reset();
            m_rightRumbleValues.Reset();
        }

        public override void ResumeHaptics()
        {
            if (m_leftRumbleValues.Equals(m_rightRumbleValues) && m_leftRumbleValues.HasValues)
            {
                var cmd = NPadDeviceIOCTLOutputCommand.Create();
                m_leftRumbleValues.ApplyRumbleValues(ref cmd);
                ExecuteCommand(ref cmd);
            }
            else
            {
                if (m_leftRumbleValues.HasValues)
                {
                    var cmd = NPadDeviceIOCTLOutputCommand.Create(NPadDeviceIOCTLOutputCommand.NPadRumblePostion.Left);
                    m_leftRumbleValues.ApplyRumbleValues(ref cmd);
                    ExecuteCommand(ref cmd);
                }

                if (m_rightRumbleValues.HasValues)
                {
                    var cmd = NPadDeviceIOCTLOutputCommand.Create(NPadDeviceIOCTLOutputCommand.NPadRumblePostion.Right);
                    m_rightRumbleValues.ApplyRumbleValues(ref cmd);
                    ExecuteCommand(ref cmd);
                }
            }
        }

        /// <summary>
        /// Set rummble intensity for all low and high frequency rumble motors
        /// </summary>
        /// <param name="lowFrequency">Low frequency motor's vibration intensity, 0..1 range</param>
        /// <param name="highFrequency">High frequency motor's vibration intensity, 0..1 range</param>
        public override void SetMotorSpeeds(float lowFrequency, float highFrequency)
        {
            SetMotorSpeeds(lowFrequency, NPadDeviceIOCTLOutputCommand.DefaultFrequencyLow, highFrequency, NPadDeviceIOCTLOutputCommand.DefaultFrequencyHigh);
        }

        /// <summary>
        /// Set the intensity and vibration frequency for all low and high frequency rumble motors
        /// </summary>
        /// <param name="lowAmplitude">Low frequency motor's vibration intensity, 0..1 range</param>
        /// <param name="lowFrequency">Low frequency motor's vibration frequency in Hz</param>
        /// <param name="highAmplitude">High frequency motor's vibration intensity, 0..1 range</param>
        /// <param name="highFrequency">High frequency motor's vibration frequency in Hz</param>
        public void SetMotorSpeeds(float lowAmplitude, float lowFrequency, float highAmplitude, float highFrequency)
        {
            m_leftRumbleValues.SetRumbleValues(lowAmplitude, lowFrequency, highAmplitude, highFrequency);
            m_rightRumbleValues.SetRumbleValues(lowAmplitude, lowFrequency, highAmplitude, highFrequency);

            var cmd = NPadDeviceIOCTLOutputCommand.Create();
            m_leftRumbleValues.ApplyRumbleValues(ref cmd);
            ExecuteCommand(ref cmd);
        }

        /// <summary>
        /// Set the intensity and vibration frequency for the left low and high frequency rumble motors
        /// </summary>
        /// <param name="lowAmplitude">Low frequency motor's vibration intensity, 0..1 range</param>
        /// <param name="lowFrequency">Low frequency motor's vibration frequency in Hz</param>
        /// <param name="highAmplitude">High frequency motor's vibration intensity, 0..1 range</param>
        /// <param name="highFrequency">High frequency motor's vibration frequency in Hz</param>
        public void SetMotorSpeedLeft(float lowAmplitude, float lowFrequency, float highAmplitude, float highFrequency)
        {
            m_leftRumbleValues.SetRumbleValues(lowAmplitude, lowFrequency, highAmplitude, highFrequency);

            var cmd = NPadDeviceIOCTLOutputCommand.Create(NPadDeviceIOCTLOutputCommand.NPadRumblePostion.Left);
            m_leftRumbleValues.ApplyRumbleValues(ref cmd);
            ExecuteCommand(ref cmd);
        }

        /// <summary>
        /// Set the intensity and vibration frequency for the right low and high frequency rumble motors
        /// </summary>
        /// <param name="lowAmplitude">Low frequency motor's vibration intensity, 0..1 range</param>
        /// <param name="lowFrequency">Low frequency motor's vibration frequency in Hz</param>
        /// <param name="highAmplitude">High frequency motor's vibration intensity, 0..1 range</param>
        /// <param name="highFrequency">High frequency motor's vibration frequency in Hz</param>
        public void SetMotorSpeedRight(float lowAmplitude, float lowFrequency, float highAmplitude, float highFrequency)
        {
            m_rightRumbleValues.SetRumbleValues(lowAmplitude, lowFrequency, highAmplitude, highFrequency);

            var cmd = NPadDeviceIOCTLOutputCommand.Create(NPadDeviceIOCTLOutputCommand.NPadRumblePostion.Right);
            m_rightRumbleValues.ApplyRumbleValues(ref cmd);
            ExecuteCommand(ref cmd);
        }
    }
}
#endif // UNITY_EDITOR || UNITY_SWITCH

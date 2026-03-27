using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsAudioSetup
{
    internal enum EDataFlow
    {
        eRender = 0,
        eCapture = 1,
        eAll = 2
    }

    internal enum ERole
    {
        eConsole = 0,
        eMultimedia = 1,
        eCommunications = 2
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PROPERTYKEY
    {
        public Guid fmtid;
        public int pid;

        public PROPERTYKEY(Guid formatId, int propertyId)
        {
            fmtid = formatId;
            pid = propertyId;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct PROPVARIANT
    {
        [FieldOffset(0)] public ushort vt;
        [FieldOffset(8)] public IntPtr pointerValue;
    }

    [ComImport]
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceEnumerator
    {
        int NotImpl1();
        int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppEndpoint);
        int GetDevice([MarshalAs(UnmanagedType.LPWStr)] string pwstrId, out IMMDevice ppDevice);
        int EnumAudioEndpoints(EDataFlow dataFlow, int dwStateMask, out IMMDeviceCollection ppDevices);
    }

    [ComImport]
    [Guid("0BD7A1BE-7A1A-44DB-8397-C0A2B91F0DB2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceCollection
    {
        int GetCount(out int pcDevices);
        int Item(int nDevice, out IMMDevice ppDevice);
    }

    [ComImport]
    [Guid("D666063F-1587-4E43-81F1-B948E807363F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDevice
    {
        int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
        int OpenPropertyStore(int stgmAccess, out IPropertyStore ppProperties);
        int GetId([MarshalAs(UnmanagedType.LPWStr)] out string ppstrId);
        int GetState(out int pdwState);
    }

    [ComImport]
    [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertyStore
    {
        int GetCount(out int cProps);
        int GetAt(int iProp, out PROPERTYKEY pkey);
        int GetValue(ref PROPERTYKEY key, out PROPVARIANT pv);
        int SetValue(ref PROPERTYKEY key, ref PROPVARIANT pv);
        int Commit();
    }

    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class MMDeviceEnumeratorComObject
    {
    }

    [ComImport]
    [Guid("870af99c-171d-4f9e-af0d-e63df40c2bc9")]
    internal class PolicyConfigClient
    {
    }

    [ComImport]
    [Guid("f8679f50-850a-41cf-9c72-430f290290c8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPolicyConfig
    {
        int GetMixFormat();
        int GetDeviceFormat();
        int ResetDeviceFormat();
        int SetDeviceFormat();
        int GetProcessingPeriod();
        int SetProcessingPeriod();
        int GetShareMode();
        int SetShareMode();
        int GetPropertyValue();
        int SetPropertyValue();
        int SetDefaultEndpoint([MarshalAs(UnmanagedType.LPWStr)] string wszDeviceId, ERole eRole);
        int SetEndpointVisibility([MarshalAs(UnmanagedType.LPWStr)] string wszDeviceId, int bVisible);
    }

    internal static class NativeMethods
    {
        [DllImport("Ole32.dll")]
        public static extern int PropVariantClear(ref PROPVARIANT pvar);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
    }

    internal sealed class DeviceInfo
    {
        public string Id;
        public string Name;
        public int State;
        public EDataFlow Flow;
    }

    internal static class AudioHelper
    {
        private static PROPERTYKEY PKEY_Device_FriendlyName =
            new PROPERTYKEY(new Guid("a45c254e-df1c-4efd-8020-67d146a850e0"), 14);

        public static string LastEnumerationMessage;
        public static string LastDefaultMessage;

        public static DeviceInfo GetDefault(EDataFlow flow, ERole role)
        {
            LastDefaultMessage = null;

            try
            {
                IMMDeviceEnumerator enumerator = (IMMDeviceEnumerator)(new MMDeviceEnumeratorComObject());
                IMMDevice device;
                int hr = enumerator.GetDefaultAudioEndpoint(flow, role, out device);
                if (hr != 0)
                {
                    return null;
                }

                return CreateDeviceInfo(device, flow);
            }
            catch (Exception ex)
            {
                LastDefaultMessage = "譌｢螳壹ョ繝舌う繧ｹ蜿門ｾ励↓螟ｱ謨励＠縺ｾ縺励◆縲・low=" + flow + ", Role=" + role + ", 隧ｳ邏ｰ: " + ex.Message;
                return null;
            }
        }

        public static List<DeviceInfo> Enumerate(EDataFlow flow)
        {
            LastEnumerationMessage = null;
            List<DeviceInfo> list = new List<DeviceInfo>();

            try
            {
                string branch = flow == EDataFlow.eRender ? @"SOFTWARE\Microsoft\Windows\CurrentVersion\MMDevices\Audio\Render" : @"SOFTWARE\Microsoft\Windows\CurrentVersion\MMDevices\Audio\Capture";
                using (RegistryKey baseKey = Registry.LocalMachine.OpenSubKey(branch, false))
                {
                    if (baseKey == null)
                    {
                        LastEnumerationMessage = "MMDevices 繝ｬ繧ｸ繧ｹ繝医Μ縺瑚ｦ九▽縺九ｊ縺ｾ縺帙ｓ縲・;
                        return list;
                    }

                    string[] subKeys = baseKey.GetSubKeyNames();
                    int i;
                    for (i = 0; i < subKeys.Length; i++)
                    {
                        string subKeyName = subKeys[i];
                        try
                        {
                            using (RegistryKey endpointKey = baseKey.OpenSubKey(subKeyName, false))
                            {
                                if (endpointKey == null)
                                {
                                    continue;
                                }

                                DeviceInfo info = CreateDeviceInfoFromRegistry(endpointKey, subKeyName, flow);
                                list.Add(info);
                            }
                        }
                        catch (Exception exItem)
                        {
                            DeviceInfo info = new DeviceInfo();
                            info.Id = subKeyName;
                            info.Name = "[蜿門ｾ怜､ｱ謨余 " + exItem.GetType().FullName;
                            info.State = 0;
                            info.Flow = flow;
                            list.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LastEnumerationMessage = "繝ｬ繧ｸ繧ｹ繝医Μ縺九ｉ縺ｮ繝・ヰ繧､繧ｹ蛻玲嫌縺ｫ螟ｱ謨励＠縺ｾ縺励◆縲・隧ｳ邏ｰ: " + ex.Message;
            }

            return list;
        }

        private static DeviceInfo CreateDeviceInfoFromRegistry(RegistryKey endpointKey, string subKeyName, EDataFlow flow)
        {
            int state = 0;
            object rawState = endpointKey.GetValue("DeviceState", 0);
            if (rawState is int)
            {
                state = (int)rawState;
            }

            string name = null;
            string id = null;

            using (RegistryKey propertiesKey = endpointKey.OpenSubKey("Properties", false))
            {
                if (propertiesKey != null)
                {
                    // PKEY_Device_FriendlyName (,14) 竊・e.g. "Speakers (THX Spatial Audio)"
                    // PKEY_Device_DeviceDesc   (,2)  竊・e.g. "Speakers"  (short fallback)
                    name = ReadRegistryString(propertiesKey, "{a45c254e-df1c-4efd-8020-67d146a850e0},14");
                    if (string.IsNullOrEmpty(name))
                    {
                        name = ReadRegistryString(propertiesKey, "{a45c254e-df1c-4efd-8020-67d146a850e0},2");
                    }

                    id = FindEndpointId(propertiesKey, subKeyName, flow);
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                name = subKeyName;
            }

            if (string.IsNullOrEmpty(id))
            {
                id = BuildEndpointIdFromSubKey(flow, subKeyName);
            }

            if (string.IsNullOrEmpty(id))
            {
                id = subKeyName;
            }

            int comState;
            string comName;
            if (TryGetComDeviceInfo(id, out comState, out comName))
            {
                state = comState;
                if (!string.IsNullOrEmpty(comName))
                {
                    name = comName;
                }
            }

            DeviceInfo info = new DeviceInfo();
            info.Id = id;
            info.Name = name;
            info.State = NormalizeRegistryState(state);
            info.Flow = flow;
            return info;
        }

        private static bool TryGetComDeviceInfo(string endpointId, out int state, out string name)
        {
            state = 0;
            name = null;
            if (string.IsNullOrEmpty(endpointId))
            {
                return false;
            }

            if (endpointId.IndexOf("{0.0.", StringComparison.OrdinalIgnoreCase) < 0)
            {
                return false;
            }

            try
            {
                IMMDeviceEnumerator enumerator = (IMMDeviceEnumerator)(new MMDeviceEnumeratorComObject());
                IMMDevice device;
                int hr = enumerator.GetDevice(endpointId, out device);
                if (hr != 0)
                {
                    return false;
                }

                device.GetState(out state);

                IPropertyStore store;
                hr = device.OpenPropertyStore(0, out store);
                if (hr == 0)
                {
                    PROPVARIANT pv;
                    hr = store.GetValue(ref PKEY_Device_FriendlyName, out pv);
                    if (hr == 0)
                    {
                        try
                        {
                            if (pv.vt == 31 && pv.pointerValue != IntPtr.Zero)
                            {
                                name = Marshal.PtrToStringUni(pv.pointerValue);
                            }
                        }
                        finally
                        {
                            NativeMethods.PropVariantClear(ref pv);
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string ReadRegistryString(RegistryKey key, string valueName)
        {
            object value = key.GetValue(valueName, null);
            return value as string;
        }

        private static string FindEndpointId(RegistryKey propertiesKey, string subKeyName, EDataFlow flow)
        {
            string[] valueNames = propertiesKey.GetValueNames();
            int i;
            for (i = 0; i < valueNames.Length; i++)
            {
                object value = propertiesKey.GetValue(valueNames[i], null);
                string text = value as string;
                if (string.IsNullOrEmpty(text))
                {
                    continue;
                }

                if (text.IndexOf(subKeyName, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                string endpointId = ExtractEndpointId(text, flow);
                if (!string.IsNullOrEmpty(endpointId))
                {
                    return endpointId;
                }
            }

            // Some systems store endpoint IDs in value names rather than value data.
            for (i = 0; i < valueNames.Length; i++)
            {
                string endpointId = ExtractEndpointId(valueNames[i], flow);
                if (!string.IsNullOrEmpty(endpointId) && endpointId.IndexOf(subKeyName, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return endpointId;
                }
            }

            return null;
        }

        private static string ExtractEndpointId(string text, EDataFlow flow)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            string flowIndex = flow == EDataFlow.eRender ? "0" : "1";
            string pattern = "\\{0\\.0\\." + flowIndex + "\\.00000000\\}\\.\\{[0-9a-fA-F\\-]+\\}";
            Match match = Regex.Match(text, pattern);
            if (!match.Success)
            {
                return null;
            }

            return match.Value;
        }

        private static string BuildEndpointIdFromSubKey(EDataFlow flow, string subKeyName)
        {
            if (string.IsNullOrEmpty(subKeyName))
            {
                return null;
            }

            string guid = subKeyName.Trim();
            if (!guid.StartsWith("{", StringComparison.Ordinal))
            {
                guid = "{" + guid;
            }
            if (!guid.EndsWith("}", StringComparison.Ordinal))
            {
                guid = guid + "}";
            }

            string flowIndex = flow == EDataFlow.eRender ? "0" : "1";
            return "{0.0." + flowIndex + ".00000000}." + guid;
        }

        private static int NormalizeRegistryState(int state)
        {
            int lowBits = state & 0x0F;
            if (lowBits != 0)
            {
                return lowBits;
            }

            return state;
        }

        private static DeviceInfo CreateDeviceInfo(IMMDevice device, EDataFlow flow)
        {
            int hr;
            string id;
            hr = device.GetId(out id);
            if (hr != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            int state;
            hr = device.GetState(out state);
            if (hr != 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            IPropertyStore store;
            hr = device.OpenPropertyStore(0, out store);
            string name = string.Empty;
            if (hr == 0)
            {
                PROPVARIANT pv;
                hr = store.GetValue(ref PKEY_Device_FriendlyName, out pv);
                if (hr == 0)
                {
                    try
                    {
                        if (pv.vt == 31 && pv.pointerValue != IntPtr.Zero)
                        {
                            name = Marshal.PtrToStringUni(pv.pointerValue);
                        }
                    }
                    finally
                    {
                        NativeMethods.PropVariantClear(ref pv);
                    }
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                name = id;
            }

            DeviceInfo info = new DeviceInfo();
            info.Id = id;
            info.Name = name ?? string.Empty;
            info.State = state;
            info.Flow = flow;
            return info;
        }

        public static bool SetDefault(string deviceId, ERole role)
        {
            IPolicyConfig policy = (IPolicyConfig)(new PolicyConfigClient());
            int hr = policy.SetDefaultEndpoint(deviceId, role);
            return hr == 0;
        }

        public static bool SetVisible(string deviceId, bool visible)
        {
            IPolicyConfig policy = (IPolicyConfig)(new PolicyConfigClient());
            int hr = policy.SetEndpointVisibility(deviceId, visible ? 1 : 0);
            return hr == 0;
        }
    }

    internal sealed class MainForm : Form
    {
        private Button applyButton;
        private Button restoreBusinessButton;
        private Button refreshButton;
        private Button openSoundButton;
        private TextBox logBox;
        private GroupBox playbackGroupBox;
        private GroupBox recordingGroupBox;
        private ListView playbackList;
        private ListView recordingList;
        private Label summaryLabel;
        private GroupBox defaultGroupBox;
        private Label playbackDefaultLabel;
        private Label playbackCommLabel;
        private Label recordingDefaultLabel;
        private Label recordingCommLabel;
        private Label statusLabel;

        public MainForm()
        {
            InitializeComponent();
            this.Shown += MainForm_Shown;
            this.Resize += MainForm_Resize;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            ForceShowFront();
            SafeRefreshDevices();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            LayoutResponsiveControls();
        }

        private void ForceShowFront()
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.TopMost = true;
            this.Activate();
            this.BringToFront();

            if (this.Handle != IntPtr.Zero)
            {
                NativeMethods.ShowWindowAsync(this.Handle, 5);
                NativeMethods.SetForegroundWindow(this.Handle);
            }

            Timer timer = new Timer();
            timer.Interval = 1200;
            timer.Tick += delegate(object sender, EventArgs e)
            {
                timer.Stop();
                timer.Dispose();
                this.TopMost = false;
            };
            timer.Start();
        }

        private void InitializeComponent()
        {
            this.Text = "Windows 繧ｵ繧ｦ繝ｳ繝芽ｨｭ螳壹ヤ繝ｼ繝ｫ";
            this.ClientSize = new Size(1220, 700);
            this.MinimumSize = new Size(1080, 640);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);

            summaryLabel = new Label();
            summaryLabel.AutoSize = false;
            summaryLabel.Location = new Point(20, 20);
            summaryLabel.Size = new Size(1170, 100);
            summaryLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            summaryLabel.Text =
                "閾ｪ蜍戊ｨｭ螳壼・螳ｹ [髢狗匱逕ｨ]\r\n" +
                "1. 蜀咲函: 繝倥ャ繝峨・繝ｳ (Realtek) -> 譌｢螳壹・繝・ヰ繧､繧ｹ\r\n" +
                "2. 蜀咲函: Voicemeeter Input -> 譌｢螳壹・騾壻ｿ｡繝・ヰ繧､繧ｹ\r\n" +
                "3. 骭ｲ髻ｳ: Voicemeeter Out A1 -> 辟｡蜉ｹ縺ｪ繧画怏蜉ｹ蛹悶ｒ隧ｦ陦圭r\n" +
                "4. 骭ｲ髻ｳ: Voicemeeter Out B1 -> 譌｢螳壹・繝・ヰ繧､繧ｹ\r\n" +
                "5. 骭ｲ髻ｳ: 螟紋ｻ倥￠繝槭う繧ｯ (Realtek) -> 譌｢螳壹・騾壻ｿ｡繝・ヰ繧､繧ｹ";

            applyButton = new Button();
            applyButton.Text = "閾ｪ蜍戊ｨｭ螳壹ｒ螳溯｡・;
            applyButton.Location = new Point(20, 130);
            applyButton.Size = new Size(220, 38);
            applyButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            applyButton.Click += ApplyButton_Click;

            refreshButton = new Button();
            refreshButton.Text = "繝・ヰ繧､繧ｹ荳隕ｧ繧呈峩譁ｰ";
            refreshButton.Location = new Point(255, 130);
            refreshButton.Size = new Size(240, 38);
            refreshButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            refreshButton.Click += RefreshButton_Click;

            openSoundButton = new Button();
            openSoundButton.Text = "繧ｵ繧ｦ繝ｳ繝芽ｨｭ螳壹ｒ髢九￥";
            openSoundButton.Location = new Point(510, 130);
            openSoundButton.Size = new Size(240, 38);
            openSoundButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            openSoundButton.Click += OpenSoundButton_Click;

            restoreBusinessButton = new Button();
            restoreBusinessButton.Text = "騾壼ｸｸ讌ｭ蜍吶∈謌ｻ縺・(Voicemeeter辟｡蜉ｹ蛹・+ Realtek譌｢螳・";
            restoreBusinessButton.Location = new Point(765, 130);
            restoreBusinessButton.Size = new Size(425, 38);
            restoreBusinessButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            restoreBusinessButton.Click += RestoreBusinessButton_Click;

            defaultGroupBox = new GroupBox();
            defaultGroupBox.Text = "讀懷・縺輔ｌ縺滓里螳壹ョ繝舌う繧ｹ";
            defaultGroupBox.Location = new Point(20, 180);
            defaultGroupBox.Size = new Size(1170, 150);
            defaultGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            playbackDefaultLabel = new Label();
            playbackDefaultLabel.AutoSize = false;
            playbackDefaultLabel.Location = new Point(15, 24);
            playbackDefaultLabel.Size = new Size(1130, 26);
            playbackDefaultLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            playbackCommLabel = new Label();
            playbackCommLabel.AutoSize = false;
            playbackCommLabel.Location = new Point(15, 54);
            playbackCommLabel.Size = new Size(1130, 26);
            playbackCommLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            recordingDefaultLabel = new Label();
            recordingDefaultLabel.AutoSize = false;
            recordingDefaultLabel.Location = new Point(15, 84);
            recordingDefaultLabel.Size = new Size(1130, 26);
            recordingDefaultLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            recordingCommLabel = new Label();
            recordingCommLabel.AutoSize = false;
            recordingCommLabel.Location = new Point(15, 114);
            recordingCommLabel.Size = new Size(1130, 26);
            recordingCommLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            defaultGroupBox.Controls.Add(playbackDefaultLabel);
            defaultGroupBox.Controls.Add(playbackCommLabel);
            defaultGroupBox.Controls.Add(recordingDefaultLabel);
            defaultGroupBox.Controls.Add(recordingCommLabel);

            playbackGroupBox = new GroupBox();
            playbackGroupBox.Text = "蜀咲函繝・ヰ繧､繧ｹ荳隕ｧ";
            playbackGroupBox.Location = new Point(20, 345);
            playbackGroupBox.Size = new Size(575, 180);
            playbackGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            playbackList = new ListView();
            playbackList.Location = new Point(15, 25);
            playbackList.Size = new Size(545, 140);
            playbackList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            playbackList.View = View.Details;
            playbackList.FullRowSelect = true;
            playbackList.GridLines = true;
            playbackList.Columns.Add("迥ｶ諷・, 100);
            playbackList.Columns.Add("繝・ヰ繧､繧ｹ蜷・, 420);
            playbackGroupBox.Controls.Add(playbackList);

            recordingGroupBox = new GroupBox();
            recordingGroupBox.Text = "骭ｲ髻ｳ繝・ヰ繧､繧ｹ荳隕ｧ";
            recordingGroupBox.Location = new Point(615, 345);
            recordingGroupBox.Size = new Size(575, 180);
            recordingGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            recordingList = new ListView();
            recordingList.Location = new Point(15, 25);
            recordingList.Size = new Size(545, 140);
            recordingList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            recordingList.View = View.Details;
            recordingList.FullRowSelect = true;
            recordingList.GridLines = true;
            recordingList.Columns.Add("迥ｶ諷・, 100);
            recordingList.Columns.Add("繝・ヰ繧､繧ｹ蜷・, 420);
            recordingGroupBox.Controls.Add(recordingList);

            logBox = new TextBox();
            logBox.Location = new Point(20, 540);
            logBox.Size = new Size(1170, 70);
            logBox.Multiline = true;
            logBox.ScrollBars = ScrollBars.Vertical;
            logBox.ReadOnly = true;
            logBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            statusLabel = new Label();
            statusLabel.AutoSize = false;
            statusLabel.Location = new Point(20, 620);
            statusLabel.Size = new Size(1170, 18);
            statusLabel.Text = "蠕・ｩ滉ｸｭ";
            statusLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.Controls.Add(summaryLabel);
            this.Controls.Add(applyButton);
            this.Controls.Add(refreshButton);
            this.Controls.Add(openSoundButton);
            this.Controls.Add(restoreBusinessButton);
            this.Controls.Add(defaultGroupBox);
            this.Controls.Add(playbackGroupBox);
            this.Controls.Add(recordingGroupBox);
            this.Controls.Add(logBox);
            this.Controls.Add(statusLabel);

            LayoutResponsiveControls();
        }

        private void LayoutResponsiveControls()
        {
            int margin = 20;
            int sectionGap = 15;
            int buttonGap = 15;
            int contentWidth = Math.Max(900, this.ClientSize.Width - (margin * 2));

            summaryLabel.Location = new Point(margin, margin);
            summaryLabel.Size = new Size(contentWidth, 100);

            int buttonTop = summaryLabel.Bottom + 10;
            int buttonHeight = 38;
            int applyWidth = Math.Max(185, (contentWidth * 18) / 100);
            int refreshWidth = Math.Max(200, (contentWidth * 20) / 100);
            int openWidth = Math.Max(200, (contentWidth * 20) / 100);
            int restoreWidth = contentWidth - applyWidth - refreshWidth - openWidth - (buttonGap * 3);
            if (restoreWidth < 240)
            {
                restoreWidth = 240;
                openWidth = Math.Max(180, contentWidth - applyWidth - refreshWidth - restoreWidth - (buttonGap * 3));
            }

            applyButton.Location = new Point(margin, buttonTop);
            applyButton.Size = new Size(applyWidth, buttonHeight);

            refreshButton.Location = new Point(applyButton.Right + buttonGap, buttonTop);
            refreshButton.Size = new Size(refreshWidth, buttonHeight);

            openSoundButton.Location = new Point(refreshButton.Right + buttonGap, buttonTop);
            openSoundButton.Size = new Size(openWidth, buttonHeight);

            restoreBusinessButton.Location = new Point(openSoundButton.Right + buttonGap, buttonTop);
            restoreBusinessButton.Size = new Size(contentWidth - (restoreBusinessButton.Left - margin), buttonHeight);

            defaultGroupBox.Location = new Point(margin, applyButton.Bottom + 12);
            defaultGroupBox.Size = new Size(contentWidth, 150);

            int defaultLabelWidth = defaultGroupBox.ClientSize.Width - 30;
            playbackDefaultLabel.Location = new Point(15, 24);
            playbackDefaultLabel.Size = new Size(defaultLabelWidth, 26);
            playbackCommLabel.Location = new Point(15, 54);
            playbackCommLabel.Size = new Size(defaultLabelWidth, 26);
            recordingDefaultLabel.Location = new Point(15, 84);
            recordingDefaultLabel.Size = new Size(defaultLabelWidth, 26);
            recordingCommLabel.Location = new Point(15, 114);
            recordingCommLabel.Size = new Size(defaultLabelWidth, 26);

            int listsTop = defaultGroupBox.Bottom + sectionGap;
            int statusHeight = 18;
            int bottomMargin = 20;
            int logGap = 15;
            int statusGap = 10;
            int remainingHeight = this.ClientSize.Height - listsTop - bottomMargin - statusHeight - statusGap;
            int logHeight = Math.Max(80, remainingHeight / 3);
            int listsHeight = Math.Max(180, remainingHeight - logHeight - logGap);

            int listGap = 20;
            int groupWidth = (contentWidth - listGap) / 2;
            playbackGroupBox.Location = new Point(margin, listsTop);
            playbackGroupBox.Size = new Size(groupWidth, listsHeight);

            recordingGroupBox.Location = new Point(playbackGroupBox.Right + listGap, listsTop);
            recordingGroupBox.Size = new Size(groupWidth, listsHeight);

            playbackList.Location = new Point(15, 25);
            playbackList.Size = new Size(playbackGroupBox.ClientSize.Width - 30, playbackGroupBox.ClientSize.Height - 40);

            recordingList.Location = new Point(15, 25);
            recordingList.Size = new Size(recordingGroupBox.ClientSize.Width - 30, recordingGroupBox.ClientSize.Height - 40);

            logBox.Location = new Point(margin, playbackGroupBox.Bottom + logGap);
            logBox.Size = new Size(contentWidth, logHeight);

            statusLabel.Location = new Point(margin, logBox.Bottom + statusGap);
            statusLabel.Size = new Size(contentWidth, statusHeight);

            UpdateListColumns(playbackList);
            UpdateListColumns(recordingList);
        }

        private void UpdateListColumns(ListView listView)
        {
            if (listView.Columns.Count < 2)
            {
                return;
            }

            int stateWidth = 100;
            int deviceWidth = Math.Max(220, listView.ClientSize.Width - stateWidth - 4);
            listView.Columns[0].Width = stateWidth;
            listView.Columns[1].Width = deviceWidth;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            applyButton.Enabled = false;
            restoreBusinessButton.Enabled = false;
            refreshButton.Enabled = false;
            openSoundButton.Enabled = false;
            Cursor previousCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                logBox.Clear();
                RunAutomaticSetup();
            }
            finally
            {
                this.Cursor = previousCursor;
                applyButton.Enabled = true;
                restoreBusinessButton.Enabled = true;
                refreshButton.Enabled = true;
                openSoundButton.Enabled = true;
                SafeRefreshDevices();
            }
        }

        private void RestoreBusinessButton_Click(object sender, EventArgs e)
        {
            applyButton.Enabled = false;
            restoreBusinessButton.Enabled = false;
            refreshButton.Enabled = false;
            openSoundButton.Enabled = false;
            Cursor previousCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                logBox.Clear();
                RunRestoreBusinessSetup();
            }
            finally
            {
                this.Cursor = previousCursor;
                applyButton.Enabled = true;
                restoreBusinessButton.Enabled = true;
                refreshButton.Enabled = true;
                openSoundButton.Enabled = true;
                SafeRefreshDevices();
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            SafeRefreshDevices();
        }

        private void OpenSoundButton_Click(object sender, EventArgs e)
        {
            Process.Start("control.exe", "mmsys.cpl");
        }

        private void RefreshDevices()
        {
            statusLabel.Text = "繝・ヰ繧､繧ｹ荳隕ｧ繧呈峩譁ｰ荳ｭ...";
            statusLabel.Refresh();
            this.Update();
            playbackList.Items.Clear();
            recordingList.Items.Clear();
            logBox.AppendText("繝・ヰ繧､繧ｹ荳隕ｧ繧呈峩譁ｰ縺励※縺・∪縺・..\r\n");

            SafeAddDevicesToList(EDataFlow.eRender, "蜀咲函");
            SafeAddDevicesToList(EDataFlow.eCapture, "骭ｲ髻ｳ");
            SafeUpdateDefaultDeviceLabels();
            statusLabel.Text = "繝・ヰ繧､繧ｹ荳隕ｧ縺ｮ譖ｴ譁ｰ縺檎ｵゆｺ・＠縺ｾ縺励◆縲・;
            statusLabel.Refresh();
            AppendLog("繝・ヰ繧､繧ｹ荳隕ｧ縺ｮ譖ｴ譁ｰ縺檎ｵゆｺ・＠縺ｾ縺励◆縲・);
        }

        private void SafeRefreshDevices()
        {
            try
            {
                RefreshDevices();
            }
            catch (Exception ex)
            {
                AppendException("[ERROR] 繝・ヰ繧､繧ｹ荳隕ｧ縺ｮ蜿門ｾ励↓螟ｱ謨励＠縺ｾ縺励◆", ex);
                playbackDefaultLabel.Text = "蜀咲函 譌｢螳・ 蜿門ｾ怜､ｱ謨・;
                playbackCommLabel.Text = "蜀咲函 騾壻ｿ｡: 蜿門ｾ怜､ｱ謨・;
                recordingDefaultLabel.Text = "骭ｲ髻ｳ 譌｢螳・ 蜿門ｾ怜､ｱ謨・;
                recordingCommLabel.Text = "骭ｲ髻ｳ 騾壻ｿ｡: 蜿門ｾ怜､ｱ謨・;
                statusLabel.Text = "繝・ヰ繧､繧ｹ荳隕ｧ縺ｮ譖ｴ譁ｰ縺ｫ螟ｱ謨励＠縺ｾ縺励◆縲・;
                AppendLog("繝・ヰ繧､繧ｹ荳隕ｧ縺ｮ譖ｴ譁ｰ縺ｫ螟ｱ謨励＠縺ｾ縺励◆縲・);
            }
        }

        private void SafeAddDevicesToList(EDataFlow flow, string label)
        {
            try
            {
                AddDevicesToList(flow, label);
            }
            catch (Exception ex)
            {
                AppendException("[ERROR] " + label + "繝・ヰ繧､繧ｹ荳隕ｧ縺ｮ蜿門ｾ励↓螟ｱ謨励＠縺ｾ縺励◆", ex);
                ListViewItem item = new ListViewItem("蜿門ｾ怜､ｱ謨・);
                item.SubItems.Add(ex.GetType().FullName);
                GetListViewForFlow(flow).Items.Add(item);
            }
        }

        private void SafeUpdateDefaultDeviceLabels()
        {
            try
            {
                UpdateDefaultDeviceLabels();
            }
            catch (Exception ex)
            {
                AppendException("[ERROR] 譌｢螳壹ョ繝舌う繧ｹ陦ｨ遉ｺ縺ｮ譖ｴ譁ｰ縺ｫ螟ｱ謨励＠縺ｾ縺励◆", ex);
                playbackDefaultLabel.Text = "蜀咲函 譌｢螳・ 蜿門ｾ怜､ｱ謨・;
                playbackCommLabel.Text = "蜀咲函 騾壻ｿ｡: 蜿門ｾ怜､ｱ謨・;
                recordingDefaultLabel.Text = "骭ｲ髻ｳ 譌｢螳・ 蜿門ｾ怜､ｱ謨・;
                recordingCommLabel.Text = "骭ｲ髻ｳ 騾壻ｿ｡: 蜿門ｾ怜､ｱ謨・;
            }
        }

        private void AddDevicesToList(EDataFlow flow, string label)
        {
            List<DeviceInfo> devices = AudioHelper.Enumerate(flow);
            if (!string.IsNullOrEmpty(AudioHelper.LastEnumerationMessage))
            {
                AppendLog("[WARN] " + label + ": " + AudioHelper.LastEnumerationMessage);
            }

            devices.Sort(CompareDeviceDisplayOrder);

            int i;
            for (i = 0; i < devices.Count; i++)
            {
                DeviceInfo device = devices[i];
                if (!IsVisibleInMmsysList(device))
                {
                    continue;
                }

                ListViewItem item = new ListViewItem(GetStateLabel(device.State));
                item.SubItems.Add(device.Name);
                GetListViewForFlow(flow).Items.Add(item);
            }
        }

        private static bool IsVisibleInMmsysList(DeviceInfo device)
        {
            if (device == null)
            {
                return false;
            }

            // DEVICE_STATE_ACTIVE (1) 窶・mmsys.cpl 騾壼ｸｸ陦ｨ遉ｺ
            // DEVICE_STATE_DISABLED (2) 窶・mmsys.cpl縲檎┌蜉ｹ縺ｪ繝・ヰ繧､繧ｹ縺ｮ陦ｨ遉ｺ縲阪〒蜃ｺ繧九ｂ縺ｮ
            return (device.State & 1) == 1 || (device.State & 2) == 2;
        }

        private ListView GetListViewForFlow(EDataFlow flow)
        {
            return flow == EDataFlow.eRender ? playbackList : recordingList;
        }

        private static int CompareDeviceDisplayOrder(DeviceInfo left, DeviceInfo right)
        {
            int leftPriority = GetDisplayPriority(left != null ? left.State : 0);
            int rightPriority = GetDisplayPriority(right != null ? right.State : 0);
            int compare = leftPriority.CompareTo(rightPriority);
            if (compare != 0)
            {
                return compare;
            }

            string leftName = left != null ? left.Name : string.Empty;
            string rightName = right != null ? right.Name : string.Empty;
            return string.Compare(leftName, rightName, StringComparison.OrdinalIgnoreCase);
        }

        private static int GetDisplayPriority(int state)
        {
            if ((state & 1) == 1)
            {
                return 0;
            }

            if ((state & 2) == 2)
            {
                return 1;
            }

            if ((state & 4) == 4 || (state & 8) == 8)
            {
                return 2;
            }

            return 3;
        }

        private void RunAutomaticSetup()
        {
            try
            {
                DeviceInfo playbackHeadphones = FindBestRealtekHeadphones();
                DeviceInfo playbackVmInput = FindFirstMatch(EDataFlow.eRender, new string[] { "Voicemeeter Input" });
                DeviceInfo captureA1 = FindFirstMatch(EDataFlow.eCapture, new string[] { "Voicemeeter Out A1" });
                DeviceInfo captureB1 = FindFirstMatch(EDataFlow.eCapture, new string[] { "Voicemeeter Out B1" });
                DeviceInfo captureMic = FindBestRealtekExternalMic();

                if (playbackHeadphones == null)
                {
                    throw new InvalidOperationException("蜀咲函繝・ヰ繧､繧ｹ縲弱・繝・ラ繝帙Φ (Realtek)縲上′隕九▽縺九ｊ縺ｾ縺帙ｓ縲・);
                }
                if (playbackVmInput == null)
                {
                    throw new InvalidOperationException("蜀咲函繝・ヰ繧､繧ｹ縲桟oicemeeter Input縲上′隕九▽縺九ｊ縺ｾ縺帙ｓ縲・);
                }
                if (captureA1 == null)
                {
                    throw new InvalidOperationException("骭ｲ髻ｳ繝・ヰ繧､繧ｹ縲桟oicemeeter Out A1縲上′隕九▽縺九ｊ縺ｾ縺帙ｓ縲・);
                }
                if (captureB1 == null)
                {
                    throw new InvalidOperationException("骭ｲ髻ｳ繝・ヰ繧､繧ｹ縲桟oicemeeter Out B1縲上′隕九▽縺九ｊ縺ｾ縺帙ｓ縲・);
                }
                if (captureMic == null)
                {
                    throw new InvalidOperationException("骭ｲ髻ｳ繝・ヰ繧､繧ｹ縲主､紋ｻ倥￠繝槭う繧ｯ (Realtek)縲上′隕九▽縺九ｊ縺ｾ縺帙ｓ縲・);
                }

                // [蜑榊・逅・ Voicemeeter 繝・ヰ繧､繧ｹ繧貞・縺ｫ蜈ｨ縺ｦ譛牙柑蛹悶＠縺ｦ縺九ｉ譌｢螳夊ｨｭ螳壹ｒ陦後≧
                AppendLog("[蜑榊・逅・ Voicemeeter 繝・ヰ繧､繧ｹ縺ｮ譛牙柑蛹悶ｒ遒ｺ隱阪＠縺ｾ縺吶・);
                bool anyEnabled = false;
                anyEnabled |= EnsureEnabled(playbackVmInput);
                anyEnabled |= EnsureEnabled(captureA1);
                anyEnabled |= EnsureEnabled(captureB1);
                if (anyEnabled)
                {
                    AppendLog("      Windows 縺ｫ蜿肴丐縺輔○繧九◆繧∝ｰ代・ｾ・ｩ溘＠縺ｾ縺・..");
                    System.Threading.Thread.Sleep(1500);
                }
                else
                {
                    AppendLog("      蜈ｨ繝・ヰ繧､繧ｹ譌｢縺ｫ譛牙柑縺ｧ縺吶・);
                }

                AppendLog("[1/5] 蜀咲函: " + playbackHeadphones.Name + " 繧呈里螳壹・繝・ヰ繧､繧ｹ縺ｸ險ｭ螳壹＠縺ｾ縺吶・);
                SetDefaultOrThrow(playbackHeadphones.Id, new ERole[] { ERole.eConsole, ERole.eMultimedia });
                AppendLog("      螳御ｺ・);

                AppendLog("[2/5] 蜀咲函: Voicemeeter Input 繧呈里螳壹・騾壻ｿ｡繝・ヰ繧､繧ｹ縺ｸ險ｭ螳壹＠縺ｾ縺吶・);
                SetDefaultOrThrow(playbackVmInput.Id, new ERole[] { ERole.eCommunications });
                AppendLog("      螳御ｺ・);

                AppendLog("[3/5] 骭ｲ髻ｳ: Voicemeeter Out A1 縺ｮ譛牙柑蛹悶ｒ遒ｺ隱阪＠縺ｾ縺吶・);
                AppendLog("      譛牙柑蛹匁ｸ医∩縺ｧ縺吶・);

                AppendLog("[4/5] 骭ｲ髻ｳ: Voicemeeter Out B1 繧呈里螳壹・繝・ヰ繧､繧ｹ縺ｸ險ｭ螳壹＠縺ｾ縺吶・);
                SetDefaultOrThrow(captureB1.Id, new ERole[] { ERole.eConsole, ERole.eMultimedia });
                AppendLog("      螳御ｺ・);

                AppendLog("[5/5] 骭ｲ髻ｳ: " + captureMic.Name + " 繧呈里螳壹・騾壻ｿ｡繝・ヰ繧､繧ｹ縺ｸ險ｭ螳壹＠縺ｾ縺吶・);
                SetDefaultOrThrow(captureMic.Id, new ERole[] { ERole.eCommunications });
                AppendLog("      螳御ｺ・);

                AppendLog(string.Empty);
                AppendLog("縺吶∋縺ｦ縺ｮ險ｭ螳壹′螳御ｺ・＠縺ｾ縺励◆縲・);
                UpdateDefaultDeviceLabels();
                MessageBox.Show(this, "閾ｪ蜍戊ｨｭ螳壹′螳御ｺ・＠縺ｾ縺励◆縲・, "螳御ｺ・, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendLog(string.Empty);
                AppendException("[ERROR] 閾ｪ蜍戊ｨｭ螳壹↓螟ｱ謨励＠縺ｾ縺励◆", ex);
                AppendLog("謇句虚繝輔か繝ｼ繝ｫ繝舌ャ繧ｯ逕ｨ縺ｫ繧ｵ繧ｦ繝ｳ繝芽ｨｭ螳壹ｒ髢九″縺ｾ縺吶・);
                Process.Start("control.exe", "mmsys.cpl");
                MessageBox.Show(
                    this,
                    "閾ｪ蜍戊ｨｭ螳壹↓螟ｱ謨励＠縺ｾ縺励◆縲・r\n\r\n" + ex.Message + "\r\n\r\n繧ｵ繧ｦ繝ｳ繝芽ｨｭ螳壹ｒ髢九＞縺溘・縺ｧ縲∝ｿ・ｦ√↓蠢懊§縺ｦ謇句虚縺ｧ莉穂ｸ翫￡縺ｦ縺上□縺輔＞縲・,
                    "繧ｨ繝ｩ繝ｼ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private bool EnsureEnabled(DeviceInfo device)
        {
            if ((device.State & 2) == 2)
            {
                bool ok = AudioHelper.SetVisible(device.Id, true);
                string label = device.Name ?? device.Id;
                AppendLog("      " + label + ": " + (ok ? "譛牙柑蛹悶＠縺ｾ縺励◆縲・ : "譛牙柑蛹悶↓螟ｱ謨励＠縺ｾ縺励◆縲よ焔蜍輔〒譛牙柑蛹悶＠縺ｦ縺上□縺輔＞縲・));
                return ok;
            }
            return false;
        }

        private void RunRestoreBusinessSetup()
        {
            try
            {
                DeviceInfo vmInput  = FindFirstMatch(EDataFlow.eRender,  new string[] { "Voicemeeter Input" });
                DeviceInfo captureA1 = FindFirstMatch(EDataFlow.eCapture, new string[] { "Voicemeeter Out A1" });
                DeviceInfo captureB1 = FindFirstMatch(EDataFlow.eCapture, new string[] { "Voicemeeter Out B1" });
                DeviceInfo playbackHeadphones = FindBestRealtekHeadphones();
                DeviceInfo captureMic = FindBestRealtekExternalMic();

                if (playbackHeadphones == null)
                {
                    throw new InvalidOperationException("蜀咲函繝・ヰ繧､繧ｹ縲弱・繝・ラ繝帙Φ (Realtek)縲上′隕九▽縺九ｊ縺ｾ縺帙ｓ縲・);
                }
                if (captureMic == null)
                {
                    throw new InvalidOperationException("骭ｲ髻ｳ繝・ヰ繧､繧ｹ縲主､紋ｻ倥￠繝槭う繧ｯ (Realtek)縲上′隕九▽縺九ｊ縺ｾ縺帙ｓ縲・);
                }

                AppendLog("[1/5] 蜀咲函: Voicemeeter Input 繧堤┌蜉ｹ蛹悶＠縺ｾ縺吶・);
                if (vmInput != null)
                {
                    bool ok = AudioHelper.SetVisible(vmInput.Id, false);
                    AppendLog(ok ? "      螳御ｺ・ : "      閾ｪ蜍慕┌蜉ｹ蛹悶・螟ｱ謨励＠縺ｾ縺励◆縲ょｿ・ｦ√↑繧画焔蜍輔〒辟｡蜉ｹ蛹悶＠縺ｦ縺上□縺輔＞縲・);
                }
                else
                {
                    AppendLog("      繝・ヰ繧､繧ｹ縺瑚ｦ九▽縺九ｉ縺ｪ縺・◆繧√せ繧ｭ繝・・縺励∪縺吶・);
                }

                AppendLog("[2/5] 骭ｲ髻ｳ: Voicemeeter Out A1 繧堤┌蜉ｹ蛹悶＠縺ｾ縺吶・);
                if (captureA1 != null)
                {
                    bool ok = AudioHelper.SetVisible(captureA1.Id, false);
                    AppendLog(ok ? "      螳御ｺ・ : "      閾ｪ蜍慕┌蜉ｹ蛹悶・螟ｱ謨励＠縺ｾ縺励◆縲ょｿ・ｦ√↑繧画焔蜍輔〒辟｡蜉ｹ蛹悶＠縺ｦ縺上□縺輔＞縲・);
                }
                else
                {
                    AppendLog("      繝・ヰ繧､繧ｹ縺瑚ｦ九▽縺九ｉ縺ｪ縺・◆繧√せ繧ｭ繝・・縺励∪縺吶・);
                }

                AppendLog("[3/5] 骭ｲ髻ｳ: Voicemeeter Out B1 繧堤┌蜉ｹ蛹悶＠縺ｾ縺吶・);
                if (captureB1 != null)
                {
                    bool ok = AudioHelper.SetVisible(captureB1.Id, false);
                    AppendLog(ok ? "      螳御ｺ・ : "      閾ｪ蜍慕┌蜉ｹ蛹悶・螟ｱ謨励＠縺ｾ縺励◆縲ょｿ・ｦ√↑繧画焔蜍輔〒辟｡蜉ｹ蛹悶＠縺ｦ縺上□縺輔＞縲・);
                }
                else
                {
                    AppendLog("      繝・ヰ繧､繧ｹ縺瑚ｦ九▽縺九ｉ縺ｪ縺・◆繧√せ繧ｭ繝・・縺励∪縺吶・);
                }

                AppendLog("[4/5] 蜀咲函: " + playbackHeadphones.Name + " 繧呈里螳・騾壻ｿ｡繝・ヰ繧､繧ｹ縺ｸ險ｭ螳壹＠縺ｾ縺吶・);
                SetDefaultOrThrow(playbackHeadphones.Id, new ERole[] { ERole.eConsole, ERole.eMultimedia, ERole.eCommunications });
                AppendLog("      螳御ｺ・);

                AppendLog("[5/5] 骭ｲ髻ｳ: " + captureMic.Name + " 繧呈里螳・騾壻ｿ｡繝・ヰ繧､繧ｹ縺ｸ險ｭ螳壹＠縺ｾ縺吶・);
                SetDefaultOrThrow(captureMic.Id, new ERole[] { ERole.eConsole, ERole.eMultimedia, ERole.eCommunications });
                AppendLog("      螳御ｺ・);

                AppendLog(string.Empty);
                AppendLog("騾壼ｸｸ讌ｭ蜍吶∈謌ｻ縺・縺悟ｮ御ｺ・＠縺ｾ縺励◆縲・);
                UpdateDefaultDeviceLabels();
                MessageBox.Show(this, "騾壼ｸｸ讌ｭ蜍吶∈謌ｻ縺・縺悟ｮ御ｺ・＠縺ｾ縺励◆縲・, "螳御ｺ・, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendLog(string.Empty);
                AppendException("[ERROR] 騾壼ｸｸ讌ｭ蜍吶∈謌ｻ縺・縺ｫ螟ｱ謨励＠縺ｾ縺励◆", ex);
                MessageBox.Show(
                    this,
                    "騾壼ｸｸ讌ｭ蜍吶∈謌ｻ縺・縺ｫ螟ｱ謨励＠縺ｾ縺励◆縲・r\n\r\n" + ex.Message,
                    "繧ｨ繝ｩ繝ｼ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void UpdateDefaultDeviceLabels()
        {
            DeviceInfo playbackDefault = AudioHelper.GetDefault(EDataFlow.eRender, ERole.eConsole);
            playbackDefaultLabel.Text = "蜀咲函 譌｢螳・ " + FormatDeviceLabel(playbackDefault);
            if (!string.IsNullOrEmpty(AudioHelper.LastDefaultMessage))
            {
                AppendLog("[WARN] " + AudioHelper.LastDefaultMessage);
            }

            DeviceInfo playbackComm = AudioHelper.GetDefault(EDataFlow.eRender, ERole.eCommunications);
            playbackCommLabel.Text = "蜀咲函 騾壻ｿ｡: " + FormatDeviceLabel(playbackComm);
            if (!string.IsNullOrEmpty(AudioHelper.LastDefaultMessage))
            {
                AppendLog("[WARN] " + AudioHelper.LastDefaultMessage);
            }

            DeviceInfo recordingDefault = AudioHelper.GetDefault(EDataFlow.eCapture, ERole.eConsole);
            recordingDefaultLabel.Text = "骭ｲ髻ｳ 譌｢螳・ " + FormatDeviceLabel(recordingDefault);
            if (!string.IsNullOrEmpty(AudioHelper.LastDefaultMessage))
            {
                AppendLog("[WARN] " + AudioHelper.LastDefaultMessage);
            }

            DeviceInfo recordingComm = AudioHelper.GetDefault(EDataFlow.eCapture, ERole.eCommunications);
            recordingCommLabel.Text = "骭ｲ髻ｳ 騾壻ｿ｡: " + FormatDeviceLabel(recordingComm);
            if (!string.IsNullOrEmpty(AudioHelper.LastDefaultMessage))
            {
                AppendLog("[WARN] " + AudioHelper.LastDefaultMessage);
            }
        }

        private static string FormatDeviceLabel(DeviceInfo device)
        {
            if (device == null)
            {
                return "譛ｪ讀懷・";
            }

            return device.Name + " [" + GetStateLabel(device.State) + "]";
        }

        private void SetDefaultOrThrow(string deviceId, ERole[] roles)
        {
            int i;
            for (i = 0; i < roles.Length; i++)
            {
                bool ok = AudioHelper.SetDefault(deviceId, roles[i]);
                if (!ok)
                {
                    throw new InvalidOperationException("譌｢螳壹ョ繝舌う繧ｹ縺ｮ險ｭ螳壹↓螟ｱ謨励＠縺ｾ縺励◆縲３ole=" + roles[i]);
                }
            }
        }

        private DeviceInfo FindFirstMatch(EDataFlow flow, string[] keywords)
        {
            List<DeviceInfo> devices = AudioHelper.Enumerate(flow);
            int i;
            for (i = 0; i < devices.Count; i++)
            {
                DeviceInfo device = devices[i];
                if (ContainsAll(device.Name, keywords))
                {
                    return device;
                }
            }
            return null;
        }

        private DeviceInfo FindBestRealtekHeadphones()
        {
            List<DeviceInfo> devices = AudioHelper.Enumerate(EDataFlow.eRender);
            DeviceInfo best = null;
            int bestScore = int.MinValue;
            int i;

            for (i = 0; i < devices.Count; i++)
            {
                DeviceInfo device = devices[i];
                if (device.Name == null)
                {
                    continue;
                }

                if (device.Name.IndexOf("Realtek", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                int score = 0;
                if (device.Name.IndexOf("Realtek", StringComparison.OrdinalIgnoreCase) >= 0) score += 4;
                if (device.Name.IndexOf("繝倥ャ繝峨・繝ｳ", StringComparison.OrdinalIgnoreCase) >= 0) score += 10;
                if (device.Name.IndexOf("Headphone", StringComparison.OrdinalIgnoreCase) >= 0) score += 10;
                if ((device.State & 1) == 1) score += 2;

                if (score > bestScore)
                {
                    best = device;
                    bestScore = score;
                }
            }

            return best;
        }

        private DeviceInfo FindBestRealtekExternalMic()
        {
            List<DeviceInfo> devices = AudioHelper.Enumerate(EDataFlow.eCapture);
            DeviceInfo best = null;
            int bestScore = int.MinValue;
            int i;

            for (i = 0; i < devices.Count; i++)
            {
                DeviceInfo device = devices[i];
                if (device.Name == null)
                {
                    continue;
                }

                if (device.Name.IndexOf("Realtek", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                int score = 0;
                if (device.Name.IndexOf("Realtek", StringComparison.OrdinalIgnoreCase) >= 0) score += 4;
                if (device.Name.IndexOf("繝槭う繧ｯ", StringComparison.OrdinalIgnoreCase) >= 0) score += 6;
                if (device.Name.IndexOf("Mic", StringComparison.OrdinalIgnoreCase) >= 0) score += 6;
                if (device.Name.IndexOf("Microphone", StringComparison.OrdinalIgnoreCase) >= 0) score += 6;
                // 螟紋ｻ倥￠繝槭う繧ｯ繧貞━蜈医＠縲・・蛻励・繧､繧ｯ(繝槭う繧ｯ驟榊・)縺ｯ荳九￡繧・                if (device.Name.IndexOf("螟紋ｻ倥￠", StringComparison.OrdinalIgnoreCase) >= 0) score += 10;
                if (device.Name.IndexOf("驟榊・", StringComparison.OrdinalIgnoreCase) >= 0) score -= 6;
                if (device.Name.IndexOf("Array", StringComparison.OrdinalIgnoreCase) >= 0) score -= 6;
                if ((device.State & 1) == 1) score += 2;

                if (score > bestScore)
                {
                    best = device;
                    bestScore = score;
                }
            }

            return best;
        }

        private DeviceInfo FindBestPlantronicsRenderHeadset()
        {
            return FindFirstMatch(EDataFlow.eRender, new string[] { "Plantronics" });
        }

        private DeviceInfo FindBestPlantronicsCaptureMic()
        {
            return FindFirstMatch(EDataFlow.eCapture, new string[] { "Plantronics" });
        }

        private static bool ContainsAll(string source, string[] keywords)
        {
            if (source == null)
            {
                return false;
            }

            int i;
            for (i = 0; i < keywords.Length; i++)
            {
                if (source.IndexOf(keywords[i], StringComparison.OrdinalIgnoreCase) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetStateLabel(int state)
        {
            if ((state & 2) == 2) return "辟｡蜉ｹ";
            if ((state & 8) == 8) return "辟｡蜉ｹ(譛ｪ謗･邯・";
            if ((state & 4) == 4) return "辟｡蜉ｹ(譛ｪ謗･邯・";
            if ((state & 1) == 1) return "譛牙柑";
            return "荳肴・";
        }

        private void AppendLog(string message)
        {
            logBox.AppendText(message + Environment.NewLine);
        }

        private void AppendException(string title, Exception ex)
        {
            AppendLog(title);
            AppendLog(ex.ToString());
        }
    }

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                ShowFatalError(ex);
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ShowFatalError(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex == null)
            {
                ex = new Exception("荳肴・縺ｪ萓句､悶′逋ｺ逕溘＠縺ｾ縺励◆縲・);
            }

            ShowFatalError(ex);
        }

        private static void ShowFatalError(Exception ex)
        {
            MessageBox.Show(
                "繝・・繝ｫ縺ｮ襍ｷ蜍輔∪縺溘・螳溯｡御ｸｭ縺ｫ繧ｨ繝ｩ繝ｼ縺檎匱逕溘＠縺ｾ縺励◆縲・r\n\r\n" + ex.Message,
                "Windows 繧ｵ繧ｦ繝ｳ繝芽ｨｭ螳壹ヤ繝ｼ繝ｫ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}

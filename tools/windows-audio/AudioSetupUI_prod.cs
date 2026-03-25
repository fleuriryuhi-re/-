using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
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
                LastDefaultMessage = "既定デバイス取得に失敗しました。Flow=" + flow + ", Role=" + role + ", 詳細: " + ex.Message;
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
                        LastEnumerationMessage = "MMDevices レジストリが見つかりません。";
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
                            info.Name = "[取得失敗] " + exItem.GetType().FullName;
                            info.State = 0;
                            info.Flow = flow;
                            list.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LastEnumerationMessage = "レジストリからのデバイス列挙に失敗しました。 詳細: " + ex.Message;
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
                    // PKEY_Device_FriendlyName (,14) → e.g. "Speakers (THX Spatial Audio)"
                    // PKEY_Device_DeviceDesc   (,2)  → e.g. "Speakers"  (short fallback)
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
            this.Text = "Windows サウンド設定ツール";
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
                "自動設定内容\r\n" +
                "1. 再生: Plantronics DA80 -> 既定のデバイス\r\n" +
                "2. 再生: Voicemeeter Input -> 既定の通信デバイス\r\n" +
                "3. 録音: Voicemeeter Out A1 -> 無効なら有効化を試行\r\n" +
                "4. 録音: Voicemeeter Out B1 -> 既定のデバイス\r\n" +
                "5. 録音: Plantronics DA80 -> 既定の通信デバイス";
            // [開発差分候補/コメントアウト]
            // summaryLabel.Text += "\r\n※ 同一状態でも整合性確認のため再設定を行います";

            applyButton = new Button();
            applyButton.Text = "自動設定を実行";
            applyButton.Location = new Point(20, 130);
            applyButton.Size = new Size(220, 38);
            applyButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            applyButton.Click += ApplyButton_Click;

            refreshButton = new Button();
            refreshButton.Text = "デバイス一覧を更新";
            refreshButton.Location = new Point(255, 130);
            refreshButton.Size = new Size(240, 38);
            refreshButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            refreshButton.Click += RefreshButton_Click;

            openSoundButton = new Button();
            openSoundButton.Text = "サウンド設定を開く";
            openSoundButton.Location = new Point(510, 130);
            openSoundButton.Size = new Size(240, 38);
            openSoundButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            openSoundButton.Click += OpenSoundButton_Click;

            restoreBusinessButton = new Button();
            restoreBusinessButton.Text = "業務用(Plantronics DA80)へ戻す";
            restoreBusinessButton.Location = new Point(765, 130);
            restoreBusinessButton.Size = new Size(425, 38);
            restoreBusinessButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            restoreBusinessButton.Click += RestoreBusinessButton_Click;

            defaultGroupBox = new GroupBox();
            defaultGroupBox.Text = "検出された既定デバイス";
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
            playbackGroupBox.Text = "再生デバイス一覧";
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
            playbackList.Columns.Add("状態", 100);
            playbackList.Columns.Add("デバイス名", 420);
            playbackGroupBox.Controls.Add(playbackList);

            recordingGroupBox = new GroupBox();
            recordingGroupBox.Text = "録音デバイス一覧";
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
            recordingList.Columns.Add("状態", 100);
            recordingList.Columns.Add("デバイス名", 420);
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
            statusLabel.Text = "待機中";
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
            statusLabel.Text = "デバイス一覧を更新中...";
            statusLabel.Refresh();
            this.Update();
            playbackList.Items.Clear();
            recordingList.Items.Clear();
            logBox.AppendText("デバイス一覧を更新しています...\r\n");

            SafeAddDevicesToList(EDataFlow.eRender, "再生");
            SafeAddDevicesToList(EDataFlow.eCapture, "録音");
            SafeUpdateDefaultDeviceLabels();
            statusLabel.Text = "デバイス一覧の更新が終了しました。";
            statusLabel.Refresh();
            AppendLog("デバイス一覧の更新が終了しました。");
        }

        private void SafeRefreshDevices()
        {
            try
            {
                RefreshDevices();
            }
            catch (Exception ex)
            {
                AppendException("[ERROR] デバイス一覧の取得に失敗しました", ex);
                playbackDefaultLabel.Text = "再生 既定: 取得失敗";
                playbackCommLabel.Text = "再生 通信: 取得失敗";
                recordingDefaultLabel.Text = "録音 既定: 取得失敗";
                recordingCommLabel.Text = "録音 通信: 取得失敗";
                statusLabel.Text = "デバイス一覧の更新に失敗しました。";
                AppendLog("デバイス一覧の更新に失敗しました。");
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
                AppendException("[ERROR] " + label + "デバイス一覧の取得に失敗しました", ex);
                ListViewItem item = new ListViewItem("取得失敗");
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
                AppendException("[ERROR] 既定デバイス表示の更新に失敗しました", ex);
                playbackDefaultLabel.Text = "再生 既定: 取得失敗";
                playbackCommLabel.Text = "再生 通信: 取得失敗";
                recordingDefaultLabel.Text = "録音 既定: 取得失敗";
                recordingCommLabel.Text = "録音 通信: 取得失敗";
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

            // DEVICE_STATE_ACTIVE (1) — mmsys.cpl 通常表示
            // DEVICE_STATE_DISABLED (2) — mmsys.cpl「無効なデバイスの表示」で出るもの
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
                DeviceInfo playbackPlantronics = FindBestPlantronicsRenderHeadset();
                DeviceInfo playbackVmInput = FindFirstMatch(EDataFlow.eRender, new string[] { "Voicemeeter Input" });
                DeviceInfo captureA1 = FindFirstMatch(EDataFlow.eCapture, new string[] { "Voicemeeter Out A1" });
                DeviceInfo captureB1 = FindFirstMatch(EDataFlow.eCapture, new string[] { "Voicemeeter Out B1" });
                DeviceInfo capturePlantronics = FindBestPlantronicsCaptureMic();

                if (playbackPlantronics == null)
                {
                    throw new InvalidOperationException("再生デバイス『Plantronics DA80 Headset/Earphone』が見つかりません。");
                }
                if (playbackVmInput == null)
                {
                    throw new InvalidOperationException("再生デバイス『Voicemeeter Input』が見つかりません。");
                }
                if (captureA1 == null)
                {
                    throw new InvalidOperationException("録音デバイス『Voicemeeter Out A1』が見つかりません。");
                }
                if (captureB1 == null)
                {
                    throw new InvalidOperationException("録音デバイス『Voicemeeter Out B1』が見つかりません。");
                }
                if (capturePlantronics == null)
                {
                    throw new InvalidOperationException("録音デバイス『Plantronics DA80 Mic/Headset』が見つかりません。");
                }

                // [開発差分候補/コメントアウト]
                // bool playbackDefaultOk = IsDefaultForRoles(playbackPlantronics.Id, EDataFlow.eRender, new ERole[] { ERole.eConsole, ERole.eMultimedia });
                // bool playbackCommOk = IsDefaultForRoles(playbackVmInput.Id, EDataFlow.eRender, new ERole[] { ERole.eCommunications });
                // bool recordingDefaultOk = IsDefaultForRoles(captureB1.Id, EDataFlow.eCapture, new ERole[] { ERole.eConsole, ERole.eMultimedia });
                // bool recordingCommOk = IsDefaultForRoles(capturePlantronics.Id, EDataFlow.eCapture, new ERole[] { ERole.eCommunications });
                // bool alreadyApplied = playbackDefaultOk && playbackCommOk && recordingDefaultOk && recordingCommOk;
                // if (alreadyApplied)
                // {
                //     AppendLog("自動設定は既に適用済みですが、整合性確認のため再適用を実行します。");
                // }

                AppendLog("[1/5] 再生: Plantronics DA80 (Headset/Earphone) を既定デバイスへ設定します。");
                SetDefaultOrThrow(playbackPlantronics.Id, new ERole[] { ERole.eConsole, ERole.eMultimedia });
                AppendLog("      完了");

                AppendLog("[2/5] 再生: Voicemeeter Input を既定の通信デバイスへ設定します。");
                SetDefaultOrThrow(playbackVmInput.Id, new ERole[] { ERole.eCommunications });
                AppendLog("      完了");

                AppendLog("[3/5] 録音: Voicemeeter Out A1 の有効化を確認します。");
                if ((captureA1.State & 2) == 2)
                {
                    bool visibilityChanged = AudioHelper.SetVisible(captureA1.Id, true);
                    if (visibilityChanged)
                    {
                        AppendLog("      有効化の要求を実行しました。");
                    }
                    else
                    {
                        AppendLog("      自動有効化は失敗しました。必要なら手動で有効化してください。");
                    }
                }
                else
                {
                    AppendLog("      既に有効です。");
                }

                AppendLog("[4/5] 録音: Voicemeeter Out B1 を既定のデバイスへ設定します。");
                SetDefaultOrThrow(captureB1.Id, new ERole[] { ERole.eConsole, ERole.eMultimedia });
                AppendLog("      完了");

                AppendLog("[5/5] 録音: Plantronics DA80 (Mic/Headset) を既定の通信デバイスへ設定します。");
                SetDefaultOrThrow(capturePlantronics.Id, new ERole[] { ERole.eCommunications });
                AppendLog("      完了");

                // [開発差分候補/コメントアウト]
                // DisableNonTargetDevices(playbackPlantronics, playbackVmInput, captureB1, capturePlantronics, captureA1);

                AppendLog(string.Empty);
                AppendLog("すべての設定が完了しました。");
                UpdateDefaultDeviceLabels();
                MessageBox.Show(this, "自動設定が完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendLog(string.Empty);
                AppendException("[ERROR] 自動設定に失敗しました", ex);
                AppendLog("手動フォールバック用にサウンド設定を開きます。");
                Process.Start("control.exe", "mmsys.cpl");
                MessageBox.Show(
                    this,
                    "自動設定に失敗しました。\r\n\r\n" + ex.Message + "\r\n\r\nサウンド設定を開いたので、必要に応じて手動で仕上げてください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void RunRestoreBusinessSetup()
        {
            try
            {
                DeviceInfo playbackHeadset = FindBestPlantronicsRenderHeadset();
                DeviceInfo captureMic = FindBestPlantronicsCaptureMic();

                if (playbackHeadset == null)
                {
                    throw new InvalidOperationException("再生デバイス『Plantronics DA80 Headset/Earphone』が見つかりません。");
                }

                if (captureMic == null)
                {
                    throw new InvalidOperationException("録音デバイス『Plantronics DA80 Mic/Headset』が見つかりません。");
                }

                // [開発差分候補/コメントアウト]
                // bool alreadyApplied = IsBusinessSetupAlreadyApplied(playbackHeadset, captureMic, null, null, null);
                // if (alreadyApplied)
                // {
                //     AppendLog("通常業務構成は既に適用済みですが、整合性確認のため再適用を実行します。");
                // }

                AppendLog("[1/2] 再生: Plantronics DA80 (Headset/Earphone) を既定/通信へ設定します。");
                SetDefaultOrThrow(playbackHeadset.Id, new ERole[] { ERole.eConsole, ERole.eMultimedia, ERole.eCommunications });
                AppendLog("      完了");

                AppendLog("[2/2] 録音: Plantronics DA80 (Mic/Headset) を既定/通信へ設定します。");
                SetDefaultOrThrow(captureMic.Id, new ERole[] { ERole.eConsole, ERole.eMultimedia, ERole.eCommunications });
                AppendLog("      完了");

                AppendLog(string.Empty);
                AppendLog("業務用(Plantronics DA80)への復帰が完了しました。");
                UpdateDefaultDeviceLabels();
                MessageBox.Show(this, "業務用(Plantronics DA80)への復帰が完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendLog(string.Empty);
                AppendException("[ERROR] 業務用(Plantronics DA80)への復帰に失敗しました", ex);
                MessageBox.Show(
                    this,
                    "業務用(Plantronics DA80)への復帰に失敗しました。\r\n\r\n" + ex.Message,
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void UpdateDefaultDeviceLabels()
        {
            DeviceInfo playbackDefault = AudioHelper.GetDefault(EDataFlow.eRender, ERole.eConsole);
            playbackDefaultLabel.Text = "再生 既定: " + FormatDeviceLabel(playbackDefault);
            if (!string.IsNullOrEmpty(AudioHelper.LastDefaultMessage))
            {
                AppendLog("[WARN] " + AudioHelper.LastDefaultMessage);
            }

            DeviceInfo playbackComm = AudioHelper.GetDefault(EDataFlow.eRender, ERole.eCommunications);
            playbackCommLabel.Text = "再生 通信: " + FormatDeviceLabel(playbackComm);
            if (!string.IsNullOrEmpty(AudioHelper.LastDefaultMessage))
            {
                AppendLog("[WARN] " + AudioHelper.LastDefaultMessage);
            }

            DeviceInfo recordingDefault = AudioHelper.GetDefault(EDataFlow.eCapture, ERole.eConsole);
            recordingDefaultLabel.Text = "録音 既定: " + FormatDeviceLabel(recordingDefault);
            if (!string.IsNullOrEmpty(AudioHelper.LastDefaultMessage))
            {
                AppendLog("[WARN] " + AudioHelper.LastDefaultMessage);
            }

            DeviceInfo recordingComm = AudioHelper.GetDefault(EDataFlow.eCapture, ERole.eCommunications);
            recordingCommLabel.Text = "録音 通信: " + FormatDeviceLabel(recordingComm);
            if (!string.IsNullOrEmpty(AudioHelper.LastDefaultMessage))
            {
                AppendLog("[WARN] " + AudioHelper.LastDefaultMessage);
            }
        }

        private static string FormatDeviceLabel(DeviceInfo device)
        {
            if (device == null)
            {
                return "未検出";
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
                    throw new InvalidOperationException("既定デバイスの設定に失敗しました。Role=" + roles[i]);
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

        private DeviceInfo FindBestPlantronicsRenderHeadset()
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

                string normalizedName = NormalizeDeviceNameForMatch(device.Name);
                bool hasDa80 = normalizedName.IndexOf("da80", StringComparison.OrdinalIgnoreCase) >= 0;
                bool hasBrand = normalizedName.IndexOf("plantronics", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                normalizedName.IndexOf("poly", StringComparison.OrdinalIgnoreCase) >= 0;
                if (!hasDa80 && !hasBrand)
                {
                    continue;
                }

                int score = 0;
                if (hasBrand) score += 6;
                if (hasDa80) score += 14;
                if (normalizedName.IndexOf("headset", StringComparison.OrdinalIgnoreCase) >= 0) score += 8;
                if (normalizedName.IndexOf("ヘッドセット", StringComparison.OrdinalIgnoreCase) >= 0) score += 8;
                if (normalizedName.IndexOf("hedset", StringComparison.OrdinalIgnoreCase) >= 0) score += 8;
                if (normalizedName.IndexOf("earphone", StringComparison.OrdinalIgnoreCase) >= 0) score += 8;
                if (normalizedName.IndexOf("ertphone", StringComparison.OrdinalIgnoreCase) >= 0) score += 8;
                if (normalizedName.IndexOf("speaker", StringComparison.OrdinalIgnoreCase) >= 0) score += 3;
                if (normalizedName.IndexOf("スピーカー", StringComparison.OrdinalIgnoreCase) >= 0) score += 3;
                if (normalizedName.IndexOf("mic", StringComparison.OrdinalIgnoreCase) >= 0 || normalizedName.IndexOf("microphone", StringComparison.OrdinalIgnoreCase) >= 0 || normalizedName.IndexOf("マイク", StringComparison.OrdinalIgnoreCase) >= 0) score -= 4;
                if ((device.State & 1) == 1) score += 1;

                if (score > bestScore)
                {
                    best = device;
                    bestScore = score;
                }
            }

            return best;
        }

        private DeviceInfo FindBestPlantronicsCaptureMic()
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

                string normalizedName = NormalizeDeviceNameForMatch(device.Name);
                bool hasDa80 = normalizedName.IndexOf("da80", StringComparison.OrdinalIgnoreCase) >= 0;
                bool hasBrand = normalizedName.IndexOf("plantronics", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                normalizedName.IndexOf("poly", StringComparison.OrdinalIgnoreCase) >= 0;
                if (!hasDa80 && !hasBrand)
                {
                    continue;
                }

                int score = 0;
                if (hasBrand) score += 6;
                if (hasDa80) score += 14;
                if (normalizedName.IndexOf("mic", StringComparison.OrdinalIgnoreCase) >= 0) score += 8;
                if (normalizedName.IndexOf("microphone", StringComparison.OrdinalIgnoreCase) >= 0) score += 8;
                if (normalizedName.IndexOf("マイク", StringComparison.OrdinalIgnoreCase) >= 0) score += 8;
                if (normalizedName.IndexOf("headset", StringComparison.OrdinalIgnoreCase) >= 0) score += 2;
                if (normalizedName.IndexOf("ヘッドセット", StringComparison.OrdinalIgnoreCase) >= 0) score += 2;
                if (normalizedName.IndexOf("hedset", StringComparison.OrdinalIgnoreCase) >= 0) score += 2;
                if ((device.State & 1) == 1) score += 1;

                if (score > bestScore)
                {
                    best = device;
                    bestScore = score;
                }
            }

            return best;
        }

        private static string NormalizeDeviceNameForMatch(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            string normalized = source.Normalize(NormalizationForm.FormKC);
            normalized = normalized.Replace("　", " ");
            normalized = Regex.Replace(normalized, "[\\s\\-_/()\\[\\]\\.]", string.Empty);
            return normalized.ToLowerInvariant();
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
            if ((state & 2) == 2) return "無効";
            if ((state & 8) == 8) return "無効(未接続)";
            if ((state & 4) == 4) return "無効(未接続)";
            if ((state & 1) == 1) return "有効";
            return "不明";
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
                ex = new Exception("不明な例外が発生しました。");
            }

            ShowFatalError(ex);
        }

        private static void ShowFatalError(Exception ex)
        {
            MessageBox.Show(
                "ツールの起動または実行中にエラーが発生しました。\r\n\r\n" + ex.Message,
                "Windows サウンド設定ツール",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}

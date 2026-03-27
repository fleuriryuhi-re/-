using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

[assembly: AssemblyCompany("KDDICorporation AI戦略推進部")]
[assembly: AssemblyProduct("Windows サウンド設定ツール")]
[assembly: AssemblyCopyright("KDDICorporation AI戦略推進部")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0.0")]
[assembly: NeutralResourcesLanguage("ja-JP")]

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

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetDllDirectory(string lpPathName);
    }

    internal static class VoicemeeterHelper
    {
        public static bool TryStartVoicemeeterX64(out string message)
        {
            string[] runningNames = new string[] { "voicemeeter_x64", "voicemeeter64", "voicemeeterx64" };
            int r;
            for (r = 0; r < runningNames.Length; r++)
            {
                if (Process.GetProcessesByName(runningNames[r]).Length > 0)
                {
                    message = "Voicemeeter x64 は既に起動しています。(" + runningNames[r] + ")";
                    return true;
                }
            }

            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string[] exeCandidates = new string[]
            {
                Path.Combine(programFiles, "VB", "Voicemeeter", "Voicemeeter_x64.exe"),
                Path.Combine(programFilesX86, "VB", "Voicemeeter", "Voicemeeter_x64.exe")
            };

            int i;
            for (i = 0; i < exeCandidates.Length; i++)
            {
                string exePath = exeCandidates[i];
                if (!File.Exists(exePath))
                {
                    continue;
                }

                Process.Start(exePath);
                message = "Voicemeeter x64 の起動要求を送信しました。(" + Path.GetFileName(exePath) + ")";
                return true;
            }

            message = "Voicemeeter_x64.exe が見つかりません。";
            return false;
        }

        public static bool TryStopVoicemeeterX64(out string message)
        {
            string[] runningNames = new string[] { "voicemeeter_x64", "voicemeeter64", "voicemeeterx64" };
            int i;
            bool requestedAny = false;
            bool allExited = true;
            for (i = 0; i < runningNames.Length; i++)
            {
                Process[] list = Process.GetProcessesByName(runningNames[i]);
                int j;
                for (j = 0; j < list.Length; j++)
                {
                    try
                    {
                        Process p = list[j];
                        if (p.HasExited)
                        {
                            continue;
                        }

                        // 強制終了ではなく通常終了を依頼する
                        bool closeRequested = p.CloseMainWindow();
                        if (closeRequested)
                        {
                            requestedAny = true;
                            if (!p.WaitForExit(5000))
                            {
                                allExited = false;
                            }
                        }
                        else
                        {
                            // メインウィンドウが取得できない場合は強制終了せず失敗扱い
                            allExited = false;
                        }
                    }
                    catch { }
                }
            }

            if (requestedAny && allExited)
            {
                message = "Voicemeeter に通常終了を依頼し、終了を確認しました。";
                return true;
            }

            if (requestedAny && !allExited)
            {
                message = "Voicemeeter に通常終了を依頼しましたが、終了確認できないプロセスがあります。";
                return false;
            }

            // プロセスは存在するが通常終了依頼できないケース（ウィンドウ未取得等）
            for (i = 0; i < runningNames.Length; i++)
            {
                if (Process.GetProcessesByName(runningNames[i]).Length > 0)
                {
                    message = "Voicemeeter は起動中ですが、通常終了を依頼できませんでした。";
                    return false;
                }
            }

            message = "Voicemeeter は起動していません。";
            return true;
        }

        public static bool IsVoicemeeterX64Running()
        {
            string[] runningNames = new string[] { "voicemeeter_x64", "voicemeeter64", "voicemeeterx64" };
            int i;
            for (i = 0; i < runningNames.Length; i++)
            {
                if (Process.GetProcessesByName(runningNames[i]).Length > 0)
                {
                    return true;
                }
            }

            return false;
        }
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
                    LastDefaultMessage = BuildDefaultEndpointErrorMessage(flow, role, hr);
                    return null;
                }

                return CreateDeviceInfo(device, flow);
            }
            catch (Exception ex)
            {
                LastDefaultMessage = "既定デバイス取得に失敗しました。Flow=" + flow + ", Role=" + role + ", 例外=" + ex.GetType().FullName + ", 詳細: " + ex.Message;
                return null;
            }
        }

        private static string BuildDefaultEndpointErrorMessage(EDataFlow flow, ERole role, int hr)
        {
            StringBuilder message = new StringBuilder();
            message.Append("既定デバイス取得に失敗しました。Flow=");
            message.Append(flow);
            message.Append(", Role=");
            message.Append(role);
            message.Append(", HRESULT=0x");
            message.Append(((uint)hr).ToString("X8"));

            string detail = GetHResultMessage(hr);
            if (!string.IsNullOrEmpty(detail))
            {
                message.Append(", 詳細: ");
                message.Append(detail);
            }

            if (hr == unchecked((int)0x80070490))
            {
                message.Append("。既定デバイス未設定、または対象ロールに対応するデバイスが存在しない可能性があります。");
            }

            return message.ToString();
        }

        private static string GetHResultMessage(int hr)
        {
            try
            {
                Exception hrException = Marshal.GetExceptionForHR(hr);
                if (hrException == null)
                {
                    return null;
                }

                return hrException.Message;
            }
            catch
            {
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

        public static List<DeviceInfo> EnumerateCom(EDataFlow flow, int stateMask)
        {
            LastEnumerationMessage = null;
            List<DeviceInfo> list = new List<DeviceInfo>();

            try
            {
                IMMDeviceEnumerator enumerator = (IMMDeviceEnumerator)(new MMDeviceEnumeratorComObject());
                IMMDeviceCollection devices;
                int hr = enumerator.EnumAudioEndpoints(flow, stateMask, out devices);
                if (hr != 0)
                {
                    LastEnumerationMessage = "COM 列挙に失敗しました。Flow=" + flow + ", HRESULT=0x" + ((uint)hr).ToString("X8");
                    return list;
                }

                int count;
                hr = devices.GetCount(out count);
                if (hr != 0)
                {
                    LastEnumerationMessage = "COM デバイス数取得に失敗しました。Flow=" + flow + ", HRESULT=0x" + ((uint)hr).ToString("X8");
                    return list;
                }

                int i;
                for (i = 0; i < count; i++)
                {
                    IMMDevice device;
                    hr = devices.Item(i, out device);
                    if (hr != 0 || device == null)
                    {
                        continue;
                    }

                    try
                    {
                        list.Add(CreateDeviceInfo(device, flow));
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                LastEnumerationMessage = "COM からのデバイス列挙に失敗しました。 詳細: " + ex.Message;
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
        private Label voicemeeterA1Label;

        public MainForm()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.Shown += MainForm_Shown;
            this.Resize += MainForm_Resize;
            this.FormClosing += MainForm_FormClosing;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && VoicemeeterHelper.IsVoicemeeterX64Running())
            {
                DialogResult closeResult = MessageBox.Show(
                    this,
                    "Voicemeeter x64 が起動中です。\r\nこのまま閉じると Voicemeeter を終了します。\r\n\r\n本当に終了しますか？",
                    "警告",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);

                if (closeResult != DialogResult.OK)
                {
                    e.Cancel = true;
                    return;
                }
            }

            string message;
            VoicemeeterHelper.TryStopVoicemeeterX64(out message);
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
            summaryLabel.Size = new Size(1170, 130);
            summaryLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            summaryLabel.Text =
                "自動設定内容 [開発用]\r\n" +
                "1. 再生: ヘッドホン (Realtek) -> 既定デバイス\r\n" +
                "2. 再生: Voicemeeter Input -> 既定の通信デバイス\r\n" +
                "3. 録音: Voicemeeter Out A1 -> 無効なら有効化\r\n" +
                "4. 録音: Voicemeeter Out B1 -> 既定デバイス\r\n" +
                "5. 録音: 外付けマイク (Realtek) -> 既定の通信デバイス\r\n" +
                "※ 同一状態でも整合性確認のため再設定を行います";

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
            restoreBusinessButton.Text = "通常業務へ戻す (Voicemeeter無効化 + Realtek既定)";
            restoreBusinessButton.Location = new Point(765, 130);
            restoreBusinessButton.Size = new Size(425, 38);
            restoreBusinessButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            restoreBusinessButton.Click += RestoreBusinessButton_Click;

            defaultGroupBox = new GroupBox();
            defaultGroupBox.Text = "検出された既定デバイス";
            defaultGroupBox.Location = new Point(20, 180);
            defaultGroupBox.Size = new Size(1170, 180);
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

            voicemeeterA1Label = new Label();
            voicemeeterA1Label.AutoSize = false;
            voicemeeterA1Label.Location = new Point(15, 144);
            voicemeeterA1Label.Size = new Size(1130, 26);
            voicemeeterA1Label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            defaultGroupBox.Controls.Add(playbackDefaultLabel);
            defaultGroupBox.Controls.Add(playbackCommLabel);
            defaultGroupBox.Controls.Add(recordingDefaultLabel);
            defaultGroupBox.Controls.Add(recordingCommLabel);
            defaultGroupBox.Controls.Add(voicemeeterA1Label);

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

            this.Controls.Add(summaryLabel);
            this.Controls.Add(applyButton);
            this.Controls.Add(refreshButton);
            this.Controls.Add(openSoundButton);
            this.Controls.Add(restoreBusinessButton);
            this.Controls.Add(defaultGroupBox);
            this.Controls.Add(playbackGroupBox);
            this.Controls.Add(recordingGroupBox);
            this.Controls.Add(logBox);

            LayoutResponsiveControls();
        }

        private void LayoutResponsiveControls()
        {
            int margin = 20;
            int sectionGap = 15;
            int buttonGap = 15;
            int contentWidth = Math.Max(900, this.ClientSize.Width - (margin * 2));

            summaryLabel.Location = new Point(margin, margin);
            summaryLabel.Size = new Size(contentWidth, 130);

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
            defaultGroupBox.Size = new Size(contentWidth, 180);

            int defaultLabelWidth = defaultGroupBox.ClientSize.Width - 30;
            playbackDefaultLabel.Location = new Point(15, 24);
            playbackDefaultLabel.Size = new Size(defaultLabelWidth, 26);
            playbackCommLabel.Location = new Point(15, 54);
            playbackCommLabel.Size = new Size(defaultLabelWidth, 26);
            recordingDefaultLabel.Location = new Point(15, 84);
            recordingDefaultLabel.Size = new Size(defaultLabelWidth, 26);
            recordingCommLabel.Location = new Point(15, 114);
            recordingCommLabel.Size = new Size(defaultLabelWidth, 26);
            voicemeeterA1Label.Location = new Point(15, 144);
            voicemeeterA1Label.Size = new Size(defaultLabelWidth, 26);

            int listsTop = defaultGroupBox.Bottom + sectionGap;
            int bottomMargin = 20;
            int logGap = 15;
            int remainingHeight = this.ClientSize.Height - listsTop - bottomMargin;
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
            catch (Exception ex)
            {
                AppendException("[ERROR] 自動設定処理で予期しないエラーが発生しました", ex);
                MessageBox.Show(
                    this,
                    "自動設定処理で予期しないエラーが発生しました。\r\n\r\n" + ex.Message,
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
            catch (Exception ex)
            {
                AppendException("[ERROR] 通常業務復帰処理で予期しないエラーが発生しました", ex);
                MessageBox.Show(
                    this,
                    "通常業務復帰処理で予期しないエラーが発生しました。\r\n\r\n" + ex.Message,
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
            TryOpenSoundSettings();
        }

        private void TryOpenSoundSettings()
        {
            try
            {
                Process.Start("control.exe", "mmsys.cpl");
            }
            catch (Exception ex)
            {
                AppendException("[ERROR] サウンド設定を開けませんでした", ex);
                MessageBox.Show(
                    this,
                    "Windows サウンド設定を開けませんでした。\r\n\r\n" +
                    "[詳細]\r\n" + ex.Message,
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void RefreshDevices()
        {
            this.Update();
            playbackList.Items.Clear();
            recordingList.Items.Clear();
            logBox.AppendText("デバイス一覧を更新しています...\r\n");

            SafeAddDevicesToList(EDataFlow.eRender, "再生");
            SafeAddDevicesToList(EDataFlow.eCapture, "録音");
            SafeUpdateDefaultDeviceLabels();

            if (AreAllVisibleDevicesDisabled())
            {
                AppendLog("[WARN] 再生/録音デバイスがすべて無効状態です。マニュアルを確認して有効化してください。");
                DialogResult dialogResult = MessageBox.Show(
                    this,
                    "再生/録音デバイスがすべて無効状態です。\r\n\r\n" +
                    "[対応]\r\n" +
                    "1. 仮想オーディオデバイス操作マニュアルの3ページを確認してください。\r\n" +
                    "2. 必要なデバイスを有効化してください。\r\n\r\n" +
                    "OK を押すと Windows サウンド設定を開きます。",
                    "デバイス有効化が必要です",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.OK)
                {
                    TryOpenSoundSettings();
                }
            }

            AppendLog("デバイス一覧の更新が終了しました。");
        }

        private bool AreAllVisibleDevicesDisabled()
        {
            List<DeviceInfo> renderDevices = AudioHelper.Enumerate(EDataFlow.eRender);
            List<DeviceInfo> captureDevices = AudioHelper.Enumerate(EDataFlow.eCapture);

            bool hasVisibleDevices = false;
            int i;
            for (i = 0; i < renderDevices.Count; i++)
            {
                DeviceInfo device = renderDevices[i];
                if (!IsVisibleInMmsysList(device))
                {
                    continue;
                }

                hasVisibleDevices = true;
                if ((device.State & 1) == 1)
                {
                    return false;
                }
            }

            for (i = 0; i < captureDevices.Count; i++)
            {
                DeviceInfo device = captureDevices[i];
                if (!IsVisibleInMmsysList(device))
                {
                    continue;
                }

                hasVisibleDevices = true;
                if ((device.State & 1) == 1)
                {
                    return false;
                }
            }

            return hasVisibleDevices;
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
                voicemeeterA1Label.Text = "Voicemeeter Out A1: 取得失敗";
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
                voicemeeterA1Label.Text = "Voicemeeter Out A1: 取得失敗";
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
                DeviceInfo playbackHeadphones = FindBestRealtekHeadphones();
                DeviceInfo playbackTarget = playbackHeadphones;
                DeviceInfo playbackVmInput = FindFirstMatch(EDataFlow.eRender, new string[] { "Voicemeeter Input" });
                DeviceInfo captureA1 = FindFirstMatch(EDataFlow.eCapture, new string[] { "Voicemeeter Out A1" });
                DeviceInfo captureB1 = FindFirstMatch(EDataFlow.eCapture, new string[] { "Voicemeeter Out B1" });
                DeviceInfo captureMic = FindBestRealtekExternalMic();
                DeviceInfo captureCommTarget = captureMic;

                if (playbackTarget == null)
                {
                    StringBuilder missingRealtekMessage = new StringBuilder();
                    missingRealtekMessage.Append("自動設定に必要な Realtek デバイスが見つかりません。");
                    missingRealtekMessage.Append("\r\n\r\n[未検出Realtek]");
                    missingRealtekMessage.Append("\r\n - 再生デバイス (ヘッドホン Realtek)");
                    throw new InvalidOperationException(missingRealtekMessage.ToString());
                }
                if (playbackVmInput == null)
                {
                    throw new InvalidOperationException(
                        "再生デバイス『Voicemeeter Input』が見つかりません。Voicemeeter がインストールされていない可能性があります。\r\n" +
                        "VB-Audio 公式サイトから Voicemeeter x64 をインストールし、PC 再起動後に再実行してください。");
                }
                if (captureA1 == null)
                {
                    throw new InvalidOperationException(
                        "録音デバイス『Voicemeeter Out A1』が見つかりません。Voicemeeter がインストールされていない可能性があります。\r\n" +
                        "VB-Audio 公式サイトから Voicemeeter x64 をインストールし、PC 再起動後に再実行してください。");
                }
                if (captureB1 == null)
                {
                    throw new InvalidOperationException(
                        "録音デバイス『Voicemeeter Out B1』が見つかりません。Voicemeeter がインストールされていない可能性があります。\r\n" +
                        "VB-Audio 公式サイトから Voicemeeter x64 をインストールし、PC 再起動後に再実行してください。");
                }
                if (captureCommTarget == null)
                {
                    StringBuilder missingRealtekMessage = new StringBuilder();
                    missingRealtekMessage.Append("自動設定に必要な Realtek デバイスが見つかりません。");
                    missingRealtekMessage.Append("\r\n\r\n[未検出Realtek]");
                    missingRealtekMessage.Append("\r\n - 録音デバイス (外付けマイク Realtek)");
                    throw new InvalidOperationException(missingRealtekMessage.ToString());
                }

                bool headphoneNotConnected = (playbackTarget.State & 4) == 4 || (playbackTarget.State & 8) == 8;
                if (headphoneNotConnected)
                {
                    StringBuilder missingRealtekMessage = new StringBuilder();
                    missingRealtekMessage.Append("自動設定に必要な Realtek ヘッドホンが接続されていません。");
                    missingRealtekMessage.Append("\r\n\r\n[未検出Realtek]");
                    missingRealtekMessage.Append("\r\n - 再生デバイス (ヘッドホン Realtek): ");
                    missingRealtekMessage.Append(GetStateLabel(playbackTarget.State));
                    throw new InvalidOperationException(missingRealtekMessage.ToString());
                }

                // Debug: Check each condition separately
                bool playbackDefaultOk = IsDefaultForRoles(playbackTarget.Id, EDataFlow.eRender, new ERole[] { ERole.eConsole, ERole.eMultimedia });
                bool playbackCommOk = IsDefaultForRoles(playbackVmInput.Id, EDataFlow.eRender, new ERole[] { ERole.eCommunications });
                bool recordingDefaultOk = IsDefaultForRoles(captureB1.Id, EDataFlow.eCapture, new ERole[] { ERole.eConsole, ERole.eMultimedia });
                bool recordingCommOk = IsDefaultForRoles(captureCommTarget.Id, EDataFlow.eCapture, new ERole[] { ERole.eCommunications });
                
                bool alreadyApplied = playbackDefaultOk && playbackCommOk && recordingDefaultOk && recordingCommOk;
                if (alreadyApplied)
                {
                    AppendLog("自動設定は既に適用済みですが、整合性確認のため再適用を実行します。");
                }

                AppendLog("[前処理-0] Realtek デバイスの有効化を確認します。");
                EnsureEnabledOrThrow(playbackTarget, "再生デバイス(Realtek)");
                EnsureEnabledOrThrow(captureCommTarget, "録音デバイス(Realtek)");

                AppendLog("[前処理-2] 通信録音デバイスの有効化を確認します。");
                if (EnsureEnabled(captureCommTarget))
                {
                    AppendLog("      反映のため少々待機します...");
                    System.Threading.Thread.Sleep(1200);
                }
                else
                {
                    AppendLog("      既に有効です。");
                }

                // [前処理] Voicemeeter デバイスを先に全て有効化してから既定設定を行う
                AppendLog("[前処理] Voicemeeter デバイスの有効化を確認します。");
                bool anyEnabled = false;
                anyEnabled |= EnsureEnabled(playbackVmInput);
                anyEnabled |= EnsureEnabled(captureA1);
                anyEnabled |= EnsureEnabled(captureB1);
                if (anyEnabled)
                {
                    AppendLog("      Windows に反映させるため少々待機します...");
                    System.Threading.Thread.Sleep(1500);
                }
                else
                {
                    AppendLog("      全デバイス既に有効です。");
                }

                AppendLog("[1/5] 再生: " + playbackTarget.Name + " を既定のデバイスへ設定します。");
                SetDefaultOrThrow(playbackTarget.Id, new ERole[] { ERole.eConsole, ERole.eMultimedia });
                AppendLog("      完了");

                AppendLog("[2/5] 再生: Voicemeeter Input を既定の通信デバイスへ設定します。");
                SetDefaultOrThrow(playbackVmInput.Id, new ERole[] { ERole.eCommunications });
                AppendLog("      完了");

                AppendLog("[3/5] 録音: Voicemeeter Out A1 の有効化を確認します。");
                AppendLog("      有効化済みです。");

                AppendLog("[4/5] 録音: Voicemeeter Out B1 を既定のデバイスへ設定します。");
                SetDefaultOrThrow(captureB1.Id, new ERole[] { ERole.eConsole, ERole.eMultimedia });
                AppendLog("      完了");

                AppendLog("[5/5] 録音: " + captureCommTarget.Name + " を既定の通信デバイスへ設定します。");
                SetDefaultOrThrow(captureCommTarget.Id, new ERole[] { ERole.eCommunications });
                AppendLog("      完了");

                DisableNonTargetDevices(playbackTarget, playbackVmInput, captureB1, captureCommTarget, captureA1);

                AppendLog(string.Empty);
                AppendLog("すべての設定が完了しました。");
                UpdateDefaultDeviceLabels();

                AppendLog("[後処理] 完了ダイアログ表示中に Voicemeeter x64 を起動します。");
                string vmStartMessage;
                if (!VoicemeeterHelper.TryStartVoicemeeterX64(out vmStartMessage))
                {
                    AppendImportantLog("      [WARN] " + vmStartMessage);
                }
                else
                {
                    AppendImportantLog("      " + vmStartMessage);
                }

                if (alreadyApplied)
                {
                    MessageBox.Show(this, "現在の構成は既に自動設定済みです。\r\n整合性確認として再適用しました。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "自動設定が完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                AppendLog(string.Empty);
                AppendException("[ERROR] 自動設定に失敗しました", ex);
                AppendLog("手動フォールバック用にサウンド設定を開きます。");

                bool isMissingRealtekError =
                    ex is InvalidOperationException &&
                    ex.Message != null &&
                    ex.Message.IndexOf("[未検出Realtek]", StringComparison.OrdinalIgnoreCase) >= 0;

                if (isMissingRealtekError)
                {
                    DialogResult missingDialogResult = MessageBox.Show(
                        this,
                        "自動設定に失敗しました。\r\n\r\n" +
                        "[原因]\r\n" + ex.Message + "\r\n\r\n" +
                        "[対応]\r\n" +
                        "1. Realtek デバイスの接続状態を確認してください。\r\n" +
                        "2. 仮想オーディオデバイス操作マニュアルの3ページを確認してください。\r\n" +
                        "3. OK を押すと Windows サウンド設定を開きます。",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    if (missingDialogResult == DialogResult.OK)
                    {
                        TryOpenSoundSettings();
                    }
                    return;
                }

                DialogResult dialogResult = MessageBox.Show(
                    this,
                    "自動設定に失敗しました。\r\n\r\n" +
                    "[原因]\r\n" + ex.Message + "\r\n\r\n" +
                    "[対応]\r\n" +
                    "1. OK を押すと Windows サウンド設定を開きます。\r\n" +
                    "2. 仮想オーディオデバイス操作マニュアルの3ページを確認してください。\r\n" +
                    "3. 必要なデバイスを有効化後、再実行してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.OK)
                {
                    TryOpenSoundSettings();
                }
            }
        }

        private bool EnsureEnabled(DeviceInfo device)
        {
            if ((device.State & 2) == 2)
            {
                bool ok = AudioHelper.SetVisible(device.Id, true);
                string label = device.Name ?? device.Id;
                AppendLog("      " + label + ": " + (ok ? "有効化しました。" : "有効化に失敗しました。手動で有効化してください。"));
                return ok;
            }
            return false;
        }

        private void EnsureEnabledOrThrow(DeviceInfo device, string category)
        {
            if (device == null || string.IsNullOrEmpty(device.Id))
            {
                throw new InvalidOperationException(category + " の対象デバイス情報が不正です。");
            }

            DeviceInfo current = ResolveCurrentDeviceByIdOrName(device) ?? device;
            if (IsDeviceUnplugged(current.State))
            {
                throw new InvalidOperationException("[未接続デバイス]\r\n - " + category + " " + current.Name + ": " + GetStateLabel(current.State));
            }

            if (IsDeviceActive(current.State))
            {
                AppendLog("      " + category + " " + current.Name + ": 既に有効です。");
                return;
            }

            bool ok = AudioHelper.SetVisible(current.Id, true);
            if (!ok)
            {
                throw new InvalidOperationException(category + " " + current.Name + " の有効化に失敗しました。現在状態: " + GetStateLabel(current.State) + "。サウンド設定から手動で有効化してください。");
            }

            DeviceInfo refreshed = WaitAndResolveDevice(device);
            if (refreshed == null)
            {
                throw new InvalidOperationException(category + " の有効化後にデバイスを再取得できませんでした。サウンド設定を確認してください。");
            }

            if (!IsDeviceActive(refreshed.State))
            {
                throw new InvalidOperationException(category + " " + refreshed.Name + " は有効化後も利用可能状態になっていません。現在状態: " + GetStateLabel(refreshed.State));
            }

            AppendLog("      " + category + " " + refreshed.Name + ": 有効化しました。");
            device.Id = refreshed.Id;
            device.Name = refreshed.Name;
            device.State = refreshed.State;
            System.Threading.Thread.Sleep(1200);
        }

        private void EnsureEnabledIfPresent(DeviceInfo device, string category)
        {
            if (device == null || string.IsNullOrEmpty(device.Id))
            {
                AppendLog("      " + category + ": 対象デバイスが見つからないためスキップします。");
                return;
            }

            DeviceInfo current = ResolveCurrentDeviceByIdOrName(device) ?? device;
            if (IsDeviceUnplugged(current.State))
            {
                AppendLog("      [WARN] " + category + " " + current.Name + ": 未接続のため自動有効化をスキップします。現在状態: " + GetStateLabel(current.State));
                return;
            }

            if (IsDeviceActive(current.State))
            {
                AppendLog("      " + category + " " + current.Name + ": 既に有効です。");
                return;
            }

            bool ok = AudioHelper.SetVisible(current.Id, true);
            if (!ok)
            {
                AppendLog("      [WARN] " + category + " " + current.Name + " の有効化に失敗しました。現在状態: " + GetStateLabel(current.State) + "。必要なら手動で有効化してください。");
                return;
            }

            DeviceInfo refreshed = WaitAndResolveDevice(device);
            if (refreshed == null)
            {
                AppendLog("      [WARN] " + category + " の有効化後にデバイス再取得ができませんでした。必要なら手動で確認してください。");
                return;
            }

            if (!IsDeviceActive(refreshed.State))
            {
                AppendLog("      [WARN] " + category + " " + refreshed.Name + " は有効化後も利用可能状態になっていません。現在状態: " + GetStateLabel(refreshed.State));
                return;
            }

            AppendLog("      " + category + " " + refreshed.Name + ": 有効化しました。");
            device.Id = refreshed.Id;
            device.Name = refreshed.Name;
            device.State = refreshed.State;
            System.Threading.Thread.Sleep(1200);
        }

        private void EnsureAllRealtekSpeakersEnabled()
        {
            List<DeviceInfo> devices = AudioHelper.Enumerate(EDataFlow.eRender);
            bool hasRealtekSpeaker = false;
            bool enabledAny = false;

            int i;
            for (i = 0; i < devices.Count; i++)
            {
                DeviceInfo device = devices[i];
                if (device == null || string.IsNullOrEmpty(device.Id) || string.IsNullOrEmpty(device.Name))
                {
                    continue;
                }

                if (!IsRealtekSpeakerName(device.Name))
                {
                    continue;
                }

                hasRealtekSpeaker = true;
                DeviceInfo current = ResolveCurrentDeviceByIdOrName(device) ?? device;
                if (IsDeviceUnplugged(current.State))
                {
                    AppendLog("      [WARN] 再生デバイス(スピーカー Realtek) " + current.Name + ": 未接続のためスキップします。現在状態: " + GetStateLabel(current.State));
                    continue;
                }

                if (IsDeviceActive(current.State))
                {
                    AppendLog("      再生デバイス(スピーカー Realtek) " + current.Name + ": 既に有効です。");
                    continue;
                }

                bool ok = AudioHelper.SetVisible(current.Id, true);
                if (ok)
                {
                    AppendLog("      再生デバイス(スピーカー Realtek) " + current.Name + ": 有効化しました。");
                    enabledAny = true;
                }
                else
                {
                    AppendLog("      [WARN] 再生デバイス(スピーカー Realtek) " + current.Name + " の有効化に失敗しました。必要なら手動で有効化してください。");
                }
            }

            if (!hasRealtekSpeaker)
            {
                AppendLog("      再生デバイス(スピーカー Realtek): 対象デバイスが見つからないためスキップします。");
                return;
            }

            if (enabledAny)
            {
                System.Threading.Thread.Sleep(900);
            }
        }

        private static bool IsRealtekSpeakerName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            bool hasRealtek = name.IndexOf("Realtek", StringComparison.OrdinalIgnoreCase) >= 0;
            bool hasSpeaker =
                name.IndexOf("スピーカー", StringComparison.OrdinalIgnoreCase) >= 0 ||
                name.IndexOf("Speaker", StringComparison.OrdinalIgnoreCase) >= 0;

            return hasRealtek && hasSpeaker;
        }

        private static bool IsDeviceActive(int state)
        {
            return (state & 1) == 1 && (state & 2) != 2 && (state & 4) != 4 && (state & 8) != 8;
        }

        private static bool IsDeviceUnplugged(int state)
        {
            return (state & 4) == 4 || (state & 8) == 8;
        }

        private DeviceInfo WaitAndResolveDevice(DeviceInfo expected)
        {
            int attempt;
            for (attempt = 0; attempt < 4; attempt++)
            {
                System.Threading.Thread.Sleep(350);
                DeviceInfo refreshed = ResolveCurrentDeviceByIdOrName(expected);
                if (refreshed != null)
                {
                    return refreshed;
                }
            }

            return null;
        }

        private void RunRestoreBusinessSetup()
        {
            try
            {
                DeviceInfo vmInput  = FindFirstMatch(EDataFlow.eRender,  new string[] { "Voicemeeter Input" });
                DeviceInfo captureA1 = FindFirstMatch(EDataFlow.eCapture, new string[] { "Voicemeeter Out A1" });
                DeviceInfo captureB1 = FindFirstMatch(EDataFlow.eCapture, new string[] { "Voicemeeter Out B1" });
                DeviceInfo playbackHeadphones = FindBestRealtekHeadphones();
                DeviceInfo playbackSpeakers = FindBestRealtekSpeakers();
                DeviceInfo playbackIntelSpeakers = FindBestIntelSmartSoundSpeakers();
                DeviceInfo captureMic = FindBestRealtekExternalMic();
                DeviceInfo captureArrayMic = FindBestIntelSmartSoundArrayMic();
                DeviceInfo playbackPrimary = ResolveBusinessPlaybackPrimary(playbackHeadphones, playbackSpeakers, playbackIntelSpeakers);
                DeviceInfo capturePrimary = ResolveBusinessCapturePrimary(captureMic, captureArrayMic);

                List<string> missingDevices = new List<string>();
                if (playbackPrimary == null)
                {
                    missingDevices.Add("再生デバイス (ヘッドホン/スピーカー Realtek または スピーカー Intel SST)");
                }
                if (capturePrimary == null)
                {
                    missingDevices.Add("録音デバイス (外付けマイク Realtek または マイク配列 Intel SST)");
                }

                if (missingDevices.Count > 0)
                {
                    StringBuilder missingMessage = new StringBuilder();
                    missingMessage.Append("通常業務へ戻す に必要なデバイスが見つかりません。");
                    missingMessage.Append("\r\n\r\n[未検出デバイス]");
                    int i;
                    for (i = 0; i < missingDevices.Count; i++)
                    {
                        missingMessage.Append("\r\n - ");
                        missingMessage.Append(missingDevices[i]);
                    }

                    throw new InvalidOperationException(missingMessage.ToString());
                }

                bool alreadyApplied = IsBusinessSetupAlreadyApplied(playbackPrimary, capturePrimary, vmInput, captureA1, captureB1);
                if (alreadyApplied)
                {
                    AppendLog("通常業務構成は既に適用済みですが、整合性確認のため再適用を実行します。");
                }

                AppendLog("[前処理] Voicemeeter を終了します。");
                string vmStopMessage;
                if (!VoicemeeterHelper.TryStopVoicemeeterX64(out vmStopMessage))
                {
                    AppendLog("      [WARN] " + vmStopMessage);
                }
                else
                {
                    AppendLog("      " + vmStopMessage);
                }

                AppendLog("[1/5] 再生: Voicemeeter Input を無効化します。");
                if (vmInput != null)
                {
                    bool ok = AudioHelper.SetVisible(vmInput.Id, false);
                    AppendLog(ok ? "      完了" : "      自動無効化は失敗しました。必要なら手動で無効化してください。");
                }
                else
                {
                    AppendLog("      デバイスが見つからないためスキップします。");
                }

                AppendLog("[2/5] 録音: Voicemeeter Out A1 を無効化します。");
                if (captureA1 != null)
                {
                    bool ok = AudioHelper.SetVisible(captureA1.Id, false);
                    AppendLog(ok ? "      完了" : "      自動無効化は失敗しました。必要なら手動で無効化してください。");
                }
                else
                {
                    AppendLog("      デバイスが見つからないためスキップします。");
                }

                AppendLog("[3/5] 録音: Voicemeeter Out B1 を無効化します。");
                if (captureB1 != null)
                {
                    bool ok = AudioHelper.SetVisible(captureB1.Id, false);
                    AppendLog(ok ? "      完了" : "      自動無効化は失敗しました。必要なら手動で無効化してください。");
                }
                else
                {
                    AppendLog("      デバイスが見つからないためスキップします。");
                }

                AppendLog("[前処理-2] Realtek スピーカーの有効化を確認します。");
                EnsureAllRealtekSpeakersEnabled();
                playbackSpeakers = FindBestRealtekSpeakers();
                playbackPrimary = ResolvePlaybackPrimaryForBusiness(playbackPrimary, playbackHeadphones, playbackSpeakers, playbackIntelSpeakers);

                playbackPrimary = ResolveCurrentDeviceByIdOrName(playbackPrimary) ?? playbackPrimary;
                capturePrimary = ResolveCurrentDeviceByIdOrName(capturePrimary) ?? capturePrimary;

                AppendLog("[前処理-3] 業務用デバイスの有効化を確認します。");
                EnsureEnabledOrThrow(playbackPrimary, "再生デバイス");
                EnsureEnabledOrThrow(capturePrimary, "録音デバイス");

                AppendLog("[前処理-4] 追加候補デバイスの有効化を確認します。");
                EnsureEnabledIfPresent(playbackHeadphones, "再生デバイス(ヘッドホン Realtek)");
                EnsureEnabledIfPresent(playbackSpeakers, "再生デバイス(スピーカー Realtek)");
                EnsureEnabledIfPresent(playbackIntelSpeakers, "再生デバイス(スピーカー Intel SST)");
                EnsureEnabledIfPresent(captureMic, "録音デバイス(外付けマイク Realtek)");
                EnsureEnabledIfPresent(captureArrayMic, "録音デバイス(マイク配列 Intel SST)");

                DeviceInfo resolvedPlaybackPrimary = ResolveCurrentDeviceByIdOrName(playbackPrimary);
                if (resolvedPlaybackPrimary != null)
                {
                    playbackPrimary = resolvedPlaybackPrimary;
                }
                else
                {
                    DeviceInfo fallbackPlayback = ResolveBusinessPlaybackPrimary();
                    if (fallbackPlayback != null)
                    {
                        playbackPrimary = fallbackPlayback;
                    }
                }

                DeviceInfo resolvedCapturePrimary = ResolveCurrentDeviceByIdOrName(capturePrimary);
                if (resolvedCapturePrimary != null)
                {
                    capturePrimary = resolvedCapturePrimary;
                }
                else
                {
                    DeviceInfo fallbackCapture = ResolveBusinessCapturePrimary();
                    if (fallbackCapture != null)
                    {
                        capturePrimary = fallbackCapture;
                    }
                }

                if (playbackPrimary == null || string.IsNullOrEmpty(playbackPrimary.Id))
                {
                    throw new InvalidOperationException("既定設定対象の再生デバイスを特定できませんでした。サウンド設定を確認してください。");
                }

                if (capturePrimary == null || string.IsNullOrEmpty(capturePrimary.Id))
                {
                    throw new InvalidOperationException("既定設定対象の録音デバイスを特定できませんでした。サウンド設定を確認してください。");
                }

                AppendLog("[4/5] 再生: " + playbackPrimary.Name + " を既定/通信デバイスへ設定します。");
                SetDefaultWithRefreshOrThrow(playbackPrimary, new ERole[] { ERole.eConsole, ERole.eMultimedia, ERole.eCommunications }, ResolveBusinessPlaybackPrimary, "再生デバイス");
                AppendLog("      完了");

                AppendLog("[5/5] 録音: " + capturePrimary.Name + " を既定/通信デバイスへ設定します。");
                SetDefaultWithRefreshOrThrow(capturePrimary, new ERole[] { ERole.eConsole, ERole.eMultimedia, ERole.eCommunications }, ResolveBusinessCapturePrimary, "録音デバイス");
                AppendLog("      完了");

                AppendLog(string.Empty);
                AppendLog("通常業務へ戻す が完了しました。");
                UpdateDefaultDeviceLabels();
                if (alreadyApplied)
                {
                    MessageBox.Show(this, "現在の構成は既に通常業務モードです。\r\n整合性確認として再適用しました。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "通常業務へ戻す が完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                AppendLog(string.Empty);
                AppendException("[ERROR] 通常業務へ戻す に失敗しました", ex);

                bool isMissingDeviceError =
                    ex is InvalidOperationException &&
                    ex.Message != null &&
                    ex.Message.IndexOf("[未検出デバイス]", StringComparison.OrdinalIgnoreCase) >= 0;

                if (isMissingDeviceError)
                {
                    AppendLog("手動確認のためサウンド設定を開きます。");
                    DialogResult dialogResult = MessageBox.Show(
                        this,
                        "通常業務へ戻す に失敗しました。\r\n\r\n" +
                        "[原因]\r\n" + ex.Message + "\r\n\r\n" +
                        "[対応]\r\n" +
                        "1. OK を押すと Windows サウンド設定を開きます。\r\n" +
                        "2. 上記デバイスの接続/有効化を確認してください。",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.OK)
                    {
                        TryOpenSoundSettings();
                    }
                    return;
                }

                bool isUnpluggedDeviceError =
                    ex is InvalidOperationException &&
                    ex.Message != null &&
                    ex.Message.IndexOf("[未接続デバイス]", StringComparison.OrdinalIgnoreCase) >= 0;

                if (isUnpluggedDeviceError)
                {
                    AppendLog("手動確認のためサウンド設定を開きます。");
                    DialogResult dialogResult = MessageBox.Show(
                        this,
                        "通常業務へ戻す に失敗しました。\r\n\r\n" +
                        "[原因]\r\n" + ex.Message + "\r\n\r\n" +
                        "[対応]\r\n" +
                        "1. 対象デバイスの接続状態を確認してください。\r\n" +
                        "2. OK を押すと Windows サウンド設定を開きます。",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.OK)
                    {
                        TryOpenSoundSettings();
                    }
                    return;
                }

                MessageBox.Show(
                    this,
                    "通常業務へ戻す に失敗しました。\r\n\r\n" + ex.Message,
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void UpdateDefaultDeviceLabels()
        {
            bool initialized = TryInitializeRenderDefaultsIfMissing();
            if (initialized)
            {
                System.Threading.Thread.Sleep(250);
            }

            DeviceInfo playbackDefault = AudioHelper.GetDefault(EDataFlow.eRender, ERole.eConsole);
            playbackDefaultLabel.Text = "再生 既定: " + FormatDeviceLabel(playbackDefault);
            AppendDefaultEndpointMessage();

            DeviceInfo playbackComm = AudioHelper.GetDefault(EDataFlow.eRender, ERole.eCommunications);
            playbackCommLabel.Text = "再生 通信: " + FormatDeviceLabel(playbackComm);
            AppendDefaultEndpointMessage();

            DeviceInfo recordingDefault = AudioHelper.GetDefault(EDataFlow.eCapture, ERole.eConsole);
            recordingDefaultLabel.Text = "録音 既定: " + FormatDeviceLabel(recordingDefault);
            AppendDefaultEndpointMessage();

            DeviceInfo recordingComm = AudioHelper.GetDefault(EDataFlow.eCapture, ERole.eCommunications);
            recordingCommLabel.Text = "録音 通信: " + FormatDeviceLabel(recordingComm);
            AppendDefaultEndpointMessage();

            if (playbackDefault == null && playbackComm == null && recordingDefault == null && recordingComm == null)
            {
                AppendLog("[INFO] 再生/録音の既定デバイスが未設定の可能性があります。Windows のサウンド設定で既定と既定の通信デバイスを確認してください。");
            }

            DeviceInfo captureA1 = FindFirstMatch(EDataFlow.eCapture, new string[] { "Voicemeeter Out A1" });
            voicemeeterA1Label.Text = "Voicemeeter Out A1: " + FormatReadyOrDisabledLabel(captureA1);
        }

        private bool TryInitializeRenderDefaultsIfMissing()
        {
            try
            {
                DeviceInfo renderConsole = AudioHelper.GetDefault(EDataFlow.eRender, ERole.eConsole);
                DeviceInfo renderComm = AudioHelper.GetDefault(EDataFlow.eRender, ERole.eCommunications);
                if (renderConsole != null && renderComm != null)
                {
                    return false;
                }

                DeviceInfo candidate = ResolveBusinessPlaybackPrimary();
                if (candidate == null || string.IsNullOrEmpty(candidate.Id) || !IsDeviceActive(candidate.State))
                {
                    candidate = FindFirstActiveDevice(EDataFlow.eRender);
                }

                if (candidate == null || string.IsNullOrEmpty(candidate.Id))
                {
                    return false;
                }

                return TrySetRenderDefaultRoles(candidate, new ERole[] { ERole.eConsole, ERole.eMultimedia, ERole.eCommunications });
            }
            catch
            {
                return false;
            }
        }

        private bool TrySetRenderDefaultRoles(DeviceInfo target, ERole[] roles)
        {
            if (target == null || string.IsNullOrEmpty(target.Id))
            {
                return false;
            }

            DeviceInfo current = ResolveCurrentDeviceByIdOrName(target) ?? target;
            int attempt;
            for (attempt = 0; attempt < 3; attempt++)
            {
                bool allOk = true;

                int i;
                for (i = 0; i < roles.Length; i++)
                {
                    if (!AudioHelper.SetDefault(current.Id, roles[i]))
                    {
                        allOk = false;
                        break;
                    }
                }

                if (allOk)
                {
                    return true;
                }

                DeviceInfo refreshed = ResolveCurrentDeviceByIdOrName(current);
                if (refreshed == null)
                {
                    refreshed = FindFirstActiveDevice(EDataFlow.eRender);
                }

                if (refreshed == null || string.IsNullOrEmpty(refreshed.Id))
                {
                    return false;
                }

                current = refreshed;
                System.Threading.Thread.Sleep(200);
            }

            return false;
        }

        private DeviceInfo FindFirstActiveDevice(EDataFlow flow)
        {
            List<DeviceInfo> devices = AudioHelper.EnumerateCom(flow, 15);
            if (devices == null || devices.Count == 0)
            {
                devices = AudioHelper.Enumerate(flow);
            }

            int i;
            for (i = 0; i < devices.Count; i++)
            {
                DeviceInfo device = devices[i];
                if (device == null || string.IsNullOrEmpty(device.Id))
                {
                    continue;
                }

                if (IsDeviceActive(device.State))
                {
                    return device;
                }
            }

            return null;
        }

        private void AppendDefaultEndpointMessage()
        {
            if (string.IsNullOrEmpty(AudioHelper.LastDefaultMessage))
            {
                return;
            }

            string message = AudioHelper.LastDefaultMessage;
            if (message.IndexOf("0x80070490", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                // 既定未設定時に一覧更新のたびに同じ情報を出し続けない
                return;
            }

            AppendLog("[WARN] " + message);
        }

        private static string FormatDeviceLabel(DeviceInfo device)
        {
            if (device == null)
            {
                return "未検出";
            }

            return device.Name + " [ステータス: " + GetDefaultStatusLabel(device.State) + "]";
        }

        private static string FormatReadyOrDisabledLabel(DeviceInfo device)
        {
            if (device == null)
            {
                return "未検出";
            }

            return device.Name + " [ステータス: " + GetReadyOrDisabledStatusLabel(device.State) + "]";
        }

        private static string GetDefaultStatusLabel(int state)
        {
            if ((state & 2) == 2) return "無効";
            if ((state & 1) == 1) return "準備完了";
            return GetStateLabel(state);
        }

        private static string GetReadyOrDisabledStatusLabel(int state)
        {
            if ((state & 2) == 2) return "無効";
            return "準備完了";
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

        private void SetDefaultWithRefreshOrThrow(DeviceInfo targetDevice, ERole[] roles, Func<DeviceInfo> refreshResolver, string category)
        {
            if (targetDevice == null || string.IsNullOrEmpty(targetDevice.Id))
            {
                throw new InvalidOperationException(category + " の既定設定対象が未検出です。");
            }

            DeviceInfo current = targetDevice;
            int attempt;
            for (attempt = 0; attempt < 3; attempt++)
            {
                try
                {
                    SetDefaultOrThrow(current.Id, roles);
                    return;
                }
                catch (COMException comEx)
                {
                    if (IsElementNotFound(comEx))
                    {
                        AppendLog("      [WARN] " + category + " の既定設定で要素未検出(0x80070490)が発生しました。デバイス候補を切り替えて再試行します。");
                    }

                    DeviceInfo refreshed = ResolveRefreshedTarget(current, refreshResolver);
                    if (refreshed != null && !string.Equals(refreshed.Id, current.Id, StringComparison.OrdinalIgnoreCase))
                    {
                        current = refreshed;
                        System.Threading.Thread.Sleep(350);
                        continue;
                    }

                    if (attempt < 2)
                    {
                        DeviceInfo flowFallback = FindFirstActiveDevice(current.Flow) ?? FindFirstConnectedDevice(current.Flow);
                        if (flowFallback != null && !string.IsNullOrEmpty(flowFallback.Id) && !string.Equals(flowFallback.Id, current.Id, StringComparison.OrdinalIgnoreCase))
                        {
                            AppendLog("      [WARN] " + category + " の既定設定対象をフォールバックデバイスへ切り替えます: " + flowFallback.Name);
                            current = flowFallback;
                            System.Threading.Thread.Sleep(300);
                            continue;
                        }
                    }

                    if (TrySetRequiredRolesWithoutCommunications(current, roles, refreshResolver, category))
                    {
                        return;
                    }

                    throw new InvalidOperationException(category + " の既定デバイス設定に失敗しました。詳細: " + comEx.Message, comEx);
                }
                catch (InvalidOperationException)
                {
                    DeviceInfo refreshed = ResolveRefreshedTarget(current, refreshResolver);
                    if (refreshed != null && !string.Equals(refreshed.Id, current.Id, StringComparison.OrdinalIgnoreCase))
                    {
                        AppendLog("      [WARN] " + category + " のデバイスIDを再解決して再試行します。");
                        current = refreshed;
                        System.Threading.Thread.Sleep(300);
                        continue;
                    }

                    if (attempt < 2)
                    {
                        DeviceInfo flowFallback = FindFirstActiveDevice(current.Flow) ?? FindFirstConnectedDevice(current.Flow);
                        if (flowFallback != null && !string.IsNullOrEmpty(flowFallback.Id) && !string.Equals(flowFallback.Id, current.Id, StringComparison.OrdinalIgnoreCase))
                        {
                            AppendLog("      [WARN] " + category + " の既定設定対象をフォールバックデバイスへ切り替えます: " + flowFallback.Name);
                            current = flowFallback;
                            System.Threading.Thread.Sleep(300);
                            continue;
                        }
                    }

                    if (TrySetRequiredRolesWithoutCommunications(current, roles, refreshResolver, category))
                    {
                        return;
                    }

                    throw;
                }
            }

            if (TrySetRequiredRolesWithoutCommunications(current, roles, refreshResolver, category))
            {
                return;
            }

            throw new InvalidOperationException(category + " の既定デバイス設定に失敗しました。再試行しても設定できませんでした。");
        }

        private bool TrySetRequiredRolesWithoutCommunications(DeviceInfo current, ERole[] roles, Func<DeviceInfo> refreshResolver, string category)
        {
            if (!ContainsRole(roles, ERole.eCommunications))
            {
                return false;
            }

            ERole[] requiredRoles = ExcludeRole(roles, ERole.eCommunications);
            if (requiredRoles.Length == 0)
            {
                return false;
            }

            List<DeviceInfo> candidates = new List<DeviceInfo>();
            AddCandidateIfValid(candidates, current);
            AddCandidateIfValid(candidates, ResolveRefreshedTarget(current, refreshResolver));
            AddCandidateIfValid(candidates, FindFirstActiveDevice(current.Flow));
            AddCandidateIfValid(candidates, FindFirstConnectedDevice(current.Flow));

            int i;
            for (i = 0; i < candidates.Count; i++)
            {
                DeviceInfo candidate = candidates[i];
                try
                {
                    SetDefaultOrThrow(candidate.Id, requiredRoles);
                    AppendLog("      [WARN] " + category + " の通信既定設定はスキップしました。既定デバイス設定のみ適用します。");
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        private static bool ContainsRole(ERole[] roles, ERole targetRole)
        {
            int i;
            for (i = 0; i < roles.Length; i++)
            {
                if (roles[i] == targetRole)
                {
                    return true;
                }
            }

            return false;
        }

        private static ERole[] ExcludeRole(ERole[] roles, ERole excludedRole)
        {
            List<ERole> filtered = new List<ERole>();
            int i;
            for (i = 0; i < roles.Length; i++)
            {
                if (roles[i] != excludedRole)
                {
                    filtered.Add(roles[i]);
                }
            }

            return filtered.ToArray();
        }

        private static void AddCandidateIfValid(List<DeviceInfo> list, DeviceInfo candidate)
        {
            if (candidate == null || string.IsNullOrEmpty(candidate.Id))
            {
                return;
            }

            int i;
            for (i = 0; i < list.Count; i++)
            {
                if (string.Equals(list[i].Id, candidate.Id, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            list.Add(candidate);
        }

        private DeviceInfo ResolveRefreshedTarget(DeviceInfo current, Func<DeviceInfo> refreshResolver)
        {
            DeviceInfo refreshed = ResolveCurrentDeviceByIdOrName(current);
            if (refreshed != null)
            {
                return refreshed;
            }

            if (refreshResolver != null)
            {
                refreshed = refreshResolver();
            }

            if (refreshed == null || string.IsNullOrEmpty(refreshed.Id))
            {
                return null;
            }

            return refreshed;
        }

        private static bool IsElementNotFound(COMException ex)
        {
            return ex != null && ex.ErrorCode == unchecked((int)0x80070490);
        }

        private DeviceInfo ResolveCurrentDeviceByIdOrName(DeviceInfo expected)
        {
            if (expected == null)
            {
                return null;
            }

            List<DeviceInfo> devices = AudioHelper.Enumerate(expected.Flow);
            if (devices == null || devices.Count == 0)
            {
                return null;
            }

            DeviceInfo exactNameMatch = null;
            string normalizedExpectedName = NormalizeDeviceNameForMatch(expected.Name);
            DeviceInfo normalizedNameMatch = null;

            int i;
            for (i = 0; i < devices.Count; i++)
            {
                DeviceInfo device = devices[i];
                if (device == null || string.IsNullOrEmpty(device.Id))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(expected.Id) && string.Equals(device.Id, expected.Id, StringComparison.OrdinalIgnoreCase))
                {
                    return device;
                }

                if (!string.IsNullOrEmpty(expected.Name) && !string.IsNullOrEmpty(device.Name) && string.Equals(device.Name, expected.Name, StringComparison.OrdinalIgnoreCase))
                {
                    if (exactNameMatch == null || IsBetterActiveCandidate(device, exactNameMatch))
                    {
                        exactNameMatch = device;
                    }
                }

                if (!string.IsNullOrEmpty(normalizedExpectedName) && !string.IsNullOrEmpty(device.Name))
                {
                    string normalizedCurrentName = NormalizeDeviceNameForMatch(device.Name);
                    if (string.Equals(normalizedCurrentName, normalizedExpectedName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (normalizedNameMatch == null || IsBetterActiveCandidate(device, normalizedNameMatch))
                        {
                            normalizedNameMatch = device;
                        }
                    }
                }
            }

            if (exactNameMatch != null)
            {
                return exactNameMatch;
            }

            return normalizedNameMatch;
        }

        private static bool IsBetterActiveCandidate(DeviceInfo candidate, DeviceInfo currentBest)
        {
            if (candidate == null)
            {
                return false;
            }

            if (currentBest == null)
            {
                return true;
            }

            bool candidateActive = (candidate.State & 1) == 1 && (candidate.State & 2) != 2;
            bool currentBestActive = (currentBest.State & 1) == 1 && (currentBest.State & 2) != 2;
            if (candidateActive && !currentBestActive)
            {
                return true;
            }

            if (!candidateActive && currentBestActive)
            {
                return false;
            }

            bool candidateDisabled = (candidate.State & 2) == 2;
            bool currentBestDisabled = (currentBest.State & 2) == 2;
            if (!candidateDisabled && currentBestDisabled)
            {
                return true;
            }

            return false;
        }

        private DeviceInfo ResolveBusinessPlaybackPrimary()
        {
            DeviceInfo playbackHeadphones = FindBestRealtekHeadphones();
            DeviceInfo playbackSpeakers = FindBestRealtekSpeakers();
            DeviceInfo playbackIntelSpeakers = FindBestIntelSmartSoundSpeakers();
            return ResolveBusinessPlaybackPrimary(playbackHeadphones, playbackSpeakers, playbackIntelSpeakers);
        }

        private DeviceInfo ResolveBusinessCapturePrimary()
        {
            DeviceInfo captureMic = FindBestRealtekExternalMic();
            DeviceInfo captureArrayMic = FindBestIntelSmartSoundArrayMic();
            return ResolveBusinessCapturePrimary(captureMic, captureArrayMic);
        }

        private DeviceInfo ResolveBusinessPlaybackPrimary(DeviceInfo playbackHeadphones, DeviceInfo playbackSpeakers, DeviceInfo playbackIntelSpeakers)
        {
            DeviceInfo[] candidates = new DeviceInfo[] { playbackHeadphones, playbackSpeakers, playbackIntelSpeakers };
            return SelectBestBusinessPrimary(candidates);
        }

        private DeviceInfo ResolveBusinessCapturePrimary(DeviceInfo captureMic, DeviceInfo captureArrayMic)
        {
            DeviceInfo[] candidates = new DeviceInfo[] { captureMic, captureArrayMic };
            return SelectBestBusinessPrimary(candidates);
        }

        private DeviceInfo SelectBestBusinessPrimary(DeviceInfo[] candidates)
        {
            DeviceInfo firstConnectedCandidate = null;
            int i;
            for (i = 0; i < candidates.Length; i++)
            {
                DeviceInfo candidate = candidates[i];
                if (candidate == null)
                {
                    continue;
                }

                DeviceInfo current = candidate;
                if (IsDeviceActive(current.State))
                {
                    return current;
                }

                if (!IsDeviceUnplugged(current.State) && firstConnectedCandidate == null)
                {
                    firstConnectedCandidate = current;
                }
            }

            if (firstConnectedCandidate != null)
            {
                return firstConnectedCandidate;
            }

            for (i = 0; i < candidates.Length; i++)
            {
                if (candidates[i] != null)
                {
                    return candidates[i];
                }
            }

            return null;
        }

        private DeviceInfo ResolvePlaybackPrimaryForBusiness(DeviceInfo currentPrimary, DeviceInfo playbackHeadphones, DeviceInfo playbackSpeakers, DeviceInfo playbackIntelSpeakers)
        {
            DeviceInfo current = ResolveCurrentDeviceByIdOrName(currentPrimary) ?? currentPrimary;
            if (current != null && !IsDeviceUnplugged(current.State))
            {
                return current;
            }

            DeviceInfo[] preferred = new DeviceInfo[] { playbackSpeakers, playbackIntelSpeakers, playbackHeadphones };
            int i;
            for (i = 0; i < preferred.Length; i++)
            {
                DeviceInfo candidate = preferred[i];
                if (candidate == null)
                {
                    continue;
                }

                DeviceInfo resolved = ResolveCurrentDeviceByIdOrName(candidate) ?? candidate;
                if (resolved != null && !IsDeviceUnplugged(resolved.State))
                {
                    if (current != null && !string.Equals(current.Id, resolved.Id, StringComparison.OrdinalIgnoreCase))
                    {
                        AppendLog("      [INFO] 再生デバイスを未接続候補から切り替えます: " + resolved.Name);
                    }
                    return resolved;
                }
            }

            DeviceInfo connectedFallback = FindFirstConnectedDevice(EDataFlow.eRender);
            if (connectedFallback != null)
            {
                AppendLog("      [INFO] 再生デバイスを接続済みフォールバックへ切り替えます: " + connectedFallback.Name);
                return connectedFallback;
            }

            return current;
        }

        private DeviceInfo FindFirstConnectedDevice(EDataFlow flow)
        {
            List<DeviceInfo> devices = AudioHelper.EnumerateCom(flow, 15);
            if (devices == null || devices.Count == 0)
            {
                devices = AudioHelper.Enumerate(flow);
            }

            int i;
            for (i = 0; i < devices.Count; i++)
            {
                DeviceInfo device = devices[i];
                if (device == null || string.IsNullOrEmpty(device.Id))
                {
                    continue;
                }

                if ((device.State & 2) == 2)
                {
                    continue;
                }

                if (!IsDeviceUnplugged(device.State))
                {
                    return device;
                }
            }

            return null;
        }

        private bool IsAutomaticSetupAlreadyApplied(DeviceInfo playbackHeadphones, DeviceInfo playbackVmInput, DeviceInfo captureB1, DeviceInfo captureMic)
        {
            bool playbackDefaultOk = IsDefaultForRoles(playbackHeadphones.Id, EDataFlow.eRender, new ERole[] { ERole.eConsole, ERole.eMultimedia });
            bool playbackCommOk = IsDefaultForRoles(playbackVmInput.Id, EDataFlow.eRender, new ERole[] { ERole.eCommunications });
            bool recordingDefaultOk = IsDefaultForRoles(captureB1.Id, EDataFlow.eCapture, new ERole[] { ERole.eConsole, ERole.eMultimedia });
            bool recordingCommOk = IsDefaultForRoles(captureMic.Id, EDataFlow.eCapture, new ERole[] { ERole.eCommunications });
            // Note: Voicemeeter running status is checked separately in the caller, not here
            return playbackDefaultOk && playbackCommOk && recordingDefaultOk && recordingCommOk;
        }

        private bool IsBusinessSetupAlreadyApplied(DeviceInfo playbackHeadphones, DeviceInfo captureMic, DeviceInfo vmInput, DeviceInfo captureA1, DeviceInfo captureB1)
        {
            bool playbackAllOk = IsDefaultForRoles(playbackHeadphones.Id, EDataFlow.eRender, new ERole[] { ERole.eConsole, ERole.eMultimedia, ERole.eCommunications });
            bool recordingAllOk = IsDefaultForRoles(captureMic.Id, EDataFlow.eCapture, new ERole[] { ERole.eConsole, ERole.eMultimedia, ERole.eCommunications });
            bool vmStopped = !VoicemeeterHelper.IsVoicemeeterX64Running();
            bool vmDevicesDisabled = IsDisabledOrMissing(vmInput) && IsDisabledOrMissing(captureA1) && IsDisabledOrMissing(captureB1);
            return playbackAllOk && recordingAllOk && vmStopped && vmDevicesDisabled;
        }

        private static bool IsDefaultForRoles(string expectedDeviceId, EDataFlow flow, ERole[] roles)
        {
            if (string.IsNullOrEmpty(expectedDeviceId))
            {
                return false;
            }

            int i;
            for (i = 0; i < roles.Length; i++)
            {
                DeviceInfo current = AudioHelper.GetDefault(flow, roles[i]);
                if (current == null || !string.Equals(current.Id, expectedDeviceId, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsDisabledOrMissing(DeviceInfo device)
        {
            if (device == null)
            {
                return true;
            }

            return (device.State & 2) == 2;
        }

        private void DisableNonTargetDevices(DeviceInfo playbackHeadphones, DeviceInfo playbackVmInput, DeviceInfo captureB1, DeviceInfo captureMic, DeviceInfo captureA1)
        {
            AppendLog("[後処理] 対象外デバイスを無効化します。");

            List<DeviceInfo> allRender = AudioHelper.Enumerate(EDataFlow.eRender);
            int i;
            for (i = 0; i < allRender.Count; i++)
            {
                DeviceInfo dev = allRender[i];
                if (dev == null || string.IsNullOrEmpty(dev.Id))
                {
                    continue;
                }

                bool isTarget =
                    (playbackHeadphones != null && string.Equals(dev.Id, playbackHeadphones.Id, StringComparison.OrdinalIgnoreCase)) ||
                    (playbackVmInput != null && string.Equals(dev.Id, playbackVmInput.Id, StringComparison.OrdinalIgnoreCase));

                if (!isTarget && (dev.State & 2) != 2)
                {
                    bool ok = AudioHelper.SetVisible(dev.Id, false);
                    AppendLog("      再生 無効化: " + dev.Name + (ok ? " 完了" : " 失敗"));
                }
            }

            List<DeviceInfo> allCapture = AudioHelper.Enumerate(EDataFlow.eCapture);
            int j;
            for (j = 0; j < allCapture.Count; j++)
            {
                DeviceInfo dev = allCapture[j];
                if (dev == null || string.IsNullOrEmpty(dev.Id))
                {
                    continue;
                }

                bool isTarget =
                    (captureB1 != null && string.Equals(dev.Id, captureB1.Id, StringComparison.OrdinalIgnoreCase)) ||
                    (captureMic != null && string.Equals(dev.Id, captureMic.Id, StringComparison.OrdinalIgnoreCase)) ||
                    (captureA1 != null && string.Equals(dev.Id, captureA1.Id, StringComparison.OrdinalIgnoreCase));

                if (!isTarget && (dev.State & 2) != 2)
                {
                    bool ok = AudioHelper.SetVisible(dev.Id, false);
                    AppendLog("      録音 無効化: " + dev.Name + (ok ? " 完了" : " 失敗"));
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
                if (device.Name.IndexOf("ヘッドホン", StringComparison.OrdinalIgnoreCase) >= 0) score += 10;
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
                if (device.Name.IndexOf("マイク", StringComparison.OrdinalIgnoreCase) >= 0) score += 6;
                if (device.Name.IndexOf("Mic", StringComparison.OrdinalIgnoreCase) >= 0) score += 6;
                if (device.Name.IndexOf("Microphone", StringComparison.OrdinalIgnoreCase) >= 0) score += 6;
                // 外付けマイクを優先し、配列マイク(マイク配列)は下げる
                if (device.Name.IndexOf("外付け", StringComparison.OrdinalIgnoreCase) >= 0) score += 10;
                if (device.Name.IndexOf("配列", StringComparison.OrdinalIgnoreCase) >= 0) score -= 6;
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

        private DeviceInfo FindBestRealtekSpeakers()
        {
            List<DeviceInfo> devices = AudioHelper.Enumerate(EDataFlow.eRender);
            DeviceInfo best = null;
            int bestScore = int.MinValue;
            int i;

            for (i = 0; i < devices.Count; i++)
            {
                DeviceInfo device = devices[i];
                if (device == null || string.IsNullOrEmpty(device.Name))
                {
                    continue;
                }

                if (device.Name.IndexOf("Realtek", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                int score = 0;
                if (device.Name.IndexOf("Realtek", StringComparison.OrdinalIgnoreCase) >= 0) score += 4;
                if (device.Name.IndexOf("スピーカー", StringComparison.OrdinalIgnoreCase) >= 0) score += 12;
                if (device.Name.IndexOf("Speaker", StringComparison.OrdinalIgnoreCase) >= 0) score += 12;
                if (device.Name.IndexOf("ヘッドホン", StringComparison.OrdinalIgnoreCase) >= 0) score -= 8;
                if (device.Name.IndexOf("Headphone", StringComparison.OrdinalIgnoreCase) >= 0) score -= 8;
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
            List<DeviceInfo> devices = AudioHelper.Enumerate(EDataFlow.eRender);
            DeviceInfo best = null;
            int bestScore = int.MinValue;
            int i;

            for (i = 0; i < devices.Count; i++)
            {
                DeviceInfo device = devices[i];
                if (device == null || string.IsNullOrEmpty(device.Name))
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
                if (normalizedName.IndexOf("earphone", StringComparison.OrdinalIgnoreCase) >= 0) score += 8;
                if (normalizedName.IndexOf("speaker", StringComparison.OrdinalIgnoreCase) >= 0) score += 3;
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
                if (device == null || string.IsNullOrEmpty(device.Name))
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
                if ((device.State & 1) == 1) score += 1;

                if (score > bestScore)
                {
                    best = device;
                    bestScore = score;
                }
            }

            return best;
        }

        private DeviceInfo FindBestIntelSmartSoundArrayMic()
        {
            List<DeviceInfo> devices = AudioHelper.Enumerate(EDataFlow.eCapture);
            DeviceInfo best = null;
            int bestScore = int.MinValue;
            int i;

            for (i = 0; i < devices.Count; i++)
            {
                DeviceInfo device = devices[i];
                if (device == null || string.IsNullOrEmpty(device.Name))
                {
                    continue;
                }

                string name = device.Name;
                bool hasIntel = name.IndexOf("Intel", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                name.IndexOf("インテル", StringComparison.OrdinalIgnoreCase) >= 0;
                bool hasSmartSound = name.IndexOf("Smart Sound", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                     (name.IndexOf("スマート", StringComparison.OrdinalIgnoreCase) >= 0 && name.IndexOf("サウンド", StringComparison.OrdinalIgnoreCase) >= 0);
                bool hasArrayMic = name.IndexOf("マイク配列", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                   name.IndexOf("Array", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                   name.IndexOf("デジタルマイク", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                   name.IndexOf("Digital Mic", StringComparison.OrdinalIgnoreCase) >= 0;

                if (!hasIntel || !hasSmartSound || !hasArrayMic)
                {
                    continue;
                }

                int score = 0;
                if (hasIntel) score += 5;
                if (hasSmartSound) score += 6;
                if (hasArrayMic) score += 8;
                if ((device.State & 1) == 1) score += 1;

                if (score > bestScore)
                {
                    best = device;
                    bestScore = score;
                }
            }

            return best;
        }

        private DeviceInfo FindBestIntelSmartSoundSpeakers()
        {
            List<DeviceInfo> devices = AudioHelper.Enumerate(EDataFlow.eRender);
            DeviceInfo best = null;
            int bestScore = int.MinValue;
            int i;

            for (i = 0; i < devices.Count; i++)
            {
                DeviceInfo device = devices[i];
                if (device == null || string.IsNullOrEmpty(device.Name))
                {
                    continue;
                }

                string name = device.Name;
                bool hasIntel = name.IndexOf("Intel", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                name.IndexOf("インテル", StringComparison.OrdinalIgnoreCase) >= 0;
                bool hasSmartSound = name.IndexOf("Smart Sound", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                     name.IndexOf("SST", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                     (name.IndexOf("スマート", StringComparison.OrdinalIgnoreCase) >= 0 && name.IndexOf("サウンド", StringComparison.OrdinalIgnoreCase) >= 0);
                bool hasSpeaker = name.IndexOf("スピーカー", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                  name.IndexOf("Speaker", StringComparison.OrdinalIgnoreCase) >= 0;

                if (!hasIntel || !hasSpeaker)
                {
                    continue;
                }

                int score = 0;
                if (hasIntel) score += 6;
                if (hasSmartSound) score += 5;
                if (hasSpeaker) score += 8;
                if ((device.State & 1) == 1) score += 2;

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

        private void AppendImportantLog(string message)
        {
            AppendLog("==============================================");
            AppendLog("[重要] Voicemeeter 起動情報");
            AppendLog(message);
            AppendLog("==============================================");
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

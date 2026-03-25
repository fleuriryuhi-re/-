$ErrorActionPreference = 'Stop'

[Console]::OutputEncoding = [System.Text.UTF8Encoding]::new($false)
$OutputEncoding = [Console]::OutputEncoding

Add-Type -Language CSharp -TypeDefinition @"
using System;
using System.Runtime.InteropServices;

namespace AudioAutomation {
    public enum EDataFlow { eRender = 0, eCapture = 1, eAll = 2 }
    public enum ERole { eConsole = 0, eMultimedia = 1, eCommunications = 2 }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROPERTYKEY {
        public Guid fmtid;
        public int pid;
        public PROPERTYKEY(Guid f, int p) { fmtid = f; pid = p; }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct PROPVARIANT {
        [FieldOffset(0)] public ushort vt;
        [FieldOffset(8)] public IntPtr pointerValue;
    }

    [ComImport]
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMDeviceEnumerator {
        int NotImpl1();
        int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppEndpoint);
        int GetDevice([MarshalAs(UnmanagedType.LPWStr)] string pwstrId, out IMMDevice ppDevice);
        int EnumAudioEndpoints(EDataFlow dataFlow, int dwStateMask, out IMMDeviceCollection ppDevices);
    }

    [ComImport]
    [Guid("0BD7A1BE-7A1A-44DB-8397-C0A2B91F0DB2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMDeviceCollection {
        int GetCount(out int pcDevices);
        int Item(int nDevice, out IMMDevice ppDevice);
    }

    [ComImport]
    [Guid("D666063F-1587-4E43-81F1-B948E807363F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMMDevice {
        int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
        int OpenPropertyStore(int stgmAccess, out IPropertyStore ppProperties);
        int GetId([MarshalAs(UnmanagedType.LPWStr)] out string ppstrId);
        int GetState(out int pdwState);
    }

    [ComImport]
    [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyStore {
        int GetCount(out int cProps);
        int GetAt(int iProp, out PROPERTYKEY pkey);
        int GetValue(ref PROPERTYKEY key, out PROPVARIANT pv);
        int SetValue(ref PROPERTYKEY key, ref PROPVARIANT pv);
        int Commit();
    }

    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    public class MMDeviceEnumeratorComObject { }

    [ComImport]
    [Guid("870af99c-171d-4f9e-af0d-e63df40c2bc9")]
    public class PolicyConfigClient { }

    [ComImport]
    [Guid("f8679f50-850a-41cf-9c72-430f290290c8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPolicyConfig {
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

    public static class NativeMethods {
        [DllImport("Ole32.dll")]
        public static extern int PropVariantClear(ref PROPVARIANT pvar);
    }

    public class DeviceInfo {
        public string Id;
        public string Name;
        public int State;
        public EDataFlow Flow;
    }

    public static class AudioHelper {
        private static readonly PROPERTYKEY PKEY_Device_FriendlyName =
            new PROPERTYKEY(new Guid("a45c254e-df1c-4efd-8020-67d146a850e0"), 14);

        public static DeviceInfo[] Enumerate(EDataFlow flow) {
            var enumerator = (IMMDeviceEnumerator)(new MMDeviceEnumeratorComObject());
            IMMDeviceCollection collection;
            int hr = enumerator.EnumAudioEndpoints(flow, 0x0F, out collection); // all states
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);

            int count;
            hr = collection.GetCount(out count);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);

            var list = new DeviceInfo[count];
            for (int i = 0; i < count; i++) {
                IMMDevice device;
                hr = collection.Item(i, out device);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                string id;
                hr = device.GetId(out id);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                int state;
                hr = device.GetState(out state);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                IPropertyStore store;
                hr = device.OpenPropertyStore(0, out store);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                PROPVARIANT pv;
                hr = store.GetValue(ref PKEY_Device_FriendlyName, out pv);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                string name = null;
                try {
                    if (pv.vt == 31 && pv.pointerValue != IntPtr.Zero) {
                        name = Marshal.PtrToStringUni(pv.pointerValue);
                    }
                }
                finally {
                    NativeMethods.PropVariantClear(ref pv);
                }

                list[i] = new DeviceInfo { Id = id, Name = name ?? "", State = state, Flow = flow };
            }
            return list;
        }

        public static bool SetDefault(string deviceId, ERole role) {
            var policy = (IPolicyConfig)(new PolicyConfigClient());
            int hr = policy.SetDefaultEndpoint(deviceId, role);
            return hr == 0;
        }

        public static bool SetVisible(string deviceId, bool visible) {
            var policy = (IPolicyConfig)(new PolicyConfigClient());
            int hr = policy.SetEndpointVisibility(deviceId, visible ? 1 : 0);
            return hr == 0;
        }
    }
}
"@

function Find-DeviceByKeyword {
    param(
        [AudioAutomation.EDataFlow]$Flow,
        [string]$Keyword
    )

    $devices = [AudioAutomation.AudioHelper]::Enumerate($Flow)
    $hit = $devices | Where-Object { $_.Name -like "*$Keyword*" } | Select-Object -First 1
    return $hit
}

function Get-StateLabel {
    param([int]$State)
    if ($State -band 1) { return "有効" }
    if ($State -band 2) { return "無効" }
    if ($State -band 4) { return "未接続" }
    if ($State -band 8) { return "存在しない" }
    return "不明"
}

function Set-DefaultRoles {
    param(
        [string]$DeviceId,
        [AudioAutomation.ERole[]]$Roles
    )

    foreach ($role in $Roles) {
        $ok = [AudioAutomation.AudioHelper]::SetDefault($DeviceId, $role)
        if (-not $ok) {
            throw "既定デバイス設定に失敗しました。Role=$role"
        }
    }
}

Write-Host "============================================================" -ForegroundColor White
Write-Host " Windows サウンド設定 自動化（管理者不要）" -ForegroundColor White
Write-Host "============================================================" -ForegroundColor White
Write-Host ""

try {
    $playVmInput = Find-DeviceByKeyword -Flow ([AudioAutomation.EDataFlow]::eRender) -Keyword "Voicemeeter Input"
    if (-not $playVmInput) { throw "再生デバイス 'Voicemeeter Input' が見つかりません。" }

    $recA1 = Find-DeviceByKeyword -Flow ([AudioAutomation.EDataFlow]::eCapture) -Keyword "Voicemeeter Out A1"
    if (-not $recA1) { throw "録音デバイス 'Voicemeeter Out A1' が見つかりません。" }

    $recB1 = Find-DeviceByKeyword -Flow ([AudioAutomation.EDataFlow]::eCapture) -Keyword "Voicemeeter Out B1"
    if (-not $recB1) { throw "録音デバイス 'Voicemeeter Out B1' が見つかりません。" }

    $recRealtek = Find-DeviceByKeyword -Flow ([AudioAutomation.EDataFlow]::eCapture) -Keyword "Realtek"
    if (-not $recRealtek) { throw "録音デバイス 'Realtek' が見つかりません。" }

    Write-Host "[1/4] 再生: Voicemeeter Input を既定の通信デバイスに設定..." -ForegroundColor Cyan
    Set-DefaultRoles -DeviceId $playVmInput.Id -Roles @([AudioAutomation.ERole]::eCommunications)
    Write-Host "      完了" -ForegroundColor Green

    Write-Host "[2/4] 録音: Voicemeeter Out A1 を有効化（無効時）..." -ForegroundColor Cyan
    $a1Current = Find-DeviceByKeyword -Flow ([AudioAutomation.EDataFlow]::eCapture) -Keyword "Voicemeeter Out A1"
    if ($a1Current.State -band 2) {
        $visibleOk = [AudioAutomation.AudioHelper]::SetVisible($a1Current.Id, $true)
        Start-Sleep -Milliseconds 300
        $a1After = Find-DeviceByKeyword -Flow ([AudioAutomation.EDataFlow]::eCapture) -Keyword "Voicemeeter Out A1"

        if (($a1After.State -band 1) -or $visibleOk) {
            Write-Host "      有効化を実行しました（状態: $((Get-StateLabel $a1After.State)))" -ForegroundColor Green
        } else {
            Write-Warning "      自動有効化に失敗しました。手動で有効化が必要です。"
        }
    } else {
        Write-Host "      既に有効です（状態: $((Get-StateLabel $a1Current.State)))" -ForegroundColor Green
    }

    Write-Host "[3/4] 録音: Voicemeeter Out B1 を既定のデバイスに設定..." -ForegroundColor Cyan
    Set-DefaultRoles -DeviceId $recB1.Id -Roles @([AudioAutomation.ERole]::eConsole, [AudioAutomation.ERole]::eMultimedia)
    Write-Host "      完了" -ForegroundColor Green

    Write-Host "[4/4] 録音: Realtek を既定の通信デバイスに設定..." -ForegroundColor Cyan
    Set-DefaultRoles -DeviceId $recRealtek.Id -Roles @([AudioAutomation.ERole]::eCommunications)
    Write-Host "      完了" -ForegroundColor Green

    Write-Host ""
    Write-Host "すべての設定処理が完了しました。" -ForegroundColor Green
    exit 0
}
catch {
    Write-Host "" 
    Write-Host "[ERROR] 自動設定に失敗しました: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "以下の手動手順で設定してください（管理者不要）:" -ForegroundColor Yellow
    Write-Host "  1) 再生タブ: Voicemeeter Input -> 既定の通信デバイス" -ForegroundColor Yellow
    Write-Host "  2) 録音タブ: Voicemeeter Out A1 を有効化" -ForegroundColor Yellow
    Write-Host "  3) 録音タブ: Voicemeeter Out B1 -> 既定のデバイス" -ForegroundColor Yellow
    Write-Host "  4) 録音タブ: Realtek -> 既定の通信デバイス" -ForegroundColor Yellow
    Write-Host ""
    Start-Process -FilePath "control.exe" -ArgumentList "mmsys.cpl"
    exit 1
}

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsAudioSetup
{
    internal sealed class BuildDragDropForm : Form
    {
        private readonly Label instructionLabel;
        private readonly Button buildDefaultButton;
        private readonly TextBox logBox;

        public BuildDragDropForm(string[] startupArgs)
        {
            Text = "Build DragDrop";
            ClientSize = new Size(820, 500);
            MinimumSize = new Size(700, 420);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            AllowDrop = true;

            instructionLabel = new Label();
            instructionLabel.AutoSize = false;
            instructionLabel.Location = new Point(16, 16);
            instructionLabel.Size = new Size(780, 80);
            instructionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            instructionLabel.Text =
                "Drop a .cs file or folder onto this window to build.\r\n" +
                "- File: build only the dropped C# file\r\n" +
                "- Folder: run build.bat / build-dev.bat if present";

            buildDefaultButton = new Button();
            buildDefaultButton.Text = "Build Default (build.bat + build-dev.bat)";
            buildDefaultButton.Location = new Point(16, 105);
            buildDefaultButton.Size = new Size(350, 34);
            buildDefaultButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            buildDefaultButton.Click += async delegate { await RunBuildAsync(new string[0]); };

            logBox = new TextBox();
            logBox.Location = new Point(16, 150);
            logBox.Size = new Size(780, 330);
            logBox.Multiline = true;
            logBox.ScrollBars = ScrollBars.Vertical;
            logBox.ReadOnly = true;
            logBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            Controls.Add(instructionLabel);
            Controls.Add(buildDefaultButton);
            Controls.Add(logBox);

            DragEnter += OnFormDragEnter;
            DragDrop += OnFormDragDrop;
            Shown += async delegate
            {
                if (startupArgs != null && startupArgs.Length > 0)
                {
                    await RunBuildAsync(startupArgs);
                }
            };
        }

        private void OnFormDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                return;
            }

            e.Effect = DragDropEffects.None;
        }

        private async void OnFormDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }

            object raw = e.Data.GetData(DataFormats.FileDrop);
            string[] dropped = raw as string[];
            if (dropped == null || dropped.Length == 0)
            {
                return;
            }

            await RunBuildAsync(dropped);
        }

        private async Task RunBuildAsync(string[] inputs)
        {
            buildDefaultButton.Enabled = false;
            Cursor previous = Cursor;
            Cursor = Cursors.WaitCursor;

            try
            {
                AppendLog(string.Empty);
                AppendLog("==== Build Start " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ====");

                int code = await Task.Run(delegate
                {
                    if (inputs == null || inputs.Length == 0)
                    {
                        return BuildEngine.BuildDefault(AppDomain.CurrentDomain.BaseDirectory, AppendLog);
                    }

                    int i;
                    for (i = 0; i < inputs.Length; i++)
                    {
                        int itemCode = BuildEngine.BuildInput(inputs[i], AppendLog);
                        if (itemCode != 0)
                        {
                            return itemCode;
                        }
                    }

                    return 0;
                });

                AppendLog(code == 0 ? "[DONE] Build completed." : "[ERROR] Build failed. ExitCode=" + code);
            }
            finally
            {
                Cursor = previous;
                buildDefaultButton.Enabled = true;
            }
        }

        private void AppendLog(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AppendLog), message);
                return;
            }

            logBox.AppendText(message + Environment.NewLine);
        }
    }

    internal static class BuildEngine
    {
        private const string CscPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe";

        public static int BuildInput(string rawInputPath, Action<string> log)
        {
            if (!File.Exists(CscPath))
            {
                log("[ERROR] csc.exe was not found: " + CscPath);
                return 1;
            }

            string inputPath = Path.GetFullPath(rawInputPath);
            if (Directory.Exists(inputPath))
            {
                return BuildFromFolder(inputPath, log);
            }

            if (File.Exists(inputPath) && string.Equals(Path.GetExtension(inputPath), ".cs", StringComparison.OrdinalIgnoreCase))
            {
                return BuildFromSource(inputPath, log);
            }

            log("[ERROR] Unsupported input: " + inputPath);
            log("        Drop a .cs file or a folder.");
            return 1;
        }

        public static int BuildDefault(string baseDir, Action<string> log)
        {
            if (!File.Exists(CscPath))
            {
                log("[ERROR] csc.exe was not found: " + CscPath);
                return 1;
            }

            string prod = Path.Combine(baseDir, "build.bat");
            string dev = Path.Combine(baseDir, "build-dev.bat");

            if (File.Exists(prod))
            {
                int code = RunBatch(prod, log);
                if (code != 0)
                {
                    return code;
                }
            }

            if (File.Exists(dev))
            {
                int code = RunBatch(dev, log);
                if (code != 0)
                {
                    return code;
                }
            }

            if (!File.Exists(prod) && !File.Exists(dev))
            {
                log("[ERROR] build.bat and build-dev.bat were not found next to this exe.");
                return 1;
            }

            log("[DONE] Production and Dev builds completed.");
            return 0;
        }

        private static int BuildFromFolder(string folder, Action<string> log)
        {
            bool builtAny = false;

            string prod = Path.Combine(folder, "build.bat");
            string dev = Path.Combine(folder, "build-dev.bat");

            if (File.Exists(prod))
            {
                int code = RunBatch(prod, log);
                if (code != 0)
                {
                    return code;
                }

                builtAny = true;
            }

            if (File.Exists(dev))
            {
                int code = RunBatch(dev, log);
                if (code != 0)
                {
                    return code;
                }

                builtAny = true;
            }

            if (builtAny)
            {
                log("[DONE] Folder build finished.");
                return 0;
            }

            string prodSource = Path.Combine(folder, "AudioSetupUI.cs");
            string devSource = Path.Combine(folder, "AudioSetupUI_dev.cs");

            if (File.Exists(prodSource))
            {
                int code = BuildFromSource(prodSource, log);
                if (code != 0)
                {
                    return code;
                }

                builtAny = true;
            }

            if (File.Exists(devSource))
            {
                int code = BuildFromSource(devSource, log);
                if (code != 0)
                {
                    return code;
                }

                builtAny = true;
            }

            if (builtAny)
            {
                log("[DONE] Folder build finished.");
                return 0;
            }

            log("[ERROR] No build target found in folder: " + folder);
            log("        Expected build.bat, build-dev.bat, AudioSetupUI.cs, or AudioSetupUI_dev.cs");
            return 1;
        }

        private static int BuildFromSource(string sourceFile, Action<string> log)
        {
            string sourceDir = Path.GetDirectoryName(sourceFile) ?? Environment.CurrentDirectory;
            string sourceName = Path.GetFileName(sourceFile);
            string outputName = ResolveOutputNameByFileName(sourceName);
            string outputFile = Path.Combine(sourceDir, outputName);
            string icon = Path.Combine(sourceDir, "assets", "headset.ico");

            StringBuilder args = new StringBuilder();
            args.Append("/nologo ");
            args.Append("/target:winexe ");
            args.Append("/platform:anycpu ");
            args.Append("/optimize+ ");
            if (File.Exists(icon))
            {
                args.Append("/win32icon:\"").Append(icon).Append("\" ");
            }

            args.Append("/out:\"").Append(outputFile).Append("\" ");
            args.Append("/reference:System.dll ");
            args.Append("/reference:System.Windows.Forms.dll ");
            args.Append("/reference:System.Drawing.dll ");
            args.Append("\"").Append(sourceFile).Append("\"");

            log("[INFO] Building source: " + sourceFile);
            int exitCode = RunProcess(CscPath, args.ToString(), sourceDir, log);
            if (exitCode != 0)
            {
                log("[ERROR] Build failed: " + sourceFile);
                return exitCode;
            }

            log("[DONE] Output: " + outputFile);
            return 0;
        }

        private static string ResolveOutputNameByFileName(string sourceName)
        {
            string baseName = Path.GetFileNameWithoutExtension(sourceName) ?? string.Empty;
            string normalized = baseName
                .Replace("_", string.Empty)
                .Replace("-", string.Empty)
                .Replace(".", string.Empty)
                .ToLowerInvariant();

            if (normalized.StartsWith("audiosetupui", StringComparison.Ordinal))
            {
                if (normalized.Contains("dev"))
                {
                    return "オーディオ一括設定dev.exe";
                }

                return "オーディオ一括設定.exe";
            }

            return baseName + ".exe";
        }

        private static int RunBatch(string batchPath, Action<string> log)
        {
            string workingDir = Path.GetDirectoryName(batchPath) ?? Environment.CurrentDirectory;
            log("[INFO] Running: " + batchPath);
            return RunProcess("cmd.exe", "/c \"\"" + batchPath + "\"\"", workingDir, log);
        }

        private static int RunProcess(string fileName, string arguments, string workingDir, Action<string> log)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = fileName;
            psi.Arguments = arguments;
            psi.WorkingDirectory = workingDir;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;

            using (Process process = new Process())
            {
                process.StartInfo = psi;
                process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        log(e.Data);
                    }
                };

                process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        log(e.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                return process.ExitCode;
            }
        }
    }

    internal static class BuildDragDropProgram
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BuildDragDropForm(args));
        }
    }
}
using OhMySys.Common.Const;
using OhMySys.Models.Records;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace OhMySys.Common.Helpers;

public static class ProcessHelper
{
    public static bool CanAccessMainModule(this Process process)
    {
        try
        {
            return !ProtectedProcesses.Names.Contains(process.MainWindowTitle);
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public static ProcessModule? GetProcessMainModule(this Process process)
    {
        if (!process.CanAccessMainModule())
        {
            return null;
        }

        return process.MainModule;
    }

    public static long GetProcessMemoryInMb(this Process process)
         => process.WorkingSet64 / (1024 * 1024);

    public static SystemImpact GetProcessImpact(this Process process)
    {
        var memoryInMb = process.WorkingSet64 / (1024 * 1024);

        return memoryInMb switch
        {
            < 100 => SystemImpact.Low,
            < 500 => SystemImpact.Medium,
            _ => SystemImpact.High
        };
    }

    public static string GetName(this Process process)
        => process.ProcessName;

    public static string GetTitle(this Process process)
    => process.MainWindowTitle;

    public static ProcessStatus GetStatus(this Process process)
    {
        if (!process.Responding && !process.HasExited)
        {
            return ProcessStatus.NotResponding;
        }

        return process.Responding ? ProcessStatus.Running : ProcessStatus.Stopped;
    }

    public static string GetDescription(this Process process)
        => process.CanAccessMainModule() ? process.MainModule.FileVersionInfo?.FileDescription : string.Empty;

    public static string GetInnerName(this Process process)
    => process.CanAccessMainModule() ? process.MainModule.FileVersionInfo?.InternalName : string.Empty;

    public static string GetPath(this Process process)
        => process.CanAccessMainModule() ? process.MainModule.FileName : string.Empty;

    public static BitmapSource? GetIcon(this Process process)
    {
        var mainModule = process.GetProcessMainModule();

        if (mainModule == null || string.IsNullOrEmpty(mainModule?.FileName))
        {
            return null;
        }

        var icon = Icon.ExtractAssociatedIcon(mainModule.FileName);

        return ToImageSource(icon);
    }

    public static BitmapSource? ToImageSource(this Icon? icon)
    {
        if (icon == null)
            return null;

        using (var iconStream = new MemoryStream())
        {
            icon.Save(iconStream);
            iconStream.Seek(0, SeekOrigin.Begin);

            var bitmap = new Bitmap(iconStream);
            var hBitmap = bitmap.GetHbitmap();

            return Application.Current.Dispatcher.Invoke(() =>
                Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions()));
        }
    }
}
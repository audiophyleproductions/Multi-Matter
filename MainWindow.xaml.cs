using System.IO;
using System.Media;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace USBFormatter;

public partial class MainWindow : Window
{
    private readonly List<(CheckBox Box, UsbVolume Drive)> _rows = new();
    private bool _busy;
    private SoundPlayer? _completionSound;
    private Stream? _completionStream;
    private static readonly string PreferencesPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Multi-Matter", "preferences.txt");
    private bool _preferencesLoaded;

    private static bool ReadSoundPreference()
    {
        try { return !File.Exists(PreferencesPath) || File.ReadAllText(PreferencesPath).Trim() != "sound=off"; }
        catch { return true; }
    }

    private void SaveSoundPreference()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(PreferencesPath)!);
            File.WriteAllText(PreferencesPath, SoundToggle.IsChecked == true ? "sound=on" : "sound=off");
        }
        catch { /* Non-critical preference: formatting must not depend on settings I/O. */ }
    }
    private void SoundToggle_Changed(object sender, RoutedEventArgs e)
    {
        if (_preferencesLoaded) SaveSoundPreference();
    }
    private void UpdateSelectAllState()
    {
        bool all = _rows.Count > 0 && _rows.All(x => x.Box.IsChecked == true);
        SelectAllButton.Content = all ? "☑  Deselect All" : "☐  Select All";
        SelectAllButton.IsEnabled = !_busy && _rows.Count > 0;
    }
    private void About_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(this,
            "Multi-Matter - Removable Drive Batch Formatter V1.0.1\n\n" +
            "Sound settings:\n" +
            "• Tick Play sound when batch finishes to enable the completion sound.\n" +
            "• Untick it to disable the completion sound.\n" +
            "• Click Test Sound to hear it at any time.\n" +
            "Your sound setting is saved automatically.\n\n" +
            "Copyright © 2026 Patrick Tiley",
            "About Multi-Matter", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    private void PlayCompletionSound(bool force = false)
    {
        if (!force && SoundToggle.IsChecked != true) return;
        try
        {
            if (_completionSound is not null)
            {
                // Asynchronous playback: the results dialog can remain responsive.
                _completionSound.Play();
            }
            else
            {
                SystemSounds.Asterisk.Play();
            }
        }
        catch
        {
            try { SystemSounds.Asterisk.Play(); } catch { /* Audio is non-critical. */ }
        }
    }

    private void TestSound_Click(object sender, RoutedEventArgs e) => PlayCompletionSound(force: true);

    public MainWindow()
    {
        InitializeComponent();
        SoundToggle.IsChecked = ReadSoundPreference();
        _preferencesLoaded = true;
        // Private builds can include an embedded PCM WAV as an optional resource.
        // A public source build without that resource uses the OS notification sound.
        try
        {
            _completionStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("MultiMatter.CompletionChime.wav");
            if (_completionStream is not null)
            {
                var player = new SoundPlayer(_completionStream);
                player.Load();
                _completionSound = player;
            }
        }
        catch
        {
            _completionSound?.Dispose();
            _completionSound = null;
            _completionStream?.Dispose();
            _completionStream = null;
        }
        Closed += (_, _) =>
        {
            _completionSound?.Dispose();
            _completionStream?.Dispose();
        };
        Loaded += async (_, _) => await RefreshAsync();
        Closing += (_, e) => { if (_busy) { e.Cancel = true; MessageBox.Show("Please allow the active formatting operation to finish."); } };
    }
    private void SetBusy(bool busy)
    {
        _busy = busy;
        RefreshButton.IsEnabled = !busy;
        SelectAllButton.IsEnabled = !busy;
        FormatButton.IsEnabled = !busy;
        FileSystemBox.IsEnabled = !busy;
        LabelBox.IsEnabled = !busy;
        foreach (var row in _rows) row.Box.IsEnabled = !busy;
        UpdateSelectAllState();
    }
    private async Task RefreshAsync()
    {
        SetBusy(true);
        StatusText.Text = "Scanning Windows removable drives...";
        try
        {
            var drives = await Task.Run(Storage.Find);
            DriveList.Children.Clear();
            _rows.Clear();
            DriveCount.Text = $"Connected removable drives ({drives.Count})";
            foreach (var drive in drives)
            {
                var checkbox = new CheckBox { Content = drive.Description, Margin = new Thickness(9, 11, 9, 11), FontSize = 13 };
                var border = new Border { CornerRadius = new CornerRadius(7), Background = new SolidColorBrush(Color.FromRgb(54, 54, 54)), Margin = new Thickness(0, 0, 0, 6), Child = checkbox };
                DriveList.Children.Add(border);
                _rows.Add((checkbox, drive));
                checkbox.Checked += (_, _) => UpdateSelectAllState();
                checkbox.Unchecked += (_, _) => UpdateSelectAllState();
            }
            if (drives.Count == 0)
                DriveList.Children.Add(new TextBlock { Text = "No eligible removable drives detected. Insert USB/SD media and select Refresh.", Foreground = Brushes.LightGray, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(10) });
            StatusText.Text = "Ready. Select the removable drives to format.";
        }
        catch (Exception ex) { StatusText.Text = "Drive scan failed: " + ex.Message; }
        finally { SetBusy(false); }
    }
    private async void Refresh_Click(object sender, RoutedEventArgs e) => await RefreshAsync();
    private void SelectAll_Click(object sender, RoutedEventArgs e)
    {
        if (_busy || _rows.Count == 0) return;
        var select = !_rows.All(x => x.Box.IsChecked == true);
        foreach (var row in _rows) row.Box.IsChecked = select;
        UpdateSelectAllState();
    }

    private async void Format_Click(object sender, RoutedEventArgs e)
    {
        if (_busy) return;
        var selected = _rows.Where(x => x.Box.IsChecked == true).Select(x => x.Drive).ToArray();
        if (selected.Length == 0) { MessageBox.Show("Select at least one removable drive."); return; }
        var fs = ((ComboBoxItem)FileSystemBox.SelectedItem).Content.ToString()!;
        var label = LabelBox.Text.Trim();
        var max = fs == "FAT32" ? 11 : fs == "exFAT" ? 15 : 32;
        if (label.Length > max || Regex.IsMatch(label, "[\\\\/:*?\"<>|\\x00-\\x1F]") || label.EndsWith('.'))
        {
            MessageBox.Show($"Invalid label. {fs} supports a maximum of {max} characters. Avoid reserved characters and a final period.", "Invalid label");
            return;
        }
        if (fs == "FAT32" && selected.Any(x => x.PartitionBytes > 32UL * 1024 * 1024 * 1024))
        {
            MessageBox.Show("Windows may reject FAT32 formatting above 32 GiB. Choose exFAT."); return;
        }
        // Validate all choices again BEFORE showing the final erase confirmation.
        List<UsbVolume> fresh;
        try { fresh = await Task.Run(Storage.Find); }
        catch (Exception ex) { MessageBox.Show("Could not recheck drives: " + ex.Message); return; }
        if (selected.Any(d => fresh.Count(x => x.Letter == d.Letter && x.Identity == d.Identity && x.Bytes == d.Bytes) != 1))
        {
            MessageBox.Show("A selected removable drive changed. Refresh and reselect drives.", "Safety check"); return;
        }
        var letters = string.Join(", ", selected.Select(x => x.Letter + ":"));
        var answer = MessageBox.Show($"PERMANENTLY ERASE ALL FILES on:\n\n{letters}\n\nFile system: {fs}\nVolume label: {(label.Length == 0 ? "(blank)" : label)}\n\nContinue?",
            "Confirm permanent erasure", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
        if (answer != MessageBoxResult.Yes) return;
        SetBusy(true);
        var ok = 0; var errors = new List<string>();
        Progress.Value = 0;
        try
        {
            for (int i = 0; i < selected.Length; i++)
            {
                var d = selected[i];
                StatusText.Text = $"Formatting {d.Letter}: ({i + 1} of {selected.Length}) as {fs}...";
                try
                {
                    await Task.Run(() => Storage.Format(d, fs, label));
                    ok++;
                    StatusText.Text = $"{d.Letter}: completed.";
                    var row = _rows.FirstOrDefault(x => x.Drive.Letter == d.Letter);
                    if (row.Box != null) row.Box.Content = d.Description + "   [DONE]";
                }
                catch (Exception ex)
                {
                    errors.Add($"{d.Letter}: {ex.Message}");
                    var row = _rows.FirstOrDefault(x => x.Drive.Letter == d.Letter);
                    if (row.Box != null) row.Box.Content = d.Description + "   [FAILED]";
                }
                Progress.Value = 100d * (i + 1) / selected.Length;
            }
            StatusText.Text = $"Finished: {ok} successful, {errors.Count} failed. Refresh to update drive details.";
            PlayCompletionSound();
            MessageBox.Show($"Successful: {ok}\nFailed: {errors.Count}" + (errors.Count > 0 ? "\n\n" + string.Join("\n", errors) : ""),
                "Batch formatting results", MessageBoxButton.OK, MessageBoxImage.None);
        }
        finally { SetBusy(false); }
    }
}

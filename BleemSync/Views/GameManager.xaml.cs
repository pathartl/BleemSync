using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using BleemSync.Data.Models;
using BleemSync.Services;

namespace AvaloniaApplication1.Views
{
    public class GameManager : UserControl
    {
        private GameService GameService { get; set; }

        public GameManager()
        {
            InitializeComponent();

            GameService = new GameService();

            AddHandler(DragDrop.DropEvent, Drop);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.Contains(DataFormats.FileNames))
            {
                e.DragEffects = DragDropEffects.None;
            }
            else
            {
                e.DragEffects = DragDropEffects.Link;

                foreach (var file in e.Data.GetFileNames())
                {
                    var fingerprintService = new FingerprintService();

                    string fingerprint = fingerprintService.GetFingerprint(file);

                    var game = new Game() {
                        Path = file,
                        Fingerprint = fingerprint
                    };

                    GameService.Add(game);
                }
            }
        }
    }
}

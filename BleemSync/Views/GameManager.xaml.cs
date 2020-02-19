using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using BleemSync.Data.Models;
using BleemSync.Services;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BleemSync.Views
{
    public class GameManager : UserControl
    {
        private class PageViewModel : ReactiveObject
        {
            public ObservableCollection<string> _profiles;

            public ObservableCollection<string> Profiles
            {
                get => _profiles;
                set => this.RaiseAndSetIfChanged(ref _profiles, value);
            }

            private ObservableCollection<Game> _games;

            public ObservableCollection<Game> Games {
                get => _games;
                set => this.RaiseAndSetIfChanged(ref _games, value);
            }

            public PageViewModel(ICollection<Game> games)
            {
                Games = new ObservableCollection<Game>(games);
                Profiles = new ObservableCollection<string>()
                {
                    "Profile 1",
                    "Profile 2",
                    "Profile 3"
                };
            }
        }

        private GameService GameService { get; set; }
        private PlatformService PlatformService { get; set; }
        private PageViewModel ViewModel { get; set; }

        public GameManager()
        {
            InitializeComponent();

            GameService = new GameService();
            PlatformService = new PlatformService();

            ViewModel = new PageViewModel(GameService.Get().ToList());
            DataContext = ViewModel;

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
                    var scraperService = new ScraperService();
                    
                    string fingerprint = fingerprintService.GetFingerprint(file);

                    var game = scraperService.ScrapeGame(fingerprint);

                    game.PlatformId = PlatformService.Get("PlayStation").Id;
                    game.Path = file;

                    GameService.Add(game);

                    ViewModel.Games.Add(game);
                }
            }
        }
    }
}

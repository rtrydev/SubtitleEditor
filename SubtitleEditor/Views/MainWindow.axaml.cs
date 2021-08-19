using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LibVLCSharp.Shared;
using NAudio.Wave;
using SubtitleEditor.Subtitles;
using SubtitleEditor.ViewModels;

namespace SubtitleEditor.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            File.Delete("tmp.srt");
            AvaloniaXamlLoader.Load(this);
        }

        private async void LoadSubtitleFile(object? sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            var result = await fileDialog.ShowAsync(this);
            if(result is null) return;
            if(result[0].Split(".")[^1] != "srt") return;
            var vm = DataContext as MainWindowViewModel;
            var loader = new SubtitleLoader();
            vm.Subtitles = loader.LoadSubtitles(result[0]);
            var editor = new SubtitleEdit();
            editor.SaveTempSubs(vm.Subtitles);
            if(vm.MediaPlayer.Media is not null)
                vm.Play(vm.VideoLocation);
        }

        private async void OpenVideoFile(object? sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            var result = await fileDialog.ShowAsync(this);
            if(result is null) return;
            var vm = DataContext as MainWindowViewModel;

            vm?.Play(result[0]);
            vm.VideoLocation = result[0];
            //var audio = new AudioExtractor();
            //audio.Extract(result[0]);
        }

        private void PlayStop(object? sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            if(vm.MediaPlayer.Media is null) return;
            if (vm.MediaPlayer.IsPlaying)
            {
                vm.MediaPlayer.Pause();
                (sender as Button).Content = "";
            }
            else
            {
                vm.MediaPlayer.Play();
                (sender as Button).Content = "";
            }

        }

        private void AdjustSubtitles(object? sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            if(vm.MediaPlayer.Length == -1) return;
            var timeStamp =(long)(vm.MediaPlayer.Position * vm.MediaPlayer.Length);
            var ms =(int) timeStamp % 1000;
            var s =(int) timeStamp / 1000;
            var remS = s % 60;
            var m = s / 60;
            var time = new TimeSpan(0, 0, m, remS, ms);
            var editor = new SubtitleEdit();
            editor.SetStartTime(vm.Subtitles, time);
            var loader = new SubtitleLoader();
            vm.Subtitles = loader.LoadSubtitles("tmp.srt");
            vm.Play(vm.VideoLocation);
            
        }

        private void GoBackInVideo(object? sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            if(vm.MediaPlayer.Length == -1) return;
            vm.VideoPosition -= 1000f / vm.MediaPlayer.Length;
        }
        
        private void GoForwardInVideo(object? sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            if(vm.MediaPlayer.Length == -1) return;
            vm.VideoPosition += 1000f / vm.MediaPlayer.Length;
        }

        private async void SaveFile(object? sender, RoutedEventArgs e)
        {
            if(!File.Exists("tmp.srt")) return;
            var dialog = new SaveFileDialog();
            var destination = await dialog.ShowAsync(this);
            if(File.Exists(destination)) File.Delete(destination);
            if(destination is not null)
                File.Copy("tmp.srt", destination);
        }
    }
}
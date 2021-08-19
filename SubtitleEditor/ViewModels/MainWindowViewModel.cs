using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using LibVLCSharp.Shared;
using ReactiveUI.Fody.Helpers;
using SubtitleEditor.Models;

namespace SubtitleEditor.ViewModels
{
    
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly LibVLC _libVlc = new LibVLC("--sub-file=tmp.srt");
        public MainWindowViewModel()
        {
            MediaPlayer = new MediaPlayer(_libVlc);
            MediaPlayer.LengthChanged += (sender, e) =>
            {
                VideoLength = MediaPlayer.Length;
            };
            MediaPlayer.PositionChanged += (sender, e) =>
            {
                if (Math.Abs(MediaPlayer.Position - VideoPosition) > 0.001) MediaPlayer.Position = VideoPosition;
                else VideoPosition = MediaPlayer.Position;
            };
            MediaPlayer.PausableChanged += (sender, e) =>
            {
                if (MediaPlayer.CanPause) Playing = true;
                else Playing = false;
            };
        }

        public void Play(string path)
        {
            if (MediaPlayer.IsPlaying)
            {
                Dispose();
                return;
            }
            using var media = new Media(_libVlc, new Uri(path));
            MediaPlayer.Play(media);
        }
        
        public MediaPlayer MediaPlayer { get; }
        [Reactive]public long VideoLength { get; set; }
        [Reactive]public float VideoPosition { get; set; }
        [Reactive]public bool Playing { get; set; }
        [Reactive]public List<Subtitle> Subtitles { get; set; }
        [Reactive] public string WaveformLocation { get; set; }
        [Reactive]public string VideoLocation { get; set; }

        public void Dispose()
        {
            MediaPlayer?.Dispose();
            _libVlc?.Dispose();
        }

    }
}
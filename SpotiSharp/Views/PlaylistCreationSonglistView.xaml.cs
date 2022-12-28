using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotiSharp.ViewModels;

namespace SpotiSharp.Views;

public partial class PlaylistCreationSonglistView : ContentView
{
    public PlaylistCreationSonglistView()
    {
        InitializeComponent();
        BindingContext = new PlaylistCreationSonglistViewModel();
    }
}
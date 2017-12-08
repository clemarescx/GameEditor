using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using GameEditor.Models;
using GameEditor.Properties;

namespace GameEditor.ViewModels
{
    /// <summary>
    ///     Bindings for visualising a Tile object
    /// </summary>
    public class TileViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _characterToSpawn;
        private string _destinationAreaName;
        private bool _isWalkable;
        private string _spriteName;
        private Tile _tile;
        public Tile Tile
        {
            get => _tile;
            set
            {
                _tile = value;
                OnPropertyChanged();
            }
        }

        public string SpriteName
        {
            get => _spriteName;
            set
            {
                _spriteName = value;
                OnPropertyChanged();
            }
        }

        public string CharacterToSpawn
        {
            get => _characterToSpawn;
            set
            {
                _characterToSpawn = value;
                OnPropertyChanged();
            }
        }
        public string DestinationAreaName
        {
            get => _destinationAreaName;
            set
            {
                _destinationAreaName = value;
                OnPropertyChanged();
            }
        }

        public bool IsWalkable
        {
            get => _isWalkable;
            set
            {
                _isWalkable = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Console.WriteLine($"changed {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public new event PropertyChangedEventHandler PropertyChanged;
    }
}

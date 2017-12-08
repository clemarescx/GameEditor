using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GameEditor.Messages;
using GameEditor.Models;
using GameEditor.Properties;
using GameEditor.Services;

namespace GameEditor.ViewModels
{
    public class CampaignEditorViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // This is for the MVVMlight framework, not really necessary as we don't use the
        // "design" implementation 
        private readonly ICampaignEditorService _campaignEditorService;

        private ObservableCollection<Map> _campaignMaps;
        private RelayCommand _btnAddMapCommand;
        private RelayCommand _btnCreateCampaignCommand;
        private RelayCommand _btnLoadCampaignCommand;
        private RelayCommand _btnPrintCampaignCommand;
        private RelayCommand _btnRemoveMapCommand;
        private RelayCommand _btnSaveCampaignCommand;

        private int _mapNamingCounter = 1;
        private Map _selectedMap;
        private Campaign _currentCampaign;

        ///////
        // All member data with OnPropertyChanged()
        // are tracked for updates by the view
        public ObservableCollection<Map> CampaignMaps
        {
            get => _campaignMaps;
            set
            {
                _campaignMaps = value;
                OnPropertyChanged();
                Messenger.Default.Send(new AvailableMapsMessage(new List<Map>(_campaignMaps)));
            }
        }
        /// <summary>
        ///     A copy of the loaded model.
        /// </summary>
        public Campaign CurrentCampaign
        {
            get => _currentCampaign;
            set
            {
                _currentCampaign = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     The map currently selected in the listbox.
        ///     Sent to the MapEditor as soon as it is selected
        /// </summary>
        public Map SelectedMap
        {
            get => _selectedMap;
            set
            {
                _selectedMap = value;
                Messenger.Default.Send(new MapSelectedMessage(_selectedMap));
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Load the campaign model
        /// </summary>
        public RelayCommand BtnLoadCampaignCommand =>
            _btnLoadCampaignCommand ?? ( _btnLoadCampaignCommand = new RelayCommand(LoadCampaign) );

        /// <summary>
        ///     Save the current campaign
        /// </summary>
        public RelayCommand BtnSaveCampaignCommand
        {
            get
            {
                return _btnSaveCampaignCommand
                       ?? ( _btnSaveCampaignCommand = new RelayCommand(SaveCampaign, () => CurrentCampaign != null) );
            }
        }

        /// <summary>
        ///     Add a Map to the currentCampaign
        /// </summary>
        public RelayCommand BtnAddMapCommand
        {
            get
            {
                return _btnAddMapCommand
                       ?? ( _btnAddMapCommand = new RelayCommand(
                                () => {
                                    var newmapname = $"newMap{_mapNamingCounter++}";
                                    CampaignMaps.Add(new Map(newmapname));
                                },
                                () => CurrentCampaign != null) );
            }
        }

        /// <summary>
        ///     Remove a Map from the currentCampaign
        /// </summary>
        public RelayCommand BtnRemoveMapCommand
        {
            get
            {
                return _btnRemoveMapCommand
                       ?? ( _btnRemoveMapCommand = new RelayCommand(
                                () => CampaignMaps.Remove(SelectedMap),
                                () => CurrentCampaign != null) );
            }
        }

        /// <summary>
        ///     Create a new Campaign with one starting Map by default
        /// </summary>
        public RelayCommand BtnCreateCampaignCommand =>
            _btnCreateCampaignCommand ?? ( _btnCreateCampaignCommand = new RelayCommand(CreateCampaign) );

        /// <summary>
        ///     Command to print the content of the loaded currentCampaign
        /// </summary>
        public RelayCommand BtnPrintCampaignCommand
        {
            get
            {
                return _btnPrintCampaignCommand
                       ?? ( _btnPrintCampaignCommand = new RelayCommand(PrintCampaign, () => CurrentCampaign != null) );
            }
        }

        /// <summary>
        ///     ViewModel for the CampaignEditor view. Contains commands and functionality
        ///     that would otherwise be in the code-behind.
        ///     The constructor registers to messages fired by other viewModels.
        /// </summary>
        /// <param name="service"></param>
        public CampaignEditorViewModel(ICampaignEditorService service)
        {
            _campaignEditorService = service;
            CampaignMaps = new ObservableCollection<Map>();

            // inter-viewModel messaging system 
            // with MVVMlight ;)
            // Update the selected Map with the one opened in MapEditor
            Messenger.Default.Register<SaveMapMessage>(
                this,
                msg => {
                    if(CampaignMaps.Contains(SelectedMap))
                    {
                        Console.WriteLine("Removing '{0}'", SelectedMap.Name);
                        CampaignMaps.Remove(SelectedMap);
                        CampaignMaps.Add(msg.SavedMap);
                        SelectedMap = msg.SavedMap;
                    }
                });
            
            CreateCampaign();
        }

        private void CreateCampaign()
        {
            if(CurrentCampaign != null)
            {
                var res = MessageBox.Show(
                    "Would you like to save the current campaign?",
                    "Load campaign",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);
                switch(res)
                {
                    case MessageBoxResult.Yes:
                        SaveCampaign();
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        return;
                }
            }

            CurrentCampaign = new Campaign("newCampaign");
            CampaignMaps.Clear();
            CampaignMaps.Add(new Map("Start_Area"));
            PrintCampaign();
        }

        /// <summary>
        ///     A debug function to make sure the ObservableCollection CampaignMaps
        ///     and the CurrentCampaign's Maps are synchronised when it matters
        ///     (e.g: when saving the currentCampaign)
        /// </summary>
        private void PrintCampaign()
        {
            Console.WriteLine($@"Content of currentCampaign '{CurrentCampaign.Name}':");
            foreach(var m in CurrentCampaign.Maps)
                Console.WriteLine($@"	{m.Name}");

            Console.WriteLine($@"Content of CampaignMaps:");
            foreach(var m in CampaignMaps)
                Console.WriteLine($@"	{m.Name}");
        }

        /// <summary>
        ///     Load the currentCampaign by callback from associated service.
        /// </summary>
        private void LoadCampaign()
        {
            if(CurrentCampaign != null)
            {
                var res = MessageBox.Show(
                    "Would you like to save the current campaign?",
                    "Load campaign",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);
                switch(res)
                {
                    case MessageBoxResult.Yes:
                        SaveCampaign();
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        return;
                }
            }

            _campaignEditorService.LoadCampaign(
                (campaign, error) => {
                    if(error != null)
                        MessageBox.Show("Could not load Campaign from service: " + error.Message);
                    else
                    {
                        if(campaign != null)
                        {
                            Console.WriteLine("Loaded!");
                            CurrentCampaign = campaign;
                            CampaignMaps = new ObservableCollection<Map>(CurrentCampaign.Maps);
                            PrintCampaign();
                        }
                    }
                });
        }

        /// <summary>
        ///     WE'RE GONNA SAVE THE *burrp* WORLD MORTY
        /// </summary>
        private void SaveCampaign()
        {
            CurrentCampaign.Maps = CampaignMaps.ToList();
            _campaignEditorService.SaveCampaign(CurrentCampaign);
            PrintCampaign();
        }

        /// <summary>
        ///     "Enhanced" implementation of INotifyPropertyChanged,
        ///     [CallerMemberName] automatically finds the source property
        ///     calling it. No more bugs from spelling mistakes in the argument string!
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Console.WriteLine($"{propertyName} modified.");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public new event PropertyChangedEventHandler PropertyChanged;
    }
}

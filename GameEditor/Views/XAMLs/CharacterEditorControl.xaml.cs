using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameEditor.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GameEditor
{
    /// <summary>
    ///     Interaction logic for CharacterEditorControl.xaml
    /// </summary>
    public partial class CharacterEditorControl : UserControl
    {
        private readonly string _applicationDirectory;
        private readonly string _characterssDirPath;
        private Character _currentCharacter;

        public Character CurrentCharacter
        {
            get { return _currentCharacter; }
            private set { _currentCharacter = value; }
        }

        public CharacterEditorControl()
        {
            CurrentCharacter = new Character();
            InitializeComponent();

            var currentDirPath = Assembly.GetExecutingAssembly().Location;
            _applicationDirectory = Path.GetDirectoryName(currentDirPath);

            if(_applicationDirectory != null)
                _characterssDirPath = Path.Combine(_applicationDirectory, @"characters\");
            Directory.CreateDirectory(_characterssDirPath);

            LoadDefaultCharacterData();
        }

        private void LoadDefaultCharacterData()
        {
            // create the "data" folder
            var dataDirPath = Path.Combine(_applicationDirectory, @"data\");
            Directory.CreateDirectory(dataDirPath);

            var raceDbFilepath = Path.Combine(Path.GetDirectoryName(dataDirPath), "races.json");

            if(!File.Exists(raceDbFilepath)) CreateDefaultRaceDatabase(raceDbFilepath);

            try
            {
                var raceFile = File.ReadAllText(raceDbFilepath);
                var races = JsonConvert.DeserializeObject<string[]>(raceFile);
                CmbRace.ItemsSource = races;
                CmbRace.SelectedIndex = 0;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        private void CreateDefaultRaceDatabase(string racesDbFilepath)
        {
            // create races.json populated with default hardcoded values
            var jsonConvertedRaces = JsonConvert.SerializeObject(new[]{ "Human", "Elf", "Dwarf", "Orc" });
            try
            {
                File.WriteAllText(racesDbFilepath, jsonConvertedRaces);
            }
            catch(Exception e)
            {
                Console.WriteLine($@"Could not create races.json: {e.Message}");
                throw;
            }
        }

        // Serialize Character to JSON
        private void BtnSaveChar(object sender, RoutedEventArgs e)
        {
            CurrentCharacter.Name = TxtCharacterName.Text;
            CurrentCharacter.Strength = (int)SldStrength.Value;
            CurrentCharacter.Dexterity = (int)SldDexterity.Value;
            CurrentCharacter.RaceIndex = CmbRace.SelectedIndex;
            CurrentCharacter.Inventory = new List<string>();

            var jsonConvertCharacter = JsonConvert.SerializeObject(CurrentCharacter);

            var saveFileDialog = new SaveFileDialog{
                FileName = CurrentCharacter.Name + ".json",
                InitialDirectory = _characterssDirPath,
                Filter = "JSON file (*.json)|*.json"
            };

            if(saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, jsonConvertCharacter);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: \n" + ex.Message);
                }
            }
        }

        private void SaveCharacterOnline()
        {
            //TODO: refactor DatabaseManager project to work with GameEditor.Character instead of Character_Test
        }

        // Deserialize Character to JSON
        private void BtnLoadChar(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog{ InitialDirectory = _characterssDirPath };

            if(openFileDialog.ShowDialog() != true)
                return;

            var characterJson = File.ReadAllText(openFileDialog.FileName);
            CurrentCharacter = JsonConvert.DeserializeObject<Character>(characterJson);
            
            TxtCharacterName.Text = CurrentCharacter.Name;
            TxtHealthPoints.Text = CurrentCharacter.HealthPoints.ToString();
            SldStrength.Value = CurrentCharacter.Strength;
            SldDexterity.Value = CurrentCharacter.Dexterity;
            CmbRace.SelectedIndex = CurrentCharacter.RaceIndex;

            var itemList = "";
            foreach(var item in CurrentCharacter.Inventory) itemList += item + "\n";

            TxtCharacterDetails.Text = itemList;
        }

        // Quick validation check for health points
        private void OnHealthPointsChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if(string.IsNullOrWhiteSpace(TxtHealthPoints.Text)) TxtHealthPoints.Text = "";
            else
            {
                if(!int.TryParse(TxtHealthPoints.Text, out var hp)) TxtHealthPoints.Foreground = Brushes.Red;
                else
                {
                    TxtHealthPoints.Foreground = Brushes.Black;
                    CurrentCharacter.HealthPoints = hp;
                }
            }
        }
    }
}

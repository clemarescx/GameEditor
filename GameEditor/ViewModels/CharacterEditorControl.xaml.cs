using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GameEditor{
	/// <summary>
	/// Interaction logic for CharacterEditorControl.xaml
	/// </summary>
	public partial class CharacterEditorControl : UserControl{
		private readonly string _characterssDirPath;

		private readonly string _applicationDirectory;

		//		private readonly string _currentDirPath;
		private Character _character;

		public bool IsDirty{ get; set; }

		public CharacterEditorControl(){
			InitializeComponent();

			var currentDirPath = Assembly.GetExecutingAssembly().Location;
			_applicationDirectory = Path.GetDirectoryName(currentDirPath);

			if(_applicationDirectory != null)
				_characterssDirPath = Path.Combine(_applicationDirectory, @"creatures\");
			Directory.CreateDirectory(_characterssDirPath);

			LoadDefaultCharacterData();
		}

		private void LoadDefaultCharacterData(){
			// create the "data" folder
			var dataDirPath = Path.Combine(_applicationDirectory, @"data\");
			Directory.CreateDirectory(dataDirPath);

			var race_db_filepath = Path.Combine(Path.GetDirectoryName(dataDirPath), "races.json");

			if(!File.Exists(race_db_filepath)){
				CreateDefaultRaceDatabase(race_db_filepath);
			}

			try{
				var raceFile = File.ReadAllText(race_db_filepath);
				var races = JsonConvert.DeserializeObject<string[]>(raceFile);
				cmbRace.ItemsSource = races;
				cmbRace.SelectedIndex = 0;
			}
			catch(Exception e){
				Console.WriteLine(e);
				throw;
			}
			
		}

		private void CreateDefaultRaceDatabase(string races_db_filepath){
			Console.WriteLine("Creating default races.json...");
			// create races.json populated with default hardcoded values
			var jsonConvertedRaces = JsonConvert.SerializeObject(new[] { "Human", "Elf", "Dwarf", "Orc" });
			try {
				File.WriteAllText(races_db_filepath, jsonConvertedRaces);
			}
			catch(Exception e) {
				Console.WriteLine($@"Could not create races.json: {e.Message}");
				throw;
			}
		}


		private void BtnSaveChar(object sender, RoutedEventArgs e){

			if(IsDirty){
				Console.WriteLine("CONTENT LOADED!!!");
			}

			_character.Name = txtCreatureName.Text;
			_character.Strength = (int)sldStrength.Value;
			_character.Dexterity = (int)sldDexterity.Value;
			_character.RaceIndex = cmbRace.SelectedIndex;

			var jsonConvertCreature = JsonConvert.SerializeObject(_character);

			SaveFileDialog saveFileDialog = new SaveFileDialog{
				FileName = _character.Name + ".json",
				InitialDirectory = _characterssDirPath,
				Filter = "JSON file (*.json)|*.json"
			};

			if(saveFileDialog.ShowDialog() == true){
				try{
					File.WriteAllText(saveFileDialog.FileName, jsonConvertCreature);
				}
				catch(Exception ex){
					MessageBox.Show("Error: \n" + ex.Message);
				}
			}
		}

		private void SaveCharacter(){
			
		}

		private void BtnLoadChar(object sender, RoutedEventArgs e){
			var openFileDialog = new OpenFileDialog{ InitialDirectory = _characterssDirPath };

			if(openFileDialog.ShowDialog() != true)
				return;

			try{
				var jsonCreature = File.ReadAllText(openFileDialog.FileName);
				_character = JsonConvert.DeserializeObject<Character>(jsonCreature);

				txtCreatureName.Text = _character.Name;
				txtHealthPoints.Text = _character.HealthPoints.ToString();
				sldStrength.Value = _character.Strength;
				sldDexterity.Value = _character.Dexterity;
				cmbRace.SelectedIndex = _character.RaceIndex;

				var itemList = "";
				foreach(var item in _character.Inventory){
					itemList += item + "\n";
				}

				txtCreatureDetails.Text = itemList;
			}
			catch(Exception ex){
				MessageBox.Show("Error: \n" + ex.Message);
				throw;
			}
		}

		private void OnHealthPointsChanged(object sender, TextChangedEventArgs textChangedEventArgs){
			if(string.IsNullOrWhiteSpace(txtHealthPoints.Text)){
				txtHealthPoints.Text = "";
			}
			else{
				if(!int.TryParse(txtHealthPoints.Text, out int hp)){
					txtHealthPoints.Foreground = Brushes.Red;
				}
				else{
					txtHealthPoints.Foreground = Brushes.Black;
					_character.HealthPoints = hp;
				}
			}
		}
	}
}
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

		private Character _character;

		public CharacterEditorControl()
		{
			InitializeComponent();

			var currentDirPath = Assembly.GetExecutingAssembly().Location;
			_applicationDirectory = Path.GetDirectoryName(currentDirPath);

			if(_applicationDirectory != null)
				_characterssDirPath = Path.Combine(_applicationDirectory, @"creatures\");
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
				cmbRace.ItemsSource = races;
				cmbRace.SelectedIndex = 0;
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}


		private void CreateDefaultRaceDatabase(string racesDbFilepath)
		{
			Console.WriteLine(@"Done.");
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
			_character.Name = txtCreatureName.Text;
			_character.Strength = (int)sldStrength.Value;
			_character.Dexterity = (int)sldDexterity.Value;
			_character.RaceIndex = cmbRace.SelectedIndex;

			var jsonConvertCreature = JsonConvert.SerializeObject(_character);

			var saveFileDialog = new SaveFileDialog{
				FileName = _character.Name + ".json",
				InitialDirectory = _characterssDirPath,
				Filter = "JSON file (*.json)|*.json"
			};

			if(saveFileDialog.ShowDialog() == true)
			{
				try
				{
					File.WriteAllText(saveFileDialog.FileName, jsonConvertCreature);
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

			try
			{
				var jsonCreature = File.ReadAllText(openFileDialog.FileName);
				_character = JsonConvert.DeserializeObject<Character>(jsonCreature);

				txtCreatureName.Text = _character.Name;
				txtHealthPoints.Text = _character.HealthPoints.ToString();
				sldStrength.Value = _character.Strength;
				sldDexterity.Value = _character.Dexterity;
				cmbRace.SelectedIndex = _character.RaceIndex;

				var itemList = "";
				foreach(var item in _character.Inventory) itemList += item + "\n";

				txtCreatureDetails.Text = itemList;
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error: \n" + ex.Message);
				throw;
			}
		}

		// Quick validation check for health points
		private void OnHealthPointsChanged(object sender, TextChangedEventArgs textChangedEventArgs)
		{
			if(string.IsNullOrWhiteSpace(txtHealthPoints.Text)) txtHealthPoints.Text = "";
			else
			{
				if(!int.TryParse(txtHealthPoints.Text, out var hp)) txtHealthPoints.Foreground = Brushes.Red;
				else
				{
					txtHealthPoints.Foreground = Brushes.Black;
					_character.HealthPoints = hp;
				}
			}
		}
	}
}

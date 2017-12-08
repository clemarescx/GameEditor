using System;
using System.IO;
using System.Windows;
using GameEditor.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GameEditor.Services
{
    internal class CampaignEditorService : ICampaignEditorService
    {
        // Load World from JSON
        public void LoadCampaign(Action<Campaign, Exception> callback)
        {
            var openFileDialog = new OpenFileDialog{ InitialDirectory = Directory.GetCurrentDirectory() };

            if(openFileDialog.ShowDialog() != true)
                return;

            Campaign campaign = null;
            Exception error = null;
            try
            {
                var campaignJson = File.ReadAllText(openFileDialog.FileName);
                campaign = JsonConvert.DeserializeObject<Campaign>(campaignJson);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: \n" + ex.Message);
                error = ex;
            }

            callback(campaign, error);
        }

        // Save to JSON
        public void SaveCampaign(Campaign campaign)
        {
            var campaignJson = JsonConvert.SerializeObject(campaign);

            var filename = string.Empty.Equals(campaign.Name) || null == campaign.Name ? "new_Campaign" : campaign.Name;
            filename += ".json";

            var saveFileDialog = new SaveFileDialog{
                FileName = filename,
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "JSON file (*.json)|*.json"
            };
            campaign.Name = saveFileDialog.SafeFileName;

            if(saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, campaignJson);
                    Console.WriteLine("Saved!");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: \n" + ex.Message);
                }
            }
        }
    }
}

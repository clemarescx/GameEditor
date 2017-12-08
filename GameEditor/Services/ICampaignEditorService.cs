using System;
using GameEditor.Models;

namespace GameEditor.Services
{
    public interface ICampaignEditorService
    {
        void LoadCampaign(Action<Campaign, Exception> callback);
        void SaveCampaign(Campaign campaign);
    }
}

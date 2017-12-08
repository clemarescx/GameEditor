using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GameEditor.Design;
using GameEditor.Services;
using GameEditor.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace GameEditor
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        ///     Instantiation of all ViewModels here
        /// </summary>
        [SuppressMessage(
            "Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ViewModels.ViewModelLocator.MainViewModel Main =>
            ServiceLocator.Current.GetInstance<ViewModels.ViewModelLocator.MainViewModel>();
        public MapEditorViewModel MapEditor => ServiceLocator.Current.GetInstance<MapEditorViewModel>();
        public CampaignEditorViewModel CampaignEditor => ServiceLocator.Current.GetInstance<CampaignEditorViewModel>();
        public TileViewModel TileView => ServiceLocator.Current.GetInstance<TileViewModel>();

        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<ViewModels.ViewModelLocator.MainViewModel>();

            if(ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.Register<IMapEditorService, DesignMapEditorService>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<IMapEditorService, MapEditorService>();
                SimpleIoc.Default.Register<ICampaignEditorService, CampaignEditorService>();
            }

            SimpleIoc.Default.Register<MapEditorViewModel>();
            SimpleIoc.Default.Register<CampaignEditorViewModel>();
            SimpleIoc.Default.Register<TileViewModel>();
        }

        public static void Cleanup()
        {
            //            throw new System.NotImplementedException();
        }
    }
}

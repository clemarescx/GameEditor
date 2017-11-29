using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GameEditor.Design;
using GameEditor.Services;
using Microsoft.Practices.ServiceLocation;

namespace GameEditor.ViewModels
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public partial class ViewModelLocator
    {
        public MapEditorModel MapEditor => ServiceLocator.Current.GetInstance<MapEditorModel>();

        /// <summary>
        ///     Gets the MainViewModel property.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();

            if(ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.Register<IMapEditorService, DesignMapEditorService>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Register<IMapEditorService, MapEditorService>();
            }

            SimpleIoc.Default.Register<MapEditorModel>();
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}

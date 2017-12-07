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
        
        /// <summary>
        ///     Gets the MainViewModel property.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public MapEditorViewModel MapEditor => ServiceLocator.Current.GetInstance<MapEditorViewModel>();
        public WorldEditorViewModel WorldEditor => ServiceLocator.Current.GetInstance<WorldEditorViewModel>();

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
                SimpleIoc.Default.Register<IWorldEditorService, WorldEditorService>();
            }

            SimpleIoc.Default.Register<MapEditorViewModel>();
            SimpleIoc.Default.Register<WorldEditorViewModel>();
        }

        public static void Cleanup()
        {
//            throw new System.NotImplementedException();
        }
    }
}

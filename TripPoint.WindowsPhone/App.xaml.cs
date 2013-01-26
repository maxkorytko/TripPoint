using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

using TripPoint.Model.Settings;
using TripPoint.Model.Data;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.State;
using TripPoint.WindowsPhone.Navigation;

namespace TripPoint.WindowsPhone
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;
            
            // Wire up dependencies
            RepositoryFactory.Initialize(new DatabaseRepositoryFactory());

            // Creates a dabase if necessary
            // Must be called before InitializeComponent
            InitializeDatabase();

            // Initializes application settings store
            // Must be called before InitializeComponent
            InitializeApplicationSettings();

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Application-specific initialization
            BootstrapPhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

        private void InitializeDatabase()
        {
            using (var dataContext = new TripPointDataContext(TripPointDataContext.ConnectionString))
            {
                if (!dataContext.DatabaseExists())
                {
                    dataContext.CreateDatabase();

                    // has the database been created?
                    if (!dataContext.DatabaseExists())
                        Logger.Log(this, "Could not create database!");
                }
            }
        }

        private static void InitializeApplicationSettings()
        {
            ApplicationSettings.Initialize(IsolatedStorageSettings.ApplicationSettings);
        }

        private void BootstrapPhoneApplication()
        {
            InitializeStateManager();
            InitializeNavigation();
            SetMapsApiKey();
            SetStartupPage();
        }

        private static void InitializeStateManager()
        {
            StateManager.Initialize(PhoneApplicationService.Current.State);
        }

        private void InitializeNavigation()
        {
            TripPointNavigation.Initialize(new TripPointNavigator(RootFrame));

            // Set custom URI mapper
            var uriMapper = new UriMapper();
            foreach (var uriMapping in TripPointUriMappings.UriMappings)
                uriMapper.UriMappings.Add(uriMapping);

            RootFrame.UriMapper = uriMapper;
        }

        /// <summary>
        /// Sets up the API key required for the static map control
        /// Search for Jeff Wilcox static map for more details
        /// </summary>
        private static void SetMapsApiKey()
        {
#if DEBUG
            var apiKey = "ApZs4BwJrG3d81SRc4_4OaRynkUcyiK3U_Ftfj9lxPr-kQRWTwR8oDy28hwNp3po";
#else
            var apiKey = "AsZpZsTO90M6EkEo95RmyJUCf3t8hd8oSD6u4MUwIgBWzI0uJk2x40DrQgAGOKNK";
#endif
            const string BING_MAPS_KEY = "BingMapsKey";

            var appResources = Application.Current.Resources;

            if (appResources.Contains(BING_MAPS_KEY)) return;
            appResources.Add(BING_MAPS_KEY, apiKey);
        }

        private static void SetStartupPage()
        {
            var tripRepository = RepositoryFactory.Create().TripRepository;

            var trips = tripRepository.Trips;
            var currentTrip = tripRepository.CurrentTrip;

            if (currentTrip != null)
                TripPointNavigation.Navigator.Navigate("/Trip/Current");
            else if (trips != null && trips.Count() > 0)
                TripPointNavigation.Navigator.Navigate("/Trips");
            else
                TripPointNavigation.Navigator.Navigate("/Trip/Create");
        }
    }
}
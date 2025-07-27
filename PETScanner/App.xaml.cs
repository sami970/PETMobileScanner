namespace PETScanner
{
    public partial class App : Application
    {
        public App()
        {
            // Register Syncfusion < sup >®</ sup > license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MDAxQDMyMzgyZTMwMmUzMGFlaU5ML011RWdGRzRJdWsxSVAxMjlJVXFiQWRnZll5MmZVRHJ3ZWUydGM9 ");
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}

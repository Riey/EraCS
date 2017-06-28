using Xamarin.Forms;

namespace EraCS.UI
{
    public class App : Application
    {
        public App(IEraConsole console)
        {
            MainPage = new MainPage(console);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

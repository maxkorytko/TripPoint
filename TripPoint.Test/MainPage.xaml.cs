using Microsoft.Phone.Controls;
using Microsoft.Silverlight.Testing;

namespace TripPoint.Test
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            const bool runUnitTests = true;
            if (runUnitTests)
            {
                Content = UnitTestSystem.CreateTestPage();
                IMobileTestPage imtp = Content as IMobileTestPage;
                if (imtp != null)
                {
                    BackKeyPress += (x, xe) => xe.Cancel = imtp.NavigateBack();
                }
            }
        }
    }
}
using LastSliceUWP.Models;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LastSliceUWP
{
    public sealed partial class MainPage : Page
    {
        private ChallengeService challengeService = new ChallengeService();

        public MainPage()
        {
            InitializeComponent();
            ShowUserCache();
        }

        private void ShowUserCache()
        {
            bool userHasLoggedIn = challengeService.HasUserLoggedIn();

            if (userHasLoggedIn)
            {
                currentUserText.Text = "Cached User in Storage";
            }
            else
            {
                currentUserText.Text = "No Cached User";
            }
        }

        private void btnClearUserCacheClick(object sender, RoutedEventArgs e)
        {
            challengeService.ClearUserCache();
            ShowUserCache();
        }

        private async void btnChallenge2Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string token = await challengeService.Login();

                if (!string.IsNullOrEmpty(token))
                {
                    string puzzle = await challengeService.GetPuzzle();

                    // TODO: Now show your Swagger and find the solution.

                    string solution = "";
                    string solutionResponse = await challengeService.PostSolutionToPuzzle(solution);

                    ResultText.Text = solutionResponse;

                    // TODO: Check the solution response to see if you got the correct solution
                }
            }
            catch (Exception ex)
            {
                string exceptionDetails = ex.ToString();
                ShowException(exceptionDetails);
            }

            ShowUserCache();
        }

        private async void ShowException(string message)
        {
            MessageDialog dialog = new MessageDialog(message, "Sorry, an error occurred.");
            await dialog.ShowAsync();
        }
    }
}

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LastSliceUWP.Models
{
    public class ChallengeService
    {
        private const string CHALLENGE_ROOT_URL = "https://kneadmoredough.azurewebsites.net/";

        private const string CHALLENGE_TWO_URL = CHALLENGE_ROOT_URL + "api/ChallengeTwo";

        private HttpClient client;

        public ChallengeService()
        {
            string tenant = "thelastslice.onmicrosoft.com";
            string clientId = "17315b0a-1d61-41c1-a614-aaa908fc6c3c";
            string webApiId = "webapi";
            string webApiResourceId = string.Format("https://{0}/{1}", tenant, webApiId);
            string signUpSignInPolicy = "B2C_1_SignUp_SignIn";
            string webApiScope = "default_scope";
            string[] scopes = new string[] { string.Format("https://{0}/{1}/{2}", tenant, webApiId, webApiScope) };
            client = new HttpClient(tenant, clientId, webApiResourceId, scopes, signUpSignInPolicy);
        }

        #region Login Methods

        public void ClearUserCache()
        {
            client.ClearCache();
        }

        public bool HasUserLoggedIn()
        {
            bool cachedUserExists = client.CachedUserExists();
            return cachedUserExists;
        }

        public async Task<string> Login()
        {
            string token = await client.GetAccessTokenAsync();
            return token;
        }

        #endregion

        #region Puzzle Methods

        public async Task<string> GetPuzzle()
        {
            var result = await client.GetUrlAsync(CHALLENGE_TWO_URL);
            string puzzleContent = await result.Content.ReadAsStringAsync();
            Print("Challenge Two puzzle", puzzleContent);
            return puzzleContent;
        }

        public async Task<string> PostSolutionToPuzzle(string solution)
        {
            var result = await client.PostUrlAsync(CHALLENGE_TWO_URL, solution);
            string solutionContent = await result.Content.ReadAsStringAsync();
            Print("Challenge Two solution response", solutionContent);
            return solutionContent;
        }

        #endregion

        #region Utility Methods

        private void Print(string message, string s)
        {
            Print(string.Format("{0}:\n {1}", message, s));
        }

        private void Print(string message)
        {
            Debug.WriteLine(message);
        }

        #endregion
    }
}

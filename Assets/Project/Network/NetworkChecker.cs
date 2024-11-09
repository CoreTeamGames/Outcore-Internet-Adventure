using System.Net;

namespace OutcoreInternetAdventure.Network
{
    /// <summary>
    /// A service for work with network
    /// </summary>
    public static class NetworkService
    {
        /// <summary>
        /// Checks internet connecion
        /// </summary>
        /// <returns></returns>
        public static bool CheckInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
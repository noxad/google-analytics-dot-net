// Code based on ASP.NET version of Google Analytics for Mobile Websites:
// -- https://developers.google.com/analytics/devguides/collection/other/mobileWebsites

using System;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace GoogleAnalyticsDotNet
{
    /// <summary>
    /// Send Google Analytics tracking requests from .NET applications
    /// </summary>
    public static class GoogleAnalyticsDotNet
    {
        private const string trackerVersion = "4.4sa";

        /// <summary>
        /// Send a tracking request to Google Analytics
        /// </summary>
        /// <param name="trackingId">Google Analytics tracking ID, e.g., UA-XXXXXXXX-XX</param>
        /// <param name="pageName">Name of the page for which a view should be logged. Could be the application name, 
        /// a particular page/dialog of the application, or just an empty string if none is desired</param>
        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
        public static void SendTrackingRequest(string trackingId, string pageName)
        {
            string visitorId = GetUniqueUserId();
            string utmGifLocation = "http://www.google-analytics.com/__utm.gif";

            // Construct the gif hit URL
            string trackingGifUrl = utmGifLocation + "?" +
                "utmwv=" + trackerVersion +
                "&utmn=" + GetRandomNumber() +
                "&utmp=" + pageName +
                "&utmac=" + trackingId +
                "&utmcc=__utma%3D999.999.999.999.999.1%3B" +
                "&utmvid=" + visitorId;

            SendRequestToGoogleAnalytics(trackingGifUrl);
        }

        /// <summary>
        /// Gets some information about the machine and currently logged on user to generate a consistent unique idenitifer for the user
        /// </summary>
        /// <returns>String to identify the user uniquely</returns>
        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
        private static string GetUniqueUserId()
        {
            string machineSid = string.Empty;
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(string.Format("WinNT://{0},Computer", Environment.MachineName)))
                {
                    SecurityIdentifier sid = new SecurityIdentifier((byte[])entry.Children.Cast<DirectoryEntry>().First().InvokeGet("objectSID"), 0).AccountDomainSid;
                    machineSid = sid.Value;
                }                
            }
            catch { }

            string machineName = string.Empty;
            try { machineName = Environment.MachineName; }
            catch { }

            string userName = string.Empty;
            try { userName = Environment.UserName; }
            catch { }

            // Take the three pieces of user/machine-specific info and create a hash with them
            using (MD5 sha = new MD5CryptoServiceProvider())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(machineSid + machineName + userName));

                string userHash = BitConverter.ToString(hash);
                userHash = userHash.Replace("-", string.Empty);

                return userHash;
            }
        }

        /// <summary>
        /// Sends the .gif tracking request
        /// </summary>
        /// <param name="trackingGifUrl">URL, including all of the necessary parameters, to send to Google Analytics</param>
        private static void SendRequestToGoogleAnalytics(string trackingGifUrl)
        {
            try
            {
                WebRequest request = WebRequest.Create(trackingGifUrl);
                request.Timeout = 30000;

                StartAsyncWebRequest(request);
            }
            catch { } // Eat any exceptions - if it fails, it fails
        }

        private static void StartAsyncWebRequest(WebRequest request)
        {
            request.BeginGetResponse(new AsyncCallback(FinishAsyncWebRequest), request);
        }

        private static void FinishAsyncWebRequest(IAsyncResult result)
        {
            using (HttpWebResponse response = (HttpWebResponse)((HttpWebRequest)result.AsyncState).EndGetResponse(result)) { }
        }

        /// <summary>
        /// Get a random number string
        /// </summary>
        /// <returns>Random number string</returns>
        private static String GetRandomNumber()
        {
            Random RandomClass = new Random();
            return RandomClass.Next(0x7fffffff).ToString();
        }
    }
}

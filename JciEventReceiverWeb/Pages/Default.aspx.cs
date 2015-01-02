using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JciEventReceiverWeb
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Uri redirectUrl;
            switch (SharePointContextProvider.CheckRedirectionStatus(Context, out redirectUrl))
            {
                case RedirectionStatus.Ok:
                    return;
                case RedirectionStatus.ShouldRedirect:
                    Response.Redirect(redirectUrl.AbsoluteUri, endResponse: true);
                    break;
                case RedirectionStatus.CanNotRedirect:
                    Response.Write("An error occurred while processing your request.");
                    Response.End();
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // The following code gets the client context and Title property by using TokenHelper.
            // To access other properties, the app may need to request permissions on the host web.
            var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);

            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {

               
                clientContext.Load(clientContext.Web, web => web.Title, web => web.CurrentUser.LoginName);
                clientContext.ExecuteQuery();
                Response.Write(clientContext.Web.Title + " " + clientContext.Web.CurrentUser);
                string fullname = GetProfilePropertyFor(clientContext, clientContext.Web.CurrentUser.LoginName, "FirstName");
                Response.Write("fullname:" + " " + fullname);
            }

          
        }

        /// <summary>
        /// Gets a user profile property Value for the specified user.
        /// </summary>
        /// <param name="ctx">An Authenticated ClientContext</param>
        /// <param name="userName">The name of the target user.</param>
        /// <param name="propertyName">The value of the property to get.</param>
        /// <returns><see cref="System.String"/>The specified profile property for the specified user. Will return an Empty String if the property is not available.</returns>
        public static string GetProfilePropertyFor(ClientContext ctx, string userName, string propertyName)
        {
            string _result = string.Empty;
            if (ctx != null)
            {
                //try
                //{
                //// PeopleManager class provides the methods for operations related to people
                PeopleManager peopleManager = new PeopleManager(ctx);
                //// GetUserProfilePropertyFor method is used to get a specific user profile property for a user
                var _profileProperty = peopleManager.GetUserProfilePropertyFor(userName, propertyName);
                ctx.ExecuteQuery();
                _result = _profileProperty.Value;
                //}
                //catch
                //{
                //    throw;
                //}
            }
            return _result;
        }
        
      
    }
}
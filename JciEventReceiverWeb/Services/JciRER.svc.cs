using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;

namespace JciEventReceiverWeb.Services
{
    public class JciRER : IRemoteEventService
    {
        /// <summary>
        /// Handles events that occur before an action occurs, such as when a user adds or deletes a list item.
        /// </summary>
        /// <param name="properties">Holds information about the remote event.</param>
        /// <returns>Holds information returned from the remote event.</returns>
        public SPRemoteEventResult ProcessEvent(SPRemoteEventProperties properties)
        {
            SPRemoteEventResult result = new SPRemoteEventResult();

            using (ClientContext clientContext = TokenHelper.CreateRemoteEventReceiverClientContext(properties))
            {
                if (clientContext != null)
                {
                    clientContext.Load(clientContext.Web);
                    
                    clientContext.ExecuteQuery();
                }
            }

            return result;
        }

        /// <summary>
        /// Handles events that occur after an action occurs, such as after a user adds an item to a list or deletes an item from a list.
        /// </summary>
        /// <param name="properties">Holds information about the remote event.</param>
        public void ProcessOneWayEvent(SPRemoteEventProperties properties)
        {
            /*
            using (ClientContext clientContext = TokenHelper.CreateRemoteEventReceiverClientContext(properties))
            {
                if (clientContext != null)
                {
                    clientContext.Load(clientContext.Web);
                    clientContext.ExecuteQuery();
                }
            }

            */
            using (ClientContext clientContext =
        TokenHelper.CreateRemoteEventReceiverClientContext(properties))
            {
                if (clientContext != null)
                {
                    string firstName =
                        properties.ItemEventProperties.AfterProperties[
                            "Title"
                            ].ToString();

                    string lastName =
                        properties.ItemEventProperties.AfterProperties[
                            "Title"
                            ].ToString();

                    List lstContacts =
                        clientContext.Web.Lists.GetByTitle(
                            properties.ItemEventProperties.ListTitle
                        );

                    ListItem itemContact =
                        lstContacts.GetItemById(
                            properties.ItemEventProperties.ListItemId
                        );

                    itemContact["Title"] =
                        String.Format("{0} {1}", firstName, lastName);
                    itemContact.Update();

                    clientContext.ExecuteQuery();
                }
            }

        }

        
        }
    }


using System;
using System.Xml.Linq;
using System.Linq;
using Xvpn.BusinessLogic.DataModels;
using System.Collections.Generic;
using System.Net;

namespace Xvpn.BusinessLogic
{
    public class XmlLocationResponseConverter : ILocationResponseConverter
    {
        public XvpnLocations Convert(string payload)
        {
            var locationResponse = new XvpnLocations();

            var xdoc = XDocument.Parse(payload);

            foreach (var element in xdoc.Root.Elements())
            {
                var name = element.Name.LocalName;

                switch (name)
                {
                    case XML_ICONS:
                        _IconDictionary = GetIconDictionary(GetIcons(element));
                        break;
                    case XML_LOCATIONS:
                        locationResponse.Locations = GetLocations(element);
                        break;
                    case XML_BUTTONTEXT:
                        locationResponse.ButtonText = element.Value;
                        break;
                    default:
                        break;
                }
            }

            return locationResponse;
        }

        private const string XML_ICONS = "icons";
        private const string XML_LOCATIONS = "locations";

        private const string XML_ICON_ID_ATTR = "id";
        private static XName IconIdAttributeName = XName.Get(XML_ICON_ID_ATTR);

        private const string XML_LOCATION_NAME_ATTR = "name";
        private static XName LocationNameAttributeName = XName.Get(XML_LOCATION_NAME_ATTR);

        private const string XML_LOCATION_SORT_ORDER_ATTR = "sort_order";
        private static XName LocationSortOrderAttributeName = XName.Get(XML_LOCATION_SORT_ORDER_ATTR);

        private const string XML_LOCATION_ICON_ID_ATTR = "icon_id";
        private static XName LocationIconIdAttributeName = XName.Get(XML_LOCATION_ICON_ID_ATTR);

        private const string XML_SERVER_IP_ATTR = "ip";
        private static XName ServerIPAttributeName = XName.Get(XML_SERVER_IP_ATTR);

        private const string XML_BUTTONTEXT = "button_text";

        private static IDictionary<int, Icon> _IconDictionary;       

        private static Icon GetIcon(XElement element)
        {
            var icon = new Icon();

            icon.Id = int.Parse(element.Attribute(IconIdAttributeName).Value);

            icon.Value = System.Convert.FromBase64String(element.Value);

            return icon;
        }

        private static IEnumerable<Icon> GetIcons(XElement element)
        {
            return element.Elements().Select(x => GetIcon(x)).ToArray();
        }

        private static IDictionary<int, Icon> GetIconDictionary(IEnumerable<Icon> icons)
        {
            return icons.GroupBy(x => x.Id).ToDictionary(x => x.Key, x => x.First());
        }                       

        private static Server GetServer(XElement element)
        {
            var server = new Server();

            server.IP = IPAddress.Parse(element.Attribute(ServerIPAttributeName).Value);

            return server;
        }

        private static IEnumerable<Server> GetServers(XElement element)
        {
            return element.Elements().Select(x => GetServer(x)).ToArray();
        }

        private static Location Getlocation(XElement element)
        {
            var location = new Location();

            location.Name = element.Attribute(LocationNameAttributeName).Value;

            location.SortOrder = int.Parse(element.Attribute(LocationSortOrderAttributeName).Value);

            var iconId = int.Parse(element.Attribute(LocationIconIdAttributeName).Value);

            if (_IconDictionary != null && _IconDictionary.ContainsKey(iconId))
            {
                location.Icon = _IconDictionary[iconId];
            }

            location.Servers = GetServers(element);

            return location;
        }

        private static IEnumerable<Location> GetLocations(XElement element)
        {
            return element.Elements().Select(x => Getlocation(x)).ToArray();
        }

    }
}
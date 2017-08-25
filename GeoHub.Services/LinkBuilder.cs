using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using GeoHub.Logic;
using GeoHub.Services.Controllers;
using GeoHub.Services.Data;

namespace GeoHub.Services
{
    public class LinkBuilder : ILinkBuilder
    {
        protected ControllerBase Controller { get; set; }
        public LinkBuilder(ControllerBase controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            Controller = controller;
        }
        public IEnumerable<Link> BuildLinks(GeoDataEntry entry)
        {

            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            return new string[] {"Airports", "Hospitals", "Suggestions"}.Select(r => new Link()
            {
                //if building link for controller that is calling this, include q
                Href =
                    string.Format("{0}?longitude={1}&latitude={2}",
                        Controller.Url.Link("DefaultApi", new {Controller = r}), entry.Longitude, entry.Latitude),
                Method = "GET",
                Rel = r
            });

        }

    }
}
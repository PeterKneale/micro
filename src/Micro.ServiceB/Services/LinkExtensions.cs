using System.Collections.Generic;
using Micro.ServiceB.Models;
using Microsoft.AspNetCore.Mvc;

namespace Micro.ServiceB.Controllers
{
    public static class LinkExtensions
    {
        public static void AddLinks(this TableSummaryModel model, IUrlHelper urls)
        {
            model.Links = new List<LinkModel>
            {
                new LinkModel(urls.Link(nameof(TablesController.GetTableAsync), new { database = model.Database, table = model.Name }), "self", "GET")
            };
        }

        public static void AddLinks(this TableDetailsModel model, IUrlHelper urls)
        {
            model.Links = new List<LinkModel>
            {
                new LinkModel(urls.Link(nameof(TablesController.ListTables), new { database = model.Database }), "siblings", "GET"),
                new LinkModel(urls.Link(nameof(DatabasesController.GetDatabaseAsync), new { database = model.Database }), "database", "GET")
            };
        }
    }
}
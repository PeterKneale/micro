using System.Collections.Generic;

namespace Micro.ServiceB.Models
{
    public class TableSummaryModel
    {
        public string Database { get; set; }

        public string Schema { get; set; }

        public string Name { get; set; }

        public List<LinkModel> Links { get; set; } = new List<LinkModel>();
    }

    public class TableDetailsModel
    {
        public string Database { get; set; }

        public string Schema { get; set; }

        public string Name { get; set; }

        public int Columns { get; set; }

        public int Constraints { get; set; }

        public List<LinkModel> Links { get; set; } = new List<LinkModel>();
    }

    public class InfoSchemaTable
    {
        public string TABLE_CATALOG { get; set; }
        public string TABLE_SCHEMA { get; set; }
        public string TABLE_NAME { get; set; }
        public string TABLE_TYPE { get; set; }
    }

    public class LinkModel
    {
        public LinkModel(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }

        public string Href { get; }
        public string Rel { get; }
        public string Method { get; }
    }
}

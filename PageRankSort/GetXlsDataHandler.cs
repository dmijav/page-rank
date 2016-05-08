using PageRankSort.DTO;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Xml;

namespace PageRankSort
{
    public class GetXlsDataHandler
    {
        private string fileDir = "";
        private const string sheetName = "task data$";
        private const string oleProvider = "Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;";

        public List<PageRankDto> Get(string input)
        {
            fileDir = input;
            var data = LoadDataFromExcel();
            return DataMapping(data);
        }

        private string LoadDataFromExcel()
        {
            var connectionString = string.Format(oleProvider, fileDir);
            DataSet ds = new DataSet();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbDataAdapter sheetAdapter = new OleDbDataAdapter("SELECT * FROM [" + sheetName + "]", conn);
                sheetAdapter.Fill(ds);
            }

            return ds.GetXml();
        }

        private List<PageRankDto> DataMapping(string data)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
          //  var t = Dictionary<int, List<int>>
            string xpath = "descendant::Table";
            var nodes = xmlDoc.SelectNodes(xpath);

            var result = new List<PageRankDto>();
            int initPageNumer = 1;
            foreach (XmlNode node in nodes)
            {
                if (!node.HasChildNodes) continue;
                var td = node;
                result.Add(new PageRankDto
                {
                    PageNumber = initPageNumer,
                    Votes = GetNodePages(node, initPageNumer)
                });
                initPageNumer++;
            }
            return result;
        }

        private List<int> GetNodePages(XmlNode node, int initPageNumer)
        {
            int webPageNumer = 1;
            var result = new List<int>();
            foreach (XmlNode cNode in node.ChildNodes)
            {
                string innerText = cNode.InnerText;
                if (innerText != "1" && innerText != "0") continue;
                if (initPageNumer == webPageNumer) webPageNumer++;
                if (innerText == "1")
                {
                    result.Add(webPageNumer);
                }
                webPageNumer++;
            }
            return result;
        }
    }
}
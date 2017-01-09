using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IndeedSearchUrlGenerator
{
    class Program
    {
        static void Main()
        {
            var cityPattern = "https://tr.indeed.com/jobs?q=&l={0}";
            var positionPattern = "https://tr.indeed.com/jobs?q={0}&l=";
            var cityPositionPattern = "https://tr.indeed.com/jobs?q={0}&l={1}";

            var searchUrlList = new List<string>();
            searchUrlList.AddRange(SearchParameterData.CityList.Select(x => string.Format(cityPattern, x)).ToList());
            searchUrlList.AddRange(SearchParameterData.PositionList.Select(x => string.Format(positionPattern, x)).ToList());
            searchUrlList.AddRange(
                (from city in SearchParameterData.CityList
                 from position in SearchParameterData.PositionList
                 select string.Format(cityPositionPattern, position, city)).ToList());

            ToInsertQuery(searchUrlList);

        }

        private static void ToInsertQuery(List<string> searchUrlList)
        {
            var fileName = "IndeedSearchUrlInsert.sql";
            if (File.Exists(fileName)) { File.Delete(fileName); }

            using (var sw = new StreamWriter(fileName, true))
            {
                foreach (var url in searchUrlList)
                {
                    sw.WriteLine($"INSERT INTO IndeedSearchUrl VALUES('{url}');");
                }
            }
        }
    }
}
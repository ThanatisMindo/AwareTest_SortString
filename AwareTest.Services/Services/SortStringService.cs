using AwareTest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwareTest.Services.IService;
using System.Text.RegularExpressions;

namespace AwareTest.Services.Services
{
    public class SortStringService : ISortStringService
    {
        public SortStringService() { 
        }
        public List<SortStringModel> GetSortString(string rawString)
        {
            var result = new List<SortStringModel>();
            try
            {
                //validate
                if (rawString == string.Empty)
                {
                    throw new ArgumentException("param is empty");
                }
                string trimmedInput = rawString.Trim();

                // Regular expression pattern to allow only alphabets, numbers, and commas
                string pattern = "[^a-zA-Z0-9,]";

                // Use Regex.Replace to filter out characters not matching the pattern
                string filteredInput = Regex.Replace(trimmedInput, pattern, "");

                string[] stringSplit = filteredInput.Split(',').Distinct().ToArray(); //distinct string in arrays
                string[] filteredParts = stringSplit.Where(s => !string.IsNullOrEmpty(s)).ToArray(); //reduce empty string in arrays

                var res = filteredParts.OrderBy(c => !int.TryParse(c, out var temp) ? c :
                                         temp.ToString("D10"))
                                        .OrderBy(c => !char.IsDigit(c[0]) ? 0 : 1).ToList();

                foreach (string str in res)
                {
                    result.Add(new SortStringModel { Rank = str });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("error SortString : " + ex.Message);
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}

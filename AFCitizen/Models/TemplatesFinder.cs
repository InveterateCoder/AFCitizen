using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AFCitizen.Models
{
    public static class TemplatesFinder
    {
        public static string[] FindTemplates(string original, Dictionary<string, string> templates)
        {
            string[] words = original.Split(" ");
            List<Item> resultList = new List<Item>();
            string[] result = new string[templates.Count];
            foreach (var template in templates)
            {
                double similarity = 0;
                foreach (var word in words)
                    if (template.Value.Contains(GetRoot(word)))
                        similarity += 0.6;
                resultList.Add(new Item { similarity = similarity, id = template.Key });
            }
            for (int i = 0; i < result.Length; i++)
            {
                double keyMax = resultList.Max(sel => sel.similarity);
                result[i] = resultList.Where(l => l.similarity == keyMax).Select(x => x.id).FirstOrDefault();
                resultList.Remove(resultList.Where(l => l.similarity == keyMax).FirstOrDefault());
            }
            return result;
        }
        private static string GetRoot(string word)
        {
            return word.Substring(0, (int)Math.Round(word.Length * 0.7));
        }
        struct Item
        {
            public double similarity;
            public string id;
        }
    }
}

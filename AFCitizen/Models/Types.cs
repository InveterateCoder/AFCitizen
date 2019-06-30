using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AFCitizen.Models
{
    public static class DocType
    {
        public static string Заявление => "Заявление";
        public static string Жалоба => "Жалоба";
        public static string Предложение => "Предложение";
        public static IEnumerable<string> Names() => typeof(DocType).GetProperties().Select(i => (string)i.GetValue(null));
    }
    public class Document
    {
        [Required]
        public string From { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Text { get; set; }
    }

    public class Redirect
    {
        public string To { get; set; }
        public string Comment { get; set; }
    }

    public class Accept
    {
        public string To { get; set; }
        public string AgentFullName { get; set; }
        public string Position { get; set; }
    }

    public class Reply
    {
        public string To { get; set; }
        public string From { get; set; }
        public string AgentFullName { get; set; }
        public string Position { get; set; }
        public string Body { get; set; }
        public string Comment { get; set; }
    }
    public static class AuthorityType
    {
        public static string Прокуратура => "Прокуратура";
        public static string АрбСуд => "Арбитражный Суд";
        public static IEnumerable<string> Names() => typeof(AuthorityType).GetProperties().Select(i => (string)i.GetValue(null));
    }
    public class Authority
    {
        public static Dictionary<string, Dictionary<string, Authority[]>> Cities = new Dictionary<string, Dictionary<string, Authority[]>>
        {
            ["Ессентуки"] = new Dictionary<string, Authority[]>
            {
                [AuthorityType.АрбСуд] = new Authority[4]
                {
                    new Authority
                    {
                        Name = "Арбитражный суд Ставропольского края",
                        Level = 1
                    },
                    new Authority
                    {
                        Name = "Шестнадцатый арбитражный апелляционный суд",
                        Level = 2
                    },
                    new Authority
                    {
                        Name = "Арбитражный суд Северо-Кавказского округ",
                        Level = 2
                    },
                    new Authority
                    {
                        Name = "Высший Арбитражный Суд РФ",
                        Level = 3
                    }
                },
                [AuthorityType.Прокуратура] = new Authority[3]
                {
                    new Authority
                    {
                        Name = "Прокуратура города Ессентуки",
                        Level = 1
                    },
                    new Authority
                    {
                        Name = "Прокуратура Ставропольского края",
                        Level = 2
                    },
                    new Authority
                    {
                        Name = "Генеральная Прокуратура РФ",
                        Level = 3
                    }
                }
            },
            ["Ставрополь"] = new Dictionary<string, Authority[]>
            {
                [AuthorityType.АрбСуд] = new Authority[4]
                {
                    new Authority
                    {
                        Name = "Арбитражный суд Ставропольского края",
                        Level = 1
                    },
                    new Authority
                    {
                        Name = "Шестнадцатый арбитражный апелляционный суд",
                        Level = 2
                    },
                    new Authority
                    {
                        Name = "Арбитражный суд Северо-Кавказского округ",
                        Level = 2
                    },
                    new Authority
                    {
                        Name = "Высший Арбитражный Суд РФ",
                        Level = 3
                    }
                },
                [AuthorityType.Прокуратура] = new Authority[3]
                {
                    new Authority
                    {
                        Name = "Прокуратура города Ставрополь",
                        Level = 1
                    },
                    new Authority
                    {
                        Name = "Прокуратура Ставропольского края",
                        Level = 2
                    },
                    new Authority
                    {
                        Name = "Генеральная Прокуратура РФ",
                        Level = 3
                    }
                }
            },
            ["Москва"] = new Dictionary<string, Authority[]>
            {
                [AuthorityType.АрбСуд] = new Authority[4]
                {
                    new Authority
                    {
                        Name = "Арбитражный суд города Москвы",
                        Level = 1
                    },
                    new Authority
                    {
                        Name = "Девятый арбитражный апелляционный суд",
                        Level = 2
                    },
                    new Authority
                    {
                        Name = "Арбитражный суд Московского округа",
                        Level = 2
                    },
                    new Authority
                    {
                        Name = "Высший Арбитражный Суд РФ",
                        Level = 3
                    }
                },
                [AuthorityType.Прокуратура] = new Authority[3]
                {
                    new Authority
                    {
                        Name = "Прокуратура города Москвы",
                        Level = 1
                    },
                    new Authority
                    {
                        Name = "Прокуратура Московской области",
                        Level = 2
                    },
                    new Authority
                    {
                        Name = "Генеральная Прокуратура РФ",
                        Level = 3
                    }
                }
            }
        };
        public string Name { get; set; }
        public ushort Level { get; set; }
    }
    
}
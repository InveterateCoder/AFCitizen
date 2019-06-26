using System.Collections.Generic;

namespace AFCitizen.Models
{
    public static class DocType
    {
        public static string Заявление = "Заявление";
        public static string Жалоба = "Жалоба";
        public static string Предложение = "Предложение";
    }

    public static class AuthorityType
    {
        public static string МВД = "МВД";
        public static string Прокуратура = "Прокуратура";
        public static string АрбСуд = "Арбитражный Суд";
    }
    public class Body
    {
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }

    public class Redirect
    {
        public string To { get; set; }
        public string Comment { get; set; }
    }

    public class Appeal
    {
        public Body Body { get; set; }
        public Close Close { get; set; }
    }

    public class Assign
    {
        public string To { get; set; }
        public string AgentLastName { get; set; }
        public string AgentName { get; set; }
        public string Position { get; set; }
    }

    public class Close
    {
        public string Reply { get; set; }
    }

    public class Authority
    {
        public string Name { get; set; }
        public int HierarchyLevel { get; set; }
    }
    /*public static class Federation
    {
        public static Dictionary<string, Subject> Subjects = new Dictionary<string, Subject>
        {
            ["Ставропольский Край"] = new Subject
            {
                Authorities = new Dictionary<string, string>
                {
                    [AuthorityType.МВД] = "МВД по Став. краю",
                    [AuthorityType.Прокуратура] = "Прокуратура Ст. края",
                    [AuthorityType.АрбСуд] = "Арбитражный Суд Ст. края"
                },
                Cities = new Dictionary<string, Dictionary<string, string>>
                {
                    ["Ессентуки"] = new Dictionary<string, string>
                    {
                        [AuthorityType.МВД] = "",
                        [AuthorityType.Прокуратура] = "",
                        [AuthorityType.АрбСуд] = ""
                    },
                    ["Ставрополь"] = new Dictionary<string, string>
                    {
                        [AuthorityType.МВД] = "",
                        [AuthorityType.Прокуратура] = "",
                        [AuthorityType.АрбСуд] = ""
                    }
                }
            },
            ["Московская область"] = new Subject
            {
                Authorities = new Dictionary<string, string>
                {
                    [AuthorityType.МВД] = "МВД по Мос. области",
                    [AuthorityType.Прокуратура] = "Прокуратура Мос. области",
                    [AuthorityType.АрбСуд] = "Арбитражный Суд Мос. области"
                },
                Cities = new Dictionary<string, Dictionary<string, string>>
                {
                    ["Балашиха"] = new Dictionary<string, string>
                    {
                        [AuthorityType.МВД] = "",
                        [AuthorityType.Прокуратура] = "",
                        [AuthorityType.АрбСуд] = ""
                    },
                    ["Москва"] = new Dictionary<string, string>
                    {
                        [AuthorityType.МВД] = "",
                        [AuthorityType.Прокуратура] = "",
                        [AuthorityType.АрбСуд] = ""
                    }
                }
            }
        }
    }

    public class Subject
    {
        public Dictionary<string, string> Authorities;
        public Dictionary<string, Dictionary<string, string>> Cities;
    }*/
}
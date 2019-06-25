
namespace AFCitizen.Models
{
    public static class DocType
    {
        public static string Заявление = "Заявление";
        public static string Жалоба = "Жалоба";
        public static string Предложение = "Предложение";
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

}

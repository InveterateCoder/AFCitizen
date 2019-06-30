using System;
using System.Security.Cryptography;
using System.Text;

namespace AFCitizen.Models
{
    public enum BlockType { Open, Close, Redirect, Accept }
    public class Block
    {
        public string Id { get; set; }
        public string DocId { get; set; }
        public string TimeStamp { get; set; }
        public bool isClosed { get; set; }
        public BlockType Type { get; set; }
        public string AuthorityType { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Document { get; set; } // JSON for Document
        public string Replies { get; set; } // JSON for Reply[]
        public string TypeMessage { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public static string ComputeHash(Block block)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{block.DocId}-{block.TimeStamp}-{block.isClosed}-{block.Type}-{block.From}-{block.To}-{block.Document}-{block.Replies}-{block.PreviousHash}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }
        public void Lock()
        {
            TimeStamp = DateTime.Now.ToString();
            Hash = ComputeHash(this);
        }
    }
}

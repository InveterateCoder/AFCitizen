using System;
using System.Security.Cryptography;
using System.Text;

namespace AFCitizen.Models
{
    public enum BlockType { Open, Close, Redirect, Assign, Confirm }
    public class Block
    {
        public string Id { get; set; }
        public Guid DocId { get; } = Guid.NewGuid();
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public BlockType Type { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public ushort AuthorityLevel { get; set; }
        public string Document { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public string ComputeHash()
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{DocId}-{TimeStamp}-{Type}-{From}-{To}-{AuthorityLevel}-{Document}-{PreviousHash}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }
    }
}

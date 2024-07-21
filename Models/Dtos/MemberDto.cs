using System.ComponentModel.DataAnnotations;

namespace Smart_Librarian.Models.Dtos
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime JoinDate { get; set; }
    }
    public class AddMemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime JoinDate { get; set; }
    }
    public class EditMemberDto
    {
        public string Name { get; set; }
        public DateTime JoinDate { get; set; }
    }
}

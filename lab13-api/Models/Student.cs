namespace lab13_api.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public Grade Grade { get; set; }
        public int GradeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

    }
}

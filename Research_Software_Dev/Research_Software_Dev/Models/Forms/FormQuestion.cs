namespace Research_Software_Dev.Models.Forms
{
    public class FormQuestion
    {
        
        public int QuestionId { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionDescription { get; set; }

        //Form FK
        public int FormId { get; set; }
        public Form Form { get; set; }
    }
}

namespace Application.Common.DTOs
{
    public class EmailDto
    {
        public string SenderEmail { get; set;}
        public string RecipientEmail { get; set;}
        public string Subject {get; set;}
        public string Body {get; set;}
    }
}
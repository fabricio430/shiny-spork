namespace DevFreela.API.Models.Projects
{
    public class CreateProjectCommentInputModel
    {
        public string Content { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
    }
}

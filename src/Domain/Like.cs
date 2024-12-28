namespace Domain;

public class Like
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public int LinkId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation Property
    public Link Link { get; set; }
}

namespace TodoList.Dtos;

public class TodoResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
}

namespace TodoList.Dtos;

public class CreateTodoDto
{
  public string Title { get; set; } = string.Empty;
  public int UserId { get;set; }
}
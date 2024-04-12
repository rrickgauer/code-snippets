public class ExampleLayoutModel
{
    public string Title { get; }

    public ExampleLayoutModel(string title)
    {
        Title = title;
    }
}

public class ExampleLayoutModel<T> : ExampleLayoutModel
{
    public required T PageModel { get; set; }
    
    public ExampleLayoutModel(string title) : base(title) 
    {
        
    }
}

public class ExamplePageModel
{
    public required int UserId { get; set; }
}
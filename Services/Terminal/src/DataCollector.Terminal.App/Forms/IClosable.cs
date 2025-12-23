namespace DataCollector.Terminal.App.Forms;

public interface IClosable
{
    Func<Task>? CloseAsyncCallback { get; set; }
}

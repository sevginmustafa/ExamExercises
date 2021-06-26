namespace SIS.MvcFramework
{
    public interface IView
    {
        string ExecuteTemplate(object model, string user);
    }
}

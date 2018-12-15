namespace Model.Runtime.Interfaces
{
    public interface IViewModel<out T>
    {
        T GetDataModel();
    }
}

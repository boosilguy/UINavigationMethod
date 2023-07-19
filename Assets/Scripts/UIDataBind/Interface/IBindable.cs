namespace uidatabind
{
    public interface IBindable
    {
        string Key { get; }
        void Bind(DataContext context);
    }
}

namespace Rose.VExtension.VK
{
    public interface IVKRequestHandler
    {
        IVKResponse GetResponse(IVKRequest request);
    }
}

namespace AnnonceManager.Repositories
{
    public interface ISendMail
    {
        void Send(string to, string subject, string body);
    }
}

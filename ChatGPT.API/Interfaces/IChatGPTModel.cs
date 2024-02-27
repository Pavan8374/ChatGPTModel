namespace ChatGPT.API.Interfaces
{
    public interface IChatGPTModel
    {
        Task<string> GetResponse(string inputText);
    }
}

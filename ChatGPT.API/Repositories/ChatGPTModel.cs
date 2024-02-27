using ChatGPT.API.Interfaces;
using ChatGPT.API.Models;
using Microsoft.ML;

namespace ChatGPT.API.Repositories
{
    public class ChatGPTModel : IChatGPTModel
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _transformer;
        private readonly List<string> _vocabulary;

        public ChatGPTModel(string modelPath, List<string> vocabulary)
        {
            _mlContext = new MLContext();
            _transformer = _mlContext.Model.Load(modelPath, out var schema);
            _vocabulary = vocabulary;
        }

        public async Task<string> GetResponse(string inputText)
        {
            var inputIds = TokenizeInput(inputText);
            var inputData = new InputData { InputIds = inputIds };
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<InputData, OutputData>(_transformer);
            var outputData = predictionEngine.Predict(inputData);
            var outputIds = outputData.OutputIds;
            var outputText = ConvertOutputIdsToText(outputIds);
            return outputText;
        }

        private int[] TokenizeInput(string inputText)
        {
            var tokens = inputText.Split(' ').Select(token => token.ToLower()).ToList();
            var inputIds = tokens.Select(token =>
            {
                if (_vocabulary.Contains(token))
                    return _vocabulary.IndexOf(token);
                else
                    return _vocabulary.IndexOf("<unk>"); // Handle unknown tokens
            }).ToArray();

            // Padding to fixed length (e.g., 256)
            if (inputIds.Length < 256)
            {
                var paddedInputIds = new int[256];
                inputIds.CopyTo(paddedInputIds, 0);
                return paddedInputIds;
            }
            else
            {
                return inputIds.Take(256).ToArray();
            }
        }

        private string ConvertOutputIdsToText(int[] outputIds)
        {
            var tokens = outputIds.Select(id =>
            {
                if (id >= 0 && id < _vocabulary.Count)
                    return _vocabulary[id];
                else
                    return "<unk>"; // Handle unknown tokens
            });
            return string.Join(" ", tokens);
        }
    }
}

using Microsoft.ML;
using Microsoft.ML.Data;

namespace ChatGPT.API.Models
{
    public class InputData
    {
        [VectorType(256)]
        public int[] InputIds { get; set; }
    }

    public class OutputData
    {
        [VectorType(256)]
        public int[] OutputIds { get; set; }
    }
}

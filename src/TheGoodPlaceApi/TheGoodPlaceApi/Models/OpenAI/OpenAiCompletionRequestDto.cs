namespace TheGoodPlaceApi.Models.OpenAI
{    public class OpenAiCompletionRequestDto
    {
        public string id { get; set; }
        public string @object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public List<PromptFilterResult> prompt_filter_results { get; set; }
        public List<Choice> choices { get; set; }
        public Usage usage { get; set; }
        public string system_fingerprint { get; set; }
    }

    public class Choice
    {
        public int index { get; set; }
        public string finish_reason { get; set; }
        public Message message { get; set; }
        public ContentFilterResult content_filter_result { get; set; }
        public ContentFilterResults content_filter_results { get; set; }
    }

    public class ContentFilterResult
    {
        public Error error { get; set; }
    }

    public class ContentFilterResults
    {
    }

    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class FunctionCall
    {
        public string name { get; set; }
        public string arguments { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public FunctionCall function_call { get; set; }
    }

    public class PromptFilterResult
    {
        public int prompt_index { get; set; }
        public ContentFilterResults content_filter_results { get; set; }
    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }


}

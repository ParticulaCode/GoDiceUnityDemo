namespace GoDice.Shared.Logging.Formatters
{
    public class TagFormatter : ILogFormatter
    {
        private readonly string _tag;
        
        public TagFormatter(string tag) => _tag = tag;

        public string Format(string msg) => $"{_tag} {msg}";
    }
}
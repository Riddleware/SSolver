using CommandLine;

namespace SSolver
{
    public class CommandLineOptions
    {
        // [Value(index: 0, Required = true, HelpText = "Image file Path to analyze.")]
        // public string Path { get; set; }

        [Option(shortName: 'd', longName: "delay", Required = false, HelpText = "Delay", Default = 0)]
        public int Delay { get; set; }
    }
}
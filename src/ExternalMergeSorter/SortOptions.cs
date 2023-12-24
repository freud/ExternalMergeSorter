using System.Text;

namespace Sorter;

internal class SortOptions
{
    public int InputBufferSize { get; }
    public int OutputBufferSize { get; }
    public string SourceDataFilePath { get; }
    public string SortedOutputFilePath { get; }
    public string WorkingDirectory { get; }
    public int MaximumMemoryConsumption { get; }
    public IComparer<string> Comparer { get; }
    public Encoding FilesEncoding  { get; }

    public SortOptions(
        string sourceDataFilePath, int maximumMemoryConsumption,
        IComparer<string> comparer, Encoding filesEncoding,
        int inputBufferSize, int outputBufferSize)
    {
        if (File.Exists(sourceDataFilePath) == false)
        {
            throw new FileNotFoundException(
                $"Data file {sourceDataFilePath} cannot be found",
                nameof(sourceDataFilePath)
                );
        }
        if (maximumMemoryConsumption == 0)
        {
            throw new ArgumentException("Must be greater than 0", nameof(maximumMemoryConsumption));
        }
        if (inputBufferSize == 0)
        {
            throw new ArgumentException("Must be greater than 0", nameof(inputBufferSize));
        }
        if (outputBufferSize == 0)
        {
            throw new ArgumentException("Must be greater than 0", nameof(outputBufferSize));
        }

        SourceDataFilePath = sourceDataFilePath;
        MaximumMemoryConsumption = maximumMemoryConsumption;
        Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        FilesEncoding = filesEncoding;
        InputBufferSize = inputBufferSize;
        OutputBufferSize = outputBufferSize;
        WorkingDirectory = Path.GetDirectoryName(SourceDataFilePath)!;
        SortedOutputFilePath = Path.Combine(
            Path.GetDirectoryName(SourceDataFilePath)!,
            $"{Path.GetFileNameWithoutExtension(SourceDataFilePath)}.out.txt");
    }

    public string GetChunkFilePath(int chunkIndex)
    {
        var prefix = Path.GetFileNameWithoutExtension(SourceDataFilePath);
        var chunkFilename = $"{prefix}.chunk{chunkIndex}.txt";
        return Path.Combine(WorkingDirectory, chunkFilename);
    }
}
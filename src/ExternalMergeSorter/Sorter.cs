namespace Sorter;

internal class Sorter
{
    private const int EstimatedLineLength = 15;
    private readonly int _singleChunkBufferSize;

    private readonly SortOptions _options;

    public Sorter(SortOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _singleChunkBufferSize = options.MaximumMemoryConsumption / EstimatedLineLength;
    }

    public async Task Sort()
    {
        var numberOfChunks = await SortAndSplitIntoChunks();

        var maxPartSize = _options.MaximumMemoryConsumption / numberOfChunks;

        var readers = Enumerable
            .Range(0, numberOfChunks)
            .Select(chunkIndex => new ChunkReader(chunkIndex, maxPartSize, _options))
            .ToList();

        await using var sw = new StreamWriter(
            _options.SortedOutputFilePath, false,
            _options.FilesEncoding, _options.OutputBufferSize);

        while (true)
        {
            var nextReader = readers
                .Select(reader => new
                {
                    Reader = reader,
                    Line = reader.ReadCurrentLine()
                })
                .Where(d => d.Line != null)
                .MinBy(task => task.Line!, new StringAndNumberLinesComparer());

            if (nextReader?.Line == null)
            {
                break;
            }

            await sw.WriteLineAsync(nextReader.Line!);
            nextReader.Reader.MoveNext();
        }
    }

    private async Task<int> SortAndSplitIntoChunks()
    {
        var singleChunkOutputBuffer = new List<string>(_singleChunkBufferSize);

        using var sr = new StreamReader(_options.SourceDataFilePath,
            _options.FilesEncoding, true, _options.InputBufferSize);

        var chunkIndex = 0;
        long chunkSize = 0;
        string? line;
        while ((line = await sr.ReadLineAsync()) != null)
        {
            chunkSize += _options.FilesEncoding.GetByteCount(line) + Environment.NewLine.Length;
            singleChunkOutputBuffer.Add(line);

            if (chunkSize >= _options.MaximumMemoryConsumption)
            {
                singleChunkOutputBuffer.Sort(new StringAndNumberLinesComparer());
                await SaveChunk(singleChunkOutputBuffer, chunkIndex);
                chunkIndex++;
                singleChunkOutputBuffer.Clear();
                chunkSize = 0;
            }
        }

        singleChunkOutputBuffer.Sort(new StringAndNumberLinesComparer());
        await SaveChunk(singleChunkOutputBuffer, chunkIndex);
        chunkIndex++;

        return chunkIndex;
    }

    private async Task SaveChunk(List<string> chunkBuffer, int chunkIndex)
    {
        var sortedChunkFilePath = _options.GetChunkFilePath(chunkIndex);

        await using var sw = new StreamWriter(sortedChunkFilePath, false, 
            _options.FilesEncoding, _options.OutputBufferSize);

        foreach (var bufferLine in chunkBuffer)
        {
            await sw.WriteLineAsync(bufferLine);
        }
    }
}
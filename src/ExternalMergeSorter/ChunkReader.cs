namespace Sorter;

internal sealed class ChunkReader
{
    private readonly int _maxPartSize;
    private readonly SortOptions _options;
    private List<string>? _singleChunkOutputBuffer;
    private int _lineNumber;
    private long _lastFilePosition;
    private readonly string _chunkFilePath;

    public ChunkReader(int chunkIndex, int maxPartSize, SortOptions options)
    {
        _maxPartSize = maxPartSize;
        _options = options;
        _singleChunkOutputBuffer = null;
        _lineNumber = 0;
        _chunkFilePath = _options.GetChunkFilePath(chunkIndex);
    }

    public string? ReadCurrentLine()
    {
        var isBufferToInit =
            _singleChunkOutputBuffer == null ||
            _lineNumber >= _singleChunkOutputBuffer.Count;

        if (isBufferToInit)
        {
            _singleChunkOutputBuffer = InitNextBuffer();
            if (_singleChunkOutputBuffer == null)
            {
                return null;
            }

            _lineNumber = 0;
        }

        return _singleChunkOutputBuffer![_lineNumber];
    }

    public void MoveNext()
    {
        _lineNumber++;
    }

    private List<string>? InitNextBuffer()
    {
        using var sr = new StreamReader(_chunkFilePath, _options.FilesEncoding, true, new FileStreamOptions
        {
            BufferSize = _options.InputBufferSize,
            Options = FileOptions.SequentialScan
        });

        if (_lastFilePosition >= sr.BaseStream.Length)
        {
            return null;
        }

        var newBuffer = new List<string>();
        sr.BaseStream.Seek(_lastFilePosition, SeekOrigin.Begin);
        long partSize = 0;
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            var lineSize = 
                _options.FilesEncoding.GetByteCount(line) +
                _options.FilesEncoding.GetByteCount(Environment.NewLine);

            partSize += lineSize;

            _lastFilePosition += lineSize;

            if (string.IsNullOrEmpty(line) == false)
            {
                newBuffer.Add(line);
            }

            if (partSize >= _maxPartSize)
            {
                break;
            }
        }

        return newBuffer;
    }
}
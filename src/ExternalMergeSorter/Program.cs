using System.Diagnostics;
using System.Text;
using Sorter;

var executionTimeWatcher = new Stopwatch();
executionTimeWatcher.Start();

var sorter = new Sorter.Sorter(
    new SortOptions(
        sourceDataFilePath: @"d:\external_sort\test.txt",
        comparer: new StringAndNumberLinesComparer(),
        maximumMemoryConsumption: 100 * MemoryUnits.MB,
        filesEncoding: new UTF8Encoding(false),
        inputBufferSize: 10 * MemoryUnits.MB,
        outputBufferSize: 10 * MemoryUnits.MB
    )
);

await sorter.Sort();

executionTimeWatcher.Stop();
Console.WriteLine(executionTimeWatcher.Elapsed.TotalSeconds);
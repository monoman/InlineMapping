using System;

namespace InlineMapping.PerformanceTests
{
  [MapTo(typeof(Destination))]
  public class Source
  {
    public uint Age { get; set; }
    public byte[]? Buffer { get; set; }
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime When { get; set; }
  }
}
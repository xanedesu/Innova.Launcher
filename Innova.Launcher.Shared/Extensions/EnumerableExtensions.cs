// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Extensions.EnumerableExtensions
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Innova.Launcher.Shared.Extensions
{
  public static class EnumerableExtensions
  {
    public static async Task ParallelAsync<T>(
      this IEnumerable<T> source,
      int degreeOfParallelism,
      Func<T, Task> action)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      await Task.WhenAll(Partitioner.Create<T>(source).GetPartitions(degreeOfParallelism).Select<IEnumerator<T>, Task>((Func<IEnumerator<T>, Task>) (partition => Task.Run((Func<Task>) (() =>
      {
        using (partition)
        {
          while (partition.MoveNext())
            await action(partition.Current).ConfigureAwait(false);
        }
      })))));
    }
  }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Machine.Core.Utility
{
  public class ReaderWriterLockStatistics
  {
    public static ReaderWriterLockStatistics Singleton = new ReaderWriterLockStatistics();

    private readonly ReaderWriterLock _lock = new ReaderWriterLock();
    private readonly List<ReaderWriterUsage> _usages = new List<ReaderWriterUsage>();

    public void AddUsage(ReaderWriterUsage usage)
    {
      using (RWLock.AsWriter(_lock))
      {
        _usages.Add(usage);
      }
    }

    public IDictionary<IReaderWriterLock, List<ReaderWriterUsage>> GroupByLock()
    {
      using (RWLock.AsReader(_lock))
      {
        Dictionary<IReaderWriterLock, List<ReaderWriterUsage>> byLock = new Dictionary<IReaderWriterLock, List<ReaderWriterUsage>>();
        foreach (ReaderWriterUsage usage in _usages)
        {
          if (!byLock.ContainsKey(usage.Lock))
          {
            byLock[usage.Lock] = new List<ReaderWriterUsage>();
          }
          byLock[usage.Lock].Add(usage);
        }
        return byLock;
      }
    }

    public Report CreateReport()
    {
      Report report = new Report();
      foreach (KeyValuePair<IReaderWriterLock, List<ReaderWriterUsage>> entry in GroupByLock())
      {
        CreateReportRow(report, entry.Key, entry.Value);
      }
      return report;
    }

    private static void CreateReportRow(Report report, IReaderWriterLock  lok, IEnumerable<ReaderWriterUsage> usages)
    {
      long numberOfAcquires = 0;
      long numberOfReads = 0;
      long numberOfWrites = 0;
      long numberOfUpgrades = 0;
      long totalTicksInLock = 0;
      long totalTicksAsReader = 0;
      long totalTicksAsWriter = 0;
      long totalTicksWaiting = 0;
      long totalTicksWaitingToRead = 0;
      long totalTicksWaitingToWrite = 0;
      foreach (ReaderWriterUsage usage in usages)
      {
        if (usage.WasUpgraded)
        {
          numberOfUpgrades++;
        }
        if (usage.InitiallyAReader || usage.WasUpgraded)
        {
          numberOfReads++;
        }
        else
        {
          numberOfWrites++;
        }
        numberOfAcquires++;
        totalTicksAsWriter += usage.TimeAsWriter;
        totalTicksAsReader += usage.TimeAsReader;
        totalTicksWaiting += usage.TimeWaitingToAcqure;
        totalTicksWaitingToRead += usage.TimeWaitingToRead;
        totalTicksWaitingToWrite += usage.TimeWaitingToWrite;
        totalTicksInLock += usage.TimeSpentInLock;
      }

      double frequency = (double)Stopwatch.Frequency / 1000.0;

      Report.Row row = report.AddRow();
      row.AddColumn("Acquires", numberOfAcquires);
      row.AddColumn("Reads", numberOfReads);
      row.AddColumn("Writes", numberOfWrites);
      row.AddColumn("Upgrades", numberOfUpgrades);
      row.AddColumn("WaitingToRead", totalTicksWaitingToRead / frequency);
      row.AddColumn("WaitingToWrite", totalTicksWaitingToWrite / frequency);
      row.AddColumn("Waiting", totalTicksWaiting / frequency);
      row.AddColumn("Lock", lok.Name);
    }

    public class Report
    {
      public class Row
      {
        private readonly List<ColumnValue> _values = new List<ColumnValue>();
        private readonly Report _report;

        public Row(Report report)
        {
          _report = report;
        }

        public Row AddColumn(string name, double value)
        {
          Column column = _report.AddColumn(name);
          _values.Add(new ColumnDoubleValue() { Column = column, Value = value });
          return this;
        }

        public Row AddColumn(string name, string value)
        {
          Column column = _report.AddColumn(name);
          _values.Add(new ColumnStringValue() { Column = column, Value = value });
          return this;
        }

        public IEnumerable EnumerateValuesInColumnOrder(IEnumerable<Column> columns)
        {
          foreach (Column column in columns)
          {
            foreach (ColumnValue value in _values)
            {
              if (value.Column == column)
              {
                yield return value;
              }
            }
          }
        }
      }

      public class Column
      {
        public string Name;
      }

      public abstract class ColumnValue
      {
        public Column Column;
      }

      public class ColumnStringValue : ColumnValue
      {
        public string Value;

        public override string ToString()
        {
          return this.Value;
        }
      }

      public class ColumnDoubleValue : ColumnValue
      {
        public double Value;

        public override string ToString()
        {
          return this.Value.ToString("###.####");
        }
      }

      private readonly List<Column> _columns = new List<Column>();
      private readonly List<Row> _rows = new List<Row>();

      public Row AddRow()
      {
        Row row = new Row(this);
        _rows.Add(row);
        return row;
      }

      public Column AddColumn(string name)
      {
        foreach (Column column in _columns)
        {
          if (column.Name == name)
          {
            return column;
          }
        }
        Column newColumn = new Column() { Name = name };
        _columns.Add(newColumn);
        return newColumn;
      }

      public string ToAscii()
      {
        StringBuilder sb = new StringBuilder();
        foreach (Column column in _columns)
        {
          sb.AppendFormat(@"{0,20}", column.Name);
        }
        sb.AppendLine();
        foreach (Row row in _rows)
        {
          foreach (ColumnValue value in row.EnumerateValuesInColumnOrder(_columns))
          {
            sb.AppendFormat(@"{0,20}", value.ToString());
          }
          sb.AppendLine();
        }
        return sb.ToString();
      }
    }
  }
}
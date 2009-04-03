using System.Collections.Generic;
using System.Threading;

using Machine.Core.Utility;

namespace Machine.Container.Model
{
  public class ServiceEntryLockBroker
  {
    public static readonly ServiceEntryLockBroker Singleton = new ServiceEntryLockBroker();

    #if DEBUGGING_LOCKS
    private readonly IReaderWriterLock _lock = ReaderWriterLockFactory.CreateLock("ServiceEntryLockBroker");
    #else
    private readonly ReaderWriterLock _lock = new ReaderWriterLock();
    #endif
    private readonly Dictionary<ServiceEntry, ServiceEntryLock> _map = new Dictionary<ServiceEntry, ServiceEntryLock>();

    public ServiceEntryLock GetLockForEntry(ServiceEntry entry)
    {
      using (RWLock.AsReader(_lock))
      {
        if (!_map.ContainsKey(entry))
        {
          _lock.UpgradeToWriterLock(Timeout.Infinite);
          if (!_map.ContainsKey(entry))
          {
            _map[entry] = new ServiceEntryLock(entry.ToString());
          }
        }
        return _map[entry];
      }
    }
  }
}
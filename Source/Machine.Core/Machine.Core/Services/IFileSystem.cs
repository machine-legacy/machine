using System;
using System.Collections.Generic;
using System.IO;

namespace Machine.Core.Services
{
  public interface IFileSystem
  {
    string[] GetDirectories(string path);
    string[] GetFiles(string path);
    string[] GetFiles(string path, string pattern);
    string[] GetFilesRecursively(string path);
    string[] GetFilesRecursively(string path, string pattern);
    string[] GetEntries(string path);
    bool IsFile(string path);
    bool IsDirectory(string path);
    FileProperties GetFileProperties(string path);
    Stream OpenFile(string path);
    StreamReader OpenText(string path);
    Stream CreateFile(string path);
    StreamWriter CreateText(string path);
    void CopyFile(string source, string destination, bool overwrite);
    TemporaryDirectoryHandle CreateTemporaryDirectory();
    TemporaryDirectoryHandle CreateTemporaryDirectory(string path);
    void CreateDirectory(string path);
    void RemoveDirectory(string path);
    string GetTempFileName();
    void MoveFile(string source, string destination);
  }

  public class FileProperties
  {
    string _fullName;
    long _length;
    DateTime _lastAccessTime;
    DateTime _lastWriteTime;
    DateTime _creationTime;
#if !SILVERLIGHT
    private DateTime _lastWriteTimeUtc;
    private DateTime _lastAccessTimeUtc;
    private DateTime _creationTimeUtc;
#endif

    public string FullName
    {
      get { return _fullName; }
      set { _fullName = value; }
    }

    public long Length
    {
      get { return _length; }
      set { _length = value; }
    }

    public DateTime LastAccessTime
    {
      get { return _lastAccessTime; }
      set { _lastAccessTime = value; }
    }

    public DateTime LastWriteTime
    {
      get { return _lastWriteTime; }
      set { _lastWriteTime = value; }
    }

    public DateTime CreationTime
    {
      get { return _creationTime; }
      set { _creationTime = value; }
    }

#if !SILVERLIGHT
    public DateTime LastAccessTimeUtc
    {
      get { return _lastAccessTimeUtc; }
      set { _lastAccessTimeUtc = value; }
    }

    public DateTime LastWriteTimeUtc
    {
      get { return _lastWriteTimeUtc; }
      set { _lastWriteTimeUtc = value; }
    }

    public DateTime CreationTimeUtc
    {
      get { return _creationTimeUtc; }
      set { _creationTimeUtc = value; }
    }
#endif

    public FileProperties(FileInfo fileInfo)
    {
      _fullName = fileInfo.FullName;
      _length = fileInfo.Length;
      _lastAccessTime = fileInfo.LastAccessTime;
      _creationTime = fileInfo.CreationTime;
      _lastWriteTime = fileInfo.LastWriteTime;
#if !SILVERLIGHT
      _lastAccessTimeUtc = fileInfo.LastAccessTimeUtc;
      _lastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
      _creationTimeUtc = fileInfo.CreationTimeUtc;
#endif
    }

    public FileProperties()
    {
    }
  }
}
// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnectionPool
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Threading;

namespace System.Data.SQLite
{
  /// <summary>
  /// This default method implementations in this class should not be used by
  /// applications that make use of COM (either directly or indirectly) due
  /// to possible deadlocks that can occur during finalization of some COM
  /// objects.
  /// </summary>
  internal static class SQLiteConnectionPool
  {
    /// <summary>
    /// This field is used to synchronize access to the private static data
    /// in this class.
    /// </summary>
    private static readonly object _syncRoot = new object();
    /// <summary>
    /// When this field is non-null, it will be used to provide the
    /// implementation of all the connection pool methods; otherwise,
    /// the default method implementations will be used.
    /// </summary>
    private static ISQLiteConnectionPool _connectionPool = (ISQLiteConnectionPool) null;
    /// <summary>
    /// The dictionary of connection pools, based on the normalized file
    /// name of the SQLite database.
    /// </summary>
    private static SortedList<string, SQLiteConnectionPool.PoolQueue> _queueList = new SortedList<string, SQLiteConnectionPool.PoolQueue>((IComparer<string>) StringComparer.OrdinalIgnoreCase);
    /// <summary>The default version number new pools will get.</summary>
    private static int _poolVersion = 1;
    /// <summary>
    /// The number of connections successfully opened from any pool.
    /// This value is incremented by the Remove method.
    /// </summary>
    private static int _poolOpened = 0;
    /// <summary>
    /// The number of connections successfully closed from any pool.
    /// This value is incremented by the Add method.
    /// </summary>
    private static int _poolClosed = 0;

    /// <summary>
    /// Counts the number of pool entries matching the specified file name.
    /// </summary>
    /// <param name="fileName">
    /// The file name to match or null to match all files.
    /// </param>
    /// <param name="counts">
    /// The pool entry counts for each matching file.
    /// </param>
    /// <param name="openCount">
    /// The total number of connections successfully opened from any pool.
    /// </param>
    /// <param name="closeCount">
    /// The total number of connections successfully closed from any pool.
    /// </param>
    /// <param name="totalCount">
    /// The total number of pool entries for all matching files.
    /// </param>
    internal static void GetCounts(
      string fileName,
      ref Dictionary<string, int> counts,
      ref int openCount,
      ref int closeCount,
      ref int totalCount)
    {
      ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
      if (connectionPool != null)
      {
        connectionPool.GetCounts(fileName, ref counts, ref openCount, ref closeCount, ref totalCount);
      }
      else
      {
        lock (SQLiteConnectionPool._syncRoot)
        {
          openCount = SQLiteConnectionPool._poolOpened;
          closeCount = SQLiteConnectionPool._poolClosed;
          if (counts == null)
            counts = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
          if (fileName != null)
          {
            SQLiteConnectionPool.PoolQueue poolQueue;
            if (!SQLiteConnectionPool._queueList.TryGetValue(fileName, out poolQueue))
              return;
            Queue<WeakReference> queue = poolQueue.Queue;
            int num = queue != null ? queue.Count : 0;
            counts.Add(fileName, num);
            totalCount += num;
          }
          else
          {
            foreach (KeyValuePair<string, SQLiteConnectionPool.PoolQueue> queue1 in SQLiteConnectionPool._queueList)
            {
              if (queue1.Value != null)
              {
                Queue<WeakReference> queue2 = queue1.Value.Queue;
                int num = queue2 != null ? queue2.Count : 0;
                counts.Add(queue1.Key, num);
                totalCount += num;
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Disposes of all pooled connections associated with the specified
    /// database file name.
    /// </summary>
    /// <param name="fileName">The database file name.</param>
    internal static void ClearPool(string fileName)
    {
      ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
      if (connectionPool != null)
      {
        connectionPool.ClearPool(fileName);
      }
      else
      {
        lock (SQLiteConnectionPool._syncRoot)
        {
          SQLiteConnectionPool.PoolQueue poolQueue;
          if (!SQLiteConnectionPool._queueList.TryGetValue(fileName, out poolQueue))
            return;
          ++poolQueue.PoolVersion;
          Queue<WeakReference> queue = poolQueue.Queue;
          if (queue == null)
            return;
          while (queue.Count > 0)
          {
            WeakReference weakReference = queue.Dequeue();
            if (weakReference != null)
            {
              if (weakReference.Target is SQLiteConnectionHandle target7)
                target7.Dispose();
              GC.KeepAlive((object) target7);
            }
          }
        }
      }
    }

    /// <summary>Disposes of all pooled connections.</summary>
    internal static void ClearAllPools()
    {
      ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
      if (connectionPool != null)
      {
        connectionPool.ClearAllPools();
      }
      else
      {
        lock (SQLiteConnectionPool._syncRoot)
        {
          foreach (KeyValuePair<string, SQLiteConnectionPool.PoolQueue> queue1 in SQLiteConnectionPool._queueList)
          {
            if (queue1.Value != null)
            {
              Queue<WeakReference> queue2 = queue1.Value.Queue;
              while (queue2.Count > 0)
              {
                WeakReference weakReference = queue2.Dequeue();
                if (weakReference != null)
                {
                  if (weakReference.Target is SQLiteConnectionHandle target10)
                    target10.Dispose();
                  GC.KeepAlive((object) target10);
                }
              }
              if (SQLiteConnectionPool._poolVersion <= queue1.Value.PoolVersion)
                SQLiteConnectionPool._poolVersion = queue1.Value.PoolVersion + 1;
            }
          }
          SQLiteConnectionPool._queueList.Clear();
        }
      }
    }

    /// <summary>
    /// Adds a connection to the pool of those associated with the
    /// specified database file name.
    /// </summary>
    /// <param name="fileName">The database file name.</param>
    /// <param name="handle">The database connection handle.</param>
    /// <param name="version">
    /// The connection pool version at the point the database connection
    /// handle was received from the connection pool.  This is also the
    /// connection pool version that the database connection handle was
    /// created under.
    /// </param>
    internal static void Add(string fileName, SQLiteConnectionHandle handle, int version)
    {
      ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
      if (connectionPool != null)
      {
        connectionPool.Add(fileName, (object) handle, version);
      }
      else
      {
        lock (SQLiteConnectionPool._syncRoot)
        {
          SQLiteConnectionPool.PoolQueue queue1;
          if (SQLiteConnectionPool._queueList.TryGetValue(fileName, out queue1) && version == queue1.PoolVersion)
          {
            SQLiteConnectionPool.ResizePool(queue1, true);
            Queue<WeakReference> queue2 = queue1.Queue;
            if (queue2 == null)
              return;
            queue2.Enqueue(new WeakReference((object) handle, false));
            Interlocked.Increment(ref SQLiteConnectionPool._poolClosed);
          }
          else
            handle.Close();
          GC.KeepAlive((object) handle);
        }
      }
    }

    /// <summary>
    /// Removes a connection from the pool of those associated with the
    /// specified database file name with the intent of using it to
    /// interact with the database.
    /// </summary>
    /// <param name="fileName">The database file name.</param>
    /// <param name="maxPoolSize">
    /// The new maximum size of the connection pool for the specified
    /// database file name.
    /// </param>
    /// <param name="version">
    /// The connection pool version associated with the returned database
    /// connection handle, if any.
    /// </param>
    /// <returns>
    /// The database connection handle associated with the specified
    /// database file name or null if it cannot be obtained.
    /// </returns>
    internal static SQLiteConnectionHandle Remove(
      string fileName,
      int maxPoolSize,
      out int version)
    {
      ISQLiteConnectionPool connectionPool = SQLiteConnectionPool.GetConnectionPool();
      if (connectionPool != null)
        return connectionPool.Remove(fileName, maxPoolSize, out version) as SQLiteConnectionHandle;
      int poolVersion;
      Queue<WeakReference> weakReferenceQueue;
      lock (SQLiteConnectionPool._syncRoot)
      {
        version = SQLiteConnectionPool._poolVersion;
        SQLiteConnectionPool.PoolQueue queue;
        if (!SQLiteConnectionPool._queueList.TryGetValue(fileName, out queue))
        {
          queue = new SQLiteConnectionPool.PoolQueue(SQLiteConnectionPool._poolVersion, maxPoolSize);
          SQLiteConnectionPool._queueList.Add(fileName, queue);
          return (SQLiteConnectionHandle) null;
        }
        version = poolVersion = queue.PoolVersion;
        queue.MaxPoolSize = maxPoolSize;
        SQLiteConnectionPool.ResizePool(queue, false);
        weakReferenceQueue = queue.Queue;
        if (weakReferenceQueue == null)
          return (SQLiteConnectionHandle) null;
        SQLiteConnectionPool._queueList.Remove(fileName);
        weakReferenceQueue = new Queue<WeakReference>((IEnumerable<WeakReference>) weakReferenceQueue);
      }
      try
      {
        while (weakReferenceQueue.Count > 0)
        {
          WeakReference weakReference = weakReferenceQueue.Dequeue();
          if (weakReference != null && weakReference.Target is SQLiteConnectionHandle target4)
          {
            GC.SuppressFinalize((object) target4);
            try
            {
              GC.WaitForPendingFinalizers();
              if (!target4.IsInvalid)
              {
                if (!target4.IsClosed)
                {
                  Interlocked.Increment(ref SQLiteConnectionPool._poolOpened);
                  return target4;
                }
              }
            }
            finally
            {
              GC.ReRegisterForFinalize((object) target4);
            }
            GC.KeepAlive((object) target4);
          }
        }
      }
      finally
      {
        lock (SQLiteConnectionPool._syncRoot)
        {
          SQLiteConnectionPool.PoolQueue queue1;
          bool flag;
          if (SQLiteConnectionPool._queueList.TryGetValue(fileName, out queue1))
          {
            flag = false;
          }
          else
          {
            flag = true;
            queue1 = new SQLiteConnectionPool.PoolQueue(poolVersion, maxPoolSize);
          }
          Queue<WeakReference> queue2 = queue1.Queue;
          while (weakReferenceQueue.Count > 0)
            queue2.Enqueue(weakReferenceQueue.Dequeue());
          SQLiteConnectionPool.ResizePool(queue1, false);
          if (flag)
            SQLiteConnectionPool._queueList.Add(fileName, queue1);
        }
      }
      return (SQLiteConnectionHandle) null;
    }

    /// <summary>
    /// This method is used to obtain a reference to the custom connection
    /// pool implementation currently in use, if any.
    /// </summary>
    /// <returns>
    /// The custom connection pool implementation or null if the default
    /// connection pool implementation should be used.
    /// </returns>
    internal static ISQLiteConnectionPool GetConnectionPool()
    {
      lock (SQLiteConnectionPool._syncRoot)
        return SQLiteConnectionPool._connectionPool;
    }

    /// <summary>
    /// This method is used to set the reference to the custom connection
    /// pool implementation to use, if any.
    /// </summary>
    /// <param name="connectionPool">
    /// The custom connection pool implementation to use or null if the
    /// default connection pool implementation should be used.
    /// </param>
    internal static void SetConnectionPool(ISQLiteConnectionPool connectionPool)
    {
      lock (SQLiteConnectionPool._syncRoot)
        SQLiteConnectionPool._connectionPool = connectionPool;
    }

    /// <summary>
    /// We do not have to thread-lock anything in this function, because it
    /// is only called by other functions above which already take the lock.
    /// </summary>
    /// <param name="queue">The pool queue to resize.</param>
    /// <param name="add">
    /// If a function intends to add to the pool, this is true, which
    /// forces the resize to take one more than it needs from the pool.
    /// </param>
    private static void ResizePool(SQLiteConnectionPool.PoolQueue queue, bool add)
    {
      int maxPoolSize = queue.MaxPoolSize;
      if (add && maxPoolSize > 0)
        --maxPoolSize;
      Queue<WeakReference> queue1 = queue.Queue;
      if (queue1 == null)
        return;
      while (queue1.Count > maxPoolSize)
      {
        WeakReference weakReference = queue1.Dequeue();
        if (weakReference != null)
        {
          if (weakReference.Target is SQLiteConnectionHandle target4)
            target4.Dispose();
          GC.KeepAlive((object) target4);
        }
      }
    }

    /// <summary>
    /// Keeps track of connections made on a specified file.  The PoolVersion
    /// dictates whether old objects get returned to the pool or discarded
    /// when no longer in use.
    /// </summary>
    private sealed class PoolQueue
    {
      /// <summary>
      /// The queue of weak references to the actual database connection
      /// handles.
      /// </summary>
      internal readonly Queue<WeakReference> Queue = new Queue<WeakReference>();
      /// <summary>
      /// This pool version associated with the database connection
      /// handles in this pool queue.
      /// </summary>
      internal int PoolVersion;
      /// <summary>The maximum size of this pool queue.</summary>
      internal int MaxPoolSize;

      /// <summary>
      /// Constructs a connection pool queue using the specified version
      /// and maximum size.  Normally, all the database connection
      /// handles in this pool are associated with a single database file
      /// name.
      /// </summary>
      /// <param name="version">
      /// The initial pool version for this connection pool queue.
      /// </param>
      /// <param name="maxSize">
      /// The initial maximum size for this connection pool queue.
      /// </param>
      internal PoolQueue(int version, int maxSize)
      {
        this.PoolVersion = version;
        this.MaxPoolSize = maxSize;
      }
    }
  }
}

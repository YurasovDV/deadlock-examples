# deadlock-examples
Deadlock examples
1. MonitorAwait. <br/>
Reentrant lock is not possible after await statement since the continuation may run in another thread so thread effectively locks. 
This is the reason why await is not permitted inside lock block. 
Solution: never use await between Monitor.Enter and Monitor.Exit.
2. DoubleObjectLock<br/>
The classic example of deadlock: both threads try to lock two different resources but in different order thus locking each other at last step.
Solution: always take locks in the same order for all actors.
3. DeadlockForm <br/>
Gives an example of SynchronizationContext-related issues: explicit locking (call to Task.Result) makes UI thread irresponsive and prevents continuation from executing. 
Solution: never try to retrieve Result, use await.
4. DbConcurrentUpdateLocks <br/>
Two simultaneous transactions try to update the same rows but in reverse order (similar to p. 2), and one of them falls victim.
Solution: always take locks in the same order for all actors.
5. LockOnIndexKey <br/>
The first transaction puts exclusive lock on one row and corresponding nonclustered index key. In several milliseconds Transaction 2 uses UPDLOCK hint in order to place update lock (obviously) on another row. Next command in Transaction 1 that is trying to access another row may never succeed thus deadlock.
The problem here is that attempt to update different rows does not lock two transactions, but if the change is affecting indexed properties, there is possibility to get involved in fight over index keys. It's especially nasty when it happens in different order.
Solution: if there is identical SQL for both transactions, for example, 1) SELECT(UPDLOCK) 2) UPDATE, then you can use SERIALIZABLE isolation level since it won't allow to place U locks on index pages affected.
If you are dealing with the situation like the one in repo, I would go for sp_getapplock with custom sync object in order to forbid placing locks on nonclustered and clustered indexes altogether.

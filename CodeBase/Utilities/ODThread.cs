using System;
using System.Collections.Generic;
using System.Threading;

namespace CodeBase {
	///<summary>A wrapper for the c# Thread class.  The purpose of this class is to help implement a well defined pattern throughout our applications.  It also allows us to better document threading where C# lacks documentation.</summary>
	public class ODThread {
		///<summary>The C# thread that is used to run ODThread internally.</summary>
		private Thread _thread=null;
		///<summary>Gets set to true when QuitSync() or QuitAsync() has been called or if this thread has finished and no timed interval was set.</summary>
		private bool _hasQuit=false;
		///<summary>The amount of time in milliseconds that this thread will sleep before calling the WorkerDelegate again.  Setting the interval to zero or a negative number will call the WorkerDelegate once and then quit itself.</summary>
		public int TimeInterval=0;
		///<summary>Pointer to the function from the calling code which will perform the majority of this thread's work.</summary>
		private WorkerDelegate _worker=null;
		///<summary>Custom data which can be set before launching the thread and then safely accessed within the WorkerDelegate.  Helps prevent the need to lock objects due to multi-threading, most of the time.</summary>
		public object Tag=null;
		///<summary>Custom data which can be used within the WorkerDelegate.  Helps prevent the need to lock objects due to multi-threading, most of the time.</summary>
		public object[] Parameters=null;
		///<summary>Used to identify groups of ODThread objects.  Helpful when you need to wait for or quit an entire group of threads.  Blank means default group.</summary>
		public string GroupName="";
		///<summary>Global list of all ODThreads which have not been quit.  Used for thread group operations.</summary>
		private static List<ODThread> _listOdThreads=new List<ODThread>();

		///<summary>Creates a thread that will only run once after Start() is called.</summary>
		public ODThread(WorkerDelegate worker) : this(worker,null) {
		}

		///<summary>Creates a thread that will only run once after Start() is called.</summary>
		public ODThread(WorkerDelegate worker,params object[] parameters) : this(0,worker,parameters) {
		}

		///<summary>Creates a thread that will continue to run the WorkerDelegate after Start() is called and will stop running once one of the quit methods has been called or the application itself is closing.  timeInterval (in milliseconds) determines how long the thread will wait before executing the WorkerDelegate again.  Set timeInterval to zero or a negative number to have the WorkerDelegate only execute once and then quit itself.</summary>
		public ODThread(int timeInterval,WorkerDelegate worker,params object[] parameters) {
			_listOdThreads.Add(this);
			_thread=new Thread(new ThreadStart(this.Run));
			TimeInterval=timeInterval;
			_worker+=worker;
			Parameters=parameters;
		}

		///<summary>Starts the thread and returns immediately.  If the thread is already started or has already finished, then this function will have no effect.</summary>
		public void Start() {
			if(_thread.IsAlive) {
				return;//The thread is already running.
			}
			if(_hasQuit) {
				return;//The thread has finished.
			}
			_thread.Start();
		}

		///<summary>Main thread loop that executes the WorkerDelegate and sleeps for the designated timeInterval (in milliseconds) if one was set.</summary>
		private void Run() {
			while(!_hasQuit) {
				_worker(this);
				if(TimeInterval>0) {
					Thread.Sleep(TimeInterval);
				}
				else if(TimeInterval<=0) {//Interval was set to a negative number, so do work once and then quits the thread.
					_hasQuit=true;
				}
			}
		}

		///<summary>Forces the calling thread to synchronously wait for the current thread to finish doing work.</summary>
		public void Join() {
			_thread.Join();
		}

		///<summary>Synchronously waits for all threads in the specified group to finish doing work.</summary>
		public static void JoinThreadsByGroupName(string groupName) {
			for(int i=0;i<_listOdThreads.Count;i++) {
				if(_listOdThreads[i].GroupName==groupName) {
					_listOdThreads[i].Join();
				}
			}
		}

		///<summary>Immediately returns after flagging the thread to quit itself asynchronously.  The thread may execute a bit longer.  If the thread has been forgotten, it will be forcefully quit on closing of the main application.</summary>
		public void QuitAsync() {
			_hasQuit=true;
			_listOdThreads.Remove(this);
		}

		///<summary>Waits for this thread to quit itself before returning.  If the thread has been forgotten, it will be forcefully quit on closing of the main application.</summary>
		public void QuitSync() {
			_hasQuit=true;
			_thread.Abort();//Causes a ThreadAbortException() to be created at the current line of code execution within Run() or _worker().
			_thread.Join();//Causes the main thread to wait for this thread to finish.
			_listOdThreads.Remove(this);
		}

		///<summary>Waits for ALL threads in the group to finish doing work before returning.  If the thread has been forgotten, it will be forcefully quit on closing of the main application.</summary>
		public static void QuitSyncThreadsByGroupName(string groupName) {
			for(int i=_listOdThreads.Count-1;i>=0;i--) {//Loop backwards since the threads are being removed as we loop through.
				if(_listOdThreads[i].GroupName==groupName) {
					_listOdThreads[i].QuitSync();
				}
			}
		}

		///<summary>Should only be called when the main application is closing.  Loops through ALL ODThreads that are still running and synchronously quits them.  The main application thread will wait for all threads to finish doing work.</summary>
		public static void QuitSyncAllOdThreads() {
			for(int i=_listOdThreads.Count-1;i>=0;i--) {//Loop backwards since the threads are being removed as we loop through.
				_listOdThreads[i].QuitSync();
			}
		}

		///<summary>Returns the specified group of threads in the same order they were created.  The primary reason to use this function is to have access to the individual ODThread.Tag objects after a group is done doing work.</summary>
		public static List<ODThread> GetThreadsByGroupName(string groupName) {
			List<ODThread> listThreadsForGroup=new List<ODThread>();
			for(int i=0;i<_listOdThreads.Count;i++) {
				if(_listOdThreads[i].GroupName==groupName) {
					listThreadsForGroup.Add(_listOdThreads[i]);
				}
			}
			return listThreadsForGroup;
		}

		///<summary>Pointer delegate to the method that does the work for this thread.  The worker method has to take an ODThread as a parameter so that it has access to Tag and other variables when needed.</summary>
		public delegate void WorkerDelegate(ODThread odThread);

	}
}

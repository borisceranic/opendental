using System;
using System.Collections.Generic;
using System.Threading;

namespace CodeBase {
	///<summary>A wrapper for the c# Thread class.  The purpose of this class is to help implement a well defined pattern throughout our applications.  It also allows us to better document threading where C# lacks documentation.  Since there is no way to get the list of managed threads for an application, the only way we can maintain a list is to do it ourselves.  The advantage of maintaining a list of managed threads is that we can much more easily ensure that all threads are gracefully quit when the program exits.</summary>
	public class ODThread {
		///<summary>The C# thread that is used to run ODThread internally.</summary>
		private Thread _thread=null;
		///<summary>Sleep timer which can be interrupted elegantly.</summary>
		private AutoResetEvent _waitEvent=new AutoResetEvent(false);
		///<summary>The exact time when this thread was started.  Useful for determining thread run times.</summary>
		private DateTime _dateTimeStart=DateTime.MinValue;
		///<summary>The exact time when this thread was quit.  Useful for determining thread run times.</summary>
		private DateTime _dateTimeQuit=DateTime.MinValue;
		///<summary>Gets set to true when QuitSync() or QuitAsync() has been called or if this thread has finished and no timed interval was set.</summary>
		private bool _hasQuit=false;
		///<summary>Indicates if ODThread has been scheduled to quit. Check this from within a resource intensive thread periodically if you want to exit gracefully during the course of the WorkerDelegate function.</summary>
		public bool HasQuit {
			get {
				return _hasQuit;
			}
		}
		///<summary>The amount of time in milliseconds that this thread will sleep before calling the WorkerDelegate again.  Setting the interval to zero or a negative number will call the WorkerDelegate once and then quit itself.</summary>
		public int TimeIntervalMS=0;
		///<summary>Pointer to the function from the calling code which will perform the majority of this thread's work.</summary>
		private WorkerDelegate _worker=null;
		///<summary>Pointer to the function from the calling code which will be alerted when the run function has thrown an unhandled exception.</summary>
		private ExceptionDelegate _exceptionHandler=null;
		///<summary>Pointer to the function from the calling code which will be alerted when the run function has completed.  This will NOT fire if Join() times out.</summary>
		private WorkerDelegate _threadExitHandler=null;		
		///<summary>Custom data which can be set before launching the thread and then safely accessed within the WorkerDelegate.  Helps prevent the need to lock objects due to multi-threading, most of the time.</summary>
		public object Tag=null;
		///<summary>Custom data which can be used within the WorkerDelegate.  Helps prevent the need to lock objects due to multi-threading, most of the time.</summary>
		public object[] Parameters=null;
		///<summary>Used to identify groups of ODThread objects.  Helpful when you need to wait for or quit an entire group of threads.  Initially set to "default".</summary>
		public string GroupName="default";
		///<summary>Global list of all ODThreads which have not been quit.  Used for thread group operations.</summary>
		private static List<ODThread> _listOdThreads=new List<ODThread>();
		///<summary>Thread safe lock object.  Any time a static variable is accessed, it MUST be wrapped with a lock.  Failing to lock will result in a potential for unsafe access by multiple threads at the same time.</summary>
		private static object _lockObj=new object();
		///<summary>Only set when calling Start().  Causes this thread to automatically remove itself from the global list of ODThreads once it has finished doing work.</summary>
		private bool _isAutoCleanup;

		///<summary>Gets or sets the name of the C# thread to make it easier to find specific threads while debugging.</summary>
		public string Name { 
			get {
				return _thread.Name;
			}
			set {
				_thread.Name=value;
			}
		}

		///<summary>Creates a thread that will only run once after Start() is called.</summary>
		public ODThread(WorkerDelegate worker) : this(worker,null) {
		}

		///<summary>Creates a thread that will only run once after Start() is called.</summary>
		public ODThread(WorkerDelegate worker,params object[] parameters) : this(0,worker,parameters) {
		}

		///<summary>Creates a thread that will continue to run the WorkerDelegate after Start() is called and will stop running once one of the quit methods has been called or the application itself is closing.  timeIntervalMS (in milliseconds) determines how long the thread will wait before executing the WorkerDelegate again.  Set timeIntervalMS to zero or a negative number to have the WorkerDelegate only execute once and then quit itself.</summary>
		public ODThread(int timeIntervalMS,WorkerDelegate worker,params object[] parameters) {
			lock(_lockObj) {
				_listOdThreads.Add(this);
			}
			_thread=new Thread(new ThreadStart(this.Run));
			TimeIntervalMS=timeIntervalMS;
			_worker+=worker;
			Parameters=parameters;
		}

		public override string ToString() {
			return Name;
		}

		///<summary>Start all threads for a given group. If thread has already been started then take no action on that thread.</summary>
		public static void StartThreadsByGroupName(string groupName) {
			List<ODThread> listOdThreadsForGroup=GetThreadsByGroupName(groupName);
			for(int i=0;i<listOdThreadsForGroup.Count;i++) {
				listOdThreadsForGroup[i].Start();
			}			
		}

		///<summary>Starts the thread and returns immediately.  If the thread is already started or has already finished, then this function will have no effect.</summary>
		public void Start() {
			Start(false);
		}

		///<summary>Starts the thread and returns immediately.  If the thread is already started or has already finished, then this function will have no effect.  Set isAutoCleanup to true to have this thread automatically remove itself from the global list of ODThreads once it has finished doing work.</summary>
		public void Start(bool isAutoCleanup) {
			_isAutoCleanup=isAutoCleanup;
			if(_thread.IsAlive) {
				return;//The thread is already running.
			}
			if(_hasQuit) {
				return;//The thread has finished.
			}
			_dateTimeStart=DateTime.Now;
			_thread.Start();
		}

		///<summary>If the thread is currently waiting, this will interrupt the wait and force the thread to continue running instantly.</summary>
		public void Wakeup() {
			_waitEvent.Set();
		}

		///<summary>Main thread loop that executes the WorkerDelegate and sleeps for the designated timeIntervalMS (in milliseconds) if one was set.</summary>
		private void Run() {
			while(!_hasQuit) {
				try {
					_worker(this);
				}
				catch(ThreadAbortException) {
					//We know that a join failed by exceeding the allotted timeout.
					_dateTimeQuit=DateTime.Now;
					_hasQuit=true;
					return;
				}
				catch(Exception e) { //An exception was unhandled by the worker delegate. Alert the caller if they have subscribed to this event.
					if(_exceptionHandler!=null) {
						_exceptionHandler(e);
					}
					else { //Caller has not subscribed to this event so stop program execution and alert end user that something unforeseen has failed.
						throw e;
					}
				}
				if(TimeIntervalMS>0) {
					_waitEvent.WaitOne(TimeIntervalMS);//WaitOne is used instead of Sleep so that threads can be 'woken up' in the middle of waiting in order to process pertinent information.
				}
				else if(TimeIntervalMS<=0) {//Interval was set to zero or a negative number, so do work once and then quits the thread.
					_hasQuit=true;
				}
			}
			_dateTimeQuit=DateTime.Now;
			if(_threadExitHandler!=null) {
				_threadExitHandler(this);
			}
			if(_isAutoCleanup) {
				lock(_lockObj) {
					_listOdThreads.Remove(this);
				}
			}
		}

		///<summary>Forces the calling thread to synchronously wait for the current thread to finish doing work.  Pass Timeout.Infinite into timeoutMS if you wish to wait as long as necessary for the thread to join.  The thread will be aborted if the timeout was reached and then will return false.</summary>
		public bool Join(int timeoutMS) {
			bool hasJoined=_thread.Join(timeoutMS);
			if(!hasJoined) {
				//The timeout expired and the thread is still alive so we want to abort it manually.
				//Abort exceptions will be swallowed within Run()
				_thread.Abort();
			}
			return hasJoined;
		}

		///<summary>Synchronously waits for all threads in the specified group to finish doing work.  Pass Timeout.Infinite into timeoutMS if you wish to wait as long as necessary for all threads to join.  Set doRemoveThreads to true to remove all threads from the global list of threads.</summary>
		public static void JoinThreadsByGroupName(int timeoutMS,string groupName,bool doRemoveThreads=false) {
			List<ODThread> listOdThreadsForGroup=GetThreadsByGroupName(groupName);
			for(int i=0;i<listOdThreadsForGroup.Count;i++) {
				listOdThreadsForGroup[i].Join(timeoutMS);
			}
			if(doRemoveThreads) {
				//Remove all threads from the global list of ODThreads.
				foreach(ODThread thread in listOdThreadsForGroup) {
					thread.QuitAsync();
				}
			}
		}

		///<summary>Immediately returns after flagging the thread to quit itself asynchronously.  The thread may execute a bit longer.  If the thread has been forgotten, it will be forcefully quit on closing of the main application.</summary>
		public void QuitAsync() {
			QuitAsync(true);
		}

		///<summary>Immediately returns after flagging the thread to quit itself asynchronously.  The thread may execute a bit longer.  If the thread has been forgotten, it will be forcefully quit on closing of the main application.  Set removeThread false if you want this thread to stay in the global list of ODThreads.</summary>
		public void QuitAsync(bool removeThread) {
			_hasQuit=true;
			//If thread is in idle due to wait event, then wake it immediately so we can more quickly quit.  Helps the thread quit within timeoutMS.
			Wakeup();
			if(removeThread) {
				lock(_lockObj) {
					_listOdThreads.Remove(this);
				}
			}
		}

		///<summary>Waits for this thread to quit itself before returning.  If the thread has been forgotten, it will be forcefully quit on closing of the main application.</summary>
		public void QuitSync(int timeoutMS) {
			_hasQuit=true;
			//If thread is in waiting on wait event, wake it can quit gracefully.
			Wakeup();
			try {
				Join(timeoutMS);//Wait for allotted time before throwing ThreadAbortException.
			}
			catch {
				//Guards against re-entrance into this function just in case the main thread called QuitSyncAllOdThreads() and this thread itself called QuitSync() at the same time.
				//This will be very rare and if we get to this point, we know that the thread has already been joined or aborted and thus has already finished doing work so it is fine to remove.
			}
			finally {
				lock(_lockObj) {
					_listOdThreads.Remove(this);
				}
			}
		}

		///<summary>Waits for ALL threads in the group to finish doing work before returning.  Each thread will be given the timeoutMS to quit.  Try to keep in mind how many threads are going to be quitting when setting the milliseconds for the timeout.  If the thread has been forgotten, it will be forcefully quit on closing of the main application.  Removes all threads from the global list of ODThreads after the threads have quit.</summary>
		public static void QuitSyncThreadsByGroupName(int timeoutMS,string groupName) {
			List<ODThread> listThreadsForGroup=GetThreadsByGroupName(groupName);
			for(int i=0;i<listThreadsForGroup.Count;i++) { //Quit all threads in parallel so our wait times are not cummulative.
				listThreadsForGroup[i].QuitAsync(false);//Do not remove threads from global list so that the Join can have access to them.
			}
			//Wait for all threads to end or timeout, whichever comes first.
			JoinThreadsByGroupName(timeoutMS,groupName,true);
		}

		///<summary>Should only be called when the main application is closing.  Loops through ALL ODThreads that are still running and aborts them instantly.  If you want to give each thread a chance to gracefully quit, call QuitSyncThreadsByGroupName instead.</summary>
		public static void QuitSyncAllOdThreads() {
			QuitSyncThreadsByGroupName(0,"");
		}

		///<summary>Returns the specified group of threads in the same order they were created.  If groupName is empty, then returns the list of all current ODThreads.</summary>
		public static List<ODThread> GetThreadsByGroupName(string groupName) {
			List<ODThread> listOdThreadsForGroup=new List<ODThread>();
			lock(_lockObj) {
				for(int i=0;i<_listOdThreads.Count;i++) {
					if(groupName=="" || _listOdThreads[i].GroupName==groupName) {
						listOdThreadsForGroup.Add(_listOdThreads[i]);
					}
				}
			}
			return listOdThreadsForGroup;
		}

		///<summary>Add an exception handler to be alerted of unhandled exceptions in the work delegate.</summary>
		public void AddExceptionHandler(ExceptionDelegate exceptionHandler) {
			_exceptionHandler+=exceptionHandler;
		}

		///<summary>Add an exception handler to be alerted of unhandled exceptions in the work delegate.</summary>
		public void AddThreadExitHandler(WorkerDelegate threadExitHandler) {
			_threadExitHandler+=threadExitHandler;
		}

		///<summary>If the thread has not started, then returns 0.  If the thread has started but has not quit yet, then returns the amount of time which has elapsed since the thread was started.  If the thread has quit, returns the time elapsed between when the thread was started and when the thread was quit.</summary>
		public TimeSpan GetTimeElapsed() {
			if(_hasQuit) {
				return (_dateTimeQuit-_dateTimeStart);
			}
			else if(_dateTimeStart>DateTime.MinValue) {
				return (DateTime.Now-_dateTimeStart);
			}
			return TimeSpan.Zero;
		}

		///<summary>Pointer delegate to the method that does the work for this thread.  The worker method has to take an ODThread as a parameter so that it has access to Tag and other variables when needed.</summary>
		public delegate void WorkerDelegate(ODThread odThread);

		///<summary>Pointer delegate to the method that gets called when the worker delegate throws an unhandled exception.</summary>
		public delegate void ExceptionDelegate(Exception e);
	}
}

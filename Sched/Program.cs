using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sched
{
    public class Program
    {
        private static System.IO.FileSystemWatcher watcher;
        private static CancellationTokenSource cts;
        private static IScheduler scheduler;
        private static void WatcherInitial()
        {
            watcher = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(watcher)).BeginInit();
            watcher.EnableRaisingEvents = true;
            watcher.Filter = "*.xlsx";
            ((System.ComponentModel.ISupportInitialize)(watcher)).EndInit();

            watcher.Path = ConfigurationManager.AppSettings["Folder"];
            watcher.Created += new FileSystemEventHandler(OnCreated);
            watcher.Deleted += new FileSystemEventHandler(OnDeleted);

        }
        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            Thread.Sleep(4000);
            RunProgramRunExample(true).GetAwaiter();//.GetResult();
            Thread.Sleep(4000);
        }
        private static void OnDeleted(object source, FileSystemEventArgs e)
        {
            Thread.Sleep(4000);
            RunProgramRunExample(true).GetAwaiter();//.GetResult();
            Thread.Sleep(4000);
        }
        private static void Main(string[] args)
        {
            WatcherInitial();
            //RunProgramRunExample().GetAwaiter().GetResult();
            Console.ReadKey();
        }

        private static async Task RunProgramRunExample(bool Stop = false)
        {
            try
            {
                //if (cts != null && Stop)
                if (Stop)
                {
                    //cts.Cancel();
                    //cts.Dispose();
                    //await scheduler.Shutdown();    
                    if (scheduler != null)
                        await scheduler.Clear();

                    //if (cts.Token.IsCancellationRequested)
                    await Console.Out.WriteLineAsync("scheduler is cleared");
                    //await scheduler.Shutdown();

                }
                //cts = new CancellationTokenSource();

                // Grab the Scheduler instance from the Factory
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                //IScheduler scheduler = await factory.GetScheduler(cts.Token);
                scheduler = await factory.GetScheduler();

                // and start it off
                await scheduler.Start();
                await Console.Out.WriteLineAsync("scheduler is started");

                var Files = Directory.GetFiles(ConfigurationManager.AppSettings["Folder"], "*.xlsx");
                Dictionary<string, string> dic = null;
                DateTime dt = new DateTime(1945, 5, 9);
                
                string[] Process = ConfigurationManager.AppSettings["Process"].Split(';');
                for (int i = 0; i < Files.Length; i++)
                {
                    string FileName = Path.GetFileNameWithoutExtension(Files[i]);
                    string FilePath = Files[i];
                    //if (!Process.Contains(FileName.ToUpper()))
                    bool bFind = false;
                    string ProcessItem = null;
                    foreach (string item in Process)
                    {
                        if (FileName.Contains(item))
                        {
                            bFind = true;
                            ProcessItem = item;
                            break;
                        }

                    }
                    if (!bFind)
                        continue;


                    try
                    {
                        dt = new DateTime(
                                               int.Parse(FileName.Substring(0, 4)),
                                               int.Parse(FileName.Substring(4, 2)),
                                               int.Parse(FileName.Substring(6, 2)),

                                               int.Parse(FileName.Substring(9, 2)),
                                               int.Parse(FileName.Substring(11, 2)),
                                               0
                                               );

                        dic = new Dictionary<string, string>
                            {
                                { "FileName", FileName.Split(' ')[2]},
                                { "FilePath", FilePath}
                            };
                        IJobDetail job = JobBuilder.Create<HelloJob>()
                          .WithIdentity("job" + i, "group1")
                          //.WithDescription(FileName.Split(' ')[2])
                          //.WithDescription(FileName)
                          .SetJobData(new JobDataMap(dic))
                          .Build();

                        ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("trigger" + i, "group1")
                        .StartAt(dt)
                        .Build();
                        await Console.Out.WriteLineAsync(FileName + ": " + dt.ToString() + ": OK");
                        await scheduler.ScheduleJob(job, trigger);

                    }
                    catch (Exception)
                    {
                        try
                        {

                            dic = new Dictionary<string, string>
                                {
                                    { "FileName", ProcessItem},
                                    { "FilePath", FilePath}
                                };
                            IJobDetail job = JobBuilder.Create<HelloJob>()
                              .WithIdentity("job" + i, "group1")
                              //.WithDescription(FileName.Split(' ')[2])
                              //.WithDescription(FileName)
                              .SetJobData(new JobDataMap(dic))
                              .Build();

                            //dt = DateTime.Now.Date.AddDays(1).AddMinutes(5);
                            dt = DateTime.Now.AddSeconds(10);
                            ITrigger trigger = TriggerBuilder.Create()
                            .WithIdentity("trigger" + i, "group1")
                            .StartAt(dt)
                            .Build();
                            await Console.Out.WriteLineAsync(FileName + ": " + dt.ToString() + ": OK");
                            await scheduler.ScheduleJob(job, trigger);
                        }
                        catch (Exception)
                        {
                            //dt = new DateTime(1945, 5, 9);
                            //dic = new Dictionary<string, string>
                            //    {
                            //        { "FileName", null},
                            //        { "FilePath", null}
                            //    };
                            //await Console.Out.WriteLineAsync(FileName + ": " + dt.ToString() + ": Fail filename");
                        }


                    }


                }

                await Task.Delay(TimeSpan.FromMilliseconds(-1));


                await scheduler.Shutdown();

            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }

    }

    public class HelloJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //await Console.Out.WriteLineAsync(context.JobDetail.Description);
            //await File.Move (context.JobDetail.JobDataMap["FilePath"].ToString(), @"C:\Download\GRAND\test\" + context.JobDetail.JobDataMap["FileName"].ToString());
            //await Console.Out.WriteLineAsync(context.JobDetail.JobDataMap["FilePath"].ToString());
            var Map = context.JobDetail.JobDataMap;
            string Source = Map["FilePath"].ToString();
            string FileName = Path.GetFileName(Map["FilePath"].ToString());
            string Dest = @"C:\Download\GRAND\test\" + Map["FileName"].ToString() + "\\" + FileName;
            //await Console.Out.WriteLineAsync(Dest);
            if (!File.Exists(Dest))
            {
                await Task.Run(() => File.Move(Source, Dest));
                await Console.Out.WriteLineAsync(FileName + " is moved!");
            }
            else
                await Console.Out.WriteLineAsync(Dest + " exists!");
        }
    }
}

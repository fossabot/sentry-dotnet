using (SentrySdk.Init(options =>
       {
           options.Dsn = "https://eb18e953812b41c3aeb042e666fd3b5c@o447951.ingest.sentry.io/5428537";
           options.CacheDirectoryPath = "D:/sentryCacheDir";
           options.ShutdownTimeout = TimeSpan.FromDays(1);
           options.Debug = true;
           options.InitCacheFlushTimeout= TimeSpan.FromMinutes(10);
           ;
       }))
{
    //SentrySdk.CaptureMessage("repro message", scope =>
    //{
    //    scope.AddAttachment(new Attachment(AttachmentType.Default,
    //        new FileAttachmentContent("D:/sentryCacheDir/Attachment.txt"),
    //        "Attachment.txt",
    //        null));
    //});
}

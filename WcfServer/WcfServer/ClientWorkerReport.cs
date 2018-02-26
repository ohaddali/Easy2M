namespace WcfServer
{
    public class ClientWorkerReport
    {
        public long reportId { get; set; }
        public long relatedId { get; set; }
        public string url { get; set; }
        public string date { get; set; }
        public bool workerReport { get; set; }
    }
}
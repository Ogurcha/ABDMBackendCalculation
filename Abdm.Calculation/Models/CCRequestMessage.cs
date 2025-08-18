namespace Abdm.Calculation.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CCRequestMessage
    {
        public int Id { get; set; }

        /// <summary>
        /// идентификатор искусственного сооружения
        /// </summary>
        public long issoId { get; set; }





        public string Name { get; set; }

        //public ReportStatus Status { get; set; }

        public string Message { get; set; }

        public string FilePath { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace autolandjepcore.Models
{
    public partial class jepsonmodel
    {
        public DateTime? DATEOFORIGIN { get; set; }

        public String? FLTNBR { get; set; }

        public String? SUFFIX { get; set; }

        public String? CARRIER { get; set; }

        public String? STARTSTATION { get; set; }

        public String? ENDSTATION { get; set; }

        public DateTime? SCHEDULESTARTTIME { get; set; }

        public DateTime? SCHEDULEENDTIME { get; set; }

        public DateTime? ESTINBLOCK { get; set; }

        public DateTime? ESTOFFBLOCK { get; set; }

        public DateTime? ESTTAKEOFFTIME { get; set; }

        public DateTime? ESTLANDINGTIME { get; set; }

        public DateTime? ACTINBLOCK { get; set; }

        public DateTime? ACTOFFBLOCK { get; set; }

        public DateTime? ACTTAKEOFFTIME { get; set; }

        public DateTime? ACTLANDINGTIME { get; set; }

  
        public DateTime? DOORCLOSE { get; set; }
 
        public string? STARTTIMEOFFSET { get; set; }

        public string? ENDTIMEOFFSET { get; set; }

        [NotMapped]
        public int SEQNUMBER { get; set; }

        public String? OPERATIONALSTATUS { get; set; }

        public String? SCHEDULESTATUS { get; set; }

        public String? SERVICETYPE { get; set; }

        [NotMapped]
        public String? AIRCRAFTOWNER { get; set; }
        [NotMapped]
        public String? COCKPITEMPLOYER { get; set; }
        [NotMapped]
        public String? CABINEMPLOYER { get; set; }

        public String? AIRCRAFTTYPE { get; set; }

        public String? AIRCRAFT { get; set; }


        public String? AIRCRAFTCONFIGURATION { get; set; }

        [NotMapped]
        public String? PASSENGERRESERVATIONS { get; set; }

        [NotMapped]
        public String? EFFECTIVEARRIVALSTATION { get; set; }

        [NotMapped]
        public String? DIVERSIONCODE { get; set; }

        [NotMapped]
        public DateTime? DELAY_TIME { get; set; }

        [NotMapped]
        public String? DELAYREASON { get; set; }

        [NotMapped]
        public String? DELAYNUMBER { get; set; }

        [NotMapped]
        public String? ISROOTCAUSE { get; set; }

        [NotMapped]
        public String? REMARKS { get; set; }

        public String? AUTOLAND { get; set; }

        [NotMapped]
        public String? RETURNNUMBER { get; set; }

        [NotMapped]
        public String? TRAININGTAG { get; set; }

        [NotMapped]
        public String? ONWARDFLT_CARRIER { get; set; }

        [NotMapped]
        public String? ONWARDFLT_FLTNBR { get; set; }

        [NotMapped]
        public String? ONWARDFLT_SUFFIX { get; set; }

        [NotMapped]
        public String? ONWARDFLT_AIRCRAFTCONFIGURATION { get; set; }

        [NotMapped]
        public String? ONWARDFLT_DATEOFORIGIN { get; set; }
    }
}

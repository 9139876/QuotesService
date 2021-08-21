﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesService.DAL.Entities
{
    [Table("TickersInfoes")]
    public class TickerInfoEntity
    {
        [Key]
        public int Id { get; set; }

        public int TickerId { get; set; }

        public string KeyName { get; set; }

        public string Value { get; set; }
    }
}

﻿using System;

namespace twentySix.NeuralStock.Core.Models
{
    public class Quote
    {
        public DateTime Date { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Open { get; set; }

        public double Close { get; set; }

        public double Volume { get; set; }

        public double Dividend { get; set; }
    }
}
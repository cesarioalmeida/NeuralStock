﻿namespace twentySix.NeuralStock.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using DevExpress.Mvvm;

    using twentySix.NeuralStock.Core.DTOs;

    public class HistoricalData : BindableBase, ICloneable
    {
        public HistoricalData()
        {
            Quotes = new SortedList<DateTime, Quote>();
        }

        public HistoricalData(SortedList<DateTime, Quote> quotes)
        {
            Quotes = quotes;
        }

        [Key]
        public int Id { get; set; }

        public SortedList<DateTime, Quote> Quotes
        {
            get => GetProperty(() => Quotes);
            private set => SetProperty(() => Quotes, value);
        }

        public DateTime BeginDate => Quotes.Keys.FirstOrDefault();

        public DateTime EndDate => Quotes.Keys.LastOrDefault();

        public Quote this[DateTime date] => Quotes[date];

        public static HistoricalData operator +(HistoricalData first, HistoricalData second)
        {
            if (first == null && second == null)
            {
                return null;
            }

            if (first == null)
            {
                return second;
            }

            if (second == null)
            {
                return first;
            }

            var obj = new HistoricalData { Id = first.Id, Quotes = first.Quotes };

            foreach (var item in second.Quotes)
            {
                if (!obj.Quotes.ContainsKey(item.Key))
                {
                    obj.Quotes.Add(item.Key, item.Value);
                }
            }

            return obj;
        }

        public static Tuple<HistoricalData, HistoricalData> operator /(HistoricalData data, double percentageFirst)
        {
            if (data == null)
            {
                return null;
            }

            if (percentageFirst == 0d)
            {
                return Tuple.Create((HistoricalData)null, (HistoricalData)data.Clone());
            }

            if (percentageFirst == 1d)
            {
                return Tuple.Create((HistoricalData)data.Clone(), (HistoricalData)null);
            }

            var firstPortionQuotes = data.Quotes.Take((int)(data.Quotes.Count * percentageFirst)).ToList();
            var secondPortionQuotes = data.Quotes.Except(firstPortionQuotes).ToList();

            var firstPortion = new HistoricalData
            {
                Quotes = new SortedList<DateTime, Quote>(firstPortionQuotes.ToDictionary(x => x.Key, x => x.Value))
            };

            var secondPortion = new HistoricalData
            {
                Quotes = new SortedList<DateTime, Quote>(secondPortionQuotes.ToDictionary(x => x.Key, x => x.Value))
            };

            return Tuple.Create(firstPortion, secondPortion);
        }

        public static HistoricalData FromDTO(HistoricalDataDTO dto)
        {
            if (dto == null)
            {
                return null;
            }

            var obj = new HistoricalData { Id = dto.Id };

            for (var i = 0; i < dto.DateArray.Length; i++)
            {
                var quote = new Quote
                {
                    Date = dto.DateArray[i],
                    Open = dto.OpenArray[i],
                    Close = dto.CloseArray[i],
                    AdjClose = dto.AdjCloseArray?[i] ?? dto.CloseArray[i],
                    High = dto.HighArray[i],
                    Low = dto.LowArray[i],
                    Volume = dto.VolumeArray[i],
                    Dividend = dto.DividendArray?[i] ?? 0d
                };

                obj.Quotes.Add(dto.DateArray[i], quote);
            }

            return obj;
        }

        public HistoricalDataDTO GetDTO()
        {
            return new HistoricalDataDTO
            {
                Id = Id,
                DateArray = Quotes.Keys.ToArray(),
                OpenArray = Quotes.Values.Select(x => x.Open).ToArray(),
                CloseArray = Quotes.Values.Select(x => x.Close).ToArray(),
                AdjCloseArray = Quotes.Values.Select(x => x.AdjClose).ToArray(),
                HighArray = Quotes.Values.Select(x => x.High).ToArray(),
                LowArray = Quotes.Values.Select(x => x.Low).ToArray(),
                VolumeArray = Quotes.Values.Select(x => x.Volume).ToArray(),
                DividendArray = Quotes.Values.Select(x => x.Dividend).ToArray()
            };
        }

        public void Clear()
        {
            Quotes.Clear();
        }

        public object Clone()
        {
            var result = new HistoricalData
            {
                Quotes = new SortedList<DateTime, Quote>(Quotes.ToDictionary(x => x.Key, x => x.Value))
            };

            return result;
        }

        public override int GetHashCode()
        {
            return Quotes != null ? Quotes.GetHashCode() : 0;
        }

        protected bool Equals(HistoricalData other)
        {
            return object.Equals(Quotes, other.Quotes);
        }
    }
}
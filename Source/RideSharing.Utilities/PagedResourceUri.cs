using System;
using System.Linq;

namespace RideSharing.Utilities
{
    public class PagedResourceUri
    {
        public static Page ParseRecordRangeFrom(Uri uri)
        {
            var recordRangeSection = uri.LocalPath.Split('/').LastOrDefault();
            if (String.IsNullOrWhiteSpace(recordRangeSection)) return Page.Empty;
            
            var recordRangeSectionSplit = recordRangeSection.Split(',');
            if (recordRangeSectionSplit.Length < 2) return Page.Empty;
            
            if(!Int32.TryParse(recordRangeSectionSplit[0], out int start))
            {
                return Page.Empty;
            }
            if(!Int32.TryParse(recordRangeSectionSplit[1], out int end))
            {
                return Page.Empty;
            }
            return new Page(start, end);
        }

        public class Page
        {
            public static readonly Page Empty = new Page(0, 0);
            
            public Page(int start, int end)
            {
                Start = start;
                End = end;
            }

            public int Start { get; }
            
            public int End { get; }
        }
    }
}
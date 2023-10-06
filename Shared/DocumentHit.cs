using System;
using Shared.BE;

namespace Shared
{
    /**
     * A class used to represent a document together with a
     * counter for how many words from a query that is in 
     * the document.
     * */
    public class DocumentHit
    {
        public BEDocument Document { get; set; }
        public int NoOfHits { get; set; }

        public DocumentHit() { }
        public DocumentHit(BEDocument doc, int noOfHits)
        {
            Document = doc;
            NoOfHits = noOfHits;
        }
    }
}
